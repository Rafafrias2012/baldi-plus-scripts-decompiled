using System;

// Token: 0x02000040 RID: 64
public class GottaSweep_Returning : GottaSweep_StateBase
{
	// Token: 0x06000189 RID: 393 RVA: 0x00009503 File Offset: 0x00007703
	public GottaSweep_Returning(NPC npc, GottaSweep gottaSweep) : base(npc, gottaSweep)
	{
	}

	// Token: 0x0600018A RID: 394 RVA: 0x0000950D File Offset: 0x0000770D
	public override void Enter()
	{
		base.Enter();
		base.ChangeNavigationState(new NavigationState_TargetPosition(this.npc, 63, this.gottaSweep.home));
	}

	// Token: 0x0600018B RID: 395 RVA: 0x00009534 File Offset: 0x00007734
	public override void DestinationEmpty()
	{
		base.DestinationEmpty();
		if (!this.gottaSweep.IsHome)
		{
			this.npc.behaviorStateMachine.CurrentNavigationState.UpdatePosition(this.gottaSweep.home);
			return;
		}
		this.npc.behaviorStateMachine.ChangeState(new GottaSweep_Wait(this.npc, this.gottaSweep));
	}
}
