using System;

// Token: 0x02000064 RID: 100
public class Principal_ChasingPlayer_AllKnowing : Principal_ChasingPlayer
{
	// Token: 0x0600023A RID: 570 RVA: 0x0000C7C2 File Offset: 0x0000A9C2
	public Principal_ChasingPlayer_AllKnowing(Principal principal, PlayerManager player) : base(principal, player)
	{
	}

	// Token: 0x0600023B RID: 571 RVA: 0x0000C7CC File Offset: 0x0000A9CC
	public override void Update()
	{
		base.Update();
		this.targetState.UpdatePosition(this.player.transform.position);
	}

	// Token: 0x0600023C RID: 572 RVA: 0x0000C7EF File Offset: 0x0000A9EF
	public override void DestinationEmpty()
	{
	}
}
