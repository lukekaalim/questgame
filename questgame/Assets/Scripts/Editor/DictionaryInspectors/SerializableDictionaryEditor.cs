using UnityEngine;
using UnityEditor;

using System.Collections.Generic;

namespace Serialization
{
	public abstract class SerializableDictionaryEditor<TKey,YValue> : PropertyDrawer
	{
		TKey newKey;
		int baseHeight = 4;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			SerializableDictionary<TKey, YValue> dict = property.objectReferenceValue as object as SerializableDictionary<TKey, YValue>;

			if (newKey == null)
			{
				newKey = GetNewKey();
			}

			bool containsKey = dict.ContainsKey(newKey);

			// Using BeginProperty / EndProperty on the parent property means that
			// prefab override logic works on the entire property.

			EditorGUI.BeginProperty(position, label, property);

			float labelHeight = position.height / (dict.Count + baseHeight);

			// Draw label
			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

			// Don't make child fields be indented
			var indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;

			// Draw fields - passs GUIContent.none to each so they are drawn without labels
			if (dict != null)
			{
				Rect infoLabel = new Rect(position.x, position.y, position.width, labelHeight * 2);

				EditorGUI.HelpBox(infoLabel,"Key Type: '" + dict.KeyType.ToString() + "', Value Type : '" + dict.ValueType.ToString() + "', Count: " + dict.Count, MessageType.Info);

				Rect addBox = new Rect(position.x, position.y + (labelHeight * 2), position.width/2, labelHeight);
				Rect addButton = new Rect(position.x + position.width / 2, position.y + (labelHeight * 2), position.width / 2, labelHeight);

				newKey = DrawKeyProperty(addBox, newKey);

				if (containsKey)
				{
					EditorGUI.BeginDisabledGroup(true);
					GUI.Button(addButton, "Add New Entry");
					EditorGUI.EndDisabledGroup();

					Rect errorInfo = new Rect(position.x, position.y + (labelHeight * 3), position.width, labelHeight);
					EditorGUI.HelpBox(errorInfo, "Key Already Exisits", MessageType.Error);
				}
				else
				{
					if (GUI.Button(addButton, "Add New Entry"))
					{
						dict.Add(newKey, default(YValue));
						GUI.FocusControl("");
						newKey = GetNewKey();
					}
				}

				DrawKeyValues(dict, baseHeight, position, labelHeight);
			}
			//EditorGUI.TextField(amountRect, (property.objectReferenceValue as SerializableDictionary).KeyType.ToString());

			//EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("amount"), GUIContent.none);
			//EditorGUI.PropertyField(unitRect, property.FindPropertyRelative("unit"), GUIContent.none);
			//EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("name"), GUIContent.none);

			// Set indent back to what it was
			EditorGUI.indentLevel = indent;

			EditorGUI.EndProperty();
		}

		void DrawKeyValues(SerializableDictionary<TKey, YValue> dict, int startingHeight, Rect position, float labelHeight)
		{
			int index = baseHeight;
			List<TKey> keys = new List<TKey>(dict.Keys);

			foreach (TKey key in keys)
			{
				Rect keyRect = new Rect(position.x + 25, position.y + (labelHeight * index), position.width / 2, labelHeight);
				Rect valueRect = new Rect(position.x + 25 + (position.width / 2), position.y + (labelHeight * index), (position.width / 2) - 50, labelHeight);

				Rect removeButton = new Rect((position.x + position.width - 25), position.y + (labelHeight * index), 25, labelHeight);

				if (GUI.Button(removeButton, "-"))
				{
					dict.Remove(key);
					index++;
					continue;
				}

				TKey newKey;
				newKey = DrawKeyProperty(keyRect, key);
				if (!newKey.Equals(key) && !dict.ContainsKey(newKey))
				{
					dict.Add(newKey,dict[key]);
					dict.Remove(key);
					index++;
					continue;
				}

				dict[key] = DrawValuePropert(valueRect, dict[key]);

				index++;
			}
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			SerializableDictionary<TKey, YValue> dict = property.objectReferenceValue as object as SerializableDictionary<TKey, YValue>;

			return base.GetPropertyHeight(property, label) * (dict.Count + baseHeight);
		}

		protected abstract TKey GetNewKey();
		protected abstract TKey DrawKeyProperty(Rect position, TKey key);
		protected abstract YValue DrawValuePropert(Rect position, YValue value);
	}
}
