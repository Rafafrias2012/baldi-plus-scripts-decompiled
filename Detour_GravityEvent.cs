using System;
using UnityEngine;

// Token: 0x0200006F RID: 111
public class Detour_GravityEvent : NavigationDetourState
{
	// Token: 0x06000280 RID: 640 RVA: 0x0000E114 File Offset: 0x0000C314
	public Detour_GravityEvent(GravityEvent gravityEvent, NPC npc, Vector3 position, NavigationState parentState) : base(npc, position, parentState)
	{
		this.gravityEvent = gravityEvent;
	}

	// Token: 0x06000281 RID: 641 RVA: 0x0000E127 File Offset: 0x0000C327
	public override void DestinationEmpty()
	{
		this.parentState.Enter();
	}

	// Token: 0x06000282 RID: 642 RVA: 0x0000E134 File Offset: 0x0000C334
	public override void Update()
	{
		base.Update();
		if (this.gravityEvent.NpcOrientationMatchesPlayer(this.npc))
		{
			this.parentState.DestinationEmpty();
		}
	}

	// Token: 0x06000283 RID: 643 RVA: 0x0000E15A File Offset: 0x0000C35A
	public override bool StillNeeded()
	{
		return !this.gravityEvent.NpcOrientationMatchesPlayer(this.npc);
	}

	// Token: 0x06000284 RID: 644 RVA: 0x0000E170 File Offset: 0x0000C370
	public override void Exit()
	{
		base.Exit();
		this.gravityEvent.RemoveDetour(this);
	}

	// Token: 0x0400029F RID: 671
	public GravityEvent gravityEvent;
}
