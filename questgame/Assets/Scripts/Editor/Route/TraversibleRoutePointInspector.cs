using UnityEditor;
using UnityEngine;

namespace Route
{
	[CustomEditor(typeof(TraversibleRoutePoint), true)]
	public class TraversibleRoutePointInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			TraversibleRoutePoint point = target as TraversibleRoutePoint;

			Vector2 pointCenter = EditorGUILayout.Vector2Field("Position", point.CollisionBounds.center);
			Vector2 pointExtents = EditorGUILayout.Vector2Field("Extends", point.CollisionBounds.extents);

			if (GUI.changed)
			{
				point.CollisionBounds = new Bounds(pointCenter, pointExtents * 2);
				SceneView.RepaintAll();
			}
		}
	}
}