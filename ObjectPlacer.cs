using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000198 RID: 408
[Serializable]
public class ObjectPlacer
{
	// Token: 0x06000944 RID: 2372 RVA: 0x00031430 File Offset: 0x0002F630
	public void Build(LevelBuilder builder, RoomController room, Random cRng)
	{
		List<Cell> tilesOfShape = room.GetTilesOfShape(this.eligibleShapes, this.coverage, this.includeOpen);
		int num = cRng.Next(this.min, this.max + 1);
		List<Direction> list = new List<Direction>();
		int num2 = 0;
		while (num2 < num && tilesOfShape.Count > 0)
		{
			int index = cRng.Next(0, tilesOfShape.Count);
			list.Clear();
			if (this.useOpenDir)
			{
				list.AddRange(Directions.OpenDirectionsFromBin(tilesOfShape[index].ConstBin));
			}
			if (this.useWallDir)
			{
				list.AddRange(Directions.ClosedDirectionsFromBin(tilesOfShape[index].ConstBin));
			}
			for (int i = 0; i < list.Count; i++)
			{
				if (!tilesOfShape[index].HardCoverageFits(this.coverage.Rotated(list[i])))
				{
					list.RemoveAt(i);
					i--;
				}
			}
			if (list.Count > 0)
			{
				Direction direction = list[cRng.Next(0, list.Count)];
				this.objectsPlaced.Add(builder.InstatiateEnvironmentObject(this.prefab, tilesOfShape[index], direction));
				this.objectDirections.Add(direction);
				tilesOfShape[index].HardCover(this.coverage.Rotated(direction));
			}
			else
			{
				Debug.LogWarning("ObjectPlacer for " + this.prefab.transform.name + " found no eligible directions, so it didn't build.");
			}
			tilesOfShape.RemoveAt(index);
			num2++;
		}
	}

	// Token: 0x170000C9 RID: 201
	// (get) Token: 0x06000945 RID: 2373 RVA: 0x000315BD File Offset: 0x0002F7BD
	public List<GameObject> ObjectsPlaced
	{
		get
		{
			return this.objectsPlaced;
		}
	}

	// Token: 0x170000CA RID: 202
	// (get) Token: 0x06000946 RID: 2374 RVA: 0x000315C5 File Offset: 0x0002F7C5
	public List<Direction> ObjectDirections
	{
		get
		{
			return this.objectDirections;
		}
	}

	// Token: 0x04000A27 RID: 2599
	[SerializeField]
	private GameObject prefab;

	// Token: 0x04000A28 RID: 2600
	public List<TileShape> eligibleShapes = new List<TileShape>();

	// Token: 0x04000A29 RID: 2601
	public CellCoverage coverage;

	// Token: 0x04000A2A RID: 2602
	[SerializeField]
	private int min = 1;

	// Token: 0x04000A2B RID: 2603
	[SerializeField]
	private int max = 5;

	// Token: 0x04000A2C RID: 2604
	[SerializeField]
	private bool useWallDir;

	// Token: 0x04000A2D RID: 2605
	[SerializeField]
	private bool useOpenDir;

	// Token: 0x04000A2E RID: 2606
	[SerializeField]
	private bool includeOpen = true;

	// Token: 0x04000A2F RID: 2607
	[SerializeField]
	private bool group;

	// Token: 0x04000A30 RID: 2608
	private List<GameObject> objectsPlaced = new List<GameObject>();

	// Token: 0x04000A31 RID: 2609
	private List<Direction> objectDirections = new List<Direction>();
}
