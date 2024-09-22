using System;
using UnityEngine;

// Token: 0x02000179 RID: 377
[Serializable]
public class BasicObjectData
{
	// Token: 0x060008D1 RID: 2257 RVA: 0x0002D66F File Offset: 0x0002B86F
	public BasicObjectData GetNew()
	{
		return new BasicObjectData
		{
			prefab = this.prefab,
			position = this.position,
			rotation = this.rotation
		};
	}

	// Token: 0x0400097E RID: 2430
	public Transform prefab;

	// Token: 0x0400097F RID: 2431
	public Vector3 position;

	// Token: 0x04000980 RID: 2432
	public Quaternion rotation;

	// Token: 0x04000981 RID: 2433
	public bool replaceable = true;
}
