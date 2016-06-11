using System;
using UnityEngine;

namespace Route
{
	[ExecuteInEditMode]
	public class TraversibleRoutePoint : RoutePoint
	{
		[Flags]
		public enum CollisionDirections
		{
			Left = 1,
			Right = 2,
			Top = 4,
			Bottom = 8
		}

		[SerializeField]
		TraversibleRoute _parent;

		[SerializeField, HideInInspector]
		Bounds _collisionBounds;

		[SerializeField]
		CollisionDirections _activeDirections;

		public Vector2 Extents
		{
			get
			{
				return _collisionBounds.extents;
			}
			set
			{
				_collisionBounds.extents = value;
			}
		}

		public Vector2 Position
		{
			get
			{
				return _collisionBounds.center;
			}
			set
			{
				_collisionBounds.center = value;
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
			return GetDebugPosition(0);
		}

		protected Vector3 GetDebugPosition(float modifier = 0)
		{
			return _parent.LinearTraversable.GetPointAlongDistance(Position.x + modifier) + new Vector3(0,Position.y);
		}

		//Assign
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

			Position = new Vector2();
			_collisionBounds.center = Position;
		}

		public void Assign(RouteBase newRoute, Vector2 position)
		{
			Position = position;
			_collisionBounds.center = position;
			Assign(newRoute);
		}

		void OnDestroy()
		{
			_parent.Points.Remove(this);
		}

		public override bool TestTraveller(Traveller travellerToTest, Ray travellerMovement, float distance)
		{
			float collisionDistance;
			if (_collisionBounds.IntersectRay(travellerMovement, out collisionDistance) && collisionDistance <= distance)
			{
				Vector2 collisionPoint = travellerMovement.origin + (travellerMovement.direction * collisionDistance);
				ActivatePoint(travellerToTest, collisionPoint);
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


		protected override void OnDrawGizmos()
		{
			Vector3 position = GetDebugPosition();
			Matrix4x4 originalMatrix = Gizmos.matrix;

			Vector3 velocity = _parent.LinearTraversable.GetVelocityAtIndex(_parent.LinearTraversable.GetIndexAtPoint(Position.x));
			Gizmos.matrix = Matrix4x4.TRS(position, Quaternion.LookRotation(velocity, Vector3.up), Vector3.one);

			Gizmos.color = new Color(0.8f, 0.6f, 0, 0.5f);
			Gizmos.DrawCube(Vector3.zero, new Vector3(0.1f, _collisionBounds.extents.y, _collisionBounds.extents.x * 2));

			Gizmos.color = Color.white;
			Gizmos.DrawWireCube(Vector3.zero, new Vector3(0.1f, _collisionBounds.extents.y, _collisionBounds.extents.x * 2));

			Gizmos.matrix = originalMatrix;

			Gizmos.color = Color.red;
			Gizmos.DrawCube(GetDebugPosition(-_collisionBounds.extents.x), new Vector3(0.1f, _collisionBounds.extents.y, 0.1f));
			Gizmos.DrawCube(GetDebugPosition(_collisionBounds.extents.x), new Vector3(0.1f, _collisionBounds.extents.y, 0.1f));
			base.OnDrawGizmos();
		}

#endif
	}
}
