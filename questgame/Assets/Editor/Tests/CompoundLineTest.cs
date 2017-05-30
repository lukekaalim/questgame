using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

using Shapes;

namespace Testing
{
	public class CompoundLineTest
	{
		[UnityTest]
		public IEnumerator RecalculateLengthReturnsDistanceFromFirstAndLastPointInAStraightLine()
		{
			CompoundLine line = ScriptableObject.CreateInstance<CompoundLine>();

			List<Vector3> points = new List<Vector3>
			{
				new Vector3(0, 0, 0),
				new Vector3(10, 0, 0),
				new Vector3(20, 0, 0)
			};
			float expectedTotalDistance = 20;

			line.Points = points;
			line.Recalculate();

			Assert.That(line.TotalLength, Is.EqualTo(expectedTotalDistance).Within(0.1f),
				"The total length did not approximatley equal the expected length");
			return null;
		}

		[UnityTest]
		public IEnumerator RecalculateLengthsReturnsTotalDistanceAlongLine()
		{
			CompoundLine line = ScriptableObject.CreateInstance<CompoundLine>();

			List<Vector3> points = new List<Vector3>
			{
				new Vector3(0, 0, 0),
				new Vector3(10, 0, 0),
				new Vector3(10, 10, 0)
			};
			float expectedTotalDistance = 20;

			line.Points = points;
			line.Recalculate();

			Assert.That(line.TotalLength, Is.EqualTo(expectedTotalDistance).Within(0.1f),
				"The total length did not approximatley equal the expected length");
			return null;
		}

		[UnityTest]
		public IEnumerator FindFirstSegmentPoint()
		{
			CompoundLine line = ScriptableObject.CreateInstance<CompoundLine>();

			List<Vector3> points = new List<Vector3>
			{
				new Vector3(0, 0, 0),
				new Vector3(10, 0, 0),
				new Vector3(20, 0, 0)
			};
			line.Points = points;
			line.Recalculate();

			CompoundLine.Position linePosition = line.GetPosition(0);
			Vector3 positionVector = line.ResolvePosition(linePosition);

			Assert.That(positionVector, Is.EqualTo(points[0]),
				"At distance zero is not equal to the beginning of the line");

			return null;
		}
	}
}