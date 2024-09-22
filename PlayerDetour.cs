using System;

// Token: 0x020000CB RID: 203
public class PlayerDetour
{
	// Token: 0x060004E0 RID: 1248 RVA: 0x00019665 File Offset: 0x00017865
	public PlayerDetour(int priority)
	{
		this.priority = priority;
	}

	// Token: 0x04000527 RID: 1319
	public PlayerDetour.CheckIfNeeded UseIfNeeded;

	// Token: 0x04000528 RID: 1320
	public int priority;

	// Token: 0x02000354 RID: 852
	// (Invoke) Token: 0x06001B82 RID: 7042
	public delegate bool CheckIfNeeded(NPC npc, NavigationState parentState, out NavigationDetourState state);
}
