using System.Collections.Generic;
using System.Linq;
using Blueprint41.Neo4j.Refactoring;

namespace Blueprint41.Neo4j.Schema;
public class ApplyConstraintProperty: ApplyConstraintBase
{
    internal ApplyConstraintProperty(ApplyConstraintEntity parent, Property property, List<(ApplyConstraintAction actionEnum, string? constraintOrIndexName)> commands)
        : base(parent, commands) => Property = property.Name;

    internal ApplyConstraintProperty(ApplyConstraintEntity parent, string property, List<(ApplyConstraintAction actionEnum, string? constraintOrIndexName)> commands)
     : base(parent, commands) => Property = property;

    public string Property { get; protected set; }

    /// <summary>
    /// Turns actions into cypher queries.
    /// </summary>
    /// <returns></returns>
    internal override List<string> ToCypher()
    {
        // TODO: What about if the constraint is for a property on a relationship
        Entity entity = (Entity)Parent.Entity;

        List<string> commands = new();
        foreach (var (actionEnum, constraintOrIndexName) in Commands)
        {
            switch (actionEnum)
            {
                case ApplyConstraintAction.CreateIndex:
                    if (PersistenceProvider.NodePropertyFeatures.Index)
                        commands.Add($"CREATE INDEX ON :{entity.Label.Name}({Property})");
                    break;
                case ApplyConstraintAction.CreateUniqueConstraint:
                    if (PersistenceProvider.NodePropertyFeatures.Unique)
                        commands.Add($"CREATE CONSTRAINT ON (node:{entity.Label.Name}) ASSERT node.{Property} IS UNIQUE");
                    break;
                case ApplyConstraintAction.CreateExistsConstraint:
                    if (PersistenceProvider.NodePropertyFeatures.Exists)
                        commands.Add($"CREATE CONSTRAINT ON (node:{entity.Label.Name}) ASSERT exists(node.{Property})");
                    break;
                case ApplyConstraintAction.DeleteIndex:
                    if (PersistenceProvider.NodePropertyFeatures.Index)
                        commands.Add($"DROP INDEX ON :{entity.Label.Name}({Property})");
                    break;
                case ApplyConstraintAction.DeleteUniqueConstraint:
                    if (PersistenceProvider.NodePropertyFeatures.Unique)
                        commands.Add($"DROP CONSTRAINT ON (node:{entity.Label.Name}) ASSERT node.{Property} IS UNIQUE");
                    break;
                case ApplyConstraintAction.DeleteExistsConstraint:
                    if (PersistenceProvider.NodePropertyFeatures.Exists)
                        commands.Add($"DROP CONSTRAINT ON (node:{entity.Label.Name}) ASSERT exists(node.{Property})");
                    break;
                default:
                    break;
            }
        }

        foreach (var command in commands)
            Parser.Log(command);
        return commands;
    }
    public override string ToString()
    {
        string diff = string.Join(", ", Commands.Select(item => item.ToString()).ToArray());
        return $"Differences for {Parent.Entity.Name}.{Property} -> {diff}";
    }
}
