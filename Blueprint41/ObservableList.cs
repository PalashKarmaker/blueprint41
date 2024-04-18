using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Blueprint41.Core;

namespace Blueprint41;

public interface IObservableList<T> : INotifyChanged<CollectionItem<T>?>, IEnumerable<CollectionItem<T>>
    where T : class, OGM
{
    CollectionItem<T>? this[int index] { get; set; }
    void Add(CollectionItem<T> item);
    void Clear();
    bool Contains(T item);
    int Count { get; }
    int TotalCount { get; }
    void CopyTo(CollectionItem<T>[] array, int arrayIndex);
    int[] IndexOf(T item);
    bool Remove(T item);
    void RemoveAt(int index);
}


public sealed class ObservableList<T> : IObservableList<T>
    where T : class, OGM
{
    static private readonly int[] EMPTY_INT_ARRAY = [];
    private List<CollectionItem<T>?> InnerList;
    private Dictionary<T, List<int>> InnerDict;

    public event NotifyChangedEventHandler<CollectionItem<T>?>? BeforeCollectionChanged;
    public event NotifyChangedEventHandler<CollectionItem<T>?>? CollectionChanged;

    public ObservableList()
    {
        Count = 0;
        InnerList = [];
        InnerDict = [];
    }
    public ObservableList(IEnumerable<CollectionItem<T>> items)
    {
        InnerList = new List<CollectionItem<T>?>(items.Where(item => item is not null && item.Item is not null));
        Count = InnerList.Count;

        int index = 0;
        InnerDict = InnerList
            .Select(item => (Item:item, Index:index++))
            .Where(item => item.Item?.Item is not null)
            .GroupBy(item => item.Item!.Item)
            .ToDictionary(item => item.Key, item => item.Select(item2 => item2.Index).ToList());
    }

    public CollectionItem<T>? this[int index]
    {
        get
        {
            return InnerList[index];
        }
        set
        {
            CollectionItem<T>? oldValue = InnerList[index];
            CollectionItem<T>? newValue = value;

            if (oldValue is null && newValue is null)
                return;

            if (oldValue is null || newValue is null || !oldValue.Equals(newValue))
            {
                if (!Contains(newValue!))
                {
                    NotifyChangedEventArgs<CollectionItem<T>?> eventArgs = new(NotifyCollectionChangedAction.Replace, newValue, oldValue, index);
                    BeforeCollectionChanged?.Invoke(this, eventArgs);

                    if (oldValue is not null)
                        RemoveAt(oldValue, index);

                    InnerList[index] = newValue;
                    if (newValue is not null)
                        AddIndex(newValue.Item, index);

                    CollectionChanged?.Invoke(this, eventArgs);
                }
            }
        }
    }

    public int TotalCount 
    { 
        get 
        {
            return InnerList.Count;
        }
    }
    public int Count { get; private set; }

    private void AddIndex(T item, int index)
    {
        if (!InnerDict.TryGetValue(item, out var indexes))
        {
            indexes = [];
            InnerDict.Add(item, indexes);
        }
        indexes.Add(index);
    }
    public void Add(CollectionItem<T> item)
    {
        if (!Contains(item)) // if Item is already in the list, don't add it again
        {
            NotifyChangedEventArgs<CollectionItem<T>?> eventArgs = new(NotifyCollectionChangedAction.Add, item);
            BeforeCollectionChanged?.Invoke(this, eventArgs);
            AddIndex(item.Item, InnerList.Count);
            InnerList.Add(item);
            Count++;
            CollectionChanged?.Invoke(this, eventArgs);
        }
    }
    public void Clear()
    {
        NotifyChangedEventArgs<CollectionItem<T>?> eventArgs = new(NotifyCollectionChangedAction.Reset);
        BeforeCollectionChanged?.Invoke(this, eventArgs);
        InnerList.Clear();
        InnerDict.Clear();
        Count = 0;
        CollectionChanged?.Invoke(this, eventArgs);
    }
    public bool Contains(T item)
    {
        if (InnerDict.TryGetValue(item, out var indexes))
            foreach (int index in indexes)
            {
                CollectionItem<T>? value = InnerList[index];
                if (value is not null && value.Equals(item))
                    return true;
            }
        return false;
    }
    public bool Contains(CollectionItem<T> item)
    {
        if (InnerDict.TryGetValue(item.Item, out var indexes))
        {
            foreach (int index in indexes)
            {
                CollectionItem<T>? value = InnerList[index];
                if (value is not null && value.Equals(item))
                    return true;
            }
        }
        return false;
    }
    public void CopyTo(CollectionItem<T>[] array, int arrayIndex) => InnerList.CopyTo(array, arrayIndex);
    public int[] IndexOf(T item)
    {
        if (InnerDict.TryGetValue(item, out var indexes))
            return [.. indexes];

        return EMPTY_INT_ARRAY;
    }

    public bool Remove(T item)
    {
        if (InnerDict.TryGetValue(item, out var indexes))
        {
            bool result = false;
            for (int ptr = indexes.Count - 1; ptr >= 0; ptr--)
            {
                int index = indexes![ptr];
                CollectionItem<T>? value = InnerList[index];
                if (value is not null)
                {
                    result = true;
                    RemoveAt(value, index, indexes!, ptr);
                }
            }
            return result;
        }
        return false;
    }
    public void RemoveAt(int index)
    {
        CollectionItem<T>? item = InnerList[index];
        if (item is not null)
            RemoveAt(item, index);
    }
    private void RemoveAt(CollectionItem<T> item, int index, List<int>? indexes = null, int ptr = 0)
    {
        if (item is null)
            throw new ArgumentNullException(nameof(item));

        NotifyChangedEventArgs<CollectionItem<T>?> eventArgs = new(NotifyCollectionChangedAction.Remove, item, index);
        BeforeCollectionChanged?.Invoke(this, eventArgs);

        InnerList[index] = null;
        Count--;
        if (indexes is null)
        {
            if (InnerDict.TryGetValue(item.Item, out indexes))
                ptr = indexes.IndexOf(index);
        }
        if (indexes is not null)
        {
            indexes.RemoveAt(ptr);
            if (indexes.Count == 0)
                InnerDict.Remove(item.Item);
            else
                InnerDict[item.Item] = indexes;
        }
        CollectionChanged?.Invoke(this, eventArgs);
    }

    public IEnumerator<CollectionItem<T>> GetEnumerator() => InnerList.Where(item => item is not null).GetEnumerator()!;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
