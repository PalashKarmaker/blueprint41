﻿using System.Collections.Generic;

namespace Blueprint41.Core;

internal class AddRelationshipAction : RelationshipAction
{
    internal AddRelationshipAction(RelationshipPersistenceProvider persistenceProvider, Relationship relationship, OGM inItem, OGM outItem, Dictionary<string, object>? properties)
        : base(persistenceProvider, relationship, inItem, outItem) => Properties = properties;

    public Dictionary<string, object>? Properties { get; private set; }

    protected override void InDatastoreLogic(Relationship relationship) => PersistenceProvider.Add(relationship, InItem!, OutItem!, null, false, Properties, true);

    protected override void InMemoryLogic(EntityCollectionBase target)
    {
        bool contains = target.IndexOf(target.ForeignItem(this)!).Length != 0;
        if (!contains)
            target.Add(target.NewCollectionItem(target.Parent, target.ForeignItem(this)!, null, null));
    }
}
