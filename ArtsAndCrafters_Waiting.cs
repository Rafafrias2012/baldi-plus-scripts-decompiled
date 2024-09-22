using System;

// Token: 0x0200000C RID: 12
public class ArtsAndCrafters_Waiting : ArtsAndCrafters_StateBase
{
	// Token: 0x0600004D RID: 77 RVA: 0x00004292 File Offset: 0x00002492
	public ArtsAndCrafters_Waiting(ArtsAndCrafters crafters) : base(crafters)
	{
	}

	// Token: 0x0600004E RID: 78 RVA: 0x0000429B File Offset: 0x0000249B
	public override void Enter()
	{
		base.Enter();
		this.crafters.Hide(true);
		base.ChangeNavigationState(new NavigationState_DoNothing(this.crafters, 0));
	}

	// Token: 0x0600004F RID: 79 RVA: 0x000042C4 File Offset: 0x000024C4
	public override void SpawnAt(IntVector2 position)
	{
		base.SpawnAt(position);
		this.crafters.Teleport(position);
		this.crafters.state = new ArtsAndCrafters_Ready(this.crafters);
		this.npc.behaviorStateMachine.ChangeState(this.crafters.state);
	}
}
