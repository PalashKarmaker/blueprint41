using Blueprint41.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Blueprint41.Core.RelationshipPersistenceProvider;
using persistence = Blueprint41.Neo4j.Persistence;

namespace Blueprint41;

[Flags]
public enum EventOptions
{
    SupressEvents = 0,
    EntityEvents = 1,
    GraphEvents = 2,
    AllEvents = 3,
}

public abstract class Transaction : DisposableScope<Transaction>, IStatementRunner
{
    protected Transaction()
    {
        InTransaction = true;
        DisableForeignKeyChecks = false;

        PersistenceProvider factory = PersistenceProvider.CurrentPersistenceProvider;
        PersistenceProviderFactory = factory;
    }

    #region Transaction Logic

    private static readonly Bookmark NullConsistency = new();

    public static Transaction RunningTransaction
    {
        get
        {
            Transaction? trans = Current ?? throw new InvalidOperationException("There is no transaction, you should create one first -> using (Transaction.Begin()) { ... Transaction.Commit(); }");
            if (!trans.InTransaction)
                throw new InvalidOperationException("The transaction was already committed or rolled back.");

            return trans;
        }
    }

    public bool DisableForeignKeyChecks { get; set; }

    public bool InTransaction { get; private set; }

    public OptimizeFor Mode { get; set; }

    public DateTime TransactionDate { get; private set; }

    public static Transaction Begin() => Begin(true, OptimizeFor.PartialSubGraphAccess);

    public static Transaction Begin(bool readWriteMode) => Begin(readWriteMode, OptimizeFor.PartialSubGraphAccess);

    public static Transaction Begin(OptimizeFor mode) => Begin(true, mode);

    public static Transaction Begin(bool readWriteMode, OptimizeFor mode)
    {
        if (PersistenceProvider.CurrentPersistenceProvider is null)
            throw new InvalidOperationException("PersistenceProviderFactory should be set before you start doing transactions.");

        Transaction trans = PersistenceProvider.CurrentPersistenceProvider.NewTransaction(readWriteMode);
        trans.RaiseOnBegin();
        trans.Attach();
        trans.TransactionDate = DateTime.UtcNow;
        trans.Mode = mode;
        trans.FireEvents = EventOptions.AllEvents;

        return trans;
    }

    public static void Commit()
    {
        Transaction trans = RunningTransaction;
        Commit(trans);
    }

    public static void Commit(Transaction trans)
    {
        bool repeat = false;
        do
        {
            try
            {
                repeat = false;
                trans.FlushInternal();
                trans.ApplyFunctionalIds();
                trans.CommitInternal();
            }
            catch (Exception e)
            {
                if (e.Message.Contains("can't acquire ExclusiveLock", StringComparison.InvariantCultureIgnoreCase) || e.Message.Contains("can't acquire UpdateLock", StringComparison.InvariantCultureIgnoreCase))
                {
                    repeat = true;

                    trans.actions.Clear();
                    foreach (var item in trans.forRetry)
                        trans.actions.AddLast(item);
                    trans.forRetry.Clear();

                    foreach (OGMImpl entity in trans.registeredEntities.Values.SelectMany(item => item.Values).Where(item => item is OGMImpl).ToList().Cast<OGMImpl>())
                        if (trans.beforeCommitEntityState.TryGetValue(entity, out var state))
                            entity.PersistenceState = state;
                    trans.beforeCommitEntityState.Clear();
                    trans.RetryInternal();
                }
                else
                    throw e;
            }
        }
        while (repeat);

        trans.Invalidate();
        trans.InTransaction = false;
    }

    public static void Flush()
    {
        Transaction trans = RunningTransaction;
        trans.FlushInternal();
    }

    public static void Rollback()
    {
        Transaction trans = RunningTransaction;
        Rollback(trans);
    }

    public static void Rollback(Transaction trans)
    {
        trans.RollbackInternal();
        trans.Invalidate();
        trans.InTransaction = false;
    }

    public virtual Bookmark GetConsistency() => NullConsistency;

    public abstract RawResult Run(string cypher, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    public abstract RawResult Run(string cypher, Dictionary<string, object?>? parameters, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    public virtual Transaction WithConsistency(Bookmark consistency) => this;

    public virtual Transaction WithConsistency(string consistencyToken) => this;

    public Transaction WithConsistency(params Bookmark[] consistency)
    {
        if (consistency is not null)
        {
            foreach (Bookmark item in consistency)
                WithConsistency(item);
        }

        return this;
    }

    public Transaction WithConsistency(params string[] consistencyTokens)
    {
        if (consistencyTokens is not null)
        {
            foreach (string item in consistencyTokens)
                WithConsistency(item);
        }

        return this;
    }
    protected abstract void ApplyFunctionalId(FunctionalId functionalId);

    protected void ApplyFunctionalIds()
    {
        foreach (FunctionalId functionalId in DatastoreModel.RegisteredModels.SelectMany(model => model.FunctionalIds).Where(item => item is not null))
        {
            ApplyFunctionalId(functionalId);
        }
    }

    protected override void Cleanup()
    {
        if (InTransaction)
            Rollback();
    }

    protected abstract void CommitInternal();

    // Flush is private for now, until RelationshipActions will have their own persistence state.
    protected virtual void FlushInternal()
    {
        List<OGM> entities = registeredEntities.Values.SelectMany(item => item.Values).Where(item => item is OGMImpl).ToList();
        for (int i = 0; i < entities.Count; i++)
        {
            OGMImpl entity = (OGMImpl)entities[i];
            if (entity.PersistenceState == PersistenceState.Persisted || entity.PersistenceState == PersistenceState.Deleted)
                continue;

            if (HasChanges(entity))
            {
                if (!beforeCommitEntityState.ContainsKey(entity))
                    beforeCommitEntityState.Add(entity, entity.PersistenceState);

                entity.GetEntity().RaiseOnSave((OGMImpl)entity, this);
                foreach (EntityEventArgs item in entity.EventHistory)
                    item.Flush();
            }
        }

        List<KeyValuePair<string, Dictionary<OGM, OGM>>> sortedItems = [.. registeredEntities.OrderBy(item => item.Key)]; // key is entity name
        if (!DisableForeignKeyChecks)
        {
            foreach (var entitySet in sortedItems)
            {
                foreach (OGM entity in entitySet.Value.Values.OrderBy(item => item.GetKey()))
                {
                    if (entity.PersistenceState == PersistenceState.Persisted || entity.PersistenceState == PersistenceState.Deleted)
                        continue;

                    if (HasChanges(entity))
                        entity.ValidateSave();
                }
            }
        }

        foreach (var entitySet in sortedItems)
        {
            foreach (OGM entity in entitySet.Value.Values.OrderBy(item => item.GetKey()))
            {
                if (entity.PersistenceState == PersistenceState.Persisted || entity.PersistenceState == PersistenceState.Deleted)
                    continue;

                if (HasChanges(entity))
                    entity.Save();
            }
        }

        if (actions is not null)
        {
            foreach (var action in actions)
            {
                action.ExecuteInDatastore();
                forRetry.AddLast(action);
            }
            actions.Clear();
        }

        foreach (var entitySet in sortedItems)
        {
            foreach (OGM entity in entitySet.Value.Values.OrderBy(item => item.GetKey()))
            {
                if (entity.PersistenceState == PersistenceState.Persisted || entity.PersistenceState == PersistenceState.Deleted)
                    continue;

                if (entity.PersistenceState == PersistenceState.Delete || entity.PersistenceState == PersistenceState.ForceDelete)
                {
                    if (!beforeCommitEntityState.ContainsKey(entity))
                        beforeCommitEntityState.Add(entity, entity.PersistenceState);

                    entity.ValidateDelete();
                }
            }
        }

        foreach (var entitySet in sortedItems)
            foreach (OGM entity in entitySet.Value.Values.OrderBy(item => item.GetKey()))
            {
                if (entity.PersistenceState == PersistenceState.Persisted || entity.PersistenceState == PersistenceState.Deleted)
                    continue;

                if (entity.PersistenceState == PersistenceState.Delete || entity.PersistenceState == PersistenceState.ForceDelete)
                {
                    entity.Save();
                    object? key = entity.GetKey();
                    if (key is not null && entitiesByKey.TryGetValue(entity.GetEntity().Name, out Dictionary<object, OGM>? cache))
                        cache.Remove(key);
                    //entitySet.Remove(entity);
                }
            }

        static bool HasChanges(OGM entity) =>
            entity.PersistenceState != PersistenceState.New && entity.PersistenceState != PersistenceState.Delete && entity.PersistenceState != PersistenceState.HasUid && entity.PersistenceState != PersistenceState.DoesntExist && entity.PersistenceState != PersistenceState.ForceDelete && entity.PersistenceState != PersistenceState.Loaded;

        foreach (Core.EntityCollectionBase collection in registeredCollections.Values.SelectMany(item => item.Values).SelectMany(item => item))
            collection.AfterFlush();

        foreach (var entity in from OGMImpl entity in entities
                               where entity.PersistenceState == PersistenceState.Persisted || entity.PersistenceState == PersistenceState.Deleted
                               select entity)
        {
            entity.GetEntity().RaiseOnAfterSave((OGMImpl)entity, this);
            foreach (EntityEventArgs item in entity.EventHistory)
                item.Flush();
        }
    }
    protected abstract void RetryInternal();

    protected abstract void RollbackInternal();
    #endregion Transaction Logic

    #region Registration

    private readonly Dictionary<OGM, PersistenceState> beforeCommitEntityState = [];
    private readonly Dictionary<string, Dictionary<string, HashSet<EntityCollectionBase>>> registeredCollections = new(100);
    private readonly Dictionary<string, Dictionary<OGM, OGM>> registeredEntities = new(50);
    private readonly Dictionary<string, Dictionary<object, OGM>> entitiesByKey = new(50);

    public OGM? GetEntityByKey(string? type, object key)
    {
        if (key is null || type is null)
            return null;

        if (!entitiesByKey.TryGetValue(type, out Dictionary<object, OGM>? values))
            return null;

        if (!values.TryGetValue(key, out OGM? item))
            return null;

        return item;
    }

    internal void Register(OGM item)
    {
        if (item is null)
            return;

        item.Transaction = this;

        string entityName = item.GetEntity().Name;

        if (!registeredEntities.TryGetValue(entityName, out Dictionary<OGM, OGM>? values))
        {
            values = new Dictionary<OGM, OGM>(1000);
            registeredEntities.Add(entityName, values);
        }

        if (values.TryGetValue(item, out OGM? inSet))
        {
            if (inSet.PersistenceState != PersistenceState.DoesntExist && inSet == item)
                throw new InvalidOperationException("You cannot register an already loaded object.");
        }
        else
        {
            values.Add(item, item);
        }
    }

    internal void Register(string type, OGM item, bool noError = false)
    {
        object? key = item.GetKey();
        if (key is null)
            return;

        if (!entitiesByKey.TryGetValue(type, out Dictionary<object, OGM>? values))
        {
            values = new Dictionary<object, OGM>(1000);
            entitiesByKey.Add(type, values);
        }

        if (values.TryGetValue(key, out OGM? value))
        {
            OGM similarItem = value;
            if (!noError && similarItem.PersistenceState != PersistenceState.HasUid && similarItem.PersistenceState != PersistenceState.DoesntExist)
                throw new InvalidOperationException("You cannot register an already loaded object.");
            else
                values[key] = item;
        }
        else
            values.Add(key, item);
    }

    internal void Register(Core.EntityCollectionBase item)
    {
        if (item is null)
            return;

        item.DbTransaction = this;

        string relationshipName = item.Relationship.Name;

        if (!registeredCollections.TryGetValue(relationshipName, out Dictionary<string, HashSet<EntityCollectionBase>>? properties))
        {
            properties = [];
            registeredCollections.Add(relationshipName, properties);
        }

        string propertyName = string.Concat(item.Parent.GetEntity().Name, ".", item.ParentProperty?.Name ?? "NonExisting");

        if (!properties.TryGetValue(propertyName, out HashSet<EntityCollectionBase>? values))
        {
            values = [];
            properties.Add(propertyName, values);
        }

        if (values.Contains(item))
            throw new InvalidOperationException("You cannot register an already loaded collection.");

        values.Add(item);
    }

    private void Invalidate()
    {
        forRetry.Clear();

        foreach (OGM item in registeredEntities.Values.SelectMany(item => item.Values))
        {
            item.PersistenceState = PersistenceState.OutOfScope;
            item.Transaction = null;
        }
        registeredEntities.Clear();

        foreach (Core.EntityCollectionBase item in registeredCollections.Values.SelectMany(item => item.Values).SelectMany(item => item))
            item.DbTransaction = null;

        registeredCollections.Clear();
    }
    #endregion Registration

    #region Action Distribution

    private readonly LinkedList<RelationshipAction> actions = new();
    private readonly LinkedList<RelationshipAction> forRetry = new();

    internal void LoadAll(Core.EntityCollectionBase collection)
    {
        if (collection is null)
            return;

        string relationshipName = collection.Relationship.Name;

        if (!registeredCollections.TryGetValue(relationshipName, out Dictionary<string, HashSet<EntityCollectionBase>>? properties))
        {
            properties = [];
            registeredCollections.Add(relationshipName, properties);
        }

        string propertyName = string.Concat(collection.Parent.GetEntity().Name, ".", collection.ParentProperty?.Name ?? "NonExisting");

        if (properties.TryGetValue(propertyName, out HashSet<EntityCollectionBase>? values))
        {
            List<Core.EntityCollectionBase> collections = values.Where(item => !item.IsLoaded).ToList();

            const int chunkSize = 10000;
            int initialSize = Math.Min(chunkSize, collections.Count);
            foreach (var chunk in collections.Chunks(chunkSize))
            {
                List<OGM> parents = new(initialSize);
                foreach (Core.EntityCollectionBase item in chunk)
                    if (item.Parent.PersistenceState != PersistenceState.New && item.Parent.PersistenceState != PersistenceState.NewAndChanged)
                        parents.Add(item.Parent);

                Dictionary<OGM, CollectionItemList> allItems = RelationshipPersistenceProvider.Load(parents, collection);

                foreach (Core.EntityCollectionBase item in chunk)
                {
                    if (allItems.TryGetValue(item.Parent, out CollectionItemList? items))
                        item.InitialLoad(items.Items);
                    else
                        item.InitialLoad([]);
                }
            }
        }

        if (!collection.IsLoaded)
            collection.InitialLoad([]);
    }

    internal void Register(RelationshipAction action)
    {
        actions.AddLast(action);
        Distribute(action);
    }

    internal void Register(LinkedList<RelationshipAction> actions)
    {
        foreach (RelationshipAction action in actions)
            Register(action);
    }

    internal void Replay(Core.EntityCollectionBase collection)
    {
        foreach (RelationshipAction action in actions)
            action.ExecuteInMemory(collection);
    }

    private void Distribute(RelationshipAction action)
    {
        List<EntityCollectionBase>? collections = registeredCollections.SelectMany(item => item.Value).SelectMany(item => item.Value).ToList();
        foreach (Core.EntityCollectionBase collection in collections)
            action.ExecuteInMemory(collection);
    }
    #endregion Action Distribution

    #region PersistenceProviderFactory

    public PersistenceProvider PersistenceProviderFactory { get; private set; }
    internal NodePersistenceProvider NodePersistenceProvider => PersistenceProviderFactory.NodePersistenceProvider;
    internal RelationshipPersistenceProvider RelationshipPersistenceProvider => PersistenceProviderFactory.RelationshipPersistenceProvider;

    #endregion PersistenceProviderFactory

    #region Query

    public static Query.IBlankQuery CompiledQuery
    {
        get
        {
            return new Query.Query(Current?.PersistenceProviderFactory ?? PersistenceProvider.CurrentPersistenceProvider);
        }
    }

    public static Query.ICallSubquery CompiledSubQuery
    {
        get
        {
            return new Query.Query(Current?.PersistenceProviderFactory ?? PersistenceProvider.CurrentPersistenceProvider);
        }
    }

    #endregion Query

    #region Events

    private static EventHandler<TransactionEventArgs>? onBegin;

    private Dictionary<string, object?>? customState = null;

    private EventHandler<TransactionEventArgs>? onCommit;

    public static event EventHandler<TransactionEventArgs> OnBegin
    {
        add { onBegin += value; }
        remove { onBegin -= value; }
    }

    public event EventHandler<TransactionEventArgs> OnCommit
    {
        add { onCommit += value; }
        remove { onCommit -= value; }
    }

    public static bool HasRegisteredOnBeginHandlers
    { get { return onBegin is not null; } }

    public IDictionary<string, object?> CustomState
    {
        get
        {
            if (customState is null)
            {
                lock (this)
                {
                    customState ??= [];
                }
            }
            return customState;
        }
    }

    public EventOptions FireEvents { get; private set; }

    public bool HasRegisteredOnCommitHandlers
    { get { return onCommit is not null; } }

    internal bool FireEntityEvents
    { get { return (FireEvents & EventOptions.EntityEvents) == EventOptions.EntityEvents; } }

    internal bool FireGraphEvents
    { get { return (FireEvents & EventOptions.GraphEvents) == EventOptions.GraphEvents; } }

    public static void Execute(Action action, EventOptions withEvents = EventOptions.GraphEvents)
    {
        Transaction trans = RunningTransaction;
        EventOptions oldValue = trans.FireEvents;
        trans.FireEvents = withEvents;

        try
        {
            action.Invoke();
        }
        finally
        {
            trans.FireEvents = oldValue;
        }
    }

    public static T Execute<T>(Func<T> action, EventOptions withEvents = EventOptions.GraphEvents)
    {
        if (action is null)
            return default!;

        T result;
        Transaction trans = RunningTransaction;
        EventOptions oldValue = trans.FireEvents;
        trans.FireEvents = withEvents;

        try
        {
            result = action.Invoke();
        }
        finally
        {
            trans.FireEvents = oldValue;
        }

        return result;
    }
    internal void RaiseOnBegin()
    {
        TransactionEventArgs args = TransactionEventArgs.CreateInstance(EventTypeEnum.OnBegin, this);
        onBegin?.Invoke(this, args);
    }
    internal void RaiseOnCommit()
    {
        if (!this.FireEntityEvents)
            return;

        TransactionEventArgs args = TransactionEventArgs.CreateInstance(EventTypeEnum.OnCommit, this);
        onCommit?.Invoke(this, args);

        // Wipe custom-state so the garbage collection can collect it. If anyone thinks this causes a bug for them, feel free to remove this line of code :o)
        customState = null;
    }
    #endregion Events
}