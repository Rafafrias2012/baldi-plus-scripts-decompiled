using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000147 RID: 327
[Serializable]
public class WeightedSelection<T>
{
	// Token: 0x06000780 RID: 1920 RVA: 0x00026519 File Offset: 0x00024719
	public static T ControlledRandomSelection(WeightedSelection<T>[] items, Random rng)
	{
		return items[WeightedSelection<T>.ControlledRandomIndex(items, rng)].selection;
	}

	// Token: 0x06000781 RID: 1921 RVA: 0x0002652C File Offset: 0x0002472C
	public static int ControlledRandomIndex(WeightedSelection<T>[] items, Random rng)
	{
		int num = 0;
		int num2 = 0;
		foreach (WeightedSelection<T> weightedSelection in items)
		{
			num2 += weightedSelection.weight;
		}
		int num3 = rng.Next(0, num2);
		int j;
		for (j = 0; j < items.Length; j++)
		{
			num += items[j].weight;
			if (num > num3)
			{
				break;
			}
		}
		if (j < items.Length)
		{
			return j;
		}
		Debug.Log("No valid selection found. Returning index 0");
		return 0;
	}

	// Token: 0x06000782 RID: 1922 RVA: 0x000265A0 File Offset: 0x000247A0
	public static T RandomSelection(WeightedSelection<T>[] items)
	{
		int num = 0;
		int num2 = 0;
		foreach (WeightedSelection<T> weightedSelection in items)
		{
			num2 += weightedSelection.weight;
		}
		int num3 = Random.Range(0, num2);
		int j;
		for (j = 0; j < items.Length; j++)
		{
			num += items[j].weight;
			if (num > num3)
			{
				break;
			}
		}
		if (j < items.Length)
		{
			return items[j].selection;
		}
		Debug.Log("No valid selection found. Returning index 0");
		return items[0].selection;
	}

	// Token: 0x06000783 RID: 1923 RVA: 0x00026620 File Offset: 0x00024820
	public static int ControlledRandomIndexList(List<WeightedSelection<T>> items, Random rng)
	{
		int num = 0;
		int num2 = 0;
		foreach (WeightedSelection<T> weightedSelection in items)
		{
			num2 += weightedSelection.weight;
		}
		int num3 = rng.Next(0, num2);
		int i;
		for (i = 0; i < items.Count; i++)
		{
			num += items[i].weight;
			if (num > num3)
			{
				break;
			}
		}
		if (i < items.Count)
		{
			return i;
		}
		Debug.Log("No valid selection found. Returning index 0");
		return 0;
	}

	// Token: 0x06000784 RID: 1924 RVA: 0x000266BC File Offset: 0x000248BC
	public static T ControlledRandomSelectionList(List<WeightedSelection<T>> items, Random rng)
	{
		return items[WeightedSelection<T>.ControlledRandomIndexList(items, rng)].selection;
	}

	// Token: 0x04000842 RID: 2114
	public T selection;

	// Token: 0x04000843 RID: 2115
	public int weight;
}
