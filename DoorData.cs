using System;

// Token: 0x02000178 RID: 376
[Serializable]
public class DoorData
{
	// Token: 0x060008D0 RID: 2256 RVA: 0x0002D64A File Offset: 0x0002B84A
	public DoorData(int roomId, Door doorPre, IntVector2 position, Direction dir)
	{
		this.roomId = roomId;
		this.doorPre = doorPre;
		this.position = position;
		this.dir = dir;
	}

	// Token: 0x0400097A RID: 2426
	public int roomId;

	// Token: 0x0400097B RID: 2427
	public Door doorPre;

	// Token: 0x0400097C RID: 2428
	public IntVector2 position;

	// Token: 0x0400097D RID: 2429
	public Direction dir;
}
