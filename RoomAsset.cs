using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200019C RID: 412
public class RoomAsset : ScriptableObject
{
	// Token: 0x04000A39 RID: 2617
	public List<CellData> cells = new List<CellData>();

	// Token: 0x04000A3A RID: 2618
	public new string name = "";

	// Token: 0x04000A3B RID: 2619
	public RoomCategory category;

	// Token: 0x04000A3C RID: 2620
	public RoomType type;

	// Token: 0x04000A3D RID: 2621
	public int spawnWeight = 100;

	// Token: 0x04000A3E RID: 2622
	public Color color = Color.white;

	// Token: 0x04000A3F RID: 2623
	public Material mapMaterial;

	// Token: 0x04000A40 RID: 2624
	public Material florMat;

	// Token: 0x04000A41 RID: 2625
	public Material wallMat;

	// Token: 0x04000A42 RID: 2626
	public Material ceilMat;

	// Token: 0x04000A43 RID: 2627
	public Texture2D florTex;

	// Token: 0x04000A44 RID: 2628
	public Texture2D wallTex;

	// Token: 0x04000A45 RID: 2629
	public Texture2D ceilTex;

	// Token: 0x04000A46 RID: 2630
	public StandardDoorMats doorMats;

	// Token: 0x04000A47 RID: 2631
	public WindowObject windowObject;

	// Token: 0x04000A48 RID: 2632
	public bool hasActivity;

	// Token: 0x04000A49 RID: 2633
	public bool offLimits;

	// Token: 0x04000A4A RID: 2634
	public bool keepTextures;

	// Token: 0x04000A4B RID: 2635
	public ActivityData activity;

	// Token: 0x04000A4C RID: 2636
	public RoomFunction roomFunction;

	// Token: 0x04000A4D RID: 2637
	public RoomFunctionContainer roomFunctionContainer;

	// Token: 0x04000A4E RID: 2638
	public List<BasicObjectData> basicObjects = new List<BasicObjectData>();

	// Token: 0x04000A4F RID: 2639
	public List<BasicObjectSwapData> basicSwaps = new List<BasicObjectSwapData>();

	// Token: 0x04000A50 RID: 2640
	public List<WeightedPosterObject> posters = new List<WeightedPosterObject>();

	// Token: 0x04000A51 RID: 2641
	public float posterChance = 0.25f;

	// Token: 0x04000A52 RID: 2642
	public List<ItemData> items = new List<ItemData>();

	// Token: 0x04000A53 RID: 2643
	public List<WeightedItemObject> itemList = new List<WeightedItemObject>();

	// Token: 0x04000A54 RID: 2644
	public int minItemValue;

	// Token: 0x04000A55 RID: 2645
	public int maxItemValue = 100;

	// Token: 0x04000A56 RID: 2646
	public List<ItemSpawnPoint> itemSpawnPoints = new List<ItemSpawnPoint>();

	// Token: 0x04000A57 RID: 2647
	public List<IntVector2> potentialDoorPositions = new List<IntVector2>();

	// Token: 0x04000A58 RID: 2648
	public List<IntVector2> forcedDoorPositions = new List<IntVector2>();

	// Token: 0x04000A59 RID: 2649
	public List<IntVector2> requiredDoorPositions = new List<IntVector2>();

	// Token: 0x04000A5A RID: 2650
	public List<IntVector2> entitySafeCells = new List<IntVector2>();

	// Token: 0x04000A5B RID: 2651
	public List<IntVector2> eventSafeCells = new List<IntVector2>();

	// Token: 0x04000A5C RID: 2652
	public List<IntVector2> blockedWallCells = new List<IntVector2>();

	// Token: 0x04000A5D RID: 2653
	public List<IntVector2> secretCells = new List<IntVector2>();

	// Token: 0x04000A5E RID: 2654
	public List<IntVector2> standardLightCells = new List<IntVector2>();

	// Token: 0x04000A5F RID: 2655
	public List<LightSourceData> lights = new List<LightSourceData>();

	// Token: 0x04000A60 RID: 2656
	public Transform lightPre;

	// Token: 0x04000A61 RID: 2657
	public float windowChance;

	// Token: 0x04000A62 RID: 2658
	public List<PosterData> posterDatas = new List<PosterData>();
}
