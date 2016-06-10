using UnityEditor;
using UnityEngine;

//[CustomPropertyDrawer(typeof(ShopItem))]
public class ShopItemDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty(position, label, property);

		position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), GUIContent.none);

		var keyRect = new Rect(position.x, position.y, position.width/2, position.height);
		var valueRect = new Rect(position.x + (position.width / 2), position.y, position.width / 2, position.height);


		EditorGUI.PropertyField(keyRect, property.FindPropertyRelative("Object"),GUIContent.none);
		EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("Cost"), GUIContent.none);

		EditorGUI.EndProperty();
	}
}