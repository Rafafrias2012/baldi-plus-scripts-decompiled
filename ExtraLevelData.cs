using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000170 RID: 368
[Serializable]
public class ExtraLevelData
{
	// Token: 0x060008BE RID: 2238 RVA: 0x0002CC44 File Offset: 0x0002AE44
	public static ExtraLevelData ConvertFromAsset(ExtraLevelDataAsset asset)
	{
		ExtraLevelData extraLevelData = new ExtraLevelData();
		extraLevelData.npcsToSpawn = new List<NPC>(asset.npcsToSpawn);
		extraLevelData.potentialNpcs = new List<WeightedNPC>(asset.potentialNpcs);
		extraLevelData.potentialItems = new WeightedItemObject[asset.potentialItems.Length];
		for (int i = 0; i < extraLevelData.potentialItems.Length; i++)
		{
			extraLevelData.potentialItems[i] = asset.potentialItems[i];
		}
		extraLevelData.totalPotentialNpcsToSpawn = asset.totalPotentialNpcsToSpawn;
		extraLevelData.npcSpawnPoints = new List<IntVector2>(asset.npcSpawnPoints);
		extraLevelData.minLightColor = asset.minLightColor;
		extraLevelData.lightMode = asset.lightMode;
		extraLevelData.initialEventGap = asset.initialEventGap;
		extraLevelData.minEventGap = asset.minEventGap;
		extraLevelData.maxEventGap = asset.maxEventGap;
		return extraLevelData;
	}

	// Token: 0x04000942 RID: 2370
	public List<NPC> npcsToSpawn = new List<NPC>();

	// Token: 0x04000943 RID: 2371
	public List<IntVector2> npcSpawnPoints = new List<IntVector2>();

	// Token: 0x04000944 RID: 2372
	public List<WeightedNPC> potentialNpcs = new List<WeightedNPC>();

	// Token: 0x04000945 RID: 2373
	public WeightedItemObject[] potentialItems = new WeightedItemObject[0];

	// Token: 0x04000946 RID: 2374
	public int totalPotentialNpcsToSpawn = 1;

	// Token: 0x04000947 RID: 2375
	public Color minLightColor = Color.black;

	// Token: 0x04000948 RID: 2376
	public LightMode lightMode = LightMode.Cumulative;

	// Token: 0x04000949 RID: 2377
	public float initialEventGap = 30f;

	// Token: 0x0400094A RID: 2378
	public float minEventGap = 45f;

	// Token: 0x0400094B RID: 2379
	public float maxEventGap = 180f;
}
