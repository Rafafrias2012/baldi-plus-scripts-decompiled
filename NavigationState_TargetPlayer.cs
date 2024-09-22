using System;
using UnityEngine;

// Token: 0x020000C7 RID: 199
public class NavigationState_TargetPlayer : NavigationState
{
	// Token: 0x060004AA RID: 1194 RVA: 0x000174DB File Offset: 0x000156DB
	public NavigationState_TargetPlayer(NPC npc, int priority, Vector3 position, bool holdPriority) : base(npc, priority)
	{
		this.destination = position;
		this.holdPriority = holdPriority;
	}

	// Token: 0x060004AB RID: 1195 RVA: 0x000174F4 File Offset: 0x000156F4
	public NavigationState_TargetPlayer(NPC npc, int priority, Vector3 position) : base(npc, priority)
	{
		this.destination = position;
	}

	// Token: 0x060004AC RID: 1196 RVA: 0x00017508 File Offset: 0x00015708
	protected void FindPathWithDetours()
	{
		if (this.takingDetour)
		{
			this.currentDetour.Exit();
			this.currentDetour = null;
			this.takingDetour = false;
		}
		if (this.npc.HasDetour && this.npc.ActivateDetour(this, out this.currentDetour))
		{
			this.takingDetour = true;
			this.npc.Navigator.FindPath(this.currentDetour.destination);
			return;
		}
		this.takingDetour = false;
		this.npc.Navigator.FindPath(this.destination);
	}

	// Token: 0x060004AD RID: 1197 RVA: 0x00017597 File Offset: 0x00015797
	public override void Enter()
	{
		base.Enter();
		if (!this.holdPriority)
		{
			this.priority = 0;
		}
		this.FindPathWithDetours();
	}

	// Token: 0x060004AE RID: 1198 RVA: 0x000175B4 File Offset: 0x000157B4
	public override void UpdatePosition(Vector3 position)
	{
		base.UpdatePosition(position);
		if (this.active)
		{
			this.FindPathWithDetours();
		}
	}

	// Token: 0x060004AF RID: 1199 RVA: 0x000175CB File Offset: 0x000157CB
	public override void DestinationEmpty()
	{
		if (!this.takingDetour)
		{
			base.DestinationEmpty();
			return;
		}
		if (this.currentDetour.StillNeeded())
		{
			this.currentDetour.DestinationEmpty();
			return;
		}
		this.FindPathWithDetours();
	}

	// Token: 0x040004EE RID: 1262
	protected bool holdPriority;

	// Token: 0x040004EF RID: 1263
	protected bool takingDetour;

	// Token: 0x040004F0 RID: 1264
	protected NavigationDetourState currentDetour;
}
