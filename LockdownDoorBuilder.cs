using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000A9 RID: 169
public class LockdownDoorBuilder : ObjectBuilder
{
	// Token: 0x060003DF RID: 991 RVA: 0x00014310 File Offset: 0x00012510
	public override void Build(EnvironmentController ec, LevelBuilder builder, RoomController room, Random cRng)
	{
		int num = 0;
		List<Cell> tilesOfShape = room.GetTilesOfShape(this.tileShapes, true);
		Cell cell = null;
		while (cell == null && tilesOfShape.Count > 0 && num < this.maxAttempts)
		{
			int index = cRng.Next(0, tilesOfShape.Count);
			num++;
			if (ec.TrapCheck(tilesOfShape[index]) || tilesOfShape[index].open || tilesOfShape[index].HasAnyHardCoverage)
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
			LockdownDoor lockdownDoor = Object.Instantiate<LockdownDoor>(this.doorPre, room.transform);
			Direction direction = cell.AllOpenNavDirections[cRng.Next(0, cell.AllOpenNavDirections.Count)];
			cell.HardCoverEntirely();
			lockdownDoor.transform.position = cell.FloorWorldPosition;
			lockdownDoor.transform.rotation = direction.ToRotation();
			lockdownDoor.ec = ec;
			lockdownDoor.position = cell.position;
			lockdownDoor.bOffset = direction.ToIntVector2();
			lockdownDoor.direction = direction;
			if (GameButton.BuildInArea(ec, cell.position, cell.position, this.buttonRange, lockdownDoor.gameObject, this.buttonPre, cRng) == null)
			{
				Object.Destroy(lockdownDoor.gameObject);
				return;
			}
		}
		else
		{
			Debug.LogWarning("Lockdown door builder was unable to find a valid position for the door!");
		}
	}

	// Token: 0x04000412 RID: 1042
	[SerializeField]
	private LockdownDoor doorPre;

	// Token: 0x04000413 RID: 1043
	[SerializeField]
	private GameButton buttonPre;

	// Token: 0x04000414 RID: 1044
	[SerializeField]
	private List<TileShape> tileShapes = new List<TileShape>
	{
		TileShape.Straight,
		TileShape.Single
	};

	// Token: 0x04000415 RID: 1045
	[SerializeField]
	private int maxAttempts = 10;

	// Token: 0x04000416 RID: 1046
	[SerializeField]
	private int buttonRange;
}
