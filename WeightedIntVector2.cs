using System;
using System.Collections.Generic;

// Token: 0x02000156 RID: 342
[Serializable]
public class WeightedIntVector2 : WeightedSelection<IntVector2>
{
	// Token: 0x0600079A RID: 1946 RVA: 0x00026958 File Offset: 0x00024B58
	public static List<WeightedSelection<IntVector2>> Convert(List<WeightedIntVector2> list)
	{
		List<WeightedSelection<IntVector2>> list2 = new List<WeightedSelection<IntVector2>>();
		foreach (WeightedIntVector2 item in list)
		{
			list2.Add(item);
		}
		return list2;
	}
}
