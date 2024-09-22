using System;
using UnityEngine;

// Token: 0x02000057 RID: 87
public class TestNPC_DefaultState : NpcState
{
	// Token: 0x060001F0 RID: 496 RVA: 0x0000B28C File Offset: 0x0000948C
	public TestNPC_DefaultState(NPC npc, TestNPC testNpc) : base(npc)
	{
		this.testNpc = testNpc;
	}

	// Token: 0x060001F1 RID: 497 RVA: 0x0000B29C File Offset: 0x0000949C
	public override void Enter()
	{
		base.Enter();
		Debug.Log("TestNPC entering default state.");
		base.ChangeNavigationState(new NavigationState_WanderRandom(this.npc, 0));
		this.npc.looker.Blink();
		this.npc.Navigator.SetSpeed(15f);
		this.npc.Navigator.maxSpeed = 15f;
	}

	// Token: 0x060001F2 RID: 498 RVA: 0x0000B305 File Offset: 0x00009505
	public override void PlayerSighted(PlayerManager player)
	{
		base.PlayerSighted(player);
		this.npc.behaviorStateMachine.ChangeState(new TestNPC_PursuePlayer(this.npc, this.testNpc, player));
	}

	// Token: 0x060001F3 RID: 499 RVA: 0x0000B330 File Offset: 0x00009530
	public override void OnStateTriggerEnter(Collider other)
	{
		base.OnStateTriggerEnter(other);
		if (other.CompareTag("Player"))
		{
			this.testNpc.GoCrazy();
		}
	}

	// Token: 0x04000201 RID: 513
	public TestNPC testNpc;
}
