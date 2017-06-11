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

		private static void RenderLines(Vector3[] points, Color color, float width)
		{
			using (new Handles.DrawingScope(color))
			{
				Handles.DrawAAPolyLine(width, points);
			}
		}
	}
}
