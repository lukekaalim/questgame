using System;
using UnityEngine;
using UnityEngine.UI;

namespace DisplayBinding
{
	[RequireComponent(typeof(Text))]
	class IntDisplay : DisplayBase<int>
	{
		[SerializeField, HideInInspector]
		Text textDisplay;

		void Awake()
		{
			textDisplay = GetComponent<Text>();
		}

		public override void Refresh()
		{
			textDisplay = GetComponent<Text>();
			base.Refresh();
		}

		protected override void OnValueChange(int oldValue, int newValue)
		{
			textDisplay.text = newValue.ToString();
		}
	}
}
