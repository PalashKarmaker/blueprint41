namespace Blueprint41.Neo4j.Schema;

public enum ApplyConstraintAction
{
    CreateIndex,
    CreateUniqueConstraint,
    CreateExistsConstraint,
    CreateKeyConstraint,
    DeleteIndex,
    DeleteUniqueConstraint,
    DeleteExistsConstraint,
    DeleteKeyConstraint,
    CreateCompositeUniqueConstraint,
    DeleteCompositeUniqueConstraint
}
