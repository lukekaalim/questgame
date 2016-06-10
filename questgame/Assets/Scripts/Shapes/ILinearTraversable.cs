using UnityEngine;

namespace Shapes
{
	/*
	 *
	 *	Describes and interface that allows you to move at a fixed rate though a path defined by a set of points
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

		int GetIndexAtPoint(float distance);

		/*
		 *	This is the big important function.
		 *	Index refers to the index of the point we start at in the line.
		 *	Distance is the distance from the beginning of that point along the line we travel
		 *	in Actual Units. this may be bigger that the length of the actual pair of points,
		 *	so in that case we return the new index and distance.
		 *	New Total Distance refers to the total distance along the line we have travelled.
		 *	Point Progress is the relative distance from the point we have travelled, clamped from
		 *	0 to 1.
		 */
		Vector3 AdvanceAlongLine(ref int index, ref float distance, out float newTotalDistance, out float pointProgress);

		float AbsoluteLength
		{
			get;
		}
	}
}
