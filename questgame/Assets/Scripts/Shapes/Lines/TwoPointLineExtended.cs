using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shapes
{
	public class TwoPointLineExtended : TwoPointLine
	{
		[SerializeField]
		Vector3 V0 = new Vector3();

		[SerializeField]
		Vector3 V1 = new Vector3();

		public override Vector3 GetVelocityAtIndex(int startingIndex)
		{
			return startingIndex == 0 ? V0 : V1;
		}

		public void SetVelocity(Vector3 V0, Vector3 V1)
		{
			this.V0 = V0;
			this.V1 = V1;
		}

#if UNITY_EDITOR

		protected override void OnDrawGizmos()
		{
			base.OnDrawGizmos();

			Gizmos.color = Color.red;
			Gizmos.DrawLine(this[0], this[0] + V0);
			Gizmos.DrawLine(this[1], this[1] + V1);
		}
#endif
	}
}
