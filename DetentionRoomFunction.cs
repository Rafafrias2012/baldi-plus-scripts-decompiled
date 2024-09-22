using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001A8 RID: 424
public class DetentionRoomFunction : RoomFunction
{
	// Token: 0x0600099D RID: 2461 RVA: 0x000335BC File Offset: 0x000317BC
	public override void Initialize(RoomController room)
	{
		base.Initialize(room);
		room.ec.offices.Add(room);
	}

	// Token: 0x0600099E RID: 2462 RVA: 0x000335D6 File Offset: 0x000317D6
	public void Activate(float time, EnvironmentController ec)
	{
		this.time = time;
		this.ec = ec;
		this.LockDoors();
		if (!this.active)
		{
			this.active = true;
			base.StartCoroutine(this.DetentionTimer());
		}
	}

	// Token: 0x0600099F RID: 2463 RVA: 0x00033608 File Offset: 0x00031808
	private void LockDoors()
	{
		foreach (Door door in this.room.doors)
		{
			door.Shut();
			door.LockTimed(this.time);
		}
	}

	// Token: 0x060009A0 RID: 2464 RVA: 0x0003366C File Offset: 0x0003186C
	private void LockNearestDoor(NPC npc)
	{
		foreach (Door door in this.room.doors)
		{
			if (door.aTile == this.ec.CellFromPosition(npc.transform.position) || door.bTile == this.ec.CellFromPosition(npc.transform.position))
			{
				door.Shut();
				door.LockTimed(this.time);
			}
		}
	}

	// Token: 0x060009A1 RID: 2465 RVA: 0x0003370C File Offset: 0x0003190C
	private IEnumerator DetentionTimer()
	{
		while (this.time > 0f)
		{
			this.time -= Time.deltaTime * this.ec.EnvironmentTimeScale * 1.25f;
			yield return null;
		}
		this.active = false;
		yield break;
	}

	// Token: 0x060009A2 RID: 2466 RVA: 0x0003371B File Offset: 0x0003191B
	public override void OnPlayerExit(PlayerManager player)
	{
		base.OnPlayerExit(player);
		if (this.active)
		{
			player.RuleBreak("Escaping", this.time, 0.25f);
		}
	}

	// Token: 0x060009A3 RID: 2467 RVA: 0x00033742 File Offset: 0x00031942
	public override void OnNpcEnter(NPC npc)
	{
		base.OnNpcEnter(npc);
		if (npc.Character == Character.Principal && this.active)
		{
			this.LockNearestDoor(npc);
		}
	}

	// Token: 0x060009A4 RID: 2468 RVA: 0x00033763 File Offset: 0x00031963
	public override void OnNpcExit(NPC npc)
	{
		base.OnNpcExit(npc);
		if (npc.Character == Character.Principal && this.active)
		{
			this.LockNearestDoor(npc);
		}
	}

	// Token: 0x04000AD4 RID: 2772
	private EnvironmentController ec;

	// Token: 0x04000AD5 RID: 2773
	private float time;

	// Token: 0x04000AD6 RID: 2774
	private bool active;
}
