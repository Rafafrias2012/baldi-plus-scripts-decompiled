using System;
using UnityEngine;

// Token: 0x02000063 RID: 99
public class Principal_ChasingPlayer : Principal_StateBase
{
	// Token: 0x06000235 RID: 565 RVA: 0x0000C6F4 File Offset: 0x0000A8F4
	public Principal_ChasingPlayer(Principal principal, PlayerManager player) : base(principal)
	{
		this.player = player;
	}

	// Token: 0x06000236 RID: 566 RVA: 0x0000C704 File Offset: 0x0000A904
	public override void Enter()
	{
		base.Enter();
		this.targetState = new NavigationState_TargetPlayer(this.npc, 63, this.player.transform.position);
		base.ChangeNavigationState(this.targetState);
	}

	// Token: 0x06000237 RID: 567 RVA: 0x0000C73B File Offset: 0x0000A93B
	public override void DestinationEmpty()
	{
		base.DestinationEmpty();
		base.ChangeNavigationState(new NavigationState_WanderRounds(this.npc, 0));
	}

	// Token: 0x06000238 RID: 568 RVA: 0x0000C755 File Offset: 0x0000A955
	public override void PlayerInSight(PlayerManager player)
	{
		base.PlayerInSight(player);
		if (this.player == player)
		{
			base.ChangeNavigationState(this.targetState);
			this.targetState.UpdatePosition(player.transform.position);
		}
	}

	// Token: 0x06000239 RID: 569 RVA: 0x0000C78E File Offset: 0x0000A98E
	public override void OnStateTriggerStay(Collider other)
	{
		base.OnStateTriggerEnter(other);
		if (other.CompareTag("Player") && other.GetComponent<PlayerManager>() == this.player)
		{
			this.principal.SendToDetention();
		}
	}

	// Token: 0x0400024E RID: 590
	protected NavigationState_TargetPlayer targetState;

	// Token: 0x0400024F RID: 591
	protected PlayerManager player;
}
