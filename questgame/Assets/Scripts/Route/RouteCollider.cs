using System;
using System.Collections.Generic;

using UnityEngine;

namespace Route
{
	public class RouteCollider : MonoBehaviour
	{
		public event Action<Traveller> OnEnter;

		public event Action<Traveller> OnCollide;

		public event Action<Traveller> OnExit;

		protected void TriggerOnEnter(Traveller travellerThatActivated)
		{
			OnEnter.Invoke(travellerThatActivated);
		}

		protected void TriggerOnCollide(Traveller travellerThatActivated)
		{
			OnEnter.Invoke(travellerThatActivated);
		}

		protected void TriggerOnExit(Traveller travellerThatActivated)
		{
			OnEnter.Invoke(travellerThatActivated);
		}
	}

	public abstract class RouteCollider<T> : RouteCollider where T : RouteBase
	{
		protected T _parentRoute;

		protected HashSet<Traveller> _currentlyCollidingTravellers = new HashSet<Traveller>();

		protected RoutePoint<T> _position;

		//Collision Testing Functions

		public abstract bool TestPoint(RoutePoint<T> point);

		public abstract bool TestCollider(RouteCollider<T> collider);

		public virtual void Assign(T newRoute, RoutePoint<T> newPosition)
		{
			_parentRoute = newRoute;
			_position = newPosition;
			_parentRoute.QueueColliderForAddition(this);
		}

		protected void OnEnable()
		{
			if (_parentRoute != null)
			{
				_parentRoute.QueueColliderForAddition(this);
			}

			transform.position = _position.GetWorldSpacePosition();
		}

		protected void OnDisable()
		{
			if (_parentRoute != null)
			{
				_parentRoute.QueueColliderForRemoval(this);
			}
		}

		public bool IsTravellerStillColliding(Traveller traveller)
		{
			return _currentlyCollidingTravellers.Contains(traveller);
		}
	}
}
