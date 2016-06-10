using System;
using UnityEngine;

namespace Route
{
	public abstract class RoutePoint : MonoBehaviour
	{
		public event Action<Traveller> OnActivation;

		public abstract RouteBase ParentRoute
		{
			get;
		}

		protected abstract Vector3 GetDebugPosition();

		public abstract bool TestTraveller(Traveller travellerToTest);

		public abstract void Assign(RouteBase newRoute);

		protected void ActivatePoint(Traveller travellerThatActivated)
		{
			OnActivation.Invoke(travellerThatActivated);
		}

#if UNITY_EDITOR
		void OnDrawGizmos()
		{
			float size = UnityEditor.HandleUtility.GetHandleSize(transform.position) * 0.2f;
			Gizmos.color = new Color(1, 0.75f, 0, 0.5f);
			transform.position = GetDebugPosition();
			Gizmos.DrawSphere(transform.position, size);
		}
#endif
	}
}
