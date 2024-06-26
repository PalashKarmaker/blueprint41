﻿using Blueprint41.Query;

namespace Blueprint41.Core;

public abstract class NodePersistenceProvider
{
    protected NodePersistenceProvider(PersistenceProvider factory) => this.PersistenceProviderFactory = factory;

    public PersistenceProvider PersistenceProviderFactory { get; private set; }

    public abstract List<T> GetAll<T>(Entity entity) where T : class, OGM;

    public abstract List<T> GetAll<T>(Entity entityint, int page = 0, int pageSize = 0, bool ascending = true, params Property[] orderBy) where T : class, OGM;

    public abstract List<T> LoadWhere<T>(Entity entity, string conditions, Parameter[] parameters, int page = 0, int pageSize = 0, bool ascending = true, params Property[] orderBy) where T : class, OGM;

    public abstract List<T> LoadWhere<T>(Entity entity, ICompiled query, Parameter[] parameters, int page = 0, int pageSize = 0, bool ascending = true, params Property[] orderBy) where T : class, OGM;

    internal abstract List<T> Search<T>(Entity entity, string text, Property[] properties, int page = 0, int pageSize = 0, bool ascending = true, params Property[] orderBy) where T : class, OGM;

    public abstract void Load(OGM item, bool locked = false);

    public abstract void Insert(OGM item);

    public abstract void Update(OGM item);

    public abstract void Delete(OGM item);

    public abstract void ForceDelete(OGM item);

    public abstract string NextFunctionID(FunctionalId functionalId);

    public abstract bool RelationshipExists(EntityProperty foreignProperty, OGM instance);
    public abstract List<T> LoadWhereChunked<T>(Entity entity, ICompiled query, Parameter[] parameters, int skip = 0, int? take = null, bool ascending = true, params Property[] orderBy) where T : class, OGM;
}