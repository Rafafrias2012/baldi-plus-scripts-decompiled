using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x0200016C RID: 364
public class EnvironmentController : MonoBehaviour
{
	// Token: 0x06000830 RID: 2096 RVA: 0x000295D1 File Offset: 0x000277D1
	private void Awake()
	{
		this.pauseScale.npcTimeScale = 0f;
		this.pauseScale.environmentTimeScale = 0f;
		this._tilePropertyBlock = new MaterialPropertyBlock();
	}

	// Token: 0x06000831 RID: 2097 RVA: 0x00029600 File Offset: 0x00027800
	private void Update()
	{
		this.surpassedGameTime += Time.deltaTime * this.EnvironmentTimeScale;
		this.surpassedRealTime += Time.unscaledDeltaTime;
		if (this.updateNavigation)
		{
			this.updateNavigation = false;
			for (int i = 0; i < this.npcs.Count; i++)
			{
				if (this.npcs[i].Navigator.enabled)
				{
					this.npcs[i].Navigator.CheckPath();
				}
			}
			foreach (DijkstraMap item in this.activeDijkstraMaps)
			{
				this.dijkstraMapsToUpdate.Enqueue(item);
			}
		}
		foreach (DijkstraMap dijkstraMap in this.activeDijkstraMaps)
		{
			if (dijkstraMap.UpdateIsNeeded() && !this.dijkstraMapsToUpdate.Contains(dijkstraMap))
			{
				this.dijkstraMapsToUpdate.Enqueue(dijkstraMap);
			}
		}
		if (this.dijkstraMapsToUpdate.Count > 0)
		{
			this.dijkstraMapsToUpdate.Dequeue().Calculate();
		}
		if (this.flickerLights)
		{
			if (this.flickerDelay <= 0f)
			{
				if (this.lightsToFlicker.Count > 0)
				{
					int index = Random.Range(0, this.lightsToFlicker.Count);
					this.lightsToFlicker[index].Flicker();
				}
				this.flickerDelay = this.flickerSpeed;
				return;
			}
			this.flickerDelay -= Time.deltaTime * this.EnvironmentTimeScale;
		}
	}

	// Token: 0x06000832 RID: 2098 RVA: 0x000297C4 File Offset: 0x000279C4
	private void Start()
	{
		this.UpdateFog();
	}

	// Token: 0x06000833 RID: 2099 RVA: 0x000297CC File Offset: 0x000279CC
	public void BeginPlay()
	{
		if (this.OnEnvironmentBeginPlay != null)
		{
			this.OnEnvironmentBeginPlay();
		}
	}

	// Token: 0x06000834 RID: 2100 RVA: 0x000297E4 File Offset: 0x000279E4
	public void InitializeCells(IntVector2 levelSize)
	{
		this.cells = new Cell[levelSize.x, levelSize.z];
		for (int i = 0; i < levelSize.x; i++)
		{
			for (int j = 0; j < levelSize.z; j++)
			{
				this.cells[i, j] = new Cell();
				this.cells[i, j].position = new IntVector2(i, j);
				this.cells[i, j].room = this.nullRoom;
			}
		}
	}

	// Token: 0x06000835 RID: 2101 RVA: 0x0002986D File Offset: 0x00027A6D
	public void InitializeHeap()
	{
		this._openSet = new Heap<Cell>(this.levelSize.x * this.levelSize.z);
	}

	// Token: 0x06000836 RID: 2102 RVA: 0x00029891 File Offset: 0x00027A91
	public void SetSpawn(Vector3 position, Quaternion rotation)
	{
		this.spawnPoint = position + Vector3.up * 5f;
		this.spawnRotation = rotation;
	}

	// Token: 0x06000837 RID: 2103 RVA: 0x000298B5 File Offset: 0x00027AB5
	public void BuildNavMesh()
	{
		this.nms.BuildNavMesh();
	}

	// Token: 0x06000838 RID: 2104 RVA: 0x000298C2 File Offset: 0x00027AC2
	public void SetTileInstantiation(bool instantiate)
	{
		this.instantiateTiles = instantiate;
	}

	// Token: 0x06000839 RID: 2105 RVA: 0x000298CB File Offset: 0x00027ACB
	public void CreateCell(int tileBin, Transform parent, IntVector2 position, RoomController room)
	{
		this.CreateCell(tileBin, parent, position, room, false, false);
	}

	// Token: 0x0600083A RID: 2106 RVA: 0x000298DC File Offset: 0x00027ADC
	public void CreateCell(int tileBin, Transform parent, IntVector2 position, RoomController room, bool destroyExisting, bool lockCell)
	{
		bool flag = false;
		int chunkId = 0;
		if (!this.cells[position.x, position.z].Null)
		{
			if (this.cells[position.x, position.z].HasChunk)
			{
				flag = true;
				chunkId = this.cells[position.x, position.z].Chunk.Id;
			}
			if (!destroyExisting)
			{
				this.SwapCell(position, room, tileBin);
				if (flag)
				{
					this.cullingManager.UpdateChunk(chunkId);
				}
				return;
			}
			this.DestroyCell(this.cells[position.x, position.z]);
		}
		this.cells[position.x, position.z].Initialize();
		Cell cell = this.cells[position.x, position.z];
		cell.SetShape(tileBin, this.tileShape[tileBin]);
		cell.position = position;
		cell.room = room;
		cell.room.AddTile(cell);
		cell.locked = lockCell;
		if (this.instantiateTiles)
		{
			cell.LoadTile();
		}
		cell.SetBase(room.baseMat);
		if (flag)
		{
			this.cullingManager.UpdateChunk(chunkId);
		}
	}

	// Token: 0x0600083B RID: 2107 RVA: 0x00029A1A File Offset: 0x00027C1A
	public void DestroyCell(Cell cell)
	{
		cell.Uninitialize();
		cell.room.RemoveTile(cell);
		cell.room = this.nullRoom;
	}

	// Token: 0x0600083C RID: 2108 RVA: 0x00029A3C File Offset: 0x00027C3C
	public void SwapCell(IntVector2 position, RoomController room, int tileType)
	{
		if (room != this.cells[position.x, position.z].room)
		{
			this.cells[position.x, position.z].room.RemoveTile(this.cells[position.x, position.z]);
			this.cells[position.x, position.z].room = room;
			room.AddTile(this.cells[position.x, position.z]);
			this.cells[position.x, position.z].SetBase(room.baseMat);
		}
		this.cells[position.x, position.z].SetShape(tileType, this.tileShape[tileType]);
		this.cells[position.x, position.z].RefreshNavBin();
		this.lightMap[position.x, position.z].RegenerateLightSources();
	}

	// Token: 0x0600083D RID: 2109 RVA: 0x00029B64 File Offset: 0x00027D64
	public void UpdateCell(IntVector2 position)
	{
		int num = 0;
		Cell cell = this.cells[position.x, position.z];
		List<Direction> list = new List<Direction>();
		if (!cell.locked)
		{
			List<Direction> list2 = new List<Direction>
			{
				Direction.North,
				Direction.East,
				Direction.South,
				Direction.West
			};
			RoomController room = this.cells[position.x, position.z].room;
			for (int i = 0; i < list2.Count; i++)
			{
				IntVector2 intVector = list2[i].ToIntVector2();
				IntVector2 intVector2 = new IntVector2(position.x + intVector.x, position.z + intVector.z);
				if (this.ContainsCoordinates(intVector2) && !this.cells[intVector2.x, intVector2.z].Null)
				{
					RoomController room2 = this.cells[intVector2.x, intVector2.z].room;
					Cell cell2 = this.cells[intVector2.x, intVector2.z];
					if ((room != room2 && (!cell.doorHere || !cell.doorDirs.Contains(list2[i]))) || (Directions.ClosedDirectionsFromBin(cell2.ConstBin).Contains(list2[i].GetOpposite()) && (!cell.doorHere || !cell.doorDirs.Contains(list2[i]))) || cell2.ConstBin == 16)
					{
						num += 1 << i;
					}
					else if (cell.doorHere && cell.doorDirs.Contains(list2[i]) && !cell.doorDirsSpace.Contains(list2[i]))
					{
						list.Add(list2[i]);
					}
				}
				else
				{
					num += 1 << i;
				}
			}
			this.SwapCell(position, this.cells[position.x, position.z].room, num);
		}
	}

	// Token: 0x0600083E RID: 2110 RVA: 0x00029D80 File Offset: 0x00027F80
	public void RefreshCell(IntVector2 position)
	{
		if (this.instantiateTiles)
		{
			this.CellFromPosition(position).LoadTile();
		}
		this.CellFromPosition(position).SetBase(this.CellFromPosition(position).room.baseMat);
	}

	// Token: 0x0600083F RID: 2111 RVA: 0x00029DB4 File Offset: 0x00027FB4
	public void ConnectCells(IntVector2 position, Direction dir)
	{
		Cell cell = this.CellFromPosition(position);
		Cell cell2 = this.CellFromPosition(position + dir.ToIntVector2());
		if (cell.ConstBin.ContainsDirection(dir))
		{
			this.CreateCell(cell.ConstBin - (1 << (int)dir), cell.room.transform, cell.position, cell.room);
		}
		cell = this.CellFromPosition(position);
		if (this.ContainsCoordinates(position + dir.ToIntVector2()))
		{
			if (cell2.ConstBin.ContainsDirection(dir.GetOpposite()))
			{
				this.CreateCell(cell2.ConstBin - (1 << (int)dir.GetOpposite()), cell2.room.transform, cell2.position, cell2.room);
			}
			cell2 = this.CellFromPosition(position + dir.ToIntVector2());
			return;
		}
		Debug.LogWarning(string.Concat(new string[]
		{
			"Failed to connect tile at ",
			position.x.ToString(),
			",",
			position.z.ToString(),
			" to the tile ",
			dir.ToString(),
			" of it because that is out of bounds. Target tile was still opened in the desired direction."
		}));
	}

	// Token: 0x06000840 RID: 2112 RVA: 0x00029EE4 File Offset: 0x000280E4
	public void CloseCell(IntVector2 position, Direction dir)
	{
		Cell cell = this.CellFromPosition(position);
		if (!cell.ConstBin.ContainsDirection(dir))
		{
			this.CreateCell(cell.ConstBin + (1 << (int)dir), cell.room.transform, cell.position, cell.room);
		}
	}

	// Token: 0x06000841 RID: 2113 RVA: 0x00029F34 File Offset: 0x00028134
	public void CloseSurroundingCells(IntVector2 position)
	{
		foreach (Direction direction in Directions.All())
		{
			IntVector2 intVector = position + direction.ToIntVector2();
			if (this.ContainsCoordinates(intVector) && !this.cells[intVector.x, intVector.z].Null)
			{
				this.CloseCell(intVector, direction.GetOpposite());
			}
		}
	}

	// Token: 0x06000842 RID: 2114 RVA: 0x00029FC0 File Offset: 0x000281C0
	public bool BuildPoster(PosterObject poster, Cell tile, Direction dir)
	{
		return this.BuildPoster(poster, tile, dir, true);
	}

	// Token: 0x06000843 RID: 2115 RVA: 0x00029FCC File Offset: 0x000281CC
	public bool BuildPoster(PosterObject poster, Cell tile, Direction dir, bool allowMultiPoster)
	{
		if (allowMultiPoster && poster.multiPosterArray.Length > 1)
		{
			return this.BuildMultiPoster(poster, tile, dir);
		}
		if (!(poster.baseTexture != null))
		{
			Debug.Log("Poster " + poster.name + " using outdated material. Update to use texture.");
			tile.AddPoster(dir, poster.GetMaterial(0).mainTexture);
			return false;
		}
		if (poster.textData.Length != 0)
		{
			for (int i = 0; i < this.loadedPosters.Count; i++)
			{
				if (this.loadedPosters[i] == poster)
				{
					tile.AddPoster(dir, this.posterTextures[i]);
					return false;
				}
			}
			this.posterTextures.Add(this.TextTextureGenerator.GenerateTextTexture(poster));
			this.loadedPosters.Add(poster);
			tile.AddPoster(dir, this.posterTextures[this.posterTextures.Count - 1]);
			return true;
		}
		tile.AddPoster(dir, poster.baseTexture);
		return false;
	}

	// Token: 0x06000844 RID: 2116 RVA: 0x0002A0D0 File Offset: 0x000282D0
	public bool BuildPoster(PosterObject poster, Cell tile, Direction dir, Random rng)
	{
		if (poster.multiPosterArray.Length > 1)
		{
			return this.BuildMultiPoster(poster, tile, dir, rng);
		}
		if (!(poster.baseTexture != null))
		{
			Debug.Log("Poster " + poster.name + " using outdated material. Update to use texture.");
			tile.AddPoster(dir, poster.GetMaterial(0).mainTexture);
			return false;
		}
		if (poster.textData.Length != 0)
		{
			for (int i = 0; i < this.loadedPosters.Count; i++)
			{
				if (this.loadedPosters[i] == poster)
				{
					tile.AddPoster(dir, this.posterTextures[i]);
					return false;
				}
			}
			this.posterTextures.Add(this.TextTextureGenerator.GenerateTextTexture(poster));
			this.loadedPosters.Add(poster);
			tile.AddPoster(dir, this.posterTextures[this.posterTextures.Count - 1]);
			return true;
		}
		tile.AddPoster(dir, poster.baseTexture);
		return false;
	}

	// Token: 0x06000845 RID: 2117 RVA: 0x0002A1D4 File Offset: 0x000283D4
	public void BuildPosterInRoom(RoomController room, PosterObject poster, Random rng)
	{
		List<Cell> list = new List<Cell>();
		foreach (Cell cell in room.cells)
		{
			if (cell.HasFreeWall)
			{
				list.Add(cell);
			}
		}
		if (list.Count > 0)
		{
			Cell cell2 = list[rng.Next(0, list.Count)];
			Direction dir = cell2.RandomUncoveredDirection(rng);
			this.BuildPoster(poster, cell2, dir, rng);
		}
	}

	// Token: 0x06000846 RID: 2118 RVA: 0x0002A268 File Offset: 0x00028468
	public List<Cell> TilesLeftToRight(Cell tile, Direction dir)
	{
		Direction direction = Direction.Null;
		Direction direction2 = Direction.Null;
		switch (dir)
		{
		case Direction.North:
			direction = Direction.West;
			direction2 = Direction.East;
			break;
		case Direction.East:
			direction = Direction.North;
			direction2 = Direction.South;
			break;
		case Direction.South:
			direction = Direction.East;
			direction2 = Direction.West;
			break;
		case Direction.West:
			direction = Direction.South;
			direction2 = Direction.North;
			break;
		}
		IntVector2 intVector = tile.position;
		Cell cell = this.cells[intVector.x, intVector.z];
		Cell cell2 = cell;
		while (!cell.Null && cell.room == tile.room && !cell.WallHardCovered(dir))
		{
			cell2 = cell;
			if (!Directions.OpenDirectionsFromBin(cell.ConstBin).Contains(direction))
			{
				break;
			}
			intVector += direction.ToIntVector2();
			cell = this.cells[intVector.x, intVector.z];
		}
		List<Cell> list = new List<Cell>();
		cell = cell2;
		intVector = cell2.position;
		while (!cell.Null && cell.room == tile.room && !cell.WallHardCovered(dir))
		{
			list.Add(cell);
			if (!Directions.OpenDirectionsFromBin(cell.ConstBin).Contains(direction2))
			{
				break;
			}
			intVector += direction2.ToIntVector2();
			cell = this.cells[intVector.x, intVector.z];
		}
		return list;
	}

	// Token: 0x06000847 RID: 2119 RVA: 0x0002A3A4 File Offset: 0x000285A4
	protected bool BuildMultiPoster(PosterObject poster, Cell tile, Direction dir)
	{
		bool flag = false;
		List<Cell> list = this.TilesLeftToRight(tile, dir);
		if (poster.multiPosterArray.Length <= list.Count)
		{
			int count = list.Count;
			int num = poster.multiPosterArray.Length;
			int num2 = 0;
			for (int i = 0; i < list.Count; i++)
			{
				if (tile == list[i])
				{
					num2 = i;
					break;
				}
			}
			int num3 = 0;
			while (num3 < poster.multiPosterArray.Length && num3 + num2 < list.Count)
			{
				flag = (this.BuildPoster(poster.multiPosterArray[num3], list[num3 + num2], dir, false) || flag);
				num3++;
			}
		}
		return flag;
	}

	// Token: 0x06000848 RID: 2120 RVA: 0x0002A440 File Offset: 0x00028640
	protected bool BuildMultiPoster(PosterObject poster, Cell tile, Direction dir, Random rng)
	{
		bool flag = false;
		List<Cell> list = this.TilesLeftToRight(tile, dir);
		if (poster.multiPosterArray.Length <= list.Count)
		{
			int maxValue = list.Count - poster.multiPosterArray.Length + 1;
			int num = rng.Next(0, maxValue);
			int num2 = 0;
			while (num2 < poster.multiPosterArray.Length && num2 + num < list.Count)
			{
				flag = (this.BuildPoster(poster.multiPosterArray[num2], list[num2 + num], dir, false) || flag);
				num2++;
			}
		}
		return flag;
	}

	// Token: 0x06000849 RID: 2121 RVA: 0x0002A4C6 File Offset: 0x000286C6
	public void BuildWindow(Cell tile, Direction dir, WindowObject wObject)
	{
		this.BuildWindow(tile, dir, wObject, false);
	}

	// Token: 0x0600084A RID: 2122 RVA: 0x0002A4D4 File Offset: 0x000286D4
	public void BuildWindow(Cell tile, Direction dir, WindowObject wObject, bool editorMode)
	{
		if (this.ContainsCoordinates(tile.position + dir.ToIntVector2()))
		{
			Cell cell = this.CellFromPosition(tile.position + dir.ToIntVector2());
			if (!cell.Null && Directions.ClosedDirectionsFromBin(cell.ConstBin).Contains(dir.GetOpposite()) && cell.ConstBin != 16)
			{
				IntVector2 position = tile.position;
				Window window = Object.Instantiate<Window>(wObject.windowPre, tile.room.transform);
				if (!editorMode)
				{
					this.ConnectCells(tile.position, dir);
					Cell cell2 = this.CellFromPosition(position);
					window.Initialize(this, tile.position, dir, wObject);
					cell2.HardCoverWall(dir, true);
					cell = this.CellFromPosition(tile.position + dir.ToIntVector2());
					cell.HardCoverWall(dir.GetOpposite(), true);
					return;
				}
				window.transform.position = tile.FloorWorldPosition;
				window.transform.rotation = dir.ToRotation();
			}
		}
	}

	// Token: 0x0600084B RID: 2123 RVA: 0x0002A5DC File Offset: 0x000287DC
	public bool ContainsCoordinates(int x, int z)
	{
		return x >= 0 && x < this.levelSize.x && z >= 0 && z < this.levelSize.z;
	}

	// Token: 0x0600084C RID: 2124 RVA: 0x0002A604 File Offset: 0x00028804
	public bool ContainsCoordinates(IntVector2 coordinate)
	{
		return this.ContainsCoordinates(coordinate.x, coordinate.z);
	}

	// Token: 0x0600084D RID: 2125 RVA: 0x0002A618 File Offset: 0x00028818
	public bool ContainsCoordinates(Vector3 coordinate)
	{
		return this.ContainsCoordinates(IntVector2.GetGridPosition(coordinate));
	}

	// Token: 0x0600084E RID: 2126 RVA: 0x0002A628 File Offset: 0x00028828
	public void FindPath(Cell startTile, Cell targetTile, PathType pathType, out List<Cell> path, out bool success)
	{
		this._openSet.Clear();
		this._closedSet.Clear();
		this._openSet.Add(startTile);
		if (!startTile.Null && targetTile.Null)
		{
			success = false;
			path = null;
			return;
		}
		while (this._openSet.Count > 0)
		{
			this._currentTile = this._openSet.RemoveFirst();
			this._closedSet.Add(this._currentTile);
			if (this._currentTile == targetTile)
			{
				path = this.GetPath(startTile, targetTile);
				success = true;
				return;
			}
			this.GetNavNeighbors(this._currentTile, this._neighbors, pathType);
			foreach (Cell cell in this._neighbors)
			{
				if (!this._closedSet.Contains(cell))
				{
					int num = this._currentTile.gCost + this.GetDistance(this._currentTile, cell);
					if (num < cell.gCost || !this._openSet.Contains(cell))
					{
						cell.gCost = num;
						cell.hCost = this.GetDistance(cell, targetTile);
						cell.parent = this._currentTile;
						if (!this._openSet.Contains(cell))
						{
							this._openSet.Add(cell);
						}
					}
				}
			}
		}
		path = null;
		success = false;
	}

	// Token: 0x0600084F RID: 2127 RVA: 0x0002A7A0 File Offset: 0x000289A0
	public int NavigableDistance(Cell start, Cell end, PathType pathType)
	{
		bool flag;
		this.FindPath(start, end, pathType, out this._lightPath, out flag);
		if (flag)
		{
			return this._lightPath.Count;
		}
		return -1;
	}

	// Token: 0x06000850 RID: 2128 RVA: 0x0002A7CE File Offset: 0x000289CE
	public int NavigableDistance(IntVector2 startPos, IntVector2 endPos, PathType pathType)
	{
		return this.NavigableDistance(this.CellFromPosition(startPos), this.CellFromPosition(endPos), pathType);
	}

	// Token: 0x06000851 RID: 2129 RVA: 0x0002A7E5 File Offset: 0x000289E5
	public int NavigableDistance(Vector3 startPos, Vector3 endPos, PathType pathType)
	{
		return this.NavigableDistance(this.CellFromPosition(startPos), this.CellFromPosition(endPos), pathType);
	}

	// Token: 0x06000852 RID: 2130 RVA: 0x0002A7FC File Offset: 0x000289FC
	public bool CheckPath(Cell startTile, Cell targetTile, PathType pathType)
	{
		bool result;
		this.FindPath(startTile, targetTile, pathType, out this._nullPath, out result);
		return result;
	}

	// Token: 0x06000853 RID: 2131 RVA: 0x0002A81B File Offset: 0x00028A1B
	public bool CheckPath(Vector3 startPosition, Vector3 targetPosition, PathType pathType)
	{
		return this.CheckPath(this.CellFromPosition(startPosition), this.CellFromPosition(targetPosition), pathType);
	}

	// Token: 0x06000854 RID: 2132 RVA: 0x0002A834 File Offset: 0x00028A34
	private List<Cell> GetPath(Cell startTile, Cell targetTile)
	{
		this._path.Clear();
		Cell cell;
		for (cell = targetTile; cell != startTile; cell = cell.parent)
		{
			this._path.Add(cell);
		}
		this._path.Add(cell);
		this._path.Reverse();
		return this._path;
	}

	// Token: 0x06000855 RID: 2133 RVA: 0x0002A884 File Offset: 0x00028A84
	public void FindLoop(Cell tile, Direction directionA, Direction directionB, out List<Cell> finalList)
	{
		if (!this.TrapCheck(tile))
		{
			int navBin = tile.NavBin;
			Cell startTile = this.CellFromPosition(tile.position + directionA.ToIntVector2());
			Cell targetTile = this.CellFromPosition(tile.position + directionB.ToIntVector2());
			tile.NavBin = 15;
			finalList = this.GetPath(startTile, targetTile);
			tile.NavBin = navBin;
			return;
		}
		finalList = new List<Cell>();
	}

	// Token: 0x06000856 RID: 2134 RVA: 0x0002A8F4 File Offset: 0x00028AF4
	public void TempOpenWindows()
	{
		EnvironmentController.TempObstacleManagement tempObstacleManagement = this.tempOpenWindows;
		if (tempObstacleManagement == null)
		{
			return;
		}
		tempObstacleManagement();
	}

	// Token: 0x06000857 RID: 2135 RVA: 0x0002A906 File Offset: 0x00028B06
	public void TempCloseWindows()
	{
		EnvironmentController.TempObstacleManagement tempObstacleManagement = this.tempCloseWindows;
		if (tempObstacleManagement == null)
		{
			return;
		}
		tempObstacleManagement();
	}

	// Token: 0x06000858 RID: 2136 RVA: 0x0002A918 File Offset: 0x00028B18
	public void TempOpenBully()
	{
		EnvironmentController.TempObstacleManagement tempObstacleManagement = this.tempOpenBully;
		if (tempObstacleManagement == null)
		{
			return;
		}
		tempObstacleManagement();
	}

	// Token: 0x06000859 RID: 2137 RVA: 0x0002A92A File Offset: 0x00028B2A
	public void TempCloseBully()
	{
		EnvironmentController.TempObstacleManagement tempObstacleManagement = this.tempCloseBully;
		if (tempObstacleManagement == null)
		{
			return;
		}
		tempObstacleManagement();
	}

	// Token: 0x0600085A RID: 2138 RVA: 0x0002A93C File Offset: 0x00028B3C
	public void InitializeLighting()
	{
		this.lightMap = new LightController[this.levelSize.x, this.levelSize.z];
		this.lightControllersToUpdateGrid = new bool[this.levelSize.x, this.levelSize.z];
		this.lightSourcesToRegenerateGrid = new bool[this.levelSize.x, this.levelSize.z];
		this.lightGenerationDijkstraMap = new DijkstraMap(this, PathType.Const, Array.Empty<Transform>());
		IntVector2 pos = default(IntVector2);
		for (int i = 0; i < this.levelSize.x; i++)
		{
			for (int j = 0; j < this.levelSize.z; j++)
			{
				this.lightMap[i, j] = new LightController();
				this.lightMap[i, j].Initialize(this, new IntVector2(i, j));
				pos.x = i;
				pos.z = j;
				Singleton<CoreGameManager>.Instance.UpdateLighting(this.standardDarkLevel, pos);
			}
		}
	}

	// Token: 0x0600085B RID: 2139 RVA: 0x0002AA40 File Offset: 0x00028C40
	public void QueueLightSourceForRegenerate(Cell cell)
	{
		if (!this.lightSourcesToRegenerateGrid[cell.position.x, cell.position.z])
		{
			this.lightSourcesToRegenerateGrid[cell.position.x, cell.position.z] = true;
			this.lightSourcesToRegenerate.Enqueue(cell);
		}
	}

	// Token: 0x0600085C RID: 2140 RVA: 0x0002AAA0 File Offset: 0x00028CA0
	public void QueueLightControllerForUpdate(LightController lightController)
	{
		if (!this.lightControllersToUpdateGrid[lightController.position.x, lightController.position.z])
		{
			this.lightControllersToUpdateGrid[lightController.position.x, lightController.position.z] = true;
			this.lightControllersToUpdate.Enqueue(lightController);
		}
	}

	// Token: 0x0600085D RID: 2141 RVA: 0x0002AB00 File Offset: 0x00028D00
	public void UpdateQueuedLightChanges()
	{
		if (!this.lightingOverride)
		{
			while (this.lightSourcesToRegenerate.Count > 0)
			{
				Cell cell = this.lightSourcesToRegenerate.Dequeue();
				this.RegenerateLight(cell);
				this.lightSourcesToRegenerateGrid[cell.position.x, cell.position.z] = false;
			}
			while (this.lightControllersToUpdate.Count > 0)
			{
				LightController lightController = this.lightControllersToUpdate.Dequeue();
				lightController.UpdateLighting();
				Singleton<CoreGameManager>.Instance.UpdateLighting(this.lightMap[lightController.position.x, lightController.position.z].Color, lightController.position);
				this.lightControllersToUpdateGrid[lightController.position.x, lightController.position.z] = false;
			}
		}
	}

	// Token: 0x0600085E RID: 2142 RVA: 0x0002ABD8 File Offset: 0x00028DD8
	public void GenerateLight(Cell tile, Color color, int strength)
	{
		tile.hasLight = true;
		tile.lightOn = true;
		tile.lightStrength = strength;
		tile.lightColor = color;
		tile.lightAffectingCells.Add(tile);
		this.lightMap[tile.position.x, tile.position.z].AddSource(tile, 0);
		this.lightGenerationDijkstraMap.Calculate(strength, true, new IntVector2[]
		{
			tile.position
		});
		tile.lightAffectingCells.AddRange(this.lightGenerationDijkstraMap.FoundCells());
		foreach (Cell cell in this.lightGenerationDijkstraMap.FoundCells())
		{
			this.lightMap[cell.position.x, cell.position.z].AddSource(tile, this.lightGenerationDijkstraMap.Value(cell.position));
		}
		if (!tile.permanentLight)
		{
			this.lights.Add(tile);
		}
	}

	// Token: 0x0600085F RID: 2143 RVA: 0x0002ACFC File Offset: 0x00028EFC
	public void RegenerateLight(Cell cell)
	{
		foreach (Cell cell2 in cell.lightAffectingCells)
		{
			this.lightMap[cell2.position.x, cell2.position.z].RemoveSource(cell);
			this.QueueLightControllerForUpdate(this.lightMap[cell2.position.x, cell2.position.z]);
		}
		cell.lightAffectingCells.Clear();
		this.GenerateLight(cell, cell.lightColor, cell.lightStrength);
	}

	// Token: 0x06000860 RID: 2144 RVA: 0x0002ADB4 File Offset: 0x00028FB4
	public void UpdateLightingAtCell(Cell cell)
	{
		if (!this.lightingOverride)
		{
			foreach (Cell cell2 in cell.lightAffectingCells)
			{
				this.QueueLightControllerForUpdate(this.lightMap[cell2.position.x, cell2.position.z]);
			}
		}
	}

	// Token: 0x06000861 RID: 2145 RVA: 0x0002AE30 File Offset: 0x00029030
	public void SetAllLights(bool on)
	{
		foreach (Cell cell in this.lights)
		{
			cell.SetLight(on);
		}
	}

	// Token: 0x06000862 RID: 2146 RVA: 0x0002AE84 File Offset: 0x00029084
	private IEnumerator SweepLights(bool on)
	{
		float time = 0.2f;
		int num;
		for (int x = 0; x < this.levelSize.x; x = num + 1)
		{
			for (int i = 0; i < this.levelSize.z; i++)
			{
				if (!this.cells[x, i].Null && this.cells[x, i].hasLight && !this.cells[x, i].permanentLight)
				{
					this.cells[x, i].SetLight(on);
				}
			}
			while (time > 0f)
			{
				time -= Time.deltaTime;
				yield return null;
			}
			time = 0.2f;
			yield return null;
			num = x;
		}
		yield break;
	}

	// Token: 0x06000863 RID: 2147 RVA: 0x0002AE9A File Offset: 0x0002909A
	public float LightLevel(Vector3 position)
	{
		return this.LightLevel(IntVector2.GetGridPosition(position));
	}

	// Token: 0x06000864 RID: 2148 RVA: 0x0002AEA8 File Offset: 0x000290A8
	public float LightLevel(IntVector2 position)
	{
		return this.lightMap[position.x, position.z].Level;
	}

	// Token: 0x06000865 RID: 2149 RVA: 0x0002AEC6 File Offset: 0x000290C6
	public void AddFog(Fog fog)
	{
		if (!this.fogs.Contains(fog))
		{
			this.fogs.Add(fog);
			this.UpdateFog();
		}
	}

	// Token: 0x06000866 RID: 2150 RVA: 0x0002AEE8 File Offset: 0x000290E8
	public void RemoveFog(Fog fog)
	{
		this.fogs.Remove(fog);
		this.UpdateFog();
	}

	// Token: 0x06000867 RID: 2151 RVA: 0x0002AF00 File Offset: 0x00029100
	public void UpdateFog()
	{
		if (this.fogs.Count > 0)
		{
			Fog fog = this.fogs[0];
			Shader.SetGlobalInt("_FogActive", 1);
			for (int i = 0; i < this.fogs.Count; i++)
			{
				if (this.fogs[i].priority > fog.priority)
				{
					fog = this.fogs[i];
				}
				else if (this.fogs[i].priority == fog.priority && this.fogs[i].startDist < fog.startDist)
				{
					fog = this.fogs[i];
				}
			}
			Shader.SetGlobalColor("_FogColor", fog.color);
			Shader.SetGlobalFloat("_FogStartDistance", fog.startDist);
			Shader.SetGlobalFloat("_FogMaxDistance", fog.maxDist);
			Shader.SetGlobalFloat("_FogStrength", fog.strength);
			return;
		}
		Shader.SetGlobalFloat("_FogStrength", 0f);
		Shader.SetGlobalInt("_FogActive", 0);
	}

	// Token: 0x06000868 RID: 2152 RVA: 0x0002B00E File Offset: 0x0002920E
	public bool TrapCheck(Cell tile)
	{
		return this.TrapCheck(tile, false, out this._trapCheckList);
	}

	// Token: 0x06000869 RID: 2153 RVA: 0x0002B020 File Offset: 0x00029220
	public bool TrapCheck(Cell tile, bool checkAllPaths, out List<List<Cell>> paths)
	{
		paths = new List<List<Cell>>();
		bool flag = false;
		int navBin = tile.NavBin;
		this.GetNavNeighbors(tile, this._trapNeighbors, PathType.Nav);
		tile.NavBin = 15;
		int num = 0;
		while (num < this._trapNeighbors.Count && (checkAllPaths || !flag))
		{
			int num2 = 0;
			while (num2 < this._trapNeighbors.Count && (checkAllPaths || !flag))
			{
				if (!this.CheckPath(this._trapNeighbors[num], this._trapNeighbors[num2], PathType.Nav))
				{
					tile.NavBin = navBin;
					Debug.LogWarning("Potential trap detected at " + tile.position.x.ToString() + "," + tile.position.z.ToString());
					flag = true;
				}
				num2++;
			}
			num++;
		}
		tile.NavBin = navBin;
		return flag;
	}

	// Token: 0x0600086A RID: 2154 RVA: 0x0002B0F8 File Offset: 0x000292F8
	public void SpawnNPCs()
	{
		this.npcSpawnTiles.Clear();
		this.npcSpawnTiles.AddRange(this.npcSpawnTile);
		this.npcsLeftToSpawn.Clear();
		this.npcsLeftToSpawn.AddRange(this.npcsToSpawn);
		base.StartCoroutine(this.NPCSpawner());
	}

	// Token: 0x0600086B RID: 2155 RVA: 0x0002B14A File Offset: 0x0002934A
	private IEnumerator NPCSpawner()
	{
		Vector3 vector = default(Vector3);
		Vector3 vector2 = default(Vector3);
		while (this.npcsLeftToSpawn.Count > 0)
		{
			if (Singleton<CoreGameManager>.Instance.GetPlayer(0) != null)
			{
				vector2 = Singleton<CoreGameManager>.Instance.GetPlayer(0).transform.position;
				for (int i = 0; i < this.npcsLeftToSpawn.Count; i++)
				{
					vector = this.CellFromPosition(this.npcSpawnTiles[i].position).FloorWorldPosition;
					if (this.npcsLeftToSpawn[i].IgnorePlayerOnSpawn || (Vector3.Distance(vector, vector2) > this.npcSpawnBufferRadius && (vector.x > vector2.x + this.npcSpawnBufferWidth / 2f || vector.x < vector2.x - this.npcSpawnBufferWidth / 2f) && (vector.z > vector2.z + this.npcSpawnBufferWidth / 2f || vector.z < vector2.z - this.npcSpawnBufferWidth / 2f)))
					{
						this.SpawnNPC(this.npcsLeftToSpawn[i], this.npcSpawnTiles[i].position);
						if (this.npcsLeftToSpawn[i].Character == Character.Baldi && this.angerOnSpawn)
						{
							Singleton<BaseGameManager>.Instance.AngerBaldi(Singleton<BaseGameManager>.Instance.NotebookAngerVal);
						}
						this.npcsLeftToSpawn.RemoveAt(i);
						this.npcSpawnTiles.RemoveAt(i);
						i--;
					}
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600086C RID: 2156 RVA: 0x0002B15C File Offset: 0x0002935C
	public void SpawnNPC(NPC npc, IntVector2 position)
	{
		NPC npc2 = Object.Instantiate<NPC>(npc, base.transform);
		this.npcs.Add(npc2);
		npc2.ec = this;
		npc2.transform.localPosition = new Vector3((float)position.x * 10f + 5f, 5f, (float)position.z * 10f + 5f);
		for (int i = 0; i < Singleton<CoreGameManager>.Instance.setPlayers; i++)
		{
			npc2.players.Add(Singleton<CoreGameManager>.Instance.GetPlayer(i));
		}
		npc2.Initialize();
		npc2.gameObject.SetActive(true);
	}

	// Token: 0x0600086D RID: 2157 RVA: 0x0002B204 File Offset: 0x00029404
	public Cell CellFromPosition(int x, int z)
	{
		if (this.ContainsCoordinates(x, z))
		{
			return this.cells[x, z];
		}
		int num = x;
		int num2 = z;
		if (x >= this.levelSize.x)
		{
			num = this.levelSize.x - 1;
		}
		else if (x < 0)
		{
			num = 0;
		}
		if (z >= this.levelSize.z)
		{
			num2 = this.levelSize.z - 1;
		}
		else if (z < 0)
		{
			num2 = 0;
		}
		return this.cells[num, num2];
	}

	// Token: 0x0600086E RID: 2158 RVA: 0x0002B282 File Offset: 0x00029482
	public Cell CellFromPosition(IntVector2 position)
	{
		return this.CellFromPosition(position.x, position.z);
	}

	// Token: 0x0600086F RID: 2159 RVA: 0x0002B296 File Offset: 0x00029496
	public Cell CellFromPosition(Vector3 position)
	{
		return this.CellFromPosition(IntVector2.GetGridPosition(position));
	}

	// Token: 0x06000870 RID: 2160 RVA: 0x0002B2A4 File Offset: 0x000294A4
	public Cell ClosestCellFromPosition(IntVector2 position)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 1;
		int num4 = 0;
		int num5 = 1;
		int num6 = 0;
		IntVector2 b = default(IntVector2);
		Cell cell = this.CellFromPosition(position);
		while ((cell.Null || (!cell.Null && cell.ConstBin == 16)) && num6 < this.levelSize.x * this.levelSize.z)
		{
			b.x = num2;
			b.z = num4;
			cell = this.CellFromPosition(position + b);
			if (this.ContainsCoordinates(position + b))
			{
				num6++;
			}
			if (num2 == 0 && num4 == num)
			{
				num++;
				num4 = num;
			}
			if (Mathf.Abs(num2) >= num)
			{
				num3 *= -1;
			}
			if (Mathf.Abs(num4) >= num)
			{
				num5 *= -1;
			}
			num2 += num3;
			num4 += num5;
		}
		return cell;
	}

	// Token: 0x06000871 RID: 2161 RVA: 0x0002B378 File Offset: 0x00029578
	public List<Cell> AllCells()
	{
		List<Cell> list = new List<Cell>();
		IntVector2 position = default(IntVector2);
		for (int i = 0; i < this.levelSize.x; i++)
		{
			for (int j = 0; j < this.levelSize.z; j++)
			{
				position.x = i;
				position.z = j;
				Cell cell = this.CellFromPosition(position);
				if (!cell.Null)
				{
					list.Add(cell);
				}
			}
		}
		return list;
	}

	// Token: 0x06000872 RID: 2162 RVA: 0x0002B3EC File Offset: 0x000295EC
	public Cell RandomCell(bool includeOffLimits, bool includeWithObjects, bool useEntitySafeCell)
	{
		this.AllTilesNoGarbage(includeOffLimits, includeWithObjects);
		Cell result = null;
		bool flag = false;
		if (this.allTiles.Count <= 0)
		{
			return null;
		}
		int index = Random.Range(0, this.allTiles.Count);
		if (useEntitySafeCell && this.allTiles[index].room.entitySafeCells.Count > 0)
		{
			flag = true;
			result = this.allTiles[index].room.RandomEntitySafeCellNoGarbage();
		}
		if (flag)
		{
			return result;
		}
		if (this.allTiles.Count > 0)
		{
			return this.allTiles[index];
		}
		return null;
	}

	// Token: 0x06000873 RID: 2163 RVA: 0x0002B484 File Offset: 0x00029684
	public List<Cell> CellsFromPerimeter(IntVector2 center, int size)
	{
		IntVector2 position = default(IntVector2);
		List<Cell> list = new List<Cell>();
		for (int i = -size; i < size + 1; i++)
		{
			for (int j = -size; j < size + 1; j++)
			{
				if (Mathf.Abs(i) == size || Mathf.Abs(j) == size)
				{
					position.x = i + center.x;
					position.z = j + center.z;
					if (!this.CellFromPosition(position).Null)
					{
						list.Add(this.CellFromPosition(position));
					}
				}
			}
		}
		return list;
	}

	// Token: 0x06000874 RID: 2164 RVA: 0x0002B50C File Offset: 0x0002970C
	public void GetNavNeighbors(Cell tile, List<Cell> list, PathType pathType)
	{
		list.Clear();
		for (int i = 0; i < 4; i++)
		{
			if (tile.Navigable(Directions.FromInt(i), pathType) && this.ContainsCoordinates(tile.position + Directions.Vectors[i]) && (tile.Null || !this.CellFromPosition(tile.position + Directions.Vectors[i]).Null))
			{
				list.Add(this.CellFromPosition(tile.position + Directions.Vectors[i]));
			}
		}
	}

	// Token: 0x06000875 RID: 2165 RVA: 0x0002B5A8 File Offset: 0x000297A8
	public List<Cell> GetCellNeighbors(IntVector2 position)
	{
		List<Cell> list = new List<Cell>();
		for (int i = 0; i < 4; i++)
		{
			if (!this.CellFromPosition(position + ((Direction)i).ToIntVector2()).Null)
			{
				list.Add(this.CellFromPosition(position + ((Direction)i).ToIntVector2()));
			}
		}
		return list;
	}

	// Token: 0x06000876 RID: 2166 RVA: 0x0002B5FC File Offset: 0x000297FC
	public List<RoomSpawn> GetPotentialRoomSpawnsAtCell(IntVector2 position)
	{
		List<RoomSpawn> list = new List<RoomSpawn>();
		for (int i = 0; i < 4; i++)
		{
			if (!this.CellFromPosition(position).WallHardCovered((Direction)i) && this.CellFromPosition(position + ((Direction)i).ToIntVector2()).Null)
			{
				list.Add(new RoomSpawn(position + ((Direction)i).ToIntVector2(), ((Direction)i).GetOpposite()));
			}
		}
		return list;
	}

	// Token: 0x06000877 RID: 2167 RVA: 0x0002B664 File Offset: 0x00029864
	public int GetDistance(Cell tileA, Cell tileB)
	{
		int num = Mathf.Abs(tileA.position.x - tileB.position.x);
		int num2 = Mathf.Abs(tileA.position.z - tileB.position.z);
		if (num > num2)
		{
			return 14 * num2 + 10 * (num - num2);
		}
		return 14 * num + 10 * (num2 - num);
	}

	// Token: 0x06000878 RID: 2168 RVA: 0x0002B6C8 File Offset: 0x000298C8
	public Texture2D GetMatchingTextureAtlas(Texture2D floor, Texture2D wall, Texture2D ceiling, out bool matchFound)
	{
		foreach (RoomTextureAtlas roomTextureAtlas in this.generatedTextureAtlases)
		{
			if (roomTextureAtlas.floor == floor && roomTextureAtlas.wall == wall && roomTextureAtlas.ceiling == ceiling)
			{
				matchFound = true;
				return roomTextureAtlas.atlas;
			}
		}
		matchFound = false;
		return null;
	}

	// Token: 0x06000879 RID: 2169 RVA: 0x0002B754 File Offset: 0x00029954
	public void SetElevators(bool enable)
	{
		if (enable)
		{
			using (List<Elevator>.Enumerator enumerator = this.elevators.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Elevator elevator = enumerator.Current;
					elevator.Door.Open(true, false);
					elevator.ColliderGroup.Enable(true);
				}
				return;
			}
		}
		foreach (Elevator elevator2 in this.elevators)
		{
			elevator2.Door.Shut();
			elevator2.ColliderGroup.Enable(false);
		}
	}

	// Token: 0x0600087A RID: 2170 RVA: 0x0002B80C File Offset: 0x00029A0C
	public void AddNotebook(Notebook book)
	{
		this.notebooks.Add(book);
	}

	// Token: 0x0600087B RID: 2171 RVA: 0x0002B81A File Offset: 0x00029A1A
	public void AddActivity(Activity activity)
	{
		if (!this.activities.Contains(activity))
		{
			this.activities.Add(activity);
		}
	}

	// Token: 0x0600087C RID: 2172 RVA: 0x0002B836 File Offset: 0x00029A36
	public void AddEvent(RandomEvent randomEvent, float time)
	{
		this.events.Add(randomEvent);
		this.eventTimes.Add(time);
	}

	// Token: 0x0600087D RID: 2173 RVA: 0x0002B850 File Offset: 0x00029A50
	public void RandomizeEvents(int numberOfEvents, float initialGap, float minGap, float maxGap, Random cRng)
	{
		List<RandomEvent> list = new List<RandomEvent>(this.events);
		this.events.Clear();
		this.eventTimes.Clear();
		float num = initialGap;
		int num2 = 0;
		while (num2 < numberOfEvents && list.Count > 0)
		{
			num += (float)cRng.NextDouble() * (maxGap - minGap) + minGap;
			RandomEvent randomEvent = list[cRng.Next(0, list.Count)];
			randomEvent.SetEventTime(cRng);
			this.AddEvent(randomEvent, num);
			num += randomEvent.EventTime;
			list.Remove(randomEvent);
			num2++;
		}
	}

	// Token: 0x0600087E RID: 2174 RVA: 0x0002B8E0 File Offset: 0x00029AE0
	public void StartEventTimers()
	{
		for (int i = 0; i < this.events.Count; i++)
		{
			base.StartCoroutine(this.EventTimer(this.events[i], this.eventTimes[i]));
		}
	}

	// Token: 0x0600087F RID: 2175 RVA: 0x0002B928 File Offset: 0x00029B28
	public void PauseEvents(bool val)
	{
		if (val)
		{
			using (List<RandomEvent>.Enumerator enumerator = this.events.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					RandomEvent randomEvent = enumerator.Current;
					if (randomEvent.Active)
					{
						randomEvent.Pause();
					}
				}
				return;
			}
		}
		foreach (RandomEvent randomEvent2 in this.events)
		{
			if (randomEvent2.Active)
			{
				randomEvent2.Unpause();
			}
		}
	}

	// Token: 0x06000880 RID: 2176 RVA: 0x0002B9D0 File Offset: 0x00029BD0
	public RandomEvent GetEvent(RandomEventType type)
	{
		foreach (RandomEvent randomEvent in this.currentEvents)
		{
			if (randomEvent.Type == type)
			{
				return randomEvent;
			}
		}
		return null;
	}

	// Token: 0x06000881 RID: 2177 RVA: 0x0002BA2C File Offset: 0x00029C2C
	public bool TileInDirectionCheck(IntVector2 pos, Direction dir, int distance)
	{
		bool result = false;
		for (int i = 1; i <= distance; i++)
		{
			if (!this.CellFromPosition(pos + dir.ToIntVector2() * i).Null && this.CellFromPosition(pos + dir.ToIntVector2() * i).ConstBin != 16)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	// Token: 0x06000882 RID: 2178 RVA: 0x0002BA8C File Offset: 0x00029C8C
	public void MakeNoise(Vector3 position, int value)
	{
		if (!this.silent && !this.CellFromPosition(IntVector2.GetGridPosition(position)).Silent)
		{
			foreach (NPC npc in this.npcs)
			{
				npc.Hear(position, value);
				npc.behaviorStateMachine.CurrentState.Hear(position, value);
			}
		}
	}

	// Token: 0x06000883 RID: 2179 RVA: 0x0002BB0C File Offset: 0x00029D0C
	public void MakeSilent(float time)
	{
		this.silent = true;
		if (time > this.silentTime)
		{
			if (this.silentTime > 0f)
			{
				base.StopCoroutine(this.silentTimer);
			}
			this.silentTimer = this.SilentTimer(time);
			base.StartCoroutine(this.silentTimer);
		}
	}

	// Token: 0x170000B3 RID: 179
	// (get) Token: 0x06000884 RID: 2180 RVA: 0x0002BB5C File Offset: 0x00029D5C
	public float NpcTimeScale
	{
		get
		{
			float num = 1f;
			for (int i = 0; i < this.timeScaleModifiers.Count; i++)
			{
				num *= this.timeScaleModifiers[i].npcTimeScale;
			}
			return num;
		}
	}

	// Token: 0x170000B4 RID: 180
	// (get) Token: 0x06000885 RID: 2181 RVA: 0x0002BB9C File Offset: 0x00029D9C
	public float EnvironmentTimeScale
	{
		get
		{
			float num = 1f;
			for (int i = 0; i < this.timeScaleModifiers.Count; i++)
			{
				num *= this.timeScaleModifiers[i].environmentTimeScale;
			}
			return num;
		}
	}

	// Token: 0x170000B5 RID: 181
	// (get) Token: 0x06000886 RID: 2182 RVA: 0x0002BBDC File Offset: 0x00029DDC
	public float PlayerTimeScale
	{
		get
		{
			float num = 1f;
			for (int i = 0; i < this.timeScaleModifiers.Count; i++)
			{
				num *= this.timeScaleModifiers[i].playerTimeScale;
			}
			return num;
		}
	}

	// Token: 0x06000887 RID: 2183 RVA: 0x0002BC1A File Offset: 0x00029E1A
	public void AddTimeScale(TimeScaleModifier scale)
	{
		if (!this.timeScaleModifiers.Contains(scale))
		{
			this.timeScaleModifiers.Add(scale);
		}
	}

	// Token: 0x06000888 RID: 2184 RVA: 0x0002BC36 File Offset: 0x00029E36
	public void RemoveTimeScale(TimeScaleModifier scale)
	{
		this.timeScaleModifiers.Remove(scale);
	}

	// Token: 0x06000889 RID: 2185 RVA: 0x0002BC45 File Offset: 0x00029E45
	public void PauseEnvironment(bool val)
	{
		if (val)
		{
			this.timeScaleModifiers.Add(this.pauseScale);
			return;
		}
		this.timeScaleModifiers.Remove(this.pauseScale);
	}

	// Token: 0x0600088A RID: 2186 RVA: 0x0002BC70 File Offset: 0x00029E70
	public void AssignPlayers()
	{
		for (int i = 0; i < Singleton<CoreGameManager>.Instance.setPlayers; i++)
		{
			Singleton<CoreGameManager>.Instance.GetPlayer(i).ec = this;
			this.players[i] = Singleton<CoreGameManager>.Instance.GetPlayer(i);
		}
	}

	// Token: 0x0600088B RID: 2187 RVA: 0x0002BCB8 File Offset: 0x00029EB8
	public void CountConsecutiveTiles(IntVector2 startPos, Direction dir, out int length, out IntVector2 end)
	{
		this._currentTile = this.CellFromPosition(startPos);
		length = 0;
		end = this._currentTile.position;
		while (!this._currentTile.Null && !this._currentTile.NavBin.ContainsDirection(dir))
		{
			this._currentTile = this.CellFromPosition(this._currentTile.position + dir.ToIntVector2());
			length++;
			end = this._currentTile.position;
		}
	}

	// Token: 0x0600088C RID: 2188 RVA: 0x0002BD44 File Offset: 0x00029F44
	public void FurthestTileInDir(IntVector2 startPos, Direction dir, out IntVector2 end)
	{
		end = startPos;
		int num;
		this.CountConsecutiveTiles(startPos, dir, out num, out end);
	}

	// Token: 0x0600088D RID: 2189 RVA: 0x0002BD64 File Offset: 0x00029F64
	public List<List<Cell>> FindHallways()
	{
		this._hallways.Clear();
		bool flag = false;
		for (int i = 0; i < this.levelSize.x; i++)
		{
			for (int j = 0; j < this.levelSize.z; j++)
			{
				Cell cell = this.cells[i, j];
				if (!cell.Null)
				{
					if (cell.Navigable(Direction.North, PathType.Nav) && cell.Navigable(Direction.South, PathType.Nav) && !cell.open && cell.room.type == RoomType.Hall)
					{
						if (flag)
						{
							this._hallways[this._hallways.Count - 1].Add(cell);
						}
						else
						{
							flag = true;
							this._hallways.Add(new List<Cell>());
							this._hallways[this._hallways.Count - 1].Add(cell);
						}
					}
					else
					{
						flag = false;
					}
				}
			}
		}
		flag = false;
		for (int k = 0; k < this.levelSize.z; k++)
		{
			for (int l = 0; l < this.levelSize.x; l++)
			{
				Cell cell2 = this.cells[l, k];
				if (!cell2.Null)
				{
					if (cell2.Navigable(Direction.East, PathType.Nav) && cell2.Navigable(Direction.West, PathType.Nav) && !cell2.open && cell2.room.type == RoomType.Hall)
					{
						if (flag)
						{
							this._hallways[this._hallways.Count - 1].Add(cell2);
						}
						else
						{
							flag = true;
							this._hallways.Add(new List<Cell>());
							this._hallways[this._hallways.Count - 1].Add(cell2);
						}
					}
					else
					{
						flag = false;
					}
				}
			}
		}
		return this._hallways;
	}

	// Token: 0x0600088E RID: 2190 RVA: 0x0002BF3C File Offset: 0x0002A13C
	public List<Cell> FindNearbyTiles(IntVector2 cornerA, IntVector2 cornerB, int range)
	{
		this._nearbyTiles.Clear();
		int x;
		int x2;
		if (cornerB.x - cornerA.x >= 0)
		{
			x = cornerA.x;
			x2 = cornerB.x;
		}
		else
		{
			x = cornerB.x;
			x2 = cornerA.x;
		}
		int z;
		int z2;
		if (cornerB.z - cornerA.z >= 0)
		{
			z = cornerA.z;
			z2 = cornerB.z;
		}
		else
		{
			z = cornerB.z;
			z2 = cornerA.z;
		}
		IntVector2 coordinate = default(IntVector2);
		for (int i = x - range; i <= x2 + range; i++)
		{
			for (int j = z - range; j <= z2 + range; j++)
			{
				coordinate.x = i;
				coordinate.z = j;
				if (this.ContainsCoordinates(coordinate) && !this.cells[i, j].Null)
				{
					this._nearbyTiles.Add(this.cells[i, j]);
				}
			}
		}
		return this._nearbyTiles;
	}

	// Token: 0x0600088F RID: 2191 RVA: 0x0002C032 File Offset: 0x0002A232
	public List<Cell> FindCellsInNavigableRange(int range, params IntVector2[] goal)
	{
		DijkstraMap dijkstraMap = new DijkstraMap(this, PathType.Const, Array.Empty<Transform>());
		dijkstraMap.Calculate(range, true, goal);
		return dijkstraMap.FoundCells();
	}

	// Token: 0x06000890 RID: 2192 RVA: 0x0002C050 File Offset: 0x0002A250
	public void SetupDoor(Door door, Cell tile, Direction dir)
	{
		Cell cell = this.cells[tile.position.x + dir.ToIntVector2().x, tile.position.z + dir.ToIntVector2().z];
		door.ec = this;
		door.direction = dir;
		door.transform.parent = tile.ObjectBase;
		door.transform.localPosition = new Vector3(0f, 0f, 0f);
		door.transform.rotation = dir.ToRotation();
		tile.room.doors.Add(door);
		tile.doorDirsSpace.Add(dir);
		tile.doorHere = true;
		if (!tile.doorDirs.Contains(dir))
		{
			tile.doorDirs.Add(dir);
		}
		tile.doors.Add(door);
		door.position = tile.position;
		tile.HardCoverWall(dir, true);
		cell.room.doors.Add(door);
		cell.doorDirsSpace.Add(dir.GetOpposite());
		cell.doorHere = true;
		if (!cell.doorDirs.Contains(dir.GetOpposite()))
		{
			cell.doorDirs.Add(dir.GetOpposite());
		}
		cell.doors.Add(door);
		cell.HardCoverWall(dir.GetOpposite(), true);
		door.bOffset = dir.ToIntVector2();
	}

	// Token: 0x06000891 RID: 2193 RVA: 0x0002C1B8 File Offset: 0x0002A3B8
	public void ApplyMap()
	{
		for (int i = 0; i < Singleton<CoreGameManager>.Instance.setPlayers; i++)
		{
			PlayerManager player = Singleton<CoreGameManager>.Instance.GetPlayer(i);
			this.map.targets.Add(player.transform);
		}
	}

	// Token: 0x06000892 RID: 2194 RVA: 0x0002C1FC File Offset: 0x0002A3FC
	public Pickup CreateItem(RoomController room, ItemObject item, Vector2 pos)
	{
		Pickup pickup = Object.Instantiate<Pickup>(this.pickupPre, room.objectObject.transform);
		pickup.item = item;
		pickup.transform.position = new Vector3(pos.x, 5f, pos.y);
		this.items.Add(pickup);
		return pickup;
	}

	// Token: 0x06000893 RID: 2195 RVA: 0x0002C255 File Offset: 0x0002A455
	public Vector3 RealRoomMin(RoomController room)
	{
		return new Vector3((float)room.position.x * 10f, 0f, (float)room.position.z * 10f);
	}

	// Token: 0x06000894 RID: 2196 RVA: 0x0002C288 File Offset: 0x0002A488
	public Vector3 RealRoomMax(RoomController room)
	{
		return new Vector3((float)room.position.x * 10f + (float)room.size.x * 10f, 0f, (float)room.position.z * 10f + (float)room.size.z * 10f);
	}

	// Token: 0x06000895 RID: 2197 RVA: 0x0002C2E9 File Offset: 0x0002A4E9
	public Vector3 RealRoomSize(RoomController room)
	{
		return this.RealRoomMax(room) - this.RealRoomMin(room);
	}

	// Token: 0x06000896 RID: 2198 RVA: 0x0002C2FE File Offset: 0x0002A4FE
	public Vector3 RealRoomMid(RoomController room)
	{
		return this.RealRoomSize(room) / 2f + this.RealRoomMin(room);
	}

	// Token: 0x06000897 RID: 2199 RVA: 0x0002C320 File Offset: 0x0002A520
	public Vector3 RealRoomRand(RoomController room)
	{
		return new Vector3(Random.Range(this.RealRoomMin(room).x, this.RealRoomMax(room).x), 0f, Random.Range(this.RealRoomMin(room).z, this.RealRoomMax(room).z));
	}

	// Token: 0x06000898 RID: 2200 RVA: 0x0002C374 File Offset: 0x0002A574
	public void ResetEvents()
	{
		foreach (RandomEvent randomEvent in this.currentEvents)
		{
			randomEvent.ResetConditions();
		}
	}

	// Token: 0x06000899 RID: 2201 RVA: 0x0002C3C4 File Offset: 0x0002A5C4
	private IEnumerator EventTimer(RandomEvent randomEvent, float time)
	{
		while (time > 3f)
		{
			time -= Time.deltaTime * this.EnvironmentTimeScale;
			yield return null;
		}
		this.audMan.PlaySingle(this.audEventNotification);
		Singleton<CoreGameManager>.Instance.GetHud(0).BaldiTv.AnnounceEvent(randomEvent.EventIntro);
		while (time > 0f)
		{
			time -= Time.deltaTime * this.EnvironmentTimeScale;
			yield return null;
		}
		randomEvent.Begin();
		this.currentEvents.Add(randomEvent);
		this.currentEventTypes.Add(randomEvent.Type);
		yield break;
	}

	// Token: 0x0600089A RID: 2202 RVA: 0x0002C3E1 File Offset: 0x0002A5E1
	public void EventOver(RandomEvent randomEvent)
	{
		this.currentEvents.Remove(randomEvent);
		this.currentEventTypes.Remove(randomEvent.Type);
	}

	// Token: 0x0600089B RID: 2203 RVA: 0x0002C404 File Offset: 0x0002A604
	public Baldi GetBaldi()
	{
		foreach (NPC npc in this.npcs)
		{
			if (npc.Character == Character.Baldi)
			{
				return npc.GetComponent<Baldi>();
			}
		}
		return null;
	}

	// Token: 0x170000B6 RID: 182
	// (get) Token: 0x0600089C RID: 2204 RVA: 0x0002C468 File Offset: 0x0002A668
	public List<NPC> Npcs
	{
		get
		{
			return this.npcs;
		}
	}

	// Token: 0x170000B7 RID: 183
	// (get) Token: 0x0600089D RID: 2205 RVA: 0x0002C470 File Offset: 0x0002A670
	// (set) Token: 0x0600089E RID: 2206 RVA: 0x0002C478 File Offset: 0x0002A678
	public float MaxRaycast
	{
		get
		{
			return this.maxRaycast;
		}
		set
		{
			this.maxRaycast = value;
		}
	}

	// Token: 0x0600089F RID: 2207 RVA: 0x0002C481 File Offset: 0x0002A681
	private IEnumerator SilentTimer(float time)
	{
		if (time > this.silentTime)
		{
			this.silentTime = time;
		}
		while (this.silentTime > 0f)
		{
			this.silentTime -= Time.deltaTime * this.EnvironmentTimeScale;
			yield return null;
		}
		this.silent = false;
		yield break;
	}

	// Token: 0x060008A0 RID: 2208 RVA: 0x0002C497 File Offset: 0x0002A697
	public void RecalculateNavigation()
	{
		if (!this.freezeNavigationUpdates)
		{
			this.updateNavigation = true;
		}
	}

	// Token: 0x060008A1 RID: 2209 RVA: 0x0002C4A8 File Offset: 0x0002A6A8
	public void FreezeNavigationUpdates(bool freeze)
	{
		this.freezeNavigationUpdates = freeze;
	}

	// Token: 0x060008A2 RID: 2210 RVA: 0x0002C4B1 File Offset: 0x0002A6B1
	public void AddDijkstraMap(DijkstraMap map)
	{
		if (!this.activeDijkstraMaps.Contains(map))
		{
			this.activeDijkstraMaps.Add(map);
		}
	}

	// Token: 0x060008A3 RID: 2211 RVA: 0x0002C4CD File Offset: 0x0002A6CD
	public void RemoveDijkstraMap(DijkstraMap map)
	{
		this.activeDijkstraMaps.Remove(map);
	}

	// Token: 0x060008A4 RID: 2212 RVA: 0x0002C4DC File Offset: 0x0002A6DC
	public void QueueDijkstraMap(DijkstraMap map)
	{
		this.dijkstraMapsToUpdate.Enqueue(map);
	}

	// Token: 0x170000B8 RID: 184
	// (get) Token: 0x060008A5 RID: 2213 RVA: 0x0002C4EA File Offset: 0x0002A6EA
	public PlayerManager[] Players
	{
		get
		{
			return this.players;
		}
	}

	// Token: 0x060008A6 RID: 2214 RVA: 0x0002C4F4 File Offset: 0x0002A6F4
	public List<Cell> AllTilesNoGarbage(bool includeOffLimits, bool includeWithHardCoverage)
	{
		this.allTiles.Clear();
		for (int i = 0; i < this.levelSize.x; i++)
		{
			for (int j = 0; j < this.levelSize.z; j++)
			{
				if (!this.cells[i, j].Null && (includeOffLimits || (!this.cells[i, j].room.offLimits && !this.cells[i, j].offLimits)) && (includeWithHardCoverage || !this.cells[i, j].HasAnyHardCoverage))
				{
					this.allTiles.Add(this.cells[i, j]);
				}
			}
		}
		return this.allTiles;
	}

	// Token: 0x060008A7 RID: 2215 RVA: 0x0002C5BC File Offset: 0x0002A7BC
	public void FillUnfilledDirections(Cell tile, List<Direction> list)
	{
		list.Clear();
		for (int i = 0; i < 4; i++)
		{
			if (this.ContainsCoordinates(tile.position + ((Direction)i).ToIntVector2()) && this.cells[tile.position.x + ((Direction)i).ToIntVector2().x, tile.position.z + ((Direction)i).ToIntVector2().z].Null)
			{
				list.Add((Direction)i);
			}
		}
	}

	// Token: 0x170000B9 RID: 185
	// (get) Token: 0x060008A8 RID: 2216 RVA: 0x0002C63B File Offset: 0x0002A83B
	// (set) Token: 0x060008A9 RID: 2217 RVA: 0x0002C643 File Offset: 0x0002A843
	public bool Active
	{
		get
		{
			return this.active;
		}
		set
		{
			this.active = value;
		}
	}

	// Token: 0x170000BA RID: 186
	// (get) Token: 0x060008AA RID: 2218 RVA: 0x0002C64C File Offset: 0x0002A84C
	public int EventsCount
	{
		get
		{
			return this.events.Count;
		}
	}

	// Token: 0x060008AB RID: 2219 RVA: 0x0002C65C File Offset: 0x0002A85C
	public void FlickerLights(bool val)
	{
		this.flickerLights = val;
		this.lightsToFlicker.Clear();
		foreach (Cell cell in this.lights)
		{
			if (cell.lightStrength > 1)
			{
				this.lightsToFlicker.Add(cell);
			}
		}
		if (!val)
		{
			this.reLighter = this.ReLight();
			base.StartCoroutine(this.reLighter);
			return;
		}
		if (this.restoringFlickers)
		{
			base.StopCoroutine(this.reLighter);
			this.restoringFlickers = false;
		}
	}

	// Token: 0x060008AC RID: 2220 RVA: 0x0002C708 File Offset: 0x0002A908
	private IEnumerator ReLight()
	{
		this.restoringFlickers = true;
		this._reLights.Clear();
		this._reLights.AddRange(this.lights);
		while (this._reLights.Count > 0)
		{
			int index = Random.Range(0, this._reLights.Count);
			this._reLights[index].FixFlicker();
			this._reLights.RemoveAt(index);
			yield return null;
		}
		this.restoringFlickers = false;
		yield break;
	}

	// Token: 0x170000BB RID: 187
	// (get) Token: 0x060008AD RID: 2221 RVA: 0x0002C717 File Offset: 0x0002A917
	public float Height
	{
		get
		{
			return this.height;
		}
	}

	// Token: 0x170000BC RID: 188
	// (get) Token: 0x060008AE RID: 2222 RVA: 0x0002C71F File Offset: 0x0002A91F
	public float SurpassedGameTime
	{
		get
		{
			return this.surpassedGameTime;
		}
	}

	// Token: 0x170000BD RID: 189
	// (get) Token: 0x060008AF RID: 2223 RVA: 0x0002C727 File Offset: 0x0002A927
	public float SurpassedRealTime
	{
		get
		{
			return this.surpassedRealTime;
		}
	}

	// Token: 0x170000BE RID: 190
	// (get) Token: 0x060008B0 RID: 2224 RVA: 0x0002C72F File Offset: 0x0002A92F
	public List<RandomEventType> CurrentEventTypes
	{
		get
		{
			return this.currentEventTypes;
		}
	}

	// Token: 0x060008B1 RID: 2225 RVA: 0x0002C738 File Offset: 0x0002A938
	public void GetCellDebugInfo(Cell cell)
	{
		string text = string.Format("Debug info for cell at {0},{1}\n", cell.position.x, cell.position.z);
		text += string.Format("Cell initialized: {0}\n", !cell.Null);
		text += string.Format("Cell ConstBin = {0}\n", cell.ConstBin);
		text += string.Format("Cell NavBin = {0}\n", cell.NavBin);
		text += string.Format("Cell SoundBin = {0}\n", cell.SoundBin);
		text += string.Format("Cell HardCoverageBin = {0}\n", Convert.ToString(cell.HardCoverageBin, 2));
		text += string.Format("Cell SoftCoverageBin = {0}\n", Convert.ToString(cell.SoftCoverageBin, 2));
		if (cell.room == null)
		{
			text += "Cell has no room\n";
		}
		else
		{
			text += string.Format("Cell belongs to room: {0}\n", cell.room.transform.name);
		}
		if (cell.TileLoaded)
		{
			text += "Cell has TILE.\n";
		}
		else
		{
			text += "Cell does NOT have tile.\n";
		}
		if (cell.open)
		{
			text += "Cell is OPEN.\n";
		}
		else
		{
			text += "Cell is NOT open.\n";
		}
		if (cell.offLimits)
		{
			text += "Cell is OFF LIMITS\n";
		}
		else
		{
			text += "Cell is NOT off limits\n";
		}
		Debug.Log(text, cell.room.transform);
	}

	// Token: 0x170000BF RID: 191
	// (get) Token: 0x060008B2 RID: 2226 RVA: 0x0002C8D1 File Offset: 0x0002AAD1
	public Tile TilePre
	{
		get
		{
			return this.tilePre;
		}
	}

	// Token: 0x060008B3 RID: 2227 RVA: 0x0002C8D9 File Offset: 0x0002AAD9
	public Mesh TileMesh(int index)
	{
		return this.tileMesh[index];
	}

	// Token: 0x170000C0 RID: 192
	// (get) Token: 0x060008B4 RID: 2228 RVA: 0x0002C8E3 File Offset: 0x0002AAE3
	public CullingManager CullingManager
	{
		get
		{
			return this.cullingManager;
		}
	}

	// Token: 0x040008D0 RID: 2256
	public const float tileSize = 10f;

	// Token: 0x040008D1 RID: 2257
	public const float tileOffset = 5f;

	// Token: 0x040008D2 RID: 2258
	[SerializeField]
	private Tile tilePre;

	// Token: 0x040008D3 RID: 2259
	[SerializeField]
	private Mesh[] tileMesh = new Mesh[16];

	// Token: 0x040008D4 RID: 2260
	[SerializeField]
	private TileShape[] tileShape = new TileShape[16];

	// Token: 0x040008D5 RID: 2261
	[SerializeField]
	private TextTextureGenerator TextTextureGenerator;

	// Token: 0x040008D6 RID: 2262
	[SerializeField]
	private CullingManager cullingManager;

	// Token: 0x040008D7 RID: 2263
	public List<PosterObject> loadedPosters = new List<PosterObject>();

	// Token: 0x040008D8 RID: 2264
	public List<Texture2D> posterTextures = new List<Texture2D>();

	// Token: 0x040008D9 RID: 2265
	public NavMeshSurface nms;

	// Token: 0x040008DA RID: 2266
	public Transform navMeshTransform;

	// Token: 0x040008DB RID: 2267
	public Transform soundPropagationTransform;

	// Token: 0x040008DC RID: 2268
	private Transform _objectBase;

	// Token: 0x040008DD RID: 2269
	[SerializeField]
	private Pickup pickupPre;

	// Token: 0x040008DE RID: 2270
	public Cell[,] cells;

	// Token: 0x040008DF RID: 2271
	private bool[,] lightControllersToUpdateGrid = new bool[0, 0];

	// Token: 0x040008E0 RID: 2272
	private bool[,] lightSourcesToRegenerateGrid = new bool[0, 0];

	// Token: 0x040008E1 RID: 2273
	private LightController[,] lightMap = new LightController[0, 0];

	// Token: 0x040008E2 RID: 2274
	private Queue<LightController> lightControllersToUpdate = new Queue<LightController>();

	// Token: 0x040008E3 RID: 2275
	private Queue<Cell> lightSourcesToRegenerate = new Queue<Cell>();

	// Token: 0x040008E4 RID: 2276
	public List<LightData> _unmodifiedLightSources = new List<LightData>();

	// Token: 0x040008E5 RID: 2277
	private DijkstraMap lightGenerationDijkstraMap;

	// Token: 0x040008E6 RID: 2278
	public Color standardDarkLevel;

	// Token: 0x040008E7 RID: 2279
	public List<RoomController> rooms = new List<RoomController>();

	// Token: 0x040008E8 RID: 2280
	private List<List<Cell>> _trapCheckList = new List<List<Cell>>();

	// Token: 0x040008E9 RID: 2281
	private List<Cell> allTiles = new List<Cell>();

	// Token: 0x040008EA RID: 2282
	public List<Cell> lights = new List<Cell>();

	// Token: 0x040008EB RID: 2283
	public List<Cell> lightsToFlicker = new List<Cell>();

	// Token: 0x040008EC RID: 2284
	private List<Cell> _reLights = new List<Cell>();

	// Token: 0x040008ED RID: 2285
	private List<DijkstraMap> activeDijkstraMaps = new List<DijkstraMap>();

	// Token: 0x040008EE RID: 2286
	private Queue<DijkstraMap> dijkstraMapsToUpdate = new Queue<DijkstraMap>();

	// Token: 0x040008EF RID: 2287
	private IEnumerator reLighter;

	// Token: 0x040008F0 RID: 2288
	private bool restoringFlickers;

	// Token: 0x040008F1 RID: 2289
	public EnvironmentController.EnvironmentBeginPlay OnEnvironmentBeginPlay;

	// Token: 0x040008F2 RID: 2290
	public List<Pickup> items = new List<Pickup>();

	// Token: 0x040008F3 RID: 2291
	public List<Obstacle> obstacles = new List<Obstacle>();

	// Token: 0x040008F4 RID: 2292
	private List<Fog> fogs = new List<Fog>();

	// Token: 0x040008F5 RID: 2293
	public IntVector2 levelSize;

	// Token: 0x040008F6 RID: 2294
	public Vector3 spawnPoint;

	// Token: 0x040008F7 RID: 2295
	[SerializeField]
	private float height;

	// Token: 0x040008F8 RID: 2296
	private float npcSpawnBufferRadius = 75f;

	// Token: 0x040008F9 RID: 2297
	private float npcSpawnBufferWidth = 30f;

	// Token: 0x040008FA RID: 2298
	public Quaternion spawnRotation;

	// Token: 0x040008FB RID: 2299
	public Map map;

	// Token: 0x040008FC RID: 2300
	public List<RoomController> offices = new List<RoomController>();

	// Token: 0x040008FD RID: 2301
	[SerializeField]
	private RoomController nullRoom;

	// Token: 0x040008FE RID: 2302
	public RoomController mainHall;

	// Token: 0x040008FF RID: 2303
	private PlayerManager[] players = new PlayerManager[4];

	// Token: 0x04000900 RID: 2304
	public List<Door> standardDoors = new List<Door>();

	// Token: 0x04000901 RID: 2305
	public List<Elevator> elevators = new List<Elevator>();

	// Token: 0x04000902 RID: 2306
	public List<Notebook> notebooks = new List<Notebook>();

	// Token: 0x04000903 RID: 2307
	public List<Activity> activities = new List<Activity>();

	// Token: 0x04000904 RID: 2308
	private List<NPC> npcs = new List<NPC>();

	// Token: 0x04000905 RID: 2309
	public List<NPC> npcsToSpawn = new List<NPC>();

	// Token: 0x04000906 RID: 2310
	public List<NPC> npcsLeftToSpawn = new List<NPC>();

	// Token: 0x04000907 RID: 2311
	public Cell[] npcSpawnTile;

	// Token: 0x04000908 RID: 2312
	private List<Cell> npcSpawnTiles = new List<Cell>();

	// Token: 0x04000909 RID: 2313
	public List<ItemObject> tripItems = new List<ItemObject>();

	// Token: 0x0400090A RID: 2314
	[SerializeField]
	private List<RandomEvent> events = new List<RandomEvent>();

	// Token: 0x0400090B RID: 2315
	[SerializeField]
	private List<float> eventTimes = new List<float>();

	// Token: 0x0400090C RID: 2316
	private List<RandomEvent> currentEvents = new List<RandomEvent>();

	// Token: 0x0400090D RID: 2317
	private List<RandomEventType> currentEventTypes = new List<RandomEventType>();

	// Token: 0x0400090E RID: 2318
	private Cell _currentTile;

	// Token: 0x0400090F RID: 2319
	private List<Cell> _path = new List<Cell>();

	// Token: 0x04000910 RID: 2320
	private List<Cell> _lightPath = new List<Cell>();

	// Token: 0x04000911 RID: 2321
	public List<Cell> _neighbors = new List<Cell>();

	// Token: 0x04000912 RID: 2322
	public List<Cell> _nullPath = new List<Cell>();

	// Token: 0x04000913 RID: 2323
	public List<Cell> _trapNeighbors = new List<Cell>();

	// Token: 0x04000914 RID: 2324
	public List<Cell> _nearbyTiles = new List<Cell>();

	// Token: 0x04000915 RID: 2325
	private Heap<Cell> _openSet;

	// Token: 0x04000916 RID: 2326
	private List<List<Cell>> _hallways = new List<List<Cell>>();

	// Token: 0x04000917 RID: 2327
	private HashSet<Cell> _closedSet = new HashSet<Cell>();

	// Token: 0x04000918 RID: 2328
	private List<Direction> _tileDirs = new List<Direction>();

	// Token: 0x04000919 RID: 2329
	private MaterialPropertyBlock _tilePropertyBlock;

	// Token: 0x0400091A RID: 2330
	private bool instantiateTiles = true;

	// Token: 0x0400091B RID: 2331
	public List<RoomTextureAtlas> generatedTextureAtlases = new List<RoomTextureAtlas>();

	// Token: 0x0400091C RID: 2332
	[SerializeField]
	private AudioManager audMan;

	// Token: 0x0400091D RID: 2333
	[SerializeField]
	private SoundObject audEventNotification;

	// Token: 0x0400091E RID: 2334
	private const float eventAnnounceTime = 3f;

	// Token: 0x0400091F RID: 2335
	[SerializeField]
	private float maxRaycast = float.PositiveInfinity;

	// Token: 0x04000920 RID: 2336
	private List<TimeScaleModifier> timeScaleModifiers = new List<TimeScaleModifier>();

	// Token: 0x04000921 RID: 2337
	private TimeScaleModifier pauseScale = new TimeScaleModifier();

	// Token: 0x04000922 RID: 2338
	private IEnumerator silentTimer;

	// Token: 0x04000923 RID: 2339
	private float silentTime;

	// Token: 0x04000924 RID: 2340
	private float surpassedGameTime;

	// Token: 0x04000925 RID: 2341
	private float surpassedRealTime;

	// Token: 0x04000926 RID: 2342
	public int notebookTotal;

	// Token: 0x04000927 RID: 2343
	private bool silent;

	// Token: 0x04000928 RID: 2344
	private bool updateNavigation;

	// Token: 0x04000929 RID: 2345
	private bool freezeNavigationUpdates;

	// Token: 0x0400092A RID: 2346
	private bool active;

	// Token: 0x0400092B RID: 2347
	private bool flickerLights;

	// Token: 0x0400092C RID: 2348
	public bool angerOnSpawn;

	// Token: 0x0400092D RID: 2349
	private Color lighting = Color.white;

	// Token: 0x0400092E RID: 2350
	public LightMode lightMode;

	// Token: 0x0400092F RID: 2351
	public bool lightingOverride;

	// Token: 0x04000930 RID: 2352
	public float flickerSpeed = 0.1f;

	// Token: 0x04000931 RID: 2353
	private float flickerDelay;

	// Token: 0x04000932 RID: 2354
	public EnvironmentController.TempObstacleManagement tempOpenWindows;

	// Token: 0x04000933 RID: 2355
	public EnvironmentController.TempObstacleManagement tempCloseWindows;

	// Token: 0x04000934 RID: 2356
	public EnvironmentController.TempObstacleManagement tempOpenBully;

	// Token: 0x04000935 RID: 2357
	public EnvironmentController.TempObstacleManagement tempCloseBully;

	// Token: 0x02000377 RID: 887
	// (Invoke) Token: 0x06001C43 RID: 7235
	public delegate void EnvironmentBeginPlay();

	// Token: 0x02000378 RID: 888
	// (Invoke) Token: 0x06001C47 RID: 7239
	public delegate void TempObstacleManagement();
}
