using System;

// Token: 0x020000FA RID: 250
public class FacultyTrigger : RoomFunction
{
	// Token: 0x060005E2 RID: 1506 RVA: 0x0001D7A8 File Offset: 0x0001B9A8
	public override void OnPlayerStay(PlayerManager player)
	{
		base.OnPlayerStay(player);
		player.RuleBreak("Faculty", 1f, 0.25f);
	}

	// Token: 0x060005E3 RID: 1507 RVA: 0x0001D7C6 File Offset: 0x0001B9C6
	public override void OnPlayerExit(PlayerManager player)
	{
		base.OnPlayerExit(player);
		player.RuleBreak("Faculty", 1f);
	}
}
