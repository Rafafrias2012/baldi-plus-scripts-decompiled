using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000175 RID: 373
[Serializable]
public class LevelData
{
	// Token: 0x060008C8 RID: 2248 RVA: 0x0002D01C File Offset: 0x0002B21C
	public static LevelData ConvertFromAsset(LevelAsset asset)
	{
		LevelData levelData = new LevelData();
		levelData.spawnPoint = asset.spawnPoint;
		levelData.spawnDirection = asset.spawnDirection;
		levelData.levelSize = asset.levelSize;
		levelData.tile = asset.tile;
		levelData.rooms = new List<RoomData>(asset.rooms);
		foreach (RoomData roomData in levelData.rooms)
		{
			roomData.basicObjects = new List<BasicObjectData>(roomData.basicObjects);
			roomData.items = new List<ItemData>(roomData.items);
		}
		levelData.roomAssetsPlacements = new List<RoomAssetPlacementData>(asset.roomAssetPlacements);
		levelData.doors = new List<DoorData>(asset.doors);
		levelData.exits = new List<ExitData>(asset.exits);
		levelData.lights = new List<LightSourceData>(asset.lights);
		levelData.tbos = new List<TileBasedObjectData>(asset.tbos);
		levelData.builders = new List<ObjectBuilderData>(asset.builders);
		levelData.posters = new List<PosterData>(asset.posters);
		levelData.windows = new List<WindowData>(asset.windows);
		levelData.buttons = new List<ButtonData>(asset.buttons);
		levelData.events = new List<RandomEvent>(asset.events);
		return levelData;
	}

	// Token: 0x060008C9 RID: 2249 RVA: 0x0002D17C File Offset: 0x0002B37C
	public void ConvertToAsset(LevelAsset asset)
	{
		asset.spawnPoint = this.spawnPoint;
		asset.spawnDirection = this.spawnDirection;
		asset.levelSize = this.levelSize;
		asset.tile = this.tile;
		asset.rooms = new List<RoomData>(this.rooms);
		asset.doors = new List<DoorData>(this.doors);
		asset.exits = new List<ExitData>(this.exits);
		asset.lights = new List<LightSourceData>(this.lights);
		asset.tbos = new List<TileBasedObjectData>(this.tbos);
		asset.builders = new List<ObjectBuilderData>(this.builders);
		asset.posters = new List<PosterData>(this.posters);
		asset.windows = new List<WindowData>(this.windows);
		asset.buttons = new List<ButtonData>(this.buttons);
		asset.events = new List<RandomEvent>(this.events);
	}

	// Token: 0x060008CA RID: 2250 RVA: 0x0002D264 File Offset: 0x0002B464
	public static LevelData ConvertFromContainer(LevelDataContainer container)
	{
		LevelData levelData = new LevelData();
		levelData.spawnPoint = container.spawnPoint;
		levelData.spawnDirection = container.spawnDirection;
		levelData.levelSize = container.levelSize;
		levelData.tile = container.tile;
		levelData.extraData = container.extraData;
		levelData.rooms = new List<RoomData>(container.rooms);
		foreach (RoomData roomData in levelData.rooms)
		{
			roomData.basicObjects = new List<BasicObjectData>(roomData.basicObjects);
			roomData.items = new List<ItemData>(roomData.items);
		}
		levelData.doors = new List<DoorData>(container.doors);
		levelData.exits = new List<ExitData>(container.exits);
		levelData.lights = new List<LightSourceData>(container.lights);
		levelData.tbos = new List<TileBasedObjectData>(container.tbos);
		levelData.builders = new List<ObjectBuilderData>(container.builders);
		levelData.posters = new List<PosterData>(container.posters);
		levelData.windows = new List<WindowData>(container.windows);
		levelData.buttons = new List<ButtonData>(container.buttons);
		levelData.events = new List<RandomEvent>(container.events);
		return levelData;
	}

	// Token: 0x060008CB RID: 2251 RVA: 0x0002D3C0 File Offset: 0x0002B5C0
	public void ConvertToContainer(LevelDataContainer container, LevelDataContainer parent)
	{
		container.spawnPoint = this.spawnPoint;
		container.spawnDirection = this.spawnDirection;
		container.levelSize = this.levelSize;
		if (parent == null)
		{
			container.tile = this.tile;
		}
		else
		{
			List<CellData> list = new List<CellData>();
			list.AddRange(parent.tile);
			foreach (CellData cellData in this.tile)
			{
				bool flag = true;
				foreach (CellData cellData2 in list)
				{
					if (cellData2.pos == cellData.pos)
					{
						cellData2.type = cellData.type;
						cellData2.roomId = cellData.roomId;
						flag = false;
						break;
					}
				}
				if (flag)
				{
					list.Add(cellData);
				}
			}
			container.tile = list.ToArray();
		}
		container.extraData = this.extraData;
		container.rooms = new List<RoomData>(this.rooms);
		container.doors = new List<DoorData>(this.doors);
		container.exits = new List<ExitData>(this.exits);
		container.lights = new List<LightSourceData>(this.lights);
		container.tbos = new List<TileBasedObjectData>(this.tbos);
		container.builders = new List<ObjectBuilderData>(this.builders);
		container.posters = new List<PosterData>(this.posters);
		container.windows = new List<WindowData>(this.windows);
		container.buttons = new List<ButtonData>(this.buttons);
		container.events = new List<RandomEvent>(this.events);
	}

	// Token: 0x04000964 RID: 2404
	public Vector3 spawnPoint;

	// Token: 0x04000965 RID: 2405
	public Direction spawnDirection;

	// Token: 0x04000966 RID: 2406
	public IntVector2 levelSize;

	// Token: 0x04000967 RID: 2407
	public CellData[] tile;

	// Token: 0x04000968 RID: 2408
	public ExtraLevelData extraData = new ExtraLevelData();

	// Token: 0x04000969 RID: 2409
	public List<RoomData> rooms = new List<RoomData>();

	// Token: 0x0400096A RID: 2410
	public List<RoomAssetPlacementData> roomAssetsPlacements = new List<RoomAssetPlacementData>();

	// Token: 0x0400096B RID: 2411
	public List<DoorData> doors = new List<DoorData>();

	// Token: 0x0400096C RID: 2412
	public List<ExitData> exits = new List<ExitData>();

	// Token: 0x0400096D RID: 2413
	public List<LightSourceData> lights = new List<LightSourceData>();

	// Token: 0x0400096E RID: 2414
	public List<TileBasedObjectData> tbos = new List<TileBasedObjectData>();

	// Token: 0x0400096F RID: 2415
	public List<ObjectBuilderData> builders = new List<ObjectBuilderData>();

	// Token: 0x04000970 RID: 2416
	public List<PosterData> posters = new List<PosterData>();

	// Token: 0x04000971 RID: 2417
	public List<WindowData> windows = new List<WindowData>();

	// Token: 0x04000972 RID: 2418
	public List<ButtonData> buttons = new List<ButtonData>();

	// Token: 0x04000973 RID: 2419
	public List<RandomEvent> events = new List<RandomEvent>();
}
