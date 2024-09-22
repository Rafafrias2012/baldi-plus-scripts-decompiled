using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000174 RID: 372
[Serializable]
public class LevelAsset : ScriptableObject
{
	// Token: 0x04000955 RID: 2389
	public Vector3 spawnPoint;

	// Token: 0x04000956 RID: 2390
	public Direction spawnDirection;

	// Token: 0x04000957 RID: 2391
	public IntVector2 levelSize;

	// Token: 0x04000958 RID: 2392
	public CellData[] tile;

	// Token: 0x04000959 RID: 2393
	public List<RoomData> rooms = new List<RoomData>();

	// Token: 0x0400095A RID: 2394
	public List<RoomAssetPlacementData> roomAssetPlacements = new List<RoomAssetPlacementData>();

	// Token: 0x0400095B RID: 2395
	public List<DoorData> doors = new List<DoorData>();

	// Token: 0x0400095C RID: 2396
	public List<ExitData> exits = new List<ExitData>();

	// Token: 0x0400095D RID: 2397
	public List<LightSourceData> lights = new List<LightSourceData>();

	// Token: 0x0400095E RID: 2398
	public List<TileBasedObjectData> tbos = new List<TileBasedObjectData>();

	// Token: 0x0400095F RID: 2399
	public List<ObjectBuilderData> builders = new List<ObjectBuilderData>();

	// Token: 0x04000960 RID: 2400
	public List<PosterData> posters = new List<PosterData>();

	// Token: 0x04000961 RID: 2401
	public List<WindowData> windows = new List<WindowData>();

	// Token: 0x04000962 RID: 2402
	public List<ButtonData> buttons = new List<ButtonData>();

	// Token: 0x04000963 RID: 2403
	public List<RandomEvent> events = new List<RandomEvent>();
}
