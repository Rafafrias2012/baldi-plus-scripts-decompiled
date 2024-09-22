using System;

// Token: 0x0200018A RID: 394
public struct RoomSpawn
{
	// Token: 0x06000916 RID: 2326 RVA: 0x000304D9 File Offset: 0x0002E6D9
	public RoomSpawn(IntVector2 position, Direction direction)
	{
		this.position = position;
		this.direction = direction;
	}

	// Token: 0x040009F5 RID: 2549
	public IntVector2 position;

	// Token: 0x040009F6 RID: 2550
	public Direction direction;
}
