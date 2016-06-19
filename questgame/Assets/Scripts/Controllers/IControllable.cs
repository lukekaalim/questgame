using System;
using UnityEngine;

namespace Controllers
{
	public interface IControllable
	{
		void SetController(Controller newController);

		IControllable ApplyInput(Intention controllerIntent);
	}
}
