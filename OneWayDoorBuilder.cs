using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000AB RID: 171
public class OneWayDoorBuilder : ObjectBuilder
{
	// Token: 0x060003E6 RID: 998 RVA: 0x00014528 File Offset: 0x00012728
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
			SwingDoor swingDoor = Object.Instantiate<SwingDoor>(this.doorPre, room.transform);
			Direction direction = cell.AllOpenNavDirections[cRng.Next(0, cell.AllOpenNavDirections.Count)];
			cell.HardCoverWall(direction, true);
			swingDoor.transform.position = cell.FloorWorldPosition;
			swingDoor.transform.rotation = direction.ToRotation();
			swingDoor.position = cell.position;
			swingDoor.bOffset = direction.ToIntVector2();
			swingDoor.direction = direction;
			swingDoor.ec = ec;
			if (ec.cells[cell.position.x + direction.ToIntVector2().x, cell.position.z + direction.ToIntVector2().z] != null)
			{
				cell.HardCoverWall(direction.GetOpposite(), true);
				return;
			}
		}
		else
		{
			Debug.LogWarning("One-way door builder was unable to find a valid position for the wall!");
		}
	}

	// Token: 0x04000418 RID: 1048
	[SerializeField]
	private SwingDoor doorPre;

	// Token: 0x04000419 RID: 1049
	[SerializeField]
	private List<TileShape> tileShapes = new List<TileShape>
	{
		TileShape.Straight
	};

	// Token: 0x0400041A RID: 1050
	[SerializeField]
	private int maxAttempts = 10;
}
