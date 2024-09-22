using System;
using UnityEngine;

// Token: 0x02000045 RID: 69
public class LookAtGuy_Active : LookAtGuy_BaseState
{
	// Token: 0x0600019E RID: 414 RVA: 0x00009A6D File Offset: 0x00007C6D
	public LookAtGuy_Active(LookAtGuy theTest) : base(theTest)
	{
	}

	// Token: 0x0600019F RID: 415 RVA: 0x00009A76 File Offset: 0x00007C76
	public override void OnStateTriggerEnter(Collider other)
	{
		base.OnStateTriggerEnter(other);
		if (other.CompareTag("Player") && !other.GetComponent<PlayerManager>().Tagged)
		{
			this.theTest.Blind();
		}
	}

	// Token: 0x060001A0 RID: 416 RVA: 0x00009AA4 File Offset: 0x00007CA4
	public override void PlayerInSight(PlayerManager player)
	{
		base.PlayerInSight(player);
		if (!this.npc.looker.IsVisible && !this.charging && !player.Tagged)
		{
			this.charging = true;
			this.theTest.ChargePlayer(player);
			return;
		}
		if (this.npc.looker.IsVisible && this.charging)
		{
			this.charging = false;
			this.theTest.Stop();
		}
	}

	// Token: 0x040001A9 RID: 425
	protected bool charging;
}
