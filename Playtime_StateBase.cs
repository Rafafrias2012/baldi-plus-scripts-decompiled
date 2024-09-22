using System;

// Token: 0x0200005C RID: 92
public class Playtime_StateBase : NpcState
{
	// Token: 0x06000212 RID: 530 RVA: 0x0000BC77 File Offset: 0x00009E77
	public Playtime_StateBase(NPC npc, Playtime playtime) : base(npc)
	{
		this.playtime = playtime;
	}

	// Token: 0x0400022B RID: 555
	protected Playtime playtime;
}
