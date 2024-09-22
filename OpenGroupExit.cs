using System;

// Token: 0x0200019A RID: 410
public struct OpenGroupExit
{
	// Token: 0x0600094E RID: 2382 RVA: 0x00031944 File Offset: 0x0002FB44
	public OpenGroupExit(Cell cell, Direction direction)
	{
		this.cell = cell;
		this.direction = direction;
	}

	// Token: 0x0600094F RID: 2383 RVA: 0x00031954 File Offset: 0x0002FB54
	public Cell OutputCell(EnvironmentController ec)
	{
		return ec.CellFromPosition(this.cell.position + this.direction.ToIntVector2());
	}

	// Token: 0x04000A36 RID: 2614
	public Cell cell;

	// Token: 0x04000A37 RID: 2615
	public Direction direction;
}
