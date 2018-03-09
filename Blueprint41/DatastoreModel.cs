﻿using Blueprint41.Core;
using Blueprint41.Neo4j.Persistence;
using Blueprint41.Neo4j.Refactoring;
using Blueprint41.Neo4j.Refactoring.Templates;
using Blueprint41.Neo4j.Schema;
using Force.Crc32;
using Neo4j.Driver.V1;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using model = Blueprint41.Neo4j.Model;

namespace Blueprint41
{
    public abstract class DatastoreModel : IRefactorGlobal
    {
        protected DatastoreModel()
        {
            Entities = new EntityCollection(this);
            Relations = new RelationshipCollection(this);
            FunctionalIds = new FunctionalIdCollection(this);

            Labels = new model.LabelCollection();
            RelationshipTypes = new model.RelationshipTypeCollection();
        }

        public EntityCollection Entities { get; private set; }
        public RelationshipCollection Relations { get; private set; }
        public FunctionalIdCollection FunctionalIds { get; private set; }
        public static List<DatastoreModel> RegisteredModels { get; } = new List<DatastoreModel>();

        public bool IsUpgraded
        {
            get
            {
                return executed;
            }
        }

        internal model.LabelCollection Labels { get; private set; }

        internal model.RelationshipTypeCollection RelationshipTypes { get; private set; }

        #region Execute

        private bool executed = false;

        internal List<UpgradeScript> GetUpgradeScripts()
        {
            List<UpgradeScript> scripts = new List<UpgradeScript>();

            foreach (MethodInfo info in GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                VersionAttribute attr = info.GetCustomAttribute<VersionAttribute>();
                if (attr != null)
                {
                    Action method = (Action)Delegate.CreateDelegate(typeof(Action), this, info);
                    scripts.Add(new UpgradeScript(method, attr.Major, attr.Minor, attr.Patch, info.Name));
                }
            }

            return scripts;
        }

        public void Execute(bool upgradeDatastore)
        {
            Execute(upgradeDatastore, item => true, true);

            lock (RegisteredModels)
            {
                if (RegisteredModels.Any(item => item.GetType() == this.GetType()))
                    return;

                RegisteredModels.Add(this);
            }
        }

        internal void Execute(bool upgradeDatastore, Predicate<UpgradeScript> predicate, bool standAloneScript)
        {
#pragma warning disable CS0618 // Type or member is obsolete

            if (executed && standAloneScript)
                throw new InvalidOperationException();

            List<UpgradeScript> scripts = GetUpgradeScripts();

            scripts.Sort();
            bool anyScriptRan = false;
            foreach (UpgradeScript script in scripts.Where(item => predicate.Invoke(item)))
            {
                if (upgradeDatastore && PersistenceProvider.IsNeo4j)
                {
                    using (Transaction.Begin())
                    {
                        if (!Parser.HasScript(script))
                        {
                            Debug.WriteLine("Running script: {0}.{1}.{2} ({3})", script.Major, script.Minor, script.Patch, script.Name);
                            Stopwatch sw = Stopwatch.StartNew();

                            Refactor.ApplyFunctionalIds();
                            script.Method.Invoke();
                            Refactor.ApplyFunctionalIds();

                            Parser.CommitScript(script);
                            anyScriptRan = true;
                            sw.Stop();
                            Debug.WriteLine("Finished script in {0} ms.", sw.ElapsedMilliseconds);
                        }
                        else
                        {
                            script.Method.Invoke();
                        }
                    }
                }
                else
                {
                    script.Method.Invoke();
                }
            }

            if (upgradeDatastore)
            {
                using (Transaction.Begin())
                {
                    if (!anyScriptRan && Parser.ShouldRefreshFunctionalIds())
                    {
                        Refactor.ApplyConstraints();
                        Refactor.ApplyFunctionalIds();
                        Parser.SetLastRun();
                    }
                    if (anyScriptRan)
                    {
                        Parser.ForceScript(delegate()
                        {
                            Refactor.ApplyConstraints();
                        });
                    }

                    Transaction.Commit();
                }

                SubscribeEventHandlers();
            }

            executed = true;

#pragma warning restore CS0618 // Type or member is obsolete
        }

        protected abstract void SubscribeEventHandlers();

        internal class UpgradeScript : IComparable<UpgradeScript>
        {
            public UpgradeScript(Action method, long major, long minor, long patch, string name)
            {
                Method = method;
                Major = major;
                Minor = minor;
                Patch = patch;
                Name = name;
            }

            internal UpgradeScript(INode node)
            {
                Major = (long)node.Properties["Major"];
                Minor = (long)node.Properties["Minor"];
                Patch = (long)node.Properties["Patch"];
            }

            public Action Method { get; private set; }
            public long Major { get; private set; }
            public long Minor { get; private set; }
            public long Patch { get; private set; }
            public string Name { get; private set; }

            int IComparable<UpgradeScript>.CompareTo(UpgradeScript other)
            {
                int result = this.Major.CompareTo(other.Major);
                if (result != 0)
                    return result;

                result = this.Minor.CompareTo(other.Minor);
                if (result != 0)
                    return result;

                result = this.Patch.CompareTo(other.Patch);
                if (result != 0)
                    return result;

                return this.Name.CompareTo(other.Name);
            }
        }

        #endregion


        public SchemaInfo GetSchema()
        {
            return SchemaInfo.FromDB(this);
        }

        protected IRefactorGlobal Refactor { get { return this; } }

        void IRefactorGlobal.ApplyFunctionalIds()
        {
            if (!Parser.ShouldExecute)
                return;

            GetSchema().UpdateFunctionalIds();
        }

        void IRefactorGlobal.ApplyConstraints()
        {
            if (!Parser.ShouldExecute)
                return;

            GetSchema().UpdateConstraints();
        }

        void IRefactorGlobal.SetCreationDate()
        {
            if (!Parser.ShouldExecute)
                return;

            string cypher = "MATCH ()-[r]->() WHERE NOT EXISTS(r.CreationDate)  WITH r LIMIT 10000 SET r.CreationDate = ID(r)";
            Parser.ExecuteBatched(cypher, new Dictionary<string, object>(), true);
        }

        void IRefactorGlobal.ApplyFullTextSearchIndexes()
        {
            if (!Parser.ShouldExecute)
                return;

            Parser.Execute("CALL apoc.index.remove('fts')", new Dictionary<string, object>(), true);

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("CALL apoc.index.addAllNodes('fts',");
            builder.AppendLine("\t{");

            bool first = true;
            foreach (var entity in Entities)
            {
                if (entity.FullTextIndexProperties.Count == 0)
                    continue;

                if (first)
                    first = false;
                else
                    builder.AppendLine(",");

                builder.AppendFormat("\t\t{0}:\t\t\t['", entity.Label.Name);
                builder.Append(string.Join("', '", entity.FullTextIndexProperties.Select(item => item.Name)));
                builder.Append("']");
            }

            builder.AppendLine();
            builder.AppendLine("\t}");
            builder.AppendLine(")");

            Parser.Execute(builder.ToString(), new Dictionary<string, object>(), true);
        }


        internal Guid GenerateGuid(string name)
        {
            byte[] b1 = Encoding.UTF8.GetBytes(name);
            byte[] b2 = new byte[b1.Length];
            for (int read = 0, write = b1.Length - 1; write >= 0; read++, write--)
                b2[write] = b1[read];

            Guid hash = GetHash(b1, b2);

            while (knownGuids.Contains(hash))
            {
                for (int index = 0; index < 8; index++)
                {
                    b1[index]++;
                    if (b1[index] != 0)
                        break;
                }
                hash = GetHash(b1, b2);
            }

            knownGuids.Add(hash);
            return hash;
        }

        private static Guid GetHash(byte[] b1, byte[] b2)
        {
            uint i1 = Crc32Algorithm.Compute(b1);
            uint i2 = Crc32CAlgorithm.Compute(b2);
            byte[] i3 = BitConverter.GetBytes(Crc32Algorithm.Compute(b1));
            byte[] i4 = BitConverter.GetBytes(Crc32CAlgorithm.Compute(b2));

            return new Guid(i1, (ushort)i2, (ushort)(i2 >> 16), i3[0], i3[1], i3[2], i3[3], i4[0], i4[1], i4[2], i4[3]);
        }
        private HashSet<Guid> knownGuids = new HashSet<Guid>();
    }

    public abstract class DatastoreModel<TSelf> : DatastoreModel
        where TSelf : DatastoreModel<TSelf>, new()
    {
        private static DatastoreModel model = null;
        public static DatastoreModel Model
        {
            get
            {
                if (model == null)
                {
                    lock (typeof(TSelf))
                    {
                        if (model == null)
                        {
                            model = RegisteredModels.FirstOrDefault(item => item.GetType() == typeof(TSelf));
                            if (model == null)
                            {
                                TSelf m = new TSelf();
                                m.Execute(false);
                                model = m;
                            }
                        }
                    }
                }
                return model;
            }
        }
    }
}
