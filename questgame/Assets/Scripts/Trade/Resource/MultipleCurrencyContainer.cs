using System.Collections.Generic;
using UnityEngine;

using System;
using Serialization;

namespace Trade
{
	[Serializable, ExecuteInEditMode]
	public class MultipleCurrencyContainer : ScriptableObject, ICurrencyContainer
	{
		[SerializeField]
		SerializableDictionaryStringInt dictionary;

		[SerializeField]
		public int Prime = 5;

		[SerializeField]
		protected string defaultCurrency = "";

		void Awake()
		{
			if (dictionary == null)
			{
				dictionary = CreateInstance<SerializableDictionaryStringInt>();
			}
		}

		public string DefaultCurrency
		{
			get
			{
				if (!dictionary.ContainsKey(defaultCurrency))
				{
					if (dictionary.Keys.Count > 0)
					{
						defaultCurrency = dictionary.Keys.GetEnumerator().Current;
					}
					else
					{
						defaultCurrency = null;
					}
				}
				return defaultCurrency;
			}
			set
			{
				if (dictionary.ContainsKey(value))
				{
					defaultCurrency = value;
				}
			}
		}

		public int this[string moneyType]
		{
			get
			{
				int currency;
				if (dictionary.TryGetValue(moneyType, out currency))
				{
					return currency;
				}
				else
				{
					return 0;
				}
			}
			set
			{
				dictionary[moneyType] = value;
			}
		}

		public List<string> CurrenciesOwned
		{
			get
			{
				List<string> list = new List<string>(dictionary.Keys);
				return list;
			}
		}

		public bool CanAfford(int amount)
		{
			return amount <= this[defaultCurrency];
		}

		public bool CanAfford(ICurrencyContainer currenciesToCheck)
		{
			bool hasEnough = true;
			foreach (string currency in currenciesToCheck.CurrenciesOwned)
			{
				if (this[currency] < currenciesToCheck[currency])
				{
					hasEnough = false;
				}
			}
			return hasEnough;
		}

		public bool CanAfford(string currency, int amount)
		{
			return this[currency] >= amount;
		}

		public int GetCurrency(string moneyType)
		{
			return this[moneyType];
		}

		public int GetCurrency()
		{
			return GetCurrency(defaultCurrency);
		}

		public bool Modify(int amount)
		{
			return Modify(defaultCurrency, amount);
		}

		public bool Modify(ICurrencyContainer currenciesToModify)
		{
			foreach (string currency in currenciesToModify.CurrenciesOwned)
			{
				this[currency] += currenciesToModify[currency];
			}
			return true;
		}

		public bool Modify(string currency, int amount)
		{
			this[currency] += amount;
			return true;
		}

		ICurrencyContainer IHasCurrency.GetContainer()
		{
			return this;
		}
	}
}