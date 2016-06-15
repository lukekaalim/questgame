using System;
using UnityEngine;

namespace Route.Traversible
{
	public class TraversiblePoint : RoutePoint<TraversibleRoute>
	{
		[SerializeField]
		int _leftIndex = 0;

		[SerializeField]
		float _distanceFromIndex = 0;

		[SerializeField]
		Vector2 _position;

		[SerializeField]
		TraversibleRoute _parent;

		public override TraversibleRoute ParentRoute
		{
			get
			{
				return _parent;
			}
		}

		public override Vector3 GetWorldSpacePosition()
		{
			return _parent.LinearTraversable.GetPointAlongDistance(_leftIndex, _distanceFromIndex);
		}

		//Assign
		public override void Assign(TraversibleRoute newRoute)
		{
			_parent = newRoute;
		}

		public void Assign(TraversibleRoute newRoute, Vector2 position)
		{
			Assign(newRoute);
			_position = position;
			_distanceFromIndex = position.x;
			newRoute.LinearTraversable.AdvanceAlongLine(ref _leftIndex, ref _distanceFromIndex, out position.x);
		}

#if UNITY_EDITOR

		[UnityEditor.MenuItem("Routing/Create Traversible Route Point")]
		static void CreateNewRoutePoint(UnityEditor.MenuCommand menuCommand)
		{
			// Create a custom game object
			GameObject gameObject = new GameObject("Route Point");

			TraversibleRoute traversibleRoute = UnityEditor.Selection.activeGameObject.GetComponent<TraversibleRoute>();

			TraversiblePoint point = gameObject.AddComponent<TraversiblePoint>();
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
