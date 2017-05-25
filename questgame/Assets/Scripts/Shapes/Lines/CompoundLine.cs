using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Shapes
{
	// A continuous line composed of multiple points that passes through each linenearly
	// each coninuous points comporises of a 'segment' in the compound line
	// It has a canonical 'start' and 'end', and each point can be measured by its 'distance' from the start
	[CreateAssetMenu(fileName = "newCompoundLine", menuName = "CompoundLine", order = 0)]
	public class CompoundLine : ScriptableObject
	{
		[SerializeField]
		List<Vector3> points = new List<Vector3>();

		[SerializeField]
		bool invalidLineLengths = false;

		[SerializeField]
		float totalLength = 0;

		[SerializeField]
		Color lineColor = Color.white;
		
		// The length of each segment
		// 1: distance(point[0], point[1]), 2: distance(point[1], point[2]), etc... 
		[SerializeField]
		protected List<float> segmentLengths = new List<float>();

		// Distance refers to the continuous space from this point and the beginning of the line
		// in a Line where every point is 5 units away from the previous
		// 1: [0], 2: [5], 3: [10], 4: [15], 5: [20]
		[SerializeField]
		protected List<float> pointDistance = new List<float>(); 

		public int PointCount { get { return points.Count; } }
		
		public List<float> GetSegmentLengths()
		{
			RecalculateLength();
			return segmentLengths;
		}

		public float GetTotalLength()
		{
			RecalculateLength();
			return totalLength;
		}

		public Vector3 GetPoint(int index)
		{
			return points[index];
		}

		public float GetDistancePercentage(float distance)
		{
			return distance / GetTotalLength();
		}

		public void SetPoint(int index, Vector3 newPosition)
		{
			points[index] = newPosition;
			invalidLineLengths = true;
		}

		public void InsertPoint(Vector3 newPoint, int index)
		{
			points.Insert(index, newPoint);
			invalidLineLengths = true;
		}

		public void InsertPoints(IEnumerable<Vector3> newPoints, int index)
		{
			points.InsertRange(index, newPoints);
			invalidLineLengths = true;
		}

		public void RemovePoint(int index)
		{
			points.RemoveAt(index);
			invalidLineLengths = true;
		}

		// TODO handle cases where the distance is directly on a pivot
		static int FindFirstSegmentPoint(List<float> distances, float targetDistance)
		{
			// first, we want to find a distance that is longer than our target.
			int min = 0;
			int max = distances.Count - 1;
			// Binary search
			int pivot = (min + max) / 2; // put pivot in center
			bool found = false;
			while (!found) // while the pivot is still to the right somewhere
			{
				if (distances[pivot] < targetDistance)
				{
					min = pivot;
					pivot = (min + max) / 2;
				}
				else if (distances[pivot] > targetDistance)
				{
					if (distances[pivot - 1] <= targetDistance)
					{
						found = true;
						return pivot - 1;
					}
					else
					{
						max = pivot;
						pivot = (min + max) / 2;
					}
				}
			}
			throw new Exception("You shouldn't be able to break out of the loop without finding an element");
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

		public void RecalculateLength()
		{
			if(!invalidLineLengths)
			{
				return;
			}

			segmentLengths.Clear();
			pointDistance.Clear();
			totalLength = 0;

			for (int i = 0; i < points.Count - 1; i ++)
			{
				float length = Vector3.Distance(points[i], points[i + 1]);
				segmentLengths.Add(length);
				pointDistance.Add(totalLength);
				totalLength += length;
			}
			invalidLineLengths = false;
		}

		public Vector3 GetPointAlongDistance(int startingIndex, float distanceFromIndex)
		{
			int index = startingIndex;
			float distance = distanceFromIndex;
			return AdvanceAlongLine(ref index, ref distance);
		}

		//Important Function!!!
		public Vector3 AdvanceAlongLine(ref int index, ref float distance, out float newTotalDistance, out float pointProgress)
		{
			//If distance is a positive number
			if (distance > 0)
			{
				//While you haven't reached the end of the array
				//and you're distance is still greater than the length of the line.
				while (index < PointCount - 2 && LineLengths[index] <= distance)
				{
					//Subtract the distance
					distance -= LineLengths[index];
					//And start again from the next line
					index++;
				}
			}
			//if Distance is a negative number
			else if(distance < 0)
			{
				while (index > 0 && LineLengths[index - 1] < -distance)
				{
					distance += LineLengths[index - 1];
					index--;
				}
				if (index > 0)
				{
					index--;
					distance = LineLengths[index] + distance;
				}
				else
				{
					index = 0;
				}
			}

			newTotalDistance = pointDistance[index] + distance;

			pointProgress = distance / LineLengths[index];

			return Vector3.LerpUnclamped(this[index], this[index + 1], pointProgress);
		}

		public Vector3 AdvanceAlongLine(ref int index, ref float distance, out float newTotalDistance)
		{
			if (distance > 0)
			{
				while (index < PointCount - 2 && LineLengths[index] <= distance)
				{
					distance -= LineLengths[index];
					index++;
				}
			}
			else if (distance < 0)
			{
				while (index > 0 && LineLengths[index - 1] < -distance)
				{
					distance += LineLengths[index - 1];
					index--;
				}
				if (index > 0)
				{
					index--;
					distance = LineLengths[index] + distance;
				}
				else
				{
					index = 0;
				}
			}

			newTotalDistance = pointDistance[index] + distance;
			return Vector3.LerpUnclamped(this[index], this[index + 1], distance / LineLengths[index]);
		}

		public Vector3 AdvanceAlongLine(ref int index, ref float distance)
		{
			if (distance > 0)
			{
				while (index < PointCount - 2 && LineLengths[index] <= distance)
				{
					distance -= LineLengths[index];
					index++;
				}
			}
			else if (distance < 0)
			{
				while (index > 0 && distance < 0 && LineLengths[index - 1] < -distance)
				{
					Debug.Log(distance);
					Debug.Log(LineLengths[index - 1]);

					distance += LineLengths[index - 1];
					index--;
				}
				if (index > 0)
				{
					index--;
					distance = LineLengths[index] + distance;
				}
				else
				{
					index = 0;
				}
			}
			return Vector3.LerpUnclamped(this[index], this[index + 1], distance / LineLengths[index]);
		}

		public Vector3 GetPointOnPath(float percentage, bool worldSpace = true)
		{
			percentage = Mathf.Clamp01(percentage);
			float position = (points.Count - 1) * percentage;
			float offset = position % 1;
			int index = Mathf.FloorToInt(position - offset);

			if (index == points.Count - 1)
			{
				return points[index];
			}

			return Vector3.Lerp(points[index], points[index+1], offset);
		}

		/*
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

		public int GetLeftMostIndex(float distance)
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

		/*
		public Vector3 GetVelocityAtIndex(int startingIndex)
		{
			if (startingIndex == PointCount - 1)
			{
				return this[startingIndex] - this[startingIndex - 1];
			}
			return this[startingIndex + 1] - this[startingIndex];
		}
		*/

		public Vector3 GetStartPosition()
		{
			return points[0];
		}

		public Vector3 GetEndPosition()
		{
			return points[points.Count - 1];
		}

		public Vector3 GetRelativePoint(float distance)
		{
			return GetPointOnPath(distance, true);
		}

		/*
		public Vector3 GetVelocity(float percentage, bool worldSpace = true)
		{
			int index = GetLeftMostIndex(percentage);
			return worldSpace ? transform.TransformDirection(GetVelocityAtIndex(index)) : GetVelocityAtIndex(index);
		}
		*/

		// Internal position struct
		public struct Position
		{
			int pointIndex;
			float offset;

			public Position(int pointIndex, float offset)
			{
				this.pointIndex = pointIndex;
				this.offset = offset;
			}

			public bool IsOverflowing()
			{
				return offset >= 0f && offset <= 1f;
			}
		}



#if UNITY_EDITOR
		/*
		[UnityEditor.MenuItem("GameObject/Shapes/Lines/Compound", false, 10)]
		static void CreateCustomGameObject(UnityEditor.MenuCommand menuCommand)
		{
			// Create a custom game object
			GameObject gameObject = new GameObject("Compound Line");
			// gameObject.AddComponent<CompoundLine>();

			// Ensure it gets reparented if this was a context click (otherwise does nothing)
			UnityEditor.GameObjectUtility.SetParentAndAlign(gameObject, menuCommand.context as GameObject);
			// Register the creation in the undo system
			UnityEditor.Undo.RegisterCreatedObjectUndo(gameObject, "Create " + gameObject.name);
		}

		[UnityEditor.MenuItem("GameObject/Shapes/Lines/Create From Selected Path", false, 10)]
		public static void CreateFromPathMenu()
		{
			IPathable path = UnityEditor.Selection.activeGameObject.GetComponent<IPathable>();
			if (path != null)
			{
				int levelOfDetail = UnityEditor.EditorPrefs.GetInt("splineLevelOfDetail", 20);
				CreateCompoundLineFromPath(path, levelOfDetail);
			}
		}
		*/
#endif
	}
}
