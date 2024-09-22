using System;
using UnityEngine;

// Token: 0x02000066 RID: 102
public class Principal_Detention : Principal_StateBase
{
	// Token: 0x06000244 RID: 580 RVA: 0x0000C9A5 File Offset: 0x0000ABA5
	public Principal_Detention(Principal principal, float time) : base(principal)
	{
		this.time = time;
	}

	// Token: 0x06000245 RID: 581 RVA: 0x0000C9B5 File Offset: 0x0000ABB5
	public override void Enter()
	{
		base.Enter();
		base.ChangeNavigationState(new NavigationState_DoNothing(this.npc, 0));
	}

	// Token: 0x06000246 RID: 582 RVA: 0x0000C9D0 File Offset: 0x0000ABD0
	public override void Update()
	{
		base.Update();
		this.time -= Time.deltaTime * this.npc.TimeScale;
		if (this.time <= 0f)
		{
			this.npc.behaviorStateMachine.ChangeState(new Principal_Wandering(this.principal));
		}
	}

	// Token: 0x04000252 RID: 594
	protected float time;
}
