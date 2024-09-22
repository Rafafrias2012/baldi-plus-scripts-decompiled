using System;

// Token: 0x02000007 RID: 7
public class FirstPrize_StateBase : NpcState
{
	// Token: 0x06000021 RID: 33 RVA: 0x00002BBB File Offset: 0x00000DBB
	public FirstPrize_StateBase(FirstPrize firstPrize) : base(firstPrize)
	{
		this.firstPrize = firstPrize;
	}

	// Token: 0x04000043 RID: 67
	protected FirstPrize firstPrize;
}
