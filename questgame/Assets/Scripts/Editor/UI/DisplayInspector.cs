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

			DrawDefaultInspector();

			if (GUI.changed)
			{
				display.Refresh();
				Repaint();
				SceneView.RepaintAll();
			}
		}
	}
}