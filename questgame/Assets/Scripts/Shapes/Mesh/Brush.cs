using UnityEngine;
using UnityEditor;

using System.Collections.Generic;

using Attributes;

namespace Shapes
{
	public class Brush : ScriptableObject
	{
		[SerializeField]
		List<ExtrudableTriangle> _extrudingTriangles = new List<ExtrudableTriangle>();

		//all the verticies of the brush
		[SerializeField]
		List<ExtrudableVertex> brushVertices = new List<ExtrudableVertex>();

		[SerializeField]
		List<ExtrudableTriangle> _startingTriangles = new List<ExtrudableTriangle>();
		[SerializeField]
		List<ExtrudableTriangle> _endingTriangles = new List<ExtrudableTriangle>();

		[SerializeField]
		Mesh originalMesh;

		[SerializeField]
		Color startingColor;

		[SerializeField]
		Color endingColor;

		public delegate bool IsOnStartingHalf(Vector3 position);

		public IsOnStartingHalf SortingFunction;

		public Brush()
		{
			SortingFunction = SortByPositiveZ;
		}

		public static Brush CreateNewBrush(string path, Mesh newSourceMesh)
		{
			Brush newBrush = CreateInstance<Brush>();
			newBrush.CalculateBrush(newSourceMesh);

			AssetDatabase.CreateAsset(newBrush, path);

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

			return newBrush;
		}

		public void RecalculateBrush()
		{
			_extrudingTriangles.Clear();
			brushVertices.Clear();
			_startingTriangles.Clear();
			_endingTriangles.Clear();

			CalculateBrush(originalMesh);
		}

		public void CalculateBrush(Mesh _brushMesh)
		{
			originalMesh = _brushMesh;

			Vector3[] meshPoints = _brushMesh.vertices;
			Color[] meshColors = _brushMesh.colors;
			Vector2[] uvCoordinates = _brushMesh.uv;

			int[] triangles = _brushMesh.triangles;

			brushVertices = ExtrudableVertex.MakeFromList(uvCoordinates, meshPoints, meshColors);

			for (int i = 0; i < brushVertices.Count; i++)
			{
				if (SortingFunction(meshPoints[i]))
				{
					meshColors[i] = startingColor;
				}
				else
				{
					meshColors[i] = endingColor;
				}
			}

			for(int i = 0; i < triangles.Length; i+= 3)
			{
				List<int> startingVertices = new List<int>(3);
				List<int> endingVertices = new List<int>(3);
				for (int y = 0; y < 3; y++)
				{
					int actualIndexOfVertex = i + y;
					if(SortingFunction(meshPoints[triangles[actualIndexOfVertex]]))
					{
						startingVertices.Add(triangles[actualIndexOfVertex]);
					}
					else
					{
						endingVertices.Add(triangles[actualIndexOfVertex]);
					}
				}

				if (startingVertices.Count < 3 && endingVertices.Count < 3)
				{
					_extrudingTriangles.Add(new ExtrudableTriangle(startingVertices.ToArray(), endingVertices.ToArray()));
				}
				else
				{
					if(startingVertices.Count == 3)
					{
						_startingTriangles.Add(new ExtrudableTriangle(startingVertices.ToArray(), endingVertices.ToArray()));
					}
					else if(endingVertices.Count == 3)
					{
						_endingTriangles.Add(new ExtrudableTriangle(startingVertices.ToArray(), endingVertices.ToArray()));
					}
				}
			}

			//apply colors
			_brushMesh.colors = meshColors;
		}

		public List<int> FirstSlice(float progress, ref List<ExtrudableVertex> vertexList, ref List<ExtrudableTriangle> triangleList, out List<int> zerothSlice)
		{
			vertexList = new List<ExtrudableVertex>();
			zerothSlice = new List<int>();
			List<int> firstSliceVertcies = new List<int>();

			for (int i = 0; i < _extrudingTriangles.Count; i++)
			{
				int newVerticesIndex = vertexList.Count;

				if (_extrudingTriangles[i].IsSingleStartVertex)
				{
					vertexList.Add(brushVertices[_extrudingTriangles[i].Point0]);
					zerothSlice.Add(vertexList.Count - 1);

					List<ExtrudableVertex> newVertices = new List<ExtrudableVertex>(_extrudingTriangles[i].Lerp(progress, brushVertices));

					vertexList.AddRange(newVertices);

					if (newVertices.Count == 2)
					{
						triangleList.Add(new ExtrudableTriangle( new int[] { newVerticesIndex, newVerticesIndex + 1, newVerticesIndex + 2 }));
						firstSliceVertcies.AddRange(new int[] { newVerticesIndex + 2, newVerticesIndex + 1 });
					}
				}
				else
				{
					vertexList.Add(brushVertices[_extrudingTriangles[i].Point0]);
					zerothSlice.Add(vertexList.Count - 1);
					vertexList.Add(brushVertices[_extrudingTriangles[i].Point1]);
					zerothSlice.Add(vertexList.Count - 1);

					List<ExtrudableVertex> newVertices = new List<ExtrudableVertex>(_extrudingTriangles[i].Lerp(progress, brushVertices));

					vertexList.AddRange(newVertices);

					if (newVertices.Count == 2)
					{
						triangleList.Add(new ExtrudableTriangle(new int[] { newVerticesIndex + 1, newVerticesIndex + 0, newVerticesIndex + 3}));

						triangleList.Add(new ExtrudableTriangle(new int[] { newVerticesIndex,  newVerticesIndex + 2, newVerticesIndex + 3}));

						firstSliceVertcies.AddRange(new int[] { newVerticesIndex + 3, newVerticesIndex + 2 });
					}

				}
			}

			return firstSliceVertcies;
		}

		public List<int> AddSlice(List<int> previousSlice, float progress, ref List<ExtrudableVertex> vertexList, ref List<ExtrudableTriangle> triangleList)
		{
			//The previous slice is a list of all the index's of the slice to connect to
			int currentSliceStart = vertexList.Count;

			int previousSliceIndex = 0;

			List<int> addedVertices = new List<int>();

			for(int i = 0; i < _extrudingTriangles.Count; i++)
			{
				//A list of the new vertcies that were created
				List<ExtrudableVertex> newVertices = new List<ExtrudableVertex>(_extrudingTriangles[i].Lerp(progress, brushVertices));

				vertexList.AddRange(newVertices);

				//sanity check
				if (newVertices.Count == 2)
				{
					triangleList.Add(new ExtrudableTriangle(new int[] { previousSlice[previousSliceIndex] }, new int[] { currentSliceStart, currentSliceStart + 1 }));

					triangleList.Add(new ExtrudableTriangle(new int[] { previousSlice[previousSliceIndex + 1], currentSliceStart }, new int[] { previousSlice[previousSliceIndex] }));

					addedVertices.AddRange(new int[] { currentSliceStart + 1, currentSliceStart });

				}
				previousSliceIndex += newVertices.Count;
				currentSliceStart += newVertices.Count;
			}

			return addedVertices;
		}

		public bool SortByPositiveX(Vector3 position)
		{
			return position.x > 0;
		}

		public bool SortByPositiveY(Vector3 position)
		{
			return position.y > 0;
		}

		public bool SortByPositiveZ(Vector3 position)
		{
			return position.z > 0;
		}

		public static Mesh CreateMeshFromList(List<ExtrudableVertex> vertexList, List<ExtrudableTriangle> triangleList)
		{
			Mesh createdMesh = new Mesh();

			List<Vector3> positions = new List<Vector3>(vertexList.Count);
			List<Color> colors = new List<Color>(vertexList.Count);
			List<Vector2> uvws = new List<Vector2>(vertexList.Count);

			for(int i = 0; i < vertexList.Count; i++)
			{
				positions.Add(vertexList[i].Position);
				colors.Add(vertexList[i].Color);
				uvws.Add(vertexList[i].UV);
			}

			createdMesh.vertices = positions.ToArray();
			createdMesh.colors = colors.ToArray();
			createdMesh.uv = uvws.ToArray();

			List<int> triangles = new List<int>(triangleList.Count * 3);

			for (int i = 0; i < triangleList.Count; i++)
			{
				triangles.AddRange(triangleList[i].PointsAsArray);
			}

			createdMesh.triangles = triangles.ToArray();

			return createdMesh;
		}

		[System.Serializable]
		public struct ExtrudableVertex
		{
			[ReadOnly]
			public Vector3 Position;
			[ReadOnly]
			public Color Color;
			[ReadOnly]
			public Vector2 UV;

			public ExtrudableVertex(Vector2 uvs, Vector3 position, Color color)
			{
				Position = position;
				Color = color;
				UV = uvs;
			}

			public static List<ExtrudableVertex> MakeFromList(Vector2[] uvs, Vector3[] positions, Color[] colors)
			{
				int length = positions.Length;

				List<ExtrudableVertex> vertexList = new List<ExtrudableVertex>(length);

				for (int i = 0; i < length; i++)
				{
					Color colorToSet = colors.Length > i ? colors[i] : Color.white;
					Vector2 uvToSet = uvs.Length > i ? uvs[i] : Vector2.zero;

					vertexList.Add(new ExtrudableVertex(uvToSet, positions[i], colorToSet));
				}

				return vertexList;
			}

			public static ExtrudableVertex Lerp(ExtrudableVertex start, ExtrudableVertex end, float progress)
			{
				Vector3 position = Vector3.Lerp(start.Position, end.Position, progress);
				Color color = Color.Lerp(start.Color, end.Color, progress);
				Vector2 uv = Vector2.Lerp(start.UV, end.UV, progress);

				return new ExtrudableVertex(uv, position, color);
			}
		}

		[System.Serializable]
		public struct ExtrudableTriangle
		{
			//The three points that compose a triangle
			[ReadOnly]
			public int Point0;
			[ReadOnly]
			public int Point1;
			[ReadOnly]
			public int Point2;
			[SerializeField, ReadOnly]
			bool _isExtrudable;
			public bool IsExtrudable
			{
				get
				{
					return _isExtrudable;
				}
			}
			//wether point0 alone or points 0 and 1 are on the starting side.
			[SerializeField, ReadOnly]
			private bool _isSingleStartingVertex;
			public bool IsSingleStartVertex
			{
				get
				{
					return _isSingleStartingVertex;
				}
			}

			public int[] PointsAsArray
			{
				get
				{
					return new int[] { Point0, Point1, Point2 };
				}
			}

			public ExtrudableTriangle(int[] startingVerts, int[] endingVerts)
			{
				if (startingVerts.Length == 1)
				{
					Point0 = startingVerts[0];
					Point1 = endingVerts[0];
					Point2 = endingVerts[1];

					_isSingleStartingVertex = true;
					_isExtrudable = true;
				}
				else if(endingVerts.Length == 1)
				{
					Point0 = startingVerts[0];
					Point1 = startingVerts[1];
					Point2 = endingVerts[0];

					_isSingleStartingVertex = false;
					_isExtrudable = true;
				}
				else
				{
					_isExtrudable = false;
					_isSingleStartingVertex = false;
					if (endingVerts.Length == 3)
					{
						Point0 = endingVerts[0];
						Point1 = endingVerts[1];
						Point2 = endingVerts[2];
					}

					else if (startingVerts.Length == 3)
					{
						Point0 = startingVerts[0];
						Point1 = startingVerts[1];
						Point2 = startingVerts[2];
					}
					else
					{
						Point0 = 0;
						Point1 = 0;
						Point2 = 0;
					}

				}
			}

			public ExtrudableTriangle(int[] verts)
			{
				Point0 = verts[0];
				Point1 = verts[1];
				Point2 = verts[2];

				_isSingleStartingVertex = false;
				_isExtrudable = false;
			}

			public ExtrudableVertex[] Lerp(float progress, List<ExtrudableVertex> vertexList)
			{
				progress = Mathf.Clamp01(progress);
				ExtrudableVertex[] result = new ExtrudableVertex[2];

				if (IsSingleStartVertex)
				{
					result[0] = ExtrudableVertex.Lerp(vertexList[Point0], vertexList[Point1], progress);
					result[1] = ExtrudableVertex.Lerp(vertexList[Point0], vertexList[Point2], progress);
				}
				else
				{
					result[0] = ExtrudableVertex.Lerp(vertexList[Point0], vertexList[Point2], progress);
					result[1] = ExtrudableVertex.Lerp(vertexList[Point1], vertexList[Point2], progress);
				}

				return result;
			}
		}
	}
}
