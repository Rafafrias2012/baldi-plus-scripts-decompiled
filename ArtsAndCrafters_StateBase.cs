using System;

// Token: 0x0200000B RID: 11
public class ArtsAndCrafters_StateBase : NpcState
{
	// Token: 0x0600004B RID: 75 RVA: 0x00004280 File Offset: 0x00002480
	public ArtsAndCrafters_StateBase(ArtsAndCrafters crafters) : base(crafters)
	{
		this.crafters = crafters;
	}

	// Token: 0x0600004C RID: 76 RVA: 0x00004290 File Offset: 0x00002490
	public virtual void SpawnAt(IntVector2 position)
	{
	}

	// Token: 0x04000086 RID: 134
	protected ArtsAndCrafters crafters;
}
