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

		override protected void OnValueChange(VersionInfoContainer oldValue, VersionInfoContainer newValue)
		{
			if (newValue != null && newValue.CurrentVersionInfo != null)
			{
				textToDisplayTo.text = newValue.CurrentVersionInfo.ToString();
			}
		}
	}
}
