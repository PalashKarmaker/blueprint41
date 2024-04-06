using System.Collections.Generic;
using Blueprint41.Neo4j.Persistence.Void;

namespace Blueprint41.Neo4j.Schema;

public abstract class ApplyConstraintBase
{
    protected ApplyConstraintBase(ApplyConstraintEntity parent, List<(ApplyConstraintAction actionEnum, string? constraintOrIndexName)> commands)
    {
        Parent = parent;
        Commands = commands;
        PersistenceProvider = (Neo4jPersistenceProvider)Parent.Entity.Parent.PersistenceProvider;
    }

    protected ApplyConstraintEntity Parent { get; private set; }
    protected Neo4jPersistenceProvider PersistenceProvider { get; private set; }

    public IReadOnlyList<(ApplyConstraintAction actionEnum, string? constraintOrIndexName)> Commands { get; protected set; }

    /// <summary>
    /// Turns actions into cypher queries.
    /// </summary>
    /// <returns></returns>
    internal abstract List<string> ToCypher();
}
