﻿using System;
using UnityEngine;

namespace Route
{
	//Traveller Base
	/*
	 *
	 *		A Traveller is something that moves along a route
	 *		There is a dedicated traveller type for every route type
	 *		A traveller can be updated by passing in a controller, which governs
	 *		the travellers intent.
	 *
	 */
	public abstract class Traveller : MonoBehaviour
	{
		public abstract bool IsAssigned
		{
			get;
		}

		public abstract Traveller UpdateTraveller();

		public abstract void SetController(Controller newController);

		public abstract void SetNextTraveller(Traveller nextTraveller);

		public abstract void UpdatePosition();

		public abstract RouteBase CurrentGenericRoute
		{
			get;
		}


#if UNITY_EDITOR

		void OnDrawGizmos()
		{
			float size = UnityEditor.HandleUtility.GetHandleSize(transform.position) * 0.2f;
			Gizmos.color = new Color(1, 0, 0, 0.6f);
			Gizmos.DrawSphere(transform.position, size);
			Gizmos.DrawLine(transform.position, transform.position + (transform.forward * size * 5));
		}

#endif
	}

	//Traveller generic
	public abstract class Traveller<T> : Traveller where T : RouteBase
	{
		public abstract T CurrentRoute
		{
			get;
		}

		public override RouteBase CurrentGenericRoute
		{
			get
			{
				return CurrentRoute;
			}
		}

		public abstract void Assign (T newRoute);
	}
}