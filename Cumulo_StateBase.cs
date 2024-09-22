using System;

// Token: 0x02000030 RID: 48
public class Cumulo_StateBase : NpcState
{
	// Token: 0x0600011C RID: 284 RVA: 0x00007A2E File Offset: 0x00005C2E
	public Cumulo_StateBase(Cumulo cumulo) : base(cumulo)
	{
		this.cumulo = cumulo;
	}

	// Token: 0x04000133 RID: 307
	protected Cumulo cumulo;
}
