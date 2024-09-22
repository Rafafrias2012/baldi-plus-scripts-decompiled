using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000F7 RID: 247
public class DijkstraMap
{
	// Token: 0x060005B9 RID: 1465 RVA: 0x0001CB2C File Offset: 0x0001AD2C
	public DijkstraMap(EnvironmentController environment, PathType pathType, params Transform[] target)
	{
		this.environment = environment;
		this.pathType = pathType;
		this.targets.Clear();
		this.targetPosition = new IntVector2[target.Length];
		if (this.size != environment.levelSize)
		{
			this.size = environment.levelSize;
			this.value = new int[environment.levelSize.x, environment.levelSize.z];
			this.directionToSource = new Direction[environment.levelSize.x, environment.levelSize.z];
		}
		for (int i = 0; i < target.Length; i++)
		{
			this.targets.Add(target[i]);
			this.targetPosition[i] = IntVector2.GetGridPosition(target[i].position);
		}
	}

	// Token: 0x060005BA RID: 1466 RVA: 0x0001CC42 File Offset: 0x0001AE42
	public void Activate()
	{
		this.environment.AddDijkstraMap(this);
	}

	// Token: 0x060005BB RID: 1467 RVA: 0x0001CC50 File Offset: 0x0001AE50
	public void Deactivate()
	{
		this.environment.RemoveDijkstraMap(this);
	}

	// Token: 0x060005BC RID: 1468 RVA: 0x0001CC5E File Offset: 0x0001AE5E
	public void QueueUpdate()
	{
		this.environment.QueueDijkstraMap(this);
		this.pendingUpdate = true;
	}

	// Token: 0x060005BD RID: 1469 RVA: 0x0001CC73 File Offset: 0x0001AE73
	public int Value(IntVector2 position)
	{
		return this.value[position.x, position.z];
	}

	// Token: 0x060005BE RID: 1470 RVA: 0x0001CC8C File Offset: 0x0001AE8C
	public Direction DirectionToSource(IntVector2 position)
	{
		return this.directionToSource[position.x, position.z];
	}

	// Token: 0x1700006E RID: 110
	// (get) Token: 0x060005BF RID: 1471 RVA: 0x0001CCA5 File Offset: 0x0001AEA5
	public bool PendingUpdate
	{
		get
		{
			return this.pendingUpdate;
		}
	}

	// Token: 0x060005C0 RID: 1472 RVA: 0x0001CCB0 File Offset: 0x0001AEB0
	public bool UpdateIsNeeded()
	{
		for (int i = 0; i < this.targets.Count; i++)
		{
			if (IntVector2.GetGridPosition(this.targets[i].position) != this.targetPosition[i])
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060005C1 RID: 1473 RVA: 0x0001CD00 File Offset: 0x0001AF00
	public void Calculate()
	{
		for (int i = 0; i < this.targets.Count; i++)
		{
			this.targetPosition[i] = IntVector2.GetGridPosition(this.targets[i].position);
		}
		this.Calculate(this.targetPosition);
	}

	// Token: 0x060005C2 RID: 1474 RVA: 0x0001CD51 File Offset: 0x0001AF51
	public void Calculate(params IntVector2[] goal)
	{
		this.Calculate(int.MaxValue, false, goal);
	}

	// Token: 0x060005C3 RID: 1475 RVA: 0x0001CD60 File Offset: 0x0001AF60
	public void Calculate(int limit, bool storeFoundCells, params IntVector2[] goal)
	{
		this.pendingUpdate = false;
		bool flag = false;
		this.foundCellPositions.Clear();
		for (int i = 0; i < goal.Length; i++)
		{
			if (this.environment.ContainsCoordinates(goal[i]))
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			this._fillQueue.Clear();
			if (this.size != this.environment.levelSize)
			{
				this.size = this.environment.levelSize;
				this.value = new int[this.environment.levelSize.x, this.environment.levelSize.z];
				this.directionToSource = new Direction[this.environment.levelSize.x, this.environment.levelSize.z];
			}
			for (int j = 0; j < this.size.x; j++)
			{
				for (int k = 0; k < this.size.z; k++)
				{
					this.value[j, k] = int.MaxValue;
					this.directionToSource[j, k] = Direction.Null;
				}
			}
			for (int l = 0; l < goal.Length; l++)
			{
				if (this.environment.ContainsCoordinates(goal[l]) && !this.environment.CellFromPosition(goal[l]).Null)
				{
					this.value[goal[l].x, goal[l].z] = 0;
					this._fillQueue.Enqueue(goal[l]);
				}
			}
			while (this._fillQueue.Count > 0)
			{
				IntVector2 intVector = this._fillQueue.Dequeue();
				for (int m = 0; m < 4; m++)
				{
					if (this.value[intVector.x, intVector.z] < limit && this.environment.CellFromPosition(intVector.x, intVector.z).Navigable((Direction)m, this.pathType) && this.environment.ContainsCoordinates(intVector.x + ((Direction)m).ToIntVector2().x, intVector.z + ((Direction)m).ToIntVector2().z) && !this.environment.CellFromPosition(intVector.x + ((Direction)m).ToIntVector2().x, intVector.z + ((Direction)m).ToIntVector2().z).Null && this.value[intVector.x + ((Direction)m).ToIntVector2().x, intVector.z + ((Direction)m).ToIntVector2().z] > this.value[intVector.x, intVector.z] + 1)
					{
						this.value[intVector.x + ((Direction)m).ToIntVector2().x, intVector.z + ((Direction)m).ToIntVector2().z] = this.value[intVector.x, intVector.z] + 1;
						this.directionToSource[intVector.x + ((Direction)m).ToIntVector2().x, intVector.z + ((Direction)m).ToIntVector2().z] = ((Direction)m).GetOpposite();
						IntVector2 item = new IntVector2(intVector.x + ((Direction)m).ToIntVector2().x, intVector.z + ((Direction)m).ToIntVector2().z);
						this._fillQueue.Enqueue(item);
						if (storeFoundCells)
						{
							this.foundCellPositions.Add(item);
						}
					}
				}
			}
		}
	}

	// Token: 0x060005C4 RID: 1476 RVA: 0x0001D11C File Offset: 0x0001B31C
	public List<Cell> FoundCells()
	{
		List<Cell> list = new List<Cell>();
		foreach (IntVector2 position in this.foundCellPositions)
		{
			list.Add(this.environment.CellFromPosition(position));
		}
		return list;
	}

	// Token: 0x1700006F RID: 111
	// (get) Token: 0x060005C5 RID: 1477 RVA: 0x0001D184 File Offset: 0x0001B384
	public EnvironmentController EnvironmentController
	{
		get
		{
			return this.environment;
		}
	}

	// Token: 0x040005E1 RID: 1505
	private EnvironmentController environment;

	// Token: 0x040005E2 RID: 1506
	private List<Transform> targets = new List<Transform>();

	// Token: 0x040005E3 RID: 1507
	private Direction[,] directionToSource;

	// Token: 0x040005E4 RID: 1508
	private IntVector2[] targetPosition = new IntVector2[0];

	// Token: 0x040005E5 RID: 1509
	private List<IntVector2> foundCellPositions = new List<IntVector2>();

	// Token: 0x040005E6 RID: 1510
	private IntVector2 size;

	// Token: 0x040005E7 RID: 1511
	private PathType pathType = PathType.Nav;

	// Token: 0x040005E8 RID: 1512
	private int[,] value;

	// Token: 0x040005E9 RID: 1513
	private bool pendingUpdate = true;

	// Token: 0x040005EA RID: 1514
	private Queue<IntVector2> _fillQueue = new Queue<IntVector2>();

	// Token: 0x040005EB RID: 1515
	private Queue<IntVector2> _backtrackQueue = new Queue<IntVector2>();
}
