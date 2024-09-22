using System;
using UnityEngine;

// Token: 0x020001A3 RID: 419
public class AmbienceRoomFunction : RoomFunction
{
	// Token: 0x06000992 RID: 2450 RVA: 0x000332C0 File Offset: 0x000314C0
	public override void OnPlayerEnter(PlayerManager player)
	{
		base.OnPlayerEnter(player);
		this.source.volume = 1f;
	}

	// Token: 0x06000993 RID: 2451 RVA: 0x000332D9 File Offset: 0x000314D9
	public override void OnPlayerExit(PlayerManager player)
	{
		base.OnPlayerExit(player);
		this.source.volume = 0f;
	}

	// Token: 0x04000AD0 RID: 2768
	[SerializeField]
	private AudioSource source;
}
