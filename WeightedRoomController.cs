using System;
using System.Collections.Generic;

// Token: 0x0200014C RID: 332
[Serializable]
public class WeightedRoomController : WeightedSelection<RoomController>
{
	// Token: 0x0600078B RID: 1931 RVA: 0x00026750 File Offset: 0x00024950
	public static List<WeightedSelection<RoomController>> Convert(List<WeightedRoomController> list)
	{
		List<WeightedSelection<RoomController>> list2 = new List<WeightedSelection<RoomController>>();
		foreach (WeightedRoomController item in list)
		{
			list2.Add(item);
		}
		return list2;
	}
}
