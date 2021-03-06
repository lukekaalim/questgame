﻿using UnityEditor;

namespace Route
{
	[CustomEditor(typeof(Traveller), true)]
	public class TravellerInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			Traveller traveller = target as Traveller;

			if (!EditorApplication.isPlaying && traveller.CurrentGenericRoute != null)
			{
				traveller.UpdatePosition();
			}
		}
	}
}
