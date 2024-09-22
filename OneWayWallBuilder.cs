using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000AD RID: 173
public class OneWayWallBuilder : ObjectBuilder
{
	// Token: 0x060003E9 RID: 1001 RVA: 0x000146B0 File Offset: 0x000128B0
	public override void Build(EnvironmentController ec, LevelBuilder builder, RoomController room, Random cRng)
	{
		int num = 0;
		List<Cell> tilesOfShape = room.GetTilesOfShape(this.tileShapes, true);
		Cell cell = null;
		while (cell == null && tilesOfShape.Count > 0 && num < this.maxAttempts)
		{
			int index = cRng.Next(0, tilesOfShape.Count);
			num++;
			if (ec.TrapCheck(tilesOfShape[index]) || tilesOfShape[index].open)
			{
				tilesOfShape.Remove(tilesOfShape[index]);
			}
			else
			{
				cell = tilesOfShape[index];
			}
		}
		if (cell != null)
		{
			OneWayWall oneWayWall = Object.Instantiate<OneWayWall>(this.wallPre, room.transform);
			Direction direction = cell.AllOpenNavDirections[cRng.Next(0, cell.AllOpenNavDirections.Count)];
			cell.Block(direction, true);
			cell.HardCoverWall(direction, true);
			oneWayWall.transform.position = cell.FloorWorldPosition;
			oneWayWall.transform.rotation = direction.ToRotation();
			return;
		}
		Debug.LogWarning("One-way wall builder was unable to find a valid position for the wall!");
	}

	// Token: 0x0400041C RID: 1052
	[SerializeField]
	private OneWayWall wallPre;

	// Token: 0x0400041D RID: 1053
	[SerializeField]
	private List<TileShape> tileShapes = new List<TileShape>
	{
		TileShape.Straight,
		TileShape.Single
	};

	// Token: 0x0400041E RID: 1054
	[SerializeField]
	private int maxAttempts = 10;
}
