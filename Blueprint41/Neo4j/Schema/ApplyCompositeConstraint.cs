using System.Collections.Generic;
using System.Linq;
using Blueprint41.Neo4j.Refactoring;

namespace Blueprint41.Neo4j.Schema;

public class ApplyCompositeConstraint : ApplyConstraintBase
{

    internal ApplyCompositeConstraint(ApplyConstraintEntity parent, string[] propertyNames, List<(ApplyConstraintAction actionEnum, string? constraintOrIndexName)> commands)
     : base(parent, commands) => PropertyNames = propertyNames;

    public string[] PropertyNames { get; protected set; }

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
                case ApplyConstraintAction.CreateCompositeUniqueConstraint:
                    if (PersistenceProvider.NodePropertyFeatures.Exists)
                        commands.Add(CreateCompositeUniqueConstraintCommand(entity.Label.Name, PropertyNames));
                    break;
                case ApplyConstraintAction.DeleteCompositeUniqueConstraint:
                    if (PersistenceProvider.NodePropertyFeatures.Exists)
                        commands.Add(DropCompositeUniqueConstraintCommand(entity.Label.Name, PropertyNames));
                    break;
                default:
                    break;
            }
        }

        foreach (var command in commands)
            Parser.Log(command);
        return commands;
    }
    protected virtual string CreateCompositeUniqueConstraintCommand(string label, params string[] propertyNames)
    {
        var withAlias = propertyNames.Select(p => $"n.{p}");
        return $"CREATE CONSTRAINT ON (n:{label}) ASSERT {string.Join(",", withAlias)} IS UNIQUE";
    }
    protected virtual string DropCompositeUniqueConstraintCommand(string label, params string[] propertyNames)
    {
        var withAlias = propertyNames.Select(p => $"n.{p}");
        return $"DROP CONSTRAINT ON (n:{label}) ASSERT {string.Join(",", withAlias)} IS UNIQUE";
    }
    public override string ToString()
    {
        string diff = string.Join(", ", Commands.Select(item => item.ToString()).ToArray());
        return $"Differences for {Parent.Entity.Name}.({string.Join(",", PropertyNames)}) -> {diff}";
    }
}
