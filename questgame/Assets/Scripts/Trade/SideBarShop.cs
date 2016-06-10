using System;
using System.Collections.Generic;
using UnityEngine;
using DisplayBinding;

//A side bar shop is the speciic implementation of a tab on the side that opens uup
//and allows you to buy from a list

namespace Trade
{

	public abstract class SideBarShop<TSellingType, YCurrencyType> : ShopBase<TSellingType, YCurrencyType>
		where TSellingType : MonoBehaviour, IEquatable<TSellingType>
		where YCurrencyType : ICurrencyContainer
	{
		//The amount of items the shop should have at all times.
		[SerializeField]
		protected int totalInventory;

		//The cost of each item
		[SerializeField]
		protected float defaultCostOfItem;

		//The template for the display
		protected abstract ShopDisplay<TSellingType> shopDisplayTemplate
		{
			get;
		}

		[SerializeField]
		protected bool autoUpdateInventory = false;

		//the current displays
		[SerializeField]
		protected List<ShopDisplay<TSellingType>> shopDisplays = new List<ShopDisplay<TSellingType>>();

		protected virtual void Start()
		{
			UpdateInventory();
		}

		protected override void OnPurchase(IInventory<TSellingType> inventory, ICurrencyContainer wallet, ShopItem<TSellingType> purchasedItem, int index)
		{
			RemoveInventoryItemAt(index);

			Destroy(shopDisplays[index].gameObject);
			shopDisplays.RemoveAt(index);

			base.OnPurchase(inventory, wallet, purchasedItem, index);

			if (autoUpdateInventory)
			{
				UpdateInventory();
			}
		}
		//TODO Redo all this code. its rubbish
		public void UpdateInventory()
		{
			/*
			//If we haev less than the amount we should, create some more
			if (GetInventoryCount() < totalInventory)
			{
				int difference = totalInventory - GetInventoryCount();
				for (int i = 0; i < difference; i++)
				{
					//Add them to the shops inventory
					/*T newItem =
					CreateNewItem();
					//VisibleInventory.Add(shopItem);
				}
			}

			//if we have more than the amount we should, delete some.
			if (GetInventoryCount() > totalInventory)
			{
				int difference = GetInventoryCount() - totalInventory;
				int startingIndex = GetInventoryCount() - 1 - difference;

				for (int i = 0; i < GetInventoryCount() - totalInventory; i++)
				{
					RemoveInventoryItemAt(i + startingIndex);
					//VisibleInventory.RemoveAt(i + startingIndex);
					i--;
				}

				for (int i = startingIndex; i < shopDisplays.Count; i++)
				{
					ShopDisplay<T> display = shopDisplays[i];

					if (display != null)
					{
						Destroy(shopDisplays[i].gameObject);
						shopDisplays.RemoveAt(i);
						i--;
					}
				}
			}

			if (shopDisplays.Count > GetInventoryCount())
			{
				int difference = shopDisplays.Count - GetInventoryCount();
				shopDisplays.RemoveRange(shopDisplays.Count - difference - 1, difference);
			}

			//Make sure everything has a display
			for (int i = 0; i < GetInventoryCount(); i++)
			{
				if (i >= shopDisplays.Count || shopDisplays[i] == null)
				{
					//Create the display for them
					ShopDisplay<T> newShopDisplay = Instantiate(shopDisplayTemplate.gameObject).GetComponent<ShopDisplay<T>>();
					newShopDisplay.transform.SetParent(transform, false);
					newShopDisplay.SetOwner(this);

					if (i >= shopDisplays.Count)
					{
						shopDisplays.Add(newShopDisplay);
					}
					else if (shopDisplays[i] == null)
					{
						shopDisplays[i] = newShopDisplay;
					}
				}

				shopDisplays[i].CurrentValue = GetInventoryItemAt(i);
			}
			*/
		}

		protected abstract TSellingType CreateNewItem();

	}
}