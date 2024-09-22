using System;
using UnityEngine;

// Token: 0x0200003E RID: 62
public class GottaSweep_Wait : GottaSweep_StateBase
{
	// Token: 0x06000183 RID: 387 RVA: 0x000093C4 File Offset: 0x000075C4
	public GottaSweep_Wait(NPC npc, GottaSweep gottaSweep) : base(npc, gottaSweep)
	{
	}

	// Token: 0x06000184 RID: 388 RVA: 0x000093CE File Offset: 0x000075CE
	public override void Enter()
	{
		base.Enter();
		base.ChangeNavigationState(new NavigationState_DoNothing(this.npc, 0));
		this.waitTime = this.gottaSweep.GetRandomDelay;
		this.gottaSweep.StopSweeping();
	}

	// Token: 0x06000185 RID: 389 RVA: 0x00009404 File Offset: 0x00007604
	public override void Update()
	{
		base.Update();
		this.waitTime -= Time.deltaTime * this.npc.TimeScale;
		if (this.waitTime <= 0f)
		{
			this.npc.behaviorStateMachine.ChangeState(new GottaSweep_SweepingTime(this.npc, this.gottaSweep));
		}
	}

	// Token: 0x04000193 RID: 403
	private float waitTime;
}
