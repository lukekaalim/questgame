using System;
using UnityEngine;

namespace Route
{
	public class TraversibleRouteTraveller : Traveller<TraversibleRoute>
	{
		[SerializeField]
		int currentPointIndex;

		[SerializeField]
		float distanceFromIndex;

		float _absoluteDistance;
		float _absoluteDistanceLastFrame;

		Traveller _nextTraveller;

		[SerializeField]
		float naturalVelocity;

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

		public float AbsoluteDistance
		{
			get
			{
				return _absoluteDistance;
			}
		}

		public float AbsoluteDistanceLastFrame
		{
			get
			{
				return _absoluteDistanceLastFrame;
			}
		}

		public override Traveller UpdateTraveller()
		{
			CurrentRoute.CheckPoints(this);

			return this;
		}

		void Update()
		{
			_absoluteDistanceLastFrame = _absoluteDistance;

			transform.position = _currentRoute.LinearTraversable.AdvanceAlongLine(currentPointIndex, distanceFromIndex + naturalVelocity * Time.deltaTime, out currentPointIndex, out distanceFromIndex, out _absoluteDistance);

			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(_currentRoute.LinearTraversable.GetVelocityAtIndex(currentPointIndex), Vector3.up), 5f * Time.deltaTime);

			CurrentRoute.CheckPoints(this);
		}

		public override void UpdatePosition()
		{

		}


		public override void Assign(TraversibleRoute newRoute)
		{
			Assign(newRoute, null, null);
		}

		public void Assign(TraversibleRoute newRoute, RouteBranch branch = null, Traveller oldTraveller = null)
		{
			_currentRoute = newRoute;
			if (branch == null)
			{
				currentPointIndex = 0;
				distanceFromIndex = 0f;
				_absoluteDistance = 0f;
			}
		}

		public override void SetController(Controller newController)
		{
			throw new NotImplementedException();
		}

		public override void SetNextTraveller(Traveller nextTraveller)
		{
			throw new NotImplementedException();
		}
	}
}
