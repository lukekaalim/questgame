using UnityEngine;
using UnityEditor;

namespace Serialization
{
	public abstract class InferfacePropertyDrawer<T> : PropertyDrawer where T : class
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

			SerializableInterface<T> serializedInterface = property.objectReferenceValue as object as SerializableInterface<T>;

			if (serializedInterface != null)
			{

				Object objectValue = serializedInterface.Value as Object;

				if (objectValue != null)
				{
					MonoBehaviour newValue = EditorGUI.ObjectField(position, objectValue, typeof(MonoBehaviour), true) as MonoBehaviour;
					T castValue = newValue as T;
					if (castValue == null && newValue != null)
					{
						castValue = newValue.GetComponent<T>();
						if (castValue == null)
						{
							castValue = serializedInterface.Value;
						}
					}

					if (serializedInterface.Value != castValue)
					{
						serializedInterface.Value = castValue;
					}
				}

				if (serializedInterface.Value == null)
				{
					MonoBehaviour newValue = EditorGUI.ObjectField(position, null, typeof(MonoBehaviour), true) as MonoBehaviour;
					T castValue = newValue as T;
					if (castValue == null && newValue != null)
					{
						castValue = newValue.GetComponent<T>();
						if (castValue != null && castValue != serializedInterface.Value)
						{
							serializedInterface.Value = castValue;
						}
					}

					if (serializedInterface.Value != castValue)
					{
						serializedInterface.Value = castValue;
					}
				}

			}
			else
			{
				property.objectReferenceValue = CreateInstance();
			}

			EditorGUI.EndProperty();
		}

		protected abstract SerializableInterface<T> CreateInstance();
	}

	[CustomPropertyDrawer(typeof(SIHasCurrency), true)]
	class SIHasCurrencyDrawer : InferfacePropertyDrawer<Trade.IHasCurrency>
	{
		protected override SerializableInterface<Trade.IHasCurrency> CreateInstance()
		{
			return ScriptableObject.CreateInstance<SIHasCurrency>();
		}
	}

	[CustomPropertyDrawer(typeof(SICurrencyContainer), true)]
	class SICurrencyCollectionDrawer : InferfacePropertyDrawer<Trade.ICurrencyContainer>
	{
		protected override SerializableInterface<Trade.ICurrencyContainer> CreateInstance()
		{
			return ScriptableObject.CreateInstance<SICurrencyContainer>();
		}
	}

	[CustomPropertyDrawer(typeof(SIPathable), true)]
	class SIPathableDrawer : InferfacePropertyDrawer<Shapes.IPathable>
	{
		protected override SerializableInterface<Shapes.IPathable> CreateInstance()
		{
			return ScriptableObject.CreateInstance<SIPathable>();
		}
	}

	[CustomPropertyDrawer(typeof(SIPointLine), true)]
	class SIPointLineDrawer : InferfacePropertyDrawer<Shapes.IPointLine>
	{
		protected override SerializableInterface<Shapes.IPointLine> CreateInstance()
		{
			return ScriptableObject.CreateInstance<SIPointLine>();
		}
	}
}
