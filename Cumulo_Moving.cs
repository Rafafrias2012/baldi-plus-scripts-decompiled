using System;

// Token: 0x02000031 RID: 49
public class Cumulo_Moving : Cumulo_StateBase
{
	// Token: 0x0600011D RID: 285 RVA: 0x00007A3E File Offset: 0x00005C3E
	public Cumulo_Moving(Cumulo cumulo) : base(cumulo)
	{
	}

	// Token: 0x0600011E RID: 286 RVA: 0x00007A47 File Offset: 0x00005C47
	public override void Enter()
	{
		base.Enter();
		this.cumulo.FindDestination();
	}

	// Token: 0x0600011F RID: 287 RVA: 0x00007A5A File Offset: 0x00005C5A
	public override void DestinationEmpty()
	{
		base.DestinationEmpty();
		this.npc.behaviorStateMachine.ChangeState(new Cumulo_Blowing(this.cumulo, this.cumulo.RandomBlowTime));
	}
}
