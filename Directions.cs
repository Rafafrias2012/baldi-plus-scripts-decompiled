using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000F9 RID: 249
public static class Directions
{
	// Token: 0x060005C6 RID: 1478 RVA: 0x0001D18C File Offset: 0x0001B38C
	public static Direction ControlledRandomDirection(Random rng)
	{
		return (Direction)rng.Next(0, 4);
	}

	// Token: 0x060005C7 RID: 1479 RVA: 0x0001D196 File Offset: 0x0001B396
	public static Direction FromInt(int i)
	{
		return (Direction)i;
	}

	// Token: 0x17000070 RID: 112
	// (get) Token: 0x060005C8 RID: 1480 RVA: 0x0001D199 File Offset: 0x0001B399
	public static Direction RandomDirection
	{
		get
		{
			return (Direction)Random.Range(0, 4);
		}
	}

	// Token: 0x060005C9 RID: 1481 RVA: 0x0001D1A2 File Offset: 0x0001B3A2
	public static IntVector2 CellDataRotationVector(Direction direction)
	{
		return Directions.cellDataRotationVectors[(int)direction];
	}

	// Token: 0x17000071 RID: 113
	// (get) Token: 0x060005CA RID: 1482 RVA: 0x0001D1AF File Offset: 0x0001B3AF
	public static IntVector2[] Vectors
	{
		get
		{
			return Directions.vectors;
		}
	}

	// Token: 0x060005CB RID: 1483 RVA: 0x0001D1B6 File Offset: 0x0001B3B6
	public static IntVector2 ToIntVector2(this Direction direction)
	{
		return Directions.vectors[(int)direction];
	}

	// Token: 0x060005CC RID: 1484 RVA: 0x0001D1C3 File Offset: 0x0001B3C3
	public static Vector3 ToVector3(this Direction direction)
	{
		return Directions.vector3s[(int)direction];
	}

	// Token: 0x060005CD RID: 1485 RVA: 0x0001D1D0 File Offset: 0x0001B3D0
	public static CellCoverage ToCoverage(this Direction direction)
	{
		return Directions.coverages[(int)direction];
	}

	// Token: 0x060005CE RID: 1486 RVA: 0x0001D1D9 File Offset: 0x0001B3D9
	public static Direction GetOpposite(this Direction direction)
	{
		return Directions.opposites[(int)direction];
	}

	// Token: 0x060005CF RID: 1487 RVA: 0x0001D1E4 File Offset: 0x0001B3E4
	public static Direction RotatedRelativeToNorth(this Direction direction, Direction rotation)
	{
		int num = (int)(direction + (int)rotation);
		if (num >= 4)
		{
			num -= 4;
		}
		return (Direction)num;
	}

	// Token: 0x060005D0 RID: 1488 RVA: 0x0001D200 File Offset: 0x0001B400
	public static Quaternion ToRotation(this Direction direction)
	{
		return Directions.rotations[(int)direction];
	}

	// Token: 0x060005D1 RID: 1489 RVA: 0x0001D20D File Offset: 0x0001B40D
	public static Quaternion ToUiRotation(this Direction direction)
	{
		return Directions.uiRotations[(int)direction];
	}

	// Token: 0x060005D2 RID: 1490 RVA: 0x0001D21A File Offset: 0x0001B41A
	public static float ToDegrees(this Direction direction)
	{
		return Directions.degrees[(int)direction];
	}

	// Token: 0x060005D3 RID: 1491 RVA: 0x0001D223 File Offset: 0x0001B423
	public static int ToBinary(this Direction direction)
	{
		return Directions.binary[(int)direction];
	}

	// Token: 0x060005D4 RID: 1492 RVA: 0x0001D22C File Offset: 0x0001B42C
	public static List<Direction> PerpendicularList(this Direction dir)
	{
		List<Direction> list = new List<Direction>();
		if (dir == Direction.North)
		{
			list.Add(Direction.East);
			list.Add(Direction.West);
		}
		else if (dir == Direction.East)
		{
			list.Add(Direction.North);
			list.Add(Direction.South);
		}
		else if (dir == Direction.South)
		{
			list.Add(Direction.East);
			list.Add(Direction.West);
		}
		else if (dir == Direction.West)
		{
			list.Add(Direction.North);
			list.Add(Direction.South);
		}
		return list;
	}

	// Token: 0x060005D5 RID: 1493 RVA: 0x0001D290 File Offset: 0x0001B490
	public static List<Direction> OpenDirectionsFromBin(int bin)
	{
		List<Direction> list = new List<Direction>();
		for (int i = 0; i < 4; i++)
		{
			if ((bin & 1 << i) == 0)
			{
				list.Add((Direction)i);
			}
		}
		return list;
	}

	// Token: 0x060005D6 RID: 1494 RVA: 0x0001D2C4 File Offset: 0x0001B4C4
	public static void FillOpenDirectionsFromBin(List<Direction> list, int bin)
	{
		list.Clear();
		for (int i = 0; i < 4; i++)
		{
			if ((bin & 1 << i) == 0)
			{
				list.Add((Direction)i);
			}
		}
	}

	// Token: 0x060005D7 RID: 1495 RVA: 0x0001D2F4 File Offset: 0x0001B4F4
	public static List<Direction> ClosedDirectionsFromBin(int bin)
	{
		List<Direction> list = new List<Direction>();
		for (int i = 0; i < 4; i++)
		{
			if ((bin & 1 << i) > 0)
			{
				list.Add((Direction)i);
			}
		}
		return list;
	}

	// Token: 0x060005D8 RID: 1496 RVA: 0x0001D328 File Offset: 0x0001B528
	public static void FillClosedDirectionsFromBin(List<Direction> list, int bin)
	{
		list.Clear();
		for (int i = 0; i < 4; i++)
		{
			if ((bin & 1 << i) > 0)
			{
				list.Add((Direction)i);
			}
		}
	}

	// Token: 0x060005D9 RID: 1497 RVA: 0x0001D359 File Offset: 0x0001B559
	public static bool ContainsDirection(this int val, Direction direction)
	{
		return (val & 1 << direction.BitPosition()) > 0;
	}

	// Token: 0x060005DA RID: 1498 RVA: 0x0001D36B File Offset: 0x0001B56B
	public static List<Direction> All()
	{
		return new List<Direction>
		{
			Direction.North,
			Direction.East,
			Direction.South,
			Direction.West
		};
	}

	// Token: 0x060005DB RID: 1499 RVA: 0x0001D38E File Offset: 0x0001B58E
	public static void FillWithAll(List<Direction> list)
	{
		list.Clear();
		list.Add(Direction.North);
		list.Add(Direction.East);
		list.Add(Direction.South);
		list.Add(Direction.West);
	}

	// Token: 0x060005DC RID: 1500 RVA: 0x0001D3B4 File Offset: 0x0001B5B4
	public static void DirsFromVector3(Vector3 vector, List<Direction> list)
	{
		list.Clear();
		float num = Vector3.SignedAngle(vector, Vector3.right, Vector3.up);
		if (num >= 10f && num <= 170f)
		{
			list.Add(Direction.North);
		}
		if (num >= -80f && num <= 80f)
		{
			list.Add(Direction.East);
		}
		if (num >= -170f && num <= -10f)
		{
			list.Add(Direction.South);
		}
		if (num <= -100f || num >= 100f)
		{
			list.Add(Direction.West);
		}
	}

	// Token: 0x060005DD RID: 1501 RVA: 0x0001D434 File Offset: 0x0001B634
	public static Direction DirFromVector3(Vector3 vector, float buffer)
	{
		float num = Vector3.SignedAngle(vector, Vector3.right, Vector3.up);
		if (num >= 90f - buffer && num <= 90f + buffer)
		{
			return Direction.North;
		}
		if (num >= 0f - buffer && num <= 0f + buffer)
		{
			return Direction.East;
		}
		if (num >= -90f - buffer && num <= -90f + buffer)
		{
			return Direction.South;
		}
		if (num <= -180f + buffer || num >= 180f - buffer)
		{
			return Direction.West;
		}
		Debug.LogWarning(string.Concat(new string[]
		{
			"DirFromVector3 with angle ",
			num.ToString(),
			" and buffer ",
			buffer.ToString(),
			" returned null"
		}));
		return Direction.Null;
	}

	// Token: 0x060005DE RID: 1502 RVA: 0x0001D4E8 File Offset: 0x0001B6E8
	public static void ReverseList(List<Direction> list)
	{
		for (int i = 0; i < list.Count; i++)
		{
			list[i] = list[i].GetOpposite();
		}
	}

	// Token: 0x060005DF RID: 1503 RVA: 0x0001D519 File Offset: 0x0001B719
	public static int BitPosition(this Direction dir)
	{
		switch (dir)
		{
		case Direction.North:
			return 0;
		case Direction.East:
			return 1;
		case Direction.South:
			return 2;
		case Direction.West:
			return 3;
		default:
			return 0;
		}
	}

	// Token: 0x060005E0 RID: 1504 RVA: 0x0001D53C File Offset: 0x0001B73C
	public static int RotateBin(int type, Direction direction)
	{
		int num = type;
		for (int i = 0; i < (int)direction; i++)
		{
			num <<= 1;
			num = ((num | num >> 4) & 15);
		}
		return num;
	}

	// Token: 0x040005F2 RID: 1522
	public const int Count = 4;

	// Token: 0x040005F3 RID: 1523
	private const float angleBuffer = 80f;

	// Token: 0x040005F4 RID: 1524
	private static IntVector2[] vectors = new IntVector2[]
	{
		new IntVector2(0, 1),
		new IntVector2(1, 0),
		new IntVector2(0, -1),
		new IntVector2(-1, 0),
		new IntVector2(0, 0)
	};

	// Token: 0x040005F5 RID: 1525
	private static IntVector2[] cellDataRotationVectors = new IntVector2[]
	{
		new IntVector2(1, 1),
		new IntVector2(1, -1),
		new IntVector2(-1, -1),
		new IntVector2(-1, 1)
	};

	// Token: 0x040005F6 RID: 1526
	private static Vector3[] vector3s = new Vector3[]
	{
		new Vector3(0f, 0f, 1f),
		new Vector3(1f, 0f, 0f),
		new Vector3(0f, 0f, -1f),
		new Vector3(-1f, 0f, 0f)
	};

	// Token: 0x040005F7 RID: 1527
	private static CellCoverage[] coverages = new CellCoverage[]
	{
		CellCoverage.North,
		CellCoverage.East,
		CellCoverage.South,
		CellCoverage.West
	};

	// Token: 0x040005F8 RID: 1528
	private static Direction[] opposites = new Direction[]
	{
		Direction.South,
		Direction.West,
		Direction.North,
		Direction.East,
		Direction.Null
	};

	// Token: 0x040005F9 RID: 1529
	private static Quaternion[] rotations = new Quaternion[]
	{
		Quaternion.identity,
		Quaternion.Euler(0f, 90f, 0f),
		Quaternion.Euler(0f, 180f, 0f),
		Quaternion.Euler(0f, 270f, 0f)
	};

	// Token: 0x040005FA RID: 1530
	private static Quaternion[] uiRotations = new Quaternion[]
	{
		Quaternion.identity,
		Quaternion.Euler(0f, 0f, 270f),
		Quaternion.Euler(0f, 0f, 180f),
		Quaternion.Euler(0f, 0f, 90f)
	};

	// Token: 0x040005FB RID: 1531
	private static float[] degrees = new float[]
	{
		0f,
		90f,
		180f,
		270f
	};

	// Token: 0x040005FC RID: 1532
	private static int[] binary = new int[]
	{
		1,
		2,
		4,
		8
	};
}
