using System;
using UnityEngine;

// Token: 0x020000C6 RID: 198
public class NavigationState_TargetPosition : NavigationState
{
	// Token: 0x060004A6 RID: 1190 RVA: 0x00017442 File Offset: 0x00015642
	public NavigationState_TargetPosition(NPC npc, int priority, Vector3 position, bool holdPriority) : base(npc, priority)
	{
		this.destination = position;
		this.holdPriority = holdPriority;
	}

	// Token: 0x060004A7 RID: 1191 RVA: 0x0001745B File Offset: 0x0001565B
	public NavigationState_TargetPosition(NPC npc, int priority, Vector3 position) : base(npc, priority)
	{
		this.destination = position;
	}

	// Token: 0x060004A8 RID: 1192 RVA: 0x0001746C File Offset: 0x0001566C
	public override void Enter()
	{
		base.Enter();
		if (!this.holdPriority)
		{
			this.priority = 0;
		}
		this.npc.Navigator.FindPath(this.npc.transform.position, this.destination);
	}

	// Token: 0x060004A9 RID: 1193 RVA: 0x000174A9 File Offset: 0x000156A9
	public override void UpdatePosition(Vector3 position)
	{
		base.UpdatePosition(position);
		if (this.active)
		{
			this.npc.Navigator.FindPath(this.npc.transform.position, position);
		}
	}

	// Token: 0x040004ED RID: 1261
	protected bool holdPriority;
}
