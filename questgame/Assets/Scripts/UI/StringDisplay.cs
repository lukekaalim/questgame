using UnityEngine;
using UnityEngine.UI;

namespace DisplayBinding
{
	[RequireComponent(typeof(Text))]
	public class StringDisplay : DisplayBase<string>
	{
		[SerializeField, HideInInspector]
		Text textDisplay;

		void OnEnable()
		{
			textDisplay = GetComponent<Text>();
		}

		public override void Refresh()
		{
			textDisplay = GetComponent<Text>();
			base.Refresh();
		}

		protected override void OnValueChange(string oldValue, string newValue)
		{
			textDisplay.text = newValue;
		}
	}
}