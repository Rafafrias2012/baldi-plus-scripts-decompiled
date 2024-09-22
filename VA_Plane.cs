using System;
using UnityEngine;

// Token: 0x02000224 RID: 548
[Serializable]
public class VA_Plane
{
	// Token: 0x06000C64 RID: 3172 RVA: 0x0004172D File Offset: 0x0003F92D
	public VA_Plane(Vector3 a, Vector3 b, Vector3 c)
	{
		this.Normal = Vector3.Normalize(Vector3.Cross(b - a, c - a));
		this.Distance = -Vector3.Dot(this.Normal, a);
	}

	// Token: 0x06000C65 RID: 3173 RVA: 0x00041768 File Offset: 0x0003F968
	public bool SideOf(Vector3 p)
	{
		return this.Normal.x * p.x + this.Normal.y * p.y + this.Normal.z * p.z + this.Distance > 0f;
	}

	// Token: 0x06000C66 RID: 3174 RVA: 0x000417BB File Offset: 0x0003F9BB
	public float DistanceTo(Vector3 p)
	{
		return Vector3.Dot(this.Normal, p) + this.Distance;
	}

	// Token: 0x06000C67 RID: 3175 RVA: 0x000417D0 File Offset: 0x0003F9D0
	public Vector3 ClosestTo(Vector3 p)
	{
		return p - this.Normal * (Vector3.Dot(this.Normal, p) + this.Distance);
	}

	// Token: 0x04000ECC RID: 3788
	public Vector3 Normal;

	// Token: 0x04000ECD RID: 3789
	public float Distance;
}
