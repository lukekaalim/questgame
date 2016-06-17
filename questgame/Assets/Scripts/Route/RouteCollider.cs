using System;
using System.Collections.Generic;

using UnityEngine;

namespace Route
{
	[ExecuteInEditMode]
	public class RouteCollider : MonoBehaviour
	{
		public event Action<Traveller> OnEnter;
		public event Action<Traveller> OnCollide;
		public event Action<Traveller> OnExit;

		protected void TriggerOnEnter(Traveller travellerThatActivated)
		{
			if (OnEnter != null)
			{
				OnEnter.Invoke(travellerThatActivated);
			}
		}

		protected void TriggerOnCollide(Traveller travellerThatActivated)
		{
			if (OnCollide != null)
			{
				OnCollide.Invoke(travellerThatActivated);
			}
		}

		protected void TriggerOnExit(Traveller travellerThatActivated)
		{
			if (OnExit != null)
			{
				OnExit.Invoke(travellerThatActivated);
			}
		}
	}

	public abstract class RouteCollider<T> : RouteCollider where T : RouteBase
	{
		protected T ParentRoute
		{
			get
			{
				return Position.ParentRoute;
			}
		}

		protected HashSet<Traveller> _currentlyCollidingTravellers = new HashSet<Traveller>();

		public abstract RoutePoint<T> Position
		{
			get;
			set;
		}

		//Collision Testing Functions

		public abstract bool TestPoint(RoutePoint<T> point);

		public abstract bool TestCollider(RouteCollider<T> collider);

		public abstract void Assign(RoutePoint<T> newPosition);

		protected abstract void PointMoved();

		protected void OnEnable()
		{
			if (Position == null)
			{
				Position = GetComponent<RoutePoint<T>>();
			}
			if (Position != null)
			{
				Position.OnPointMove += PointMoved;
			}

			if (ParentRoute != null)
			{
				ParentRoute.QueueColliderForAddition(this);
			}

			transform.position = Position.GetWorldSpacePosition();
		}


		protected void OnDisable()
		{
			if (ParentRoute != null)
			{
				ParentRoute.QueueColliderForRemoval(this);
			}
			if (Position != null)
			{
				Position.OnPointMove -= PointMoved;
			}
		}

		public bool IsTravellerStillColliding(Traveller traveller)
		{
			return _currentlyCollidingTravellers.Contains(traveller);
		}

		/*
		protected void TriggerOnDetailedEnter(Traveller travellerThatActivated, RoutePoint<T> collisionPoint)
		{
			if (OnDetailedEnter != null)
			{
				OnDetailedEnter.Invoke(travellerThatActivated, collisionPoint);
			}
		}

		protected void TriggerOnDetailedCollide(Traveller travellerThatActivated, RoutePoint<T> collisionPoint)
		{
			if (OnDetailedCollide != null)
			{
				OnDetailedCollide.Invoke(travellerThatActivated, collisionPoint);
			}
		}

		protected void TriggerOnDetailedExit(Traveller travellerThatActivated)
		{
			if (OnDetailedExit != null)
			{
				OnDetailedExit.Invoke(travellerThatActivated);
			}
		}
		*/
	}
}
