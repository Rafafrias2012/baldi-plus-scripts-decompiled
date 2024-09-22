using System;
using UnityEngine;

// Token: 0x02000065 RID: 101
public class Principal_ChasingNpc : Principal_StateBase
{
	// Token: 0x0600023D RID: 573 RVA: 0x0000C7F1 File Offset: 0x0000A9F1
	public Principal_ChasingNpc(Principal principal, NPC targetedNpc) : base(principal)
	{
		this.targetedNpc = targetedNpc;
		this.targetState = new NavigationState_TargetPosition(this.npc, 63, targetedNpc.transform.position);
	}

	// Token: 0x0600023E RID: 574 RVA: 0x0000C81F File Offset: 0x0000AA1F
	public override void Enter()
	{
		base.Enter();
		if (this.targetedNpc.Character == Character.Bully)
		{
			this.npc.Navigator.passableObstacles.Add(PassableObstacle.Bully);
		}
		base.ChangeNavigationState(this.targetState);
	}

	// Token: 0x0600023F RID: 575 RVA: 0x0000C857 File Offset: 0x0000AA57
	public override void Resume()
	{
		base.Resume();
		this.principal.behaviorStateMachine.ChangeState(new Principal_Wandering(this.principal));
	}

	// Token: 0x06000240 RID: 576 RVA: 0x0000C87A File Offset: 0x0000AA7A
	public override void Update()
	{
		base.Update();
		this.targetState.UpdatePosition(this.targetedNpc.transform.position);
	}

	// Token: 0x06000241 RID: 577 RVA: 0x0000C8A0 File Offset: 0x0000AAA0
	public override void OnStateTriggerStay(Collider other)
	{
		base.OnStateTriggerEnter(other);
		if (other.transform == this.targetedNpc.transform)
		{
			int index = Random.Range(0, this.principal.ec.offices.Count);
			this.targetedNpc.transform.position = this.principal.ec.offices[index].RandomEntitySafeCellNoGarbage().FloorWorldPosition + Vector3.up * 5f;
			this.targetedNpc.SentToDetention();
			this.principal.behaviorStateMachine.ChangeState(new Principal_Wandering(this.principal));
		}
	}

	// Token: 0x06000242 RID: 578 RVA: 0x0000C955 File Offset: 0x0000AB55
	public override void DestinationEmpty()
	{
		base.DestinationEmpty();
		this.principal.behaviorStateMachine.ChangeState(new Principal_Wandering(this.principal));
	}

	// Token: 0x06000243 RID: 579 RVA: 0x0000C978 File Offset: 0x0000AB78
	public override void Exit()
	{
		base.Exit();
		if (this.targetedNpc.Character == Character.Bully)
		{
			this.npc.Navigator.passableObstacles.Remove(PassableObstacle.Bully);
		}
	}

	// Token: 0x04000250 RID: 592
	protected NavigationState_TargetPosition targetState;

	// Token: 0x04000251 RID: 593
	protected NPC targetedNpc;
}
