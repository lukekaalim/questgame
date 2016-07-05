using UnityEngine;

using System.Collections.Generic;

namespace Shapes
{
	[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
	public class ExtrudableMesh : MonoBehaviour
	{
		[SerializeField]
		Brush _brush;

		[SerializeField]
		MeshFilter _filter;

		[SerializeField]
		BezierSpline _spline;

		[SerializeField]
		int splitAmount;

		[SerializeField]
		List<Vector3> positionOffsets;

		[SerializeField]
		Color jointColors;

		public bool IsValidForGeneration
		{
			get
			{
				return _brush != null && _filter != null && _spline != null;
			}
		}

		public void GenerateMesh()
		{
			List<Brush.ExtrudableVertex> vertexList = new List<Brush.ExtrudableVertex>();
			List<Brush.ExtrudableTriangle> triangleList = new List<Brush.ExtrudableTriangle>();

			if (splitAmount > 0)
			{
				List<int> firstSlice;
				List<int> previousSlice = _brush.FirstSlice(1 / (float)splitAmount, ref vertexList, ref triangleList, out firstSlice);

				TransformVertices(ref vertexList, firstSlice, positionOffsets[0]);
				TransformVertices(ref vertexList, previousSlice, positionOffsets[1]);

				for (int i = 1; i < splitAmount; i++)
				{
					previousSlice = _brush.AddSlice(previousSlice, (1 + i) / (float)splitAmount, ref vertexList, ref triangleList);
					TransformVertices(ref vertexList, previousSlice, positionOffsets[i + 1]);
				}
			}

			_filter.sharedMesh = Brush.CreateMeshFromList(vertexList, triangleList);
		}

		public void TransformVertices(ref List<Brush.ExtrudableVertex> vertexList, List<int> vertexIndex, Vector3 position)
		{
			List<Vector3> points = new List<Vector3>();
			for(int i = 0; i < vertexIndex.Count; i++)
			{
				points.Add(vertexList[vertexIndex[i]].Position);
			}

			Vector3 center = GetCenter(points);
			Debug.Log(center);

			for(int i = 0; i < points.Count; i++)
			{
				Brush.ExtrudableVertex oldVertex = vertexList[vertexIndex[i]];

				Vector3 newPosition = oldVertex.Position - center + position;

				Brush.ExtrudableVertex newVertex = new Brush.ExtrudableVertex(oldVertex.UV, newPosition, oldVertex.Color);

				vertexList[vertexIndex[i]] = newVertex;
			}
		}

		public Vector3 GetCenter(List<Vector3> points)
		{
			/*float xSum = 0;
			float ySum = 0;
			float zSum = 0;

			int count = points.Count;
			for(int i = 0; i < count; i++)
			{
				xSum += points[i].x;
				ySum += points[i].y;
				zSum += points[i].z;
			}
			*/

			//return new Vector3(xSum / count, ySum / count, zSum / count);

			Bounds bounds = new Bounds(points[0], Vector3.zero);

			for (int i = 1; i < points.Count; i++)
			{
				bounds.Encapsulate(points[i]);
			}

			return bounds.center;
		}
	}
}
