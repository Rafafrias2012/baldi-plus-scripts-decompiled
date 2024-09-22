using System;

// Token: 0x02000036 RID: 54
public class DrReflex_Noticing : DrReflex_StateBase
{
	// Token: 0x06000158 RID: 344 RVA: 0x00008B55 File Offset: 0x00006D55
	public DrReflex_Noticing(DrReflex drReflex, PlayerManager player) : base(drReflex)
	{
		this.player = player;
	}

	// Token: 0x06000159 RID: 345 RVA: 0x00008B65 File Offset: 0x00006D65
	public override void Enter()
	{
		base.Enter();
		base.ChangeNavigationState(new NavigationState_TargetPlayer(this.npc, 63, this.player.transform.position));
	}

	// Token: 0x0600015A RID: 346 RVA: 0x00008B90 File Offset: 0x00006D90
	public override void Update()
	{
		if (this.noticedPlayer)
		{
			if (!this.drReflex.Turning)
			{
				if (this.npc.looker.PlayerInSight(this.player) && !this.player.Tagged)
				{
					this.npc.behaviorStateMachine.ChangeState(new DrReflex_Chasing(this.drReflex, this.player));
					return;
				}
				this.npc.behaviorStateMachine.ChangeState(new DrReflex_Wandering(this.drReflex, 0f));
				return;
			}
		}
		else if (!this.drReflex.FacingPlayer(this.player))
		{
			this.drReflex.PauseAndTurn(this.player);
			this.noticedPlayer = true;
		}
	}

	// Token: 0x04000178 RID: 376
	protected PlayerManager player;

	// Token: 0x04000179 RID: 377
	private bool noticedPlayer;
}
