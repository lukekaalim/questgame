using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Utility;

namespace Shapes.Editors.CompoundLineEditors
{
	public class PointHandler
	{
		CompoundLineEditor baseEditor;

		public PointHandler(CompoundLineEditor baseEditor)
		{
			this.baseEditor = baseEditor;
		}

		public void DrawPointHandlers()
		{
			CompoundLine line = baseEditor.Target;
			Dictionary<int, Vector3> selectedVectors = new Dictionary<int, Vector3>();

			for (int i = 0; i < line.Points.Count; i++)
			{
				Vector3 point = line.Points[i];

				if (baseEditor.Selection.Contains(i))
				{
					selectedVectors.Add(i, line.Points[i]);
					if (baseEditor.Selection.Count == 1)
					{
						Vector3 newPosition = Handles.DoPositionHandle(point, Quaternion.identity);
						if (newPosition != line.Points[i])
						{
							line.Points[i] = newPosition;
							line.Recalculate();
						}
					}
					else
					{
						using (new Handles.DrawingScope(line.lineColor * 2))
						{
							if (CompoundLineRenderer.RenderPointButton(point))
							{
								baseEditor.SelectPoint(i);
							}
						}
					}
				}
				else if (CompoundLineRenderer.RenderPointButton(point))
				{
					baseEditor.SelectPoint(i);
				}

				if (i < line.Points.Count - 1)
				{
					CompoundLineRenderer.RenderLabelAtMidpoint(
						point,
						line.Points[i + 1],
						line.SegmentLengths[i].ToString(),
						line.lineColor
						);
				}
			}

			if (baseEditor.Selection.Count > 1)
			{
				Vector3 average = VectorUtilities.AverageVectors(selectedVectors.Values);
				Vector3 movedPosition = Handles.PositionHandle(average, Quaternion.identity);
				if (average != movedPosition)
				{
					Vector3 delta = movedPosition - average;
					foreach (KeyValuePair<int, Vector3> entry in selectedVectors)
					{
						Vector3 newPosition = entry.Value + delta;
						line.Points[entry.Key] = newPosition;
					}
					line.Recalculate();
				}
			}
		}
	}
}
