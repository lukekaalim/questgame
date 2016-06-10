using UnityEngine;

namespace Shapes
{
	/*
	 *
	 *	Describes and interface that allows you to move at a fixed rate though a path
	 *
	 */
	public interface ILinearTraversable : IPathable
	{
		Vector3 GetStartPosition();

		Vector3 GetEndPosition();

		Vector3 GetPointAlongDistance(float distance);

		Vector3 GetPointAlongDistance(int startingIndex, float distanceFromIndex);

		Vector3 GetVelocityAtIndex(int startingIndex);

		Vector3 GetRelativePoint(float distance);

		Vector3 AdvanceAlongLine(int startingIndex, float newDistance, out int endIndex, out float endDistance, out float newTotalDistance);

		float AbsoluteLength
		{
			get;
		}
	}
}
