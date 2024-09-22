using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000A5 RID: 165
public class CoinDoorBuilder : ObjectBuilder
{
	// Token: 0x060003C9 RID: 969 RVA: 0x000139A8 File Offset: 0x00011BA8
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
			Door door = Object.Instantiate<SwingDoor>(this.doorPre, room.transform);
			Direction dir = cell.AllOpenNavDirections[cRng.Next(0, cell.AllOpenNavDirections.Count)];
			ec.SetupDoor(door, cell, dir);
			return;
		}
		Debug.LogWarning("One-way door builder was unable to find a valid position for the wall!");
	}

	// Token: 0x040003FB RID: 1019
	[SerializeField]
	private SwingDoor doorPre;

	// Token: 0x040003FC RID: 1020
	[SerializeField]
	private List<TileShape> tileShapes = new List<TileShape>
	{
		TileShape.Straight
	};

	// Token: 0x040003FD RID: 1021
	[SerializeField]
	private int maxAttempts = 10;
}
