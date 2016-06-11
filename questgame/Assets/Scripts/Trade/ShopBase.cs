using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Serialization;

using UnityEngine;

/*
 *
 *		The Base class for trading
 *		Takes in two types, the type it will be selling, and what it will consider currency
 *		The specific implementation of a shop is left to the child class, but this should provide
 *		all the shared code
 *
 */

namespace Trade
{
	public abstract class ShopBase : MonoBehaviour
	{
		protected abstract int GetInventoryCount();
	}

	//this shop sells T to the Game
	public abstract class ShopBase<TSellingType, YCurrencyType> : ShopBase
		where TSellingType : IEquatable<TSellingType>
		where YCurrencyType : ICurrencyContainer
	{

		public enum PurchaseResults
		{
			OutOfStock,
			NotEnoughMoney,
			NullInventory,
			InventoryRejected,
			Success
		}

		public virtual PurchaseResults TryBuy(ICurrencyContainer wallet, TSellingType ObjectToBuy, IInventory<TSellingType> inventory = null)
		{
			return TryBuy(wallet, GetIndexOf(ObjectToBuy), inventory);
		}

		public virtual PurchaseResults TryBuy(ICurrencyContainer wallet, int itemIndex, IInventory<TSellingType> inventory = null)
		{
			ICurrencyContainer cost;
			bool inStock = false;

			ShopItem<TSellingType> shopItem = GetInventoryItemAt(itemIndex);

			inStock = (shopItem != null);

			if (!inStock)
			{
				return PurchaseResults.OutOfStock;
			}

			cost = shopItem.Cost;

			if (!wallet.CanAfford(cost))
			{
				return PurchaseResults.NotEnoughMoney;
			}

			if (inventory == null)
			{
				return PurchaseResults.NullInventory;
			}

			if (!inventory.TryAddItem(shopItem.Item))
			{
				return PurchaseResults.InventoryRejected;
			}

			OnPurchase(inventory, wallet, shopItem, itemIndex);

			return PurchaseResults.Success;
		}

		protected virtual void OnPurchase(IInventory<TSellingType> inventory, ICurrencyContainer wallet, ShopItem<TSellingType> purchasedItem, int index)
		{
			wallet.Modify(purchasedItem.Cost);
		}

		protected virtual ShopItem GetItem(TSellingType item)
		{
			for (int i = 0; i < GetInventoryCount(); i++)
			{
				if (GetInventoryItemAt(i).Item.Equals(item))
				{
					return GetInventoryItemAt(i);
				}
			}
			return null;
		}

		protected virtual ShopItem GetItem(TSellingType item, out int index)
		{
			for (int i = 0; i < GetInventoryCount(); i++)
			{
				if (GetInventoryItemAt(i).Item.Equals(item))
				{
					index = i;
					return GetInventoryItemAt(i);
				}
			}
			index = -1;
			return null;
		}

		protected abstract int GetIndexOf(TSellingType item);
		protected abstract ShopItem<TSellingType> GetInventoryItemAt(int index);
		protected abstract void RemoveInventoryItemAt(int index);

	}

	[Serializable]
	public abstract class ShopItem : IEquatable<ShopItem>
	{
		[SerializeField]
		protected ICurrencyContainer _cost;
		public ICurrencyContainer Cost { get { return _cost; } }

		public abstract bool Equals(ShopItem other);

		public ShopItem(ICurrencyContainer newCost)
		{
			_cost = newCost;
		}
	}

	public abstract class ShopItem<T> : ShopItem, IEquatable<ShopItem<T>>
	{
		public ShopItem(ICurrencyContainer newCost) : base(newCost) { }

		public abstract T Item
		{
			get;
			set;
		}

		public bool Equals(ShopItem<T> other)
		{
			return Item.Equals(other);
		}

		public override bool Equals(ShopItem other)
		{
			return Item.Equals(other);
		}
	}
}