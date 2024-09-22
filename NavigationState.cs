using System;
using UnityEngine;

// Token: 0x020000BF RID: 191
public class NavigationState
{
	// Token: 0x0600048D RID: 1165 RVA: 0x00017266 File Offset: 0x00015466
	public NavigationState(NPC owner, int priority)
	{
		this.npc = owner;
		this.priority = priority;
	}

	// Token: 0x0600048E RID: 1166 RVA: 0x0001727C File Offset: 0x0001547C
	public virtual void Enter()
	{
		this.active = true;
	}

	// Token: 0x0600048F RID: 1167 RVA: 0x00017285 File Offset: 0x00015485
	public virtual void UpdatePosition(Vector3 position)
	{
		this.destination = position;
	}

	// Token: 0x06000490 RID: 1168 RVA: 0x0001728E File Offset: 0x0001548E
	public virtual void DestinationEmpty()
	{
		this.npc.behaviorStateMachine.CurrentState.DestinationEmpty();
	}

	// Token: 0x06000491 RID: 1169 RVA: 0x000172A5 File Offset: 0x000154A5
	public virtual void Exit()
	{
		this.active = false;
	}

	// Token: 0x040004E6 RID: 1254
	public NPC npc;

	// Token: 0x040004E7 RID: 1255
	public Vector3 destination;

	// Token: 0x040004E8 RID: 1256
	public int priority;

	// Token: 0x040004E9 RID: 1257
	protected bool active;
}
