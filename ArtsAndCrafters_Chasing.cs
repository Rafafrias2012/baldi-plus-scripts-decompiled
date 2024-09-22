using System;
using UnityEngine;

// Token: 0x02000010 RID: 16
public class ArtsAndCrafters_Chasing : ArtsAndCrafters_StateBase
{
	// Token: 0x0600005A RID: 90 RVA: 0x0000462E File Offset: 0x0000282E
	public ArtsAndCrafters_Chasing(ArtsAndCrafters crafters, PlayerManager player) : base(crafters)
	{
		this.player = player;
	}

	// Token: 0x0600005B RID: 91 RVA: 0x0000463E File Offset: 0x0000283E
	public override void Enter()
	{
		base.Enter();
		this.targetState = new NavigationState_TargetPlayer(this.npc, 63, this.player.transform.position);
		base.ChangeNavigationState(this.targetState);
	}

	// Token: 0x0600005C RID: 92 RVA: 0x00004675 File Offset: 0x00002875
	public override void DestinationEmpty()
	{
		base.DestinationEmpty();
		base.ChangeNavigationState(new NavigationState_WanderRandom(this.npc, 0));
	}

	// Token: 0x0600005D RID: 93 RVA: 0x0000468F File Offset: 0x0000288F
	public override void PlayerInSight(PlayerManager player)
	{
		base.PlayerSighted(player);
		if (this.player == player)
		{
			base.ChangeNavigationState(this.targetState);
			this.targetState.UpdatePosition(player.transform.position);
		}
	}

	// Token: 0x0600005E RID: 94 RVA: 0x000046C8 File Offset: 0x000028C8
	public override void OnStateTriggerEnter(Collider other)
	{
		base.OnStateTriggerEnter(other);
		if (other.CompareTag("Player"))
		{
			this.crafters.state = new ArtsAndCrafters_Teleporting(this.crafters, this.player);
			this.npc.behaviorStateMachine.ChangeState(this.crafters.state);
		}
	}

	// Token: 0x0400008C RID: 140
	protected NavigationState_TargetPlayer targetState;

	// Token: 0x0400008D RID: 141
	protected PlayerManager player;
}
