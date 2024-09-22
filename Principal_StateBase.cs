using System;

// Token: 0x02000061 RID: 97
public class Principal_StateBase : NpcState
{
	// Token: 0x0600022D RID: 557 RVA: 0x0000C475 File Offset: 0x0000A675
	public Principal_StateBase(Principal principal) : base(principal)
	{
		this.principal = principal;
	}

	// Token: 0x0400024C RID: 588
	protected Principal principal;
}
