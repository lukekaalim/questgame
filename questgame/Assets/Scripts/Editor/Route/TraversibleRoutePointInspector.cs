using UnityEditor;

namespace Route
{
	[CustomEditor(typeof(TraversibleRoutePoint), true)]
	public class TraversibleRoutePointInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			TraversibleRoutePoint point = target as TraversibleRoutePoint;

			point.Position = EditorGUILayout.Vector2Field("Position", point.Position);
			point.Extents = EditorGUILayout.Vector2Field("Extends", point.Extents);

			if (UnityEngine.GUI.changed)
			{
				SceneView.RepaintAll();
			}
		}
	}
}