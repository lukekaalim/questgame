using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;

namespace Shapes
{
	public class BezierSpline : MonoBehaviour, IPathable, ILinearTraversable
	{
		public enum ControlMode
		{
			None = 0,
			Linked = 1,
			Mirrored = 2
		}

		[SerializeField, HideInInspector]
		List<Vector3> _controlPoints = new List<Vector3>();

		[SerializeField, HideInInspector]
		List<ControlMode> _controlPointMode = new List<ControlMode>();

		[SerializeField, HideInInspector]
		bool lineRepresentationNeedsUpdating = true;

		[SerializeField, HideInInspector]
		int representationSamples = 100;

		/*
		 *	Optomization stuff
		 *
		 *	[SerializeField, HideInInspector]
			List<int> representationSamplesPerCurve = new List<int>();	*/

		[SerializeField, HideInInspector]
		CompoundLine _lineRepresentation = null;

#if UNITY_EDITOR
		[SerializeField]
		Color displayColor = Color.white;

		public void SetColor(Color newColor)
		{
			displayColor = newColor;
		}

		public Color GetColor()
		{
			return displayColor;
		}

		public int RepresentationSamples
		{
			get
			{
				return representationSamples;
			}
			set
			{
				representationSamples = Mathf.Clamp(value, 1, int.MaxValue);
			}
		}
#endif

		public CompoundLine LineRepresentation
		{
			get
			{
				if (_lineRepresentation == null)
				{
					RegenerateLineRepresentation();
				}
				return _lineRepresentation;
			}
		}

		public float AbsoluteLength
		{
			get
			{
				if (lineRepresentationNeedsUpdating)
				{
					RegenerateLineRepresentation();
				}
				return _lineRepresentation.TotalLength;
			}
		}

		public ReadOnlyCollection<Vector3> ControlPoints
		{
			get
			{
				return _controlPoints.AsReadOnly();
			}
		}

		public int CurveCount
		{
			get
			{
				return (_controlPoints.Count - 1) / 3;
			}
		}

		public bool IsTangentControlPoint(int index)
		{
			return index % 3 != 0;
		}

		public int GetMainControlPoint(int index)
		{
			return Mathf.FloorToInt((index + 1) / 3) * 3;
		}

		public int GetControlPointModeIndex(int controlPointIndex)
		{
			return Mathf.FloorToInt((controlPointIndex + 1) / 3);
		}

		public ControlMode GetControlPointMode(int controlPointIndex)
		{
			return _controlPointMode[Mathf.FloorToInt((controlPointIndex + 1) / 3)];
		}

		public int GetLeftMostControlPoint(float position)
		{
			position = position * (_controlPoints.Count - 1);
			float offset = position % 3;
			int index = Mathf.FloorToInt(position - offset);

			if (index == _controlPoints.Count - 1)
			{
				return index - 3;
			}

			return index;
		}

		public int GetLeftMostControlPoint(float position, out float offset)
		{
			position = position * (_controlPoints.Count - 1);
			offset = position % 3;
			int index = Mathf.FloorToInt(position - offset);
			offset /= 3;
			if (index == _controlPoints.Count - 1)
			{
				offset = 1;
				return index - 3;
			}

			return index;
		}

		public void SetControlPointMode(int controlPointIndex, ControlMode newMode)
		{
			lineRepresentationNeedsUpdating = true;
			_controlPointMode[Mathf.FloorToInt((controlPointIndex + 1) / 3)] = newMode;
		}

		public void SetControlPoint(int index, Vector3 newPosition, bool worldSpace = true, bool moveTangents = false, bool ignoreControlMode = false)
		{
			bool isTangent = index % 3 != 0;
			lineRepresentationNeedsUpdating = true;

			if (!ignoreControlMode && isTangent)
			{
				int mainControlPoint = Mathf.FloorToInt((index + 1) / 3) * 3;
				int difference = index - mainControlPoint;
				int oppositeControlPoint = mainControlPoint - difference;

				if (oppositeControlPoint < ControlPoints.Count && oppositeControlPoint >= 0)
				{
					ControlMode pointMode = GetControlPointMode(index);

					if (pointMode == ControlMode.Mirrored)
					{
						Vector3 transformedPoint = GetControlPoint(index, false) - GetControlPoint(mainControlPoint, false);
						transformedPoint = -transformedPoint;
						transformedPoint += GetControlPoint(mainControlPoint, false);
						SetControlPoint(oppositeControlPoint, transformedPoint, false, false, true);
					}

					if (pointMode == ControlMode.Linked)
					{
						float length = Vector3.Distance(GetControlPoint(oppositeControlPoint, false), GetControlPoint(mainControlPoint, false));

						Vector3 transformedPoint = GetControlPoint(index, false) - GetControlPoint(mainControlPoint, false);
						transformedPoint = -transformedPoint;
						transformedPoint = transformedPoint.normalized * length;
						transformedPoint += GetControlPoint(mainControlPoint, false);
						SetControlPoint(oppositeControlPoint, transformedPoint, false, false, true);
					}
				}
			}

			if (moveTangents && !isTangent)
			{
				Vector3 movementDelta = newPosition - GetControlPoint(index);
				if (index > 0)
				{
					SetControlPoint(index - 1, GetControlPoint(index - 1) + movementDelta, worldSpace, false, true);
				}
				if (_controlPoints.Count - 1 > index)
				{
					SetControlPoint(index + 1, GetControlPoint(index + 1) + movementDelta, worldSpace, false, true);
				}
			}
			_controlPoints[index] = worldSpace ? transform.InverseTransformPoint(newPosition) : newPosition;
		}

		public Vector3 GetControlPoint(int index, bool worldSpace = true)
		{
			return worldSpace ? transform.TransformPoint(_controlPoints[index]) : _controlPoints[index];
		}

		public void RegenerateLineRepresentation()
		{
			if (_lineRepresentation != null)
			{
				DestroyImmediate(_lineRepresentation.gameObject, false);
			}
			_lineRepresentation = CompoundLine.CreateCompoundLineFromPath(this, representationSamples);
			_lineRepresentation.gameObject.name = name + " representation line";
			_lineRepresentation.transform.SetParent(transform, true);

			lineRepresentationNeedsUpdating = false;
		}

		//Add a new set of control points that define a new curve
		public void AddCurveToFront(List<Vector3> newPoints = null)
		{
			lineRepresentationNeedsUpdating = true;
			//If there are no control points, or the list hasn't been initalised yet.
			if (_controlPoints == null || _controlPoints.Count == 0)
			{
				if (newPoints != null && newPoints.Count == 4)
				{
					_controlPoints = newPoints;
				}
				else
				{
					_controlPoints = new List<Vector3>(new Vector3[4]);
				}
				_controlPointMode.Add(ControlMode.None);
				_controlPointMode.Add(ControlMode.None);
			}
			else
			{
				if (newPoints != null && newPoints.Count == 3)
				{
					_controlPoints.AddRange(newPoints);
				}
				else
				{
					_controlPoints.AddRange(new Vector3[3]);
				}
				_controlPointMode.Add(ControlMode.None);
			}
		}

		public void AddCurve(List<Vector3> newPoints, int index, bool worldSpace = true)
		{
			if (index >= _controlPoints.Count || newPoints.Count != 3)
			{
				return;
			}

			lineRepresentationNeedsUpdating = true;

			for (int i = 0; i < newPoints.Count; i++)
			{
				newPoints[i] = worldSpace ? transform.InverseTransformPoint(newPoints[i]) : newPoints[i];
			}

			_controlPointMode.Insert(GetControlPointModeIndex(index), ControlMode.None);
			_controlPoints.InsertRange(index, newPoints);
		}

		//Removes the last 3 points on the spline
		public void RemoveLastCurve()
		{
			if (_controlPoints.Count < 4)
			{
				return;
			}
			else if (_controlPoints.Count == 4)
			{
				_controlPoints.RemoveRange(0, 4);
				_controlPointMode.RemoveRange(0, 2);
			}
			else
			{
				_controlPoints.RemoveRange(_controlPoints.Count - 3, 3);
				_controlPointMode.RemoveAt(_controlPointMode.Count - 1);
			}
			lineRepresentationNeedsUpdating = true;
		}

		public Vector3 GetPointOnPath(float percentage, bool worldSpace = true)
		{
			if (_controlPoints.Count == 0)
			{
				return Vector3.zero;
			}

			percentage = Mathf.Clamp01(percentage);
			if (percentage == 1)
			{
				return worldSpace ? transform.TransformPoint(_controlPoints[ControlPoints.Count - 1]) : _controlPoints[ControlPoints.Count - 1];
			}

			float position = percentage * (_controlPoints.Count - 1);
			float offset = position % 3;
			int index = Mathf.FloorToInt(position - offset);

			Vector3 pointPosition = Bezier.GetPoint(new Bezier.QuadraticBezierControlPointSet(_controlPoints[index], _controlPoints[index + 1], _controlPoints[index + 2], _controlPoints[index + 3]), offset / 3);

			return worldSpace ? transform.TransformPoint(pointPosition) : pointPosition;
		}

		//Gets the velocity of a point on the path
		public Vector3 GetVelocityOnPath(float percentage, bool worldSpace = true)
		{
			if (_controlPoints.Count == 0)
			{
				return Vector3.zero;
			}

			float position = percentage * (_controlPoints.Count - 1);
			float offset = position % 3;
			int index = Mathf.FloorToInt(position - offset);
			if (percentage == 1)
			{
				index = _controlPoints.Count - 4;
				offset = 3;
			}
			Vector3 pointPosition = Bezier.GetVelocity(new Bezier.QuadraticBezierControlPointSet(_controlPoints[index], _controlPoints[index + 1], _controlPoints[index + 2], _controlPoints[index + 3]), offset / 3);

			return worldSpace ? transform.TransformPoint(pointPosition) : pointPosition;
		}

		//Interface Stuff

		public Vector3 GetStartPosition()
		{
			return LineRepresentation.GetStartPosition();
		}

		public Vector3 GetEndPosition()
		{
			return LineRepresentation.GetEndPosition();
		}

		public Vector3 GetPointAlongDistance(float distance)
		{
			return LineRepresentation.GetPointAlongDistance(distance);
		}

		public Vector3 GetRelativePoint(float distance)
		{
			return LineRepresentation.GetRelativePoint(distance);
		}

		public Vector3 AdvanceAlongLine(ref int index, ref float distance, out float newTotalDistance, out float pointProgress)
		{
			return LineRepresentation.AdvanceAlongLine(ref index, ref distance, out newTotalDistance, out pointProgress);
		}

		public Vector3 GetPointAlongDistance(int startingIndex, float distanceFromIndex)
		{
			return LineRepresentation.GetPointAlongDistance(startingIndex, distanceFromIndex);
		}

		public Vector3 GetVelocityAtIndex(int startingIndex)
		{
			return LineRepresentation.GetVelocityAtIndex(startingIndex);
		}

		public int GetIndexAtPoint(float distance)
		{
			return LineRepresentation.GetIndexAtPoint(distance);
		}

		public int PointCount
		{
			get
			{
				return LineRepresentation.PointCount;
			}
		}

#if UNITY_EDITOR

		void OnDrawGizmos()
		{
			Gizmos.color = displayColor;
			UnityEditor.Handles.color = displayColor;

			for (int i = 0; i < _controlPoints.Count - 1; i += 3)
			{
				Vector3 controlPoint0 = GetControlPoint(i);
				Vector3 controlPoint1 = GetControlPoint(i + 1);
				Vector3 controlPoint2 = GetControlPoint(i + 2);
				Vector3 controlPoint3 = GetControlPoint(i + 3);

				UnityEditor.Handles.DrawBezier(controlPoint0, controlPoint3, controlPoint1, controlPoint2, displayColor, null, 5);

				UnityEditor.Handles.DrawDottedLine(controlPoint0, controlPoint1,10);
				UnityEditor.Handles.DrawDottedLine(controlPoint2, controlPoint3,10);

				for (int y = 0; y < 4; y++)
				{
					Vector3 controlPoint = GetControlPoint(i + y);
					float size = (UnityEditor.HandleUtility.GetHandleSize(GetControlPoint(i + y)) / 7.5f);
					Gizmos.DrawSphere(controlPoint, size);
				}
			}
		}

		[UnityEditor.MenuItem("GameObject/Shapes/Bezier Spline", false, 10)]
		static void CreateCustomGameObject(UnityEditor.MenuCommand menuCommand)
		{
			// Create a custom game object
			GameObject gameObject = new GameObject("Bezier Spline");
			gameObject.AddComponent<BezierSpline>().SetColor(new Color(UnityEngine.Random.Range(0.5f, 1f), UnityEngine.Random.Range(0.5f, 1f), UnityEngine.Random.Range(0.5f, 1f)));

			// Ensure it gets reparented if this was a context click (otherwise does nothing)
			UnityEditor.GameObjectUtility.SetParentAndAlign(gameObject, menuCommand.context as GameObject);
			// Register the creation in the undo system
			UnityEditor.Undo.RegisterCreatedObjectUndo(gameObject, "Create " + gameObject.name);
		}

#endif
	}
}
