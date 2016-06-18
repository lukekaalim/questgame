using System;
using UnityEngine;

namespace Utility
{
	public static class MonobehaviourExtensions
	{
		public static T AddOrGetComponent<T>(this GameObject value) where T : MonoBehaviour
		{
			if (value == null)
				return null;

			T component = value.GetComponent<T>();
			if (component == null)
			{
				component = value.AddComponent<T>();
			}

			return component;
		}

		public static T AddOrGetComponent<T>(this MonoBehaviour value) where T : MonoBehaviour
		{
			if (value == null)
				return null;

			T component = value.GetComponent<T>();
			if (component == null)
			{
				component = value.gameObject.AddComponent<T>();
			}

			return component;
		}
	}
}
