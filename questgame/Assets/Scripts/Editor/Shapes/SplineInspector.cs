using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Shapes
{
	[CustomEditor(typeof(BezierSpline), true)]
	class SplineInspector : Editor
	{
		int selectedControl = -1;
		int levelOfDetail = 10;
		Quaternion handleRotation;
		bool tangentsMoveWithControlPoints = false;
		bool showConstructionLines = true;

		float selectionPercentage = 50;

		public void OnEnable()
		{
			levelOfDetail = EditorPrefs.GetInt("splineLevelOfDetail", 10);
			tangentsMoveWithControlPoints = EditorPrefs.GetBool("tangentsMoveWithControlPoints", false);
			showConstructionLines = EditorPrefs.GetBool("showConstructionLines", false);
		}

		public override void OnInspectorGUI()
		{
			int oldLevelOfDetail = levelOfDetail;
			BezierSpline spline = target as BezierSpline;


			levelOfDetail = Mathf.Clamp(EditorGUILayout.IntField("Global Level of Detail", levelOfDetail), 0, int.MaxValue);
			tangentsMoveWithControlPoints = EditorGUILayout.Toggle("Tangents Move with Control Points", tangentsMoveWithControlPoints);
			showConstructionLines = EditorGUILayout.Toggle("Show Construction Lines", showConstructionLines);

			if (GUI.changed)
			{
				SceneView.RepaintAll();
			}

			EditorPrefs.SetInt("splineLevelOfDetail", levelOfDetail);
			EditorPrefs.SetBool("tangentsMoveWithControlPoints", tangentsMoveWithControlPoints);
			EditorPrefs.SetBool("showConstructionLines", showConstructionLines);

			if (Tools.current != Tool.None)
			{
				if (GUILayout.Button("Hide Current Tool"))
				{
					Tools.current = Tool.None;
				}
			}
			else
			{
				if (GUILayout.Button("Show Transfrom Tool"))
				{
					Tools.current = Tool.Move;
				}
			}

			if (oldLevelOfDetail != levelOfDetail)
			{
				SceneView.RepaintAll();
			}
			DrawDefaultInspector();

			EditorGUILayout.LabelField("Curve Count", spline.CurveCount.ToString());

			EditorGUILayout.BeginHorizontal();

			EditorGUILayout.LabelField("Add or remove Spline");

			if (GUILayout.Button("+", GUILayout.Width(50)))
			{
				Undo.RecordObject(spline, "Added Control Point");
				Vector3 direction = spline.GetVelocityOnPath(1, false) * 0.5f;
				Vector3 position = spline.GetPointOnPath(1, false);

				spline.AddCurveToFront(new List<Vector3>(new Vector3[]{ position + (direction * 0.3f), position + (direction * 0.6f) , position + (direction * 1) } ));

				SceneView.RepaintAll();
			}

			if (GUILayout.Button("-", GUILayout.Width(50)))
			{
				Undo.RecordObject(spline, "Removed Control Point");
				spline.RemoveLastCurve();
				SceneView.RepaintAll();
			}
			EditorGUILayout.EndHorizontal();

			if (spline.LineRepresentation != null)
			{
				EditorGUILayout.BeginHorizontal();
				if (GUILayout.Button("Regenerate Representation Line"))
				{
					spline.RegenerateLineRepresentation();
				}

				GUI.enabled = false;
				EditorGUILayout.ObjectField(spline.LineRepresentation, typeof(CompoundLine), true);
				GUI.enabled = true;

				EditorGUILayout.EndHorizontal();
				spline.RepresentationSamples = EditorGUILayout.IntField("RepresentationSamples", spline.RepresentationSamples);

				EditorGUILayout.LabelField("Lenth", spline.AbsoluteLength.ToString());
			}
			else
			{
				spline.RepresentationSamples = EditorGUILayout.IntField("RepresentationSamples", spline.RepresentationSamples);
				if (GUILayout.Button("Regenerate Representation Line"))
				{
					spline.RegenerateLineRepresentation();
				}
			}

			EditorGUILayout.BeginHorizontal();

			float newCelenctionPercentage = EditorGUILayout.Slider(selectionPercentage, 0, 100f);

			if (selectionPercentage != newCelenctionPercentage)
			{
				selectionPercentage = newCelenctionPercentage;
				SceneView.RepaintAll();
			}

			if (GUILayout.Button("Split Curve"))
			{
				Undo.RecordObject(spline, "Split Curve");
				float offset;
				int leftmostControlPoint = spline.GetLeftMostControlPoint(selectionPercentage / 100, out offset);

				Vector3 point0 = spline.GetControlPoint(leftmostControlPoint);
				Vector3 point1 = spline.GetControlPoint(leftmostControlPoint + 1);
				Vector3 point2 = spline.GetControlPoint(leftmostControlPoint + 2);
				Vector3 point3 = spline.GetControlPoint(leftmostControlPoint + 3);

				Vector3[] newPoints = new Vector3[] { point0, point1, point2, point3 };

				newPoints = Bezier.GetNextSetOfControlPoints(newPoints, offset);

				spline.SetControlPoint(leftmostControlPoint + 1, newPoints[0]);
				spline.SetControlPoint(leftmostControlPoint + 2, newPoints[2]);

				newPoints = Bezier.GetNextSetOfControlPoints(newPoints, offset);

				newPoints = new Vector3[] { newPoints[0], Vector3.Lerp(newPoints[0], newPoints[1], offset) , newPoints[1]};

				spline.AddCurve(new List<Vector3>(newPoints), leftmostControlPoint + 2, true);

				Repaint();
				SceneView.RepaintAll();
			}

			EditorGUILayout.EndHorizontal();

			for (int i = 0; i < spline.ControlPoints.Count; i++)
			{
				EditorGUILayout.BeginHorizontal();

				if (selectedControl == i)
				{
					EditorGUILayout.LabelField(i.ToString(), GUILayout.Width(25));
				}
				else
				{
					if (GUILayout.Button(i.ToString(), GUILayout.Width(25)))
					{
						selectedControl = i;
						Repaint();
						SceneView.RepaintAll();
					}
				}

				Vector3 oldControlPoint = spline.GetControlPoint(i, false);
				Vector3 newControlPoint = EditorGUILayout.Vector3Field(GUIContent.none,oldControlPoint);
				spline.SetControlPointMode(i, (BezierSpline.ControlMode)EditorGUILayout.EnumPopup(GUIContent.none,spline.GetControlPointMode(i)));

				if (oldControlPoint != newControlPoint)
				{
					Undo.RecordObject(spline, "Moved Control Point");
					spline.SetControlPoint(i, newControlPoint, false, tangentsMoveWithControlPoints);
					SceneView.RepaintAll();
				}

				EditorGUILayout.EndHorizontal();
			}
		}

		void OnSceneGUI()
		{
			BezierSpline bezier = target as BezierSpline;

			Camera currentCamera = Camera.current;

			handleRotation = Tools.pivotRotation == PivotRotation.Local ? bezier.transform.rotation : Quaternion.identity;

			if (currentCamera == null)
			{
				return;
			}

			Handles.DrawLine(bezier.GetPointOnPath(1, true), (bezier.GetVelocityOnPath(1, true) * 0.5f) + bezier.GetPointOnPath(1, true));


			Handles.color = bezier.GetColor() * new Color(1, 1, 1, 0.5f);
			Vector3 selectedPosition = bezier.GetPointOnPath(selectionPercentage / 100);
			Handles.SphereCap(0, selectedPosition, Quaternion.identity, HandleUtility.GetHandleSize(selectedPosition) * 0.6f);

			Vector3 cameraPosition = currentCamera.transform.position;

			for (int i = 0; i < bezier.ControlPoints.Count; i++)
			{
				Vector3 position = bezier.GetControlPoint(i, true);

				float size = (HandleUtility.GetHandleSize(position) / 6);

				Color handleColor = Color.grey;

				BezierSpline.ControlMode mode = bezier.GetControlPointMode(i);
				if (mode == BezierSpline.ControlMode.Mirrored)
				{
					handleColor = new Color(0.2f, 0.5f, 0.3f);
				}
				if (mode == BezierSpline.ControlMode.Linked)
				{
					handleColor = new Color(0.3f, 0.6f, 0.6f);
				}
				if (mode == BezierSpline.ControlMode.None)
				{
					handleColor = new Color(0.8f, 0.8f, 0.8f);
				}

				Handles.color = handleColor;

				if (bezier.IsTangentControlPoint(i))
				{
					int mainControlPoint = bezier.GetMainControlPoint(i);
					Handles.DrawAAPolyLine(5, 2 , new Vector3[] { position, bezier.GetControlPoint(mainControlPoint) });
				}

				Vector3 directionToCamera = position - cameraPosition;
				if (selectedControl != i)
				{
					if (Handles.Button(position, Quaternion.LookRotation(directionToCamera, Vector3.up), size, size, Handles.SphereCap))
					{
						selectedControl = i;
						Repaint();
					}
				}
				else
				{
					Handles.color = Color.red;
					Handles.Button(position, Quaternion.LookRotation(directionToCamera, Vector3.up), size * 1.25f, size * 1.25f, Handles.SphereCap);
					Vector3 newHandlePosition = Handles.DoPositionHandle(position, handleRotation);
					if (newHandlePosition != position)
					{
						Undo.RecordObject(bezier, "Moved Control Point");
						bezier.SetControlPoint(i, newHandlePosition, true, tangentsMoveWithControlPoints);
					}


				}
			}

			if (showConstructionLines && bezier.CurveCount > 0)
			{
				float offset;
				int leftmostControlPoint = bezier.GetLeftMostControlPoint(selectionPercentage / 100, out offset);

				Vector3 point0 = bezier.GetControlPoint(leftmostControlPoint, true);
				Vector3 point1 = bezier.GetControlPoint(leftmostControlPoint + 1, true);
				Vector3 point2 = bezier.GetControlPoint(leftmostControlPoint + 2, true);
				Vector3 point3 = bezier.GetControlPoint(leftmostControlPoint + 3, true);

				Handles.color = Color.cyan;

				Vector3[] newPoints = new Vector3[] { point0, point1, point2, point3 };

				while (newPoints.Length > 1)
				{
					Handles.DrawAAPolyLine(null, 5f, newPoints);

					newPoints = Bezier.GetNextSetOfControlPoints(newPoints, offset);
				}
			}

			//bezier.AddCurve(new List<Vector3>(newPoints), spline.GetLeftMostControlPoint(selectionPercentage / 100));
		}
	}
}
