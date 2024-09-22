using System;
using UnityEngine;

// Token: 0x02000032 RID: 50
public class Cumulo_Blowing : Cumulo_StateBase
{
	// Token: 0x06000120 RID: 288 RVA: 0x00007A88 File Offset: 0x00005C88
	public Cumulo_Blowing(Cumulo cumulo, float time) : base(cumulo)
	{
		this.time = time;
	}

	// Token: 0x06000121 RID: 289 RVA: 0x00007A98 File Offset: 0x00005C98
	public override void Enter()
	{
		base.Enter();
		this.cumulo.Blow();
		this.startPosition = this.cumulo.transform.position;
	}

	// Token: 0x06000122 RID: 290 RVA: 0x00007AC4 File Offset: 0x00005CC4
	public override void Update()
	{
		base.Update();
		this.time -= Time.deltaTime * this.npc.TimeScale;
		if (this.time <= 0f || Vector3.Distance(this.startPosition, this.cumulo.transform.position) >= 0.1f)
		{
			this.npc.behaviorStateMachine.ChangeState(new Cumulo_Moving(this.cumulo));
		}
	}

	// Token: 0x06000123 RID: 291 RVA: 0x00007B3F File Offset: 0x00005D3F
	public override void Exit()
	{
		base.Exit();
		this.cumulo.StopBlowing();
	}

	// Token: 0x04000134 RID: 308
	private Vector3 startPosition;

	// Token: 0x04000135 RID: 309
	private float time;
}
