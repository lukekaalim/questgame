using System;
using UnityEngine;
using UnityEditor;

using Utils;

namespace Trade
{
	/*
	[CustomPropertyDrawer(typeof(SingleCurrencyContainer))]
	class SingleCurrencyDrawer : PropertyDrawer
	{
		bool showMinMax = false;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			SingleCurrencyContainer container = property.objectReferenceValue as object as SingleCurrencyContainer;

			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

			Rect nameRect;
			Rect amountRect;
			Rect minMaxToggle;

			if (!showMinMax)
			{
				nameRect = new Rect(position.x, position.y, position.width / 2, position.height);
				amountRect = new Rect(position.x + position.width / 2, position.y, (position.width / 2) - 20, position.height);
				minMaxToggle = new Rect(position.x + position.width - 20, position.y, 20, position.height);
			}
			else
			{
				nameRect = new Rect(position.x, position.y, position.width / 2, position.height/2);
				amountRect = new Rect(position.x + position.width / 2, position.y, (position.width / 2) - 20, position.height/2);
				minMaxToggle = new Rect(position.x + position.width - 20, position.y, 20, position.height/2);
			}

			if (container != null)
			{
				container.CurrencyName = EditorGUI.TextField(nameRect, container.CurrencyName);
				container.SetCurrency(EditorGUI.IntField(amountRect, container.GetCurrency()));

				showMinMax = EditorGUI.Toggle(minMaxToggle, showMinMax);

				if (showMinMax)
				{
					Rect minMaxFields = RectUtils.BottomHalf(position);

					Rect minField = RectUtils.LeftHalf(minMaxFields);
					Rect maxfield = RectUtils.RightHalf(minMaxFields);

					EditorGUI.LabelField(RectUtils.LeftHalf(minField),"Min");

					EditorGUI.LabelField(RectUtils.LeftHalf(maxfield), "Max");

					container.SetMinMax(EditorGUI.IntField(RectUtils.RightHalf(minField), container.MinCap), EditorGUI.IntField(RectUtils.RightHalf(maxfield), container.MaxCap));
				}
			}
			else
			{
				if (GUI.Button(position, "Create new SingleCurrencyContainer"))
				{
					property.objectReferenceValue = SingleCurrencyContainer.CreateInstance("currencyName", 0);
				}
			}


			EditorGUI.EndProperty();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			if (showMinMax)
			{
				return base.GetPropertyHeight(property, label) * 2;
			}
			else
			{
				return base.GetPropertyHeight(property, label);
			}
		}
	}

	[CustomPropertyDrawer(typeof(MultipleCurrencyContainer))]
	class MultipleCurrencyDrawer : PropertyDrawer
	{
		SerializedProperty dictionaryProperty;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			MultipleCurrencyContainer container = property.objectReferenceValue as object as MultipleCurrencyContainer;

			EditorGUI.BeginProperty(position, label, property);

			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

			if (container == null)
			{
				if (GUI.Button(position, "Create new MultipleCurrencyContainer"))
				{
					property.objectReferenceValue = ScriptableObject.CreateInstance<MultipleCurrencyContainer>();
				}
			}
			else
			{

				SerializedObject serializedObject = new SerializedObject(container);

				dictionaryProperty = serializedObject.FindProperty("dictionary");
				
			}

			EditorGUI.EndProperty();
		}
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			MultipleCurrencyContainer container = property.objectReferenceValue as object as MultipleCurrencyContainer;

			if (container == null)
			{
				return base.GetPropertyHeight(property, label);
			}
			else
			{
				if (dictionaryProperty == null)
				{
					SerializedObject serializedObject = new SerializedObject(container);
					dictionaryProperty = serializedObject.FindProperty("dictionary");
				}
				return editor.GetPropertyHeight(dictionaryProperty, GUIContent.none);
			}
		}
	}
	*/
}
