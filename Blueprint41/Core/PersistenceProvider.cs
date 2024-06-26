﻿using Blueprint41.Neo4j.Model;
using Blueprint41.Neo4j.Persistence.Void;
using Blueprint41.Neo4j.Schema;
using Neo4j.Driver;

namespace Blueprint41.Core;

public abstract class PersistenceProvider
{
    public abstract IDriver Driver { get; }
#pragma warning disable IDE0200

    protected PersistenceProvider()
    {
        //WARNING: do not refactor warning IDE0200, the wrong translator will be returned
        supportedTypeMappings = new Lazy<List<TypeMapping>>(delegate ()
                {
                    return Translator.FilterSupportedTypeMappings(GetSupportedTypeMappings()).ToList();
                },
                true
            );

        convertToStoredType = new Lazy<Dictionary<Type, Conversion?>>(
                delegate ()
                {
                    return SupportedTypeMappings.ToDictionary(item => item.ReturnType, item => Conversion.GetConverter(item.ReturnType, item.PersistedType));
                },
                true
            );

        convertFromStoredType = new Lazy<Dictionary<Type, Conversion?>>(
                delegate ()
                {
                    return SupportedTypeMappings.ToDictionary(item => item.ReturnType, item => Conversion.GetConverter(item.PersistedType, item.ReturnType));
                },
                true
            );

        _nodePersistenceProvider = new Lazy<NodePersistenceProvider>(() => Translator.GetNodePersistenceProvider());
        _relationshipPersistenceProvider = new Lazy<RelationshipPersistenceProvider>(() => Translator.GetRelationshipPersistenceProvider());
        _templates = new Lazy<RefactorTemplates>(() => Translator.GetTemplates()); //do not refactor warning IDE0200, the wrong translator will be returned
    }

#pragma warning restore  IDE0200

    internal NodePersistenceProvider NodePersistenceProvider => _nodePersistenceProvider.Value;
    private readonly Lazy<NodePersistenceProvider> _nodePersistenceProvider;

    internal RelationshipPersistenceProvider RelationshipPersistenceProvider => _relationshipPersistenceProvider.Value;
    private readonly Lazy<RelationshipPersistenceProvider> _relationshipPersistenceProvider;

    public abstract Session NewSession(bool readWriteMode);

    public abstract Transaction NewTransaction(bool readWriteMode);

    public bool IsMemgraph { get; set; } = false;
    public abstract FeatureSupport NodePropertyFeatures { get; }
    public abstract FeatureSupport RelationshipPropertyFeatures { get; }

    public virtual string ToToken(Bookmark consistency) => string.Empty;

    public virtual Bookmark FromToken(string consistencyToken) => Bookmark.NullBookmark;

    public IReadOnlyList<TypeMapping> SupportedTypeMappings => supportedTypeMappings.Value;
    private readonly Lazy<List<TypeMapping>> supportedTypeMappings;

    protected internal abstract List<TypeMapping> GetSupportedTypeMappings();

    internal Dictionary<Type, Conversion?> ConvertToStoredTypeCache
    { get { return convertToStoredType.Value; } }

    private readonly Lazy<Dictionary<Type, Conversion?>> convertToStoredType;

    internal Dictionary<Type, Conversion?> ConvertFromStoredTypeCache
    { get { return convertFromStoredType.Value; } }

    private readonly Lazy<Dictionary<Type, Conversion?>> convertFromStoredType;

    public static PersistenceProvider CurrentPersistenceProvider { get; set; } = Neo4jPersistenceProvider.VoidPersistenceProvider;
    public static bool IsConfigured => CurrentPersistenceProvider is Neo4jPersistenceProvider { Uri: not null };

    public static bool IsNeo4j
    {
        get
        {
            if (CurrentPersistenceProvider is null)
                return false;

            return CurrentPersistenceProvider.GetType().IsSubclassOfOrSelf(typeof(Neo4jPersistenceProvider));
        }
    }

    #region Conversion

    public object? ConvertToStoredType<TValue>(TValue value) => ConvertToStoredTypeCacheVia<TValue>.Convert(this, value);

    public object? ConvertToStoredType(Type returnType, object? value)
    {
        if (returnType is null)
            return value;

        if (!ConvertToStoredTypeCache.TryGetValue(returnType, out Conversion? converter))
            return value;

        if (converter is null)
            return value;

        return converter.Convert(value);
    }

    public TReturnType ConvertFromStoredType<TReturnType>(object? value) => (TReturnType)ConvertFromStoredTypeCacheVia<TReturnType>.Convert(this, value)!;

    public object? ConvertFromStoredType(Type returnType, object? value)
    {
        if (returnType is null)
            return value;

        if (!ConvertFromStoredTypeCache.TryGetValue(returnType, out Conversion? converter))
            return value;

        if (converter is null)
            return value;

        return converter.Convert(value);
    }

    public Conversion? GetConverterToStoredType(Type returnType)
    {
        if (returnType is null)
            return null;

        ConvertToStoredTypeCache.TryGetValue(returnType, out Conversion? converter);

        return converter;
    }

    public Conversion? GetConverterFromStoredType(Type returnType)
    {
        if (returnType is null)
            return null;

        ConvertFromStoredTypeCache.TryGetValue(returnType, out Conversion? converter);

        return converter;
    }

    public Type? GetStoredType<TReturnType>()
    {
        ConvertFromStoredTypeCacheVia<TReturnType>.Initialize(this);
        return ConvertFromStoredTypeCacheVia<TReturnType>.Converter?.FromType ?? typeof(TReturnType);
    }

    public Type? GetStoredType(Type returnType)
    {
        if (!ConvertFromStoredTypeCache.TryGetValue(returnType, out Conversion? converter))
            return null;

        return converter?.FromType ?? returnType;
    }

    private class ConvertToStoredTypeCacheVia<TReturnType>
    {
        public static Conversion? Converter = null;
        public static bool IsInitialized = false;

        public static object? Convert(PersistenceProvider factory, object? value)
        {
            Initialize(factory);

            if (Converter is null)
                return value;

            return Converter.Convert(value);
        }

        internal static void Initialize(PersistenceProvider factory)
        {
            if (!IsInitialized)
            {
                lock (typeof(ConvertFromStoredTypeCacheVia<TReturnType>))
                {
                    if (!IsInitialized)
                    {
                        factory.ConvertToStoredTypeCache.TryGetValue(typeof(TReturnType), out Converter);
                        IsInitialized = true;
                    }
                }
            }
        }
    }

    private class ConvertFromStoredTypeCacheVia<TReturnType>
    {
        public static Conversion? Converter = null;
        public static bool IsInitialized = false;

        public static object? Convert(PersistenceProvider factory, object? value)
        {
            Initialize(factory);

            if (Converter is null)
                return value;

            return Converter.Convert(value);
        }

        internal static void Initialize(PersistenceProvider factory)
        {
            if (!IsInitialized)
            {
                lock (typeof(ConvertFromStoredTypeCacheVia<TReturnType>))
                {
                    if (!IsInitialized)
                    {
                        factory.ConvertFromStoredTypeCache.TryGetValue(typeof(TReturnType), out Converter);
                        IsInitialized = true;
                    }
                }
            }
        }
    }

    #endregion Conversion

    #region Factory: Refactoring Templates

    internal RefactorTemplates Templates => _templates.Value;
    private readonly Lazy<RefactorTemplates> _templates;

    #endregion Factory: Refactoring Templates

    #region Factory: Query Translator

    internal abstract QueryTranslator Translator { get; }

    #endregion Factory: Query Translator

    #region Factory: Schema Info

    internal SchemaInfo GetSchemaInfo(DatastoreModel datastoreModel) => Translator.GetSchemaInfo(datastoreModel);

    #endregion Factory: Schema Info
}