using System;

// Token: 0x02000020 RID: 32
public class Beans_ChewPrep : Beans_StateBase
{
	// Token: 0x060000C3 RID: 195 RVA: 0x00005A1A File Offset: 0x00003C1A
	public Beans_ChewPrep(Beans beans, PlayerManager player) : base(beans)
	{
		this.player = player;
	}

	// Token: 0x060000C4 RID: 196 RVA: 0x00005A2A File Offset: 0x00003C2A
	public override void Enter()
	{
		base.Enter();
		base.ChangeNavigationState(new NavigationState_TargetPosition(this.npc, 63, this.npc.Navigator.NextPoint));
	}

	// Token: 0x060000C5 RID: 197 RVA: 0x00005A58 File Offset: 0x00003C58
	public override void DestinationEmpty()
	{
		if (this.npc.looker.PlayerInSight() && !this.player.Tagged)
		{
			base.DestinationEmpty();
			this.beans.Chew();
			this.npc.behaviorStateMachine.ChangeState(new Beans_Chewing(this.beans, this.player, this.beans.ChewTime));
			return;
		}
		this.npc.behaviorStateMachine.ChangeState(new Beans_NewWandering(this.beans));
	}

	// Token: 0x040000D3 RID: 211
	protected PlayerManager player;
}
