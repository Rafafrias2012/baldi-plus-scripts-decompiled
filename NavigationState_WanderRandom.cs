using System;

// Token: 0x020000C2 RID: 194
public class NavigationState_WanderRandom : NavigationState
{
	// Token: 0x06000499 RID: 1177 RVA: 0x00017304 File Offset: 0x00015504
	public NavigationState_WanderRandom(NPC npc, int priority) : base(npc, priority)
	{
	}

	// Token: 0x0600049A RID: 1178 RVA: 0x0001730E File Offset: 0x0001550E
	public override void Enter()
	{
		base.Enter();
		this.priority = 0;
		this.npc.Navigator.WanderRandom();
	}

	// Token: 0x0600049B RID: 1179 RVA: 0x0001732D File Offset: 0x0001552D
	public override void DestinationEmpty()
	{
		this.npc.Navigator.WanderRandom();
	}
}
