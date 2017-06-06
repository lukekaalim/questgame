using UnityEditor;
using UnityEngine;

namespace Shapes.Editors
{
	public class CompoundLineEditor : VisualEditor
	{
		CompoundLine target;
		int selectedIndex = -1;

		public CompoundLineEditor(CompoundLine target)
		{
			this.target = target;
		}

		public void DrawGUI()
		{
			if (GUILayout.Button("Recalculate Line"))
			{
				target.Recalculate();
			}
		}

		public void RenderScene()
		{
			for (int i = 0; i < target.Points.Count; i++)
			{
				Vector3 point = target.Points[i];

				if(selectedIndex == i)
				{
					Vector3 newPosition = Handles.DoPositionHandle(point, Quaternion.identity);
					if(newPosition != target.Points[i])
					{
						target.Points[i] = newPosition;
						target.Recalculate();
					}
				}
				else if(RenderPointButton(point))
				{
					selectedIndex = i;
				}

				if (i < target.Points.Count - 1)
				{
					RenderLabelAtMidpoint(
						point,
						target.Points[i + 1],
						target.SegmentLengths[i].ToString(),
						target.lineColor
						);
				}
			}
		}

		static bool RenderPointButton(Vector3 point)
		{
			const float MIN_SIZE = 0.1f;

			float consistentHandleSize = HandleUtility.GetHandleSize(point) * 0.1f;
			float size = (consistentHandleSize > MIN_SIZE) ? consistentHandleSize : MIN_SIZE;

			Quaternion rotation = Quaternion.identity;

			if (Camera.current != null)
			{
				rotation = Camera.current.transform.rotation;
			}

			bool selectPoint = Handles.Button(
				point,
				rotation,
				size,
				size,
				Handles.CircleHandleCap
				);

			return selectPoint;
		}

		static void RenderLabelAtMidpoint(Vector3 start, Vector3 end, string label, Color color)
		{
			if (EditorPrefs.GetBool("CompoundLineEditor/RenderMidpointLengths", false))
			{
				GUIStyle style = new GUIStyle();
				style.normal.textColor = color;

				Handles.Label((start + end) / 2, label, style);
			}
		}
	}
}
