using System;
using UnityEngine;

// Token: 0x0200005D RID: 93
public class Playtime_Wandering : Playtime_StateBase
{
	// Token: 0x06000213 RID: 531 RVA: 0x0000BC87 File Offset: 0x00009E87
	public Playtime_Wandering(NPC npc, Playtime playtime) : base(npc, playtime)
	{
	}

	// Token: 0x06000214 RID: 532 RVA: 0x0000BC9C File Offset: 0x00009E9C
	public override void Enter()
	{
		base.Enter();
		this.npc.looker.Blink();
		if (!this.npc.Navigator.HasDestination)
		{
			base.ChangeNavigationState(new NavigationState_WanderRandom(this.npc, 0));
		}
	}

	// Token: 0x06000215 RID: 533 RVA: 0x0000BCD8 File Offset: 0x00009ED8
	public override void Update()
	{
		base.Update();
		this.calloutTime -= Time.deltaTime * this.npc.TimeScale;
		if (this.calloutTime <= 0f)
		{
			this.playtime.CalloutChance();
			this.calloutTime = 3f;
		}
	}

	// Token: 0x06000216 RID: 534 RVA: 0x0000BD2C File Offset: 0x00009F2C
	public override void OnStateTriggerEnter(Collider other)
	{
		base.OnStateTriggerEnter(other);
		if (other.CompareTag("Player"))
		{
			PlayerManager component = other.GetComponent<PlayerManager>();
			if (!component.Tagged)
			{
				this.playtime.StartJumprope(component);
			}
		}
	}

	// Token: 0x06000217 RID: 535 RVA: 0x0000BD68 File Offset: 0x00009F68
	public override void DestinationEmpty()
	{
		base.DestinationEmpty();
		base.ChangeNavigationState(new NavigationState_WanderRandom(this.npc, 0));
	}

	// Token: 0x06000218 RID: 536 RVA: 0x0000BD82 File Offset: 0x00009F82
	public override void PlayerSighted(PlayerManager player)
	{
		base.PlayerSighted(player);
		if (!player.Tagged)
		{
			this.playtime.StartPersuingPlayer(player);
		}
	}

	// Token: 0x06000219 RID: 537 RVA: 0x0000BD9F File Offset: 0x00009F9F
	public override void PlayerInSight(PlayerManager player)
	{
		base.PlayerInSight(player);
		if (!player.Tagged)
		{
			this.playtime.PersuePlayer(player);
		}
	}

	// Token: 0x0600021A RID: 538 RVA: 0x0000BDBC File Offset: 0x00009FBC
	public override void PlayerLost(PlayerManager player)
	{
		base.PlayerLost(player);
		this.playtime.PlayerTurnAround(player);
	}

	// Token: 0x0400022C RID: 556
	private float calloutTime = 3f;
}
