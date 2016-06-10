using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

namespace DisplayBinding
{
	[RequireComponent(typeof(RectTransform)), ExecuteInEditMode]
	public abstract class BarDisplayBase<T> : LimitedDisplay<T> where T : IEquatable<T>, IComparable<T>
	{
		//The Container for the Bar. This is the transform that the Component is attached to.
		[SerializeField, HideInInspector]
		RectTransform container;
		//The child of the container. This is the object that will be expanding and contracting
		//auto generated when not already existsing
		[SerializeField, HideInInspector]
		RectTransform fill = null;

		[SerializeField]
		protected bool _vertical = true;
		public bool Vertical { get { return _vertical; } set { _vertical = value; } }

		void Awake()
		{
			for(int i = 0; i < transform.childCount; i++)
			{
				Transform child = transform.GetChild(i);
				if (child.name == "Bar Display Fill")
				{
					fill = child.GetComponent<RectTransform>();
					break;
				}
			}

			if (fill == null)
			{
				container = GetComponent<RectTransform>();

				GameObject fillObject = new GameObject("Bar Display Fill");

				fill = fillObject.AddComponent<RectTransform>();
				fillObject.AddComponent<Image>();

				fill.SetParent(container);
			}

			fill.anchorMax = Vector2.one;
			fill.anchorMin = Vector2.zero;

			fill.localPosition = Vector3.zero;

			fill.localScale = Vector3.one;

			fill.anchoredPosition = Vector2.zero;
			fill.sizeDelta = Vector2.zero;
		}

		void Start()
		{
			OnValueChange(CurrentValue, CurrentValue);
		}

		protected void CalculateDisplay(float value, float min, float max)
		{
			float percentage = value / max;

			if (Vertical)
			{
				fill.pivot = new Vector2(0.5f, 0);
				fill.sizeDelta = new Vector2(1, (percentage - 1) * container.rect.height);
			}
			if (!Vertical)
			{
				fill.pivot = new Vector2(0, 0.5f);
				fill.sizeDelta = new Vector2((percentage - 1) * container.rect.width, 1);
			}
		}
	}
}