using System;
using UnityEngine;

using Shapes;

namespace Route.Linear
{
	[ExecuteInEditMode]
	public class LinearBranch : MonoBehaviour
	{
		[SerializeField]
		LinearCollider _collider;

		[SerializeField]
		LinearPoint _endPoint;

		public bool IsValidBranch
		{
			get
			{
				return _collider != null && _endPoint != null;
			}
		}

		void OnEnable()
		{
			if (IsValidBranch)
			{
				_collider.OnDetailedCollide += OnBranchEnter;
			}
		}

		void CreateBranch()
		{

		}

		void OnDisable()
		{
			_collider.OnDetailedCollide -= OnBranchEnter;
		}

		void OnBranchEnter(Traveller travellerThatEntered, Vector2 collisionPoint)
		{
			//float distanceThroughCollider = collisionPoint.x - (_collider.ColliderBounds.extents.x + _collider.ColliderBounds.center.x);

			//LinearTraveller traveleler = travellerThatEntered as LinearTraveller;

			CreateBranch();

			//traveleler.Assign(transportRoute, new Vector2(distanceThroughCollider, collisionPoint.y));
		}



#if UNITY_EDITOR


		void OnGizmos()
		{

		}

		/*
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
		*/
#endif

	}
}
