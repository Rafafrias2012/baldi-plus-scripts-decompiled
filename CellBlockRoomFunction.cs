using System;
using UnityEngine;

// Token: 0x020001A5 RID: 421
public class CellBlockRoomFunction : RoomFunction
{
	// Token: 0x06000997 RID: 2455 RVA: 0x00033324 File Offset: 0x00031524
	public override void OnGenerationFinished()
	{
		base.OnGenerationFinished();
		foreach (Transform transform in this.cellToBlock)
		{
			Cell cell = this.room.ec.CellFromPosition(transform.position);
			for (int j = 0; j < 4; j++)
			{
				if (cell.ConstNavigable((Direction)j))
				{
					this.room.ec.CellFromPosition(cell.position + ((Direction)j).ToIntVector2()).Block(((Direction)j).GetOpposite(), true);
				}
			}
		}
	}

	// Token: 0x04000AD2 RID: 2770
	[SerializeField]
	private Transform[] cellToBlock = new Transform[0];
}
