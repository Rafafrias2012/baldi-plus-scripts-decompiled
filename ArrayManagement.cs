using System;

// Token: 0x020000EC RID: 236
public static class ArrayManagement
{
	// Token: 0x06000573 RID: 1395 RVA: 0x0001BAF0 File Offset: 0x00019CF0
	public static void ShiftForward<T>(this T[] array)
	{
		for (int i = array.Length - 1; i > 0; i--)
		{
			array[i] = array[i - 1];
		}
	}

	// Token: 0x06000574 RID: 1396 RVA: 0x0001BB20 File Offset: 0x00019D20
	public static void ShiftBack<T>(this T[] array)
	{
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = array[i + 1];
		}
	}

	// Token: 0x06000575 RID: 1397 RVA: 0x0001BB4C File Offset: 0x00019D4C
	public static T[,] ConvertTo2d<T>(this T[] array, int sizeX, int sizeZ)
	{
		T[,] array2 = new T[sizeX, sizeZ];
		for (int i = 0; i < sizeX; i++)
		{
			for (int j = 0; j < sizeZ; j++)
			{
				array2[i, j] = array[i * sizeZ + j];
			}
		}
		return array2;
	}

	// Token: 0x06000576 RID: 1398 RVA: 0x0001BB90 File Offset: 0x00019D90
	public static T[] ConvertTo1d<T>(this T[,] array, int sizeX, int sizeZ)
	{
		T[] array2 = new T[sizeX * sizeZ];
		for (int i = 0; i < sizeX; i++)
		{
			for (int j = 0; j < sizeZ; j++)
			{
				array2[i * sizeZ + j] = array[i, j];
			}
		}
		return array2;
	}
}
