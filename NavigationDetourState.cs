using System;
using UnityEngine;

// Token: 0x020000C8 RID: 200
public class NavigationDetourState : NavigationState
{
	// Token: 0x060004B0 RID: 1200 RVA: 0x000175FB File Offset: 0x000157FB
	public NavigationDetourState(NPC npc, Vector3 position, NavigationState parentState) : base(npc, 0)
	{
		this.destination = position;
		this.parentState = parentState;
	}

	// Token: 0x060004B1 RID: 1201 RVA: 0x00017613 File Offset: 0x00015813
	public virtual void Update()
	{
	}

	// Token: 0x060004B2 RID: 1202 RVA: 0x00017615 File Offset: 0x00015815
	public virtual bool StillNeeded()
	{
		return false;
	}

	// Token: 0x040004F1 RID: 1265
	protected NavigationState parentState;
}
