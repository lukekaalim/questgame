using UnityEngine;
using UnityEditor;

namespace Shapes
{
	//The inspector that edits the points in a line.
	[CustomEditor(typeof(CompoundLine))]
	public class CompoundLineInspector : Editor
	{
		/*
		int selectedPoint = -1;

		public override void OnInspectorGUI()
		{
			CompoundLine line = target as CompoundLine;

			DrawDefaultInspector();

			EditorGUILayout.LabelField("Point Count", line.PointCount.ToString());

			EditorGUILayout.BeginHorizontal();

			EditorGUILayout.LabelField("Total Length", line.TotalLength.ToString());

			if (GUILayout.Button("+", GUILayout.Width(100)))
			{
				Undo.RecordObject(line, "Added Point");
				line.AddPoint(Vector3.zero);
			}

			EditorGUILayout.EndHorizontal();

			for (int i = 0; i < line.PointCount; i++)
			{
				EditorGUILayout.BeginHorizontal();

				if (selectedPoint == i)
				{
					EditorGUILayout.LabelField(i.ToString(), GUILayout.Width(50));
				}
				else
				{
					if(GUILayout.Button(i.ToString(), GUILayout.Width(50)))
					{
						selectedPoint = i;
					}
				}

				Vector3 newPosition = EditorGUILayout.Vector3Field(GUIContent.none, line.GetPoint(i));

				if (GUILayout.Button("-", GUILayout.Width(50)))
				{
					Undo.RecordObject(line, "Removed Point");
					line.RemovePoint(i);
					i--;
				}

				if (newPosition != line.GetPoint(i))
				{
					Undo.RecordObject(line, "Moved Point");
					line.SetPoint(i, newPosition);
				}

				EditorGUILayout.EndHorizontal();
			}

			if (GUI.changed)
			{
				SceneView.RepaintAll();
			}

		}

		//On the scene rendering
		void OnSceneGUI()
		{
			CompoundLine line = target as CompoundLine;


			Handles.color = Color.white;
			Quaternion handleRotation = Tools.pivotRotation == PivotRotation.Local ? line.transform.rotation : Quaternion.identity;


			for (int i = 0; i < line.PointCount; i++)
			{

				if (i < line.PointCount - 1)
				{
					Handles.DrawLine(line.GetPoint(i), line.GetPoint(i+1));

					GUIStyle style = new GUIStyle();
					style.normal.textColor = Color.white;

					Handles.Label(Vector3.Lerp(line.GetPoint(i), line.GetPoint(i+1), 0.5f), line.LineLengths[i].ToString(), style);
				}

				float size = (HandleUtility.GetHandleSize(line.GetPoint(i)) * 0.25f);

				if (Handles.Button(line.GetPoint(i), Quaternion.identity, size, size, Handles.SphereCap))
				{
					selectedPoint = i;
					Repaint();
				}

				if (i == selectedPoint)
				{
					Vector3 newPosition = Handles.DoPositionHandle(line.GetPoint(i), handleRotation);

					if (newPosition != line.GetPoint(i))
					{
						Undo.RecordObject(line, "Move Point");
						line.SetPoint(i, newPosition);
					}
				}

			}


		}
		*/
	}
}
