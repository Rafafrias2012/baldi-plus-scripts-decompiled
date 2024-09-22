using System;
using UnityEngine;

// Token: 0x02000011 RID: 17
public class ArtsAndCrafters_Teleporting : ArtsAndCrafters_StateBase
{
	// Token: 0x0600005F RID: 95 RVA: 0x00004720 File Offset: 0x00002920
	public ArtsAndCrafters_Teleporting(ArtsAndCrafters crafters, PlayerManager player) : base(crafters)
	{
		this.player = player;
	}

	// Token: 0x06000060 RID: 96 RVA: 0x00004730 File Offset: 0x00002930
	public override void Enter()
	{
		base.Enter();
		this.crafters.Attack(this.player);
		this.speed = this.crafters.AttackSpinSpeed;
		this.accel = this.crafters.AttackSpinAccel;
		this.echoDistance = 0f;
		this.currentAngle = this.player.transform.forward * -1f;
		this.time = this.crafters.AttackTime;
	}

	// Token: 0x06000061 RID: 97 RVA: 0x000047B4 File Offset: 0x000029B4
	public override void Update()
	{
		base.Update();
		this.currentAngle = Quaternion.AngleAxis(this.speed * Time.deltaTime * this.npc.TimeScale, Vector3.up) * this.currentAngle;
		this.crafters.UpdateTeleportAnimation(this.player, this.currentAngle, this.echoDistance);
		this.speed += this.accel * Time.deltaTime * this.npc.TimeScale;
		this.echoDistance = Mathf.Min(this.echoDistance + this.crafters.EchoIncrease * Time.deltaTime * this.npc.TimeScale, this.crafters.MaxEchoDistance);
		this.time -= Time.deltaTime * this.npc.TimeScale;
		if (this.time <= 0f)
		{
			this.crafters.TeleportPlayer(this.player);
		}
	}

	// Token: 0x0400008E RID: 142
	protected PlayerManager player;

	// Token: 0x0400008F RID: 143
	private float speed;

	// Token: 0x04000090 RID: 144
	private float accel;

	// Token: 0x04000091 RID: 145
	private float echoDistance;

	// Token: 0x04000092 RID: 146
	private Vector3 currentAngle;

	// Token: 0x04000093 RID: 147
	private float time;
}
