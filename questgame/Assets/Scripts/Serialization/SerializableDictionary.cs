using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

namespace Serialization
{
	//Empty base class so it can be rendered by the unity editor
	[Serializable]
	public abstract class SerializableDictionary : ScriptableObject
	{
		public abstract Type KeyType
		{
			get;
		}
		public abstract Type ValueType
		{
			get;
		}

		public abstract int Count
		{
			get;
		}
	}

	/*
	 *	Represents a dictionary that can be both seralized and displayed in an editor by unity.
	 *	Can be cast back an forth from the Dictionary class
	 */
	[Serializable]
	public class SerializableDictionary<TKey, TValue> : SerializableDictionary, IDictionary<TKey, TValue>, ISerializationCallbackReceiver
	{
		//Inhertiance stuff
		public override Type KeyType
		{
			get
			{
				return typeof(TKey);
			}
		}
		public override Type ValueType
		{
			get
			{
				return typeof(TValue);
			}
		}
		//Serialization Stuff

		[SerializeField]
		protected Dictionary<TKey, TValue> _internalDictionary = new Dictionary<TKey, TValue>();

		[SerializeField]
		public List<TKey> _tempKeys = new List<TKey>();
		[SerializeField]
		public List<TValue> _tempValues = new List<TValue>();

		public void OnBeforeSerialize()
		{
			_tempKeys.Clear();
			_tempValues.Clear();
			foreach (var kvp in _internalDictionary)
			{
				_tempKeys.Add(kvp.Key);
				_tempValues.Add(kvp.Value);
			}
		}

		public void OnAfterDeserialize()
		{
			_internalDictionary.Clear();
			for (int i = 0; i != Math.Min(_tempKeys.Count, _tempValues.Count); i++)
			{
				_internalDictionary.Add(_tempKeys[i], _tempValues[i]);
			}
		}

		//Casting stuff

		public static implicit operator Dictionary<TKey, TValue>(SerializableDictionary<TKey, TValue> dictionary)
		{
			return dictionary._internalDictionary;
		}

		public static implicit operator SerializableDictionary<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
		{
			SerializableDictionary<TKey, TValue> serializableDictionary = CreateInstance<SerializableDictionary<TKey, TValue>>();
			serializableDictionary._internalDictionary = dictionary;
			return serializableDictionary;
		}

		//Dictionary Stuff

		public TValue this[TKey key] { get { return _internalDictionary[key]; } set { _internalDictionary[key] = value; } }

		public override int Count { get { return _internalDictionary.Count; } }

		public bool IsReadOnly { get { return false; } }

		public ICollection<TKey> Keys { get { return _internalDictionary.Keys; } }

		public ICollection<TValue> Values { get { return _internalDictionary.Values; } }

		//Dictionary methods

		public void Add(KeyValuePair<TKey, TValue> item)
		{
			_internalDictionary.Add(item.Key, item.Value);
		}

		public void Add(TKey key, TValue value)
		{
			_internalDictionary.Add(key, value);
		}

		public void Clear()
		{
			_internalDictionary.Clear();
		}

		public bool Contains(KeyValuePair<TKey, TValue> item)
		{
			TValue tryItem;
			return _internalDictionary.TryGetValue(item.Key, out tryItem) && tryItem.Equals(item.Value);
		}

		public bool ContainsKey(TKey key)
		{
			return _internalDictionary.ContainsKey(key);
		}

		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			int index = 0;
			foreach (TKey key in _internalDictionary.Keys)
			{
				TValue value = _internalDictionary[key];
				array[arrayIndex + index] = new KeyValuePair<TKey, TValue>(key, value);
				index++;
			}
		}

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return _internalDictionary.GetEnumerator();
		}

		public bool Remove(KeyValuePair<TKey, TValue> item)
		{
			TValue tryItem;
			bool contains = _internalDictionary.TryGetValue(item.Key, out tryItem) && tryItem.Equals(item.Value);
			if (contains)
			{
				return _internalDictionary.Remove(item.Key);
			}
			return false;
		}

		public bool Remove(TKey key)
		{
			return _internalDictionary.Remove(key);
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			return _internalDictionary.TryGetValue(key, out value);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _internalDictionary.GetEnumerator();
		}
	}
}
