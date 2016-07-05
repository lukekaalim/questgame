using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Shapes
{
	//A continuous line composed of multiple points that passes through each linenearly
	public class CompoundLine : Line, IPointLine
	{
		[SerializeField, HideInInspector]
		protected List<Vector3> _points = new List<Vector3>();

		[SerializeField, HideInInspector]
		protected bool lengthNeedsRecalculation = true;

		[SerializeField, HideInInspector]
		protected float _totalLength;

		[SerializeField]
		Color _diplayColor = new Color();

		//The length of each individual line
		[SerializeField, HideInInspector]
		protected List<float> _lineLengths = new List<float>();

		//The total length of all the lines so far for each index
		[SerializeField, HideInInspector]
		protected List<float> _lineDistance = new List<float>();

		//Properties

		public bool LengthNeedsRecalculation
		{
			get
			{
				return lengthNeedsRecalculation;
			}
		}

		public float AbsoluteLength
		{
			get
			{
				return TotalLength;
			}
		}


		public List<float> LineLengths
		{
			get
			{
				if (lengthNeedsRecalculation)
				{
					RecalculateLength();
				}
				return _lineLengths;
			}
		}

		public float TotalLength
		{
			get
			{
				if (lengthNeedsRecalculation)
				{
					RecalculateLength();
				}
				return _totalLength;
			}
		}

		public override int PointCount
		{
			get
			{
				return _points.Count;
			}
		}

		public override Color DisplayColor
		{
			get
			{
				return _diplayColor;
			}
		}

		public override Vector3 this[int index, bool global = true]
		{
			get
			{
				return GetPoint(index, global);
			}

			set
			{
				SetPoint(index, value, global);
			}
		}

		//Methods
		public Vector3 GetPoint(int index, bool worldPosition = true)
		{
			return worldPosition ? transform.TransformPoint(_points[index]) : _points[index];
		}

		public float GetRelativeDistance(float distance)
		{
			return distance / _totalLength;
		}

		public Vector3 GetAbsolutePosition(float distance, bool worldSpace = true)
		{
			float traversed = 0;
			int index = 0;
			while (traversed < _totalLength)
			{
				float newTraversed = traversed + _lineLengths[index];
				if (newTraversed >= distance)
				{
					float offset = distance - traversed;
					Vector3 result =  Vector3.LerpUnclamped(_points[index], _points[index + 1], offset / Vector3.Distance(_points[index], _points[index + 1]));
					return worldSpace ? transform.TransformPoint(result) : result;
				}
				else
				{
					traversed = newTraversed;
					index++;
				}
			}
			return worldSpace ? transform.TransformPoint(_points[_points.Count - 1]) : _points[_points.Count - 1];
		}

		public void SetPoint(int index, Vector3 newPosition, bool worldPosition = true)
		{
			lengthNeedsRecalculation = true;
			_points[index] = worldPosition ? transform.InverseTransformPoint(newPosition) : newPosition;
		}

		public void AddPoint(Vector3 newPoint, bool worldPosition = true, int index = -1)
		{
			newPoint = worldPosition ? transform.InverseTransformPoint(newPoint) : newPoint;

			if (index == -1)
			{
				_points.Add(newPoint);
			}
			else
			{
				_points.Insert(index, newPoint);
			}

			lengthNeedsRecalculation = true;
		}

		public void AddPoint(Vector3[] newPoints, bool worldPosition = true, int index = -1)
		{
			if (worldPosition)
			{
				for (int i = 0; i < newPoints.Length; i++)
				{
					newPoints[i] = transform.InverseTransformPoint(newPoints[i]);
				}
			}

			if (index == -1)
			{
				_points.AddRange(newPoints);
			}
			else
			{
				_points.InsertRange(index, newPoints);
			}

			lengthNeedsRecalculation = true;
		}

		public void RemovePoint(int index)
		{
			_points.RemoveAt(index);
		}

		public void RecalculateLength()
		{
			_lineLengths.Clear();
			_lineDistance.Clear();
			_totalLength = 0;

			for (int i = 0; i < _points.Count - 1; i ++)
			{
				float length = Vector3.Distance(GetPoint(i, false), GetPoint(i + 1, false));
				_lineLengths.Add(length);
				_lineDistance.Add(_totalLength);
				_totalLength += length;
			}
			lengthNeedsRecalculation = false;
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

			newTotalDistance = _lineDistance[index] + distance;

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

			newTotalDistance = _lineDistance[index] + distance;
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

		public override Vector3 GetPointOnPath(float percentage, bool worldSpace = true)
		{
			percentage = Mathf.Clamp01(percentage);
			float position = (_points.Count - 1) * percentage;
			float offset = position % 1;
			int index = Mathf.FloorToInt(position - offset);

			if (index == _points.Count - 1)
			{
				return this[index];
			}

			return Vector3.Lerp(this[index, worldSpace], this[index + 1, worldSpace], offset);
		}

		public List<Vector3> GetPointSample(float startPosition, float endPosition)
		{
			List<Vector3> samples = new List<Vector3>();
			int index = 0;
			samples.Add(AdvanceAlongLine(ref index, ref startPosition));
			while (index < PointCount - 3 && _lineDistance[index + 1] < endPosition)
			{
				index++;
				samples.Add(this[index]);
			}
			samples.Add(GetPointAlongDistance(endPosition));

			return samples;
		}

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
		}

		public int GetLeftMostIndex(float distance)
		{
			float traversed = 0;
			int index = 0;
			while (traversed < _totalLength)
			{
				float newTraversed = traversed + _lineLengths[index];
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

		public Vector3 GetVelocityAtIndex(int startingIndex)
		{
			if (startingIndex == PointCount - 1)
			{
				return this[startingIndex] - this[startingIndex - 1];
			}
			return this[startingIndex + 1] - this[startingIndex];
		}

		public Vector3 GetStartPosition()
		{
			return _points[0];
		}

		public Vector3 GetEndPosition()
		{
			return _points[_points.Count - 1];
		}

		public Vector3 GetPointAlongDistance(float distance)
		{
			return GetAbsolutePosition(distance, true);
		}

		public Vector3 GetRelativePoint(float distance)
		{
			return GetPointOnPath(distance, true);
		}

		public override Vector3 GetVelocity(float percentage, bool worldSpace = true)
		{
			int index = GetLeftMostIndex(percentage);
			return worldSpace ? transform.TransformDirection(GetVelocityAtIndex(index)) : GetVelocityAtIndex(index);
		}



#if UNITY_EDITOR
		[UnityEditor.MenuItem("GameObject/Shapes/Lines/Compound", false, 10)]
		static void CreateCustomGameObject(UnityEditor.MenuCommand menuCommand)
		{
			// Create a custom game object
			GameObject gameObject = new GameObject("Compound Line");
			gameObject.AddComponent<CompoundLine>();

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

#endif
	}
}
