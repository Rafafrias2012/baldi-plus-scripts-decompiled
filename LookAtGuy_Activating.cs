using System;
using UnityEngine;

// Token: 0x02000044 RID: 68
public class LookAtGuy_Activating : LookAtGuy_BaseState
{
	// Token: 0x0600019B RID: 411 RVA: 0x000099F8 File Offset: 0x00007BF8
	public LookAtGuy_Activating(LookAtGuy theTest) : base(theTest)
	{
	}

	// Token: 0x0600019C RID: 412 RVA: 0x00009A0C File Offset: 0x00007C0C
	public override void Enter()
	{
		base.Enter();
		this.theTest.Activate();
	}

	// Token: 0x0600019D RID: 413 RVA: 0x00009A20 File Offset: 0x00007C20
	public override void Update()
	{
		base.Update();
		this.time -= Time.deltaTime;
		if (this.time <= 0f)
		{
			this.npc.behaviorStateMachine.ChangeState(new LookAtGuy_Active(this.theTest));
		}
	}

	// Token: 0x040001A8 RID: 424
	protected float time = 5f;
}
