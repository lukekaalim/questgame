using UnityEngine;
using System.Collections.Generic;

namespace Utility
{
	public static class VectorUtilities
	{
		public static Vector3 AverageVectors(IEnumerable<Vector3> vectorList)
		{
			Vector3 result = Vector3.zero;
			int count = 0;
			foreach(Vector3 vector in vectorList)
			{
				result += vector;
				count++;
			}
			result /= count;
			return result;
		}
	}
}
