using UnityEngine;
using UnityEditor;
/*
[CustomEditor(typeof(ShopBase), true)]
public class ShopInspector : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		ShopBase shop = target as ShopBase;

		UpgradeStore upgradeShop = shop as UpgradeStore;
		if(upgradeShop != null)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Upgrades");
			if(GUILayout.Button("+",GUILayout.Width(25)))
			{
				upgradeShop.VisibleInventory.Add(CreateInstance<ShopItem<Upgrade>>());
			}
			EditorGUILayout.EndHorizontal();
			EditorGUI.indentLevel++;
			for (int i = 0; i < upgradeShop.VisibleInventory.Count; i++)
			{
				if (upgradeShop.VisibleInventory[i] == null)
				{
					EditorGUILayout.LabelField("Something has gone wrong");
					upgradeShop.VisibleInventory[i] = ShopItem<Upgrade>.CreateInstance<ShopItem<Upgrade>>();
					continue;
				}

				EditorGUILayout.BeginHorizontal();

				upgradeShop.VisibleInventory[i].Item = EditorGUILayout.ObjectField(upgradeShop.VisibleInventory[i].Item, typeof(Upgrade), true) as Upgrade;
				upgradeShop.VisibleInventory[i].Cost = EditorGUILayout.FloatField(upgradeShop.VisibleInventory[i].Cost);

				if (GUILayout.Button("-", GUILayout.Width(25)))
				{
					upgradeShop.VisibleInventory.RemoveAt(i);
				}

				EditorGUILayout.EndHorizontal();
			}
			EditorGUI.indentLevel--;
		}
	}

	void DrawInventory<T> ()
	{

	}
}
*/