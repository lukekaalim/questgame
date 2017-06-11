using UnityEditor;
using UnityEngine;

namespace Shapes.Editors
{
	public class CompoundLineEditor : IVisualEditor
	{
		CompoundLine target;
		int selectedIndex = -1;
		Texture sliderBackground, pointTexture;
		float scaleMin, scaleMax;

		public CompoundLineEditor(CompoundLine target)
		{
			this.target = target;
			sliderBackground = EditorGUIUtility.Load("scrollBackground.png") as Texture;
			pointTexture = EditorGUIUtility.Load("point.png") as Texture;
			scaleMin = 0;
			scaleMax = 1;
		}

		public void DrawGUI()
		{
			const float TOTAL_HEIGHT = 60f;
			const float HORIZONTAL_PADDING = 20f;
			const float LINE_HEIGHT = 5f;
			const float VERTICAL_PADDING = 10f;
			const float SCALE_HEIGHT = 15f;

			Rect pointEditorRect = GUILayoutUtility.GetRect(EditorGUIUtility.currentViewWidth, TOTAL_HEIGHT);
			Rect scaleRect = new Rect(pointEditorRect);
			scaleRect.xMin += HORIZONTAL_PADDING;
			scaleRect.xMax -= HORIZONTAL_PADDING;
			scaleRect.yMin += 5f;
			scaleRect.yMax = scaleRect.yMin + SCALE_HEIGHT;

			Rect scrollBackground = new Rect(pointEditorRect);

			scrollBackground.xMin += HORIZONTAL_PADDING;
			scrollBackground.xMax -= HORIZONTAL_PADDING;

			scrollBackground.yMin += (VERTICAL_PADDING * 2) + SCALE_HEIGHT;
			scrollBackground.yMax = scrollBackground.yMin + LINE_HEIGHT;

			GUI.DrawTexture(scrollBackground, sliderBackground);

			EditorGUI.MinMaxSlider(scaleRect, ref scaleMin, ref scaleMax, 0, 1);

			for(int i = 0; i < target.Points.Count; i++)
			{
				float relativeDistance = 1;
				if(i != target.Points.Count - 1)
				{
					relativeDistance = target.PointDistances[i] / target.TotalLength;
				}
				float scale =  (scaleMax - scaleMin);
				float offset = scaleMin;
				float calculatedPosition = (relativeDistance - offset) / scale;
				if(calculatedPosition < 0 || calculatedPosition > 1)
				{
					continue;
				}

				float xPosition = Mathf.LerpUnclamped(scrollBackground.xMin, scrollBackground.xMax, calculatedPosition);

				float yPosition = (scrollBackground.yMax + scrollBackground.yMin) / 2;

				float textWidth = GUI.skin.button.CalcSize(new GUIContent(i.ToString())).x + 0f;

				Rect pointPosition = new Rect(xPosition -textWidth/2, yPosition - 7f, textWidth, 14f);

				if(GUI.Button(pointPosition, new GUIContent(i.ToString())))
				{
					selectedIndex = i;
				} 
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
