using System;
using UnityEngine;

// Token: 0x02000009 RID: 9
public class FirstPrize_Stunned : FirstPrize_StateBase
{
	// Token: 0x06000032 RID: 50 RVA: 0x000038EB File Offset: 0x00001AEB
	public FirstPrize_Stunned(FirstPrize firstPrize, float time) : base(firstPrize)
	{
		this.time = time;
	}

	// Token: 0x06000033 RID: 51 RVA: 0x000038FB File Offset: 0x00001AFB
	public override void Enter()
	{
		base.Enter();
		base.ChangeNavigationState(new NavigationState_Disabled(this.npc));
	}

	// Token: 0x06000034 RID: 52 RVA: 0x00003914 File Offset: 0x00001B14
	public override void Update()
	{
		base.Update();
		this.time -= Time.deltaTime * this.npc.TimeScale;
		if (this.time <= 0f)
		{
			this.npc.behaviorStateMachine.ChangeState(new FirstPrize_Active(this.firstPrize));
		}
		this.npc.Navigator.maxSpeed = 0f;
		this.npc.Navigator.SetSpeed(0f);
		Vector3 eulerAngles = this.npc.transform.eulerAngles;
		eulerAngles.y += this.firstPrize.turnSpeed * 10f * Time.deltaTime * this.npc.TimeScale;
		this.npc.transform.eulerAngles = eulerAngles;
	}

	// Token: 0x06000035 RID: 53 RVA: 0x000039E7 File Offset: 0x00001BE7
	public override void Exit()
	{
		base.Exit();
		this.currentNavigationState.priority = 0;
	}

	// Token: 0x0400005F RID: 95
	private float time;
}
