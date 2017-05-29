using System;
using System.Collections.Generic;

namespace Utility
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

		public static bool IsLastIndex<T>(this List<T> list, int index)
		{
			return index == list.Count - 1;
		}

		public static bool IsFirstIndex<T>(this List<T> list, int index)
		{
			return index == 0;
		}
	}
}
