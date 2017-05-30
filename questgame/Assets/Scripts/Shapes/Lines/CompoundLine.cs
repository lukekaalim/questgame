using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using Utility;

namespace Shapes
{
	// A continuous line composed of multiple points that passes through each linenearly
	// each coninuous points comporises of a 'segment' in the compound line
	// It has a canonical 'start' and 'end', and each point can be measured by its 'distance' from the start
	public class CompoundLine : ScriptableObject
	{
		[Serializable]
		public struct Position
		{
			public readonly int segmentIndex;
			public readonly float offset;

			public Position(int segmentIndex, float offset)
			{
				this.segmentIndex = segmentIndex;
				this.offset = offset;
			}

			public static Position operator +(Position position, float offset)
			{
				return new Position(position.segmentIndex, position.offset + offset);
			}
		}

		[SerializeField]
		List<Vector3> points = new List<Vector3>(new Vector3[2]);

		[SerializeField]
		float totalLength = 0f;

		// The length of each segment
		// 1: distance(point[0], point[1]), 2: distance(point[1], point[2]), etc... 
		[SerializeField]
		List<float> segmentLengths = new List<float>();

		// Distance refers to the continuous space from this point and the beginning of the line
		// in a Line where every point is 5 units away from the previous
		// 1: [0], 2: [5], 3: [10], 4: [15], 5: [20]
		[SerializeField]
		List<float> pointDistances = new List<float>();

		public float TotalLength { get { return totalLength; } }

		public List<Vector3> Points { get { return points; } set { points = value; } }

		public Position GetPosition(float distance)
		{
			float offset;
			int index;

			if (distance >= totalLength)
			{
				offset = totalLength - distance;
				index = points.Count - 2;
			}
			else if (distance <= 0)
			{
				index = 0;
				offset = distance;
			}
			else
			{
				index = Search.Binary(pointDistances, distance, FindFirstSegmentComparer);
				offset = distance - pointDistances[index];
			}
			return new Position(index, offset);
		}

		public Position UpdatePosition(Position position)
		{
			float lengthOfSegment = segmentLengths[position.segmentIndex];

			float offset = position.offset;
			int segmentIndex = position.segmentIndex;

			Position resolvedPosition = position;

			while (offset < 0f || offset > lengthOfSegment)
			{
				if (segmentIndex <= 0 || segmentIndex >= segmentLengths.Count - 2)
				{
					break;
				}

				if (offset < 0f)
				{
					segmentIndex--;
					lengthOfSegment = segmentLengths[segmentIndex];
					offset += lengthOfSegment;
				}

				if (offset > lengthOfSegment)
				{
					segmentIndex++;
					lengthOfSegment = segmentLengths[segmentIndex];
					offset -= lengthOfSegment;
				}

				resolvedPosition = new Position(segmentIndex, offset);
			}
			return resolvedPosition;
		}

		public Vector3 ResolvePosition(Position position)
		{
			float percentage = position.offset / segmentLengths[position.segmentIndex];

			Vector3 firstPosition = points[position.segmentIndex];
			Vector3 secondPosition = points[position.segmentIndex + 1];

			if (percentage == 0)
			{
				return firstPosition;
			}
			else if (percentage == 1)
			{
				return secondPosition;
			}
			else
			{
				return Vector3.LerpUnclamped(firstPosition, secondPosition, percentage);
			}
		}

		public Vector3 GetVelocityAtIndex(int startingIndex)
		{
			if (startingIndex == points.Count - 1)
			{
				return points[startingIndex] - points[startingIndex - 1];
			}
			return points[startingIndex + 1] - points[startingIndex];
		}

		int FindFirstSegmentComparer(List<float> list, int index, float target)
		{
			float value = list[index];

			if (value < target)
			{
				if (list.IsLastIndex(index) || list[index + 1] > target)
				{
					return 0;
				}
				return -1;
			}
			else if (value > target)
			{
				if (list.IsFirstIndex(index))
				{
					return 0;
				}
				return 1;
			}
			else
			{
				return 0;
			}
		}

#if UNITY_EDITOR

		[SerializeField]
		Color lineColor = Color.white;

		public static List<CompoundLine> enabledCompoundLines = new List<CompoundLine>();

		public void RecalculateLength()
		{
			segmentLengths.Clear();
			pointDistances.Clear();
			totalLength = 0;

			for (int i = 0; i < points.Count - 1; i++)
			{
				float length = Vector3.Distance(points[i], points[i + 1]);
				segmentLengths.Add(length);
				pointDistances.Add(totalLength);
				totalLength += length;
			}
		}

		private void OnEnable()
		{
			enabledCompoundLines.Add(this);
		}

		private void OnDisable()
		{
			enabledCompoundLines.Remove(this);
		}
#endif
		/*
		 *	Need to revist these functions later
		 * 
		public List<Vector3> GetPointSample(float startPosition, float endPosition)
		{
			List<Vector3> samples = new List<Vector3>();
			int index = 0;
			samples.Add(AdvanceAlongLine(ref index, ref startPosition));
			while (index < PointCount - 3 && pointDistance[index + 1] < endPosition)
			{
				index++;
				samples.Add(this[index]);
			}
			samples.Add(GetPointAlongDistance(endPosition));

			return samples;
		}
		*/

		/*
		public static CompoundLine CreateCompoundLineFromPath(IPathable path, int segments)
		{
			GameObject gameObject = new GameObject("Compound Line");
			CompoundLine line = gameObject.AddComponent<CompoundLine>();

			for (int i = 0; i < segments + 1; i++)
			{
				float postitionInCurve = i / (float)segments;
				line.AddPoint(path.GetPointOnPath(postitionInCurve));
			}

			return line;
		}*/
	}
}
