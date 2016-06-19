using System;
using UnityEngine;

using Shapes;
using Utility;
using Route;
using Route.Linear;

namespace Level
{
	public class Branch : MonoBehaviour
	{
		//The start point
		[SerializeField]
		RouteCollider _collider;

		//The end point
		[SerializeField]
		RoutePoint _endPoint;

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
				_collider.OnCollide += OnBranchEnter;
			}
		}

		protected void SetBranch(Vector3 startPoint, Traveller traveller)
		{
			GameObject branchObject = new GameObject(name + "  temp route");
			branchObject.transform.SetParent(_collider.ParentRouteAsGeneric.transform, true);

			TwoPointLineExtended line = branchObject.AddComponent<TwoPointLineExtended>();

			line[0] = startPoint;
			line[1] = _endPoint.GetWorldSpacePosition();

			line.SetVelocity(traveller.transform.forward, _endPoint.GetVelocity());

			LinearRoute route = branchObject.AddComponent<LinearRoute>();
			route.LinearTraversable = line;

			LinearTraveller newTraveller = traveller.AddOrGetComponent<LinearTraveller>();

			traveller.UnAssign(newTraveller);

			GameObject colliderObject = new GameObject(name + " endCollider");
			colliderObject.transform.SetParent(branchObject.transform, true);

			LinearLimitCollider endCollider = colliderObject.AddComponent<LinearLimitCollider>();
			endCollider.Assign(route, new Vector2(line.AbsoluteLength, 0), true);

			endCollider.OnEnter += OnBranchExit;

			newTraveller.Assign(route, new Vector2(0,0));
			newTraveller.FixedUpdate();
		}

		protected void OnBranchExit(Traveller travellerThatIsExiting)
		{
			Destroy(travellerThatIsExiting.CurrentGenericRoute.gameObject);
			_endPoint.ParentRouteAsRouteBase.GenerateNewTraveller(travellerThatIsExiting, _endPoint);
		}

		void OnDisable()
		{
			_collider.OnCollide -= OnBranchEnter;
		}

		protected void OnBranchEnter(Traveller travellerThatEntered)
		{
			//float distanceThroughCollider = collisionPoint.x - (_collider.ColliderBounds.extents.x + _collider.ColliderBounds.center.x);

			//LinearTraveller traveleler = travellerThatEntered as LinearTraveller;

			SetBranch(travellerThatEntered.GetWorldSpacePosition(), travellerThatEntered);

			//traveleler.Assign(transportRoute, new Vector2(distanceThroughCollider, collisionPoint.y));
		}



#if UNITY_EDITOR


		protected virtual void OnDrawGizmos()
		{
			if (IsValidBranch)
			{
				Gizmos.color = Color.blue;
				Gizmos.DrawLine(_collider.RoutePointAsGeneric.GetWorldSpacePosition(), _endPoint.GetWorldSpacePosition());
			}
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
