using System;
using System.Collections.Generic;
using UnityEngine;

using Shapes;
using Serialization;
using Utility;

namespace Route.Linear
{
	[ExecuteInEditMode]
	public class LinearRoute : RouteBase
	{
		[SerializeField]
		HashSet<LinearCollider> _colliders = new HashSet<LinearCollider>();

		[SerializeField]
		SIPointLine _linearTraversable;

		public override float Length
		{
			get
			{
				return _linearTraversable.Value.AbsoluteLength;
			}
		}

		public IPointLine LinearTraversable
		{
			get
			{
				return _linearTraversable.Value;
			}
			set
			{
				_linearTraversable.Value = value;
			}
		}

		protected override void ModifyPoints()
		{
			foreach (PointToBeProcessed pointModification in pointQueue)
			{
				LinearCollider collider = pointModification._point as LinearCollider;

				if (pointModification._isAdding)
				{
					_colliders.Add(collider);
				}
				else
				{
					_colliders.Remove(collider);
				}
			}
			pointQueue.Clear();
		}

		public override Traveller GenerateNewTraveller()
		{
			LinearTraveller newTraveller = new GameObject("Traveller").AddComponent<LinearTraveller>();

			newTraveller.Assign(this);

			return newTraveller;
		}

		public override Traveller GenerateNewTraveller(Traveller oldTraveller, RoutePoint position)
		{
			LinearPoint point = position as LinearPoint;

			LinearTraveller newTraveller = oldTraveller.gameObject.AddOrGetComponent<LinearTraveller>();

			oldTraveller.UnAssign(newTraveller);

			newTraveller.Assign(point.ParentRoute, point);

			return newTraveller;
		}

		public void CheckTravellerCollision(LinearTraveller travellerToCheck, Ray travellerMovement, float distance)
		{
			canAddToQueue = false;
			foreach (LinearCollider collider in _colliders)
			{
				collider.TestTraveller(travellerToCheck, travellerMovement, distance);
			}
			canAddToQueue = true;
			ModifyPoints();
		}

		public override RoutePoint GenerateNewPoint()
		{
			LinearPoint point = new GameObject("Traveller").AddComponent<LinearPoint>();
			point.Assign(this);
			return point;
		}

		void OnEnable()
		{
			if (_linearTraversable == null)
			{
				_linearTraversable = ScriptableObject.CreateInstance<SIPointLine>();
			}
		}

#if UNITY_EDITOR
		[UnityEditor.MenuItem("Routing/Make new Traversible Route")]
		static void CreateCustomGameObject(UnityEditor.MenuCommand menuCommand)
		{
			// Create a custom game object
			GameObject gameObject = new GameObject("Traversible Route");
			gameObject.AddComponent<LinearRoute>();

			// Ensure it gets reparented if this was a context click (otherwise does nothing)
			UnityEditor.GameObjectUtility.SetParentAndAlign(gameObject, menuCommand.context as GameObject);
			// Register the creation in the undo system
			UnityEditor.Undo.RegisterCreatedObjectUndo(gameObject, "Create " + gameObject.name);
		}

		[UnityEditor.MenuItem("Routing/Make Traveller from selected Route")]
		static void MakeTraveller(UnityEditor.MenuCommand menuCommand)
		{
			// Create a custom game object
			GameObject gameObject = UnityEditor.Selection.activeGameObject.GetComponent<RouteBase>().GenerateNewTraveller().gameObject;

			// Ensure it gets reparented if this was a context click (otherwise does nothing)
			UnityEditor.GameObjectUtility.SetParentAndAlign(gameObject, menuCommand.context as GameObject);
			// Register the creation in the undo system
			UnityEditor.Undo.RegisterCreatedObjectUndo(gameObject, "Create " + gameObject.name);
		}

		[UnityEditor.MenuItem("Routing/Make Traveller from selected Route", true, 10)]
		static bool CanMakeTraveller(UnityEditor.MenuCommand menuCommand)
		{
			return (UnityEditor.Selection.activeGameObject != null && UnityEditor.Selection.activeGameObject.GetComponent<RouteBase>() != null) ;
		}
#endif
	}
}
