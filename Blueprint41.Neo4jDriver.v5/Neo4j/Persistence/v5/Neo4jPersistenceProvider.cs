using System;
using System.Collections.Generic;
using System.Linq;

using Neo4j.Driver;

using Blueprint41.Core;

namespace Blueprint41.Neo4j.Persistence.Driver.v5
{
    public partial class Neo4jPersistenceProvider : Void.Neo4jPersistenceProvider
    {
        private const int MAX_CONNECTION_POOL_SIZE = 400;
        private const int DEFAULT_READWRITESIZE = 65536;
        private const int DEFAULT_READWRITESIZE_MAX = 655360;
        private IDriver? driver = null;

        public IDriver Driver
        {
            get
            {
                if (driver is null)
                {
                    lock (typeof(Neo4jPersistenceProvider))
                    {
                        driver ??= GraphDatabase.Driver(Uri, AuthTokens.Basic(Username, Password),
                                o =>
                                {
                                    o.WithFetchSize(Config.Infinite);
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

        private class HostResolver(AdvancedConfig config) : IServerAddressResolver
        {
            public ISet<ServerAddress> Resolve(ServerAddress address)
            {
                return new HashSet<ServerAddress>(config.DNSResolverHook!(new Neo4jHost() { Host = address.Host, Port = address.Port }).Select(host => ServerAddress.From(host.Host, host.Port)));
            }
        }

        private Neo4jPersistenceProvider() : this(null, null, null)
        {
        }

        public Neo4jPersistenceProvider(string? uri, string? username, string? password, AdvancedConfig? advancedConfig = null) : base(uri, username, password, advancedConfig)
        {
            Core.ExtensionMethods.RegisterAsConversion(typeof(Neo4jPersistenceProvider), typeof(RawNode), from => from is INode item ? new Neo4jRawNode(item) : null);
            Core.ExtensionMethods.RegisterAsConversion(typeof(Neo4jPersistenceProvider), typeof(RawRelationship), from => from is IRelationship item ? new Neo4jRawRelationship(item) : null);
        }

        public Neo4jPersistenceProvider(string? uri, string? username, string? password, string database, AdvancedConfig? advancedConfig = null) : base(uri, username, password, database, advancedConfig)
        {
            Core.ExtensionMethods.RegisterAsConversion(typeof(Neo4jPersistenceProvider), typeof(RawNode), from => from is INode item ? new Neo4jRawNode(item) : null);
            Core.ExtensionMethods.RegisterAsConversion(typeof(Neo4jPersistenceProvider), typeof(RawRelationship), from => from is IRelationship item ? new Neo4jRawRelationship(item) : null);
        }

        public override Session NewSession(bool readWriteMode) => new Neo4jSession(this, readWriteMode, TransactionLogger);

        public override Transaction NewTransaction(bool readWriteMode) => new Neo4jTransaction(this, readWriteMode, TransactionLogger);

        public override Bookmark FromToken(string consistencyToken) => Neo4jBookmark.FromTokenInternal(consistencyToken);

        public override string ToToken(Bookmark consistency) => Neo4jBookmark.ToTokenInternal(consistency);

        internal CustomTaskScheduler TaskScheduler
        {
            get
            {
                if (taskScheduler is null)
                {
                    lock (sync)
                    {
                        if (taskScheduler is null)
                        {
                            CustomTaskQueueOptions main = new(10, 50);
                            CustomTaskQueueOptions sub = new(4, 20);
                            taskScheduler = new CustomThreadSafeTaskScheduler(main, sub);
                        }
                    }
                }

                return taskScheduler;
            }
        }

        private CustomTaskScheduler? taskScheduler = null;
        private static readonly object sync = new();

        public Neo4jPersistenceProvider ConfigureTaskScheduler(CustomTaskQueueOptions mainQueue) => ConfigureTaskScheduler(mainQueue, CustomTaskQueueOptions.Disabled);

        public Neo4jPersistenceProvider ConfigureTaskScheduler(CustomTaskQueueOptions mainQueue, CustomTaskQueueOptions subQueue)
        {
            lock (sync)
            {
                if (taskScheduler is not null)
                    throw new InvalidOperationException("You can only configure the TaskScheduler during initialization of Blueprint41.");

                taskScheduler = new CustomThreadSafeTaskScheduler(mainQueue, subQueue);
            }

            return this;
        }
    }
}