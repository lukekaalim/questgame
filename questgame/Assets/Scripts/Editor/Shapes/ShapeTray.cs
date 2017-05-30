using UnityEngine;
using UnityEditor;

namespace Shapes
{
	public class ShapeTray : EditorWindow
	{
		bool showDistances = true;

		[MenuItem("Window/Shapes")]
		static void ShowWindow()
		{
			ShapeTray window = GetWindow<ShapeTray>(false, "Shape Tray", true);
			window.minSize = new Vector2(300f, 30f);
			window.Show();
		}

		void OnEnable()
		{
			SceneView.onSceneGUIDelegate += OnSceneGUI;
		}

		void OnDestroy()
		{
			SceneView.onSceneGUIDelegate -= OnSceneGUI;
		}

		void OnSceneGUI(SceneView view)
		{
			foreach (CompoundLine line in CompoundLine.enabledCompoundLines)
			{
				for (int y = 0; y < line.Points.Count; y++)
				{
					Vector3 point = line.Points[y];
					Vector3 newPoint = Handles.DoPositionHandle(point, Quaternion.identity);

					if (point != newPoint)
					{
						line.Points[y] = newPoint;
						line.Recalculate();
					}

					using (new Handles.DrawingScope(line.lineColor))
					{
						Handles.DrawAAPolyLine(4f, line.Points.ToArray());
					}

					if (y < line.Points.Count - 1)
					{
						if (showDistances)
						{
							GUIStyle style = new GUIStyle();
							style.normal.textColor = line.lineColor;

							Handles.Label((point + line.Points[y + 1]) / 2, line.SegmentLengths[y].ToString(), style);
						}
					}
				}
			}

			HandleUtility.Repaint();
		}

		void OnGUI()
		{
			EditorGUILayout.LabelField("CompoundLine count", CompoundLine.enabledCompoundLines.Count.ToString());
			showDistances = EditorGUILayout.Toggle("Show Point Distances", showDistances);
			for (int i = 0; i < CompoundLine.enabledCompoundLines.Count; i++)
			{
				CompoundLine line = CompoundLine.enabledCompoundLines[i];
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel(line.Points.Count.ToString());
				if (GUILayout.Button("delete line"))
				{
					DestroyImmediate(line);
				}
				EditorGUILayout.EndHorizontal();
			}
		}
	}
}