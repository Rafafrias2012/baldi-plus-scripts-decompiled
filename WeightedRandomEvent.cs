using System;
using System.Collections.Generic;

// Token: 0x0200014F RID: 335
[Serializable]
public class WeightedRandomEvent : WeightedSelection<RandomEvent>
{
	// Token: 0x06000790 RID: 1936 RVA: 0x00026818 File Offset: 0x00024A18
	public static List<WeightedSelection<RandomEvent>> Convert(List<WeightedRandomEvent> list)
	{
		List<WeightedSelection<RandomEvent>> list2 = new List<WeightedSelection<RandomEvent>>();
		foreach (WeightedRandomEvent item in list)
		{
			list2.Add(item);
		}
		return list2;
	}
}
