using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000074 RID: 116
public class MysteryRoom : RandomEvent
{
	// Token: 0x0600029F RID: 671 RVA: 0x0000E742 File Offset: 0x0000C942
	public override void AssignRoom(RoomController room)
	{
		base.AssignRoom(room);
		room.doorPre = this.mysteryDoorPre.GetComponent<Door>();
		room.potentialDoorPositions.Clear();
		room.forcedDoorPositions.Clear();
	}

	// Token: 0x060002A0 RID: 672 RVA: 0x0000E774 File Offset: 0x0000C974
	public override void PremadeSetup()
	{
		foreach (RoomController roomController in this.ec.rooms)
		{
			if (roomController.category == RoomCategory.Mystery)
			{
				this.room = roomController;
				break;
			}
		}
		this.UpdateDoor();
	}

	// Token: 0x060002A1 RID: 673 RVA: 0x0000E7E0 File Offset: 0x0000C9E0
	public override void AfterUpdateSetup()
	{
		foreach (Cell cell in this.room.GetNewTileList())
		{
			cell.HardCoverEntirely();
			for (int i = 0; i < 4; i++)
			{
				if (cell.HasWallInDirection((Direction)i))
				{
					cell.HardCoverWall((Direction)i, true);
				}
			}
		}
		this.UpdateDoor();
		this.SpawnItem();
	}

	// Token: 0x060002A2 RID: 674 RVA: 0x0000E860 File Offset: 0x0000CA60
	public void UpdateDoor()
	{
		if (this.room != null)
		{
			foreach (Door door in this.room.doors)
			{
				this.mysteryDoors.Add(door.GetComponent<MysteryDoor>());
				MaterialModifier.SetBase(this.mysteryDoors[this.mysteryDoors.Count - 1].Cover, door.bTile.room.wallTex);
				this.mysteryDoors[this.mysteryDoors.Count - 1].HideDoor(true);
				door.bTile.Block(door.direction.GetOpposite(), true);
			}
		}
	}

	// Token: 0x060002A3 RID: 675 RVA: 0x0000E940 File Offset: 0x0000CB40
	public void SpawnItem()
	{
		EnvironmentController ec = this.ec;
		RoomController room = this.room;
		WeightedSelection<ItemObject>[] array = this.items;
		ec.CreateItem(room, WeightedSelection<ItemObject>.ControlledRandomSelection(array, this.crng), new Vector2(20f, 20f));
	}

	// Token: 0x060002A4 RID: 676 RVA: 0x0000E984 File Offset: 0x0000CB84
	public override void Begin()
	{
		if (this.room != null)
		{
			base.Begin();
			foreach (MysteryDoor mysteryDoor in this.mysteryDoors)
			{
				mysteryDoor.HideDoor(false);
				mysteryDoor.Door.Unlock();
			}
		}
	}

	// Token: 0x060002A5 RID: 677 RVA: 0x0000E9F4 File Offset: 0x0000CBF4
	public override void End()
	{
		if (this.room != null)
		{
			base.End();
			foreach (MysteryDoor mysteryDoor in this.mysteryDoors)
			{
				mysteryDoor.HideDoor(true);
				mysteryDoor.Door.Lock(true);
			}
		}
	}

	// Token: 0x040002BC RID: 700
	[SerializeField]
	private MysteryDoor mysteryDoorPre;

	// Token: 0x040002BD RID: 701
	[SerializeField]
	private List<MysteryDoor> mysteryDoors = new List<MysteryDoor>();

	// Token: 0x040002BE RID: 702
	[SerializeField]
	private WeightedItemObject[] items = new WeightedItemObject[0];

	// Token: 0x040002BF RID: 703
	[SerializeField]
	private Texture2D roomTex;

	// Token: 0x040002C0 RID: 704
	[SerializeField]
	private StandardDoorMats doorMats;
}
