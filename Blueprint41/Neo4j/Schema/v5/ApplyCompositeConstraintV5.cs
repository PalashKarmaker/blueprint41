using Blueprint41.Neo4j.Refactoring;
using System.Collections.Generic;
using System.Linq;

namespace Blueprint41.Neo4j.Schema.v5;

public class ApplyCompositeConstraintV5 : ApplyCompositeConstraint
{
    internal ApplyCompositeConstraintV5(ApplyConstraintEntity parent, string[] propertyNames, List<(ApplyConstraintAction actionEnum, string? constraintOrIndexName)> commands)
     : base(parent, propertyNames, commands) { }
    internal override List<string> ToCypher()
    {
        // TODO: Fix case where constraint is for a property on a relationship
        //       https://neo4j.com/docs/cypher-manual/current/constraints/
        var entity = Parent.Entity;

        List<string> commands = new();
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

    private string BuildConstraintName(string label, string constraintType, params string[] propertyNames) =>
        $"{label}_" + string.Join("_", propertyNames) + $"_{constraintType}Constraint";
    protected override string CreateCompositeUniqueConstraintCommand(string label, bool forRelationShip = false, params string[] propertyNames)
    {
        if (forRelationShip)
        {
            var withAlias = propertyNames.Select(p => $"r.{p}");
            return $"CREATE CONSTRAINT {BuildConstraintName(label, "Unique", propertyNames)} FOR ()-[r:{label}]->() REQUIRE ({string.Join(",", withAlias)}) IS UNIQUE";
        }
        else
        {
            var withAlias = propertyNames.Select(p => $"n.{p}");
            return $"CREATE CONSTRAINT {BuildConstraintName(label, "Unique", propertyNames)} FOR (n:{label}) REQUIRE ({string.Join(",", withAlias)}) IS UNIQUE";
        }
    }
    protected override string DropCompositeUniqueConstraintCommand(string label, params string[] propertyNames) =>
        $"DROP CONSTRAINT {BuildConstraintName(label, "Unique", propertyNames)} IF EXISTS";
}