using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001B0 RID: 432
public class SpecialRoomSwingingDoorsBuilder : RoomFunction
{
	// Token: 0x060009C3 RID: 2499 RVA: 0x000340D0 File Offset: 0x000322D0
	public override void Build(LevelBuilder builder, Random rng)
	{
		base.Build(builder, rng);
		this.room.doorPre = this.swingDoorPre;
		List<List<IntVector2>> list = new List<List<IntVector2>>();
		for (int i = 0; i < 4; i++)
		{
			list.Add(new List<IntVector2>());
		}
		foreach (IntVector2 intVector in this.room.potentialDoorPositions)
		{
			for (int j = 0; j < 4; j++)
			{
				if (this.room.ec.CellFromPosition(intVector).HasWallInDirection((Direction)j))
				{
					list[j].Add(intVector);
				}
			}
		}
		foreach (List<IntVector2> list2 in list)
		{
			while (list2.Count > 0)
			{
				int index = rng.Next(0, list2.Count);
				RoomController newRoom;
				IntVector2 intVector2;
				if (builder.BuildDoorIfPossible(list2[index], this.room, true, false, out newRoom, out intVector2))
				{
					this.room.ConnectRooms(newRoom);
					break;
				}
				list2.RemoveAt(index);
			}
		}
	}

	// Token: 0x04000AFF RID: 2815
	[SerializeField]
	private Door swingDoorPre;
}
