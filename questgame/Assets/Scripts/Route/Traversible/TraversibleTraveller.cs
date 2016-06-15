using System;
using UnityEngine;

using Controllers;

namespace Route.Traversible
{
	public class TraversibleTraveller : Traveller<TraversibleRoute>
	{
		[SerializeField]
		int currentPointIndex;

		[SerializeField]
		float distanceFromIndex;

		[SerializeField]
		Vector2 _position;

		//Readonly copy of the vector
		public Vector2 Position
		{
			get
			{
				return new Vector2(_position.x, _position.y);
			}
		}

		Traveller _nextTraveller;

		[SerializeField]
		Vector2 _velocity;

		[SerializeField]
		TraversibleRoute _currentRoute;

		//properties

		public override TraversibleRoute CurrentRoute
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

		public override Traveller UpdateTraveller()
		{
			return this;
		}

		void Update()
		{
			float time = Time.deltaTime;

			distanceFromIndex += _velocity.x * time;
			_position.y += _velocity.y * time;

			CurrentRoute.CheckCollision(this, new Ray(_position, _velocity), _velocity.magnitude * time);

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
			if (currentPointIndex < _currentRoute.LinearTraversable.PointCount - 2)
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


		public override void Assign(TraversibleRoute newRoute)
		{
			Assign(newRoute, null);
		}
		//TODO
		public void Assign(TraversibleRoute newRoute, Traveller oldTraveller = null)
		{
			_currentRoute = newRoute;

			currentPointIndex = 0;
			distanceFromIndex = 0f;
		}

		public override void SetController(Controller newController)
		{
			throw new NotImplementedException();
		}

		public override void SetNextTraveller(Traveller nextTraveller)
		{
			throw new NotImplementedException();
		}


#if UNITY_EDITOR

		protected override void OnDrawGizmos()
		{
			base.OnDrawGizmos();
		}

#endif
	}
}
