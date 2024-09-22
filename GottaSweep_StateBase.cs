using System;

// Token: 0x0200003D RID: 61
public class GottaSweep_StateBase : NpcState
{
	// Token: 0x06000182 RID: 386 RVA: 0x000093B4 File Offset: 0x000075B4
	public GottaSweep_StateBase(NPC npc, GottaSweep gottaSweep) : base(npc)
	{
		this.gottaSweep = gottaSweep;
	}

	// Token: 0x04000192 RID: 402
	protected GottaSweep gottaSweep;
}
