using System;
using UnityEngine;

// Token: 0x0200004F RID: 79
public class NoLateTeacher_AttackDelay : NoLateTeacher_StateBase
{
	// Token: 0x060001D7 RID: 471 RVA: 0x0000AED1 File Offset: 0x000090D1
	public NoLateTeacher_AttackDelay(NoLateTeacher pomp, PlayerManager player, float delay) : base(pomp)
	{
		this.player = player;
		this.delay = delay;
	}

	// Token: 0x060001D8 RID: 472 RVA: 0x0000AEE8 File Offset: 0x000090E8
	public override void Update()
	{
		base.Update();
		this.delay -= Time.deltaTime * this.npc.TimeScale;
		if (this.delay <= 0f)
		{
			this.npc.behaviorStateMachine.ChangeState(new NoLateTeacher_Returning(this.pomp, this.player));
		}
	}

	// Token: 0x040001F0 RID: 496
	private PlayerManager player;

	// Token: 0x040001F1 RID: 497
	private float delay;
}
