using System;

// Token: 0x0200005E RID: 94
public class Playtime_Playing : Playtime_StateBase
{
	// Token: 0x0600021B RID: 539 RVA: 0x0000BDD1 File Offset: 0x00009FD1
	public Playtime_Playing(NPC npc, Playtime playtime) : base(npc, playtime)
	{
	}

	// Token: 0x0600021C RID: 540 RVA: 0x0000BDDB File Offset: 0x00009FDB
	public override void Enter()
	{
	}

	// Token: 0x0600021D RID: 541 RVA: 0x0000BDDD File Offset: 0x00009FDD
	public override void PlayerLost(PlayerManager player)
	{
		base.PlayerLost(player);
		this.playtime.EndJumprope(false);
	}
}
