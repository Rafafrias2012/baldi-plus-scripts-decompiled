using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200016F RID: 367
[CreateAssetMenu(fileName = "Extra Level Data Asset", menuName = "Custom Assets/Extra Level Data Asset", order = 7)]
public class ExtraLevelDataAsset : ScriptableObject
{
	// Token: 0x04000938 RID: 2360
	public List<NPC> npcsToSpawn = new List<NPC>();

	// Token: 0x04000939 RID: 2361
	public List<IntVector2> npcSpawnPoints;

	// Token: 0x0400093A RID: 2362
	public List<WeightedNPC> potentialNpcs = new List<WeightedNPC>();

	// Token: 0x0400093B RID: 2363
	public WeightedItemObject[] potentialItems = new WeightedItemObject[0];

	// Token: 0x0400093C RID: 2364
	public int totalPotentialNpcsToSpawn = 1;

	// Token: 0x0400093D RID: 2365
	public Color minLightColor = Color.black;

	// Token: 0x0400093E RID: 2366
	public LightMode lightMode = LightMode.Cumulative;

	// Token: 0x0400093F RID: 2367
	public float initialEventGap = 30f;

	// Token: 0x04000940 RID: 2368
	public float minEventGap = 45f;

	// Token: 0x04000941 RID: 2369
	public float maxEventGap = 180f;
}
