using UnityEngine;
using UnityEditor;

namespace Shapes
{
	public class ExtrudableBrushWindow : EditorWindow
	{
		Mesh sourceMesh;

		[MenuItem("Window/Brushes")]
		static void Init()
		{
			ExtrudableBrushWindow window = GetWindow<ExtrudableBrushWindow>("Extrudable Brushes", false);
			window.Show();
		}

		void OnGUI()
		{
			EditorGUILayout.LabelField("New Brush");
			sourceMesh = EditorGUILayout.ObjectField("Brush source mesh",sourceMesh, typeof(Mesh), true) as Mesh;

			if(sourceMesh != null && GUILayout.Button("Create Brush from source"))
			{
				string savePath = EditorUtility.SaveFilePanelInProject("Save new Brush", "New Brush", "asset", "Select the save location for the new brush");
				if (savePath != "")
				{
					Brush.CreateNewBrush(savePath, sourceMesh);
					sourceMesh = null;
				}
			}
		}
	}
}
