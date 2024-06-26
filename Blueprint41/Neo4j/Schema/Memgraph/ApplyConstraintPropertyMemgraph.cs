﻿using System.Collections.Generic;
using System.Linq;
using Blueprint41.Neo4j.Refactoring;
using Blueprint41.Neo4j.Schema.v5;

namespace Blueprint41.Neo4j.Schema.Memgraph;

public class ApplyConstraintPropertyMemgraph : ApplyConstraintPropertyV5
{
    internal ApplyConstraintPropertyMemgraph(ApplyConstraintEntity parent, Property property, List<(ApplyConstraintAction actionEnum, string? constraintOrIndexName)> commands) : base(parent, property, commands) { }
    internal ApplyConstraintPropertyMemgraph(ApplyConstraintEntity parent, string property, List<(ApplyConstraintAction actionEnum, string? constraintOrIndexName)> commands) : base(parent, property, commands) { }

    internal override List<string> ToCypher()
    {
        IEntity entity = Parent.Entity;
        List<string> commands = [];
        foreach ((ApplyConstraintAction action, _) in Commands)
            if (ShouldAddConstraint(action, entity))
            {
                string command = GenerateConstraintCommand(action, entity);
                if (!string.IsNullOrEmpty(command))
                {
                    commands.Add(command);
                    Parser.Log(command);
                }
            }

        return commands;
    }

    private bool ShouldAddConstraint(ApplyConstraintAction action, IEntity entity)
    {
        var features = entity is Entity ? PersistenceProvider.NodePropertyFeatures : PersistenceProvider.RelationshipPropertyFeatures;

        return action switch
        {
            ApplyConstraintAction.CreateIndex or ApplyConstraintAction.DeleteIndex => features.Index,
            ApplyConstraintAction.CreateUniqueConstraint or ApplyConstraintAction.DeleteUniqueConstraint => features.Unique,
            ApplyConstraintAction.CreateExistsConstraint or ApplyConstraintAction.DeleteExistsConstraint => features.Exists,
            ApplyConstraintAction.CreateKeyConstraint or ApplyConstraintAction.DeleteKeyConstraint => features.Key,
            ApplyConstraintAction.CreateCompositeUniqueConstraint or ApplyConstraintAction.DeleteCompositeUniqueConstraint => features.CompositeUnique,
            _ => false,
        };
    }

    private string GenerateConstraintCommand(ApplyConstraintAction action, IEntity entity)
    {
        string targetEntityType = (entity is Entity) ? $"(node:{entity.Neo4jName})" : $"()-[rel:{entity.Neo4jName}]-()";
        string alias = (entity is Entity) ? "node" : "rel";
        string propertyName = Property;

        return action switch
        {
            ApplyConstraintAction.CreateIndex => CreateIndexCommand(entity.Neo4jName, propertyName),
            ApplyConstraintAction.CreateKeyConstraint => CreateKeyConstraintCommand(targetEntityType, alias, propertyName),
            ApplyConstraintAction.CreateUniqueConstraint => CreateUniqueConstraintCommand(targetEntityType, alias, propertyName),
            ApplyConstraintAction.CreateExistsConstraint => CreateExistsConstraintCommand(targetEntityType, alias, propertyName),
            ApplyConstraintAction.DeleteUniqueConstraint => DropUniqueConstraintCommand(targetEntityType, alias, propertyName),
            ApplyConstraintAction.DeleteExistsConstraint => DropExistsConstraintCommand(targetEntityType, alias, propertyName),
            ApplyConstraintAction.DeleteIndex => DropIndexCommand(entity.Neo4jName, propertyName),
            ApplyConstraintAction.DeleteKeyConstraint => DropKeyConstraintCommand(targetEntityType, alias, propertyName),
            ApplyConstraintAction.CreateCompositeUniqueConstraint => CreateCompositeUniqueConstraintCommand(targetEntityType, alias, propertyName),
            ApplyConstraintAction.DeleteCompositeUniqueConstraint => DropCompositeUniqueConstraintCommand(targetEntityType, alias, propertyName),
            _ => string.Empty,
        };
    }


    private static string CreateIndexCommand(string neo4jName, string propertyName) => $"CREATE INDEX ON :{neo4jName}({propertyName})";
    private string CreateKeyConstraintCommand(string targetEntityType, string alias, string propertyName) => 
        $"{CreateExistsConstraintCommand(targetEntityType, alias, propertyName)}; " +
        $"{CreateUniqueConstraintCommand(targetEntityType, alias, propertyName)}";
    private static string CreateUniqueConstraintCommand(string targetEntityType, string alias, string propertyName) =>
        $"CREATE CONSTRAINT ON {targetEntityType} ASSERT {alias}.{propertyName} IS UNIQUE";

    private static string CreateExistsConstraintCommand(string targetEntityType, string alias, string propertyName) => $"CREATE CONSTRAINT ON {targetEntityType} ASSERT EXISTS ({alias}.{propertyName})";
    private static string DropUniqueConstraintCommand(string targetEntityType, string alias, string propertyName) => $"DROP CONSTRAINT ON {targetEntityType} ASSERT {alias}.{propertyName} IS UNIQUE";
    private static string CreateCompositeUniqueConstraintCommand(string targetEntityType, string alias, params string[] propertyNames)
    {
        var withAlias = propertyNames.Select(p => $"{alias}.{p}");
        return $"CREATE CONSTRAINT ON {targetEntityType} ASSERT {string.Join(",", withAlias)} IS UNIQUE";
    }
    private static string DropCompositeUniqueConstraintCommand(string targetEntityType, string alias, params string[] propertyNames)
    {
        var withAlias = propertyNames.Select(p => $"{alias}.{p}");
        return $"DROP CONSTRAINT ON {targetEntityType} ASSERT {string.Join(",", withAlias)} IS UNIQUE";
    }
    private static string DropExistsConstraintCommand(string targetEntityType, string alias, string propertyName) => $"DROP CONSTRAINT ON {targetEntityType} ASSERT EXISTS ({alias}.{propertyName})";
    private static string DropIndexCommand(string neo4jName, string propertyName) => $"DROP INDEX ON :{neo4jName}({propertyName})";
    private string DropKeyConstraintCommand(string targetEntityType, string alias, string propertyName) => $"{DropExistsConstraintCommand(targetEntityType, alias, propertyName)}; " +
                $"{DropUniqueConstraintCommand(targetEntityType, alias, propertyName)}";
}
