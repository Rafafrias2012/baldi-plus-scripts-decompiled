using System;
using UnityEngine;

// Token: 0x0200005F RID: 95
public class Playtime_Cooldown : Playtime_StateBase
{
	// Token: 0x0600021E RID: 542 RVA: 0x0000BDF2 File Offset: 0x00009FF2
	public Playtime_Cooldown(NPC npc, Playtime playtime, float time) : base(npc, playtime)
	{
		this.time = time;
	}

	// Token: 0x0600021F RID: 543 RVA: 0x0000BE03 File Offset: 0x0000A003
	public override void Enter()
	{
		base.Enter();
		base.ChangeNavigationState(new NavigationState_WanderRandom(this.npc, 0));
	}

	// Token: 0x06000220 RID: 544 RVA: 0x0000BE1D File Offset: 0x0000A01D
	public override void Update()
	{
		base.Update();
		this.time -= Time.deltaTime * this.npc.TimeScale;
		if (this.time <= 0f)
		{
			this.playtime.EndCooldown();
		}
	}

	// Token: 0x0400022D RID: 557
	private float time;
}
