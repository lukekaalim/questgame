using System.Collections.Generic;

namespace Trade
{
	/*
	 *	The ICurrencyContainer interface is a representation of a collection of
	 *	currencies, identified by their strings. Using this interface,
	 *	you can check wether you can afford someting, whether you possess
	 *	the right currency and that kind of stuff.
	 */

	public interface ICurrencyContainer : IHasCurrency
	{
		bool Modify(ICurrencyContainer currenciesToModify);
		bool Modify(string currency, int amount);
		bool Modify(int amount);

		bool CanAfford(ICurrencyContainer currenciesToCheck);
		bool CanAfford(int amount);

		int GetCurrency(string moneyType);
		int GetCurrency();

		List<string> CurrenciesOwned
		{
			get;
		}

		int this[string moneyType]
		{
			get;
		}
	}
}