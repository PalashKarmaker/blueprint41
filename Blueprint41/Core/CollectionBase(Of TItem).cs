﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blueprint41.Core;

public abstract class CollectionBase<TItem> : IEnumerable<TItem>
{
    internal CollectionBase()
    {
        collection = [];
    }

    protected Dictionary<string, TItem> collection;
    internal void Add(string name, TItem value)
    {
        if (collection.ContainsKey(name))
            throw new ArgumentException(string.Format("A {0} with the name '{1}' already exists.", typeof(TItem).Name, name), "name");

        collection.Add(name, value);
    }
    internal void Remove(string name)
    {
        if (!collection.ContainsKey(name))
            throw new ArgumentException(string.Format("A {0} with the name '{1}' did not exist.", typeof(TItem).Name, name), "name");

        collection.Remove(name);
    }

    public TItem this[string name]
    {
        get
        {
            if (!collection.TryGetValue(name, out var value))
                throw new ArgumentOutOfRangeException(string.Format("{0} with name '{1}' was not found.", typeof(TItem).Name, name));

            return value;
        }
    }

    public bool Contains(string name) => collection.ContainsKey(name);

    public int Count { get { return collection.Count; } }

    #region IEnumerable<NodeType>

    IEnumerator IEnumerable.GetEnumerator() => collection.Values.GetEnumerator();
    IEnumerator<TItem> IEnumerable<TItem>.GetEnumerator() => collection.Values.GetEnumerator();

    #endregion
}
