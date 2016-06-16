using System;
using UnityEngine;

using Route;

namespace Level
{
	public class Branch : MonoBehaviour
	{
		[SerializeField]
		public RoutePoint _startingPoint;
		[SerializeField]
		public RoutePoint _endingPoint;
		[SerializeField]
		public RouteCollider _collider;

		void OnEnable()
		{
			_collider.OnCollide += OnCollision;
		}

		void OnCollision(Traveller travellerThatHit)
		{

		}


#if UNITY_EDITOR

		[UnityEditor.MenuItem("Level/Branch")]
		static void CreateNewBranch(UnityEditor.MenuCommand menuCommand)
		{
			// Create a custom game object
			GameObject gameObject = new GameObject("Branch");
			Branch newBranch = gameObject.AddComponent<Branch>();

			if (UnityEditor.Selection.activeGameObject != null)
			{
				RouteBase selectedRoute = UnityEditor.Selection.activeGameObject.GetComponent<RouteBase>();
				if (selectedRoute != null)
				{
					RoutePoint newPoint = selectedRoute.GenerateNewPoint();
					newBranch._startingPoint = newPoint;
				}
				else
				{
					RoutePoint selectedPoint = UnityEditor.Selection.activeGameObject.GetComponent<RoutePoint>();
					if (selectedPoint != null)
					{
						newBranch._startingPoint = selectedPoint;
					}
				}
			}

			// Ensure it gets reparented if this was a context click (otherwise does nothing)
			UnityEditor.GameObjectUtility.SetParentAndAlign(gameObject, menuCommand.context as GameObject);
			// Register the creation in the undo system
			UnityEditor.Undo.RegisterCreatedObjectUndo(gameObject, "Create " + gameObject.name);
		}

#endif
	}
}
