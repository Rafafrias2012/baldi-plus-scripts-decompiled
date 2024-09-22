using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000B0 RID: 176
public class RotoHallBuilder : ObjectBuilder
{
	// Token: 0x060003F5 RID: 1013 RVA: 0x00014C78 File Offset: 0x00012E78
	public override void Build(EnvironmentController ec, LevelBuilder builder, RoomController room, Random cRng)
	{
		List<Cell> list = new List<Cell>();
		List<TileShape> list2 = new List<TileShape>();
		list2.Add(TileShape.Open);
		list = room.GetTilesOfShape(list2, false);
		for (int i = 0; i < list.Count; i++)
		{
			if (ec.TrapCheck(list[i]))
			{
				list.RemoveAt(i);
				i--;
			}
		}
		if (list.Count == 0)
		{
			list2.Clear();
			list2.Add(TileShape.Single);
			list = room.GetTilesOfShape(list2, false);
		}
		Cell cell = null;
		while (list.Count > 0 && cell == null)
		{
			int index = cRng.Next(0, list.Count);
			if (list[index].HasAnyHardCoverage || ec.TrapCheck(list[index]))
			{
				list.RemoveAt(index);
			}
			else
			{
				cell = list[index];
			}
		}
		if (cell != null)
		{
			this.rotoHall = Object.Instantiate<RotoHall>(this.rotoHallPre, cell.ObjectBase);
			this.rotoHall.Ec = ec;
			MeshRenderer newCylinder = this.straightCylinderPre;
			CylinderShape shape = CylinderShape.Straight;
			if (cRng.Next(0, 2) == 1 || cell.shape == TileShape.Single)
			{
				newCylinder = this.cornerCylinderPre;
				shape = CylinderShape.Corner;
			}
			bool spinClockwise = true;
			if (cRng.Next(0, 2) == 1)
			{
				spinClockwise = false;
			}
			this.rotoHall.Setup((Direction)cRng.Next(0, 4), newCylinder, shape, cell, spinClockwise);
			cell.HardCoverEntirely();
			if (GameButton.BuildInArea(ec, cell.position, cell.position, this.buttonRange, this.rotoHall.gameObject, this.buttonPre, cRng) == null)
			{
				Debug.LogWarning("No suitable location for a roto hall button was found. Destroying the roto hall");
				Object.Destroy(this.rotoHall);
			}
		}
	}

	// Token: 0x0400042D RID: 1069
	[SerializeField]
	private RotoHall rotoHallPre;

	// Token: 0x0400042E RID: 1070
	private RotoHall rotoHall;

	// Token: 0x0400042F RID: 1071
	[SerializeField]
	private MeshRenderer cornerCylinderPre;

	// Token: 0x04000430 RID: 1072
	[SerializeField]
	private MeshRenderer straightCylinderPre;

	// Token: 0x04000431 RID: 1073
	[SerializeField]
	private GameButton buttonPre;

	// Token: 0x04000432 RID: 1074
	[SerializeField]
	private int buttonRange = 6;
}
