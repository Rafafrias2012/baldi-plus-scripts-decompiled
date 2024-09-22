using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000114 RID: 276
public static class IntVector2Extensions
{
	// Token: 0x060006D2 RID: 1746 RVA: 0x00022BA4 File Offset: 0x00020DA4
	public static void Adjust(this List<IntVector2> list, IntVector2 position, IntVector2 pivot, Direction direction)
	{
		for (int i = 0; i < list.Count; i++)
		{
			list[i] = list[i].Adjusted(pivot, direction) + position;
		}
	}

	// Token: 0x060006D3 RID: 1747 RVA: 0x00022BE0 File Offset: 0x00020DE0
	public static void RemoveMatchingIntVector2s(this List<IntVector2> list, IntVector2 toRemove)
	{
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i] == toRemove)
			{
				list.RemoveAt(i);
				i--;
				Debug.Log("Found and removed matching IV2");
			}
		}
	}

	// Token: 0x060006D4 RID: 1748 RVA: 0x00022C24 File Offset: 0x00020E24
	public static void AddRangeExceptDuplicates(this List<IntVector2> list, List<IntVector2> toAdd)
	{
		for (int i = 0; i < toAdd.Count; i++)
		{
			if (!list.Contains(toAdd[i]))
			{
				list.Add(toAdd[i]);
			}
		}
	}
}
