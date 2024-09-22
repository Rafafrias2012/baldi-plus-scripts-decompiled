using System;

// Token: 0x020001A4 RID: 420
public class CallSpecialManagerRoomFunction : RoomFunction
{
	// Token: 0x06000995 RID: 2453 RVA: 0x000332FA File Offset: 0x000314FA
	public override void OnPlayerEnter(PlayerManager player)
	{
		base.OnPlayerEnter(player);
		Singleton<BaseGameManager>.Instance.CallSpecialManagerFunction(this.value, base.gameObject);
	}

	// Token: 0x04000AD1 RID: 2769
	public int value;
}
