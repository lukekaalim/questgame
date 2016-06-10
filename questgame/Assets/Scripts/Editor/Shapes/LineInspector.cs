using UnityEngine;
using UnityEditor;

namespace Shapes
{
	//The inspector that edits the points in a line.
	[CustomEditor(typeof(Line),true)]
	public class LineInspector : Editor
	{
		//On the scene rendering
		void OnSceneGUI()
		{
			Line line = target as Line;

			Handles.color = Color.white;
			Quaternion handleRotation = Tools.pivotRotation == PivotRotation.Local ? line.transform.rotation : Quaternion.identity;

			for (int i = 0; i < line.PointCount - 1; i++)
			{
				//Handles.DrawLine(line[i], line[i+1]);

				Vector3 newPosition01 = Handles.DoPositionHandle(line[i], handleRotation);
				Vector3 newPosition02 = Handles.DoPositionHandle(line[i+1], handleRotation);
				if (newPosition01 != line[i] || newPosition02 != line[i + 1])
				{
					Undo.RecordObject(line, "Move Point");
					line[i] = newPosition01;
					line[i + 1] = newPosition02;
				}
			}
		}
	}
}
