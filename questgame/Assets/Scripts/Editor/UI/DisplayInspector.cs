using UnityEngine;
using UnityEditor;
using System.Collections;

namespace DisplayBinding
{
	[CustomEditor(typeof(DisplayBase), true)]
	public class DisplayInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			DisplayBase display = target as DisplayBase;

			if (GUILayout.Button("Refresh"))
			{
				display.Refresh();
				Repaint();
				SceneView.RepaintAll();
			}

			DrawDefaultInspector();
		}
	}
}