using System;
using UnityEngine;

namespace Route
{
	[ExecuteInEditMode]
	public class TraversibleRoutePoint : RoutePoint
	{
		[SerializeField]
		TraversibleRoute _parent;

		[SerializeField, HideInInspector]
		Bounds _collisionBounds;

		public Bounds CollisionBounds
		{
			get
			{
				return _collisionBounds;
			}
			set
			{
				_collisionBounds = value;
			}
		}

		public override RouteBase ParentRoute
		{
			get
			{
				return _parent;
			}
		}

		protected override Vector3 GetWorldSpacePosition()
		{
			return GetDebugPosition(0);
		}

		protected Vector3 GetDebugPosition(float modifier = 0)
		{
			return _parent.LinearTraversable.GetPointAlongDistance(CollisionBounds.center.x + modifier) + new Vector3(0, CollisionBounds.center.y);
		}

		//Assign
		public override void Assign(RouteBase newRoute)
		{
			if (_parent != null)
			{
				OnDisable();
			}

			_parent = newRoute as TraversibleRoute;
			if (_parent == null && newRoute != null)
			{
				Debug.LogWarning("Error: Attempting to assign Route Point to the wrong type of Route");
				return;
			}

			_parent.Points.Add(this);
		}

		public void Assign(RouteBase newRoute, Vector2 position)
		{
			_collisionBounds.center = position;
			Assign(newRoute);
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
			/*
			 *
			//Get the world-space position of the point via a semi-expensive call the route
			Vector3 position = GetDebugPosition();
			//Store the original matrix
			Matrix4x4 originalMatrix = Gizmos.matrix;

			//Get the velocity(direction) of the point on the traversable
			Vector3 velocity = _parent.LinearTraversable.GetVelocityAtIndex(_parent.LinearTraversable.GetIndexAtPoint(CollisionBounds.center.x));


			//Modify the matrix to point it at the right postiion
			Gizmos.matrix = Matrix4x4.TRS(position, Quaternion.LookRotation(velocity, Vector3.up), Vector3.one);

			//Draw Extents Solid
			Gizmos.color = new Color(0.8f, 0.6f, 0, 0.5f);
			Gizmos.DrawCube(Vector3.zero, new Vector3(0.1f, _collisionBounds.extents.y * 2, _collisionBounds.extents.x * 2));

			//Draw Extents Outline
			Gizmos.color = Color.white;
			Gizmos.DrawWireCube(Vector3.zero, new Vector3(0.1f, _collisionBounds.extents.y * 2, _collisionBounds.extents.x * 2));

			//Revert back to the original matrix
			Gizmos.matrix = originalMatrix;

			*/

			//Draw the Extents Edges in proper space, since the previous extents solid won't warp along the contours of the traversible
			Gizmos.color = Color.red;
			Gizmos.DrawCube(GetDebugPosition(-_collisionBounds.extents.x), new Vector3(0.1f, _collisionBounds.extents.y * 2, 0.1f));
			Gizmos.DrawCube(GetDebugPosition(_collisionBounds.extents.x), new Vector3(0.1f, _collisionBounds.extents.y * 2, 0.1f));

			//And draw the regular point
			base.OnDrawGizmos();
		}

#endif
	}
}
