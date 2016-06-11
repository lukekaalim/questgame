using System;
using UnityEngine;

namespace Route
{
	public abstract class RoutePoint : MonoBehaviour
	{
		public event Action<Traveller, Vector2> OnActivation;

		public abstract RouteBase ParentRoute
		{
			get;
		}

		protected abstract Vector3 GetDebugPosition();

		public abstract bool TestTraveller(Traveller travellerToTest, Ray travellerMovement, float distance);

		//Assign is called right after you create a route point, so it can be added to
		//the appropriate route for collision checking and positioning
		public abstract void Assign(RouteBase newRoute);

		protected void ActivatePoint(Traveller travellerThatActivated, Vector2 collisionPoint)
		{
			OnActivation.Invoke(travellerThatActivated, collisionPoint);
		}

		void OnEnable()
		{
			if (ParentRoute != null && !ParentRoute.Points.Contains(this))
			{
				ParentRoute.Points.Add(this);
			}
		}

		void OnDisable()
		{
			if (ParentRoute != null && ParentRoute.Points.Contains(this))
			{
				ParentRoute.QueuePointForRemoval(this);
			}
		}

#if UNITY_EDITOR
		protected virtual void OnDrawGizmos()
		{
			float size = UnityEditor.HandleUtility.GetHandleSize(transform.position) * 0.2f;
			Gizmos.color = new Color(1, 0.75f, 0, 0.5f);
			transform.position = GetDebugPosition();
			Gizmos.DrawSphere(transform.position, size);
		}
#endif
	}
}
