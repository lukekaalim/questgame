using System;
using UnityEngine;

using Shapes;
using Serialization;

namespace Route
{
	[ExecuteInEditMode]
	public class TraversibleRoute : RouteBase
	{
		[SerializeField]
		SILinearTraversible _linearTraversable;

		public override float Length
		{
			get
			{
				return _linearTraversable.Value.AbsoluteLength;
			}
		}

		public ILinearTraversable LinearTraversable
		{
			get
			{
				return _linearTraversable.Value;
			}
		}

		public override Traveller GenerateNewTraveller(Traveller oldTraveller = null)
		{
			return GenerateNewTraveller(null, oldTraveller);
		}

		public Traveller GenerateNewTraveller(RouteBranch branch, Traveller oldTraveller = null)
		{
			TraversibleRouteTraveller newTraveller = new GameObject("Traveller").AddComponent<TraversibleRouteTraveller>();

			newTraveller.Assign(this, branch, oldTraveller);

			return newTraveller;
		}

		public void CheckPoints(Traveller travellerToCheck)
		{
			Traveller newTraveller = travellerToCheck;
			for (int i = 0; i < _points.Count; i++)
			{
				_points[i].TestTraveller(travellerToCheck);
			}
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
