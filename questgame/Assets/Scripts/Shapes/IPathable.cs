using UnityEngine;

namespace Shapes
{
	//represents a set of functions that can be used to traverse a path
	public interface IPathable
	{
		Vector3 GetPointOnPath(float percentage, bool worldSpace = true);
	}
}
