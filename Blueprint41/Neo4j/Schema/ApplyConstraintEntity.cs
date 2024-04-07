using System.Collections.Generic;
using System.Linq;

namespace Blueprint41.Neo4j.Schema;

public class ApplyConstraintEntity
{
    internal ApplyConstraintEntity(SchemaInfo parent, IEntity entity)
    {
        Parent = parent;
        Entity = entity;

        Actions = Initialize();
    }

    internal virtual List<ApplyConstraintBase> Initialize()
    {

        List<ApplyConstraintBase> actions = new();
        IEnumerable<ConstraintInfo> entityConstraints = Parent.Constraints.Where(item => item.Entity.Count == 1 && item.Entity[0] == Entity.Neo4jName);
        IEnumerable<IndexInfo> entityIndexes = Parent.Indexes.Where(item => item.Entity.Count == 1 && item.Entity[0] == Entity.Neo4jName);
        IReadOnlyList<Property> properties = Entity.GetPropertiesOfBaseTypesAndSelf();

        foreach (Property property in properties)
        {
            if (property.SystemReturnType is null)
                continue;

            IEnumerable<ConstraintInfo> constraints = entityConstraints.Where(item => item.Field.Count == 1 && item.Field[0] == property.Name);
            IEnumerable<IndexInfo> indexes = entityIndexes.Where(item => item.Field.Count == 1 && item.Field[0] == property.Name);

            List<(ApplyConstraintAction, string?)> commands = Parent.ComputeCommands(Entity, property.IndexType, property.Nullable, property.IsKey, constraints, indexes);

            if (commands.Count > 0)
                actions.Add(Parent.NewApplyConstraintProperty(this, property, commands));
        }
        List<string> propertiesWithIndexOrConstraint = entityIndexes.SelectMany(item => item.Field).Union(entityConstraints.SelectMany(item => item.Field)).Distinct().ToList();
        List<string> propertiesThatAreAllowedToHaveIndexOrConstraint = properties.Where(property => property.SystemReturnType is not null).Select(item => item.Name).ToList();

        foreach (string property in propertiesWithIndexOrConstraint.Where(item => !propertiesThatAreAllowedToHaveIndexOrConstraint.Contains(item)))
        {
            IEnumerable<ConstraintInfo> constraints = entityConstraints.Where(item => item.Field.Count == 1 && item.Field[0] == property);
            IEnumerable<IndexInfo> indexes = entityIndexes.Where(item => item.Field.Count == 1 && item.Field[0] == property);

            List<(ApplyConstraintAction, string?)> commands = Parent.ComputeCommands(Entity, IndexType.None, true, false, constraints, indexes);

            if (commands.Count > 0)
                actions.Add(Parent.NewApplyConstraintProperty(this, property, commands));
        }

        ApplyCompositeConstraints(actions);
        return actions;
    }

    private void ApplyCompositeConstraints(List<ApplyConstraintBase> actions)
    {
        foreach ((var names, var cIndexType) in Entity.CompositeConstraints)
        {
            if (cIndexType != IndexType.Unique)
                continue; //TODO: Yet to be implemented
            var acts = new List<(ApplyConstraintAction, string?)>()
            {
                new(ApplyConstraintAction.DeleteCompositeUniqueConstraint, null),
                new(ApplyConstraintAction.CreateCompositeUniqueConstraint, null)
            };
            actions.Add(Parent.NewApplyCompositeConstraint(this, names, acts));
        }
    }

    protected SchemaInfo Parent { get; set; }
    public IEntity Entity { get; protected set; }
    public IReadOnlyList<ApplyConstraintBase> Actions { get; protected set; }

    public override string ToString()
    {
        return $"Differences for {Entity.Name}";
    }
}
