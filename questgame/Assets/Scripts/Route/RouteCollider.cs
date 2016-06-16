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
		[SerializeField]
		protected T _parentRoute;

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
			_parentRoute = Position.ParentRoute;

			if (_parentRoute != null)
			{
				_parentRoute.QueueColliderForAddition(this);
			}

			if (Position == null)
			{
				Position = GetComponent<RoutePoint<T>>();
			}
			if (Position != null)
			{
				Position.OnPointMove += PointMoved;
			}

			transform.position = Position.GetWorldSpacePosition();
		}


		protected void OnDisable()
		{
			if (_parentRoute != null)
			{
				_parentRoute.QueueColliderForRemoval(this);
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
	}
}
