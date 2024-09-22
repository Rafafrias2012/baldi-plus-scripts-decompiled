using System;
using System.Collections.Generic;

// Token: 0x02000158 RID: 344
public class WeightedRoomSpawn : WeightedSelection<RoomSpawn>
{
	// Token: 0x0600079D RID: 1949 RVA: 0x000269C0 File Offset: 0x00024BC0
	public static List<WeightedSelection<RoomSpawn>> Convert(List<WeightedRoomSpawn> list)
	{
		List<WeightedSelection<RoomSpawn>> list2 = new List<WeightedSelection<RoomSpawn>>();
		foreach (WeightedRoomSpawn item in list)
		{
			list2.Add(item);
		}
		return list2;
	}
}
