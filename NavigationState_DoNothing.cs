using System;
using UnityEngine;

// Token: 0x020000C0 RID: 192
public class NavigationState_DoNothing : NavigationState
{
	// Token: 0x06000492 RID: 1170 RVA: 0x000172AE File Offset: 0x000154AE
	public NavigationState_DoNothing(NPC npc, int priority, bool holdPriority) : base(npc, priority)
	{
		this.holdPriority = holdPriority;
	}

	// Token: 0x06000493 RID: 1171 RVA: 0x000172BF File Offset: 0x000154BF
	public NavigationState_DoNothing(NPC npc, int priority) : base(npc, priority)
	{
	}

	// Token: 0x06000494 RID: 1172 RVA: 0x000172C9 File Offset: 0x000154C9
	public override void DestinationEmpty()
	{
	}

	// Token: 0x06000495 RID: 1173 RVA: 0x000172CB File Offset: 0x000154CB
	public override void Enter()
	{
		base.Enter();
		if (!this.holdPriority)
		{
			this.priority = 0;
		}
		this.npc.Navigator.ClearDestination();
	}

	// Token: 0x06000496 RID: 1174 RVA: 0x000172F2 File Offset: 0x000154F2
	public override void Exit()
	{
	}

	// Token: 0x06000497 RID: 1175 RVA: 0x000172F4 File Offset: 0x000154F4
	public override void UpdatePosition(Vector3 position)
	{
	}

	// Token: 0x040004EA RID: 1258
	private bool holdPriority;
}
