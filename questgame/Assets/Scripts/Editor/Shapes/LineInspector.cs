using UnityEngine;
using UnityEditor;

namespace Shapes
{
	//The inspector that edits the points in a line.
	[CustomEditor(typeof(Line),true)]
	public class LineInspector : Editor
	{
		int selectedPoint = -1;

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
			Line line = target as Line;

			for (int i = 0; i < line.PointCount; i++)
			{
				EditorGUILayout.BeginHorizontal();
				if (i != selectedPoint && GUILayout.Button(i.ToString(), GUILayout.Width(25)))
				{
					selectedPoint = i;
				}
				line[i, false] = EditorGUILayout.Vector3Field(GUIContent.none, line[i, false]);
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
			Line line = target as Line;

			Handles.color = Color.white;
			Quaternion handleRotation = Tools.pivotRotation == PivotRotation.Local ? line.transform.rotation : Quaternion.identity;

			for (int i = 0; i < line.PointCount - 1; i++)
			{
				Vector3 P0 = line[i];
				Vector3 P1 = line[i + 1];

				GUIStyle style = new GUIStyle();
				style.normal.textColor = line.DisplayColor;

				Handles.Label(Vector3.Lerp(P0, P1, 0.5f), Vector3.Distance(P0, P1).ToString(), style);

				Vector3 newPosition01 = Handles.DoPositionHandle(P0, handleRotation);
				Vector3 newPosition02 = Handles.DoPositionHandle(P1, handleRotation);

				if (newPosition01 != P0 || newPosition02 != P1)
				{
					Undo.RecordObject(line, "Move Point");
					line[i] = newPosition01;
					line[i + 1] = newPosition02;
				}
			}
		}
	}
}
