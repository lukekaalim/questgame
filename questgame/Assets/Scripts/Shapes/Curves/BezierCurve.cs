using UnityEngine;
using System.Collections.Generic;

namespace Shapes
{
	public class BezierCurve : MonoBehaviour
	{
		[SerializeField]
		List<Vector3> _controlPoints = new List<Vector3>();

#if UNITY_EDITOR
		[SerializeField]
		Color displayColor;

		public void SetColor(Color newColor)
		{
			displayColor = newColor;
		}
#endif

		public Vector3 GetPoint(float position, bool worldPosition = true)
		{
			return worldPosition ? transform.TransformPoint(Bezier.GetPoint(_controlPoints, position)) : Bezier.GetPoint(_controlPoints, position);
		}

		public Vector3 GetControlPoint(int index, bool worldPosition = true)
		{
			return worldPosition ? transform.TransformPoint(_controlPoints[index]) : _controlPoints[index];
		}

		public void SetControlPoint(int index, Vector3 newPosition, bool worldPosition = true)
		{
			_controlPoints[index] = worldPosition ? transform.InverseTransformPoint(newPosition) : newPosition;
		}

		public int GetControlPointCount()
		{
			return _controlPoints.Count;
		}

		public Vector3 GetPointOnPath(float percentage, bool worldSpace = true)
		{
			return GetPoint(percentage, worldSpace);
		}

		public Vector3 GetVelocity(float percentage, bool worldSpace = true)
		{
			return worldSpace ? transform.TransformDirection(Bezier.GetVelocity(_controlPoints, percentage)) : Bezier.GetVelocity(_controlPoints, percentage);
		}

#if UNITY_EDITOR

		void OnDrawGizmos()
		{

			int levelOfDetail = UnityEditor.EditorPrefs.GetInt("splineLevelOfDetail", 10);
			Gizmos.color = displayColor;

			for (int i = 0; i < levelOfDetail; i++)
			{
				float postitionInCurve01 = i / (float)levelOfDetail;
				float postitionInCurve02 = (i + 1) / (float)levelOfDetail;
				Gizmos.DrawLine(GetPoint(postitionInCurve01), GetPoint(postitionInCurve02));
			}

			for (int i = 0; i < GetControlPointCount(); i++)
			{

				Vector3 position = GetControlPoint(i);
				float size = (UnityEditor.HandleUtility.GetHandleSize(position) / 7.5f);

				Gizmos.DrawSphere(position, size);


				if (i < GetControlPointCount() - 1)
				{
					UnityEditor.Handles.color = displayColor;
					UnityEditor.Handles.DrawDottedLine(position, GetControlPoint(i + 1), 10);
				}
			}
		}

		[UnityEditor.MenuItem("GameObject/Shapes/Bezier Curve", false, 10)]
		static void CreateCustomGameObject(UnityEditor.MenuCommand menuCommand)
		{
			// Create a custom game object
			GameObject gameObject = new GameObject("Bezier Curve");
			gameObject.AddComponent<BezierCurve>().SetColor(new Color(UnityEngine.Random.Range(0.5f, 1f), UnityEngine.Random.Range(0.5f, 1f), UnityEngine.Random.Range(0.5f, 1f)));

			// Ensure it gets reparented if this was a context click (otherwise does nothing)
			UnityEditor.GameObjectUtility.SetParentAndAlign(gameObject, menuCommand.context as GameObject);
			// Register the creation in the undo system
			UnityEditor.Undo.RegisterCreatedObjectUndo(gameObject, "Create " + gameObject.name);
		}
#endif
	}
}
