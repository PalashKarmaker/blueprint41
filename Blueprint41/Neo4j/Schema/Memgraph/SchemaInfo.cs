﻿using System;
using System.Collections.Generic;
using System.Linq;

using Blueprint41.Core;
using Blueprint41.Neo4j.Persistence.Void;
using Blueprint41.Neo4j.Refactoring;
using Blueprint41.Neo4j.Schema.v5;

namespace Blueprint41.Neo4j.Schema.Memgraph;

public class SchemaInfo_Memgraph : v5.SchemaInfo_v5
{
    internal SchemaInfo_Memgraph(DatastoreModel model, Neo4jPersistenceProvider persistenceProvider) : base(model, persistenceProvider) { }

    protected override void Initialize()
    {
        using (Session.Begin())
        {
            Indexes = LoadData("SHOW INDEX INFO", record => NewIndexInfo(record, PersistenceProvider)).Where(item => item.Entity is not null && item.Field is not null).ToArray();
            Constraints = LoadData("SHOW CONSTRAINT INFO", record => NewConstraintInfo(record, PersistenceProvider)).Where(item => item.Entity is not null && item.Field is not null).ToArray();
            Labels = LoadData("SHOW NODE_LABELS INFO", record => record.Values["node labels"].As<string>());
            RelationshipTypes = LoadData("SHOW EDGE_TYPES INFO", record => record.Values["edge types"].As<string>());
        }
        FunctionalIds = [];
    }
    protected override long FindMaxId(FunctionalId functionalId) => throw new NotSupportedException("FunctionalIds are not supported on Memgraph.");

    protected override ConstraintInfo NewConstraintInfo(RawRecord rawRecord, Neo4jPersistenceProvider neo4JPersistenceProvider) => new ConstraintInfo_Memgraph(rawRecord, neo4JPersistenceProvider);
    protected override IndexInfo NewIndexInfo(RawRecord rawRecord, Neo4jPersistenceProvider neo4JPersistenceProvider) => new IndexInfo_Memgraph(rawRecord, neo4JPersistenceProvider);
    internal override ApplyConstraintProperty NewApplyConstraintProperty(ApplyConstraintEntity parent, Property property, List<(ApplyConstraintAction, string?)> commands) => new ApplyConstraintPropertyMemgraph(parent, property, commands);
    internal override ApplyConstraintProperty NewApplyConstraintProperty(ApplyConstraintEntity parent, string property, List<(ApplyConstraintAction, string?)> commands) => new ApplyConstraintPropertyMemgraph(parent, property, commands);
    internal override ApplyCompositeConstraint NewApplyCompositeConstraint(ApplyConstraintEntity parent, string[] names, List<(ApplyConstraintAction, string?)> acts) =>
        new ApplyCompositeConstraintMemgraph(parent, names, acts);

    internal override IReadOnlyList<ApplyFunctionalId> GetFunctionalIdDifferences() => [];
    internal override void UpdateFunctionalIds()
    {
        using (Session.Begin())
        {
            foreach (var diff in GetFunctionalIdDifferences())
            {
                Parser.Log(diff.ToString());
                foreach (var query in diff.ToCypher())
                    Session.RunningSession.Run(query);
            }
        }
    }
    internal override void UpdateConstraints()
    {
        using (Session.Begin())
        {
            foreach (var diff in GetConstraintDifferences())
            {
                foreach (var action in diff.Actions)
                {
                    foreach (var cql in action.ToCypher())
                    {
                        Parser.Log(cql);
                        Session.RunningSession.Run(cql);
                    }
                }
            }
        }
    }

    //internal override void RemoveIndexesAndContraints(Property property)
    //{
    //    if (!property.Nullable || property.IndexType != IndexType.None)
    //    {
    //        property.Nullable = true;
    //        property.IndexType = IndexType.None;

    //        foreach (var entity in property.Parent.GetSubclassesOrSelf())
    //        {
    //            ApplyConstraintEntity applyConstraint = NewApplyConstraintEntity(entity);
    //            foreach (var action in applyConstraint.Actions.Where(c => c.Property == property.Name))
    //            {
    //                foreach (string query in action.ToCypher())
    //                {
    //                    Parser.Execute(query, null, !PersistenceProvider.IsMemgraph);
    //                }
    //            }
    //        }
    //    }
    //}
    internal override List<(ApplyConstraintAction, string?)> ComputeCommands(
        IEntity entity,
        IndexType indexType,
        bool nullable,
        bool isKey,
        IEnumerable<ConstraintInfo> constraints,
        IEnumerable<IndexInfo> indexes)
    {
        var commands = new List<(ApplyConstraintAction, string?)>();
        var constraintsAndIndex = GetRelevantConstraintsAndIndex(entity, constraints, indexes);

        indexType = AdjustIndexTypeBasedOnContext(entity, indexType, isKey);
        ManageIndexConstraints(commands, indexType, constraintsAndIndex.Unique, constraintsAndIndex.Index);
        HandleKeyConstraints(commands, isKey, constraintsAndIndex.Key);
        HandleMandatoryConstraints(commands, nullable, constraintsAndIndex.Mandatory);

        return commands;
    }

    private static IndexType AdjustIndexTypeBasedOnContext(IEntity entity, IndexType indexType, bool isKey)
    {
        if (entity.IsAbstract && indexType == IndexType.Unique)
            return IndexType.Indexed;

        return isKey ? IndexType.None : indexType;
    }

    private void ManageIndexConstraints(
        List<(ApplyConstraintAction, string?)> commands,
        IndexType indexType,
        ConstraintInfo? uniqueConstraint,
        IndexInfo? indexInfo)
    {
        switch (indexType)
        {
            case IndexType.None:
                DeleteUniqueConstraintIfPresent(commands, uniqueConstraint);
                DeleteIndexIfNoOwningConstraint(commands, indexInfo);
                break;
            case IndexType.Indexed:
                TransitionToIndexed(commands, uniqueConstraint, indexInfo);
                break;
            case IndexType.Unique:
                TransitionToUnique(commands, uniqueConstraint, indexInfo);
                break;
        }
    }

    private static void DeleteUniqueConstraintIfPresent(List<(ApplyConstraintAction, string?)> commands, ConstraintInfo? constraint)
    {
        if (constraint != null)
            commands.Add((ApplyConstraintAction.DeleteUniqueConstraint, constraint.Name));
    }

    private static void DeleteIndexIfNoOwningConstraint(List<(ApplyConstraintAction, string?)> commands, IndexInfo? indexInfo)
    {
        if (indexInfo != null && indexInfo.OwningConstraint == null)
            commands.Add((ApplyConstraintAction.DeleteIndex, indexInfo.Name));
    }

    private void TransitionToIndexed(List<(ApplyConstraintAction, string?)> commands, ConstraintInfo? uniqueConstraint, IndexInfo? indexInfo)
    {
        DeleteUniqueConstraintIfPresent(commands, uniqueConstraint);
        if (uniqueConstraint != null || indexInfo == null)
            commands.Add((ApplyConstraintAction.CreateIndex, null));
    }

    private void TransitionToUnique(List<(ApplyConstraintAction, string?)> commands, ConstraintInfo? uniqueConstraint, IndexInfo? indexInfo)
    {
        if (uniqueConstraint == null)
        {
            DeleteIndexIfNoOwningConstraint(commands, indexInfo);
            commands.Add((ApplyConstraintAction.CreateUniqueConstraint, null));
        }
    }

    private static void HandleKeyConstraints(List<(ApplyConstraintAction, string?)> commands, bool isKey, ConstraintInfo? keyConstraint)
    {
        if (!isKey && keyConstraint != null)
            commands.Add((ApplyConstraintAction.DeleteKeyConstraint, keyConstraint.Name));
        else if (isKey && keyConstraint == null)
            commands.Add((ApplyConstraintAction.CreateKeyConstraint, null));
    }

    private static void HandleMandatoryConstraints(List<(ApplyConstraintAction, string?)> commands, bool nullable, ConstraintInfo? mandatoryConstraint)
    {
        if (nullable && mandatoryConstraint != null)
            commands.Add((ApplyConstraintAction.DeleteExistsConstraint, mandatoryConstraint.Name));
        else if (!nullable && mandatoryConstraint == null)
            commands.Add((ApplyConstraintAction.CreateExistsConstraint, null));
    }

    private static (ConstraintInfo? Unique, ConstraintInfo? Mandatory, ConstraintInfo? Key, IndexInfo? Index) GetRelevantConstraintsAndIndex(
        IEntity entity,
        IEnumerable<ConstraintInfo> constraints,
        IEnumerable<IndexInfo> indexes)
    {
        if (entity.IsVirtual)
            return (null, null, null, null);

        return (
            constraints.FirstOrDefault(c => c.IsUnique),
            constraints.FirstOrDefault(c => c.IsMandatory),
            constraints.FirstOrDefault(c => c.IsKey),
            indexes.FirstOrDefault(i => i.IsIndexed)
        );
    }
}