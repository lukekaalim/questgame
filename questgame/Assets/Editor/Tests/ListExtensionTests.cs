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
	public class ListExtensionTests
	{
		[Test]
		public void IsLastIndexReturnsTrueIfInputIsOneLessThanListCount()
		{
			List<int> list = new List<int>(new int[5]);

			Assert.That(list.IsLastIndex(4), Is.True);

			Assert.That(list.IsLastIndex(5), Is.False);
			Assert.That(list.IsLastIndex(0), Is.False);
			Assert.That(list.IsLastIndex(100), Is.False);
		}
	}
}
