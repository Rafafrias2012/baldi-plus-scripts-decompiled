using System;
using System.Collections.Generic;

// Token: 0x0200014B RID: 331
[Serializable]
public class WeightedPosterObject : WeightedSelection<PosterObject>
{
	// Token: 0x06000789 RID: 1929 RVA: 0x000266F0 File Offset: 0x000248F0
	public static List<WeightedSelection<PosterObject>> Convert(List<WeightedPosterObject> list)
	{
		List<WeightedSelection<PosterObject>> list2 = new List<WeightedSelection<PosterObject>>();
		foreach (WeightedPosterObject item in list)
		{
			list2.Add(item);
		}
		return list2;
	}
}
