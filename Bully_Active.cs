using System;
using UnityEngine;

// Token: 0x02000028 RID: 40
public class Bully_Active : Bully_StateBase
{
	// Token: 0x060000F0 RID: 240 RVA: 0x00006963 File Offset: 0x00004B63
	public Bully_Active(NPC npc, Bully bully, float time) : base(npc, bully)
	{
		this.time = time;
	}

	// Token: 0x060000F1 RID: 241 RVA: 0x00006974 File Offset: 0x00004B74
	public override void Update()
	{
		base.Update();
		this.time -= Time.deltaTime * this.npc.TimeScale;
		if (this.time <= 0f)
		{
			this.bully.Hide();
			this.bully.ExpressBoredom();
		}
	}

	// Token: 0x060000F2 RID: 242 RVA: 0x000069C8 File Offset: 0x00004BC8
	public override void OnStateTriggerEnter(Collider other)
	{
		if (!other.CompareTag("Player"))
		{
			if (other.CompareTag("NPC"))
			{
				Entity component = other.GetComponent<Entity>();
				if (component != null)
				{
					this.bully.Push(component);
				}
			}
			return;
		}
		PlayerManager component2 = other.GetComponent<PlayerManager>();
		if (!component2.Tagged)
		{
			this.bully.StealItem(component2);
			return;
		}
		this.bully.Hide();
	}

	// Token: 0x060000F3 RID: 243 RVA: 0x00006A33 File Offset: 0x00004C33
	public override void PlayerSighted(PlayerManager player)
	{
		base.PlayerSighted(player);
		if (!this.spoken)
		{
			this.bully.Taunt();
			this.spoken = true;
		}
	}

	// Token: 0x060000F4 RID: 244 RVA: 0x00006A56 File Offset: 0x00004C56
	public override void PlayerInSight(PlayerManager player)
	{
		base.PlayerInSight(player);
		this.bully.SetGuilty();
	}

	// Token: 0x04000106 RID: 262
	private float time;

	// Token: 0x04000107 RID: 263
	private bool spoken;
}
