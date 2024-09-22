using System;

// Token: 0x02000050 RID: 80
public class NoLateTeacher_Triggered : NoLateTeacher_StateBase
{
	// Token: 0x060001D9 RID: 473 RVA: 0x0000AF47 File Offset: 0x00009147
	public NoLateTeacher_Triggered(NoLateTeacher pomp, PlayerManager player) : base(pomp)
	{
		this.player = player;
	}

	// Token: 0x060001DA RID: 474 RVA: 0x0000AF5E File Offset: 0x0000915E
	public override void Enter()
	{
		base.Enter();
		base.ChangeNavigationState(new NavigationState_TargetPosition(this.npc, 127, this.pomp.TargetClassRoom.RandomEntitySafeCellNoGarbage().FloorWorldPosition));
	}

	// Token: 0x060001DB RID: 475 RVA: 0x0000AF90 File Offset: 0x00009190
	public override void Update()
	{
		base.Update();
		if (this.draggable)
		{
			this.draggable = this.pomp.Drag(this.player);
		}
		else
		{
			this.pomp.ReturnToPlayerCheck(this.player);
		}
		if (this.npc.ec.CellFromPosition(this.player.transform.position).room == this.pomp.TargetClassRoom)
		{
			this.pomp.Teach(this.player);
		}
	}

	// Token: 0x060001DC RID: 476 RVA: 0x0000B01D File Offset: 0x0000921D
	public override void DestinationEmpty()
	{
		base.DestinationEmpty();
		this.npc.behaviorStateMachine.ChangeState(new NoLateTeacher_Returning(this.pomp, this.player));
	}

	// Token: 0x040001F2 RID: 498
	private PlayerManager player;

	// Token: 0x040001F3 RID: 499
	private bool draggable = true;
}
