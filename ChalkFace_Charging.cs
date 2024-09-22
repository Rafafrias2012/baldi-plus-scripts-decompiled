using System;

// Token: 0x0200002C RID: 44
public class ChalkFace_Charging : ChalkFace_StateBase
{
	// Token: 0x06000107 RID: 263 RVA: 0x000071DD File Offset: 0x000053DD
	public ChalkFace_Charging(ChalkFace chalkles, Chalkboard chalkboard) : base(chalkles)
	{
		this.chalkles = chalkles;
		this.chalkboard = chalkboard;
	}

	// Token: 0x06000108 RID: 264 RVA: 0x000071F4 File Offset: 0x000053F4
	public override void Enter()
	{
		base.Enter();
		this.chalkles.Teleport(this.chalkboard);
	}

	// Token: 0x06000109 RID: 265 RVA: 0x00007210 File Offset: 0x00005410
	public override void Update()
	{
		base.Update();
		if (this.chalkles.AdvanceTimer())
		{
			this.chalkles.state = new ChalkFace_Laughing(this.chalkles, this.chalkboard);
			this.npc.behaviorStateMachine.ChangeState(this.chalkles.state);
		}
	}

	// Token: 0x0400011F RID: 287
	private Chalkboard chalkboard;
}
