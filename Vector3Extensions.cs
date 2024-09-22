using System;
using UnityEngine;

// Token: 0x02000142 RID: 322
public static class Vector3Extensions
{
	// Token: 0x06000772 RID: 1906 RVA: 0x000260A6 File Offset: 0x000242A6
	public static Vector3 ZeroOutY(this Vector3 vector)
	{
		return new Vector3(vector.x, 0f, vector.z);
	}
}
