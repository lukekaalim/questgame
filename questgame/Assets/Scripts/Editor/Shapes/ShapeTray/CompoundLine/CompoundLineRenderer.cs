using UnityEditor;
using UnityEngine;

namespace Shapes.Editors
{
	public static class CompoundLineRenderer
	{
		public static int controlHandleId = 0;

		public static void Render(CompoundLine lineToRender)
		{
			Vector3[] points = lineToRender.Points.ToArray();

			RenderLines(
				points,
				lineToRender.lineColor,
				3f);

			if (MouseClickedOnLine(points))
			{
				Selection.activeObject = lineToRender;
			}
		}

		private static bool MouseClickedOnLine(Vector3[] points)
		{
			const float MIN_CLICK_PIXEL_DISTANCE = 10f;

			if (Event.current.button != 0)
			{
				return false;
			}
			controlHandleId = GUIUtility.GetControlID(FocusType.Passive);
			EventType currentEventType = Event.current.GetTypeForControl(controlHandleId);

			bool mouseUp = (currentEventType == EventType.MouseUp);
			bool mouseDown = (currentEventType == EventType.MouseDown);

			if (!mouseUp && !mouseDown)
			{
				return false;
			}

			float distance = HandleUtility.DistanceToPolyLine(points);

			if (distance > MIN_CLICK_PIXEL_DISTANCE)
			{
				if (GUIUtility.hotControl == controlHandleId)
				{
					GUIUtility.hotControl = 0;
				}
				return false;
			}

			Event.current.Use();

			if (mouseUp)
			{
				GUIUtility.hotControl = 0;
				return true;
			}
			else
			{
				GUIUtility.hotControl = controlHandleId;
				return false;
			}
		}

		public static void RenderLines(Vector3[] points, Color color, float width)
		{
			using (new Handles.DrawingScope(color))
			{
				Handles.DrawAAPolyLine(width, points);
			}
		}

		public static bool RenderPointButton(Vector3 point)
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

		public static void RenderLabelAtMidpoint(Vector3 start, Vector3 end, string label, Color color)
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
