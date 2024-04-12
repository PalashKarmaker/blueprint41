using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Blueprint41.Query;

namespace Blueprint41.Core;

public class UnidentifiedPropertiesAliasDictionary : IReadOnlyDictionary<string, FieldResult>
{
    public UnidentifiedPropertiesAliasDictionary(Dictionary<string, FieldResult> items, Entity entity, AliasResult alias)
    {
        Items = items;
        Entity = entity;
        Alias = alias;
    }

    private IDictionary<string, FieldResult> Items;
    private Entity Entity;
    private AliasResult Alias;

    public FieldResult this[string key]
    {
        get
        {
            if (!Items.TryGetValue(key, out FieldResult? value))
                value = new MiscResult(Alias, key, Entity, null, null);

            return value;
        }
    }

    public IEnumerable<string> Keys { get { return Items.Keys; } }

    public IEnumerable<FieldResult> Values { get { return Items.Values; } }

    public int Count { get { return Items.Count; } }

    public bool ContainsKey(string key) => Items.ContainsKey(key);

    public IEnumerator<KeyValuePair<string, FieldResult>> GetEnumerator() => Items.GetEnumerator();

    public bool TryGetValue(string key, out FieldResult value)
    {
        value = this[key];
        return true;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}