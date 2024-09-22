using System;

// Token: 0x0200002A RID: 42
public class ChalkFace_StateBase : NpcState
{
	// Token: 0x06000101 RID: 257 RVA: 0x00007130 File Offset: 0x00005330
	public ChalkFace_StateBase(ChalkFace chalkles) : base(chalkles)
	{
		this.chalkles = chalkles;
	}

	// Token: 0x06000102 RID: 258 RVA: 0x00007140 File Offset: 0x00005340
	public virtual void Teleport(Chalkboard chalkboard)
	{
		this.chalkles.state = new ChalkFace_Charging(this.chalkles, chalkboard);
		this.chalkles.behaviorStateMachine.ChangeState(this.chalkles.state);
	}

	// Token: 0x06000103 RID: 259 RVA: 0x00007174 File Offset: 0x00005374
	public virtual void Cancel()
	{
		this.chalkles.state = new ChalkFace_Idle(this.chalkles);
		this.chalkles.behaviorStateMachine.ChangeState(this.chalkles.state);
	}

	// Token: 0x0400011E RID: 286
	public ChalkFace chalkles;
}
