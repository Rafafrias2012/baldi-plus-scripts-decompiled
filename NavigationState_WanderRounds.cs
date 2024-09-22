using System;

// Token: 0x020000C3 RID: 195
public class NavigationState_WanderRounds : NavigationState
{
	// Token: 0x0600049C RID: 1180 RVA: 0x0001733F File Offset: 0x0001553F
	public NavigationState_WanderRounds(NPC npc, int priority) : base(npc, priority)
	{
	}

	// Token: 0x0600049D RID: 1181 RVA: 0x00017349 File Offset: 0x00015549
	public override void Enter()
	{
		base.Enter();
		this.priority = 0;
		this.npc.Navigator.WanderRounds();
	}

	// Token: 0x0600049E RID: 1182 RVA: 0x00017368 File Offset: 0x00015568
	public override void DestinationEmpty()
	{
		this.npc.Navigator.WanderRounds();
	}
}
