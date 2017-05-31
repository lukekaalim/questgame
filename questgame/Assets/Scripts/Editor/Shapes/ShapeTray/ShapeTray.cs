using UnityEngine;
using UnityEditor;

using Shapes.Renderers;

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
				CompoundLineRenderer.RenderLine(line);
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