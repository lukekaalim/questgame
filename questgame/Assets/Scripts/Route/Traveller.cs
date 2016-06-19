using System;
using UnityEngine;

using Controllers;
using Utility;

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
		protected Controller _activeController;

		protected Traveller _nextTraveller;

		public abstract bool IsAssigned
		{
			get;
		}

		public abstract RouteBase CurrentGenericRoute
		{
			get;
		}

		public abstract Traveller UpdateTraveller();

		public abstract void UnAssign(Traveller nextTraveller);

		public abstract Vector3 GetWorldSpacePosition();

		public abstract void UpdatePosition();

		public virtual Traveller GoToPoint(RoutePoint startPoint)
		{
			if (startPoint.ParentRouteAsRouteBase.GetType() == typeof(Linear.LinearRoute))
			{
				Linear.LinearPoint point = startPoint as Linear.LinearPoint;
				Linear.LinearTraveller traveller = gameObject.AddOrGetComponent<Linear.LinearTraveller>();

				traveller.Assign(point.ParentRoute, point);

				return traveller;
			}
			return null;
		}

		public virtual void SetController(Controller newController)
		{
			_activeController = newController;
		}


#if UNITY_EDITOR

		protected virtual void OnDrawGizmos()
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
		public abstract void Assign (T newRoute, RoutePoint<T> position);
	}
}
