using System;

// Token: 0x02000067 RID: 103
public class Principal_SubState : Principal_StateBase
{
	// Token: 0x06000247 RID: 583 RVA: 0x0000CA29 File Offset: 0x0000AC29
	public Principal_SubState(Principal principal, NpcState previousState) : base(principal)
	{
		this.previousState = previousState;
	}

	// Token: 0x04000253 RID: 595
	protected NpcState previousState;
}
