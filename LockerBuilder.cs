using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000A3 RID: 163
public class LockerBuilder : HallBuilder
{
	// Token: 0x060003C2 RID: 962 RVA: 0x00013548 File Offset: 0x00011748
	public override void Build(Cell tile, EnvironmentController ec, Random cRNG)
	{
		this.lockerCount = cRNG.Next(this.minLockers, this.maxLockers);
		Direction direction = tile.RandomConstDirection(cRNG);
		if (direction != Direction.Null)
		{
			List<Direction> list = direction.PerpendicularList();
			this.coverage |= direction.ToCoverage();
			List<Cell> list2 = new List<Cell>();
			list2.Add(tile);
			while (this.totalLockers < this.lockerCount && list2.Count > 0)
			{
				Cell cell = list2[0];
				list2.RemoveAt(0);
				if (direction == Direction.North || direction == Direction.South)
				{
					this.currentSign = (int)Mathf.Sign((float)(cell.position.x - tile.position.x)) * direction.ToIntVector2().z;
				}
				else
				{
					this.currentSign = (int)Mathf.Sign((float)(cell.position.z - tile.position.z)) * direction.ToIntVector2().x;
				}
				this.AddLockers(cell, direction, this.currentSign, cRNG);
				foreach (Direction direction2 in list)
				{
					if (Directions.OpenDirectionsFromBin(cell.ConstBin).Contains(direction2))
					{
						IntVector2 intVector = direction2.ToIntVector2();
						IntVector2 intVector2 = new IntVector2(cell.position.x + intVector.x, cell.position.z + intVector.z);
						if (ec.ContainsCoordinates(intVector2) && !ec.cells[intVector2.x, intVector2.z].Null && ec.cells[intVector2.x, intVector2.z].room == tile.room)
						{
							Cell cell2 = ec.cells[intVector2.x, intVector2.z];
							if (Directions.ClosedDirectionsFromBin(cell2.ConstBin).Contains(direction) && cell2.HardCoverageFits(this.coverage))
							{
								list2.Add(cell2);
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x060003C3 RID: 963 RVA: 0x00013784 File Offset: 0x00011984
	private void AddLockers(Cell cell, Direction dir, int sign, Random cRNG)
	{
		if (cell.HardCoverageFits(this.coverage))
		{
			int num = -2 * sign;
			while (num <= 2 && num >= -2 && this.totalLockers < this.lockerCount)
			{
				MeshRenderer meshRenderer;
				if ((float)cRNG.NextDouble() * 100f < this.hideableChance)
				{
					meshRenderer = Object.Instantiate<MeshRenderer>(this.hideLockerPre, cell.ObjectBase);
					cell.AddRenderer(meshRenderer);
				}
				else
				{
					meshRenderer = Object.Instantiate<MeshRenderer>(this.lockerPre, cell.ObjectBase);
					cell.AddRenderer(meshRenderer);
				}
				meshRenderer.transform.rotation = dir.ToRotation();
				IntVector2 intVector = dir.ToIntVector2();
				meshRenderer.transform.position = cell.FloorWorldPosition + new Vector3((float)(intVector.x * 4 + num * 2 * intVector.z), 0f, (float)(intVector.z * 4 + num * 2 * intVector.x));
				this.totalLockers++;
				cell.HardCover(this.coverage);
				num += sign;
			}
		}
	}

	// Token: 0x040003EE RID: 1006
	[SerializeField]
	private MeshRenderer lockerPre;

	// Token: 0x040003EF RID: 1007
	[SerializeField]
	private MeshRenderer hideLockerPre;

	// Token: 0x040003F0 RID: 1008
	private CellCoverage coverage = CellCoverage.Down;

	// Token: 0x040003F1 RID: 1009
	[SerializeField]
	private float hideableChance = 2f;

	// Token: 0x040003F2 RID: 1010
	[SerializeField]
	private int minLockers = 15;

	// Token: 0x040003F3 RID: 1011
	[SerializeField]
	private int maxLockers = 40;

	// Token: 0x040003F4 RID: 1012
	private int totalLockers;

	// Token: 0x040003F5 RID: 1013
	private int lockerCount;

	// Token: 0x040003F6 RID: 1014
	private int currentSign;
}
