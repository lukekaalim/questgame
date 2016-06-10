using System;
using UnityEngine;

using Serialization;
using Trade;
using System.Collections.Generic;

namespace Test
{
	[ExecuteInEditMode]
	public class InspectorTester : MonoBehaviour
	{
		[SerializeField]
		public SerializableDictionaryStringInt dict;

		[SerializeField]
		SIHasCurrency currency;

		[SerializeField]
		string currencyToPrint;

		void Awake()
		{
			if (dict == null)
			{
				dict = ScriptableObject.CreateInstance<SerializableDictionaryStringInt>();
			}
			if (currency == null)
			{
				currency = ScriptableObject.CreateInstance<SIHasCurrency>();
			}
		}

		void Start()
		{
			if (currency.Value != null)
			{
				Debug.Log(currency.Value.GetContainer()[currencyToPrint]);
			}
		}
	}
}
