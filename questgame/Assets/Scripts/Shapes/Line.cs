using System;
using UnityEngine;

namespace Shapes
{
	//Global functions for interacting with a line, aka a collection of points.
	public abstract class Line : MonoBehaviour, IPathable
	{
		public abstract int PointCount
		{
			get;
		}

		public abstract Vector3 this[int index, bool global = true]
		{
			get;
			set;
		}

		public abstract Vector3 GetPointOnPath(float percentage, bool worldSpace = true);


#if UNITY_EDITOR
		void OnDrawGizmos()
		{
			for (int i = 0; i < PointCount - 1; i++)
			{
				Gizmos.DrawLine(this[i], this[i + 1]);

				Gizmos.DrawSphere(this[i], UnityEditor.HandleUtility.GetHandleSize(this[i])/15);
				Gizmos.DrawSphere(this[i + 1], UnityEditor.HandleUtility.GetHandleSize(this[i + 1])/15);
			}
		}
#endif

	}
}
