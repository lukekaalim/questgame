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
		protected List<RoutePoint> _points;

		public List<RoutePoint> Points
		{
			get
			{
				return _points;
			}
		}

		//Properties
		public abstract float Length
		{
			get;
		}

		public abstract Traveller GenerateNewTraveller(Traveller oldTraveller = null);
	}
}
