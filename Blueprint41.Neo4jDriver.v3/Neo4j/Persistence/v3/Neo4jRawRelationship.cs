﻿using System.Collections.Generic;

using Neo4j.Driver;
using Blueprint41.Core;

namespace Blueprint41.Neo4j.Persistence.Driver.v3
{
    internal class Neo4jRawRelationship : RawRelationship
    {
        internal Neo4jRawRelationship(IRelationship relationship)
        {
            Relationship = relationship;
        }
        private IRelationship Relationship;

        public override long Id => Relationship.Id;
        public override string Type => Relationship.Type;
        public override long StartNodeId => Relationship.StartNodeId;
        public override long EndNodeId => Relationship.EndNodeId;
        public override IReadOnlyDictionary<string, object> Properties => Relationship.Properties;
        public override object this[string key] => Relationship[key];
        public override bool Equals(RawRelationship other) => other is Neo4jRawRelationship rawOther ? Relationship.Equals(rawOther.Relationship) : false;
    }
}
