using System;
using UnityEngine;

// Token: 0x0200019E RID: 414
[Serializable]
public class ItemSpawnPoint
{
	// Token: 0x06000955 RID: 2389 RVA: 0x000322A6 File Offset: 0x000304A6
	public ItemSpawnPoint()
	{
		this.chance = 0.25f;
		this.minValue = 0;
		this.maxValue = 100;
	}

	// Token: 0x06000956 RID: 2390 RVA: 0x000322D0 File Offset: 0x000304D0
	public ItemSpawnPoint GetNew()
	{
		return new ItemSpawnPoint
		{
			position = this.position,
			chance = this.chance,
			weight = this.weight,
			minValue = this.minValue,
			maxValue = this.maxValue
		};
	}

	// Token: 0x04000A89 RID: 2697
	public Vector2 position;

	// Token: 0x04000A8A RID: 2698
	public float chance;

	// Token: 0x04000A8B RID: 2699
	public int weight = 10;

	// Token: 0x04000A8C RID: 2700
	public int minValue;

	// Token: 0x04000A8D RID: 2701
	public int maxValue;
}
