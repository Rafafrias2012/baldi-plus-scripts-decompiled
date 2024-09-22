using System;
using UnityEngine;

// Token: 0x0200003A RID: 58
public class DrReflex_Hunting : DrReflex_StateBase
{
	// Token: 0x06000168 RID: 360 RVA: 0x00008E78 File Offset: 0x00007078
	public DrReflex_Hunting(DrReflex drReflex, PlayerManager player) : base(drReflex)
	{
		this.player = player;
	}

	// Token: 0x06000169 RID: 361 RVA: 0x00008E94 File Offset: 0x00007094
	public override void Enter()
	{
		base.Enter();
		this.targetState = new NavigationState_TargetPlayer(this.npc, 63, this.drReflex.LastSightedPlayerLocation);
		base.ChangeNavigationState(this.targetState);
		this.drReflex.GetHammer();
		this.SetNextRandomHammerTime();
		this.npc.Navigator.SetRoomAvoidance(false);
		this.npc.Navigator.passableObstacles.Add(PassableObstacle.Bully);
	}

	// Token: 0x0600016A RID: 362 RVA: 0x00008F09 File Offset: 0x00007109
	private void SetNextRandomHammerTime()
	{
		this.timeToNextRandomHammer = Random.Range(0.2f, 2f);
	}

	// Token: 0x0600016B RID: 363 RVA: 0x00008F20 File Offset: 0x00007120
	public override void Update()
	{
		base.Update();
		this.drReflex.HammerCheck(this.player);
		this.timeToNextRandomHammer -= Time.deltaTime * this.npc.TimeScale;
		if (this.talking && !this.drReflex.AudioManager.QueuedAudioIsPlaying)
		{
			this.talking = false;
			this.drReflex.Animator.SetBool("Chill", false);
		}
		if (this.timeToNextRandomHammer <= 0f)
		{
			this.drReflex.Hammer(null);
			this.SetNextRandomHammerTime();
		}
		this.calloutTime -= Time.deltaTime * this.npc.TimeScale;
		if (this.calloutTime <= 0f)
		{
			this.calloutTime = 3f;
			if (this.drReflex.CalloutChance(false))
			{
				this.talking = true;
			}
		}
	}

	// Token: 0x0600016C RID: 364 RVA: 0x00009003 File Offset: 0x00007203
	public override void PlayerInSight(PlayerManager player)
	{
		base.PlayerInSight(player);
		if (this.player == player)
		{
			base.ChangeNavigationState(this.targetState);
			this.targetState.UpdatePosition(player.transform.position);
		}
	}

	// Token: 0x0600016D RID: 365 RVA: 0x0000903C File Offset: 0x0000723C
	public override void Hear(Vector3 position, int value)
	{
		base.Hear(position, value);
		if (!this.npc.looker.PlayerInSight())
		{
			base.ChangeNavigationState(this.targetState);
			this.targetState.UpdatePosition(position);
		}
	}

	// Token: 0x0600016E RID: 366 RVA: 0x00009070 File Offset: 0x00007270
	public override void DestinationEmpty()
	{
		base.DestinationEmpty();
		base.ChangeNavigationState(new NavigationState_WanderRounds(this.npc, 0));
	}

	// Token: 0x0600016F RID: 367 RVA: 0x0000908A File Offset: 0x0000728A
	public override void Exit()
	{
		base.Exit();
		this.npc.Navigator.SetRoomAvoidance(true);
		this.npc.Navigator.passableObstacles.Remove(PassableObstacle.Bully);
	}

	// Token: 0x0400017E RID: 382
	protected NavigationState_TargetPlayer targetState;

	// Token: 0x0400017F RID: 383
	protected PlayerManager player;

	// Token: 0x04000180 RID: 384
	private float timeToNextRandomHammer;

	// Token: 0x04000181 RID: 385
	private float calloutTime = 3f;

	// Token: 0x04000182 RID: 386
	private bool talking;
}
