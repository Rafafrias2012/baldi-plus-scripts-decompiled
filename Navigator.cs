using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x020000CA RID: 202
public class Navigator : MonoBehaviour
{
	// Token: 0x060004B3 RID: 1203 RVA: 0x00017618 File Offset: 0x00015818
	private void Awake()
	{
		this._navMeshPath = new NavMeshPath();
	}

	// Token: 0x060004B4 RID: 1204 RVA: 0x00017628 File Offset: 0x00015828
	public void Initialize(EnvironmentController ec)
	{
		this.ec = ec;
		if (this.entity != null)
		{
			this.entity.Initialize(ec, base.transform.position);
		}
		if (this.useHeatMap)
		{
			this.heatMapSize.x = ec.levelSize.x;
			this.heatMapSize.z = ec.levelSize.z;
			this.heatMap = new int[this.heatMapSize.x, this.heatMapSize.z];
		}
		this.expectedPosition = base.transform.position;
		this._startPos = base.transform.position;
	}

	// Token: 0x060004B5 RID: 1205 RVA: 0x000176D8 File Offset: 0x000158D8
	private void Update()
	{
		if (Time.timeScale > 0f && Time.deltaTime > 0f)
		{
			this._startPos = base.transform.position;
			if (this.expectedPosition != base.transform.position)
			{
				this.beenMoved = true;
			}
			else
			{
				this.beenMoved = false;
			}
			if (this.previousPosition.x != this.GetGridPosition(base.transform.localPosition).x || this.previousPosition.z != this.GetGridPosition(base.transform.localPosition).z)
			{
				if (this.realVelocity.magnitude > 0f)
				{
					Directions.DirsFromVector3(this.realVelocity, this.currentDirs);
				}
				if (this.currentTile != this.currentStartTile && this.beenMoved)
				{
					if (this.currentTargetTile != null)
					{
						this.FindPath(base.transform.position, this.currentTargetTile.FloorWorldPosition);
					}
					else
					{
						this.ClearDestination();
						this.npc.navigationStateMachine.DestinationEmpty();
					}
				}
				if (this.useHeatMap)
				{
					this.heatMap[this._gridPosition.x, this._gridPosition.z] = 0;
					this.UpdateHeatMap();
				}
				this.previousPosition = this._gridPosition;
			}
			if (!this.decelerate)
			{
				this.speed = Mathf.Min(this.speed + this.accel * Time.deltaTime * this.ec.NpcTimeScale, this.maxSpeed);
			}
			else
			{
				this.speed += Mathf.Clamp(this.accel * Time.deltaTime * this.ec.NpcTimeScale * Mathf.Sign(this.maxSpeed - this.speed), Mathf.Abs(this.maxSpeed - this.speed) * -1f, Mathf.Abs(this.maxSpeed - this.speed));
			}
			if (this.entity.Frozen)
			{
				this.expectedPosition = base.transform.position;
				return;
			}
			if (this.destinationPoints.Count > 0)
			{
				this.destination = this.destinationPoints[0];
				this._distanceToTravel = this.npc.DistanceCheck(this.speed * this.am.Multiplier * Time.deltaTime * this.ec.NpcTimeScale);
				while ((this.destination - base.transform.position).magnitude <= this._distanceToTravel && this.destinationPoints.Count > 0 && this.speed > 0f)
				{
					this._distanceToTravel -= (this.destination - base.transform.position).magnitude;
					base.transform.position = this.destination;
					this.destinationPoints.RemoveAt(0);
					if (this.destinationPoints.Count > 0)
					{
						this.destination = this.destinationPoints[0];
					}
					else
					{
						this.currentTargetTile = null;
						this.npc.navigationStateMachine.DestinationEmpty();
						bool flag = false;
						using (List<Vector3>.Enumerator enumerator = this.destinationPoints.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								if (enumerator.Current != base.transform.position)
								{
									flag = true;
								}
							}
						}
						if (!flag)
						{
							this.destinationPoints.Clear();
						}
						else
						{
							this.destination = this.destinationPoints[0];
						}
					}
				}
				this.velocity = (this.destination - base.transform.position).normalized * this._distanceToTravel;
				base.transform.position += this.velocity;
				this.expectedPosition = base.transform.position;
				return;
			}
			this.expectedPosition = base.transform.position;
			this.currentTargetTile = null;
			this.npc.navigationStateMachine.DestinationEmpty();
		}
	}

	// Token: 0x060004B6 RID: 1206 RVA: 0x00017B14 File Offset: 0x00015D14
	private void LateUpdate()
	{
		if (Time.timeScale > 0f && Time.deltaTime > 0f)
		{
			this.realVelocity = base.transform.position - this._startPos;
			if (this.autoRotate && this.realVelocity.magnitude != 0f)
			{
				base.transform.rotation = Quaternion.LookRotation(this.realVelocity.z * Vector3.forward + this.realVelocity.x * Vector3.right, Vector3.up);
			}
		}
	}

	// Token: 0x060004B7 RID: 1207 RVA: 0x00017BB8 File Offset: 0x00015DB8
	protected IntVector2 GetGridPosition(Vector3 position)
	{
		this._gridPosition.x = Mathf.FloorToInt(position.x / 10f);
		this._gridPosition.z = Mathf.FloorToInt(position.z / 10f);
		this.currentTile = this.ec.CellFromPosition(this._gridPosition);
		return this._gridPosition;
	}

	// Token: 0x060004B8 RID: 1208 RVA: 0x00017C1C File Offset: 0x00015E1C
	public void FindPath(Vector3 startPos, Vector3 targetPos, bool targeting)
	{
		targetPos.y = 5f + this.ec.Height;
		this._startTile = this.ec.CellFromPosition(this.GetGridPosition(startPos));
		this._targetTile = this.ec.CellFromPosition(this.GetGridPosition(targetPos));
		if (this._targetTile == this.currentTargetTile && (this._startTile == this.currentStartTile || !this.beenMoved) && !this.recalculatePath)
		{
			if (this.preciseTarget)
			{
				if (this.destinationPoints.Count > 0)
				{
					this.destinationPoints[this.destinationPoints.Count - 1] = targetPos;
					return;
				}
				this.destinationPoints.Add(targetPos);
				return;
			}
		}
		else
		{
			this.TempOpenObstacles();
			bool flag;
			this.ec.FindPath(this._startTile, this._targetTile, PathType.Nav, out this._path, out flag);
			this.TempCloseObstacles();
			if (flag)
			{
				this.ConvertPath(this._path, targetPos);
				if (targeting)
				{
					this.currentTargetTile = this._targetTile;
					this.currentStartTile = this._startTile;
					this.wandering = false;
					return;
				}
			}
			else
			{
				this.ClearDestination();
			}
		}
	}

	// Token: 0x060004B9 RID: 1209 RVA: 0x00017D44 File Offset: 0x00015F44
	public void CheckPath()
	{
		if (this.destinationPoints.Count > 0)
		{
			this.recalculatePath = true;
			this.FindPath(base.transform.position, this.destinationPoints[this.destinationPoints.Count - 1]);
			this.recalculatePath = false;
		}
	}

	// Token: 0x060004BA RID: 1210 RVA: 0x00017D96 File Offset: 0x00015F96
	public void FindPath(Vector3 targetPosition)
	{
		this.FindPath(base.transform.position, targetPosition);
	}

	// Token: 0x060004BB RID: 1211 RVA: 0x00017DAA File Offset: 0x00015FAA
	public void FindPath(Vector3 startPos, Vector3 targetPos)
	{
		this.FindPath(startPos, targetPos, true);
	}

	// Token: 0x060004BC RID: 1212 RVA: 0x00017DB8 File Offset: 0x00015FB8
	public void ConvertPath(List<Cell> path, Vector3 targetPos)
	{
		targetPos.y = 5f + this.ec.Height;
		Cell cell = null;
		Cell cell2 = null;
		Cell from = null;
		bool flag = false;
		this.destinationPoints.Clear();
		this.currentOpenEntryExit = new OpenGroupExit(null, Direction.Null);
		while (path.Count > 0)
		{
			if (path[0].open)
			{
				if (flag)
				{
					if (path[0].openTiles.Contains(cell2))
					{
						cell2 = path[0];
					}
					else
					{
						this.BuildNavPath(cell, cell2, targetPos);
						cell = path[0];
						cell2 = path[0];
						this.currentOpenEntryExit = cell.openTileGroup.GetEntrance(cell, from);
					}
				}
				else
				{
					flag = true;
					cell = path[0];
					cell2 = path[0];
					this.currentOpenEntryExit = cell.openTileGroup.GetEntrance(cell, from);
				}
				if (path.Count == 1)
				{
					this.BuildNavPath(cell, cell2, targetPos);
				}
			}
			else
			{
				if (flag)
				{
					flag = false;
					this.BuildNavPath(cell, cell2, targetPos);
				}
				this.destinationPoints.Add(path[0].FloorWorldPosition + Vector3.up * (5f + this.ec.Height));
				Debug.DrawRay(path[0].FloorWorldPosition, Vector3.up * 10f, Color.red, 5f);
			}
			from = path[0];
			path.RemoveAt(0);
		}
		if (this.destinationPoints.Count > 1 && (this.destinationPoints[1] - base.transform.position).magnitude < (this.destinationPoints[1] - this.destinationPoints[0]).magnitude)
		{
			this.destinationPoints.RemoveAt(0);
		}
		if (this.preciseTarget)
		{
			this.destinationPoints.Add(Vector3.right * targetPos.x + Vector3.up * (5f + this.ec.Height) + Vector3.forward * targetPos.z);
		}
		if (this.destinationPoints.Count > 2 && (this.destinationPoints[this.destinationPoints.Count - 3] - this.destinationPoints[this.destinationPoints.Count - 1]).magnitude < (this.destinationPoints[this.destinationPoints.Count - 3] - this.destinationPoints[this.destinationPoints.Count - 2]).magnitude)
		{
			this.destinationPoints.RemoveAt(this.destinationPoints.Count - 2);
		}
	}

	// Token: 0x060004BD RID: 1213 RVA: 0x0001808C File Offset: 0x0001628C
	protected void BuildNavPath(Cell firstOpenTile, Cell lastOpenTile, Vector3 targetPosition)
	{
		if (this.ec.CellFromPosition(base.transform.position) != firstOpenTile || !NavMesh.SamplePosition(base.transform.position.ZeroOutY(), out this._dummyHit, 1f, -1))
		{
			NavMesh.SamplePosition(firstOpenTile.FloorWorldPosition, out this._startHit, 10f, -1);
		}
		else
		{
			NavMesh.SamplePosition(base.transform.position.ZeroOutY(), out this._startHit, 10f, -1);
		}
		if (this.ec.CellFromPosition(targetPosition) != lastOpenTile || !this.preciseTarget || !NavMesh.SamplePosition(targetPosition.ZeroOutY(), out this._dummyHit, 1f, -1))
		{
			NavMesh.SamplePosition(lastOpenTile.FloorWorldPosition, out this._targetHit, 10f, -1);
		}
		else
		{
			NavMesh.SamplePosition(targetPosition.ZeroOutY(), out this._targetHit, 10f, -1);
		}
		NavMesh.CalculatePath(this._startHit.position, this._targetHit.position, -1, this._navMeshPath);
		foreach (Vector3 vector in this._navMeshPath.corners)
		{
			this.destinationPoints.Add(Vector3.zero + Vector3.right * vector.x + Vector3.forward * vector.z + Vector3.up * (5f + this.ec.Height));
			Debug.DrawRay(vector, Vector3.up * 10f, Color.green, 5f);
		}
	}

	// Token: 0x060004BE RID: 1214 RVA: 0x00018237 File Offset: 0x00016437
	public void ClearDestination()
	{
		this.destinationPoints.Clear();
		this.currentTargetTile = null;
		this.currentStartTile = null;
	}

	// Token: 0x060004BF RID: 1215 RVA: 0x00018252 File Offset: 0x00016452
	public void SkipCurrentDestinationPoint()
	{
		if (this.destinationPoints.Count > 1)
		{
			this.destinationPoints.RemoveAt(0);
		}
	}

	// Token: 0x060004C0 RID: 1216 RVA: 0x00018270 File Offset: 0x00016470
	public void FindPathAvoid(Vector3 target, Vector3 avoidee)
	{
		this._avoidPosition = IntVector2.GetGridPosition(avoidee);
		if (this.ec.CellFromPosition(this._avoidPosition) != null)
		{
			int navBin = this.ec.CellFromPosition(this._avoidPosition).NavBin;
			for (int i = 0; i < 4; i++)
			{
				if ((navBin & 1 << i) == 0)
				{
					this.ec.CellFromPosition(this._avoidPosition).SilentBlock((Direction)i, true);
				}
			}
			this.FindPath(base.transform.position, target);
			for (int j = 0; j < 4; j++)
			{
				if ((navBin & 1 << j) == 0)
				{
					this.ec.CellFromPosition(this._avoidPosition).SilentBlock((Direction)j, false);
				}
			}
		}
	}

	// Token: 0x060004C1 RID: 1217 RVA: 0x00018324 File Offset: 0x00016524
	public void ClearCurrentDirs()
	{
		this.currentDirs.Clear();
	}

	// Token: 0x060004C2 RID: 1218 RVA: 0x00018334 File Offset: 0x00016534
	public void WanderFlee(DijkstraMap dijkstraMap)
	{
		this.ClearDestination();
		this.currentStartTile = this.ec.CellFromPosition(this.GetGridPosition(base.transform.position));
		if (!dijkstraMap.PendingUpdate)
		{
			Directions.FillOpenDirectionsFromBin(this._potentialDirs, this.ec.CellFromPosition(this.GetGridPosition(base.transform.position)).NavBin);
			if (this.currentStartTile.open)
			{
				this.WanderFleeThroughOpenGroup(this.currentStartTile, dijkstraMap);
			}
			else
			{
				this.currentOpenEntryExit.cell = null;
				for (int i = 0; i < this._potentialDirs.Count; i++)
				{
					if (this.avoidRooms && this.currentStartTile.room.type != RoomType.Room && this.ec.CellFromPosition(this._gridPosition + this._potentialDirs[i].ToIntVector2()).room.type != RoomType.Hall)
					{
						this._potentialDirs.RemoveAt(i);
						i--;
					}
					else if (!this.currentStartTile.Null && this.ec.CellFromPosition(this._gridPosition + this._potentialDirs[i].ToIntVector2()).Null)
					{
						this._potentialDirs.RemoveAt(i);
						i--;
					}
				}
				this._potentialDirsCheck.Clear();
				this._potentialDirsCheck.AddRange(this._potentialDirs);
				foreach (Direction direction in this.currentDirs)
				{
					this._potentialDirsCheck.Remove(direction.GetOpposite());
				}
				if (this._potentialDirsCheck.Count > 0)
				{
					this._potentialDirs.Clear();
					this._potentialDirs.AddRange(this._potentialDirsCheck);
				}
				if (this._potentialDirs.Count > 0)
				{
					if (dijkstraMap.Value(this._gridPosition) < this.minSuboptimalFleeValue || Random.Range(0, dijkstraMap.Value(this._gridPosition)) <= this.minSuboptimalFleeValue)
					{
						this._potentialDirsCheck.Clear();
						this._potentialDirsCheck.AddRange(this._potentialDirs);
						int num = 0;
						foreach (Direction direction2 in this._potentialDirsCheck)
						{
							if (dijkstraMap.Value(this._gridPosition + direction2.ToIntVector2()) > num)
							{
								num = dijkstraMap.Value(this._gridPosition + direction2.ToIntVector2());
								this._potentialDirs.Clear();
							}
							if (dijkstraMap.Value(this._gridPosition + direction2.ToIntVector2()) == num)
							{
								this._potentialDirs.Add(direction2);
							}
						}
					}
					Direction direction3 = this._potentialDirs[Random.Range(0, this._potentialDirs.Count)];
					this._travelVector = direction3.ToIntVector2();
					this.destinationPoints.Add(this.ec.CellFromPosition(this._gridPosition + this._travelVector).CenterWorldPosition);
					this.currentOpenEntryExit.direction = direction3.GetOpposite();
					if (this._potentialDirs.Count > 1)
					{
						this.npc.MadeNavigationDecision();
					}
				}
			}
		}
		this.wandering = true;
	}

	// Token: 0x060004C3 RID: 1219 RVA: 0x000186B8 File Offset: 0x000168B8
	public void WanderRandom()
	{
		this.ClearDestination();
		this.currentStartTile = this.ec.CellFromPosition(this.GetGridPosition(base.transform.position));
		Directions.FillOpenDirectionsFromBin(this._potentialDirs, this.ec.CellFromPosition(this.GetGridPosition(base.transform.position)).NavBin);
		if (this.currentStartTile.open)
		{
			this.WanderRandomThroughOpenGroup(this.currentStartTile);
		}
		else
		{
			this.currentOpenEntryExit.cell = null;
			for (int i = 0; i < this._potentialDirs.Count; i++)
			{
				if (this.avoidRooms && this.currentStartTile.room.type != RoomType.Room && this.ec.CellFromPosition(this._gridPosition + this._potentialDirs[i].ToIntVector2()).room.type != RoomType.Hall)
				{
					this._potentialDirs.RemoveAt(i);
					i--;
				}
				else if (!this.currentStartTile.Null && this.ec.CellFromPosition(this._gridPosition + this._potentialDirs[i].ToIntVector2()).Null)
				{
					this._potentialDirs.RemoveAt(i);
					i--;
				}
			}
			this._potentialDirsCheck.Clear();
			this._potentialDirsCheck.AddRange(this._potentialDirs);
			foreach (Direction direction in this.currentDirs)
			{
				this._potentialDirsCheck.Remove(direction.GetOpposite());
			}
			if (this._potentialDirsCheck.Count > 0)
			{
				this._potentialDirs.Clear();
				this._potentialDirs.AddRange(this._potentialDirsCheck);
			}
			Direction item = this.CurrentRelativeDirection(0.1f);
			if (this._potentialDirs.Contains(item))
			{
				this._potentialDirs.Clear();
				this._potentialDirs.Add(item);
			}
			if (this._potentialDirs.Count > 0)
			{
				Direction direction2 = this._potentialDirs[Random.Range(0, this._potentialDirs.Count)];
				this._travelVector = direction2.ToIntVector2();
				this.destinationPoints.Add(this.ec.CellFromPosition(this._gridPosition + this._travelVector).CenterWorldPosition);
				this.currentOpenEntryExit.direction = direction2.GetOpposite();
				if (this._potentialDirs.Count > 1)
				{
					this.npc.MadeNavigationDecision();
				}
			}
		}
		this.wandering = true;
	}

	// Token: 0x060004C4 RID: 1220 RVA: 0x00018970 File Offset: 0x00016B70
	private Direction CurrentRelativeDirection(float buffer)
	{
		Vector2 vector = new Vector2(base.transform.position.x - this.currentStartTile.FloorWorldPosition.x, base.transform.position.z - this.currentStartTile.FloorWorldPosition.z) / 5f;
		if (Mathf.Abs(vector.y) > buffer && Mathf.Abs(vector.y) > Mathf.Abs(vector.x))
		{
			if (vector.y > 0f)
			{
				return Direction.North;
			}
			return Direction.South;
		}
		else
		{
			if (Mathf.Abs(vector.x) <= buffer)
			{
				return Direction.Null;
			}
			if (vector.x > 0f)
			{
				return Direction.East;
			}
			return Direction.West;
		}
	}

	// Token: 0x060004C5 RID: 1221 RVA: 0x00018A28 File Offset: 0x00016C28
	public void WanderRounds()
	{
		this.ClearDestination();
		int num = 0;
		IntVector2 gridPosition = this.GetGridPosition(base.transform.localPosition);
		this.currentStartTile = this.ec.CellFromPosition(this.GetGridPosition(base.transform.position));
		if (this.currentStartTile.open)
		{
			this.WanderRoundsThroughOpenGroup(this.currentStartTile);
		}
		else
		{
			this.currentOpenEntryExit.cell = null;
			Directions.FillOpenDirectionsFromBin(this._potentialDirs, this.ec.CellFromPosition(gridPosition).NavBin);
			foreach (Direction direction in this._potentialDirs)
			{
				if (gridPosition.x >= 0 && gridPosition.x < this.ec.levelSize.x && gridPosition.z >= 0 && gridPosition.z < this.ec.levelSize.z && this.heatMap[gridPosition.x + direction.ToIntVector2().x, gridPosition.z + direction.ToIntVector2().z] > num)
				{
					num = this.heatMap[gridPosition.x + direction.ToIntVector2().x, gridPosition.z + direction.ToIntVector2().z];
				}
			}
			for (int i = 0; i < this._potentialDirs.Count; i++)
			{
				if (this.heatMap[gridPosition.x + this._potentialDirs[i].ToIntVector2().x, gridPosition.z + this._potentialDirs[i].ToIntVector2().z] < num || (this.avoidRooms && this.ec.CellFromPosition(gridPosition + this._potentialDirs[i].ToIntVector2()).room.type == RoomType.Room && this.currentStartTile.room.type != RoomType.Room) || (!this.currentStartTile.Null && this.ec.CellFromPosition(gridPosition + this._potentialDirs[i].ToIntVector2()).Null))
				{
					this._potentialDirs.RemoveAt(i);
					i--;
				}
			}
			if (this._potentialDirs.Count > 0)
			{
				if (this._potentialDirs.Count > 1)
				{
					this.npc.MadeNavigationDecision();
				}
				Direction direction2 = this._potentialDirs[Random.Range(0, this._potentialDirs.Count)];
				this._travelVector = direction2.ToIntVector2();
				this.destinationPoints.Add(this.ec.CellFromPosition(this._gridPosition + this._travelVector).CenterWorldPosition);
				this.currentOpenEntryExit.direction = direction2.GetOpposite();
			}
		}
		this.wandering = true;
	}

	// Token: 0x060004C6 RID: 1222 RVA: 0x00018D44 File Offset: 0x00016F44
	protected void WanderRandomThroughOpenGroup(Cell startTile)
	{
		this.UpdatePotentialOpenExits(startTile);
		this.TargetOpenExit(startTile);
	}

	// Token: 0x060004C7 RID: 1223 RVA: 0x00018D54 File Offset: 0x00016F54
	protected void WanderRoundsThroughOpenGroup(Cell startTile)
	{
		this.UpdatePotentialOpenExits(startTile);
		this.GetHottestPotentialExit();
		this.TargetOpenExit(startTile);
	}

	// Token: 0x060004C8 RID: 1224 RVA: 0x00018D6C File Offset: 0x00016F6C
	protected void WanderFleeThroughOpenGroup(Cell startTile, DijkstraMap dijkstraMap)
	{
		this.UpdatePotentialOpenExits(startTile);
		if (dijkstraMap.Value(this._gridPosition) >= this.minSuboptimalFleeValue && Random.Range(0, dijkstraMap.Value(this._gridPosition)) > this.minSuboptimalFleeValue)
		{
			this.GetFurthestPotentialExits(dijkstraMap, true);
		}
		else
		{
			this.GetFurthestPotentialExits(dijkstraMap, false);
		}
		this.TargetOpenExit(startTile);
	}

	// Token: 0x060004C9 RID: 1225 RVA: 0x00018DC8 File Offset: 0x00016FC8
	private void UpdatePotentialOpenExits(Cell startTile)
	{
		this._potentialExits.Clear();
		this._potentialExits.AddRange(startTile.openTileGroup.exits);
		OpenGroupExit nearbyExit = startTile.openTileGroup.GetNearbyExit(startTile, base.transform.position);
		if (this.currentOpenEntryExit.cell == null)
		{
			this.currentOpenEntryExit.cell = startTile;
		}
		else if (nearbyExit.cell != null && nearbyExit.direction != Direction.Null && (nearbyExit.cell != this.currentOpenEntryExit.cell || nearbyExit.direction != this.currentOpenEntryExit.direction))
		{
			for (int i = 0; i < this._potentialExits.Count; i++)
			{
				if (this._potentialExits[i].cell != nearbyExit.cell || this._potentialExits[i].direction != nearbyExit.direction)
				{
					this._potentialExits.RemoveAt(i);
					i--;
				}
			}
		}
		bool flag = false;
		for (int j = 0; j < this._potentialExits.Count; j++)
		{
			OpenGroupExit openGroupExit = this._potentialExits[j];
			if (!openGroupExit.cell.Navigable(openGroupExit.direction, PathType.Nav))
			{
				this._potentialExits.RemoveAt(j);
				j--;
			}
			else if (openGroupExit.cell == this.currentOpenEntryExit.cell && (openGroupExit.direction == this.currentOpenEntryExit.direction || openGroupExit.direction == Direction.Null))
			{
				this._potentialExits.RemoveAt(j);
				j--;
			}
			else if (openGroupExit.OutputCell(this.ec).room.type != RoomType.Room)
			{
				flag = true;
			}
		}
		if (flag)
		{
			for (int k = 0; k < this._potentialExits.Count; k++)
			{
				if (this._potentialExits[k].OutputCell(this.ec).room.type == RoomType.Room && this.avoidRooms)
				{
					this._potentialExits.RemoveAt(k);
					k--;
				}
			}
		}
	}

	// Token: 0x060004CA RID: 1226 RVA: 0x00018FD4 File Offset: 0x000171D4
	private void GetHottestPotentialExit()
	{
		int num = 0;
		this._tempExits.Clear();
		for (int i = 0; i < this._potentialExits.Count; i++)
		{
			if (this.heatMap[this._potentialExits[i].OutputCell(this.ec).position.x, this._potentialExits[i].OutputCell(this.ec).position.z] > num)
			{
				num = this.heatMap[this._potentialExits[i].OutputCell(this.ec).position.x, this._potentialExits[i].OutputCell(this.ec).position.z];
				this._tempExits.Clear();
				this._tempExits.Add(this._potentialExits[i]);
			}
			else if (this.heatMap[this._potentialExits[i].OutputCell(this.ec).position.x, this._potentialExits[i].OutputCell(this.ec).position.z] == num)
			{
				this._tempExits.Add(this._potentialExits[i]);
			}
		}
		this._potentialExits.Clear();
		this._potentialExits.AddRange(this._tempExits);
	}

	// Token: 0x060004CB RID: 1227 RVA: 0x00019164 File Offset: 0x00017364
	private void GetFurthestPotentialExits(DijkstraMap dijkstraMap, bool suboptimal)
	{
		int num = 0;
		this._tempExits.Clear();
		for (int i = 0; i < this._potentialExits.Count; i++)
		{
			if (suboptimal)
			{
				if (dijkstraMap.Value(this._potentialExits[i].OutputCell(this.ec).position) >= this.minSuboptimalFleeValue)
				{
					this._tempExits.Add(this._potentialExits[i]);
				}
			}
			else
			{
				if (dijkstraMap.Value(this._potentialExits[i].OutputCell(this.ec).position) > num)
				{
					num = dijkstraMap.Value(this._potentialExits[i].OutputCell(this.ec).position);
					this._tempExits.Add(this._potentialExits[i]);
				}
				if (dijkstraMap.Value(this._potentialExits[i].OutputCell(this.ec).position) == num)
				{
					this._tempExits.Add(this._potentialExits[i]);
				}
			}
		}
		this._potentialExits.Clear();
		this._potentialExits.AddRange(this._tempExits);
	}

	// Token: 0x060004CC RID: 1228 RVA: 0x000192AC File Offset: 0x000174AC
	private void TargetOpenExit(Cell startTile)
	{
		if (this._potentialExits.Count > 0)
		{
			this.FindPath(base.transform.position, this._potentialExits[Random.Range(0, this._potentialExits.Count)].OutputCell(this.ec).FloorWorldPosition, false);
		}
		else
		{
			this.FindPath(base.transform.position, startTile.openTileGroup.randomOpenTile(startTile).FloorWorldPosition, false);
		}
		this.wandering = true;
	}

	// Token: 0x060004CD RID: 1229 RVA: 0x00019334 File Offset: 0x00017534
	private void TempOpenObstacles()
	{
		foreach (PassableObstacle passableObstacle in this.passableObstacles)
		{
			if (passableObstacle != PassableObstacle.Window)
			{
				if (passableObstacle == PassableObstacle.Bully)
				{
					this.ec.TempOpenBully();
				}
			}
			else
			{
				this.ec.TempOpenWindows();
			}
		}
	}

	// Token: 0x060004CE RID: 1230 RVA: 0x000193A4 File Offset: 0x000175A4
	private void TempCloseObstacles()
	{
		foreach (PassableObstacle passableObstacle in this.passableObstacles)
		{
			if (passableObstacle != PassableObstacle.Window)
			{
				if (passableObstacle == PassableObstacle.Bully)
				{
					this.ec.TempCloseBully();
				}
			}
			else
			{
				this.ec.TempCloseWindows();
			}
		}
	}

	// Token: 0x060004CF RID: 1231 RVA: 0x00019414 File Offset: 0x00017614
	public void SetSpeed(float val)
	{
		this.speed = val;
		if (this.maxSpeed < val)
		{
			this.maxSpeed = val;
		}
	}

	// Token: 0x060004D0 RID: 1232 RVA: 0x0001942D File Offset: 0x0001762D
	public void SetRoomAvoidance(bool val)
	{
		this.avoidRooms = val;
	}

	// Token: 0x060004D1 RID: 1233 RVA: 0x00019436 File Offset: 0x00017636
	public void ClearRemainingDistanceThisFrame()
	{
		this._distanceToTravel = 0f;
	}

	// Token: 0x060004D2 RID: 1234 RVA: 0x00019443 File Offset: 0x00017643
	public void DebugPath()
	{
		this.FindPath(base.transform.position, Singleton<CoreGameManager>.Instance.GetPlayer(0).transform.position);
	}

	// Token: 0x1700004B RID: 75
	// (get) Token: 0x060004D3 RID: 1235 RVA: 0x0001946B File Offset: 0x0001766B
	public bool HasDestination
	{
		get
		{
			return this.destinationPoints.Count > 0;
		}
	}

	// Token: 0x1700004C RID: 76
	// (get) Token: 0x060004D4 RID: 1236 RVA: 0x0001947B File Offset: 0x0001767B
	public Vector3 NextPoint
	{
		get
		{
			if (this.destinationPoints.Count > 0)
			{
				return this.destinationPoints[0];
			}
			return base.transform.position;
		}
	}

	// Token: 0x1700004D RID: 77
	// (get) Token: 0x060004D5 RID: 1237 RVA: 0x000194A3 File Offset: 0x000176A3
	public Vector3 CurrentDestination
	{
		get
		{
			if (this.destinationPoints.Count > 0)
			{
				return this.destinationPoints[this.destinationPoints.Count - 1];
			}
			return Vector3.zero;
		}
	}

	// Token: 0x060004D6 RID: 1238 RVA: 0x000194D1 File Offset: 0x000176D1
	public List<Vector3> NewListOfDestinationPoints()
	{
		return new List<Vector3>(this.destinationPoints);
	}

	// Token: 0x060004D7 RID: 1239 RVA: 0x000194E0 File Offset: 0x000176E0
	protected void UpdateHeatMap()
	{
		for (int i = 0; i < this.heatMapSize.x; i++)
		{
			for (int j = 0; j < this.heatMapSize.z; j++)
			{
				this.heatMap[i, j]++;
			}
		}
	}

	// Token: 0x1700004E RID: 78
	// (get) Token: 0x060004D8 RID: 1240 RVA: 0x0001952B File Offset: 0x0001772B
	public Entity Entity
	{
		get
		{
			return this.entity;
		}
	}

	// Token: 0x1700004F RID: 79
	// (get) Token: 0x060004D9 RID: 1241 RVA: 0x00019533 File Offset: 0x00017733
	public Vector3 Velocity
	{
		get
		{
			return this.realVelocity;
		}
	}

	// Token: 0x17000050 RID: 80
	// (get) Token: 0x060004DA RID: 1242 RVA: 0x0001953C File Offset: 0x0001773C
	public float Acceleration
	{
		get
		{
			if (!this.decelerate)
			{
				return this.accel * Time.deltaTime * this.ec.NpcTimeScale;
			}
			return this.accel * Time.deltaTime * this.ec.NpcTimeScale * Mathf.Sign(this.maxSpeed - this.speed);
		}
	}

	// Token: 0x17000051 RID: 81
	// (get) Token: 0x060004DB RID: 1243 RVA: 0x00019595 File Offset: 0x00017795
	public float Radius
	{
		get
		{
			return this.radius;
		}
	}

	// Token: 0x17000052 RID: 82
	// (get) Token: 0x060004DC RID: 1244 RVA: 0x0001959D File Offset: 0x0001779D
	public ActivityModifier Am
	{
		get
		{
			return this.am;
		}
	}

	// Token: 0x17000053 RID: 83
	// (get) Token: 0x060004DD RID: 1245 RVA: 0x000195A5 File Offset: 0x000177A5
	public float Speed
	{
		get
		{
			return this.speed;
		}
	}

	// Token: 0x17000054 RID: 84
	// (get) Token: 0x060004DE RID: 1246 RVA: 0x000195AD File Offset: 0x000177AD
	public bool Wandering
	{
		get
		{
			return this.wandering;
		}
	}

	// Token: 0x040004F6 RID: 1270
	public EnvironmentController ec;

	// Token: 0x040004F7 RID: 1271
	public NPC npc;

	// Token: 0x040004F8 RID: 1272
	[SerializeField]
	protected Entity entity;

	// Token: 0x040004F9 RID: 1273
	[SerializeField]
	protected Collider collider;

	// Token: 0x040004FA RID: 1274
	[SerializeField]
	protected ActivityModifier am;

	// Token: 0x040004FB RID: 1275
	public Cell _startTile;

	// Token: 0x040004FC RID: 1276
	public Cell _targetTile;

	// Token: 0x040004FD RID: 1277
	protected NavMeshPath _navMeshPath;

	// Token: 0x040004FE RID: 1278
	protected NavMeshHit _startHit;

	// Token: 0x040004FF RID: 1279
	protected NavMeshHit _targetHit;

	// Token: 0x04000500 RID: 1280
	protected NavMeshHit _dummyHit;

	// Token: 0x04000501 RID: 1281
	[SerializeField]
	protected float radius = 2f;

	// Token: 0x04000502 RID: 1282
	public float maxSpeed = 15f;

	// Token: 0x04000503 RID: 1283
	public float accel = 15f;

	// Token: 0x04000504 RID: 1284
	public float speed;

	// Token: 0x04000505 RID: 1285
	public float _distanceToTravel;

	// Token: 0x04000506 RID: 1286
	protected int[,] heatMap;

	// Token: 0x04000507 RID: 1287
	[SerializeField]
	private int minSuboptimalFleeValue = 25;

	// Token: 0x04000508 RID: 1288
	protected IntVector2 heatMapSize;

	// Token: 0x04000509 RID: 1289
	protected Cell currentTargetTile;

	// Token: 0x0400050A RID: 1290
	protected Cell currentStartTile;

	// Token: 0x0400050B RID: 1291
	protected Cell currentTile;

	// Token: 0x0400050C RID: 1292
	protected OpenGroupExit currentOpenEntryExit;

	// Token: 0x0400050D RID: 1293
	protected bool beenMoved;

	// Token: 0x0400050E RID: 1294
	[SerializeField]
	protected bool avoidRooms;

	// Token: 0x0400050F RID: 1295
	[SerializeField]
	protected bool useHeatMap;

	// Token: 0x04000510 RID: 1296
	[SerializeField]
	protected bool preciseTarget = true;

	// Token: 0x04000511 RID: 1297
	[SerializeField]
	protected bool autoRotate = true;

	// Token: 0x04000512 RID: 1298
	[SerializeField]
	protected bool decelerate;

	// Token: 0x04000513 RID: 1299
	protected bool recalculatePath;

	// Token: 0x04000514 RID: 1300
	protected bool wandering;

	// Token: 0x04000515 RID: 1301
	protected Vector3 destination;

	// Token: 0x04000516 RID: 1302
	protected Vector3 velocity;

	// Token: 0x04000517 RID: 1303
	protected Vector3 realVelocity;

	// Token: 0x04000518 RID: 1304
	private Vector3 _startPos;

	// Token: 0x04000519 RID: 1305
	protected List<Vector3> destinationPoints = new List<Vector3>();

	// Token: 0x0400051A RID: 1306
	public List<PassableObstacle> passableObstacles = new List<PassableObstacle>();

	// Token: 0x0400051B RID: 1307
	protected IntVector2 _gridPosition;

	// Token: 0x0400051C RID: 1308
	protected IntVector2 previousPosition;

	// Token: 0x0400051D RID: 1309
	protected IntVector2 _avoidPosition;

	// Token: 0x0400051E RID: 1310
	protected Vector3 expectedPosition;

	// Token: 0x0400051F RID: 1311
	protected IntVector2 _travelVector;

	// Token: 0x04000520 RID: 1312
	public List<Direction> currentDirs = new List<Direction>();

	// Token: 0x04000521 RID: 1313
	protected List<Direction> _potentialDirs = new List<Direction>();

	// Token: 0x04000522 RID: 1314
	protected List<Direction> _potentialDirsCheck = new List<Direction>();

	// Token: 0x04000523 RID: 1315
	protected List<Direction> _exitCheckDirs = new List<Direction>();

	// Token: 0x04000524 RID: 1316
	protected List<Cell> _path = new List<Cell>();

	// Token: 0x04000525 RID: 1317
	protected List<OpenGroupExit> _potentialExits = new List<OpenGroupExit>();

	// Token: 0x04000526 RID: 1318
	protected List<OpenGroupExit> _tempExits = new List<OpenGroupExit>();
}
