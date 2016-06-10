using System;
using UnityEngine;
using UnityEditor;

namespace Shapes
{
	[CustomEditor(typeof(BezierCurve), true)]
	class BezierInspector : Editor
	{
		int selectedControl = -1;
		Quaternion handleRotation;
		int levelOfDetail = 10;

		void OnSceneGUI()
		{
			BezierCurve bezier = target as BezierCurve;

			Camera currentCamera = Camera.current;

			handleRotation = Tools.pivotRotation == PivotRotation.Local ? bezier.transform.rotation : Quaternion.identity;

			if (currentCamera == null)
			{
				return;
			}

			Vector3 cameraPosition = currentCamera.transform.position;

			for (int i = 0; i < bezier.GetControlPointCount(); i++)
			{
				Vector3 position = bezier.GetControlPoint(i);

				float size = (HandleUtility.GetHandleSize(position) / 6) + 0.5f;

				Handles.color = Color.white;

				Vector3 directionToCamera = position - cameraPosition;
				if (selectedControl != i)
				{
					if (Handles.Button(position, Quaternion.LookRotation(directionToCamera, Vector3.up), size, size, Handles.SphereCap))
					{
						selectedControl = i;
					}
				}
				else
				{
					Handles.color = Color.red;
					Handles.Button(position, Quaternion.LookRotation(directionToCamera, Vector3.up), size * 1.25f, size * 1.25f, Handles.SphereCap);
					bezier.SetControlPoint(i, Handles.DoPositionHandle(position, handleRotation));
				}
			}
		}

		public override void OnInspectorGUI()
		{
			levelOfDetail = EditorPrefs.GetInt("splineLevelOfDetail", 10);

			int oldLevelOfDetail = levelOfDetail;

			levelOfDetail = Mathf.Clamp(EditorGUILayout.IntField("Global Level of Detail",levelOfDetail), 0, int.MaxValue);

			if (Tools.current != Tool.None)
			{
				if (GUILayout.Button("Hide Current Tool"))
				{
					Tools.current = Tool.None;
				}
			}
			else
			{
				if (GUILayout.Button("Show Transfrom Tool"))
				{
					Tools.current = Tool.Move;
				}
			}

			if (oldLevelOfDetail != levelOfDetail)
			{
				EditorPrefs.SetInt("splineLevelOfDetail", levelOfDetail);
				SceneView.RepaintAll();
			}
			DrawDefaultInspector();
		}
	}
}
