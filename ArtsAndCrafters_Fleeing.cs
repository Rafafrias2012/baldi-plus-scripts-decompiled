using System;

// Token: 0x0200000F RID: 15
public class ArtsAndCrafters_Fleeing : ArtsAndCrafters_StateBase
{
	// Token: 0x06000057 RID: 87 RVA: 0x00004596 File Offset: 0x00002796
	public ArtsAndCrafters_Fleeing(ArtsAndCrafters crafters, PlayerManager player) : base(crafters)
	{
		this.player = player;
	}

	// Token: 0x06000058 RID: 88 RVA: 0x000045A6 File Offset: 0x000027A6
	public override void Enter()
	{
		base.Enter();
		this.crafters.RunAway();
		base.ChangeNavigationState(new NavigationState_WanderFlee(this.crafters, 63, this.player.DijkstraMap));
	}

	// Token: 0x06000059 RID: 89 RVA: 0x000045D8 File Offset: 0x000027D8
	public override void Update()
	{
		base.Update();
		if (!this.npc.looker.IsVisible)
		{
			this.crafters.state = new ArtsAndCrafters_Waiting(this.crafters);
			this.npc.behaviorStateMachine.ChangeState(this.crafters.state);
		}
	}

	// Token: 0x0400008B RID: 139
	protected PlayerManager player;
}
