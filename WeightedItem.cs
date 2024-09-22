using System;
using System.Collections.Generic;

// Token: 0x02000151 RID: 337
[Serializable]
public class WeightedItem : WeightedSelection<ItemObject>
{
	// Token: 0x06000793 RID: 1939 RVA: 0x00026880 File Offset: 0x00024A80
	public static List<WeightedSelection<ItemObject>> Convert(List<WeightedItem> list)
	{
		List<WeightedSelection<ItemObject>> list2 = new List<WeightedSelection<ItemObject>>();
		foreach (WeightedItem item in list)
		{
			list2.Add(item);
		}
		return list2;
	}
}
