using System;
using UnityEngine;

// Token: 0x0200019F RID: 415
[Serializable]
public class BasicObjectSwapData
{
	// Token: 0x06000957 RID: 2391 RVA: 0x00032320 File Offset: 0x00030520
	public BasicObjectSwapData GetNew()
	{
		BasicObjectSwapData basicObjectSwapData = new BasicObjectSwapData();
		basicObjectSwapData.prefabToSwap = this.prefabToSwap;
		basicObjectSwapData.potentialReplacements = new WeightedTransform[this.potentialReplacements.Length];
		for (int i = 0; i < this.potentialReplacements.Length; i++)
		{
			basicObjectSwapData.potentialReplacements[i] = this.potentialReplacements[i];
		}
		basicObjectSwapData.chance = this.chance;
		return basicObjectSwapData;
	}

	// Token: 0x04000A8E RID: 2702
	public Transform prefabToSwap;

	// Token: 0x04000A8F RID: 2703
	public WeightedTransform[] potentialReplacements = new WeightedTransform[0];

	// Token: 0x04000A90 RID: 2704
	public float chance;
}
