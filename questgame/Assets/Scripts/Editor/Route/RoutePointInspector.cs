using System;
using UnityEngine;
using UnityEditor;

namespace Route
{
	[CustomEditor(typeof(RoutePoint),true)]
	public class RoutePointInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if (GUI.changed)
			{
				RoutePoint point = target as RoutePoint;
				if (point.ParentRouteAsRouteBase != null)
				{
					point.UpdatePosition();
				}
				SceneView.RepaintAll();
			}
		}
	}
}
