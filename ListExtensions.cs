using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000119 RID: 281
public static class ListExtensions
{
	// Token: 0x060006DC RID: 1756 RVA: 0x00022CA8 File Offset: 0x00020EA8
	public static void Shuffle<T>(this List<T> list)
	{
		List<T> list2 = new List<T>();
		while (list.Count > 0)
		{
			int index = Random.Range(0, list.Count);
			list2.Add(list[index]);
			list.RemoveAt(index);
		}
		list.AddRange(list2);
	}
}
