using System;
using UnityEngine;

// Token: 0x0200000E RID: 14
public class ArtsAndCrafters_Stalking : ArtsAndCrafters_StateBase
{
	// Token: 0x06000052 RID: 82 RVA: 0x000043FD File Offset: 0x000025FD
	public ArtsAndCrafters_Stalking(ArtsAndCrafters crafters) : base(crafters)
	{
	}

	// Token: 0x06000053 RID: 83 RVA: 0x00004411 File Offset: 0x00002611
	public override void Enter()
	{
		base.Enter();
		this.crafters.Hide(false);
	}

	// Token: 0x06000054 RID: 84 RVA: 0x00004428 File Offset: 0x00002628
	public override void Update()
	{
		base.Update();
		if (!this.npc.looker.PlayerInSight())
		{
			this.timeLeftToRespawn -= Time.deltaTime * this.npc.TimeScale;
			if (this.timeLeftToRespawn <= 0f)
			{
				this.crafters.state = new ArtsAndCrafters_Waiting(this.crafters);
				this.npc.behaviorStateMachine.ChangeState(this.crafters.state);
				return;
			}
		}
		else
		{
			this.timeLeftToRespawn = 10f;
		}
	}

	// Token: 0x06000055 RID: 85 RVA: 0x000044B8 File Offset: 0x000026B8
	public override void PlayerInSight(PlayerManager player)
	{
		base.PlayerInSight(player);
		this.timePlayerInSight += Time.deltaTime * this.npc.TimeScale;
		if (this.timePlayerInSight >= this.crafters.RunTime)
		{
			this.crafters.state = new ArtsAndCrafters_Fleeing(this.crafters, player);
			this.npc.behaviorStateMachine.ChangeState(this.crafters.state);
		}
	}

	// Token: 0x06000056 RID: 86 RVA: 0x00004530 File Offset: 0x00002730
	public override void InPlayerSight(PlayerManager player)
	{
		base.InPlayerSight(player);
		this.timeInPlayerSight += Time.deltaTime * this.npc.TimeScale;
		if (this.timeInPlayerSight >= this.crafters.AngryTime && this.crafters.Jealous && !player.Tagged)
		{
			this.crafters.GetAngry(player);
		}
	}

	// Token: 0x04000088 RID: 136
	private float timePlayerInSight;

	// Token: 0x04000089 RID: 137
	private float timeInPlayerSight;

	// Token: 0x0400008A RID: 138
	private float timeLeftToRespawn = 10f;
}
