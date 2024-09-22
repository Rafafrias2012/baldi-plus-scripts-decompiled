using System;
using UnityEngine;

// Token: 0x02000058 RID: 88
public class TestNPC_PursuePlayer : NpcState
{
	// Token: 0x060001F4 RID: 500 RVA: 0x0000B351 File Offset: 0x00009551
	public TestNPC_PursuePlayer(NPC npc, TestNPC testNpc, PlayerManager player) : base(npc)
	{
		this.testNpc = testNpc;
		this.player = player;
	}

	// Token: 0x060001F5 RID: 501 RVA: 0x0000B368 File Offset: 0x00009568
	public override void Enter()
	{
		base.Enter();
		Debug.Log("TestNPC entering pursuit state.");
		base.ChangeNavigationState(new NavigationState_TargetPlayer(this.npc, 63, this.player.transform.position));
	}

	// Token: 0x060001F6 RID: 502 RVA: 0x0000B39D File Offset: 0x0000959D
	public override void PlayerInSight(PlayerManager player)
	{
		base.PlayerInSight(player);
		this.npc.behaviorStateMachine.CurrentNavigationState.UpdatePosition(player.transform.position);
	}

	// Token: 0x060001F7 RID: 503 RVA: 0x0000B3C6 File Offset: 0x000095C6
	public override void DestinationEmpty()
	{
		base.DestinationEmpty();
		if (!this.npc.looker.PlayerInSight())
		{
			this.npc.behaviorStateMachine.ChangeState(new TestNPC_DefaultState(this.npc, this.testNpc));
		}
	}

	// Token: 0x060001F8 RID: 504 RVA: 0x0000B401 File Offset: 0x00009601
	public override void OnStateTriggerEnter(Collider other)
	{
		base.OnStateTriggerEnter(other);
		if (other.CompareTag("Player"))
		{
			this.testNpc.GoCrazy();
		}
	}

	// Token: 0x04000202 RID: 514
	public TestNPC testNpc;

	// Token: 0x04000203 RID: 515
	public PlayerManager player;
}
