﻿using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;

namespace Blueprint41;

public sealed class ObservableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, INotifyChanged<KeyValuePair<TKey, TValue>>
    where TKey : notnull
{
    private Dictionary<TKey, TValue> InnerDictionary;

    public event NotifyChangedEventHandler<KeyValuePair<TKey, TValue>>? BeforeCollectionChanged;

    public event NotifyChangedEventHandler<KeyValuePair<TKey, TValue>>? CollectionChanged;

    public ObservableDictionary() => InnerDictionary = [];

    public ObservableDictionary(IDictionary<TKey, TValue> items) => InnerDictionary = new Dictionary<TKey, TValue>(items);

    public TValue this[TKey key]
    {
        get
        {
            return InnerDictionary[key];
        }
        set
        {
            NotifyChangedEventArgs<KeyValuePair<TKey, TValue>> eventArgs = new(NotifyCollectionChangedAction.Replace, new KeyValuePair<TKey, TValue>(key, value), new KeyValuePair<TKey, TValue>(key, InnerDictionary[key]));
            BeforeCollectionChanged?.Invoke(this, eventArgs);
            InnerDictionary[key] = value;
            CollectionChanged?.Invoke(this, eventArgs);
        }
    }

    public ICollection<TKey> Keys => InnerDictionary.Keys;
    public ICollection<TValue> Values => InnerDictionary.Values;
    public int Count => InnerDictionary.Count;
    public bool IsReadOnly => false;

    public void Add(TKey key, TValue value)
    {
        NotifyChangedEventArgs<KeyValuePair<TKey, TValue>> eventArgs = new(NotifyCollectionChangedAction.Add, new KeyValuePair<TKey, TValue>(key, value));
        BeforeCollectionChanged?.Invoke(this, eventArgs);
        InnerDictionary.Add(key, value);
        CollectionChanged?.Invoke(this, eventArgs);
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        NotifyChangedEventArgs<KeyValuePair<TKey, TValue>> eventArgs = new(NotifyCollectionChangedAction.Add, item);
        BeforeCollectionChanged?.Invoke(this, eventArgs);
        ((IDictionary<TKey, TValue>)InnerDictionary).Add(item);
        CollectionChanged?.Invoke(this, eventArgs);
    }

    public void Clear()
    {
        NotifyChangedEventArgs<KeyValuePair<TKey, TValue>> eventArgs = new(NotifyCollectionChangedAction.Reset);
        BeforeCollectionChanged?.Invoke(this, eventArgs);
        InnerDictionary.Clear();
        CollectionChanged?.Invoke(this, eventArgs);
    }

    public bool Contains(KeyValuePair<TKey, TValue> item) => InnerDictionary.Contains(item);

    public bool ContainsKey(TKey key) => InnerDictionary.ContainsKey(key);

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => ((IDictionary<TKey, TValue>)InnerDictionary).CopyTo(array, arrayIndex);

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => InnerDictionary.GetEnumerator();

    public bool Remove(TKey key)
    {
        NotifyChangedEventArgs<KeyValuePair<TKey, TValue>> eventArgs = new(NotifyCollectionChangedAction.Remove, new KeyValuePair<TKey, TValue>(key, InnerDictionary[key]));
        BeforeCollectionChanged?.Invoke(this, eventArgs);
        bool result = InnerDictionary.Remove(key);
        CollectionChanged?.Invoke(this, eventArgs);
        return result;
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        NotifyChangedEventArgs<KeyValuePair<TKey, TValue>> eventArgs = new(NotifyCollectionChangedAction.Remove, item);
        BeforeCollectionChanged?.Invoke(this, eventArgs);
        bool result = ((IDictionary<TKey, TValue>)InnerDictionary).Remove(item);
        CollectionChanged?.Invoke(this, eventArgs);
        return result;
    }

    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value) => InnerDictionary.TryGetValue(key, out value);

    IEnumerator IEnumerable.GetEnumerator() => InnerDictionary.GetEnumerator();
}