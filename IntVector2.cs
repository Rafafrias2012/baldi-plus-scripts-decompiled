using System;
using UnityEngine;

// Token: 0x02000113 RID: 275
[Serializable]
public struct IntVector2
{
	// Token: 0x060006C2 RID: 1730 RVA: 0x00022882 File Offset: 0x00020A82
	public IntVector2(int x, int z)
	{
		this.x = x;
		this.z = z;
	}

	// Token: 0x060006C3 RID: 1731 RVA: 0x00022892 File Offset: 0x00020A92
	public static IntVector2 operator +(IntVector2 a, IntVector2 b)
	{
		a.x += b.x;
		a.z += b.z;
		return a;
	}

	// Token: 0x060006C4 RID: 1732 RVA: 0x000228B7 File Offset: 0x00020AB7
	public static IntVector2 operator -(IntVector2 a, IntVector2 b)
	{
		a.x -= b.x;
		a.z -= b.z;
		return a;
	}

	// Token: 0x060006C5 RID: 1733 RVA: 0x000228DC File Offset: 0x00020ADC
	public static IntVector2 operator *(IntVector2 a, int b)
	{
		a.x *= b;
		a.z *= b;
		return a;
	}

	// Token: 0x060006C6 RID: 1734 RVA: 0x00022900 File Offset: 0x00020B00
	public static Vector2 operator *(IntVector2 a, float b)
	{
		return new Vector2
		{
			x = (float)a.x * b,
			y = (float)a.z * b
		};
	}

	// Token: 0x060006C7 RID: 1735 RVA: 0x00022936 File Offset: 0x00020B36
	public static bool operator ==(IntVector2 a, IntVector2 b)
	{
		return a.x == b.x && a.z == b.z;
	}

	// Token: 0x060006C8 RID: 1736 RVA: 0x00022956 File Offset: 0x00020B56
	public static bool operator !=(IntVector2 a, IntVector2 b)
	{
		return a.x != b.x || a.z != b.z;
	}

	// Token: 0x060006C9 RID: 1737 RVA: 0x00022979 File Offset: 0x00020B79
	public IntVector2 Scale(IntVector2 toScale)
	{
		toScale.x *= this.x;
		toScale.z *= this.z;
		return toScale;
	}

	// Token: 0x060006CA RID: 1738 RVA: 0x0002299E File Offset: 0x00020B9E
	public static IntVector2 ControlledRandomPosition(int minX, int maxX, int minZ, int maxZ, Random rng)
	{
		return new IntVector2(rng.Next(minX, maxX), rng.Next(minZ, maxZ));
	}

	// Token: 0x060006CB RID: 1739 RVA: 0x000229B7 File Offset: 0x00020BB7
	public static IntVector2 RandomPosition(int rangeX, int rangeZ)
	{
		return new IntVector2(Random.Range(0, rangeX), Random.Range(0, rangeZ));
	}

	// Token: 0x060006CC RID: 1740 RVA: 0x000229CC File Offset: 0x00020BCC
	public static Vector2 ToVector2(IntVector2 iv2)
	{
		return new Vector2((float)iv2.x, (float)iv2.z);
	}

	// Token: 0x060006CD RID: 1741 RVA: 0x000229E4 File Offset: 0x00020BE4
	public static IntVector2 GetGridPosition(Vector3 position)
	{
		return new IntVector2
		{
			x = Mathf.FloorToInt(position.x / 10f),
			z = Mathf.FloorToInt(position.z / 10f)
		};
	}

	// Token: 0x060006CE RID: 1742 RVA: 0x00022A2C File Offset: 0x00020C2C
	public static IntVector2 CombineLowest(IntVector2 vectorA, IntVector2 vectorB)
	{
		IntVector2 result = default(IntVector2);
		if (vectorA.x > vectorB.x)
		{
			result.x = vectorB.x;
		}
		else
		{
			result.x = vectorA.x;
		}
		if (vectorA.z > vectorB.z)
		{
			result.z = vectorB.z;
		}
		else
		{
			result.z = vectorA.z;
		}
		return result;
	}

	// Token: 0x060006CF RID: 1743 RVA: 0x00022A98 File Offset: 0x00020C98
	public static IntVector2 CombineGreatest(IntVector2 vectorA, IntVector2 vectorB)
	{
		IntVector2 result = default(IntVector2);
		if (vectorA.x > vectorB.x)
		{
			result.x = vectorA.x;
		}
		else
		{
			result.x = vectorB.x;
		}
		if (vectorA.z > vectorB.z)
		{
			result.z = vectorA.z;
		}
		else
		{
			result.z = vectorB.z;
		}
		return result;
	}

	// Token: 0x060006D0 RID: 1744 RVA: 0x00022B04 File Offset: 0x00020D04
	public IntVector2 Adjusted(IntVector2 pivot, Direction direction)
	{
		IntVector2 intVector = default(IntVector2);
		if (direction == Direction.North || direction == Direction.South)
		{
			intVector.x = this.x - pivot.x;
			intVector.z = this.z - pivot.z;
		}
		else
		{
			intVector.x = this.z - pivot.z;
			intVector.z = this.x - pivot.x;
		}
		return intVector.Scale(Directions.CellDataRotationVector(direction));
	}

	// Token: 0x060006D1 RID: 1745 RVA: 0x00022B81 File Offset: 0x00020D81
	public new string ToString()
	{
		return string.Format("{0},{1}", this.x, this.z);
	}

	// Token: 0x040006FC RID: 1788
	public int x;

	// Token: 0x040006FD RID: 1789
	public int z;
}
