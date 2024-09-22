using System;

// Token: 0x020001B4 RID: 436
public class WallSoftCoverRoomFunction : RoomFunction
{
	// Token: 0x060009DB RID: 2523 RVA: 0x00034EE4 File Offset: 0x000330E4
	public override void Build(LevelBuilder builder, Random rng)
	{
		base.Build(builder, rng);
		foreach (Cell cell in this.room.cells)
		{
			foreach (Direction dir in Directions.All())
			{
				cell.SoftCoverWall(dir);
			}
		}
	}
}
