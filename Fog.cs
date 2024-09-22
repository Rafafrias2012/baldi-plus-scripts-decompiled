using System;
using UnityEngine;

// Token: 0x020000FB RID: 251
[Serializable]
public class Fog
{
	// Token: 0x040005FD RID: 1533
	public Color color = Color.black;

	// Token: 0x040005FE RID: 1534
	public float startDist;

	// Token: 0x040005FF RID: 1535
	public float maxDist;

	// Token: 0x04000600 RID: 1536
	public float strength;

	// Token: 0x04000601 RID: 1537
	public int priority;
}
