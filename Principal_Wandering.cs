using System;
using UnityEngine;

// Token: 0x02000062 RID: 98
public class Principal_Wandering : Principal_StateBase
{
	// Token: 0x0600022E RID: 558 RVA: 0x0000C485 File Offset: 0x0000A685
	public Principal_Wandering(Principal principal) : base(principal)
	{
	}

	// Token: 0x0600022F RID: 559 RVA: 0x0000C499 File Offset: 0x0000A699
	public override void Enter()
	{
		base.Enter();
		base.ChangeNavigationState(new NavigationState_WanderRounds(this.npc, 0));
	}

	// Token: 0x06000230 RID: 560 RVA: 0x0000C4B4 File Offset: 0x0000A6B4
	public override void Update()
	{
		base.Update();
		foreach (NPC npc in this.principal.ec.Npcs)
		{
			if (npc.Disobeying)
			{
				bool flag;
				this.principal.looker.Raycast(npc.transform, Mathf.Min(new float[]
				{
					(this.principal.transform.position - npc.transform.position).magnitude + npc.Navigator.Velocity.magnitude,
					this.principal.looker.distance,
					npc.ec.MaxRaycast
				}), out flag);
				if (flag)
				{
					this.principal.behaviorStateMachine.ChangeState(new Principal_ChasingNpc(this.principal, npc));
					this.principal.Scold(npc.BrokenRule);
					break;
				}
				break;
			}
		}
		this.whislteTime -= Time.deltaTime * this.npc.TimeScale;
		if (this.whislteTime <= 0f)
		{
			this.principal.WhistleChance();
			this.whislteTime = 1f;
		}
	}

	// Token: 0x06000231 RID: 561 RVA: 0x0000C618 File Offset: 0x0000A818
	public override void PlayerInSight(PlayerManager player)
	{
		base.PlayerInSight(player);
		this.principal.ObservePlayer(player);
	}

	// Token: 0x06000232 RID: 562 RVA: 0x0000C62D File Offset: 0x0000A82D
	public override void PlayerLost(PlayerManager player)
	{
		base.PlayerLost(player);
		this.principal.LoseTrackOfPlayer(player);
	}

	// Token: 0x06000233 RID: 563 RVA: 0x0000C644 File Offset: 0x0000A844
	public override void DoorHit(StandardDoor door)
	{
		if (!door.IsOpen)
		{
			if (this.npc.ec.CellFromPosition(this.npc.transform.position) == door.aTile)
			{
				if (door.bTile.room.category == RoomCategory.Faculty)
				{
					this.principal.FacultyDoorHit(door, door.bTile);
					return;
				}
			}
			else if (door.aTile.room.category == RoomCategory.Faculty)
			{
				this.principal.FacultyDoorHit(door, door.aTile);
				return;
			}
		}
		door.OpenTimedWithKey(door.DefaultTime, false);
	}

	// Token: 0x06000234 RID: 564 RVA: 0x0000C6DA File Offset: 0x0000A8DA
	public override void DestinationEmpty()
	{
		base.DestinationEmpty();
		base.ChangeNavigationState(new NavigationState_WanderRounds(this.principal, 0));
	}

	// Token: 0x0400024D RID: 589
	private float whislteTime = 1f;
}
