﻿using Blueprint41.Core;
using Blueprint41.Log;
using Blueprint41.Neo4j.Model;
using ND = Neo4j.Driver;

namespace Blueprint41.Neo4j.Persistence.Void;

public partial class Neo4jPersistenceProvider : PersistenceProvider
{
    protected const int MAX_CONNECTION_POOL_SIZE = 400;
    protected const int DEFAULT_READWRITESIZE = 65536;
    protected const int DEFAULT_READWRITESIZE_MAX = 655360;
    protected ND.IDriver? driver = null;
    protected readonly object _lockObject = new();

    // Precision (roughly) 10.8
    internal const decimal DECIMAL_FACTOR = 100000000m;

    public TransactionLogger? TransactionLogger { get; private set; }
    public string? Uri { get; private set; }
    public string? Username { get; private set; }
    public string? Password { get; private set; }
    public string? Database { get; private set; }
    public AdvancedConfig? AdvancedConfig { get; private set; }

    public override ND.IDriver Driver
    {
        get
        {
            if (driver is null)
            {
                lock (_lockObject)
                {
                    driver ??= ND.GraphDatabase.Driver(Uri, ND.AuthTokens.Basic(Username, Password),
                            o =>
                            {
                                o.WithFetchSize(ND.Config.Infinite);
                                o.WithMaxConnectionPoolSize(MAX_CONNECTION_POOL_SIZE);
                                o.WithDefaultReadBufferSize(DEFAULT_READWRITESIZE);
                                o.WithDefaultWriteBufferSize(DEFAULT_READWRITESIZE);
                                o.WithMaxReadBufferSize(DEFAULT_READWRITESIZE_MAX);
                                o.WithMaxWriteBufferSize(DEFAULT_READWRITESIZE_MAX);
                                o.WithMaxTransactionRetryTime(TimeSpan.Zero);

                                if (AdvancedConfig?.DNSResolverHook is not null)
                                    o.WithResolver(new HostResolver(AdvancedConfig));
                            }
                        );
                }
            }
            return driver;
        }
    }
    protected class HostResolver(AdvancedConfig config) : ND.IServerAddressResolver
    {
        public ISet<ND.ServerAddress> Resolve(ND.ServerAddress address) => new HashSet<ND.ServerAddress>(config.DNSResolverHook!(new Neo4jHost() { Host = address.Host, Port = address.Port }).Select(host => ND.ServerAddress.From(host.Host, host.Port)));
    }
    private Neo4jPersistenceProvider() : this(null, null, null, null)
    {
    }

    public Neo4jPersistenceProvider(string? uri, string? username, string? password, AdvancedConfig? advancedConfig = null) : base()
    {
        Uri = uri;
        Username = username;
        Password = password;
        Database = null;
        AdvancedConfig = advancedConfig;
        TransactionLogger = advancedConfig?.GetLogger();

        _nodePropertyFeatures = new Lazy<FeatureSupport>(() => new FeatureSupport()
        {
            Index = true,
            Exists = IsEnterpriseEdition || IsMemgraph,
            Unique = true,
            CompositeUnique = true,//TODO: Check for v4
            Key = !IsMemgraph && IsEnterpriseEdition && VersionGreaterOrEqual(5, 7),
            Type = !IsMemgraph && IsEnterpriseEdition && VersionGreaterOrEqual(5, 9),
        });
        _relationshipPropertyFeatures = new Lazy<FeatureSupport>(() => new FeatureSupport()
        {
            Index = !IsMemgraph && VersionGreaterOrEqual(4, 3),
            Exists = !IsMemgraph && IsEnterpriseEdition,
            CompositeUnique = !IsMemgraph && VersionGreaterOrEqual(5, 7),//TODO: Check for v4
            Unique = !IsMemgraph && VersionGreaterOrEqual(5, 7),
            Key = !IsMemgraph && IsEnterpriseEdition && VersionGreaterOrEqual(5, 7),
            Type = !IsMemgraph && IsEnterpriseEdition && VersionGreaterOrEqual(5, 9),
        });
    }

    public Neo4jPersistenceProvider(string? uri, string? username, string? password, string database, AdvancedConfig? advancedConfig = null) : base()
    {
        Uri = uri;
        Username = username;
        Password = password;
        Database = database;
        AdvancedConfig = advancedConfig;
        TransactionLogger = advancedConfig?.GetLogger();

        _nodePropertyFeatures = new Lazy<FeatureSupport>(() => new FeatureSupport()
        {
            Index = true,
            Exists = IsEnterpriseEdition || IsMemgraph,
            Unique = true,
            CompositeUnique = true,
            Key = !IsMemgraph && IsEnterpriseEdition && VersionGreaterOrEqual(5, 7),
            Type = !IsMemgraph && IsEnterpriseEdition && VersionGreaterOrEqual(5, 9),
        });
        _relationshipPropertyFeatures = new Lazy<FeatureSupport>(() => new FeatureSupport()
        {
            Index = !IsMemgraph && VersionGreaterOrEqual(4, 3),
            Exists = !IsMemgraph && IsEnterpriseEdition,
            Unique = !IsMemgraph && VersionGreaterOrEqual(5, 7),
            CompositeUnique = !IsMemgraph && VersionGreaterOrEqual(5, 7),
            Key = !IsMemgraph && IsEnterpriseEdition && VersionGreaterOrEqual(5, 7),
            Type = !IsMemgraph && IsEnterpriseEdition && VersionGreaterOrEqual(5, 9),
        });
    }

    public string DBMSName { get; internal set; } = string.Empty;
    public string Version { get; internal set; } = "0.0.0";
    public int Major { get; internal set; } = 0;
    public int Minor { get; internal set; } = 0;
    public int? Revision { get; internal set; } = null;
    public bool IsAura { get; set; } = false;
    public bool IsEnterpriseEdition { get; set; } = false;

    public override FeatureSupport NodePropertyFeatures => _nodePropertyFeatures.Value;
    private readonly Lazy<FeatureSupport> _nodePropertyFeatures;

    public override FeatureSupport RelationshipPropertyFeatures => _relationshipPropertyFeatures.Value;
    private readonly Lazy<FeatureSupport> _relationshipPropertyFeatures;

    public bool VersionGreaterOrEqual(int major, int minor = 0, int revision = 0)
    {
        if (Major > major) return true;
        if (Major == major && Minor > minor) return true;
        if (Major == major && Minor == minor && Revision >= revision) return true;

        return false;
    }

    public bool HasFunction(string function) => functions.Contains(function);

    protected HashSet<string> functions = [];

    public bool HasProcedure(string procedure) => procedures.Contains(procedure);

    protected HashSet<string> procedures = [];
    private QueryTranslator? translator = null;

    internal override QueryTranslator Translator
    {
        get
        {
            if (translator is null)
            {
                lock (this)
                {
                    if (translator is null)
                    {
                        if (ShouldUseVoidTranslator())
                            return GetVoidTranslator();

                        FetchDatabaseInfo();
                        translator = DetermineTranslator();
                    }
                }
            }

            return translator;
        }
    }

    private bool ShouldUseVoidTranslator() =>
        GetType() == typeof(Neo4jPersistenceProvider) || (Uri is null && Username is null && Password is null);

    private void FetchDatabaseInfo()
    {
        using (Transaction.Begin())
        {
            var components = Transaction.RunningTransaction.Run("call dbms.components() yield name, versions, edition unwind versions as version return name, version, edition").First();

            DBMSName = components["name"].As<string>();
            Version = components["version"].As<string>();
            IsEnterpriseEdition = components["edition"].As<string>().ToLowerInvariant() == "enterprise";

            ParseVersion(Version);
            LoadDbmsFunctions();
            LoadDbmsProcedures();
        }
    }

    private void ParseVersion(string version)
    {
        string[] parts = version.Split(['.']);
        Major = int.Parse(parts[0]);
        Minor = int.Parse(parts[1].Replace("-aura", ""));
        IsAura = parts[1].Contains("-aura");

        if (parts.Length > 2) Revision = int.Parse(parts[2]);
    }

    private void LoadDbmsFunctions() => functions = new HashSet<string>(Transaction.RunningTransaction.Run(GetFunctions(DBMSName, Major))
            .Select(item => item.Values["name"].As<string>()));

    private void LoadDbmsProcedures() => procedures = new HashSet<string>(Transaction.RunningTransaction.Run(GetProcedures(DBMSName, Major))
            .Select(item => item.Values["name"].As<string>()));

    private QueryTranslator DetermineTranslator()
    {
        if (DBMSName.IndexOf("memgraph", StringComparison.InvariantCultureIgnoreCase) >= 0)
            return new Memgraph.Neo4jQueryTranslator(this);

        return Major switch
        {
            3 => new v3.Neo4jQueryTranslator(this),
            4 => new v4.Neo4jQueryTranslator(this),
            5 => new v5.Neo4jQueryTranslator(this),
            _ => throw new NotSupportedException($"Neo4j v{Version} is not supported by this version of Blueprint41, please upgrade to a later version.")
        };
    }

    private static string GetFunctions(string name, int version)
    {
        if (name.IndexOf("neo4j", StringComparison.InvariantCultureIgnoreCase) >= 0)
        {
            return version switch
            {
                < 5 => "call dbms.functions() yield name return name",
                >= 5 => "show functions yield name return name",
            };
        }
        else if (name.IndexOf("memgraph", StringComparison.InvariantCultureIgnoreCase) >= 0)
        {
            return "call mg.functions() yield name return name";
        }
        else
        {
            return string.Empty;
        }
    }

    private static string GetProcedures(string name, int version)
    {
        if (name.IndexOf("neo4j", StringComparison.InvariantCultureIgnoreCase) >= 0)
        {
            return version switch
            {
                < 5 => "call dbms.procedures() yield name as name",
                >= 5 => "show procedures yield name return name",
            };
        }
        else if (name.IndexOf("memgraph", StringComparison.InvariantCultureIgnoreCase) >= 0)
        {
            return "call mg.procedures() yield name return name";
        }
        else
        {
            return string.Empty;
        }
    }

    private QueryTranslator GetVoidTranslator()
    {
        if (voidTranslator is null)
            lock (typeof(PersistenceProvider))
                voidTranslator ??= new Void.Neo4jQueryTranslator(this);

        return voidTranslator;
    }

    private static QueryTranslator? voidTranslator = null;

    public override Session NewSession(bool readWriteMode) => new Neo4jSession(readWriteMode, TransactionLogger);

    public override Transaction NewTransaction(bool readWriteMode) => new Neo4jTransaction(readWriteMode, TransactionLogger);

    public static readonly PersistenceProvider VoidPersistenceProvider = new Neo4jPersistenceProvider();
}

public record FeatureSupport
{
    public bool Index;
    public bool Exists;
    public bool Unique;
    public bool Key;
    public bool Type;
    public bool CompositeUnique;
}

/*

Neo4j type  Dotnet type Description                                                 Value range
----------- ----------- ----------------------------------------------------------- ------------------------------------------------------
boolean     bool        bit                                                         true/false
byte        sbyte       8-bit integer                                               -128 to 127, inclusive
short       short       16-bit integer                                              -32768 to 32767, inclusive
int         int         32-bit integer                                              -2147483648 to 2147483647, inclusive
long        long        64-bit integer                                              -9223372036854775808 to 9223372036854775807, inclusive
float       float       32-bit IEEE 754 floating-point number
double      double      64-bit IEEE 754 floating-point number
char        char        16-bit unsigned integers representing Unicode characters    u0000 to uffff(0 to 65535)
String      string      sequence of Unicode characters

*/