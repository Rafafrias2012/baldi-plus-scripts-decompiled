using System;
using System.Collections.Generic;

// Token: 0x020001F0 RID: 496
public static class ItemObjectFunctions
{
	// Token: 0x06000B40 RID: 2880 RVA: 0x0003B764 File Offset: 0x00039964
	public static void SortByValue(this List<ItemObject> list)
	{
		List<ItemObject> list2 = new List<ItemObject>();
		int index = 0;
		int num = 0;
		while (list.Count > 0)
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].value >= num)
				{
					index = i;
					num = list[i].value;
				}
			}
			list2.Add(list[index]);
			list.RemoveAt(index);
			index = 0;
			num = 0;
		}
		list.Clear();
		list.AddRange(list2);
	}

	// Token: 0x06000B41 RID: 2881 RVA: 0x0003B7DC File Offset: 0x000399DC
	public static int Value(this List<ItemObject> list)
	{
		int num = 0;
		for (int i = 0; i < list.Count; i++)
		{
			num += list[i].value;
		}
		return num;
	}
}
