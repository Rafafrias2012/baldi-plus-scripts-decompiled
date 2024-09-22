using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200002E RID: 46
public class Chalkboard : RoomFunction
{
	// Token: 0x0600010E RID: 270 RVA: 0x0000736C File Offset: 0x0000556C
	private void Update()
	{
		for (int i = 0; i < this.players.Count; i++)
		{
			if (this.players[i].Tagged)
			{
				this.chalkFace.state.Cancel();
			}
		}
	}

	// Token: 0x0600010F RID: 271 RVA: 0x000073B2 File Offset: 0x000055B2
	public override void OnPlayerEnter(PlayerManager player)
	{
		base.OnPlayerEnter(player);
		this.players.Add(player);
		if (!player.Tagged)
		{
			this.chalkFace.state.Teleport(this);
		}
	}

	// Token: 0x06000110 RID: 272 RVA: 0x000073E0 File Offset: 0x000055E0
	public override void OnPlayerExit(PlayerManager player)
	{
		base.OnPlayerExit(player);
		this.players.Remove(player);
		this.chalkFace.state.Cancel();
	}

	// Token: 0x06000111 RID: 273 RVA: 0x00007408 File Offset: 0x00005608
	public void Spawn(Cell tile, ChalkFace chalkFace)
	{
		this.chalkFace = chalkFace;
		this.room = tile.room;
		this.room.functions.AddFunction(this);
		Direction direction = tile.RandomUncoveredDirection(new Random());
		this.chalkboard.transform.parent = tile.TileTransform;
		this.chalkboard.transform.rotation = direction.ToRotation();
		tile.HardCoverWall(direction, true);
		base.transform.position = tile.room.ec.RealRoomMid(tile.room) + Vector3.up * 5f;
	}

	// Token: 0x17000014 RID: 20
	// (get) Token: 0x06000112 RID: 274 RVA: 0x000074AE File Offset: 0x000056AE
	public Vector3 Position
	{
		get
		{
			return this.chalkboard.transform.position;
		}
	}

	// Token: 0x17000015 RID: 21
	// (get) Token: 0x06000113 RID: 275 RVA: 0x000074C0 File Offset: 0x000056C0
	public Quaternion Rotation
	{
		get
		{
			return this.chalkboard.transform.rotation;
		}
	}

	// Token: 0x04000122 RID: 290
	private ChalkFace chalkFace;

	// Token: 0x04000123 RID: 291
	[SerializeField]
	private GameObject chalkboard;

	// Token: 0x04000124 RID: 292
	private List<PlayerManager> players = new List<PlayerManager>();
}
