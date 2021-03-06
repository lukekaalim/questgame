﻿using UnityEngine;
using UnityEditor;

namespace Shapes
{
	[CustomEditor(typeof(ExtrudableMesh), true)]
	public class ExtrudableMeshInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			ExtrudableMesh extrudable = target as ExtrudableMesh;
			DrawDefaultInspector();
			if(extrudable.IsValidForGeneration && (GUI.changed || GUILayout.Button("refresh")))
			{
				extrudable.Brush.RecalculateBrush();
				extrudable.GenerateMesh();
			}
		}
	}
}
