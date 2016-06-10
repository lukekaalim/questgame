using System;
using System.Collections.Generic;
using UnityEngine;

namespace Route
{
	//A route is a path a routewalker can follow
	public abstract class RouteBase : MonoBehaviour
	{
		//Internal Variabes
		[SerializeField]
		protected HashSet<RoutePoint> _points = new HashSet<RoutePoint>();

		public HashSet<RoutePoint> Points
		{
			get
			{
				return _points;
			}
		}

		protected HashSet<PointToBeProcessed> pointQueue = new HashSet<PointToBeProcessed>();

		//Properties
		public abstract float Length
		{
			get;
		}

		public abstract Traveller GenerateNewTraveller();

		public abstract RoutePoint GenerateNewPoint();

		public void QueuePointForRemoval(RoutePoint point)
		{
			pointQueue.Add(new PointToBeProcessed(point, false));
		}

		public void QueuePointForAddition(RoutePoint point)
		{
			pointQueue.Add(new PointToBeProcessed(point, true));
		}

		protected void ModifyPoints()
		{
			foreach (PointToBeProcessed pointModification in pointQueue)
			{
				if (pointModification._isAdding)
				{
					_points.Add(pointModification._point);
				}
				else
				{
					_points.Remove(pointModification._point);
				}
			}
			pointQueue.Clear();
		}

		protected struct PointToBeProcessed
		{
			public RoutePoint _point;
			public bool _isAdding;

			//if isAdding is false, then we are deleting
			public PointToBeProcessed(RoutePoint point, bool isAdding)
			{
				_point = point;
				_isAdding = isAdding;
			}

			public override int GetHashCode()
			{
				return _point.GetHashCode();
			}

			public override bool Equals(object obj)
			{
				return _point.Equals(obj);
			}
		}
	}
}
