using System;
using UnityEngine;

// Token: 0x02000019 RID: 25
public class Baldi_Praise : Baldi_SubState
{
	// Token: 0x0600009E RID: 158 RVA: 0x000053D4 File Offset: 0x000035D4
	public Baldi_Praise(NPC npc, Baldi baldi, NpcState previousState, float time) : base(npc, baldi, previousState)
	{
		this.time = time;
	}

	// Token: 0x0600009F RID: 159 RVA: 0x000053E7 File Offset: 0x000035E7
	public override void Enter()
	{
		this.baldi.PraiseAnimation();
	}

	// Token: 0x060000A0 RID: 160 RVA: 0x000053F4 File Offset: 0x000035F4
	public override void Update()
	{
		base.Update();
		this.time -= Time.deltaTime * this.npc.TimeScale;
		if (this.time <= 0f)
		{
			this.npc.behaviorStateMachine.ChangeState(this.previousState);
		}
	}

	// Token: 0x040000B8 RID: 184
	private float time;
}
