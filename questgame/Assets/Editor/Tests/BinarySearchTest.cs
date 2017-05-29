using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

using Utility;

namespace Testing
{
	public class BinarySearchTest
	{
		[Test]
		public void BinarySeachCanFindSpecificFloat()
		{
			float search = 5f;
			List<float> sortedList = new List<float>(new float[] { 0f, 1f, 2f, 3f, 4f, 5f, 8f, 20f });

			int resultIndex = Search.Binary(sortedList, search, (List<float> list, int index, float target) =>
			{
				float value = list[index];

				if (value < target)
				{
					return -1;
				}
				else if (value > target)
				{
					return 1;
				}
				else
				{
					return 0;
				}
			});

			// I don't trust float equality
			Assert.That(sortedList[resultIndex], Is.EqualTo(search).Within(0.01f));
		}

		[Test]
		public void BinarySeachCanFindSpecificInt()
		{
			List<int> sortedList = new List<int>(new int[] { -1, 0, 1, 2, 3, 5, 100, 1000 });

			ComparerDelegate<int> comparer = (List<int> list, int index, int target) =>
			{
				int value = list[index];

				if (value < target)
				{
					return -1;
				}
				else if (value > target)
				{
					return 1;
				}
				else
				{
					return 0;
				}
			};

			// test to find every value in the array
			foreach (int testedValue in sortedList)
			{
				Assert.That(sortedList[Search.Binary(sortedList, testedValue, comparer)], Is.EqualTo(testedValue));
			}
		}

		[Test]
		public void BinarySeachCanFindLeftMostIndexOfTarget()
		{
			List<float> sortedList = new List<float>(new float[] { 0f, 1f, 2f, 3f, 4f, 5f, 8f, 20f });
			List<float> targets = new List<float>(new float[] { 0.5f, 1.25f, 2.75f, 3.91f, 4.01f, 7f, 15.25f });

			ComparerDelegate<float> comparer = (List<float> list, int index, float target) =>
			{
				float value = list[index];

				if (value < target)
				{
					if (list.IsLastIndex(index) || list[index + 1] > target)
					{
						return 0;
					}
					return -1;
				}
				else if (value > target)
				{
					if (list.IsFirstIndex(index))
					{
						return 0;
					}
					return 1;
				}
				else
				{
					return 0;
				}
			};

			for (int i = 0; i < targets.Count; i++)
			{
				int result = Search.Binary(sortedList, targets[i], comparer);
				Assert.That(
					sortedList[result],
					Is.EqualTo(sortedList[i]),
					String.Format("Searching for {0}, expecting {1}", targets[i], sortedList[i])
					);
			}

			Assert.That(sortedList[Search.Binary(sortedList, 100f, comparer)], Is.EqualTo(20f));
			Assert.That(sortedList[Search.Binary(sortedList, -100f, comparer)], Is.EqualTo(0f));
		}
	}
}