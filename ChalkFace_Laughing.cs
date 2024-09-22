using System;
using UnityEngine;

// Token: 0x0200002D RID: 45
public class ChalkFace_Laughing : ChalkFace_StateBase
{
	// Token: 0x0600010A RID: 266 RVA: 0x00007267 File Offset: 0x00005467
	public ChalkFace_Laughing(ChalkFace chalkles, Chalkboard chalkboard) : base(chalkles)
	{
		this.chalkles = chalkles;
		this.chalkboard = chalkboard;
	}

	// Token: 0x0600010B RID: 267 RVA: 0x0000727E File Offset: 0x0000547E
	public override void Enter()
	{
		base.Enter();
		this.chalkles.Activate(this.chalkboard.Room);
		this.time = this.chalkles.LockTime;
	}

	// Token: 0x0600010C RID: 268 RVA: 0x000072B0 File Offset: 0x000054B0
	public override void Update()
	{
		base.Update();
		this.time -= Time.deltaTime * this.npc.TimeScale;
		if (this.time > 0f)
		{
			this.chalkles.AdvanceLaughter(this.chalkboard.Room, 0f);
			return;
		}
		if (this.time > -5f)
		{
			this.chalkles.AdvanceLaughter(this.chalkboard.Room, this.chalkles.Acceleration);
			return;
		}
		this.chalkles.state = new ChalkFace_Idle(this.chalkles);
		this.npc.behaviorStateMachine.ChangeState(this.chalkles.state);
	}

	// Token: 0x0600010D RID: 269 RVA: 0x0000736A File Offset: 0x0000556A
	public override void Teleport(Chalkboard chalkboard)
	{
	}

	// Token: 0x04000120 RID: 288
	private Chalkboard chalkboard;

	// Token: 0x04000121 RID: 289
	private float time;
}
