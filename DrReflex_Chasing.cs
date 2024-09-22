using System;
using UnityEngine;

// Token: 0x02000037 RID: 55
public class DrReflex_Chasing : DrReflex_StateBase
{
	// Token: 0x0600015B RID: 347 RVA: 0x00008C48 File Offset: 0x00006E48
	public DrReflex_Chasing(DrReflex drReflex, PlayerManager player) : base(drReflex)
	{
		this.player = player;
	}

	// Token: 0x0600015C RID: 348 RVA: 0x00008C58 File Offset: 0x00006E58
	public override void Enter()
	{
		base.Enter();
		this.drReflex.StartCharge();
		this.targetState = new NavigationState_TargetPlayer(this.npc, 63, this.player.transform.position, true);
		base.ChangeNavigationState(this.targetState);
	}

	// Token: 0x0600015D RID: 349 RVA: 0x00008CA6 File Offset: 0x00006EA6
	public override void Update()
	{
		base.Update();
		this.drReflex.IncreasePitch();
	}

	// Token: 0x0600015E RID: 350 RVA: 0x00008CBC File Offset: 0x00006EBC
	public override void DestinationEmpty()
	{
		base.DestinationEmpty();
		this.drReflex.AudioManager.FlushQueue(true);
		this.targetState.priority = 0;
		this.npc.behaviorStateMachine.ChangeState(new DrReflex_Wandering(this.drReflex, 0f));
	}

	// Token: 0x0600015F RID: 351 RVA: 0x00008D0C File Offset: 0x00006F0C
	public override void PlayerInSight(PlayerManager player)
	{
		base.PlayerSighted(player);
		if (this.player == player && !player.Tagged)
		{
			base.ChangeNavigationState(this.targetState);
			this.targetState.UpdatePosition(player.transform.position);
		}
	}

	// Token: 0x06000160 RID: 352 RVA: 0x00008D58 File Offset: 0x00006F58
	public override void OnStateTriggerStay(Collider other)
	{
		base.OnStateTriggerEnter(other);
		if (other.CompareTag("Player"))
		{
			PlayerManager component = other.GetComponent<PlayerManager>();
			if (!component.Tagged)
			{
				this.npc.behaviorStateMachine.ChangeState(new DrReflex_Testing(this.drReflex, component));
			}
		}
	}

	// Token: 0x06000161 RID: 353 RVA: 0x00008DA4 File Offset: 0x00006FA4
	public override void Exit()
	{
		base.Exit();
		this.targetState.priority = 0;
	}

	// Token: 0x0400017A RID: 378
	protected NavigationState_TargetPlayer targetState;

	// Token: 0x0400017B RID: 379
	protected PlayerManager player;
}
