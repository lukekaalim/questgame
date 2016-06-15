using System.Collections.Generic;
using UnityEngine;

using Shapes;
using Serialization;
using System;

namespace Route.Traversible
{
	[ExecuteInEditMode]
	public class TraversibleRoute : RouteBase
	{
		[SerializeField]
		HashSet<TraversibleCollider> _colliders;

		[SerializeField]
		SILinearTraversible _linearTraversable;

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
		}

		protected override void ModifyPoints()
		{
			foreach (PointToBeProcessed pointModification in pointQueue)
			{
				TraversibleCollider collider = pointModification._point as TraversibleCollider;

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
			TraversibleTraveller newTraveller = new GameObject("Traveller").AddComponent<TraversibleTraveller>();

			newTraveller.Assign(this);

			return newTraveller;
		}

		public void CheckCollision(Traveller travellerToCheck, Ray travellerMovement, float distance)
		{
			foreach (TraversibleCollider point in _colliders)
			{
				point.TestTraveller(travellerToCheck, travellerMovement, distance);
			}
			ModifyPoints();
		}

		public override RoutePoint GenerateNewPoint()
		{
			TraversiblePoint point = new GameObject("Traveller").AddComponent<TraversiblePoint>();
			point.Assign(this);
			return point;
		}

		void OnEnable()
		{
			if (_linearTraversable == null)
			{
				_linearTraversable = ScriptableObject.CreateInstance<SILinearTraversible>();
			}
		}

#if UNITY_EDITOR
		[UnityEditor.MenuItem("Routing/Make new Traversible Route")]
		static void CreateCustomGameObject(UnityEditor.MenuCommand menuCommand)
		{
			// Create a custom game object
			GameObject gameObject = new GameObject("Traversible Route");
			gameObject.AddComponent<TraversibleRoute>();

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
