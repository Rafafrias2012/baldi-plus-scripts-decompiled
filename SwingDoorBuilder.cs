using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000B2 RID: 178
public class SwingDoorBuilder : ObjectBuilder
{
	// Token: 0x06000401 RID: 1025 RVA: 0x00015050 File Offset: 0x00013250
	public override void Build(EnvironmentController ec, LevelBuilder builder, RoomController room, Random cRng)
	{
		this.hallways = new List<List<Cell>>(ec.FindHallways());
		for (int i = 0; i < this.hallways.Count; i++)
		{
			if (this.hallways[i].Count >= this.minHallLength && (float)cRng.NextDouble() < this.spawnChance)
			{
				List<Cell> list = new List<Cell>();
				foreach (Cell cell in this.hallways[i])
				{
					if (cell.shape == TileShape.Straight)
					{
						list.Add(cell);
					}
				}
				if (list.Count > 0)
				{
					Cell cell2 = list[cRng.Next(0, list.Count)];
					List<Direction> list2 = Directions.OpenDirectionsFromBin(cell2.ConstBin);
					cell2.FilterDirectionsThroughHardCoverage(list2, false);
					for (int j = 0; j < list2.Count; j++)
					{
						if (ec.cells[cell2.position.x + list2[j].ToIntVector2().x, cell2.position.z + list2[j].ToIntVector2().z].WallHardCovered(list2[j].GetOpposite()))
						{
							list2.RemoveAt(j);
							j--;
						}
					}
					if (list2.Count > 0)
					{
						Direction direction = list2[cRng.Next(0, list2.Count)];
						IntVector2 intVector = direction.ToIntVector2();
						bool flag = ec.cells[cell2.position.x + intVector.x, cell2.position.z + intVector.z] != null;
						SwingDoor swingDoor = Object.Instantiate<SwingDoor>(this.swingDoorPre, cell2.room.transform);
						swingDoor.transform.position = cell2.FloorWorldPosition;
						swingDoor.transform.rotation = direction.ToRotation();
						swingDoor.ec = ec;
						swingDoor.position = cell2.position;
						swingDoor.bOffset = direction.ToIntVector2();
						swingDoor.direction = direction;
						cell2.HardCoverWall(direction, true);
						if (flag)
						{
							cell2.HardCoverWall(direction.GetOpposite(), true);
						}
					}
				}
			}
		}
	}

	// Token: 0x04000440 RID: 1088
	private List<List<Cell>> hallways;

	// Token: 0x04000441 RID: 1089
	[SerializeField]
	private SwingDoor swingDoorPre;

	// Token: 0x04000442 RID: 1090
	[SerializeField]
	private float spawnChance = 0.2f;

	// Token: 0x04000443 RID: 1091
	[SerializeField]
	private int minHallLength = 5;
}
