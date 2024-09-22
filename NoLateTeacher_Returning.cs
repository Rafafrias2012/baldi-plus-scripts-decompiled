using System;

// Token: 0x02000051 RID: 81
public class NoLateTeacher_Returning : NoLateTeacher_StateBase
{
	// Token: 0x060001DD RID: 477 RVA: 0x0000B046 File Offset: 0x00009246
	public NoLateTeacher_Returning(NoLateTeacher pomp, PlayerManager player) : base(pomp)
	{
		this.player = player;
	}

	// Token: 0x060001DE RID: 478 RVA: 0x0000B056 File Offset: 0x00009256
	public override void Enter()
	{
		base.Enter();
		base.ChangeNavigationState(new NavigationState_TargetPosition(this.npc, 127, this.player.transform.position, true));
	}

	// Token: 0x060001DF RID: 479 RVA: 0x0000B084 File Offset: 0x00009284
	public override void Update()
	{
		base.Update();
		this.currentNavigationState.UpdatePosition(this.player.transform.position);
		if (this.pomp.CanDrag(this.player))
		{
			this.npc.behaviorStateMachine.ChangeState(new NoLateTeacher_Triggered(this.pomp, this.player));
		}
	}

	// Token: 0x040001F4 RID: 500
	private PlayerManager player;
}
