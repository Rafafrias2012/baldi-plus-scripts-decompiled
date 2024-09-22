using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000221 RID: 545
public class VA_MeshLinear
{
	// Token: 0x17000100 RID: 256
	// (get) Token: 0x06000C4F RID: 3151 RVA: 0x00040C6A File Offset: 0x0003EE6A
	public bool HasTriangles
	{
		get
		{
			return this.triangles != null && this.triangles.Count > 0;
		}
	}

	// Token: 0x06000C50 RID: 3152 RVA: 0x00040C84 File Offset: 0x0003EE84
	public void Clear()
	{
		if (this.triangles != null)
		{
			this.triangles.Clear();
		}
	}

	// Token: 0x06000C51 RID: 3153 RVA: 0x00040C9C File Offset: 0x0003EE9C
	public void Update(Mesh mesh)
	{
		int num = 0;
		if (this.triangles == null)
		{
			this.triangles = new List<VA_Triangle>();
		}
		if (mesh != null)
		{
			Vector3[] vertices = mesh.vertices;
			for (int i = 0; i < mesh.subMeshCount; i++)
			{
				if (mesh.GetTopology(i) == MeshTopology.Triangles)
				{
					int[] array = mesh.GetTriangles(i);
					for (int j = 0; j < array.Length; j += 3)
					{
						VA_Triangle triangle = this.GetTriangle(num++);
						triangle.A = vertices[array[j]];
						triangle.B = vertices[array[j + 1]];
						triangle.C = vertices[array[j + 2]];
						triangle.CalculatePlanes();
					}
				}
			}
		}
		this.triangles.RemoveRange(num, this.triangles.Count - num);
	}

	// Token: 0x06000C52 RID: 3154 RVA: 0x00040D60 File Offset: 0x0003EF60
	public Vector3 FindClosestPoint(Vector3 point)
	{
		Vector3 result = point;
		if (this.triangles != null)
		{
			float num = float.PositiveInfinity;
			for (int i = this.triangles.Count - 1; i >= 0; i--)
			{
				Vector3 vector = this.triangles[i].ClosestTo(point);
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
		}
		return result;
	}

	// Token: 0x06000C53 RID: 3155 RVA: 0x00040DF4 File Offset: 0x0003EFF4
	private VA_Triangle GetTriangle(int triangleCount)
	{
		if (triangleCount == this.triangles.Count)
		{
			VA_Triangle va_Triangle = new VA_Triangle();
			this.triangles.Add(va_Triangle);
			return va_Triangle;
		}
		return this.triangles[triangleCount];
	}

	// Token: 0x04000EC4 RID: 3780
	private List<VA_Triangle> triangles;
}
