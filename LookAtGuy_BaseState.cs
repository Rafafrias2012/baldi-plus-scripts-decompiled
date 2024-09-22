using System;

// Token: 0x02000042 RID: 66
public class LookAtGuy_BaseState : NpcState
{
	// Token: 0x06000195 RID: 405 RVA: 0x00009938 File Offset: 0x00007B38
	public LookAtGuy_BaseState(LookAtGuy theTest) : base(theTest)
	{
		this.theTest = theTest;
	}

	// Token: 0x06000196 RID: 406 RVA: 0x00009948 File Offset: 0x00007B48
	public override void Sighted()
	{
		base.Sighted();
		this.theTest.FreezeNPCs(true);
	}

	// Token: 0x06000197 RID: 407 RVA: 0x0000995C File Offset: 0x00007B5C
	public override void Unsighted()
	{
		base.Unsighted();
		this.theTest.FreezeNPCs(false);
	}

	// Token: 0x040001A7 RID: 423
	protected LookAtGuy theTest;
}
