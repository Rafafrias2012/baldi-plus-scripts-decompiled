using System;
using UnityEngine;

// Token: 0x020000BD RID: 189
public class NpcState
{
	// Token: 0x17000049 RID: 73
	// (get) Token: 0x06000472 RID: 1138 RVA: 0x0001713F File Offset: 0x0001533F
	public NPC Npc
	{
		get
		{
			return this.npc;
		}
	}

	// Token: 0x1700004A RID: 74
	// (get) Token: 0x06000473 RID: 1139 RVA: 0x00017147 File Offset: 0x00015347
	// (set) Token: 0x06000474 RID: 1140 RVA: 0x0001714F File Offset: 0x0001534F
	public NavigationState CurrentNavigationState
	{
		get
		{
			return this.currentNavigationState;
		}
		set
		{
			this.currentNavigationState = value;
		}
	}

	// Token: 0x06000475 RID: 1141 RVA: 0x00017158 File Offset: 0x00015358
	public NpcState(NPC npc)
	{
		this.npc = npc;
	}

	// Token: 0x06000476 RID: 1142 RVA: 0x00017167 File Offset: 0x00015367
	public virtual void Initialize()
	{
		this.initialized = true;
	}

	// Token: 0x06000477 RID: 1143 RVA: 0x00017170 File Offset: 0x00015370
	public virtual void Enter()
	{
		if (!this.initialized)
		{
			this.Initialize();
			return;
		}
		this.Resume();
	}

	// Token: 0x06000478 RID: 1144 RVA: 0x00017187 File Offset: 0x00015387
	public virtual void Resume()
	{
	}

	// Token: 0x06000479 RID: 1145 RVA: 0x00017189 File Offset: 0x00015389
	public virtual void Update()
	{
	}

	// Token: 0x0600047A RID: 1146 RVA: 0x0001718B File Offset: 0x0001538B
	public virtual void Exit()
	{
	}

	// Token: 0x0600047B RID: 1147 RVA: 0x0001718D File Offset: 0x0001538D
	public void ChangeNavigationState(NavigationState state)
	{
		this.npc.navigationStateMachine.ChangeState(state);
		this.currentNavigationState = state;
	}

	// Token: 0x0600047C RID: 1148 RVA: 0x000171A7 File Offset: 0x000153A7
	public void RestoreNavigationState()
	{
		this.npc.navigationStateMachine.ChangeState(this.currentNavigationState);
	}

	// Token: 0x0600047D RID: 1149 RVA: 0x000171BF File Offset: 0x000153BF
	public virtual void DestinationEmpty()
	{
	}

	// Token: 0x0600047E RID: 1150 RVA: 0x000171C1 File Offset: 0x000153C1
	public virtual void Hear(Vector3 position, int value)
	{
	}

	// Token: 0x0600047F RID: 1151 RVA: 0x000171C3 File Offset: 0x000153C3
	public virtual void PlayerInSight(PlayerManager player)
	{
	}

	// Token: 0x06000480 RID: 1152 RVA: 0x000171C5 File Offset: 0x000153C5
	public virtual void PlayerSighted(PlayerManager player)
	{
	}

	// Token: 0x06000481 RID: 1153 RVA: 0x000171C7 File Offset: 0x000153C7
	public virtual void PlayerLost(PlayerManager player)
	{
	}

	// Token: 0x06000482 RID: 1154 RVA: 0x000171C9 File Offset: 0x000153C9
	public virtual void Sighted()
	{
	}

	// Token: 0x06000483 RID: 1155 RVA: 0x000171CB File Offset: 0x000153CB
	public virtual void InPlayerSight(PlayerManager player)
	{
	}

	// Token: 0x06000484 RID: 1156 RVA: 0x000171CD File Offset: 0x000153CD
	public virtual void Unsighted()
	{
	}

	// Token: 0x06000485 RID: 1157 RVA: 0x000171CF File Offset: 0x000153CF
	public virtual void MadeNavigationDecision()
	{
	}

	// Token: 0x06000486 RID: 1158 RVA: 0x000171D1 File Offset: 0x000153D1
	public virtual void OnStateTriggerEnter(Collider other)
	{
	}

	// Token: 0x06000487 RID: 1159 RVA: 0x000171D3 File Offset: 0x000153D3
	public virtual void OnStateTriggerStay(Collider other)
	{
	}

	// Token: 0x06000488 RID: 1160 RVA: 0x000171D5 File Offset: 0x000153D5
	public virtual void OnStateTriggerExit(Collider other)
	{
	}

	// Token: 0x06000489 RID: 1161 RVA: 0x000171D7 File Offset: 0x000153D7
	public virtual void DoorHit(StandardDoor door)
	{
		if (!door.locked)
		{
			door.OpenTimed(door.DefaultTime, false);
		}
	}

	// Token: 0x040004E2 RID: 1250
	protected NPC npc;

	// Token: 0x040004E3 RID: 1251
	protected NavigationState currentNavigationState;

	// Token: 0x040004E4 RID: 1252
	protected bool initialized;
}
