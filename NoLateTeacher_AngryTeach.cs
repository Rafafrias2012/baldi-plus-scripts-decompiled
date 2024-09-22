using System;
using UnityEngine;

// Token: 0x02000052 RID: 82
public class NoLateTeacher_AngryTeach : NoLateTeacher_StateBase
{
	// Token: 0x060001E0 RID: 480 RVA: 0x0000B0E6 File Offset: 0x000092E6
	public NoLateTeacher_AngryTeach(NoLateTeacher pomp, float time) : base(pomp)
	{
		this.time = time;
	}

	// Token: 0x060001E1 RID: 481 RVA: 0x0000B0F6 File Offset: 0x000092F6
	public override void Enter()
	{
		base.Enter();
		this.currentNavigationState.priority = 0;
		base.ChangeNavigationState(new NavigationState_TargetPosition(this.pomp, 63, this.pomp.GetClassPosition(), true));
	}

	// Token: 0x060001E2 RID: 482 RVA: 0x0000B12C File Offset: 0x0000932C
	public override void Update()
	{
		base.Update();
		this.time -= Time.deltaTime * this.npc.TimeScale;
		if (this.time <= 0f)
		{
			this.pomp.Dismiss();
			this.currentNavigationState.priority = 0;
			this.npc.behaviorStateMachine.ChangeState(new NoLateTeacher_Cooldown(this.pomp, this.pomp.Cooldown));
		}
	}

	// Token: 0x060001E3 RID: 483 RVA: 0x0000B1A7 File Offset: 0x000093A7
	public override void DestinationEmpty()
	{
		base.DestinationEmpty();
		this.currentNavigationState.UpdatePosition(this.pomp.GetClassPosition());
	}

	// Token: 0x040001F5 RID: 501
	public float time;
}
