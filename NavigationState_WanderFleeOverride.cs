using System;

// Token: 0x020000C5 RID: 197
public class NavigationState_WanderFleeOverride : NavigationState
{
	// Token: 0x060004A2 RID: 1186 RVA: 0x000173D8 File Offset: 0x000155D8
	public NavigationState_WanderFleeOverride(NPC npc, int priority, DijkstraMap dijkstraMap) : base(npc, priority)
	{
		this.dijkstraMap = dijkstraMap;
	}

	// Token: 0x060004A3 RID: 1187 RVA: 0x000173E9 File Offset: 0x000155E9
	public override void Enter()
	{
		this.npc.Navigator.ClearCurrentDirs();
		this.npc.Navigator.WanderFlee(this.dijkstraMap);
	}

	// Token: 0x060004A4 RID: 1188 RVA: 0x00017411 File Offset: 0x00015611
	public override void DestinationEmpty()
	{
		this.npc.Navigator.WanderFlee(this.dijkstraMap);
	}

	// Token: 0x060004A5 RID: 1189 RVA: 0x00017429 File Offset: 0x00015629
	public void End()
	{
		this.priority = 0;
		this.npc.behaviorStateMachine.RestoreNavigationState();
	}

	// Token: 0x040004EC RID: 1260
	protected DijkstraMap dijkstraMap;
}
