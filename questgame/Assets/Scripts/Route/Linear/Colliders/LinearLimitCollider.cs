using UnityEngine;
using System.Collections;
using System;

namespace Route.Linear
{
	/*
	public class LinearLimitCollider : RouteCollider<LinearRoute>
	{
		[SerializeField]
		bool collideAgainstPositiveX;

		[SerializeField]
		protected LinearPoint _position;

		public override RoutePoint<LinearRoute> Position
		{
			get
			{
				return _position;
			}

			set
			{
				_position = value as LinearPoint;
			}
		}

		public override void Assign(RoutePoint<LinearRoute> newPosition)
		{
			Assign(newPosition, true);
		}

		public void Assign(LinearRoute newRoute, Vector2 position, bool positiveXCollide = true)
		{
			LinearPoint point = gameObject.AddComponent<LinearPoint>();
			point.Assign(newRoute, position);

			Assign(point, positiveXCollide);
		}

		public void Assign(RoutePoint<LinearRoute> newPosition, bool positiveXCollide)
		{
			_position = newPosition as LinearPoint;
			collideAgainstPositiveX = positiveXCollide;
			ParentRoute.QueueColliderForAddition(this);
		}

		public override bool TestCollider(RouteCollider<LinearRoute> collider)
		{
			throw new NotImplementedException();
		}

		public override bool TestPoint(RoutePoint<LinearRoute> point)
		{
			throw new NotImplementedException();
		}

		protected override void PointMoved()
		{
			return;
		}

		bool IsColliding(Vector2 targetPosition)
		{
			if (collideAgainstPositiveX && (targetPosition.x > _position.Position.x || Mathf.Approximately(targetPosition.x, _position.Position.x)))
			{
				return true;
			}
			if(!collideAgainstPositiveX && (targetPosition.x < _position.Position.x || Mathf.Approximately(targetPosition.x, _position.Position.x)))
			{
				return true;
			}

			return false;
		}

		public bool TestTraveller(LinearTraveller travellerToCheck)
		{
			if (IsColliding(travellerToCheck.Position))
			{
				if (!_currentlyCollidingTravellers.Contains(travellerToCheck))
				{
					TriggerOnEnter(travellerToCheck);
					_currentlyCollidingTravellers.Add(travellerToCheck);
				}
				TriggerOnCollide(travellerToCheck);
				return true;
			}
			else
			{
				if (_currentlyCollidingTravellers.Contains(travellerToCheck))
				{
					TriggerOnExit(travellerToCheck);
					_currentlyCollidingTravellers.Remove(travellerToCheck);
				}
				return false;
			}
		}
	}
	*/
}