using System;

// Token: 0x020000C4 RID: 196
public class NavigationState_WanderFlee : NavigationState
{
	// Token: 0x0600049F RID: 1183 RVA: 0x0001737A File Offset: 0x0001557A
	public NavigationState_WanderFlee(NPC npc, int priority, DijkstraMap dijkstraMap) : base(npc, priority)
	{
		this.dijkstraMap = dijkstraMap;
	}

	// Token: 0x060004A0 RID: 1184 RVA: 0x0001738B File Offset: 0x0001558B
	public override void Enter()
	{
		base.Enter();
		this.priority = 0;
		this.npc.Navigator.ClearCurrentDirs();
		this.npc.Navigator.WanderFlee(this.dijkstraMap);
	}

	// Token: 0x060004A1 RID: 1185 RVA: 0x000173C0 File Offset: 0x000155C0
	public override void DestinationEmpty()
	{
		this.npc.Navigator.WanderFlee(this.dijkstraMap);
	}

	// Token: 0x040004EB RID: 1259
	protected DijkstraMap dijkstraMap;
}
