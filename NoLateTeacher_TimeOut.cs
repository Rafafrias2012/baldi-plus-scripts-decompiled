using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200004E RID: 78
public class NoLateTeacher_TimeOut : NoLateTeacher_StateBase
{
	// Token: 0x060001D4 RID: 468 RVA: 0x0000AC43 File Offset: 0x00008E43
	public NoLateTeacher_TimeOut(NoLateTeacher pomp, PlayerManager player) : base(pomp)
	{
		this.player = player;
	}

	// Token: 0x060001D5 RID: 469 RVA: 0x0000AC5E File Offset: 0x00008E5E
	public override void Enter()
	{
		base.Enter();
		this.pomp.AngrySpeed();
	}

	// Token: 0x060001D6 RID: 470 RVA: 0x0000AC74 File Offset: 0x00008E74
	public override void Update()
	{
		base.Update();
		this._room = this.npc.ec.CellFromPosition(this.player.transform.position).room;
		if (this._room.type != RoomType.Hall && this._room.doors.Count > 0)
		{
			if (this._room != this.targetedRoom)
			{
				this._potentialDoors.Clear();
				for (int i = 0; i < this._room.doors.Count; i++)
				{
					if (this._room.doors[i].bTile.room.type == RoomType.Hall && this.npc.ec.CheckPath(this.npc.transform.position, this._room.doors[i].bTile.CenterWorldPosition, PathType.Nav) && this.npc.ec.CheckPath(this.npc.transform.position, this._room.doors[i].aTile.CenterWorldPosition, PathType.Nav))
					{
						this._potentialDoors.Add(this._room.doors[i]);
					}
				}
				if (this._potentialDoors.Count > 0)
				{
					this.targetedRoom = this._room;
					this.targetedDoor = this._potentialDoors[Random.Range(0, this._potentialDoors.Count)];
					base.ChangeNavigationState(new NavigationState_TargetPosition(this.pomp, 127, this.targetedDoor.bTile.CenterWorldPosition, true));
				}
				else
				{
					this.targetedRoom = null;
					this.targetedDoor = null;
				}
			}
		}
		else
		{
			this.targetedRoom = null;
			this.targetedDoor = null;
		}
		if (this.player.pc.clickedThisFrame != null && this.targetedDoor != null && !this.targetedDoor.locked && this.player.pc.clickedThisFrame == this.targetedDoor.gameObject && !this.npc.Navigator.HasDestination)
		{
			this.pomp.Attack(this.player);
		}
	}

	// Token: 0x040001EB RID: 491
	private PlayerManager player;

	// Token: 0x040001EC RID: 492
	private RoomController targetedRoom;

	// Token: 0x040001ED RID: 493
	private RoomController _room;

	// Token: 0x040001EE RID: 494
	private List<Door> _potentialDoors = new List<Door>();

	// Token: 0x040001EF RID: 495
	private Door targetedDoor;
}
