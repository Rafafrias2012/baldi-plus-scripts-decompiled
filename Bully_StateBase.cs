using System;

// Token: 0x02000026 RID: 38
public class Bully_StateBase : NpcState
{
	// Token: 0x060000ED RID: 237 RVA: 0x00006904 File Offset: 0x00004B04
	public Bully_StateBase(NPC npc, Bully bully) : base(npc)
	{
		this.bully = bully;
	}

	// Token: 0x04000104 RID: 260
	protected Bully bully;
}
