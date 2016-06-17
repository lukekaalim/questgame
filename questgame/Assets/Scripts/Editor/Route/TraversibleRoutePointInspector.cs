using UnityEditor;
using UnityEngine;

namespace Route.Linear
{
	/*
	[CustomEditor(typeof(LinearPoint), true)]
	public class TraversiblePointInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			LinearPoint point = target as LinearPoint;
			/*
			Vector2 pointCenter = EditorGUILayout.Vector2Field("Position", point.CollisionBounds.center);
			Vector2 pointExtents = EditorGUILayout.Vector2Field("Extends", point.CollisionBounds.extents);

			if (GUI.changed)
			{
				point.CollisionBounds = new Bounds(pointCenter, pointExtents * 2);
				SceneView.RepaintAll();
			}

		}
	}
*/
}