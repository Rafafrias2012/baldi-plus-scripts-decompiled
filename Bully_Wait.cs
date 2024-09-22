using System;
using UnityEngine;

// Token: 0x02000027 RID: 39
public class Bully_Wait : Bully_StateBase
{
	// Token: 0x060000EE RID: 238 RVA: 0x00006914 File Offset: 0x00004B14
	public Bully_Wait(NPC npc, Bully bully, float time) : base(npc, bully)
	{
		this.time = time;
	}

	// Token: 0x060000EF RID: 239 RVA: 0x00006925 File Offset: 0x00004B25
	public override void Update()
	{
		base.Update();
		this.time -= Time.deltaTime * this.npc.TimeScale;
		if (this.time <= 0f)
		{
			this.bully.Spawn();
		}
	}

	// Token: 0x04000105 RID: 261
	private float time;
}
