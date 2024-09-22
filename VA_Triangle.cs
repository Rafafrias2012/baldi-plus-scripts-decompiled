using System;
using UnityEngine;

// Token: 0x02000228 RID: 552
[Serializable]
public class VA_Triangle
{
	// Token: 0x17000104 RID: 260
	// (get) Token: 0x06000C77 RID: 3191 RVA: 0x00041A36 File Offset: 0x0003FC36
	public Vector3 Min
	{
		get
		{
			return Vector3.Min(this.A, Vector3.Min(this.B, this.C));
		}
	}

	// Token: 0x17000105 RID: 261
	// (get) Token: 0x06000C78 RID: 3192 RVA: 0x00041A54 File Offset: 0x0003FC54
	public Vector3 Max
	{
		get
		{
			return Vector3.Max(this.A, Vector3.Max(this.B, this.C));
		}
	}

	// Token: 0x17000106 RID: 262
	// (get) Token: 0x06000C79 RID: 3193 RVA: 0x00041A72 File Offset: 0x0003FC72
	public float MidX
	{
		get
		{
			return (this.A.x + this.B.x + this.C.x) / 3f;
		}
	}

	// Token: 0x17000107 RID: 263
	// (get) Token: 0x06000C7A RID: 3194 RVA: 0x00041A9D File Offset: 0x0003FC9D
	public float MidY
	{
		get
		{
			return (this.A.y + this.B.y + this.C.y) / 3f;
		}
	}

	// Token: 0x17000108 RID: 264
	// (get) Token: 0x06000C7B RID: 3195 RVA: 0x00041AC8 File Offset: 0x0003FCC8
	public float MidZ
	{
		get
		{
			return (this.A.z + this.B.z + this.C.z) / 3f;
		}
	}

	// Token: 0x06000C7C RID: 3196 RVA: 0x00041AF4 File Offset: 0x0003FCF4
	public void CalculatePlanes()
	{
		this.PlaneABC = new VA_Plane(this.A, this.B, this.C);
		this.PlaneAB = new VA_Plane(this.A, this.B, this.A + this.PlaneABC.Normal);
		this.PlaneBC = new VA_Plane(this.B, this.C, this.B + this.PlaneABC.Normal);
		this.PlaneCA = new VA_Plane(this.C, this.A, this.C + this.PlaneABC.Normal);
	}

	// Token: 0x06000C7D RID: 3197 RVA: 0x00041BA8 File Offset: 0x0003FDA8
	public Vector3 ClosestTo(Vector3 p)
	{
		if (this.PlaneAB.SideOf(p))
		{
			return this.ClosestPointToLineSegment(this.A, this.B, p);
		}
		if (this.PlaneBC.SideOf(p))
		{
			return this.ClosestPointToLineSegment(this.B, this.C, p);
		}
		if (this.PlaneCA.SideOf(p))
		{
			return this.ClosestPointToLineSegment(this.C, this.A, p);
		}
		return this.PlaneABC.ClosestTo(p);
	}

	// Token: 0x06000C7E RID: 3198 RVA: 0x00041C28 File Offset: 0x0003FE28
	private Vector3 ClosestPointToLineSegment(Vector3 a, Vector3 b, Vector3 p)
	{
		float num = b.x - a.x;
		float num2 = b.y - a.y;
		float num3 = b.z - a.z;
		float num4 = num * num + num2 * num2 + num3 * num3;
		if (num4 > 0f)
		{
			float num5 = p.x - a.x;
			float num6 = p.y - a.y;
			float num7 = p.z - a.z;
			float num8 = num5 * num + num6 * num2 + num7 * num3 / num4;
			if ((double)num8 <= 0.0)
			{
				num8 = 0f;
			}
			else if (num8 > 1f)
			{
				num8 = 1f;
			}
			a.x += num * num8;
			a.y += num2 * num8;
			a.z += num3 * num8;
			return a;
		}
		return a;
	}

	// Token: 0x04000ED6 RID: 3798
	public Vector3 A;

	// Token: 0x04000ED7 RID: 3799
	public Vector3 B;

	// Token: 0x04000ED8 RID: 3800
	public Vector3 C;

	// Token: 0x04000ED9 RID: 3801
	public VA_Plane PlaneABC;

	// Token: 0x04000EDA RID: 3802
	public VA_Plane PlaneAB;

	// Token: 0x04000EDB RID: 3803
	public VA_Plane PlaneBC;

	// Token: 0x04000EDC RID: 3804
	public VA_Plane PlaneCA;
}
