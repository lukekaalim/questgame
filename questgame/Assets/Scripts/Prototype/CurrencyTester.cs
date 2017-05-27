using System;
using Trade;
using UnityEngine;

namespace Test
{
	[ExecuteInEditMode]
	public class CurrencyTester : MonoBehaviour, Trade.IHasCurrency
	{
		public Trade.SingleCurrencyContainer coin = null;

		public Trade.MultipleCurrencyContainer wallet = null;

		public ICurrencyContainer GetContainer()
		{
			return wallet;
		}
	}
}
