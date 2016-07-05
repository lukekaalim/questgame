using UnityEngine;
using UnityEditor;

namespace Shapes
{
	[CustomEditor(typeof(Brush), true)]
	public class BrushInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
			Brush selectedBrush = target as Brush;

			if(GUILayout.Button("Refresh"))
			{
				selectedBrush.RecalculateBrush();
			}
		}
	}
}