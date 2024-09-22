using System;

// Token: 0x02000134 RID: 308
public class RuleFreeZone : RoomFunction
{
	// Token: 0x06000748 RID: 1864 RVA: 0x0002593C File Offset: 0x00023B3C
	public override void OnPlayerStay(PlayerManager player)
	{
		base.OnPlayerStay(player);
		player.ClearGuilt();
	}
}
