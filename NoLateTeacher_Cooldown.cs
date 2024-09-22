using System;
using UnityEngine;

// Token: 0x0200004B RID: 75
public class NoLateTeacher_Cooldown : NoLateTeacher_Wander
{
	// Token: 0x060001C8 RID: 456 RVA: 0x0000AA08 File Offset: 0x00008C08
	public NoLateTeacher_Cooldown(NoLateTeacher pomp, float time) : base(pomp)
	{
		this.time = time;
	}

	// Token: 0x060001C9 RID: 457 RVA: 0x0000AA18 File Offset: 0x00008C18
	public override void Enter()
	{
		base.Enter();
		this.pomp.WanderSpeed();
	}

	// Token: 0x060001CA RID: 458 RVA: 0x0000AA2C File Offset: 0x00008C2C
	public override void Update()
	{
		base.Update();
		this.time -= Time.deltaTime * this.npc.TimeScale;
		if (this.time <= 0f)
		{
			this.npc.behaviorStateMachine.ChangeState(new NoLateTeacher_Wander(this.pomp));
		}
	}

	// Token: 0x060001CB RID: 459 RVA: 0x0000AA85 File Offset: 0x00008C85
	public override void PlayerSighted(PlayerManager player)
	{
	}

	// Token: 0x060001CC RID: 460 RVA: 0x0000AA87 File Offset: 0x00008C87
	public override void PlayerInSight(PlayerManager player)
	{
	}

	// Token: 0x060001CD RID: 461 RVA: 0x0000AA89 File Offset: 0x00008C89
	public override void OnStateTriggerStay(Collider other)
	{
	}

	// Token: 0x040001E5 RID: 485
	private float time;
}
