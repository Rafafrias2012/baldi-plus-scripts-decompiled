using System;
using UnityEngine;

// Token: 0x0200003F RID: 63
public class GottaSweep_SweepingTime : GottaSweep_StateBase
{
	// Token: 0x06000186 RID: 390 RVA: 0x00009463 File Offset: 0x00007663
	public GottaSweep_SweepingTime(NPC npc, GottaSweep gottaSweep) : base(npc, gottaSweep)
	{
	}

	// Token: 0x06000187 RID: 391 RVA: 0x0000946D File Offset: 0x0000766D
	public override void Enter()
	{
		base.Enter();
		base.ChangeNavigationState(new NavigationState_WanderRandom(this.npc, 0));
		this.sweepTime = this.gottaSweep.GetRandomSweepTime;
		this.gottaSweep.StartSweeping();
	}

	// Token: 0x06000188 RID: 392 RVA: 0x000094A4 File Offset: 0x000076A4
	public override void Update()
	{
		base.Update();
		this.sweepTime -= Time.deltaTime * this.npc.TimeScale;
		if (this.sweepTime <= 0f)
		{
			this.npc.behaviorStateMachine.ChangeState(new GottaSweep_Returning(this.npc, this.gottaSweep));
		}
	}

	// Token: 0x04000194 RID: 404
	private float sweepTime;
}
