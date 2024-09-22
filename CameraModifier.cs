using System;
using UnityEngine;

// Token: 0x020000FD RID: 253
[Serializable]
public class CameraModifier
{
	// Token: 0x060005F1 RID: 1521 RVA: 0x0001DDA3 File Offset: 0x0001BFA3
	public CameraModifier(Vector3 posOffset, Vector3 rotOffset)
	{
		this.posOffset = posOffset;
		this.rotOffset = rotOffset;
	}

	// Token: 0x04000621 RID: 1569
	public Vector3 posOffset;

	// Token: 0x04000622 RID: 1570
	public Vector3 rotOffset;
}
