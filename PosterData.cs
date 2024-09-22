using System;

// Token: 0x0200017E RID: 382
[Serializable]
public class PosterData
{
	// Token: 0x060008D8 RID: 2264 RVA: 0x0002D764 File Offset: 0x0002B964
	public PosterData GetNew()
	{
		return new PosterData
		{
			poster = this.poster,
			position = new IntVector2(this.position.x, this.position.z),
			direction = this.direction
		};
	}

	// Token: 0x04000991 RID: 2449
	public PosterObject poster;

	// Token: 0x04000992 RID: 2450
	public IntVector2 position;

	// Token: 0x04000993 RID: 2451
	public Direction direction;
}
