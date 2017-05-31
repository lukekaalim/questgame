using UnityEngine;

using System.Collections.Generic;

namespace Shapes
{
	[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter)), ExecuteInEditMode]
	public class ExtrudableMesh : MonoBehaviour
	{
		[SerializeField]
		Brush _brush;

		[SerializeField]
		MeshFilter _filter;

		[SerializeField]
		BezierSpline _spline;

		[SerializeField]
		int sampleCount;

		[SerializeField]
		Color jointColors;

		public bool IsValidForGeneration
		{
			get
			{
				return _brush != null && _filter != null && _spline != null;
			}
		}

		void OnEnable()
		{
			if (_spline != null)
				_spline.OnLineResample += Resample;
		}

		void OnDisable()
		{
			if (_spline != null)
				_spline.OnLineResample -= Resample;
		}

		private void Resample(CompoundLine line)
		{
			if (IsValidForGeneration)
				GenerateMesh();
		}

		public Brush Brush
		{
			get
			{
				return _brush;
			}
		}

		public void GenerateMesh()
		{
			List<Brush.ExtrudableVertex> vertexList = new List<Brush.ExtrudableVertex>();
			List<Brush.ExtrudableTriangle> triangleList = new List<Brush.ExtrudableTriangle>();

			if (sampleCount > 0)
			{
				float progress = 1 / (float)sampleCount;

				List<int> firstSlice;
				List<int> previousSlice = _brush.FirstSlice(progress, ref vertexList, ref triangleList, out firstSlice);

				TransformVertices(ref vertexList, firstSlice, _spline.GetPointOnPath(0), Quaternion.LookRotation(_spline.GetVelocity(0)));
				TransformVertices(ref vertexList, previousSlice, _spline.GetPointOnPath(progress), Quaternion.LookRotation(_spline.GetVelocity(progress)));

				for (int i = 1; i < sampleCount; i++)
				{
					progress = (1 + i) / (float)sampleCount;
					previousSlice = _brush.AddSlice(previousSlice, progress, ref vertexList, ref triangleList);
					TransformVertices(ref vertexList, previousSlice, _spline.GetPointOnPath(progress), Quaternion.LookRotation(_spline.GetVelocity(progress)));
				}
			}

			_filter.sharedMesh = Brush.CreateMeshFromList(vertexList, triangleList);
			_filter.sharedMesh.RecalculateNormals();
			_filter.sharedMesh.name = name + " [Extruded Mesh]";
		}

		public void TransformVertices(ref List<Brush.ExtrudableVertex> vertexList, List<int> vertexIndex, Vector3 position, Quaternion direction)
		{
			List<Vector3> points = new List<Vector3>();
			for (int i = 0; i < vertexIndex.Count; i++)
			{
				points.Add(vertexList[vertexIndex[i]].Position);
			}

			Vector3 center = GetCenter(points);

			for (int i = 0; i < points.Count; i++)
			{
				Brush.ExtrudableVertex oldVertex = vertexList[vertexIndex[i]];

				Vector3 centeredPosition = oldVertex.Position - center;

				Matrix4x4 matrix = Matrix4x4.TRS(position, direction, Vector3.one);

				Brush.ExtrudableVertex newVertex = new Brush.ExtrudableVertex(oldVertex.UV, matrix.MultiplyPoint(centeredPosition), oldVertex.Color);

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
