using System;
using UnityEngine;

// Token: 0x02000021 RID: 33
public class Beans_Chewing : Beans_StateBase
{
	// Token: 0x060000C6 RID: 198 RVA: 0x00005ADD File Offset: 0x00003CDD
	public Beans_Chewing(Beans beans, PlayerManager player, float chewTime) : base(beans)
	{
		this.player = player;
		this.chewTime = chewTime;
	}

	// Token: 0x060000C7 RID: 199 RVA: 0x00005AF4 File Offset: 0x00003CF4
	public override void Enter()
	{
		base.Enter();
		base.ChangeNavigationState(new NavigationState_DoNothing(this.npc, 0));
	}

	// Token: 0x060000C8 RID: 200 RVA: 0x00005B10 File Offset: 0x00003D10
	public override void Update()
	{
		base.Update();
		this.chewTime -= Time.deltaTime * this.npc.TimeScale;
		if (this.chewTime <= 1.25f && !this.blowSoundPlayed)
		{
			this.beans.Blow();
			this.blowSoundPlayed = true;
		}
		if (this.chewTime <= 0f)
		{
			this.beans.Spit(this.player);
			this.npc.behaviorStateMachine.ChangeState(new Beans_Watch(this.beans));
		}
	}

	// Token: 0x040000D4 RID: 212
	private PlayerManager player;

	// Token: 0x040000D5 RID: 213
	private bool blowSoundPlayed;

	// Token: 0x040000D6 RID: 214
	private float chewTime;
}
