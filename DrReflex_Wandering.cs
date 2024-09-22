using System;
using UnityEngine;

// Token: 0x02000035 RID: 53
public class DrReflex_Wandering : DrReflex_StateBase
{
	// Token: 0x06000152 RID: 338 RVA: 0x00008938 File Offset: 0x00006B38
	public DrReflex_Wandering(DrReflex drReflex, float cooldown) : base(drReflex)
	{
		this.cooldown = cooldown;
	}

	// Token: 0x06000153 RID: 339 RVA: 0x00008953 File Offset: 0x00006B53
	public override void Enter()
	{
		base.Enter();
		base.ChangeNavigationState(new NavigationState_WanderRandom(this.npc, 0));
		this.drReflex.Reset();
		this.roomWanderCheckTime = this.drReflex.RoomWanderCycle;
	}

	// Token: 0x06000154 RID: 340 RVA: 0x00008989 File Offset: 0x00006B89
	public override void MadeNavigationDecision()
	{
		base.MadeNavigationDecision();
		this.drReflex.PauseAndTurn();
		this.stopToTurn = true;
		this.npc.Navigator.SetRoomAvoidance(true);
	}

	// Token: 0x06000155 RID: 341 RVA: 0x000089B4 File Offset: 0x00006BB4
	public override void Update()
	{
		if (this.cooldown > 0f)
		{
			this.cooldown -= Time.deltaTime * this.npc.TimeScale;
		}
		if (!this.drReflex.FacingNextPoint())
		{
			base.Update();
		}
		this.roomWanderCheckTime -= Time.deltaTime * this.npc.TimeScale;
		if (this.roomWanderCheckTime <= 0f)
		{
			this.drReflex.UpdateRoomWandering();
			this.roomWanderCheckTime = this.drReflex.RoomWanderCycle;
		}
		this.calloutTime -= Time.deltaTime * this.npc.TimeScale;
		if (this.calloutTime <= 0f)
		{
			this.drReflex.CalloutChance(true);
			this.calloutTime = 3f;
		}
	}

	// Token: 0x06000156 RID: 342 RVA: 0x00008A8C File Offset: 0x00006C8C
	public override void PlayerSighted(PlayerManager player)
	{
		base.PlayerSighted(player);
		if (this.cooldown <= 0f && !player.Tagged)
		{
			if (this.drReflex.FacingPlayer(player))
			{
				this.npc.behaviorStateMachine.ChangeState(new DrReflex_Chasing(this.drReflex, player));
				return;
			}
			this.npc.behaviorStateMachine.ChangeState(new DrReflex_Noticing(this.drReflex, player));
		}
	}

	// Token: 0x06000157 RID: 343 RVA: 0x00008AFC File Offset: 0x00006CFC
	public override void OnStateTriggerStay(Collider other)
	{
		base.OnStateTriggerEnter(other);
		if (other.CompareTag("Player") && this.cooldown <= 0f)
		{
			PlayerManager component = other.GetComponent<PlayerManager>();
			if (!component.Tagged)
			{
				this.npc.behaviorStateMachine.ChangeState(new DrReflex_Noticing(this.drReflex, component));
			}
		}
	}

	// Token: 0x04000174 RID: 372
	protected float cooldown;

	// Token: 0x04000175 RID: 373
	protected float roomWanderCheckTime;

	// Token: 0x04000176 RID: 374
	private float calloutTime = 3f;

	// Token: 0x04000177 RID: 375
	private bool stopToTurn;
}
