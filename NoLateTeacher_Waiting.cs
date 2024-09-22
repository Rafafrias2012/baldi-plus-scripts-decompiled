using System;
using UnityEngine;

// Token: 0x0200004D RID: 77
public class NoLateTeacher_Waiting : NoLateTeacher_StateBase
{
	// Token: 0x060001D0 RID: 464 RVA: 0x0000AB21 File Offset: 0x00008D21
	public NoLateTeacher_Waiting(NoLateTeacher pomp, PlayerManager player, float time) : base(pomp)
	{
		this.player = player;
		this.time = time;
	}

	// Token: 0x060001D1 RID: 465 RVA: 0x0000AB38 File Offset: 0x00008D38
	public override void Enter()
	{
		base.Enter();
		base.ChangeNavigationState(new NavigationState_TargetPosition(this.pomp, 63, this.pomp.GetClassPosition(), true));
	}

	// Token: 0x060001D2 RID: 466 RVA: 0x0000AB60 File Offset: 0x00008D60
	public override void Update()
	{
		base.Update();
		if (this.npc.ec.CellFromPosition(this.player.transform.position).room == this.pomp.TargetClassRoom)
		{
			this.currentNavigationState.priority = 0;
			this.pomp.InTime();
		}
		this.time -= Time.deltaTime * this.npc.TimeScale;
		this.pomp.UpdateTimer(this.time);
		if (this.time <= 0f)
		{
			this.currentNavigationState.priority = 0;
			this.pomp.TimeOut(this.player);
		}
	}

	// Token: 0x060001D3 RID: 467 RVA: 0x0000AC1A File Offset: 0x00008E1A
	public override void DestinationEmpty()
	{
		base.DestinationEmpty();
		this.pomp.WanderSpeed();
		this.currentNavigationState.UpdatePosition(this.pomp.GetClassPosition());
	}

	// Token: 0x040001E9 RID: 489
	protected PlayerManager player;

	// Token: 0x040001EA RID: 490
	protected float time;
}
