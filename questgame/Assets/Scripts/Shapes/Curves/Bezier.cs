using System.Collections.Generic;
using UnityEngine;

namespace Shapes
{
	public static class Bezier
	{
		public static Vector3 GetPoint(List<Vector3> controlPoints, float position)
		{
			if (controlPoints == null || controlPoints.Count == 0)
			{
				return Vector3.zero;
			}

			if (controlPoints.Count == 1)
			{
				return controlPoints[0];
			}

			position = Mathf.Clamp(position, 0, 1);

			List<Vector3> remainingPoints = new List<Vector3>(controlPoints);

			while (remainingPoints.Count > 1)
			{
				List<Vector3> newPoints = new List<Vector3>();

				for (int i = 0; i < remainingPoints.Count - 1; i++)
				{
					newPoints.Add(Vector3.Lerp(remainingPoints[i], remainingPoints[i + 1], position));
				}
				remainingPoints = newPoints;
			}

			return remainingPoints[0];
		}

		public static Vector3 GetVelocity(List<Vector3> controlPoints, float position)
		{
			if (controlPoints == null || controlPoints.Count == 0)
			{
				return Vector3.zero;
			}

			if (controlPoints.Count == 1)
			{
				return controlPoints[0];
			}

			return Vector3.zero;
		}

		public static Vector3[] GetNextSetOfControlPoints(IList<Vector3> controlPoints, float position)
		{
			if (controlPoints == null || controlPoints.Count == 0)
			{
				return new Vector3[0];
			}

			if (controlPoints.Count == 1)
			{
				return new Vector3[] { controlPoints[0] };
			}
			position = Mathf.Clamp(position, 0, 1);

			Vector3[] newPoints = new Vector3[controlPoints.Count - 1];
			for (int i = 0; i < controlPoints.Count - 1; i++)
			{
				newPoints[i] = (Vector3.Lerp(controlPoints[i], controlPoints[i + 1], position));
			}
			return newPoints;
		}

		public static Vector3 GetPoint(QuadraticBezierControlPointSet controlPoints, float position)
		{
			position = Mathf.Clamp01(position);
			float oneMinusT = 1f - position;
			return
				oneMinusT * oneMinusT * oneMinusT * controlPoints[0] +
				3f * oneMinusT * oneMinusT * position * controlPoints[1] +
				3f * oneMinusT * position * position * controlPoints[2] +
				position * position * position * controlPoints[3];
		}

		public static Vector3 GetVelocity(QuadraticBezierControlPointSet controlPoints, float position)
		{
			position = Mathf.Clamp01(position);
			float oneMinusT = 1f - position;
			return
				3f * oneMinusT * oneMinusT * (controlPoints[1] - controlPoints[0]) +
				6f * oneMinusT * position * (controlPoints[2] - controlPoints[1]) +
				3f * position * position * (controlPoints[3] - controlPoints[2]);
		}

		public struct QuadraticBezierControlPointSet
		{
			Vector3[] _controlPoints;

			public Vector3[] ControlPoints
			{
				get
				{
					return _controlPoints;
				}
			}

			public Vector3 this[int index]
			{
				get
				{
					return _controlPoints[index];
				}
				set
				{
					_controlPoints[index] = value;
				}
			}

			public QuadraticBezierControlPointSet(Vector3 point1, Vector3 point2, Vector3 point3, Vector3 point4)
			{
				_controlPoints = new Vector3[4];

				_controlPoints[0] = point1;
				_controlPoints[1] = point2;
				_controlPoints[2] = point3;
				_controlPoints[3] = point4;
			}
		}
	}
}
