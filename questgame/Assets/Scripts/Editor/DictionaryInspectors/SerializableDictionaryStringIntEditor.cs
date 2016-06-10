using UnityEngine;
using UnityEditor;

namespace Serialization
{
	[CustomPropertyDrawer(typeof(SerializableDictionaryStringInt), true)]
	class SerializableDictionaryStringIntEditor : SerializableDictionaryEditor<string, int>
	{
		protected override string GetNewKey()
		{
			return "";
		}
		protected override string DrawKeyProperty(Rect position , string key)
		{
			return EditorGUI.TextField(position, key);
		}
		protected override int DrawValuePropert(Rect position, int value)
		{
			return EditorGUI.IntField(position, value);
		}
	}
}
