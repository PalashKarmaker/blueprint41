using Blueprint41.Core;
using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blueprint41.Neo4j.Persistence.Driver.v4
{
    public partial class Neo4jPersistenceProvider : Void.Neo4jPersistenceProvider
    {
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
                            CustomTaskQueueOptions main = new CustomTaskQueueOptions(10, 50);
                            CustomTaskQueueOptions sub = new CustomTaskQueueOptions(4, 20);
                            taskScheduler = new CustomThreadSafeTaskScheduler(main, sub);
                        }
                    }
                }

                return taskScheduler;
            }
        }

        private CustomTaskScheduler? taskScheduler = null;
        private static object sync = new object();

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