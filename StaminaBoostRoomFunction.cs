using System;
using UnityEngine;

// Token: 0x020001B1 RID: 433
public class StaminaBoostRoomFunction : RoomFunction
{
	// Token: 0x060009C5 RID: 2501 RVA: 0x00034220 File Offset: 0x00032420
	public override void OnPlayerStay(PlayerManager player)
	{
		base.OnPlayerStay(player);
		player.plm.AddStamina(player.plm.staminaDrop * Time.deltaTime * player.PlayerTimeScale, true);
	}
}
