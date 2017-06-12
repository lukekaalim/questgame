using UnityEditor;
using UnityEngine;
using Utility;

namespace Shapes.Editors.CompoundLineEditors
{
	public class ScalerBar
	{
		CompoundLineEditor baseEditor;

		float scaleMin = 0;
		float scaleMax = 1;

		public float Scale { get { return (scaleMax - scaleMin); } }
		public float Offset { get { return scaleMin; } }

		public void DrawScalebar()
		{
			const float SCALER_HEIGHT = 20f;
			const float HORIZONTAL_PADDING = 15f;

			Rect scalerRect = GUILayoutUtility.GetRect(EditorGUIUtility.currentViewWidth, SCALER_HEIGHT);
			scalerRect = scalerRect.AddHorizontalPadding(HORIZONTAL_PADDING);
			EditorGUI.MinMaxSlider(scalerRect, ref scaleMin, ref scaleMax, 0, 1);
		}
	}
}
