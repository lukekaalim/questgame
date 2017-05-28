using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using Extensions;

namespace Shapes
{
	// A continuous line composed of multiple points that passes through each linenearly
	// each coninuous points comporises of a 'segment' in the compound line
	// It has a canonical 'start' and 'end', and each point can be measured by its 'distance' from the start
	public class CompoundLine : ScriptableObject
	{
		public struct Position
		{
			public readonly int segmentIndex;
			public readonly float offset;

			public Position(int segmentIndex, float offset)
			{
				this.segmentIndex = segmentIndex;
				this.offset = offset;
			}
		}

		[SerializeField]
		List<Vector3> points = new List<Vector3>(new Vector3[2]);

		[SerializeField]
		float totalLength = 0f;

#if UNITY_EDITOR
		[SerializeField]
		Color lineColor = Color.white;
#endif

		// The length of each segment
		// 1: distance(point[0], point[1]), 2: distance(point[1], point[2]), etc... 
		[SerializeField]
		List<float> segmentLengths = new List<float>();

		// Distance refers to the continuous space from this point and the beginning of the line
		// in a Line where every point is 5 units away from the previous
		// 1: [0], 2: [5], 3: [10], 4: [15], 5: [20]
		[SerializeField]
		List<float> pointDistance = new List<float>(); 
		
		public float TotalLength { get { return totalLength; } }

		public List<Vector3> Points { get { return points; } set { points = value; } }

		public float GetDistancePercentage(float distance)
		{
			return distance / totalLength;
		}

		public Vector3 GetPositionFromDistance(float distance)
		{
			float remaningDistance;

			if (distance >= totalLength)
			{
				remaningDistance = totalLength - distance;
				Vector3 lastPoint = points[points.Count - 1];
				Vector3 secondLastPoint = points[points.Count - 2];

				return Vector3.LerpUnclamped(lastPoint, secondLastPoint, remaningDistance);
			}

			if(distance <= 0)
			{
				Vector3 firstPoint = points[0];
				Vector3 secondPoint = points[1];

				return Vector3.LerpUnclamped(firstPoint, secondPoint, distance);
			}

			int firstPointIndex = FindFirstSegmentPoint(pointDistance, distance);
			remaningDistance = distance - pointDistance[firstPointIndex];
			float lengthOfLine = segmentLengths[firstPointIndex];

			return Vector3.Lerp(points[firstPointIndex], points[firstPointIndex + 1], remaningDistance / lengthOfLine);
		}

		public Vector3 GetPointFromPercentage(float percentage)
		{
			percentage = Mathf.Clamp01(percentage);
			float position = (points.Count - 1) * percentage;
			float offset = position % 1;
			int index = Mathf.FloorToInt(position - offset);

			if (index == points.Count - 1)
			{
				return points[index];
			}

			return Vector3.Lerp(points[index], points[index + 1], offset);
		}

		public Position GetValidPosition(Position position)
		{
			float lengthOfSegment = segmentLengths[position.segmentIndex];

			float offset = position.offset;
			int segmentIndex = position.segmentIndex;

			Position resolvedPosition = position;

			while(offset < 0f || offset > lengthOfSegment)
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

		public Vector3 GetResolvedPosition(Position position)
		{
			float percentage = position.offset / segmentLengths[position.segmentIndex];

			Vector3 firstPosition = points[position.segmentIndex];
			Vector3 secondPosition = points[position.segmentIndex + 1];

			return Vector3.LerpUnclamped(firstPosition, secondPosition, percentage);
		}
		
		public Vector3 GetVelocityAtIndex(int startingIndex)
		{
			if (startingIndex == points.Count - 1)
			{
				return points[startingIndex] - points[startingIndex - 1];
			}
			return points[startingIndex + 1] - points[startingIndex];
		}

		public Vector3 GetVelocityAlongDistance(float distance)
		{
			int index = FindFirstSegmentPoint(pointDistance, distance);
			return GetVelocityAtIndex(index);
		}

		public void RecalculateLength()
		{
			segmentLengths.Clear();
			pointDistance.Clear();
			totalLength = 0;

			for (int i = 0; i < points.Count - 1; i++)
			{
				float length = Vector3.Distance(points[i], points[i + 1]);
				segmentLengths.Add(length);
				pointDistance.Add(totalLength);
				totalLength += length;
			}
		}

		int GetLeftMostIndex(float distance)
		{
			float traversed = 0;
			int index = 0;
			while (traversed < totalLength)
			{
				float newTraversed = traversed + segmentLengths[index];
				if (newTraversed >= distance)
				{
					return index;
				}
				else
				{
					traversed = newTraversed;
					index++;
				}
			}
			return -1;
		}

		// TODO handle cases where the distance is directly on a pivot
		static int FindFirstSegmentPoint(List<float> distances, float targetDistance)
		{
			// first, we want to find a distance that is longer than our target.
			int min = 0;
			int max = distances.Count - 1;
			// Binary search
			int pivot = (min + max) / 2; // put pivot in center
			while (min <= max)
			{
				if (distances[pivot] < targetDistance)
				{
					min = pivot + 1;
					pivot = (min + max) / 2;
				}
				else if (distances[pivot] > targetDistance)
				{
					if (distances[pivot - 1] <= targetDistance)
					{
						return pivot - 1;
					}
					else
					{
						max = pivot - 1;
						pivot = (min + max) / 2;
					}
				}
			}
			return -1;
		}

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
