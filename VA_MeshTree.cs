using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000222 RID: 546
[Serializable]
public class VA_MeshTree
{
	// Token: 0x06000C55 RID: 3157 RVA: 0x00040E37 File Offset: 0x0003F037
	public void Clear()
	{
		this.Nodes.Clear();
		this.Triangles.Clear();
	}

	// Token: 0x06000C56 RID: 3158 RVA: 0x00040E50 File Offset: 0x0003F050
	public void Update(Mesh mesh)
	{
		this.Clear();
		if (mesh != null)
		{
			VA_MeshTree.Node node = new VA_MeshTree.Node();
			this.Nodes.Add(node);
			List<VA_Triangle> allTriangles = this.GetAllTriangles(mesh);
			this.Pack(node, allTriangles);
		}
	}

	// Token: 0x06000C57 RID: 3159 RVA: 0x00040E90 File Offset: 0x0003F090
	public Vector3 FindClosestPoint(Vector3 point)
	{
		if (this.Nodes.Count > 0)
		{
			VA_MeshTree.searchResults.Clear();
			VA_MeshTree.searchRange = float.PositiveInfinity;
			VA_MeshTree.searchPoint = point;
			this.Search(this.Nodes[0]);
			float num = float.PositiveInfinity;
			Vector3 result = point;
			for (int i = VA_MeshTree.searchResults.Count - 1; i >= 0; i--)
			{
				Vector3 vector = VA_MeshTree.searchResults[i].ClosestTo(point);
				float num2 = vector.x - point.x;
				float num3 = vector.y - point.y;
				float num4 = vector.z - point.z;
				float num5 = num2 * num2 + num3 * num3 + num4 * num4;
				if (num5 < num)
				{
					num = num5;
					result = vector;
				}
			}
			return result;
		}
		return point;
	}

	// Token: 0x06000C58 RID: 3160 RVA: 0x00040F58 File Offset: 0x0003F158
	private void Search(VA_MeshTree.Node node)
	{
		if (node.TriangleCount > 0)
		{
			this.AddToResults(node);
			return;
		}
		Bounds bound = node.Bound;
		if (bound.SqrDistance(VA_MeshTree.searchPoint) - 0.001f > VA_MeshTree.searchRange)
		{
			return;
		}
		float num = this.MaximumDistance(bound.min, bound.max);
		if (num + 0.001f < VA_MeshTree.searchRange)
		{
			VA_MeshTree.searchRange = num + 0.01f;
		}
		if (node.PositiveIndex != 0)
		{
			this.Search(this.Nodes[node.PositiveIndex]);
		}
		if (node.NegativeIndex != 0)
		{
			this.Search(this.Nodes[node.NegativeIndex]);
		}
	}

	// Token: 0x06000C59 RID: 3161 RVA: 0x00041004 File Offset: 0x0003F204
	private float MaximumDistance(Vector3 min, Vector3 max)
	{
		VA_MeshTree.searchMaximum = 0f;
		VA_MeshTree.FarSqrDistance(min.x, min.y, min.z);
		VA_MeshTree.FarSqrDistance(max.x, min.y, min.z);
		VA_MeshTree.FarSqrDistance(min.x, min.y, max.z);
		VA_MeshTree.FarSqrDistance(max.x, min.y, max.z);
		VA_MeshTree.FarSqrDistance(min.x, max.y, min.z);
		VA_MeshTree.FarSqrDistance(max.x, max.y, min.z);
		VA_MeshTree.FarSqrDistance(min.x, max.y, max.z);
		VA_MeshTree.FarSqrDistance(max.x, max.y, max.z);
		return VA_MeshTree.searchMaximum;
	}

	// Token: 0x06000C5A RID: 3162 RVA: 0x000410D8 File Offset: 0x0003F2D8
	private static void FarSqrDistance(float x, float y, float z)
	{
		x -= VA_MeshTree.searchPoint.x;
		y -= VA_MeshTree.searchPoint.y;
		z -= VA_MeshTree.searchPoint.z;
		float num = x * x + y * y + z * z;
		if (num > VA_MeshTree.searchMaximum)
		{
			VA_MeshTree.searchMaximum = num;
		}
	}

	// Token: 0x06000C5B RID: 3163 RVA: 0x0004112C File Offset: 0x0003F32C
	private void AddToResults(VA_MeshTree.Node node)
	{
		for (int i = node.TriangleIndex; i < node.TriangleIndex + node.TriangleCount; i++)
		{
			VA_MeshTree.searchResults.Add(this.Triangles[i]);
		}
	}

	// Token: 0x06000C5C RID: 3164 RVA: 0x0004116C File Offset: 0x0003F36C
	private List<VA_Triangle> GetAllTriangles(Mesh mesh)
	{
		List<VA_Triangle> list = new List<VA_Triangle>();
		Vector3[] vertices = mesh.vertices;
		for (int i = 0; i < mesh.subMeshCount; i++)
		{
			if (mesh.GetTopology(i) == MeshTopology.Triangles)
			{
				int[] triangles = mesh.GetTriangles(i);
				for (int j = 0; j < triangles.Length; j += 3)
				{
					VA_Triangle va_Triangle = new VA_Triangle();
					list.Add(va_Triangle);
					va_Triangle.A = vertices[triangles[j]];
					va_Triangle.B = vertices[triangles[j + 1]];
					va_Triangle.C = vertices[triangles[j + 2]];
					va_Triangle.CalculatePlanes();
				}
			}
		}
		return list;
	}

	// Token: 0x06000C5D RID: 3165 RVA: 0x00041208 File Offset: 0x0003F408
	private void Pack(VA_MeshTree.Node node, List<VA_Triangle> tris)
	{
		this.CalculateBound(node, tris);
		if (tris.Count < 5)
		{
			node.TriangleIndex = this.Triangles.Count;
			node.TriangleCount = tris.Count;
			this.Triangles.AddRange(tris);
			return;
		}
		List<VA_Triangle> list = new List<VA_Triangle>();
		List<VA_Triangle> list2 = new List<VA_Triangle>();
		int num = 0;
		float num2 = 0f;
		this.CalculateAxisAndPivot(tris, ref num, ref num2);
		switch (num)
		{
		case 0:
			using (List<VA_Triangle>.Enumerator enumerator = tris.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					VA_Triangle va_Triangle = enumerator.Current;
					if (va_Triangle.MidX >= num2)
					{
						list.Add(va_Triangle);
					}
					else
					{
						list2.Add(va_Triangle);
					}
				}
				goto IL_14C;
			}
			break;
		case 1:
			break;
		case 2:
			goto IL_104;
		default:
			goto IL_14C;
		}
		using (List<VA_Triangle>.Enumerator enumerator = tris.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				VA_Triangle va_Triangle2 = enumerator.Current;
				if (va_Triangle2.MidY >= num2)
				{
					list.Add(va_Triangle2);
				}
				else
				{
					list2.Add(va_Triangle2);
				}
			}
			goto IL_14C;
		}
		IL_104:
		foreach (VA_Triangle va_Triangle3 in tris)
		{
			if (va_Triangle3.MidZ >= num2)
			{
				list.Add(va_Triangle3);
			}
			else
			{
				list2.Add(va_Triangle3);
			}
		}
		IL_14C:
		if (list.Count == 0 || list2.Count == 0)
		{
			list.Clear();
			list2.Clear();
			int num3 = tris.Count / 2;
			for (int i = 0; i < num3; i++)
			{
				list.Add(tris[i]);
			}
			for (int j = num3; j < tris.Count; j++)
			{
				list2.Add(tris[j]);
			}
		}
		node.PositiveIndex = this.Nodes.Count;
		VA_MeshTree.Node node2 = new VA_MeshTree.Node();
		this.Nodes.Add(node2);
		this.Pack(node2, list);
		node.NegativeIndex = this.Nodes.Count;
		VA_MeshTree.Node node3 = new VA_MeshTree.Node();
		this.Nodes.Add(node3);
		this.Pack(node3, list2);
	}

	// Token: 0x06000C5E RID: 3166 RVA: 0x00041450 File Offset: 0x0003F650
	private void CalculateBound(VA_MeshTree.Node node, List<VA_Triangle> tris)
	{
		if (tris.Count > 0)
		{
			Vector3 vector = tris[0].Min;
			Vector3 vector2 = tris[0].Max;
			foreach (VA_Triangle va_Triangle in tris)
			{
				vector = Vector3.Min(vector, va_Triangle.Min);
				vector2 = Vector3.Max(vector2, va_Triangle.Max);
			}
			node.Bound.SetMinMax(vector, vector2);
		}
	}

	// Token: 0x06000C5F RID: 3167 RVA: 0x000414E4 File Offset: 0x0003F6E4
	private void CalculateAxisAndPivot(List<VA_Triangle> tris, ref int axis, ref float pivot)
	{
		Vector3 vector = tris[0].Min;
		Vector3 vector2 = tris[0].Max;
		Vector3 vector3 = Vector3.zero;
		foreach (VA_Triangle va_Triangle in tris)
		{
			vector = Vector3.Min(vector, va_Triangle.Min);
			vector2 = Vector3.Max(vector2, va_Triangle.Max);
			vector3 += va_Triangle.A + va_Triangle.B + va_Triangle.C;
		}
		Vector3 vector4 = vector2 - vector;
		if (vector4.x > vector4.y && vector4.x > vector4.z)
		{
			axis = 0;
			pivot = VA_Helper.Divide(vector3.x, (float)tris.Count * 3f);
			return;
		}
		if (vector4.y > vector4.x && vector4.y > vector4.z)
		{
			axis = 1;
			pivot = VA_Helper.Divide(vector3.y, (float)tris.Count * 3f);
			return;
		}
		axis = 2;
		pivot = VA_Helper.Divide(vector3.z, (float)tris.Count * 3f);
	}

	// Token: 0x04000EC5 RID: 3781
	public List<VA_MeshTree.Node> Nodes = new List<VA_MeshTree.Node>();

	// Token: 0x04000EC6 RID: 3782
	public List<VA_Triangle> Triangles = new List<VA_Triangle>();

	// Token: 0x04000EC7 RID: 3783
	private static List<VA_Triangle> searchResults = new List<VA_Triangle>();

	// Token: 0x04000EC8 RID: 3784
	private static float searchRange;

	// Token: 0x04000EC9 RID: 3785
	private static float searchMaximum;

	// Token: 0x04000ECA RID: 3786
	private static Vector3 searchPoint;

	// Token: 0x0200039C RID: 924
	[Serializable]
	public class Node
	{
		// Token: 0x04001CE3 RID: 7395
		public Bounds Bound;

		// Token: 0x04001CE4 RID: 7396
		public int PositiveIndex;

		// Token: 0x04001CE5 RID: 7397
		public int NegativeIndex;

		// Token: 0x04001CE6 RID: 7398
		public int TriangleIndex;

		// Token: 0x04001CE7 RID: 7399
		public int TriangleCount;
	}
}
