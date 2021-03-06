﻿using System;
using UnityEngine;

namespace Route.Linear
{
	public class LinearPoint : RoutePoint<LinearRoute>
	{
		[SerializeField]
		protected int _leftIndex = 0;

		[SerializeField]
		protected float _distanceFromIndex = 0;

		[SerializeField]
		protected float _progress = 0;

		[SerializeField]
		protected Vector2 _position;

		[SerializeField]
		protected LinearRoute _parent;

		public override LinearRoute ParentRoute
		{
			get
			{
				return _parent;
			}
		}

		public Vector2 Position
		{
			get
			{
				return _position;
			}
		}

		public override void UpdatePosition()
		{
			_parent.LinearTraversable.AdvanceAlongLine(ref _leftIndex, ref _distanceFromIndex, out _position.x, out _progress);
			OnPointMoveInvoke();
		}

		public override Vector3 GetWorldSpacePosition()
		{
			if (ParentRoute == null)
			{
				return Vector3.zero;
			}
			return _parent.LinearTraversable.GetPointAlongDistance(_leftIndex,_distanceFromIndex) + new Vector3(0,_position.y,0);
		}

		public Vector3 GetWorldSpacePosition(float offset)
		{
			if (ParentRoute == null)
			{
				return Vector3.zero;
			}
			return _parent.LinearTraversable.GetPointAlongDistance(_leftIndex, _distanceFromIndex + offset) + new Vector3(0, _position.y, 0);
		}

		public override Vector3 GetVelocity()
		{
			return Vector3.Lerp(
				_parent.LinearTraversable.GetVelocityAtIndex(_leftIndex),
				_parent.LinearTraversable.GetVelocityAtIndex(_leftIndex + 1),
				_progress);
		}

		//Assign
		public override void Assign(LinearRoute newRoute)
		{
			_parent = newRoute;
		}

		public void Assign(LinearRoute newRoute, Vector2 position)
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

			LinearRoute traversibleRoute = UnityEditor.Selection.activeGameObject.GetComponent<LinearRoute>();

			LinearPoint point = gameObject.AddComponent<LinearPoint>();
			point.Assign(traversibleRoute);

			// Ensure it gets reparented if this was a context click (otherwise does nothing)
			UnityEditor.GameObjectUtility.SetParentAndAlign(gameObject, menuCommand.context as GameObject);
			// Register the creation in the undo system
			UnityEditor.Undo.RegisterCreatedObjectUndo(gameObject, "Create " + gameObject.name);
		}

		[UnityEditor.MenuItem("Routing/Create Traversible Route Point", true)]
		static bool CheckIfSelectionValidTargetForNewRoutePoint(UnityEditor.MenuCommand menuCommand)
		{
			return UnityEditor.Selection.activeGameObject != null && UnityEditor.Selection.activeGameObject.GetComponent<LinearRoute>() != null;
		}
#endif
	}
}
