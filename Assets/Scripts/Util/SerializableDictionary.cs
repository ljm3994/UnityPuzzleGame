using System.Collections;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

// 인스펙터용 딕셔너리 베이스 클래스
public abstract class SerializableDictionaryBase
{
    public abstract class Storage { }

    protected class SDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        public SDictionary() { }
        public SDictionary(IDictionary<TKey, TValue> keyValuePairs) : base(keyValuePairs) { }
        public SDictionary(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    
}

[Serializable]
public abstract class SerializableDictionaryBase<TKey, TValue, TValueStorage> : SerializableDictionaryBase, IDictionary<TKey, TValue>, IDictionary, ISerializationCallbackReceiver, IDeserializationCallback, ISerializable
{
    Dictionary<TKey, TValue> keyValuePairs;
    [SerializeField]
    TKey[] keys;
    [SerializeField]
    TValueStorage[] values;

    public SerializableDictionaryBase()
    {
        keyValuePairs = new Dictionary<TKey, TValue>();
    }
    public SerializableDictionaryBase(IDictionary<TKey, TValue> keyValues)
    {
        keyValuePairs = new Dictionary<TKey, TValue>(keyValues);
    }

    protected abstract void SetValue(TValueStorage[] valueStorages, int Num, TValue value);
    protected abstract TValue GetValue(TValueStorage[] valueStorages, int Num);

    public void DicCopy(IDictionary<TKey, TValue> keyValues)
    {
        keyValuePairs.Clear();
        foreach (var item in keyValues)
        {
            keyValuePairs[item.Key] = item.Value;
        }
    }

    # region IDictionary<TKey, TValue> 구현
    public TValue this[TKey key]
    {
        get
        {
            return ((IDictionary<TKey, TValue>)keyValuePairs)[key];
        }
        set
        {
            ((IDictionary<TKey, TValue>)keyValuePairs)[key] = value;
        }
    }

    public ICollection<TKey> Keys
    {
        get
        {
            return ((IDictionary<TKey, TValue>)keyValuePairs).Keys;
        }
    }

    public ICollection<TValue> Values
    {
        get
        {
            return ((IDictionary<TKey, TValue>)keyValuePairs).Values;
        }
    }

    public int Count
    {
        get
        {
            return ((IDictionary<TKey, TValue>)keyValuePairs).Count;
        }
    }

    public bool IsReadOnly
    {
        get
        {
            return ((IDictionary<TKey, TValue>)keyValuePairs).IsReadOnly;
        }
    }

    public void Add(TKey key, TValue value)
    {
        ((IDictionary<TKey, TValue>)keyValuePairs).Add(key, value);
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        ((IDictionary<TKey, TValue>)keyValuePairs).Add(item);
    }

    public void Clear()
    {
        ((IDictionary<TKey, TValue>)keyValuePairs).Clear();
    }

    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
        return ((IDictionary<TKey, TValue>)keyValuePairs).Contains(item);
    }

    public bool ContainsKey(TKey key)
    {
        return ((IDictionary<TKey, TValue>)keyValuePairs).ContainsKey(key);
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        ((IDictionary<TKey, TValue>)keyValuePairs).CopyTo(array, arrayIndex);
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        return ((IDictionary<TKey, TValue>)keyValuePairs).GetEnumerator();
    }

    public bool Remove(TKey key)
    {
        return ((IDictionary<TKey, TValue>)keyValuePairs).Remove(key);
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        return ((IDictionary<TKey, TValue>)keyValuePairs).Remove(item);
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        return ((IDictionary<TKey, TValue>)keyValuePairs).TryGetValue(key, out value);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IDictionary<TKey, TValue>)keyValuePairs).GetEnumerator();
    }
    #endregion

    #region IDictionary 구현
    public bool IsFixedSize
    {
        get
        {
            return ((IDictionary)keyValuePairs).IsFixedSize;
        }
    }

    ICollection IDictionary.Keys
    {
        get
        {
            return ((IDictionary)keyValuePairs).Keys;
        }
    }

    ICollection IDictionary.Values
    {
        get
        {
            return ((IDictionary)keyValuePairs).Values;
        }
    }

    public bool IsSynchronized
    {
        get
        {
            return ((IDictionary)keyValuePairs).IsSynchronized;
        }
    }

    public object SyncRoot
    {
        get
        {
            return ((IDictionary)keyValuePairs).SyncRoot;
        }
    }

    public object this[object key]
    {
        get
        {
            return ((IDictionary)keyValuePairs)[key];
        }
        set
        {
            ((IDictionary)keyValuePairs)[key] = value;
        }
    }
    public void Add(object key, object value)
    {
        ((IDictionary)keyValuePairs).Add(key, value);
    }

    public bool Contains(object key)
    {
        return ((IDictionary)keyValuePairs).Contains(key);
    }

    IDictionaryEnumerator IDictionary.GetEnumerator()
    {
        return ((IDictionary)keyValuePairs).GetEnumerator();
    }

    public void Remove(object key)
    {
        ((IDictionary)keyValuePairs).Remove(key);
    }

    public void CopyTo(Array array, int index)
    {
        ((IDictionary)keyValuePairs).CopyTo(array, index);
    }
    #endregion

    #region ISerializationCallbackReceiver 구현
    public void OnBeforeSerialize()
    {
        int num = keyValuePairs.Count;
        keys = new TKey[num];
        values = new TValueStorage[num];

        int i = 0;
        foreach (var item in keyValuePairs)
        {
            keys[i] = item.Key;
            SetValue(values, i, item.Value);
            i++;
        }
    }

    public void OnAfterDeserialize()
    {
        if (keys != null && values != null && keys.Length == values.Length)
        {
            keyValuePairs.Clear();
            int num = keys.Length;
            for (int i = 0; i < num; i++)
            {
                keyValuePairs[keys[i]] = GetValue(values, i);
            }

            keys = null;
            values = null;
        }
    }
    #endregion

    #region IDeserializationCallback 구현
    public void OnDeserialization(object sender)
    {
        ((IDeserializationCallback)keyValuePairs).OnDeserialization(sender);
    }

    #endregion

    #region ISerializable 구현
    protected SerializableDictionaryBase(SerializationInfo info, StreamingContext context)
    {
        keyValuePairs = new SDictionary<TKey, TValue>(info, context);
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
         ((ISerializable)keyValuePairs).GetObjectData(info, context);
    }
    #endregion
}

public static class SerializableDictionary
{
    public class Storage<T> : SerializableDictionaryBase.Storage
    {
        public T data;
    }
}

public class SerializableDictionary<TKey, TValue> : SerializableDictionaryBase<TKey, TValue, TValue>
{
    public SerializableDictionary() { }
    public SerializableDictionary(IDictionary<TKey, TValue> keyValuePairs) : base(keyValuePairs) { }
    public SerializableDictionary(SerializationInfo info, StreamingContext context) : base(info, context) { }
    protected override TValue GetValue(TValue[] valueStorages, int Num)
    {
        return valueStorages[Num];
    }

    protected override void SetValue(TValue[] valueStorages, int Num, TValue value)
    {
        valueStorages[Num] = value;
    }
}

public class SerializableDictionary<TKey, TValue, TValueStorage> : SerializableDictionaryBase<TKey, TValue, TValueStorage> where TValueStorage : SerializableDictionary.Storage<TValue>, new()
{
    public SerializableDictionary() { }
    public SerializableDictionary(IDictionary<TKey, TValue> keyValuePairs) : base(keyValuePairs) { }
    public SerializableDictionary(SerializationInfo info, StreamingContext context) : base(info, context) { }
    protected override TValue GetValue(TValueStorage[] valueStorages, int Num)
    {
        return valueStorages[Num].data;
    }

    protected override void SetValue(TValueStorage[] valueStorages, int Num, TValue value)
    {
        valueStorages[Num] = new TValueStorage();
        valueStorages[Num].data = value;
    }
}
