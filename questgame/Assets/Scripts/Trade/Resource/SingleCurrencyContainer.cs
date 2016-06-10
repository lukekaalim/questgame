using System.Collections.Generic;
using UnityEngine;
namespace Trade
{
	public class SingleCurrencyContainer : ScriptableObject, ICurrencyContainer
	{
		[SerializeField]
		string currencyName;
		[SerializeField]
		int currencyAmount;

		[SerializeField]
		int minCap;
		[SerializeField]
		int maxCap;

		public string CurrencyName
		{
			get
			{
				return currencyName;
			}
			set
			{
				currencyName = value;
			}
		}

		public int this[string moneyType]
		{
			get
			{
				return GetCurrency(moneyType);
			}
		}

		public int MaxCap { get { return maxCap; } }

		public int MinCap { get { return minCap; } }

		public List<string> CurrenciesOwned
		{
			get
			{
				List<string> list = new List<string>();
				list.Add(currencyName);
				return list;
			}
		}

		public bool CanAfford(int amount)
		{
			return amount <= currencyAmount;
		}

		public bool CanAfford(ICurrencyContainer currenciesToCheck)
		{
			return currenciesToCheck[currencyName] >= currencyAmount;
		}

		public bool CanAfford(string currency, int amount)
		{
			return currency == currencyName && CanAfford(amount);
		}

		public int GetCurrency(string moneyType)
		{
			return moneyType == currencyName ? currencyAmount : 0;
		}

		public int GetCurrency()
		{
			return currencyAmount;
		}

		public bool Modify(int amount)
		{
			int newAmount = currencyAmount + amount;

			if (newAmount > minCap && newAmount < maxCap)
			{
				currencyAmount = newAmount;
				return true;
			}
			return false;
		}

		public bool Modify(ICurrencyContainer currenciesToModify)
		{
			return Modify(currenciesToModify[currencyName]);
		}


		public bool Modify(string currency, int amount)
		{
			return currency == currencyName ? Modify(amount) : false;
		}

		public static SingleCurrencyContainer CreateInstance(string currencyName, int startingAmount, int min = 0, int max = int.MaxValue)
		{
			SingleCurrencyContainer container = CreateInstance<SingleCurrencyContainer>();
			container.currencyName = currencyName;
			container.currencyAmount = startingAmount;
			container.minCap = min;
			container.maxCap = max;
			return container;
		}

		public void SetCurrency(int newValue)
		{
			currencyAmount = newValue;
		}

		public void SetMinMax(int min, int max)
		{
			minCap = min;
			maxCap = max;
		}

		ICurrencyContainer IHasCurrency.GetContainer()
		{
			return this;
		}
	}
}