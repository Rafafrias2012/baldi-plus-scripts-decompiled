using System;

// Token: 0x02000039 RID: 57
public class DrReflex_Angry : DrReflex_StateBase
{
	// Token: 0x06000165 RID: 357 RVA: 0x00008E2C File Offset: 0x0000702C
	public DrReflex_Angry(DrReflex drReflex, PlayerManager player) : base(drReflex)
	{
		this.player = player;
	}

	// Token: 0x06000166 RID: 358 RVA: 0x00008E3C File Offset: 0x0000703C
	public override void Enter()
	{
		base.Enter();
		this.drReflex.HeadToOffice();
	}

	// Token: 0x06000167 RID: 359 RVA: 0x00008E4F File Offset: 0x0000704F
	public override void DestinationEmpty()
	{
		base.DestinationEmpty();
		this.npc.behaviorStateMachine.ChangeState(new DrReflex_Hunting(this.drReflex, this.player));
	}

	// Token: 0x0400017D RID: 381
	protected PlayerManager player;
}
