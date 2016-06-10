using System;
using System.Collections.Generic;
using UnityEngine;

class ExclusivePanelController : MonoBehaviour
{
	[SerializeField]
	List<Panel> _controlledPanels;

	void Start()
	{
		foreach(Panel panel in _controlledPanels)
		{
			panel.OnSwitch += SwitchResponse;
			panel.gameObject.SetActive(true);
		}
	}

	public void SwitchResponse(Panel thisPanel, int newState)
	{
	//	Debug.Log("A panel is switching!");

		if(newState == 0)
		{
	//		Debug.Log("Its switchint to active! Time to tell my friends");

			foreach (Panel panel in _controlledPanels)
			{
				if (panel != thisPanel)
				{
			//		Debug.Log("Setting Panel to deactive");
					panel.CurrentState = 1;
				}
			}
		}
	}
}