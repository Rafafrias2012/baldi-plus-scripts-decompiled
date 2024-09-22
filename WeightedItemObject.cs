using System;
using System.Collections.Generic;

// Token: 0x0200014E RID: 334
[Serializable]
public class WeightedItemObject : WeightedSelection<ItemObject>
{
	// Token: 0x0600078E RID: 1934 RVA: 0x000267B8 File Offset: 0x000249B8
	public static List<WeightedSelection<ItemObject>> Convert(List<WeightedItemObject> list)
	{
		List<WeightedSelection<ItemObject>> list2 = new List<WeightedSelection<ItemObject>>();
		foreach (WeightedItemObject item in list)
		{
			list2.Add(item);
		}
		return list2;
	}
}
