using UnityEngine;
using UnityEditor;

using Shapes.Editors;

namespace Shapes
{
	public class ShapeTray : EditorWindow
	{
		bool showDistances = true;
		VisualEditor editor;

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
			Selection.selectionChanged += OnSelectionChanged;
			OnSelectionChanged();
		}

		void OnDestroy()
		{
			SceneView.onSceneGUIDelegate -= OnSceneGUI;
			Selection.selectionChanged -= OnSelectionChanged;
		}

		void OnSceneGUI(SceneView view)
		{
			if(editor != null)
			{
				editor.RenderScene();
				Repaint();
			}

			foreach (CompoundLine line in CompoundLine.enabledCompoundLines)
			{
				CompoundLineRenderer.Render(line);
			}

			HandleUtility.Repaint();
		}

		void OnSelectionChanged()
		{
			Repaint();

			CompoundLine line = Selection.activeObject as CompoundLine;
			if (line != null)
			{
				editor = new CompoundLineEditor(line);
				return;
			}

			editor = null;
		}

		void OnGUI()
		{
			if (editor != null)
			{
				editor.DrawGUI();
			}

			if(GUILayout.Button("Reset", GUILayout.Width(50f)))
			{
				OnSelectionChanged();
			}
		}
	}
}