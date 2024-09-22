using System;

// Token: 0x02000043 RID: 67
public class LookAtGuy_Inactive : LookAtGuy_BaseState
{
	// Token: 0x06000198 RID: 408 RVA: 0x00009970 File Offset: 0x00007B70
	public LookAtGuy_Inactive(LookAtGuy theTest) : base(theTest)
	{
	}

	// Token: 0x06000199 RID: 409 RVA: 0x0000997C File Offset: 0x00007B7C
	public override void Enter()
	{
		base.Enter();
		if (this.npc.behaviorStateMachine.CurrentNavigationState != null)
		{
			this.npc.behaviorStateMachine.CurrentNavigationState.priority = 0;
		}
		this.npc.behaviorStateMachine.ChangeNavigationState(new NavigationState_DoNothing(this.npc, 127, true));
	}

	// Token: 0x0600019A RID: 410 RVA: 0x000099D5 File Offset: 0x00007BD5
	public override void Sighted()
	{
		base.Sighted();
		this.npc.behaviorStateMachine.ChangeState(new LookAtGuy_Activating(this.theTest));
	}
}
