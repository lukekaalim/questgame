using UnityEngine;
using UnityEditor;
using Utility;

namespace Shapes.Editors.CompoundLineEditors
{
	public class SelectorBar
	{
		CompoundLineEditor baseEditor;
		Texture sliderBackground, scrollSelected;

		bool currentlyGUIDragSelecting = false;

		float dragStartPosition = 0;
		float dragEndPosition = 0;

		public SelectorBar(CompoundLineEditor baseEditor)
		{
			this.baseEditor = baseEditor;
			sliderBackground = EditorGUIUtility.Load("scrollBackground.png") as Texture;
			scrollSelected = EditorGUIUtility.Load("scrollSelected.png") as Texture;
		}

		public void DrawSelectorBar()
		{
			CompoundLine line = baseEditor.Target;
			const float SELECTOR_HEIGHT = 20f;
			const float HORIZONTAL_PADDING = 15f;
			const float VERTICAL_PADDING = -5f;

			Rect selectorRect = GUILayoutUtility.GetRect(EditorGUIUtility.currentViewWidth, SELECTOR_HEIGHT);
			selectorRect = selectorRect.AddPadding(HORIZONTAL_PADDING, VERTICAL_PADDING);

			GUI.DrawTexture(selectorRect, sliderBackground);

			if (currentlyGUIDragSelecting)
			{
				Rect highlighRect = new Rect(selectorRect)
				{
					xMin = Mathf.LerpUnclamped(selectorRect.xMin, selectorRect.xMax, dragStartPosition),
					xMax = Mathf.LerpUnclamped(selectorRect.xMin, selectorRect.xMax, dragEndPosition)
				};
				GUI.DrawTexture(highlighRect, scrollSelected);
			}

			for (int i = 0; i < line.Points.Count; i++)
			{
				DrawPointHandle(i, selectorRect);
			}

			Event currentEvent = Event.current;

			if (currentEvent.button == 0 && selectorRect.Contains(currentEvent.mousePosition) && currentEvent.type == EventType.MouseDown)
			{
				if (currentEvent.type == EventType.MouseDown)
				{
					currentlyGUIDragSelecting = true;
					dragStartPosition = selectorRect.GetRelativePosition(currentEvent.mousePosition).x;
					dragEndPosition = dragStartPosition;
					baseEditor.Selection.Clear();
					currentEvent.Use();
				}
			}

			if (currentlyGUIDragSelecting)
			{
				if (currentEvent.type == EventType.MouseDrag)
				{
					dragEndPosition = selectorRect.GetRelativePosition(currentEvent.mousePosition).x;
					currentEvent.Use();
				}
				else if (currentEvent.type == EventType.MouseUp)
				{
					currentlyGUIDragSelecting = false;
					currentEvent.Use();
				}
			}
		}

		void DrawPointHandle(int pointIndex, Rect container)
		{
			CompoundLine line = baseEditor.Target;

			float relativeDistance = 1;
			if (pointIndex != line.Points.Count - 1)
			{
				relativeDistance = line.PointDistances[pointIndex] / line.TotalLength;
			}

			float offset = baseEditor.ScaleBar.Offset;
			float scale = baseEditor.ScaleBar.Scale;

			float calculatedPosition = (relativeDistance - offset) / scale;

			if (calculatedPosition < -0.001 || calculatedPosition > 1.001)
			{
				return;
			}

			float xPosition = Mathf.LerpUnclamped(container.xMin, container.xMax, calculatedPosition);
			float yPosition = (container.yMax + container.yMin) / 2;
			Vector2 textSize = GUI.skin.button.CalcSize(new GUIContent(pointIndex.ToString()));

			Rect pointPosition = RectUtilities.CreatePositionRect(new Vector2(xPosition, yPosition), textSize);

			if (currentlyGUIDragSelecting)
			{
				if (calculatedPosition >= dragStartPosition && calculatedPosition <= dragEndPosition)
				{
					baseEditor.Selection.Add(pointIndex);
				}
				else
				{
					baseEditor.Selection.Remove(pointIndex);
				}
			}
			GUIStyle style = new GUIStyle(GUI.skin.button);
			if (baseEditor.Selection.Contains(pointIndex))
			{
				style.normal.background = style.active.background;
			}
			if (GUI.Button(pointPosition, new GUIContent(pointIndex.ToString()), style))
			{
				baseEditor.SelectPoint(pointIndex);
			}
		}
	}
}
