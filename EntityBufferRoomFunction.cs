using System;
using UnityEngine;

// Token: 0x020001AA RID: 426
public class EntityBufferRoomFunction : RoomFunction
{
	// Token: 0x060009A8 RID: 2472 RVA: 0x000337AC File Offset: 0x000319AC
	public override void OnGenerationFinished()
	{
		base.OnGenerationFinished();
		foreach (Cell cell in this.room.cells)
		{
			for (int i = 0; i < 4; i++)
			{
				if (!this.room.ec.ContainsCoordinates(cell.position + ((Direction)i).ToIntVector2()) || this.room.ec.CellFromPosition(cell.position + ((Direction)i).ToIntVector2()).room != this.room)
				{
					Object.Instantiate<GameObject>(this.entityBufferWall, cell.ObjectBase).transform.rotation = ((Direction)i).ToRotation();
				}
			}
		}
	}

	// Token: 0x04000AD8 RID: 2776
	[SerializeField]
	private GameObject entityBufferWall;
}
