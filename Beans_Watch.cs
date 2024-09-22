using System;

// Token: 0x02000022 RID: 34
public class Beans_Watch : Beans_StateBase
{
	// Token: 0x060000C9 RID: 201 RVA: 0x00005BA1 File Offset: 0x00003DA1
	public Beans_Watch(Beans beans) : base(beans)
	{
	}

	// Token: 0x060000CA RID: 202 RVA: 0x00005BAA File Offset: 0x00003DAA
	public override void Enter()
	{
		base.Enter();
		this.beans.Stop();
	}
}
