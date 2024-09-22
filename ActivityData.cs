using System;
using UnityEngine;

// Token: 0x02000177 RID: 375
[Serializable]
public class ActivityData
{
	// Token: 0x060008CE RID: 2254 RVA: 0x0002D617 File Offset: 0x0002B817
	public ActivityData GetNew()
	{
		return new ActivityData
		{
			prefab = this.prefab,
			position = this.position,
			direction = this.direction
		};
	}

	// Token: 0x04000977 RID: 2423
	public Activity prefab;

	// Token: 0x04000978 RID: 2424
	public Vector3 position;

	// Token: 0x04000979 RID: 2425
	public Direction direction;
}
