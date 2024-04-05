﻿using Blueprint41.Core;
using Neo4j.Driver;

namespace Blueprint41.Neo4j.Persistence.Driver.v3
{
    public partial class Neo4jPersistenceProvider : Void.Neo4jPersistenceProvider
    {
        private IDriver? driver = null;
        public IDriver Driver
        {
            get
            {
                if (driver is null)
                {
                    lock (typeof(Neo4jPersistenceProvider))
                    {
                        if (driver is null)
                            driver = GraphDatabase.Driver(Uri, AuthTokens.Basic(Username, Password));
                    }
                }
                return driver;
            }
        }

        private Neo4jPersistenceProvider() : this(null, null, null) { }
        public Neo4jPersistenceProvider(string? uri, string? username, string? password, AdvancedConfig? advancedConfig = null) : base(uri, username, password, advancedConfig)
        {
            Core.ExtensionMethods.RegisterAsConversion(typeof(Neo4jPersistenceProvider), typeof(RawNode), from => from is INode item ? new Neo4jRawNode(item) : null);
            Core.ExtensionMethods.RegisterAsConversion(typeof(Neo4jPersistenceProvider), typeof(RawRelationship), from => from is IRelationship item ? new Neo4jRawRelationship(item) : null);
        }

        public override Session NewSession(bool readWriteMode) => new Neo4jSession(this, readWriteMode, TransactionLogger);
        public override Transaction NewTransaction(bool readWriteMode) => new Neo4jTransaction(this, readWriteMode, TransactionLogger);
    }
}
