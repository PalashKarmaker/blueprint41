using System.Collections.Generic;
using System.Linq;
using Blueprint41.Neo4j.Refactoring;

namespace Blueprint41.Neo4j.Schema.Memgraph;

public class ApplyCompositeConstraintMemgraph : ApplyCompositeConstraint
{

    internal ApplyCompositeConstraintMemgraph(ApplyConstraintEntity parent, string[] propertyNames, List<(ApplyConstraintAction actionEnum, string? constraintOrIndexName)> commands)
     : base(parent, propertyNames, commands) { }

    /// <summary>
    /// Turns actions into cypher queries.
    /// </summary>
    /// <returns></returns>
    internal override List<string> ToCypher()
    {
        // TODO: What about if the constraint is for a property on a relationship
        var entity = Parent.Entity;

        List<string> commands = [];
        foreach ((var actionEnum, _) in Commands)
            switch (actionEnum)
            {
                case ApplyConstraintAction.CreateCompositeUniqueConstraint:
                    if (Parent.Entity is Entity)
                    {
                        if (PersistenceProvider.NodePropertyFeatures.CompositeUnique)// TODO: check it may be unique only
                            commands.Add(CreateCompositeUniqueConstraintCommand(entity.Neo4jName, propertyNames: PropertyNames));
                    }
                    else if (Parent.Entity is Relationship && PersistenceProvider.RelationshipPropertyFeatures.CompositeUnique)
                        commands.Add(CreateCompositeUniqueConstraintCommand(entity.Neo4jName, true, PropertyNames));
                    break;
                case ApplyConstraintAction.DeleteCompositeUniqueConstraint:
                    if (PersistenceProvider.NodePropertyFeatures.CompositeUnique)
                        commands.Add(DropCompositeUniqueConstraintCommand(entity.Neo4jName, PropertyNames));
                    break;
                default:
                    break;
            }

        foreach (var command in commands)
            Parser.Log(command);
        return commands;
    }
    protected virtual string CreateCompositeUniqueConstraintCommand(string label, bool forRelationShip = false, params string[] propertyNames)
    {
        var withAlias = propertyNames.Select(p => $"n.{p}");
        return $"CREATE CONSTRAINT ON (n:{label}) ASSERT {string.Join(",", withAlias)} IS UNIQUE";
    }
    protected virtual string DropCompositeUniqueConstraintCommand(string label, params string[] propertyNames)
    {
        var withAlias = propertyNames.Select(p => $"n.{p}");
        return $"DROP CONSTRAINT ON (n:{label}) ASSERT {string.Join(",", withAlias)} IS UNIQUE";
    }
}
