using UnityEditor;
using UnityEngine;
using Utility;
using System.Collections.Generic;
using Shapes.Editors.CompoundLineEditors;

namespace Shapes.Editors
{
	// TODO split and refactor this into mode sub classes
	public class CompoundLineEditor : IVisualEditor
	{
		CompoundLine target;
		SelectorBar selectorBar;
		ScalerBar scaleBar;
		PointHandler sceneHandlers;
		
		HashSet<int> selection = new HashSet<int>();

		public CompoundLineEditor(CompoundLine target)
		{
			this.target = target;
			selectorBar = new SelectorBar(this);
			scaleBar = new ScalerBar();
			sceneHandlers = new PointHandler(this);
		}

		public CompoundLine Target { get { return target; } }
		public HashSet<int> Selection { get { return selection; } }
		public ScalerBar ScaleBar { get { return scaleBar; } }

		public void DrawGUI()
		{
			scaleBar.DrawScalebar();
			selectorBar.DrawSelectorBar();
		}

		public void RenderScene()
		{
			sceneHandlers.DrawPointHandlers();
		}

		public void SelectPoint(int pointIndex)
		{
			if (!Event.current.shift)
			{
				selection.Clear();
			}
			selection.Add(pointIndex);
		}
	}
}
