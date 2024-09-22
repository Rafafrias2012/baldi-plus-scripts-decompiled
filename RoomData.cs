using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200019D RID: 413
[Serializable]
public class RoomData
{
	// Token: 0x06000952 RID: 2386 RVA: 0x00031A80 File Offset: 0x0002FC80
	public static RoomData ConvertFromAsset(RoomAsset asset, IntVector2 position, IntVector2 pivot, Direction direction)
	{
		RoomData roomData = new RoomData();
		roomData.name = asset.name;
		roomData.category = asset.category;
		roomData.type = asset.type;
		roomData.color = asset.color;
		roomData.mapMaterial = asset.mapMaterial;
		roomData.florTex = asset.florTex;
		roomData.wallTex = asset.wallTex;
		roomData.ceilTex = asset.ceilTex;
		roomData.doorMats = asset.doorMats;
		roomData.windowObject = asset.windowObject;
		roomData.hasActivity = asset.hasActivity;
		roomData.offLimits = asset.offLimits;
		roomData.keepTextures = asset.keepTextures;
		roomData.activity = asset.activity;
		roomData.roomFunction = asset.roomFunction;
		roomData.roomFunctionContainer = asset.roomFunctionContainer;
		roomData.minItemValue = asset.minItemValue;
		roomData.maxItemValue = asset.maxItemValue;
		roomData.basicObjects = new List<BasicObjectData>(asset.basicObjects);
		roomData.basicSwaps = new List<BasicObjectSwapData>(asset.basicSwaps);
		roomData.posters = new List<WeightedPosterObject>(asset.posters);
		roomData.posterChance = asset.posterChance;
		roomData.items = new List<ItemData>(asset.items);
		roomData.itemList = new List<WeightedItemObject>(asset.itemList);
		roomData.itemSpawnPoints = new List<ItemSpawnPoint>(asset.itemSpawnPoints);
		roomData.potentialDoorPositions = new List<IntVector2>(asset.potentialDoorPositions);
		roomData.potentialDoorPositions.Adjust(position, pivot, direction);
		roomData.forcedDoorPositions = new List<IntVector2>(asset.forcedDoorPositions);
		roomData.forcedDoorPositions.Adjust(position, pivot, direction);
		roomData.requiredDoorPositions = new List<IntVector2>(asset.requiredDoorPositions);
		roomData.requiredDoorPositions.Adjust(position, pivot, direction);
		roomData.entitySafeCells = new List<IntVector2>(asset.entitySafeCells);
		roomData.entitySafeCells.Adjust(position, pivot, direction);
		roomData.eventSafeCells = new List<IntVector2>(asset.eventSafeCells);
		roomData.eventSafeCells.Adjust(position, pivot, direction);
		roomData.blockedWallCells = new List<IntVector2>(asset.blockedWallCells);
		roomData.blockedWallCells.Adjust(position, pivot, direction);
		roomData.secretCells = new List<IntVector2>(asset.secretCells);
		roomData.secretCells.Adjust(position, pivot, direction);
		roomData.standardLightCells = new List<IntVector2>(asset.standardLightCells);
		roomData.standardLightCells.Adjust(position, pivot, direction);
		roomData.lightPre = asset.lightPre;
		roomData.windowChance = asset.windowChance;
		return roomData;
	}

	// Token: 0x06000953 RID: 2387 RVA: 0x00031CEC File Offset: 0x0002FEEC
	public void ConvertToAsset(RoomAsset asset, IntVector2 position)
	{
		asset.name = this.name;
		asset.category = this.category;
		asset.type = this.type;
		asset.color = this.color;
		asset.mapMaterial = this.mapMaterial;
		asset.florTex = this.florTex;
		asset.wallTex = this.wallTex;
		asset.ceilTex = this.ceilTex;
		asset.keepTextures = this.keepTextures;
		asset.doorMats = this.doorMats;
		asset.windowObject = this.windowObject;
		asset.hasActivity = this.hasActivity;
		asset.offLimits = this.offLimits;
		asset.activity = this.activity.GetNew();
		asset.activity.position -= new Vector3((float)(position.x * 10), 0f, (float)(position.z * 10));
		asset.roomFunction = this.roomFunction;
		asset.roomFunctionContainer = this.roomFunctionContainer;
		asset.minItemValue = this.minItemValue;
		asset.maxItemValue = this.maxItemValue;
		asset.lightPre = this.lightPre;
		asset.windowChance = this.windowChance;
		asset.basicObjects = new List<BasicObjectData>(this.basicObjects);
		for (int i = 0; i < asset.basicObjects.Count; i++)
		{
			asset.basicObjects[i] = this.basicObjects[i].GetNew();
			asset.basicObjects[i].position -= new Vector3((float)(position.x * 10), 0f, (float)(position.z * 10));
		}
		asset.basicSwaps = new List<BasicObjectSwapData>(this.basicSwaps);
		asset.posterChance = this.posterChance;
		for (int j = 0; j < asset.basicSwaps.Count; j++)
		{
			asset.basicSwaps[j] = this.basicSwaps[j].GetNew();
		}
		asset.posters = new List<WeightedPosterObject>(this.posters);
		for (int k = 0; k < asset.posters.Count; k++)
		{
			asset.posters[k] = new WeightedPosterObject();
			asset.posters[k].selection = this.posters[k].selection;
			asset.posters[k].weight = this.posters[k].weight;
		}
		asset.items = new List<ItemData>(this.items);
		asset.itemList = new List<WeightedItemObject>(this.itemList);
		for (int l = 0; l < asset.items.Count; l++)
		{
			asset.items[l] = asset.items[l].GetNew();
			asset.items[l].position -= new Vector2((float)(position.x * 10), (float)(position.z * 10));
		}
		asset.itemSpawnPoints = new List<ItemSpawnPoint>(this.itemSpawnPoints);
		for (int m = 0; m < asset.itemSpawnPoints.Count; m++)
		{
			asset.itemSpawnPoints[m] = asset.itemSpawnPoints[m].GetNew();
			asset.itemSpawnPoints[m].position -= new Vector2((float)(position.x * 10), (float)(position.z * 10));
		}
		asset.potentialDoorPositions = new List<IntVector2>(this.potentialDoorPositions);
		asset.potentialDoorPositions.Adjust(position * -1, new IntVector2(0, 0), Direction.North);
		asset.forcedDoorPositions = new List<IntVector2>(this.forcedDoorPositions);
		asset.forcedDoorPositions.Adjust(position * -1, new IntVector2(0, 0), Direction.North);
		asset.requiredDoorPositions = new List<IntVector2>(this.requiredDoorPositions);
		asset.requiredDoorPositions.Adjust(position * -1, new IntVector2(0, 0), Direction.North);
		asset.entitySafeCells = new List<IntVector2>(this.entitySafeCells);
		asset.entitySafeCells.Adjust(position * -1, new IntVector2(0, 0), Direction.North);
		asset.eventSafeCells = new List<IntVector2>(this.eventSafeCells);
		asset.eventSafeCells.Adjust(position * -1, new IntVector2(0, 0), Direction.North);
		asset.blockedWallCells = new List<IntVector2>(this.blockedWallCells);
		asset.blockedWallCells.Adjust(position * -1, new IntVector2(0, 0), Direction.North);
		asset.secretCells = new List<IntVector2>(this.secretCells);
		asset.secretCells.Adjust(position * -1, new IntVector2(0, 0), Direction.North);
		asset.standardLightCells = new List<IntVector2>(this.standardLightCells);
		asset.standardLightCells.Adjust(position * -1, new IntVector2(0, 0), Direction.North);
	}

	// Token: 0x04000A63 RID: 2659
	public string name = "";

	// Token: 0x04000A64 RID: 2660
	public RoomCategory category;

	// Token: 0x04000A65 RID: 2661
	public RoomType type;

	// Token: 0x04000A66 RID: 2662
	public Color color = Color.white;

	// Token: 0x04000A67 RID: 2663
	public Material mapMaterial;

	// Token: 0x04000A68 RID: 2664
	public Material florMat;

	// Token: 0x04000A69 RID: 2665
	public Material wallMat;

	// Token: 0x04000A6A RID: 2666
	public Material ceilMat;

	// Token: 0x04000A6B RID: 2667
	public Texture2D florTex;

	// Token: 0x04000A6C RID: 2668
	public Texture2D wallTex;

	// Token: 0x04000A6D RID: 2669
	public Texture2D ceilTex;

	// Token: 0x04000A6E RID: 2670
	public StandardDoorMats doorMats;

	// Token: 0x04000A6F RID: 2671
	public WindowObject windowObject;

	// Token: 0x04000A70 RID: 2672
	public bool hasActivity;

	// Token: 0x04000A71 RID: 2673
	public bool offLimits;

	// Token: 0x04000A72 RID: 2674
	public bool keepTextures;

	// Token: 0x04000A73 RID: 2675
	public ActivityData activity;

	// Token: 0x04000A74 RID: 2676
	public RoomFunction roomFunction;

	// Token: 0x04000A75 RID: 2677
	public RoomFunctionContainer roomFunctionContainer;

	// Token: 0x04000A76 RID: 2678
	public List<BasicObjectData> basicObjects = new List<BasicObjectData>();

	// Token: 0x04000A77 RID: 2679
	public List<BasicObjectSwapData> basicSwaps = new List<BasicObjectSwapData>();

	// Token: 0x04000A78 RID: 2680
	public List<WeightedPosterObject> posters = new List<WeightedPosterObject>();

	// Token: 0x04000A79 RID: 2681
	public float posterChance = 0.25f;

	// Token: 0x04000A7A RID: 2682
	public List<ItemData> items = new List<ItemData>();

	// Token: 0x04000A7B RID: 2683
	public List<WeightedItemObject> itemList = new List<WeightedItemObject>();

	// Token: 0x04000A7C RID: 2684
	public int minItemValue;

	// Token: 0x04000A7D RID: 2685
	public int maxItemValue = 100;

	// Token: 0x04000A7E RID: 2686
	public List<ItemSpawnPoint> itemSpawnPoints = new List<ItemSpawnPoint>();

	// Token: 0x04000A7F RID: 2687
	public List<IntVector2> potentialDoorPositions = new List<IntVector2>();

	// Token: 0x04000A80 RID: 2688
	public List<IntVector2> forcedDoorPositions = new List<IntVector2>();

	// Token: 0x04000A81 RID: 2689
	public List<IntVector2> requiredDoorPositions = new List<IntVector2>();

	// Token: 0x04000A82 RID: 2690
	public List<IntVector2> entitySafeCells = new List<IntVector2>();

	// Token: 0x04000A83 RID: 2691
	public List<IntVector2> eventSafeCells = new List<IntVector2>();

	// Token: 0x04000A84 RID: 2692
	public List<IntVector2> blockedWallCells = new List<IntVector2>();

	// Token: 0x04000A85 RID: 2693
	public List<IntVector2> secretCells = new List<IntVector2>();

	// Token: 0x04000A86 RID: 2694
	public List<IntVector2> standardLightCells = new List<IntVector2>();

	// Token: 0x04000A87 RID: 2695
	public Transform lightPre;

	// Token: 0x04000A88 RID: 2696
	public float windowChance;
}
