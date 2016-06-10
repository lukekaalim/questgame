using UnityEngine;
using System.Collections.Generic;

public class ShopTabManager : MonoBehaviour
{
	[SerializeField]
	List<Panel> _controlledPanels;

	void Start()
	{
		foreach (Panel panel in _controlledPanels)
		{
			panel.OnSwitch += SwitchResponse;
			panel.gameObject.SetActive(true);
		}
	}

	public void SwitchResponse(Panel thisPanel, int newState)
	{
		if (newState == 0)
		{

			foreach (Panel panel in _controlledPanels)
			{
				if (panel != thisPanel)
				{
					panel.CurrentState = 1;
				}
			}
		}

		if (newState == 2)
		{
			foreach (Panel panel in _controlledPanels)
			{
				if (panel != thisPanel)
				{
					panel.CurrentState = 2;
				}
			}
		}
	}
}
