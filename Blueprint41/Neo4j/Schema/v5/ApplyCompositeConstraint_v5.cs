using System.Collections.Generic;
using System.Linq;

namespace Blueprint41.Neo4j.Schema.v5;

public class ApplyCompositeConstraint_v5 : ApplyCompositeConstraint
{
    internal ApplyCompositeConstraint_v5(ApplyConstraintEntity parent, string[] propertyNames, List<(ApplyConstraintAction actionEnum, string? constraintOrIndexName)> commands)
     : base(parent, propertyNames, commands) { }
    private string BuildConstraintName(string label, string constraintType, params string[] propertyNames) =>
        $"{label}_" + string.Join("_", propertyNames) + $"_{constraintType}Constraint";
    protected override string CreateCompositeUniqueConstraintCommand(string label, params string[] propertyNames)
    {
        var withAlias = propertyNames.Select(p => $"n.{p}");
        return $"CREATE CONSTRAINT {BuildConstraintName(label, "Unique", propertyNames)} FOR (n:{label}) REQUIRE ({string.Join(",", withAlias)}) IS UNIQUE";
    }
    protected override string DropCompositeUniqueConstraintCommand(string label, params string[] propertyNames) =>
        $"DROP CONSTRAINT {BuildConstraintName(label, "Unique", propertyNames)}";    
}