using System;
using UnityEngine;

// Token: 0x02000059 RID: 89
public class TestNPC_Crazy : NpcState
{
	// Token: 0x060001F9 RID: 505 RVA: 0x0000B422 File Offset: 0x00009622
	public TestNPC_Crazy(NPC npc, TestNPC testNpc) : base(npc)
	{
		this.testNpc = testNpc;
	}

	// Token: 0x060001FA RID: 506 RVA: 0x0000B43D File Offset: 0x0000963D
	public override void Enter()
	{
		base.Enter();
		base.ChangeNavigationState(new NavigationState_WanderRandom(this.npc, 0));
		Debug.Log("CRAZY TIME");
	}

	// Token: 0x060001FB RID: 507 RVA: 0x0000B464 File Offset: 0x00009664
	public override void Update()
	{
		base.Update();
		this.npc.Navigator.SetSpeed(this.npc.Navigator.Speed + Time.deltaTime * 100f);
		this.crazyTime -= Time.deltaTime;
		if (this.crazyTime <= 0f)
		{
			this.npc.behaviorStateMachine.ChangeState(new TestNPC_DefaultState(this.npc, this.testNpc));
		}
	}

	// Token: 0x04000204 RID: 516
	public TestNPC testNpc;

	// Token: 0x04000205 RID: 517
	private float crazyTime = 30f;
}
