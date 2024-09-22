using System;

// Token: 0x02000056 RID: 86
public class TestNPC : NPC
{
	// Token: 0x060001EC RID: 492 RVA: 0x0000B24E File Offset: 0x0000944E
	public override void Initialize()
	{
		base.Initialize();
		this.behaviorStateMachine.ChangeState(new TestNPC_DefaultState(this, this));
	}

	// Token: 0x060001ED RID: 493 RVA: 0x0000B268 File Offset: 0x00009468
	protected override void VirtualUpdate()
	{
		base.VirtualUpdate();
	}

	// Token: 0x060001EE RID: 494 RVA: 0x0000B270 File Offset: 0x00009470
	public void GoCrazy()
	{
		this.behaviorStateMachine.ChangeState(new TestNPC_Crazy(this, this));
	}
}
