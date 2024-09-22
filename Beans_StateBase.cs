using System;

// Token: 0x0200001C RID: 28
public class Beans_StateBase : NpcState
{
	// Token: 0x060000B8 RID: 184 RVA: 0x000058E2 File Offset: 0x00003AE2
	public Beans_StateBase(Beans beans) : base(beans)
	{
		this.beans = beans;
	}

	// Token: 0x040000D1 RID: 209
	protected Beans beans;
}
