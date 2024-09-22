using System;

// Token: 0x02000168 RID: 360
public struct CullFill
{
	// Token: 0x06000826 RID: 2086 RVA: 0x0002939B File Offset: 0x0002759B
	public CullFill(IntVector2 position, int disabledDirections, int forwardDistance, int sidewaysDistance)
	{
		this.position = position;
		this.disabledDirections = disabledDirections;
		this.forwardDistance = forwardDistance;
		this.sidewaysDistance = sidewaysDistance;
	}

	// Token: 0x040008B9 RID: 2233
	public IntVector2 position;

	// Token: 0x040008BA RID: 2234
	public int disabledDirections;

	// Token: 0x040008BB RID: 2235
	public int forwardDistance;

	// Token: 0x040008BC RID: 2236
	public int sidewaysDistance;
}
