using System;
using UnityEngine;

namespace Route
{
	[ExecuteInEditMode]
	public class TraversibleRoutePoint : RoutePoint
	{
		[SerializeField]
		TraversibleRoute _parent;

		[SerializeField]
		protected float _position;
		public float Position
		{
			get
			{
				return _position;
			}
			set
			{
				_position = value;
			}
		}

		public override RouteBase ParentRoute
		{
			get
			{
				return _parent;
			}
		}

		protected override Vector3 GetDebugPosition()
		{
			return _parent.LinearTraversable.GetPointAlongDistance(_position);
		}

		public override void Assign(RouteBase newRoute)
		{
			if (_parent != null)
			{
				OnDestroy();
			}

			_parent = newRoute as TraversibleRoute;
			if (_parent == null && newRoute != null)
			{
				Debug.LogWarning("Error: Attempting to assign Route Point to the wrong type of Route");
				return;
			}

			_parent.Points.Add(this);
		}

		public void Assign(RouteBase newRoute, float position)
		{
			_position = position;
			Assign(newRoute);
		}

		void OnDestroy()
		{
			_parent.Points.Remove(this);
		}

		public override bool TestTraveller(Traveller travellerToTest)
		{
			TraversibleRouteTraveller traveller = travellerToTest as TraversibleRouteTraveller;

			if ((_position < traveller.AbsoluteDistance && _position > traveller.AbsoluteDistanceLastFrame) ||
				(_position > traveller.AbsoluteDistance && _position < traveller.AbsoluteDistanceLastFrame))
			{
				ActivatePoint(travellerToTest);
				return true;
			}
			return false;
		}

#if UNITY_EDITOR

		[UnityEditor.MenuItem("Routing/Create Traversible Route Point")]
		static void CreateNewRoutePoint(UnityEditor.MenuCommand menuCommand)
		{
			// Create a custom game object
			GameObject gameObject = new GameObject("Route Point");

			TraversibleRoute traversibleRoute = UnityEditor.Selection.activeGameObject.GetComponent<TraversibleRoute>();

			TraversibleRoutePoint point = gameObject.AddComponent<TraversibleRoutePoint>();
			point.Assign(traversibleRoute);

			// Ensure it gets reparented if this was a context click (otherwise does nothing)
			UnityEditor.GameObjectUtility.SetParentAndAlign(gameObject, menuCommand.context as GameObject);
			// Register the creation in the undo system
			UnityEditor.Undo.RegisterCreatedObjectUndo(gameObject, "Create " + gameObject.name);
		}

		[UnityEditor.MenuItem("Routing/Create Traversible Route Point", true)]
		static bool CheckIfSelectionValidTargetForNewRoutePoint(UnityEditor.MenuCommand menuCommand)
		{
			return UnityEditor.Selection.activeGameObject != null && UnityEditor.Selection.activeGameObject.GetComponent<TraversibleRoute>() != null;
		}

#endif
	}
}
