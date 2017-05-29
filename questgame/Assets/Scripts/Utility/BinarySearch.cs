using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility
{
	public delegate int ComparerDelegate<T>(List<T> list, int index, T target);

	public static class Search
	{
		public static int Binary<T>(List<T> list, T target, ComparerDelegate<T> comparer)
		{
			int min = 0;
			int max = list.Count - 1;


			int pivot = (min + max) / 2;
			while (min <= max)
			{
				int comparisonResult = comparer(list, pivot, target);
				if (comparisonResult < 0)
				{
					min = pivot + 1;
					pivot = (min + max) / 2;
				}
				else if (comparisonResult > 0)
				{
					max = pivot - 1;
					pivot = (min + max) / 2;
				}
				else if (comparisonResult == 0)
				{
					return pivot;
				}
			}
			return -1;
		}
	}
}
