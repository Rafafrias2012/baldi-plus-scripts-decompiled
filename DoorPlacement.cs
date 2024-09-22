using System;

// Token: 0x0200016A RID: 362
public class DoorPlacement
{
	// Token: 0x0600082F RID: 2095 RVA: 0x000295AC File Offset: 0x000277AC
	public DoorPlacement(RoomController room, Door doorPre, IntVector2 position, Direction dir)
	{
		this.room = room;
		this.doorPre = doorPre;
		this.position = position;
		this.dir = dir;
	}

	// Token: 0x040008C8 RID: 2248
	public RoomController room;

	// Token: 0x040008C9 RID: 2249
	public Door doorPre;

	// Token: 0x040008CA RID: 2250
	public IntVector2 position;

	// Token: 0x040008CB RID: 2251
	public Direction dir;
}
