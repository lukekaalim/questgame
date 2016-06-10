using System;
using UnityEngine;

namespace Shapes
{
	//A simple line made of two points.
	class TwoPointLine : Line
	{
		[SerializeField]
		Vector3 P0, P1;

		public override int PointCount
		{
			get
			{
				return 2;
			}
		}

		public override Vector3 this[int index, bool global]
		{
			get
			{
				return global ? GetWorldPosition(index) : GetLocalPosition(index);
			}

			set
			{
				if (global)
				{
					SetWorldPosition(index, value);
				}
				else
				{
					SetLocalPosition(index, value);
				}
			}
		}

		public TwoPointLine()
		{
			P0 = P1 = new Vector3();
		}

		public Vector3 GetLocalPosition(int index)
		{
			if (index == 0)
			{
				return P0;
			}
			else
			{
				return P1;
			}
		}

		public void SetLocalPosition(int index, Vector3 newPosition)
		{
			if (index == 0)
			{
				P0 = newPosition;
			}
			else
			{
				P1 = newPosition;
			}
		}

		public Vector3 GetWorldPosition(int index)
		{
			if (index == 0)
			{
				return transform.TransformPoint(P0);
			}
			else
			{
				return transform.TransformPoint(P1);
			}
		}

		public void SetWorldPosition(int index, Vector3 newPosition)
		{
			if (index == 0)
			{
				P0 = transform.InverseTransformPoint(newPosition);
			}
			else
			{
				P1 = transform.InverseTransformPoint(newPosition);
			}
		}

		public override Vector3 GetPointOnPath(float percentage, bool worldSpace = true)
		{
			return Vector3.Lerp(this[0, worldSpace], this[1, worldSpace], percentage);
		}

#if UNITY_EDITOR


		[UnityEditor.MenuItem("GameObject/Shapes/Lines/Two Point Line", false, 10)]
		static void CreateCustomGameObject(UnityEditor.MenuCommand menuCommand)
		{
			// Create a custom game object
			GameObject gameObject = new GameObject("Two Point Line");
			gameObject.AddComponent<TwoPointLine>();

			// Ensure it gets reparented if this was a context click (otherwise does nothing)
			UnityEditor.GameObjectUtility.SetParentAndAlign(gameObject, menuCommand.context as GameObject);
			// Register the creation in the undo system
			UnityEditor.Undo.RegisterCreatedObjectUndo(gameObject, "Create " + gameObject.name);
		}


#endif
	}
}
