using System;
using UnityEngine;
using UnityEngine.UI;

using Build;
using DisplayBinding;

namespace Displays
{
	public class VersionDisplay : DisplayBase<VersionInfoContainer>
	{
		[SerializeField]
		Text textToDisplayTo;

		[SerializeField]
		Image stampToColor;

		override protected void OnValueChange(VersionInfoContainer oldValue, VersionInfoContainer newValue)
		{
			if (newValue != null && newValue.CurrentVersionInfo != null)
			{
				textToDisplayTo.text = newValue.CurrentVersionInfo.ToString();
				stampToColor.color = newValue.CurrentVersionInfo.StampColor;
			}
		}
	}
}
