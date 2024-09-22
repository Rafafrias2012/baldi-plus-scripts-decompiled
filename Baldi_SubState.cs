using System;

// Token: 0x02000017 RID: 23
public class Baldi_SubState : Baldi_StateBase
{
	// Token: 0x06000099 RID: 153 RVA: 0x00005335 File Offset: 0x00003535
	public Baldi_SubState(NPC npc, Baldi baldi, NpcState previousState) : base(npc, baldi)
	{
		this.previousState = previousState;
	}

	// Token: 0x040000B6 RID: 182
	protected NpcState previousState;
}
