using System;

// Token: 0x0200001F RID: 31
public class Beans_Sprinting : Beans_Wandering
{
	// Token: 0x060000C0 RID: 192 RVA: 0x000059D0 File Offset: 0x00003BD0
	public Beans_Sprinting(Beans beans) : base(beans)
	{
	}

	// Token: 0x060000C1 RID: 193 RVA: 0x000059D9 File Offset: 0x00003BD9
	public override void Enter()
	{
		base.Enter();
		this.sprintTimer = this.beans.SprintTime;
	}

	// Token: 0x060000C2 RID: 194 RVA: 0x000059F2 File Offset: 0x00003BF2
	public override void SprintTimeExpired()
	{
		this.beans.StopSprint();
		this.npc.behaviorStateMachine.ChangeState(new Beans_Wandering(this.beans));
	}
}
