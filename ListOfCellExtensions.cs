using System;
using System.Collections.Generic;

// Token: 0x02000160 RID: 352
public static class ListOfCellExtensions
{
	// Token: 0x060007F9 RID: 2041 RVA: 0x00027A00 File Offset: 0x00025C00
	public static void RemoveDuplicateCells(this List<Cell> list, IntVector2 position)
	{
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].position == position)
			{
				list.RemoveAt(i);
				i--;
			}
		}
	}

	// Token: 0x060007FA RID: 2042 RVA: 0x00027A40 File Offset: 0x00025C40
	public static void RemoveEntityUnsafeCells(this List<Cell> list, bool onlyRemoveIfRoomHasSafeCells)
	{
		for (int i = 0; i < list.Count; i++)
		{
			if ((list[i].room.entitySafeCells.Count > 0 || !onlyRemoveIfRoomHasSafeCells) && !list[i].room.entitySafeCells.Contains(list[i].position))
			{
				list.RemoveAt(i);
				i--;
			}
		}
	}

	// Token: 0x060007FB RID: 2043 RVA: 0x00027AAC File Offset: 0x00025CAC
	public static void ConvertEntityUnsafeCells(this List<Cell> list)
	{
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].room.entitySafeCells.Count > 0 && !list[i].room.entitySafeCells.Contains(list[i].position))
			{
				list[i] = list[i].room.RandomEntitySafeCellNoGarbage();
			}
		}
	}

	// Token: 0x060007FC RID: 2044 RVA: 0x00027B20 File Offset: 0x00025D20
	public static void RemoveEventUnsafeCells(this List<Cell> list, bool onlyRemoveIfRoomHasSafeCells)
	{
		for (int i = 0; i < list.Count; i++)
		{
			if ((list[i].room.eventSafeCells.Count > 0 || !onlyRemoveIfRoomHasSafeCells) && !list[i].room.eventSafeCells.Contains(list[i].position))
			{
				list.RemoveAt(i);
				i--;
			}
		}
	}

	// Token: 0x060007FD RID: 2045 RVA: 0x00027B8C File Offset: 0x00025D8C
	public static void ConvertEventUnsafeCells(this List<Cell> list)
	{
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].room.eventSafeCells.Count > 0 && !list[i].room.eventSafeCells.Contains(list[i].position))
			{
				list[i] = list[i].room.RandomEventSafeCellNoGarbage();
			}
		}
	}
}
