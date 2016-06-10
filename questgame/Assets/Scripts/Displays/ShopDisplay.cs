using System;
using UnityEngine;
using UnityEngine.UI;
using Trade;

using DisplayBinding;

namespace DisplayBinding
{
	//T is the specific item.
	// We implement displaybase as a shopitem with a type of T
	public abstract class ShopDisplay<TSellingType> : DisplayBase<ShopItem<TSellingType>>
		where TSellingType : IEquatable<TSellingType>
	{
		[SerializeField]
		StringDisplay nameDisplay;

		[SerializeField]
		protected ShopBase<TSellingType, ICurrencyContainer> _owner;

		[SerializeField]
		Button button;

		[SerializeField]
		IntDisplay costCounter;

		[SerializeField]
		float cost;

		void Start()
		{
			if (button == null)
			{
				button = GetComponent<Button>();

				if (button == null)
				{
					button = GetComponentInChildren<Button>();
				}
			}

			button.onClick.AddListener(AttemptPurchase);
		}

		public abstract void AttemptPurchase();

		public void SetOwner(ShopBase<TSellingType, ICurrencyContainer> owner)
		{
			_owner = owner;
		}

		override protected void OnValueChange(ShopItem<TSellingType> oldValue, ShopItem<TSellingType> newValue)
		{
			UpdateValues();
		}

		public abstract void UpdateValues();
	}
}
