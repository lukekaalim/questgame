using UnityEngine;
using UnityEditor;

namespace Shapes.Editors
{
	[CustomEditor(typeof(CompoundLine))]
	[CanEditMultipleObjects]
	public class CompoundLineInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
			if(GUI.changed)
			{
				CompoundLine line = target as CompoundLine;
				line.Recalculate();
			}
		}
	}
}
