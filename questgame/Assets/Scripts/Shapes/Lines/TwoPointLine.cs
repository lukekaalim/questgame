using System.Collections.Generic;
using UnityEngine;

namespace Shapes
{
	//A simple line made of two points.
	public class TwoPointLine : Line, IPointLine
	{
		[SerializeField]
		Color _displayColor = Color.white;

		[SerializeField, HideInInspector]
		Vector3 P0 = new Vector3();

		[SerializeField, HideInInspector]
		Vector3 P1 = new Vector3();

		[SerializeField, HideInInspector]
		float _length;

		public override int PointCount
		{
			get
			{
				return 2;
			}
		}

		public override Color DisplayColor
		{
			get
			{
				return _displayColor;
			}
		}

		float IPointLine.AbsoluteLength
		{
			get
			{
				return _length;
			}
		}

		public void UpdateLength()
		{
			_length = Vector3.Distance(P0, P1);
		}

		public override Vector3 this[int index, bool global = true]
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
				UpdateLength();
			}
		}

		public TwoPointLine()
		{
			P0 = new Vector3();
			P1 = new Vector3();
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
			UpdateLength();
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
			UpdateLength();
		}

		public override Vector3 GetPointOnPath(float percentage, bool worldSpace = true)
		{
			return Vector3.Lerp(this[0, worldSpace], this[1, worldSpace], percentage);
		}

		Vector3 IPointLine.GetStartPosition()
		{
			return P0;
		}

		Vector3 IPointLine.GetEndPosition()
		{
			return P1;
		}

		public Vector3 GetPointAlongDistance(float distance)
		{
			return Vector3.Lerp(P0, P1, distance / _length);
		}

		Vector3 IPointLine.GetPointAlongDistance(int startingIndex, float distanceFromIndex)
		{
			return Vector3.Lerp(P0, P1, distanceFromIndex / _length);
		}

		Vector3 IPointLine.GetVelocityAtIndex(int startingIndex)
		{
			return P1 - P0;
		}

		Vector3 IPointLine.GetRelativePoint(float distance)
		{
			return Vector3.Lerp(P0, P1, distance / _length);
		}

		int IPointLine.GetLeftMostIndex(float distance)
		{
			return 0;
		}

		Vector3 IPointLine.AdvanceAlongLine(ref int index, ref float distance, out float newTotalDistance, out float pointProgress)
		{
			index = 0;
			pointProgress = distance / _length;
			newTotalDistance = distance;
			return Vector3.Lerp(P0, P1, pointProgress);
		}
		public Vector3 AdvanceAlongLine(ref int index, ref float distance, out float newTotalDistance)
		{
			index = 0;
			newTotalDistance = distance;
			return Vector3.Lerp(P0, P1, distance / _length);
		}

		public Vector3 AdvanceAlongLine(ref int index, ref float distance)
		{
			index = 0;
			return Vector3.Lerp(P0, P1, distance / _length);
		}

		public List<Vector3> GetPointSample(float startPoint, float endPoint)
		{
			List<Vector3> samples = new List<Vector3>();
			samples.Add(GetPointAlongDistance(startPoint));
			samples.Add(GetPointAlongDistance(endPoint));
			return samples;
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

		void OnDrawGizmos()
		{
			Gizmos.color = _displayColor;

			Gizmos.DrawLine(this[0], this[1]);

			Gizmos.DrawSphere(this[0], UnityEditor.HandleUtility.GetHandleSize(this[0]) * 0.3f);
			Gizmos.DrawSphere(this[1], UnityEditor.HandleUtility.GetHandleSize(this[1]) * 0.3f);
		}
#endif
	}
}
