﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Shapes
{
	//A continuous line composed of multiple points that passes through each linenearly
	public class CompoundLine : Line, ILinearTraversable
	{
		[SerializeField, HideInInspector]
		protected List<Vector3> _points = new List<Vector3>();

		[SerializeField, HideInInspector]
		protected bool lengthNeedsRecalculation = true;

		[SerializeField, HideInInspector]
		protected float _totalLength;

		[SerializeField, HideInInspector]
		protected List<float> _lineLengths = new List<float>();

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
					Vector3 result =  Vector3.Lerp(_points[index], _points[index + 1], offset / Vector3.Distance(_points[index], _points[index + 1]));
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
			return Vector3.Lerp(this[startingIndex], this[startingIndex + 1], distanceFromIndex / LineLengths[startingIndex]);
		}

		public Vector3 AdvanceAlongLine(int startingIndex, float newDistance, out int endIndex, out float endDistance, out float newTotalDistance)
		{
			while (startingIndex < PointCount - 2 && LineLengths[startingIndex] <= newDistance)
			{
				newDistance -= LineLengths[startingIndex];
				startingIndex++;
			}

			endIndex = startingIndex;
			endDistance = newDistance;
			newTotalDistance = _lineDistance[endIndex] + newDistance;

			return Vector3.Lerp(this[endIndex], this[endIndex + 1], endDistance / LineLengths[endIndex]);
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

		public Vector3 GetVelocityAtIndex(int startingIndex)
		{
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