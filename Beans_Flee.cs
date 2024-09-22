using System;
using UnityEngine;

// Token: 0x02000023 RID: 35
public class Beans_Flee : Beans_StateBase
{
	// Token: 0x060000CB RID: 203 RVA: 0x00005BBD File Offset: 0x00003DBD
	public Beans_Flee(Beans beans, float time, DijkstraMap gumMap) : base(beans)
	{
		this.time = time;
		this.gumMap = gumMap;
	}

	// Token: 0x060000CC RID: 204 RVA: 0x00005BD4 File Offset: 0x00003DD4
	public override void Enter()
	{
		base.Enter();
		this.beans.Sprint();
		base.ChangeNavigationState(new NavigationState_WanderFlee(this.npc, 63, this.gumMap));
	}

	// Token: 0x060000CD RID: 205 RVA: 0x00005C00 File Offset: 0x00003E00
	public override void Update()
	{
		base.Update();
		this.time -= Time.deltaTime * this.npc.TimeScale;
		if (this.time <= 0f)
		{
			this.npc.behaviorStateMachine.ChangeState(new Beans_NewWandering(this.beans));
		}
	}

	// Token: 0x060000CE RID: 206 RVA: 0x00005C59 File Offset: 0x00003E59
	public override void Exit()
	{
		base.Exit();
		this.gumMap.Deactivate();
	}

	// Token: 0x040000D7 RID: 215
	protected DijkstraMap gumMap;

	// Token: 0x040000D8 RID: 216
	protected float time;
}
