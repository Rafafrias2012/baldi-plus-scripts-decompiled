using System;
using UnityEngine;

// Token: 0x0200004A RID: 74
public class NoLateTeacher_Wander : NoLateTeacher_StateBase
{
	// Token: 0x060001C2 RID: 450 RVA: 0x0000A8F2 File Offset: 0x00008AF2
	public NoLateTeacher_Wander(NoLateTeacher pomp) : base(pomp)
	{
	}

	// Token: 0x060001C3 RID: 451 RVA: 0x0000A8FB File Offset: 0x00008AFB
	public override void Enter()
	{
		base.Enter();
		this.npc.looker.Blink();
		this.pomp.WanderSpeed();
	}

	// Token: 0x060001C4 RID: 452 RVA: 0x0000A920 File Offset: 0x00008B20
	public override void PlayerSighted(PlayerManager player)
	{
		base.PlayerSighted(player);
		if (!player.Tagged)
		{
			this.playerTargetNavigationState = new NavigationState_TargetPlayer(this.npc, 63, player.transform.position);
			base.ChangeNavigationState(this.playerTargetNavigationState);
			this.pomp.CallPlayer();
		}
	}

	// Token: 0x060001C5 RID: 453 RVA: 0x0000A971 File Offset: 0x00008B71
	public override void PlayerInSight(PlayerManager player)
	{
		base.PlayerInSight(player);
		if (!player.Tagged)
		{
			base.ChangeNavigationState(this.playerTargetNavigationState);
			this.playerTargetNavigationState.UpdatePosition(player.transform.position);
			this.pomp.ChaseSpeed();
		}
	}

	// Token: 0x060001C6 RID: 454 RVA: 0x0000A9AF File Offset: 0x00008BAF
	public override void DestinationEmpty()
	{
		base.DestinationEmpty();
		base.ChangeNavigationState(new NavigationState_WanderRounds(this.npc, 0));
		this.pomp.WanderSpeed();
	}

	// Token: 0x060001C7 RID: 455 RVA: 0x0000A9D4 File Offset: 0x00008BD4
	public override void OnStateTriggerStay(Collider other)
	{
		base.OnStateTriggerStay(other);
		if (other.CompareTag("Player") && !other.GetComponent<PlayerManager>().Tagged)
		{
			this.pomp.PlayerCaught(other.GetComponent<PlayerManager>());
		}
	}

	// Token: 0x040001E4 RID: 484
	protected NavigationState playerTargetNavigationState;
}
