using System;

// Token: 0x0200001D RID: 29
public class Beans_NewWandering : Beans_StateBase
{
	// Token: 0x060000B9 RID: 185 RVA: 0x000058F2 File Offset: 0x00003AF2
	public Beans_NewWandering(Beans beans) : base(beans)
	{
	}

	// Token: 0x060000BA RID: 186 RVA: 0x000058FB File Offset: 0x00003AFB
	public override void Enter()
	{
		base.Enter();
		base.ChangeNavigationState(new NavigationState_WanderRandom(this.npc, 0));
		this.npc.behaviorStateMachine.ChangeState(new Beans_Wandering(this.beans));
	}
}
