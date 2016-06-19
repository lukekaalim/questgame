using System;
using UnityEngine;

using Controllers;

namespace Route.Linear
{
	public class LinearCharacterTraveller : LinearTraveller
	{
		[SerializeField]
		float _floorHeight;

		[SerializeField]
		float _worldSpaceHeight;

		[SerializeField]
		bool _isOnGround;

		[SerializeField]
		float _velocityMinimum = 0.1f;

		[SerializeField]
		float _gravity;

		[SerializeField]
		Vector3 pointOnRoute = Vector3.zero;

		public override Traveller UpdateTraveller()
		{
			if (_activeController != null)
			{
				if (_isOnGround && _activeController.GetIntentionWeight(Intention.Tap) > 0)
				{
					_activeController.ConsumeIntention(Intention.Tap);
					_velocity.y += 3;
				}
			}
			return _nextTraveller;
		}

		public override void FixedUpdate()
		{
			float time = Time.deltaTime;

			distanceFromIndex += _velocity.x * time;

			if (_isOnGround)
			{
				_worldSpaceHeight = _floorHeight;
			}
			else
			{
				_velocity.y += Physics2D.gravity.y * _gravity * time;
			}

			if (_isOnGround && _velocity.y < 0)
			{
				_velocity.y = 0;
			}

			/*
			//Center out velocity once it passes this threshold
			if (_velocity.y < _velocityMinimum && _velocity.y > -_velocityMinimum)
			{
				_velocity.y = 0;
			}*/

			_worldSpaceHeight += _velocity.y * time;

			CurrentRoute.CheckTravellerCollision(this, new Ray(_position, _velocity), _velocity.magnitude * time);

			UpdatePosition();
		}

		public override void UpdatePosition()
		{
			float absoluteDistance;
			float progress;

			//distanceFromIndex += _velocity.x * Time.deltaTime;

			//Get Position
			Vector3 newWorldSpacePosition = _currentRoute.LinearTraversable.AdvanceAlongLine(ref currentPointIndex, ref distanceFromIndex, out absoluteDistance, out progress);

			pointOnRoute = newWorldSpacePosition;

			_floorHeight = newWorldSpacePosition.y;

			_position.y = _worldSpaceHeight - _floorHeight;

			newWorldSpacePosition.y = _worldSpaceHeight;

			if(_isOnGround)
			{
				_worldSpaceHeight = _floorHeight;
				_position.y = 0;
			}

			if (_position.y < 0)
			{
				_position.y = 0;
			}

			_position.x = absoluteDistance;

			_isOnGround = _velocity.y <= _velocityMinimum && (Mathf.Approximately(_position.y, 0) || _position.y < 0);

			transform.position = newWorldSpacePosition;

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

		public override Vector3 GetWorldSpacePosition()
		{
			return pointOnRoute;
		}
	}
}
