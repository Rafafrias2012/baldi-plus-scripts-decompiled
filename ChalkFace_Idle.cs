using System;

// Token: 0x0200002B RID: 43
public class ChalkFace_Idle : ChalkFace_StateBase
{
	// Token: 0x06000104 RID: 260 RVA: 0x000071A7 File Offset: 0x000053A7
	public ChalkFace_Idle(ChalkFace chalkles) : base(chalkles)
	{
		this.chalkles = chalkles;
	}

	// Token: 0x06000105 RID: 261 RVA: 0x000071B7 File Offset: 0x000053B7
	public override void Enter()
	{
		base.Enter();
		this.chalkles.Cancel();
	}

	// Token: 0x06000106 RID: 262 RVA: 0x000071CA File Offset: 0x000053CA
	public override void Update()
	{
		base.Update();
		this.chalkles.DecreaseTimer();
	}
}
