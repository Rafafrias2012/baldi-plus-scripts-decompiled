using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200018B RID: 395
public class LevelDataContainer : MonoBehaviour
{
	// Token: 0x040009F7 RID: 2551
	public Vector3 spawnPoint;

	// Token: 0x040009F8 RID: 2552
	public Direction spawnDirection;

	// Token: 0x040009F9 RID: 2553
	public IntVector2 levelSize;

	// Token: 0x040009FA RID: 2554
	public CellData[] tile;

	// Token: 0x040009FB RID: 2555
	public ExtraLevelData extraData = new ExtraLevelData();

	// Token: 0x040009FC RID: 2556
	public List<RoomData> rooms = new List<RoomData>();

	// Token: 0x040009FD RID: 2557
	public List<DoorData> doors = new List<DoorData>();

	// Token: 0x040009FE RID: 2558
	public List<ExitData> exits = new List<ExitData>();

	// Token: 0x040009FF RID: 2559
	public List<LightSourceData> lights = new List<LightSourceData>();

	// Token: 0x04000A00 RID: 2560
	public List<TileBasedObjectData> tbos = new List<TileBasedObjectData>();

	// Token: 0x04000A01 RID: 2561
	public List<ObjectBuilderData> builders = new List<ObjectBuilderData>();

	// Token: 0x04000A02 RID: 2562
	public List<PosterData> posters = new List<PosterData>();

	// Token: 0x04000A03 RID: 2563
	public List<WindowData> windows = new List<WindowData>();

	// Token: 0x04000A04 RID: 2564
	public List<ButtonData> buttons = new List<ButtonData>();

	// Token: 0x04000A05 RID: 2565
	public List<RandomEvent> events = new List<RandomEvent>();
}
