using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Serialization
{
	public abstract class SerializableInterface<T> : ScriptableObject, ISerializationCallbackReceiver where T : class
	{
		T _value;
		public T Value
		{
			get
			{
				return _value;
			}
			set
			{
				_value = value;
			}
		}

		[SerializeField]
		UnityEngine.Object unityObject;

		public static implicit operator T(SerializableInterface<T> instance)
		{
			return instance.Value;
		}

		public void OnAfterDeserialize()
		{
			if (unityObject != null)
			{
				_value = unityObject as T;
				return;
			}
		}

		public void OnBeforeSerialize()
		{
			unityObject = _value as UnityEngine.Object;
		}
	}
}
