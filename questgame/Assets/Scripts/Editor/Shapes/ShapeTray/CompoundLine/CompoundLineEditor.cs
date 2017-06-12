using UnityEditor;
using UnityEngine;
using Utility;
using System.Collections.Generic;

namespace Shapes.Editors
{
	// TODO split and refactor this into mode sub classes
	public class CompoundLineEditor : IVisualEditor
	{
		CompoundLine target;

		bool currentlyGUIDragSelecting = false;
		bool currentlySceneDragSelecting = false;
		float dragStartPosition = 0;
		float dragEndPosition = 0;
		HashSet<int> selection = new HashSet<int>();
		float scaleMin = 0;
		float scaleMax = 1;

		Texture sliderBackground, scrollSelected;

		public CompoundLineEditor(CompoundLine target)
		{
			this.target = target;
			sliderBackground = EditorGUIUtility.Load("scrollBackground.png") as Texture;
			scrollSelected = EditorGUIUtility.Load("scrollSelected.png") as Texture;
		}

		void DrawScaler()
		{
			const float SCALER_HEIGHT = 20f;
			const float HORIZONTAL_PADDING = 15f;

			Rect scalerRect = GUILayoutUtility.GetRect(EditorGUIUtility.currentViewWidth, SCALER_HEIGHT);
			scalerRect = scalerRect.AddHorizontalPadding(HORIZONTAL_PADDING);
			EditorGUI.MinMaxSlider(scalerRect, ref scaleMin, ref scaleMax, 0, 1);
		}
		
		void DrawSelector()
		{
			const float SELECTOR_HEIGHT = 20f;
			const float HORIZONTAL_PADDING = 15f;
			const float VERTICAL_PADDING = -5f;

			Rect selectorRect = GUILayoutUtility.GetRect(EditorGUIUtility.currentViewWidth, SELECTOR_HEIGHT);
			selectorRect = selectorRect.AddPadding(HORIZONTAL_PADDING, VERTICAL_PADDING);

			GUI.DrawTexture(selectorRect, sliderBackground);

			if(currentlyGUIDragSelecting)
			{
				Rect highlighRect = new Rect(selectorRect)
				{
					xMin = Mathf.LerpUnclamped(selectorRect.xMin, selectorRect.xMax, dragStartPosition),
					xMax = Mathf.LerpUnclamped(selectorRect.xMin, selectorRect.xMax, dragEndPosition)
				};
				GUI.DrawTexture(highlighRect, scrollSelected);
			}

			for (int i = 0; i < target.Points.Count; i++)
			{
				DrawPointHandle(i, selectorRect);
			}

			Event currentEvent = Event.current;

			if(currentEvent.button == 0 && selectorRect.Contains(currentEvent.mousePosition) && currentEvent.type == EventType.MouseDown)
			{
				if(currentEvent.type == EventType.MouseDown)
				{
					currentlyGUIDragSelecting = true;
					dragStartPosition = selectorRect.GetRelativePosition(currentEvent.mousePosition).x;
					dragEndPosition = dragStartPosition;
					selection.Clear();
					currentEvent.Use();
				}
			}

			if(currentlyGUIDragSelecting)
			{
				if(currentEvent.type == EventType.MouseDrag)
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
			float relativeDistance = 1;
			if (pointIndex != target.Points.Count - 1)
			{
				relativeDistance = target.PointDistances[pointIndex] / target.TotalLength;
			}

			float scale = (scaleMax - scaleMin);
			float offset = scaleMin;
			float calculatedPosition = (relativeDistance - offset) / scale;

			if (calculatedPosition < -0.001 || calculatedPosition > 1.001)
			{
				return;
			}

			float xPosition = Mathf.LerpUnclamped(container.xMin, container.xMax, calculatedPosition);
			float yPosition = (container.yMax + container.yMin) / 2;
			Vector2 textSize = GUI.skin.button.CalcSize(new GUIContent(pointIndex.ToString()));

			Rect pointPosition = RectUtilities.CreatePositionRect(new Vector2(xPosition, yPosition), textSize);

			if(currentlyGUIDragSelecting)
			{
				if (calculatedPosition >= dragStartPosition && calculatedPosition <= dragEndPosition)
				{
					selection.Add(pointIndex);
				}
				else
				{
					selection.Remove(pointIndex);
				}
			}
			GUIStyle style = new GUIStyle(GUI.skin.button);
			if(selection.Contains(pointIndex))
			{
				style.normal.background = style.active.background;
			}
			if (GUI.Button(pointPosition, new GUIContent(pointIndex.ToString()), style))
			{
				SelectPoint(pointIndex);
			}
		}

		public void DrawGUI()
		{
			DrawScaler();
			DrawSelector();
		}

		public void SelectPoint(int pointIndex)
		{
			if (!Event.current.shift)
			{
				selection.Clear();
			}
			selection.Add(pointIndex);
		}

		public void RenderScene()
		{
			Dictionary<int, Vector3> selectedVectors = new Dictionary<int, Vector3>();

			for (int i = 0; i < target.Points.Count; i++)
			{
				Vector3 point = target.Points[i];

				if(selection.Contains(i))
				{
					selectedVectors.Add(i, target.Points[i]);
					if (selection.Count == 1)
					{
						Vector3 newPosition = Handles.DoPositionHandle(point, Quaternion.identity);
						if (newPosition != target.Points[i])
						{
							target.Points[i] = newPosition;
							target.Recalculate();
						}
					}
					else
					{
						using(new Handles.DrawingScope(target.lineColor * 2))
						{
							if(CompoundLineRenderer.RenderPointButton(point))
							{
								SelectPoint(i);
							}
						}
					}
				}
				else if(CompoundLineRenderer.RenderPointButton(point))
				{
					SelectPoint(i);
				}

				if (i < target.Points.Count - 1)
				{
					CompoundLineRenderer.RenderLabelAtMidpoint(
						point,
						target.Points[i + 1],
						target.SegmentLengths[i].ToString(),
						target.lineColor
						);
				}
			}

			if(selection.Count > 1)
			{
				Vector3 average = VectorUtilities.AverageVectors(selectedVectors.Values);
				Vector3 movedPosition = Handles.PositionHandle(average, Quaternion.identity);
				if(average != movedPosition)
				{
					Vector3 delta = movedPosition - average;
					foreach (KeyValuePair<int, Vector3> entry in selectedVectors)
					{
						Vector3 newPosition = entry.Value + delta;
						target.Points[entry.Key] = newPosition;
					}
					target.Recalculate();
				}
			}
		}
	}
}
