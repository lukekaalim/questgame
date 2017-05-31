using UnityEditor;
using UnityEngine;

namespace Shapes.Renderers
{
	public static class CompoundLineRenderer
	{
		public static bool ShowDistances = true;

		public static void RenderLine(CompoundLine lineToRender)
		{
			for (int y = 0; y < lineToRender.Points.Count; y++)
			{
				Vector3 point = lineToRender.Points[y];

				RenderPoint(point, 0.5f);

				RenderLines(
					lineToRender.Points.ToArray(),
					lineToRender.lineColor,
					4f);

				if (y < lineToRender.Points.Count - 1)
				{
					RenderLabelAtMidpoint(
						point,
						lineToRender.Points[y + 1],
						lineToRender.SegmentLengths[y].ToString(),
						lineToRender.lineColor
						);
				}
			}
		}

		private static void RenderPoint(Vector3 point, float size)
		{
			const float MIN_SIZE = 1f;

			float consistentHandleSize = HandleUtility.GetHandleSize(point) * 0.1f;
			size = (consistentHandleSize > MIN_SIZE) ? consistentHandleSize : MIN_SIZE;

			Handles.Button(
				point,
				Quaternion.identity,
				size,
				size/2,
				Handles.SphereHandleCap
				);
		}

		private static void RenderLines(Vector3[] points, Color color, float width)
		{
			using (new Handles.DrawingScope(color))
			{
				Handles.DrawAAPolyLine(width, points);
			}
		}

		private static void RenderLabelAtMidpoint(Vector3 start, Vector3 end, string label, Color color)
		{
			if (ShowDistances)
			{
				GUIStyle style = new GUIStyle();
				style.normal.textColor = color;

				Handles.Label((start + end) / 2, label, style);
			}
		}
	}
}
