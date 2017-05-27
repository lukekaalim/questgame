using System;
using UnityEngine;

using Route;

namespace Test
{
	public class ColliderShouter : MonoBehaviour
	{
		[SerializeField]
		RouteCollider _collider;

		void OnEnable()
		{
			_collider.OnEnter += Shout;
		}

		void OnDisable()
		{
			_collider.OnEnter -= Shout;
		}

		void Shout(Traveller traveller)
		{
			Debug.Log(traveller.name + " hit " + name + "!");
		}
	}
}
