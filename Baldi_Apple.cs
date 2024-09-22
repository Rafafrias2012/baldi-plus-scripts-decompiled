using System;
using UnityEngine;

// Token: 0x02000018 RID: 24
public class Baldi_Apple : Baldi_SubState
{
	// Token: 0x0600009A RID: 154 RVA: 0x00005346 File Offset: 0x00003546
	public Baldi_Apple(NPC npc, Baldi baldi, NpcState previousState) : base(npc, baldi, previousState)
	{
	}

	// Token: 0x0600009B RID: 155 RVA: 0x00005351 File Offset: 0x00003551
	public override void Initialize()
	{
		base.Initialize();
		this.time = this.baldi.appleTime;
	}

	// Token: 0x0600009C RID: 156 RVA: 0x0000536A File Offset: 0x0000356A
	public override void Resume()
	{
		base.Resume();
		this.baldi.ResumeApple();
	}

	// Token: 0x0600009D RID: 157 RVA: 0x00005380 File Offset: 0x00003580
	public override void Update()
	{
		base.Update();
		this.time -= Time.deltaTime * this.npc.TimeScale;
		if (this.time <= 0f)
		{
			this.npc.behaviorStateMachine.ChangeState(this.previousState);
		}
	}

	// Token: 0x040000B7 RID: 183
	public float time;
}
