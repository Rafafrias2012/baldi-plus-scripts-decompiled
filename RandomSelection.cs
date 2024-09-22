using System;
using System.Collections.Generic;

// Token: 0x02000132 RID: 306
[Serializable]
public class RandomSelection<T>
{
	// Token: 0x06000744 RID: 1860 RVA: 0x00025868 File Offset: 0x00023A68
	public static List<T> ControlledRandomSelection(RandomSelection<T>[] items, Random rng)
	{
		List<T> list = new List<T>();
		foreach (RandomSelection<T> randomSelection in items)
		{
			if (rng.NextDouble() * 100.0 < (double)randomSelection.chance)
			{
				list.Add(randomSelection.selectable);
			}
		}
		return list;
	}

	// Token: 0x06000745 RID: 1861 RVA: 0x000258B8 File Offset: 0x00023AB8
	public static List<T> ControlledRandomSelectionList(List<RandomSelection<T>> items, Random rng)
	{
		List<T> list = new List<T>();
		foreach (RandomSelection<T> randomSelection in items)
		{
			if (rng.NextDouble() * 100.0 < (double)randomSelection.chance)
			{
				list.Add(randomSelection.selectable);
			}
		}
		return list;
	}

	// Token: 0x040007FE RID: 2046
	public T selectable;

	// Token: 0x040007FF RID: 2047
	public float chance;
}
