using System;
using UnityEngine;

// Token: 0x02000068 RID: 104
public class Principal_WhistleApproach : Principal_SubState
{
	// Token: 0x06000248 RID: 584 RVA: 0x0000CA39 File Offset: 0x0000AC39
	public Principal_WhistleApproach(Principal principal, NpcState previousState, Vector3 destination) : base(principal, previousState)
	{
		this.destination = destination;
	}

	// Token: 0x06000249 RID: 585 RVA: 0x0000CA4A File Offset: 0x0000AC4A
	public override void Initialize()
	{
		base.Initialize();
		base.ChangeNavigationState(new NavigationState_TargetPosition(this.npc, 63, this.destination));
	}

	// Token: 0x0600024A RID: 586 RVA: 0x0000CA6B File Offset: 0x0000AC6B
	public override void Resume()
	{
		base.Resume();
		this.npc.behaviorStateMachine.ChangeState(this.previousState);
	}

	// Token: 0x0600024B RID: 587 RVA: 0x0000CA89 File Offset: 0x0000AC89
	public override void DestinationEmpty()
	{
		base.DestinationEmpty();
		this.npc.behaviorStateMachine.ChangeState(this.previousState);
	}

	// Token: 0x0600024C RID: 588 RVA: 0x0000CAA7 File Offset: 0x0000ACA7
	public override void Exit()
	{
		base.Exit();
		this.principal.WhistleReached();
	}

	// Token: 0x04000254 RID: 596
	protected Vector3 destination;
}
