using System;
using UnityEngine;

namespace Route
{
	//A Route Point represents a physical point on a route.
	public abstract class RoutePoint : MonoBehaviour
	{
		public event Action OnPointMove;

		public abstract RouteBase ParentRouteAsRouteBase
		{
			get;
		}

		public abstract Vector3 GetWorldSpacePosition();

		public abstract void UpdatePosition();

		public void OnPointMoveInvoke()
		{
			if (OnPointMove != null)
			{
				OnPointMove.Invoke();
			}
		}
	}

	public abstract class RoutePoint<T> : RoutePoint where T : RouteBase
	{
		public override RouteBase ParentRouteAsRouteBase
		{
			get
			{
				return ParentRoute as RouteBase;
			}
		}

		public abstract T ParentRoute
		{
			get;
		}

		//Assign is called right after you create a route point, so it can properly identify it's parent
		public abstract void Assign(T newRoute);

#if UNITY_EDITOR
		protected virtual void OnDrawGizmos()
		{
			if (ParentRoute != null)
			{
				float size = UnityEditor.HandleUtility.GetHandleSize(transform.position) * 0.2f;

				Gizmos.color = new Color(1, 0.75f, 0, 0.5f);
				transform.position = GetWorldSpacePosition();
				Gizmos.DrawSphere(transform.position, size);
			}
		}
#endif
	}
}
