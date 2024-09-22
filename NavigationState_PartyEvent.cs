using System;

// Token: 0x02000077 RID: 119
public class NavigationState_PartyEvent : NavigationState
{
	// Token: 0x060002B2 RID: 690 RVA: 0x0000EDDB File Offset: 0x0000CFDB
	public NavigationState_PartyEvent(NPC npc, int priority, RoomController office) : base(npc, priority)
	{
		this.office = office;
	}

	// Token: 0x060002B3 RID: 691 RVA: 0x0000EDEC File Offset: 0x0000CFEC
	public override void Enter()
	{
		base.Enter();
		this.npc.Navigator.FindPath(this.office.RandomEventSafeCellNoGarbage().CenterWorldPosition);
	}

	// Token: 0x060002B4 RID: 692 RVA: 0x0000EE14 File Offset: 0x0000D014
	public override void DestinationEmpty()
	{
		this.npc.Navigator.FindPath(this.office.RandomEventSafeCellNoGarbage().CenterWorldPosition);
	}

	// Token: 0x060002B5 RID: 693 RVA: 0x0000EE36 File Offset: 0x0000D036
	public void End()
	{
		this.priority = 0;
		this.npc.behaviorStateMachine.RestoreNavigationState();
	}

	// Token: 0x040002CE RID: 718
	private RoomController office;
}
