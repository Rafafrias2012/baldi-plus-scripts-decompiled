using System;
using System.Collections.Generic;

// Token: 0x02000152 RID: 338
[Serializable]
public class WeightedNPC : WeightedSelection<NPC>
{
	// Token: 0x06000795 RID: 1941 RVA: 0x000268E0 File Offset: 0x00024AE0
	public static List<WeightedSelection<NPC>> Convert(List<WeightedNPC> list)
	{
		List<WeightedSelection<NPC>> list2 = new List<WeightedSelection<NPC>>();
		foreach (WeightedNPC item in list)
		{
			list2.Add(item);
		}
		return list2;
	}
}
