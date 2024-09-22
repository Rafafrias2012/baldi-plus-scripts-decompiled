using System;
using UnityEngine;

// Token: 0x0200001E RID: 30
public class Beans_Wandering : Beans_StateBase
{
	// Token: 0x060000BB RID: 187 RVA: 0x00005930 File Offset: 0x00003B30
	public Beans_Wandering(Beans beans) : base(beans)
	{
	}

	// Token: 0x060000BC RID: 188 RVA: 0x00005939 File Offset: 0x00003B39
	public override void Enter()
	{
		base.Enter();
		this.sprintTimer = this.beans.SprintWait;
	}

	// Token: 0x060000BD RID: 189 RVA: 0x00005952 File Offset: 0x00003B52
	public override void Update()
	{
		base.Update();
		this.sprintTimer -= Time.deltaTime * this.npc.TimeScale;
		if (this.sprintTimer <= 0f)
		{
			this.SprintTimeExpired();
		}
	}

	// Token: 0x060000BE RID: 190 RVA: 0x0000598B File Offset: 0x00003B8B
	public virtual void SprintTimeExpired()
	{
		this.beans.Sprint();
		this.npc.behaviorStateMachine.ChangeState(new Beans_Sprinting(this.beans));
	}

	// Token: 0x060000BF RID: 191 RVA: 0x000059B3 File Offset: 0x00003BB3
	public override void PlayerSighted(PlayerManager player)
	{
		base.PlayerSighted(player);
		if (!player.Tagged)
		{
			this.beans.ChewPrep(player);
		}
	}

	// Token: 0x040000D2 RID: 210
	protected float sprintTimer;
}
