using System;
using UnityEngine;

// Token: 0x0200004C RID: 76
public class NoLateTeacher_Inform : NoLateTeacher_StateBase
{
	// Token: 0x060001CE RID: 462 RVA: 0x0000AA8B File Offset: 0x00008C8B
	public NoLateTeacher_Inform(NoLateTeacher pomp, PlayerManager player, float time) : base(pomp)
	{
		this.player = player;
		this.time = time;
	}

	// Token: 0x060001CF RID: 463 RVA: 0x0000AAA4 File Offset: 0x00008CA4
	public override void Update()
	{
		base.Update();
		this.time -= Time.deltaTime;
		if (this.time <= 0f && !this.playerReleased)
		{
			this.pomp.ReleasePlayer(this.player);
			this.playerReleased = true;
		}
		if (this.time <= 0f && !this.pomp.IsSpeaking)
		{
			this.pomp.HeadToClass(this.player);
		}
	}

	// Token: 0x040001E6 RID: 486
	private PlayerManager player;

	// Token: 0x040001E7 RID: 487
	private float time;

	// Token: 0x040001E8 RID: 488
	private bool playerReleased;
}
