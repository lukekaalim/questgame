using System;
using UnityEngine;

namespace Route.Linear
{
	/*
	[ExecuteInEditMode]
	public class LinearEndPoint : LinearPoint
	{
		public override void Assign(LinearRoute newRoute)
		{
			base.Assign(newRoute);
			SetToEnd();
		}

		void SetToEnd()
		{
			_leftIndex = 0;

			_distanceFromIndex = _parent.LinearTraversable.AbsoluteLength;

			_parent.LinearTraversable.AdvanceAlongLine(ref _leftIndex, ref _distanceFromIndex, out _position.x, out _progress);
			OnPointMoveInvoke();
		}

		void OnEnable()
		{
			if (_parent != null)
			{
				SetToEnd();
			}
		}

#if UNITY_EDITOR

		protected override void OnDrawGizmos()
		{
			base.OnDrawGizmos();
			if (_parent != null)
			{
				SetToEnd();
			}
		}
#endif
	}
	*/
}
