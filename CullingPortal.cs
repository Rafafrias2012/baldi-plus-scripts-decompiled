using System;

// Token: 0x02000164 RID: 356
public struct CullingPortal
{
	// Token: 0x0600080D RID: 2061 RVA: 0x00027E27 File Offset: 0x00026027
	public CullingPortal(IntVector2 position, Direction direction)
	{
		this.position = position;
		this.direction = direction;
	}

	// Token: 0x0400089A RID: 2202
	public IntVector2 position;

	// Token: 0x0400089B RID: 2203
	public Direction direction;
}
