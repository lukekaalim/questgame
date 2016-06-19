using System;
using UnityEngine;

using Controllers;

namespace Route.Linear
{
	public class LinearTraveller : Traveller<LinearRoute>
	{
		[SerializeField]
		protected int currentPointIndex = 0;

		[SerializeField]
		protected float distanceFromIndex = 0;

		[SerializeField]
		protected Vector2 _position = Vector3.zero;

		//Readonly copy of the vector
		public Vector2 Position
		{
			get
			{
				return new Vector2(_position.x, _position.y);
			}
		}

		[SerializeField]
		protected Vector2 _velocity = Vector3.zero;

		[SerializeField]
		protected LinearRoute _currentRoute;

		//properties
		public override LinearRoute CurrentRoute
		{
			get
			{
				return _currentRoute;
			}
		}

		public override bool IsAssigned
		{
			get
			{
				return _currentRoute == null;
			}
		}

		public override IControllable ApplyInput(Intention controllerIntent)
		{
			return _nextTraveller;
		}

		protected virtual void OnEnable()
		{
			_nextTraveller = this;
		}

		public virtual void FixedUpdate()
		{
			float time = Time.fixedDeltaTime;

			distanceFromIndex += _velocity.x * time;
			_position.y += _velocity.y * time;

			CurrentRoute.CheckTravellerCollision(this, new Ray(_position, _velocity), _velocity.magnitude * time);

			UpdatePosition();
		}

		public override void UpdatePosition()
		{
			float absoluteDistance;
			float progress;

			//distanceFromIndex += _velocity.x * Time.deltaTime;

			//Get Position
			transform.position = _currentRoute.LinearTraversable.AdvanceAlongLine(ref currentPointIndex, ref distanceFromIndex, out absoluteDistance, out progress) + new Vector3(0,_position.y);

			_position.x = absoluteDistance;

			//Get Rotation
			Vector3 lerpedDirection;
			if (currentPointIndex < _currentRoute.LinearTraversable.PointCount)
			{
				lerpedDirection = Vector3.Lerp(
					_currentRoute.LinearTraversable.GetVelocityAtIndex(currentPointIndex),
					_currentRoute.LinearTraversable.GetVelocityAtIndex(currentPointIndex + 1),
					progress
					);
			}
			else
			{
				lerpedDirection = _currentRoute.LinearTraversable.GetVelocityAtIndex(currentPointIndex);
			}

			transform.rotation = Quaternion.LookRotation(lerpedDirection, Vector3.up);
		}


		public override void Assign(LinearRoute newRoute)
		{
			Assign(newRoute, null);
		}

		public void Assign(LinearRoute newRoute, Vector2 startingPosition)
		{
			_currentRoute = newRoute;

			currentPointIndex = 0;
			distanceFromIndex = startingPosition.x;
			_position.y = startingPosition.y;

			_nextTraveller = this;
			enabled = true;

			UpdatePosition();
		}

		public override void Assign(LinearRoute newRoute, RoutePoint<LinearRoute> position)
		{
			LinearPoint point = position as LinearPoint;

			Assign(newRoute, point.Position);
		}

		public override void UnAssign(Traveller nextTraveller)
		{
			_currentRoute = null;
			_nextTraveller = nextTraveller;
			enabled = false;
		}

		public override Vector3 GetWorldSpacePosition()
		{
			return transform.position;
		}


#if UNITY_EDITOR

		protected override void OnDrawGizmos()
		{
			base.OnDrawGizmos();
		}

#endif
	}
}
