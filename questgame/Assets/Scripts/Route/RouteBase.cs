using System;
using System.Collections.Generic;
using UnityEngine;

namespace Route
{
	//A route is a path a routewalker can follow
	public abstract class RouteBase : MonoBehaviour
	{
		protected HashSet<PointToBeProcessed> pointQueue = new HashSet<PointToBeProcessed>();

		//Properties
		public abstract float Length
		{
			get;
		}

		public abstract Traveller GenerateNewTraveller();

		public abstract Traveller GenerateNewTraveller(Traveller oldTraveller, RoutePoint position);

		public abstract RoutePoint GenerateNewPoint();

		protected bool canAddToQueue = true;

		public virtual void QueueColliderForRemoval(RouteCollider point)
		{
			pointQueue.Add(new PointToBeProcessed(point, false));
			if (canAddToQueue)
			{
				ModifyPoints();
			}
		}

		public virtual void QueueColliderForAddition(RouteCollider point)
		{
			pointQueue.Add(new PointToBeProcessed(point, true));
			if (canAddToQueue)
			{
				ModifyPoints();
			}
		}

		protected abstract void ModifyPoints();

		protected struct PointToBeProcessed
		{
			public RouteCollider _point;
			public bool _isAdding;

			//if isAdding is false, then we are deleting
			public PointToBeProcessed(RouteCollider point, bool isAdding)
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
