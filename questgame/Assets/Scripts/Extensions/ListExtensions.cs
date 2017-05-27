using System;
using System.Collections.Generic;

namespace Extensions
{
	public static class ListExtensions
	{
		public static bool WithinBounds<T>(this List<T> list, int index)
		{
			return index >= 0 || index <= list.Count - 1;
		}

		public static bool AtEdgeIndex<T>(this List<T> list, int index)
		{
			return index <= 0 || index >= list.Count - 1;
		}
	}
}
