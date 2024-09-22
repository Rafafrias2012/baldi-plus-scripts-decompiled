using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000189 RID: 393
public class LevelBuilder : MonoBehaviour
{
	// Token: 0x060008E0 RID: 2272 RVA: 0x0002D7FE File Offset: 0x0002B9FE
	public void Start()
	{
		if (this.generateOnStart)
		{
			this.StartGenerate();
		}
	}

	// Token: 0x060008E1 RID: 2273 RVA: 0x0002D810 File Offset: 0x0002BA10
	private void Update()
	{
		if (this.levelInProgress && !this.error)
		{
			this.framesSinceLastYield++;
			if (this.framesSinceLastYield >= 120)
			{
				Debug.LogError("Level generator crashed!");
				Singleton<CoreGameManager>.Instance.levelGenError = true;
				this.error = true;
				this.SetErrorMode(true);
			}
		}
	}

	// Token: 0x060008E2 RID: 2274 RVA: 0x0002D868 File Offset: 0x0002BA68
	private void SetErrorMode(bool value)
	{
		if (value)
		{
			Shader.SetGlobalInt("_SpriteColorGlitching", 1);
			Shader.SetGlobalFloat("_SpriteColorGlitchVal", Random.Range(0f, 1f));
			Shader.SetGlobalFloat("_SpriteColorGlitchPercent", 1f);
		}
	}

	// Token: 0x060008E3 RID: 2275 RVA: 0x0002D8A0 File Offset: 0x0002BAA0
	public virtual void StartGenerate()
	{
		if (this.levelInProgress || this.levelCreated)
		{
			base.StopAllCoroutines();
		}
		AudioListener.pause = true;
		if (Singleton<CoreGameManager>.Instance.GetCamera(0) != null)
		{
			Singleton<CoreGameManager>.Instance.GetCamera(0).StopRendering(true);
		}
		if (this.ld != null)
		{
			this.ec.transform.name = "EC_" + Singleton<CoreGameManager>.Instance.Seed().ToString() + "_" + this.ld.name;
			return;
		}
		if (this.levelAsset != null)
		{
			this.ec.transform.name = "EC_" + this.levelAsset.name;
		}
	}

	// Token: 0x060008E4 RID: 2276 RVA: 0x0002D96C File Offset: 0x0002BB6C
	protected bool FrameShouldEnd()
	{
		this.framesSinceLastYield = 0;
		return DateTime.Now.Ticks - this.TicksAtBeginningOfFrame.Ticks > this.frameTickLimit;
	}

	// Token: 0x060008E5 RID: 2277 RVA: 0x0002D9A4 File Offset: 0x0002BBA4
	protected void CreateMap()
	{
		if (this.map != null)
		{
			Object.Destroy(this.map.gameObject);
		}
		this.map = Object.Instantiate<Map>(this.mapPref, this.ec.transform);
		this.map.Initialize(this.ec, this.levelSize);
		this.ec.map = this.map;
	}

	// Token: 0x060008E6 RID: 2278 RVA: 0x0002DA14 File Offset: 0x0002BC14
	public void UpdatePotentialSpawnsForPlots(bool stickToHalls)
	{
		this.potentialRoomSpawns.Clear();
		for (int i = this.ld.edgeBuffer; i < this.levelSize.x - this.ld.edgeBuffer; i++)
		{
			for (int j = this.ld.edgeBuffer; j < this.levelSize.z - this.ld.edgeBuffer; j++)
			{
				IntVector2 intVector = new IntVector2(i, j);
				if (!stickToHalls)
				{
					if (this.ec.CellFromPosition(intVector).Null && !this.ProximityCheck(intVector, RoomType.Hall, this.ld.hallBuffer) && !this.ProximityCheck(intVector, RoomType.Null, this.ld.edgeBuffer))
					{
						this.potentialRoomSpawns.Add(new WeightedRoomSpawn());
						this.potentialRoomSpawns[this.potentialRoomSpawns.Count - 1].selection.position = intVector;
						this.potentialRoomSpawns[this.potentialRoomSpawns.Count - 1].weight = 1;
					}
				}
				else
				{
					List<Cell> cellNeighbors = this.ec.GetCellNeighbors(intVector);
					if (this.ec.CellFromPosition(intVector).Null && cellNeighbors.Count > 0)
					{
						this.potentialRoomSpawns.Add(new WeightedRoomSpawn());
						this.potentialRoomSpawns[this.potentialRoomSpawns.Count - 1].selection.position = intVector;
						this.potentialRoomSpawns[this.potentialRoomSpawns.Count - 1].weight = this.WeightFromPos(intVector, intVector);
					}
				}
			}
		}
	}

	// Token: 0x060008E7 RID: 2279 RVA: 0x0002DBBC File Offset: 0x0002BDBC
	public void UpdatePotentialSpawnsForRooms(bool stickToHalls)
	{
		this.potentialRoomSpawns.Clear();
		this.roomSpawnWeightDijkstraMap.Calculate(this.previousRoomSpawnPositions.ToArray());
		if (stickToHalls)
		{
			using (List<RoomController>.Enumerator enumerator = this.halls.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					RoomController roomController = enumerator.Current;
					if (roomController.type == RoomType.Hall)
					{
						foreach (Cell cell in roomController.cells)
						{
							foreach (RoomSpawn roomSpawn in this.ec.GetPotentialRoomSpawnsAtCell(cell.position))
							{
								WeightedRoomSpawn weightedRoomSpawn = new WeightedRoomSpawn();
								weightedRoomSpawn.selection = roomSpawn;
								weightedRoomSpawn.weight = this.WeightFromPos(roomSpawn.position, cell.position);
								this.potentialRoomSpawns.Add(weightedRoomSpawn);
							}
						}
					}
				}
				return;
			}
		}
		foreach (RoomController roomController2 in this.ec.rooms)
		{
			if (roomController2.type == RoomType.Room)
			{
				foreach (IntVector2 intVector in roomController2.potentialDoorPositions)
				{
					foreach (RoomSpawn roomSpawn2 in this.ec.GetPotentialRoomSpawnsAtCell(intVector))
					{
						WeightedRoomSpawn weightedRoomSpawn2 = new WeightedRoomSpawn();
						weightedRoomSpawn2.selection = roomSpawn2;
						weightedRoomSpawn2.weight = this.WeightFromPos(roomSpawn2.position, intVector);
						weightedRoomSpawn2.weight += this.WeightFromRoom(roomController2);
						this.potentialRoomSpawns.Add(weightedRoomSpawn2);
					}
				}
			}
		}
	}

	// Token: 0x060008E8 RID: 2280 RVA: 0x0002DE28 File Offset: 0x0002C028
	protected int PlotWeightFromPos(IntVector2 pos)
	{
		return 1;
	}

	// Token: 0x060008E9 RID: 2281 RVA: 0x0002DE2C File Offset: 0x0002C02C
	protected int WeightFromPos(IntVector2 pos, IntVector2 sourcePosition)
	{
		int num = 1;
		int num2 = Mathf.RoundToInt((0.5f - Mathf.Abs(((float)pos.x - (float)this.levelSize.x * 0.5f) / (float)this.levelSize.x) + (0.5f - Mathf.Abs(((float)pos.z - (float)this.levelSize.z * 0.5f) / (float)this.levelSize.z))) * this.ld.centerWeightMultiplier);
		num += num2;
		Mathf.RoundToInt(Mathf.Pow(this.ld.perimeterBase, (float)this.ec.CellsFromPerimeter(pos, 1).Count));
		if (this.roomSpawnWeightDijkstraMap.DirectionToSource(sourcePosition) != Direction.Null)
		{
			int num3 = Mathf.CeilToInt(Mathf.Pow((float)this.roomSpawnWeightDijkstraMap.Value(sourcePosition) * this.ld.dijkstraWeightValueMultiplier, this.ld.dijkstraWeightPower));
			num += num3;
		}
		return num;
	}

	// Token: 0x060008EA RID: 2282 RVA: 0x0002DF24 File Offset: 0x0002C124
	protected int WeightFromRoom(RoomController room)
	{
		return 0 + Mathf.RoundToInt((float)room.spawnWeight / (float)room.potentialDoorPositions.Count);
	}

	// Token: 0x060008EB RID: 2283 RVA: 0x0002DF44 File Offset: 0x0002C144
	public bool ProximityCheck(IntVector2 position, RoomType type, int buffer)
	{
		bool result = false;
		bool flag = false;
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < buffer; i++)
		{
			if (flag)
			{
				num2++;
			}
			else
			{
				num++;
			}
			flag = !flag;
		}
		for (int j = position.x - num; j <= position.x + num2; j++)
		{
			for (int k = position.z - num; k <= position.z + num2; k++)
			{
				if (this.ec.ContainsCoordinates(new IntVector2(j, k)) && !this.ec.cells[j, k].Null && this.ec.cells[j, k].room.type == type)
				{
					result = true;
				}
			}
		}
		return result;
	}

	// Token: 0x060008EC RID: 2284 RVA: 0x0002E010 File Offset: 0x0002C210
	public List<IntVector2> ProximityList(IntVector2 position, int buffer)
	{
		List<IntVector2> list = new List<IntVector2>();
		for (int i = position.x - buffer; i <= position.x + buffer; i++)
		{
			for (int j = position.z - buffer; j <= position.z + buffer; j++)
			{
				list.Add(new IntVector2(i, j));
			}
		}
		return list;
	}

	// Token: 0x060008ED RID: 2285 RVA: 0x0002E068 File Offset: 0x0002C268
	public void CreateArea(List<RoomController> list, RoomType type, RoomCategory category, int id, IntVector2 position, IntVector2 maxSize, bool createTile)
	{
		RoomController roomController = Object.Instantiate<RoomController>(this.roomControllerPre, this.ec.transform);
		list[id] = roomController;
		roomController.ec = this.ec;
		roomController.transform.position = this.ec.transform.position;
		roomController.type = type;
		roomController.category = category;
		roomController.position = position;
		roomController.size = new IntVector2(1, 1);
		roomController.maxSize = maxSize;
		roomController.transform.name = type.ToString() + "Controller(" + id.ToString() + ")";
		roomController.expandable = true;
		if (createTile)
		{
			this.ec.CreateCell(this.placeholderTileVal, list[id].transform, position, list[id]);
		}
	}

	// Token: 0x060008EE RID: 2286 RVA: 0x0002E148 File Offset: 0x0002C348
	public RoomController LoadRoom(RoomAsset asset, IntVector2 position, IntVector2 roomPivot, Direction direction, bool lockTiles)
	{
		RoomController roomController = Object.Instantiate<RoomController>(this.roomControllerPre, this.ec.transform);
		roomController.transform.name = asset.name;
		this.ec.rooms.Add(roomController);
		roomController.ec = this.ec;
		roomController.transform.position = this.ec.transform.position;
		RoomData roomData = RoomData.ConvertFromAsset(asset, position, roomPivot, direction);
		roomController.category = roomData.category;
		roomController.type = roomData.type;
		roomController.spawnWeight = asset.spawnWeight;
		roomController.dir = direction;
		roomController.color = roomData.color;
		roomController.mapMaterial = roomData.mapMaterial;
		roomController.florTex = roomData.florTex;
		roomController.wallTex = roomData.wallTex;
		roomController.ceilTex = roomData.ceilTex;
		roomController.doorMats = roomData.doorMats;
		roomController.windowObject = roomData.windowObject;
		roomController.offLimits = roomData.offLimits;
		roomController.itemList = roomData.itemList;
		roomController.minItemValue = roomData.minItemValue;
		roomController.maxItemValue = roomData.maxItemValue;
		roomController.windowChance = roomData.windowChance;
		roomController.potentialPosters = new List<WeightedPosterObject>(roomData.posters);
		roomController.posterChance = roomData.posterChance;
		roomController.itemSpawnPoints = roomData.itemSpawnPoints;
		roomController.potentialDoorPositions = roomData.potentialDoorPositions;
		roomController.forcedDoorPositions = roomData.forcedDoorPositions;
		roomController.entitySafeCells = roomData.entitySafeCells;
		roomController.connectionPoints = new List<IntVector2>(roomController.potentialDoorPositions);
		roomController.connectionPoints.AddRangeExceptDuplicates(roomController.forcedDoorPositions);
		roomController.eventSafeCells = roomData.eventSafeCells;
		roomController.standardLightCells = roomData.standardLightCells;
		roomController.lightPre = asset.lightPre;
		if (this.generateAtlases)
		{
			roomController.GenerateTextureAtlas();
		}
		this.LoadIntoExistingRoom(roomController, asset, position, roomPivot, direction, lockTiles);
		if (asset.hasActivity)
		{
			this.GenerateActivity(roomController, asset.activity);
		}
		if (!this.editorMode)
		{
			foreach (LightSourceData lightSourceData in asset.lights)
			{
				this.lightsToGenerate.Add(lightSourceData.GetNew());
				this.lightsToGenerate[this.lightsToGenerate.Count - 1].position = this.lightsToGenerate[this.lightsToGenerate.Count - 1].position.Adjusted(roomPivot, direction) + position;
			}
			foreach (PosterData posterData in asset.posterDatas)
			{
				this.postersToBuild.Add(posterData.GetNew());
				this.postersToBuild[this.postersToBuild.Count - 1].position = this.postersToBuild[this.postersToBuild.Count - 1].position.Adjusted(roomPivot, direction) + position;
				this.postersToBuild[this.postersToBuild.Count - 1].direction = this.postersToBuild[this.postersToBuild.Count - 1].direction.RotatedRelativeToNorth(direction);
			}
		}
		if (asset.roomFunctionContainer != null)
		{
			if (roomController.functionObject != null)
			{
				Object.Destroy(roomController.functionObject);
			}
			RoomFunctionContainer roomFunctionContainer = Object.Instantiate<RoomFunctionContainer>(asset.roomFunctionContainer, roomController.transform);
			roomFunctionContainer.Initialize(roomController);
			roomController.functionObject = roomFunctionContainer.gameObject;
			roomController.functions = roomFunctionContainer;
		}
		foreach (BasicObjectData basicObjectData in asset.basicObjects)
		{
			if (basicObjectData.prefab != null)
			{
				Transform transform = basicObjectData.prefab;
				if (basicObjectData.replaceable)
				{
					foreach (BasicObjectSwapData basicObjectSwapData in asset.basicSwaps)
					{
						if (basicObjectSwapData.prefabToSwap == transform)
						{
							if (this.controlledRNG.NextDouble() >= (double)basicObjectSwapData.chance)
							{
								break;
							}
							WeightedSelection<Transform>[] potentialReplacements = basicObjectSwapData.potentialReplacements;
							transform = WeightedSelection<Transform>.ControlledRandomSelection(potentialReplacements, this.controlledRNG);
						}
					}
				}
				Transform transform2 = Object.Instantiate<Transform>(transform, roomController.objectObject.transform);
				transform2.position = basicObjectData.position;
				transform2.rotation = basicObjectData.rotation;
				EnvironmentObject component = transform2.GetComponent<EnvironmentObject>();
				if (component != null)
				{
					component.Ec = this.ec;
					this.environmentObjects.Add(component);
				}
			}
		}
		foreach (ItemData itemData in asset.items)
		{
			this.CreateItem(roomController, itemData.item, itemData.position);
		}
		return roomController;
	}

	// Token: 0x060008EF RID: 2287 RVA: 0x0002E6E0 File Offset: 0x0002C8E0
	public void UpdateRoomObjectPosition(RoomController room)
	{
		room.objectObject.transform.rotation = room.dir.ToRotation();
		Vector3 position = default(Vector3);
		switch (room.dir)
		{
		case Direction.North:
			room.objectObject.transform.position = this.ec.RealRoomMin(room);
			return;
		case Direction.East:
			position.x = this.ec.RealRoomMin(room).x;
			position.z = this.ec.RealRoomMax(room).z;
			room.objectObject.transform.position = position;
			return;
		case Direction.South:
			room.objectObject.transform.position = this.ec.RealRoomMax(room);
			return;
		case Direction.West:
			position.x = this.ec.RealRoomMax(room).x;
			position.z = this.ec.RealRoomMin(room).z;
			room.objectObject.transform.position = position;
			return;
		default:
			return;
		}
	}

	// Token: 0x060008F0 RID: 2288 RVA: 0x0002E7EC File Offset: 0x0002C9EC
	public void LoadIntoExistingRoom(RoomController room, RoomAsset asset, IntVector2 position, IntVector2 roomPivot, Direction direction, bool lockTiles)
	{
		foreach (CellData cellData in asset.cells)
		{
			this.ec.CreateCell(Directions.RotateBin(cellData.type, direction), room.transform, cellData.pos.Adjusted(roomPivot, direction) + position, room, true, lockTiles);
		}
		foreach (IntVector2 intVector in asset.blockedWallCells)
		{
			this.ec.CellFromPosition(intVector.Adjusted(roomPivot, direction) + position).HardCoverEntirely();
		}
		foreach (IntVector2 intVector2 in asset.secretCells)
		{
			this.ec.CellFromPosition(intVector2.Adjusted(roomPivot, direction) + position).hideFromMap = true;
		}
		room.UpdatePosition();
	}

	// Token: 0x060008F1 RID: 2289 RVA: 0x0002E930 File Offset: 0x0002CB30
	protected bool RoomFits(RoomAsset roomAsset, IntVector2 position, IntVector2 pivot, Direction direction)
	{
		foreach (CellData cellData in roomAsset.cells)
		{
			if (!this.ec.ContainsCoordinates(cellData.pos.Adjusted(pivot, direction) + position) || !this.ec.CellFromPosition(cellData.pos.Adjusted(pivot, direction) + position).Null)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060008F2 RID: 2290 RVA: 0x0002E9CC File Offset: 0x0002CBCC
	protected void GenerateActivity(RoomController room, ActivityData data)
	{
		Notebook notebook = Object.Instantiate<Notebook>(this.notebookPre, room.objectObject.transform);
		Activity activity = Object.Instantiate<Activity>(data.prefab, room.objectObject.transform);
		activity.transform.position = data.position;
		activity.transform.rotation = data.direction.ToRotation();
		activity.room = room;
		activity.SetNotebook(notebook);
		notebook.transform.position = new Vector3(activity.transform.position.x, 5f, activity.transform.position.z);
		this.ec.AddNotebook(notebook);
		Singleton<BaseGameManager>.Instance.AddNotebookTotal(1);
		notebook.icon = this.map.AddIcon(notebook.iconPre, notebook.transform, Color.white);
	}

	// Token: 0x060008F3 RID: 2291 RVA: 0x0002EAAC File Offset: 0x0002CCAC
	public GameObject InstatiateEnvironmentObject(GameObject prefab, Cell cell, Direction dir)
	{
		GameObject gameObject = Object.Instantiate<GameObject>(prefab, cell.room.objectObject.transform);
		EnvironmentObject.GiveController(gameObject.transform, this.ec, this.environmentObjects);
		gameObject.transform.position = cell.FloorWorldPosition;
		gameObject.transform.rotation = dir.ToRotation();
		Renderer component = gameObject.GetComponent<Renderer>();
		if (component != null)
		{
			cell.renderers.Add(component);
		}
		return gameObject;
	}

	// Token: 0x060008F4 RID: 2292 RVA: 0x0002EB24 File Offset: 0x0002CD24
	protected void TextureArea(RoomController room, Texture2D wallTex, Texture2D floorTex, Texture2D ceilingTex)
	{
		room.wallTex = wallTex;
		room.florTex = floorTex;
		room.ceilTex = ceilingTex;
	}

	// Token: 0x060008F5 RID: 2293 RVA: 0x0002EB3C File Offset: 0x0002CD3C
	public List<Direction> GetPossibleDirections(RoomController room, int buffer)
	{
		List<Direction> list = new List<Direction>
		{
			Direction.North,
			Direction.East,
			Direction.South,
			Direction.West
		};
		IntVector2 intVector = room.position;
		IntVector2 intVector2 = room.size;
		for (int i = 0; i < list.Count; i++)
		{
			bool flag = false;
			IntVector2 intVector3 = list[i].ToIntVector2();
			for (int j = Mathf.Max(intVector2.x * intVector3.x + -1 * intVector3.x - buffer * Mathf.Abs(intVector3.z), 0 - buffer * Mathf.Abs(intVector3.z)); j < Mathf.Max(intVector2.x * (Mathf.Abs(intVector3.z) + intVector3.x) + buffer * Mathf.Abs(intVector3.z), 1); j++)
			{
				for (int k = Mathf.Max(intVector2.z * intVector3.z + -1 * intVector3.z - buffer * Mathf.Abs(intVector3.x), 0 - buffer * Mathf.Abs(intVector3.x)); k < Mathf.Max(intVector2.z * (Mathf.Abs(intVector3.x) + intVector3.z) + buffer * Mathf.Abs(intVector3.x), 1); k++)
				{
					IntVector2 intVector4 = new IntVector2(intVector.x + j + intVector3.x + buffer * intVector3.x, intVector.z + k + intVector3.z + buffer * intVector3.z);
					if (!this.ec.ContainsCoordinates(intVector4) || !this.ec.cells[intVector4.x, intVector4.z].Null)
					{
						flag = true;
					}
				}
			}
			if (flag)
			{
				list.RemoveAt(i);
				i--;
			}
		}
		if (room.size.x >= room.maxSize.x)
		{
			list.Remove(Direction.East);
			list.Remove(Direction.West);
		}
		if (room.size.z >= room.maxSize.z)
		{
			list.Remove(Direction.North);
			list.Remove(Direction.South);
		}
		new List<Direction>();
		List<Direction> list2 = new List<Direction>(list);
		List<Direction> list3 = new List<Direction>();
		if (list.Count > 1 && (list.Count != 2 || list[0].GetOpposite() != list[1]))
		{
			for (int l = 0; l < list.Count; l++)
			{
				Direction direction = list[l];
				list3 = new List<Direction>(list);
				list3.Remove(direction);
				switch (direction)
				{
				case Direction.North:
					intVector = room.position;
					intVector2 = room.size + direction.ToIntVector2();
					break;
				case Direction.East:
					intVector = room.position;
					intVector2 = room.size + direction.ToIntVector2();
					break;
				case Direction.South:
					intVector = room.position + direction.ToIntVector2();
					intVector2 = room.size - direction.ToIntVector2();
					break;
				case Direction.West:
					intVector = room.position + direction.ToIntVector2();
					intVector2 = room.size - direction.ToIntVector2();
					break;
				}
				for (int m = 0; m < list3.Count; m++)
				{
					bool flag = false;
					IntVector2 intVector5 = list3[m].ToIntVector2();
					for (int n = Mathf.Max(intVector2.x * intVector5.x + -1 * intVector5.x - buffer * Mathf.Abs(intVector5.z), 0 - buffer * Mathf.Abs(intVector5.z)); n < Mathf.Max(intVector2.x * (Mathf.Abs(intVector5.z) + intVector5.x) + buffer * Mathf.Abs(intVector5.z), 1); n++)
					{
						for (int num = Mathf.Max(intVector2.z * intVector5.z + -1 * intVector5.z - buffer * Mathf.Abs(intVector5.x), 0 - buffer * Mathf.Abs(intVector5.x)); num < Mathf.Max(intVector2.z * (Mathf.Abs(intVector5.x) + intVector5.z) + buffer * Mathf.Abs(intVector5.x), 1); num++)
						{
							IntVector2 intVector6 = new IntVector2(intVector.x + n + intVector5.x + buffer * intVector5.x, intVector.z + num + intVector5.z + buffer * intVector5.z);
							if (!this.ec.ContainsCoordinates(intVector6) || !this.ec.cells[intVector6.x, intVector6.z].Null)
							{
								flag = true;
							}
						}
					}
					if (flag)
					{
						list2.Remove(direction);
						break;
					}
				}
			}
			if (list2.Count > 0)
			{
				list.Clear();
				list.AddRange(list2);
			}
		}
		if (room.size.x > room.size.z && (list.Contains(Direction.North) || list.Contains(Direction.South)))
		{
			list.Remove(Direction.East);
			list.Remove(Direction.West);
		}
		else if (room.size.z > room.size.x && (list.Contains(Direction.East) || list.Contains(Direction.West)))
		{
			list.Remove(Direction.North);
			list.Remove(Direction.South);
		}
		return list;
	}

	// Token: 0x060008F6 RID: 2294 RVA: 0x0002F0D0 File Offset: 0x0002D2D0
	public void ExpandArea(RoomController room, Direction direction)
	{
		IntVector2 intVector = direction.ToIntVector2();
		IntVector2 size = room.size;
		IntVector2 position = room.position;
		for (int i = Mathf.Max(size.x * intVector.x + -1 * intVector.x, 0); i < Mathf.Max(size.x * (Mathf.Abs(intVector.z) + intVector.x), 1); i++)
		{
			for (int j = Mathf.Max(size.z * intVector.z + -1 * intVector.z, 0); j < Mathf.Max(size.z * (Mathf.Abs(intVector.x) + intVector.z), 1); j++)
			{
				IntVector2 position2 = new IntVector2(position.x + i + intVector.x, position.z + j + intVector.z);
				this.ec.CreateCell(this.placeholderTileVal, room.transform, position2, room);
			}
		}
		room.position = new IntVector2(room.position.x + Mathf.Min(intVector.x, 0), room.position.z + Mathf.Min(intVector.z, 0));
		room.size += new IntVector2(Mathf.Abs(intVector.x), Mathf.Abs(intVector.z));
	}

	// Token: 0x060008F7 RID: 2295 RVA: 0x0002F230 File Offset: 0x0002D430
	public void AddDoor(Cell doorTile, Door doorPre, Direction dir, bool addDirToRoom)
	{
		this.doorPlacements.Add(new DoorPlacement(doorTile.room, doorTile.room.doorPre, doorTile.position, dir));
		Cell cell = this.ec.cells[doorTile.position.x + dir.ToIntVector2().x, doorTile.position.z + dir.ToIntVector2().z];
		doorTile.doorHere = true;
		cell.doorHere = true;
		doorTile.doorDirs.Add(dir);
		cell.doorDirs.Add(dir.GetOpposite());
		if (addDirToRoom)
		{
			doorTile.room.doorDirs.Add(dir);
			cell.room.doorDirs.Add(dir.GetOpposite());
		}
	}

	// Token: 0x060008F8 RID: 2296 RVA: 0x0002F2FA File Offset: 0x0002D4FA
	public void DestroyRoom(RoomController room)
	{
		while (room.TileCount > 0)
		{
			room.ec.DestroyCell(room.TileAtIndex(0));
		}
		Object.Destroy(room.gameObject);
	}

	// Token: 0x060008F9 RID: 2297 RVA: 0x0002F324 File Offset: 0x0002D524
	protected void BuildDoorPlacement(DoorPlacement placement)
	{
		if (!(placement.doorPre == null))
		{
			Door door = Object.Instantiate<Door>(placement.doorPre);
			this.ec.SetupDoor(door, this.ec.cells[placement.position.x, placement.position.z], placement.dir);
			return;
		}
		if (placement.room.doorPre != null)
		{
			Door door2 = Object.Instantiate<Door>(placement.room.doorPre);
			this.ec.SetupDoor(door2, this.ec.cells[placement.position.x, placement.position.z], placement.dir);
			return;
		}
		Door door3 = Object.Instantiate<Door>(this.nullDoorPre);
		this.ec.SetupDoor(door3, this.ec.cells[placement.position.x, placement.position.z], placement.dir);
	}

	// Token: 0x060008FA RID: 2298 RVA: 0x0002F428 File Offset: 0x0002D628
	public List<Cell> MatchingAdjacentTiles(Cell tile)
	{
		List<Cell> list = new List<Cell>();
		for (int i = 0; i < 4; i++)
		{
			IntVector2 intVector = tile.position + Directions.FromInt(i).ToIntVector2();
			if (this.ec.ContainsCoordinates(intVector) && !this.ec.cells[intVector.x, intVector.z].Null)
			{
				Cell cell = this.ec.cells[intVector.x, intVector.z];
				if (cell.room == tile.room)
				{
					list.Add(cell);
				}
			}
		}
		return list;
	}

	// Token: 0x060008FB RID: 2299 RVA: 0x0002F4D0 File Offset: 0x0002D6D0
	public List<Direction> DirectionsToDestination(IntVector2 start, IntVector2 end)
	{
		List<Direction> list = new List<Direction>();
		if (start.x > end.x)
		{
			list.Add(Direction.West);
		}
		else if (start.x < end.x)
		{
			list.Add(Direction.East);
		}
		if (start.z > end.z)
		{
			list.Add(Direction.South);
		}
		else if (start.z < end.z)
		{
			list.Add(Direction.North);
		}
		return list;
	}

	// Token: 0x060008FC RID: 2300 RVA: 0x0002F53C File Offset: 0x0002D73C
	public List<Direction> PotentialPathDirections(IntVector2 pos)
	{
		List<Direction> list = new List<Direction>();
		for (int i = 0; i < 4; i++)
		{
			IntVector2 intVector = pos + Directions.FromInt(i).ToIntVector2();
			if (this.ec.ContainsCoordinates(intVector) && this.ec.cells[intVector.x, intVector.z].Null && this.DirectionSafe(pos, (Direction)i, ((Direction)i).GetOpposite()))
			{
				list.Add(Directions.FromInt(i));
			}
		}
		return list;
	}

	// Token: 0x060008FD RID: 2301 RVA: 0x0002F5BC File Offset: 0x0002D7BC
	public Queue<IntVector2> GetRandomPath(IntVector2 start, int turnChance, out bool success, out Cell tileToConnect, out Direction connectDir)
	{
		Queue<IntVector2> queue = new Queue<IntVector2>();
		tileToConnect = null;
		connectDir = Direction.Null;
		success = false;
		List<Direction> list = new List<Direction>(this.PotentialPathDirections(start));
		this.CheckDirections(start, list, Direction.Null);
		if (list.Count > 0)
		{
			Direction direction = list[this.controlledRNG.Next(0, list.Count)];
			Direction opposite = direction.GetOpposite();
			IntVector2 intVector = start + direction.ToIntVector2();
			int num = 0;
			while (this.ec.cells[intVector.x, intVector.z].Null)
			{
				queue.Enqueue(intVector);
				list = new List<Direction>();
				for (int i = 0; i < 4; i++)
				{
					if (Directions.FromInt(i) != opposite && Directions.FromInt(i) != direction.GetOpposite())
					{
						list.Add(Directions.FromInt(i));
					}
				}
				this.CheckDirections(intVector, list, opposite);
				if (list.Contains(direction))
				{
					if (list.Count > 1 && num * turnChance > this.controlledRNG.Next(0, 100) + 1)
					{
						list.Remove(direction);
						direction = list[this.controlledRNG.Next(0, list.Count)];
						num = 0;
					}
				}
				else if (list.Count > 0)
				{
					direction = list[this.controlledRNG.Next(0, list.Count)];
				}
				if (list.Count <= 0)
				{
					break;
				}
				intVector += direction.ToIntVector2();
				num++;
			}
			if (!this.ec.cells[intVector.x, intVector.z].Null && this.ec.cells[intVector.x, intVector.z].room.category != RoomCategory.Buffer)
			{
				success = true;
				tileToConnect = this.ec.cells[intVector.x, intVector.z];
				connectDir = direction;
			}
		}
		else
		{
			Debug.Log(string.Format("Tried making a path, but no suitable directions were found to travel in from {0},{1}.", start.x, start.z));
		}
		return queue;
	}

	// Token: 0x060008FE RID: 2302 RVA: 0x0002F7D8 File Offset: 0x0002D9D8
	public bool DirectionSafe(IntVector2 initPos, Direction dir, Direction noDir)
	{
		bool flag = false;
		IntVector2 intVector = dir.ToIntVector2();
		IntVector2 intVector2 = initPos + intVector;
		if (this.ec.ContainsCoordinates(intVector2) && ((!this.ec.CellFromPosition(intVector2).Null && this.ec.CellFromPosition(intVector2).room.category != RoomCategory.Buffer) || this.ec.CellFromPosition(intVector2).Null))
		{
			int num = Mathf.Max(0, intVector2.x * intVector.x);
			int num2 = Mathf.Max(intVector2.x, (this.levelSize.x - 1) * (intVector.x + Mathf.Abs(intVector.z)));
			int num3 = Mathf.Max(0, intVector2.z * intVector.z);
			int num4 = Mathf.Max(intVector2.z, (this.levelSize.z - 1) * (intVector.z + Mathf.Abs(intVector.x)));
			switch (noDir)
			{
			case Direction.North:
				num4 = intVector2.z;
				break;
			case Direction.East:
				num2 = intVector2.x;
				break;
			case Direction.South:
				num3 = intVector2.z;
				break;
			case Direction.West:
				num = intVector2.x;
				break;
			}
			for (int i = num; i <= num2; i++)
			{
				for (int j = num3; j <= num4; j++)
				{
					if (this.ec.ContainsCoordinates(new IntVector2(i, j)) && !this.ec.cells[i, j].Null)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					break;
				}
			}
		}
		return flag;
	}

	// Token: 0x060008FF RID: 2303 RVA: 0x0002F968 File Offset: 0x0002DB68
	protected void CheckDirections(IntVector2 pos, List<Direction> dirs, Direction noDir)
	{
		for (int i = 0; i < dirs.Count; i++)
		{
			if (!this.DirectionSafe(pos, dirs[i], noDir))
			{
				dirs.RemoveAt(i);
				i--;
			}
		}
	}

	// Token: 0x06000900 RID: 2304 RVA: 0x0002F9A4 File Offset: 0x0002DBA4
	protected void CreateElevator(IntVector2 pos, Direction dir, Elevator elevatorPrefab, RoomAsset elevatorRoomAsset, bool isSpawn)
	{
		this.LoadIntoExistingRoom(this.ec.CellFromPosition(pos + dir.ToIntVector2()).room, elevatorRoomAsset, pos, elevatorRoomAsset.potentialDoorPositions[0], dir, true);
		Elevator elevator = Object.Instantiate<Elevator>(elevatorPrefab, this.ec.transform);
		elevator.transform.localPosition = new Vector3((float)pos.x * 10f + 5f, 0f, (float)pos.z * 10f + 5f);
		elevator.transform.localRotation = dir.ToRotation();
		elevator.Door.ec = this.ec;
		elevator.Door.position = pos;
		elevator.Door.bOffset = dir.GetOpposite().ToIntVector2();
		elevator.Door.direction = dir.GetOpposite();
		elevator.IsSpawn = isSpawn;
		elevator.Initialize(this.ec);
		this.ec.CellFromPosition(pos + dir.GetOpposite().ToIntVector2()).HideTile(true);
		this.ec.CellFromPosition(pos + dir.GetOpposite().ToIntVector2()).offLimits = true;
	}

	// Token: 0x06000901 RID: 2305 RVA: 0x0002FAE0 File Offset: 0x0002DCE0
	protected void BlockTilesToBlock()
	{
		for (int i = 0; i < this.tilesToBlock.Count; i++)
		{
			this.ec.CellFromPosition(this.tilesToBlock[i]).Block(this.dirsToBlock[i], true);
		}
	}

	// Token: 0x170000C2 RID: 194
	// (get) Token: 0x06000902 RID: 2306 RVA: 0x0002FB2C File Offset: 0x0002DD2C
	// (set) Token: 0x06000903 RID: 2307 RVA: 0x0002FB34 File Offset: 0x0002DD34
	public EnvironmentController Ec
	{
		get
		{
			return this.ec;
		}
		set
		{
			this.ec = value;
		}
	}

	// Token: 0x06000904 RID: 2308 RVA: 0x0002FB40 File Offset: 0x0002DD40
	public Cell[] edgeTilesNorth()
	{
		Cell[] array = new Cell[this.levelSize.x];
		for (int i = this.levelSize.z - 1; i > 0; i--)
		{
			for (int j = 0; j < this.levelSize.x; j++)
			{
				if (!this.ec.cells[j, i].Null && array[j] == null)
				{
					array[j] = this.ec.cells[j, i];
				}
			}
		}
		return array;
	}

	// Token: 0x06000905 RID: 2309 RVA: 0x0002FBC0 File Offset: 0x0002DDC0
	public Cell[] edgeTilesSouth()
	{
		Cell[] array = new Cell[this.levelSize.x];
		for (int i = 0; i < this.levelSize.z; i++)
		{
			for (int j = 0; j < this.levelSize.x; j++)
			{
				if (!this.ec.cells[j, i].Null && array[j] == null)
				{
					array[j] = this.ec.cells[j, i];
				}
			}
		}
		return array;
	}

	// Token: 0x06000906 RID: 2310 RVA: 0x0002FC40 File Offset: 0x0002DE40
	public Cell[] edgeTilesEast()
	{
		Cell[] array = new Cell[this.levelSize.z];
		for (int i = this.levelSize.x - 1; i > 0; i--)
		{
			for (int j = 0; j < this.levelSize.z; j++)
			{
				if (!this.ec.cells[i, j].Null && array[j] == null)
				{
					array[j] = this.ec.cells[i, j];
				}
			}
		}
		return array;
	}

	// Token: 0x06000907 RID: 2311 RVA: 0x0002FCC0 File Offset: 0x0002DEC0
	public Cell[] edgeTilesWest()
	{
		Cell[] array = new Cell[this.levelSize.z];
		for (int i = 0; i < this.levelSize.x; i++)
		{
			for (int j = 0; j < this.levelSize.z; j++)
			{
				if (!this.ec.cells[i, j].Null && array[j] == null)
				{
					array[j] = this.ec.cells[i, j];
				}
			}
		}
		return array;
	}

	// Token: 0x06000908 RID: 2312 RVA: 0x0002FD40 File Offset: 0x0002DF40
	public void CreateItem(RoomController room, ItemObject item, Vector2 position)
	{
		Pickup pickup = Object.Instantiate<Pickup>(this.pickupPre, room.objectObject.transform);
		pickup.transform.position = new Vector3(position.x, 5f, position.y);
		pickup.item = item;
		this.ec.items.Add(pickup);
		pickup.icon = this.ec.map.AddIcon(pickup.iconPre, pickup.transform, Color.white);
		room.currentItemValue += item.value;
	}

	// Token: 0x06000909 RID: 2313 RVA: 0x0002FDD8 File Offset: 0x0002DFD8
	public void PlaceItemInRandomRoom(ItemObject item, bool highEnd, bool lowEnd, Random rng)
	{
		List<WeightedRoomController> list = new List<WeightedRoomController>();
		foreach (RoomController roomController in this.ec.rooms)
		{
			if (((roomController.category != RoomCategory.Class && !lowEnd) || (roomController.category == RoomCategory.Class && !highEnd)) && roomController.itemSpawnPoints.Count > 0)
			{
				list.Add(new WeightedRoomController());
				list[list.Count - 1].selection = roomController;
				list[list.Count - 1].weight = Mathf.Max(roomController.maxItemValue - roomController.currentItemValue, 1);
			}
		}
		if (list.Count > 0)
		{
			RoomController room = WeightedSelection<RoomController>.ControlledRandomSelectionList(WeightedRoomController.Convert(list), rng);
			this.PlaceItemInRoom(item, room, rng);
		}
	}

	// Token: 0x0600090A RID: 2314 RVA: 0x0002FEBC File Offset: 0x0002E0BC
	public void PlaceItemInRoom(ItemObject item, RoomController room, Random rng)
	{
		WeightedInt[] array = new WeightedInt[room.itemSpawnPoints.Count];
		for (int i = 0; i < room.itemSpawnPoints.Count; i++)
		{
			array[i] = new WeightedInt();
			array[i].selection = i;
			array[i].weight = room.itemSpawnPoints[i].weight;
		}
		WeightedSelection<int>[] items = array;
		int index = WeightedSelection<int>.ControlledRandomSelection(items, rng);
		this.CreateItem(room, item, room.itemSpawnPoints[index].position);
		room.itemSpawnPoints.RemoveAt(index);
	}

	// Token: 0x0600090B RID: 2315 RVA: 0x0002FF49 File Offset: 0x0002E149
	public void CreateRandomItem(RoomController room, List<WeightedItemObject> potentialItems, Vector2 position, Random rng)
	{
		if (potentialItems.Count > 0)
		{
			this.CreateItem(room, WeightedSelection<ItemObject>.ControlledRandomSelectionList(WeightedItemObject.Convert(potentialItems), rng), position);
		}
	}

	// Token: 0x0600090C RID: 2316 RVA: 0x0002FF6C File Offset: 0x0002E16C
	public void CreateRandomItemsInRoom(RoomController room, WeightedItemObject[] primaryItemArray, Random rng)
	{
		Debug.LogWarning("THIS FUNCTION HAS BEEN DEPRECATED. USE LOOT TABLES INSTEAD.");
	}

	// Token: 0x0600090D RID: 2317 RVA: 0x0002FF84 File Offset: 0x0002E184
	public bool BuildDoorIfPossible(IntVector2 position, RoomController room, bool hallsOnly, bool forceDoor, out RoomController otherRoom, out IntVector2 otherPosition)
	{
		otherRoom = null;
		otherPosition = default(IntVector2);
		bool flag = this.controlledRNG.NextDouble() < (double)((float)room.roomsFromHall / ((float)room.roomsFromHall + this.ld.hallPriorityDampening));
		Direction randomPotentialDoorDirection = this.GetRandomPotentialDoorDirection(room, position, flag || hallsOnly, !hallsOnly, false, forceDoor, this.controlledRNG);
		if (randomPotentialDoorDirection == Direction.Null && flag && !hallsOnly)
		{
			randomPotentialDoorDirection = this.GetRandomPotentialDoorDirection(room, position, false, true, true, forceDoor, this.controlledRNG);
		}
		if (randomPotentialDoorDirection != Direction.Null)
		{
			DoorPlacement doorPlacement = new DoorPlacement(room, room.doorPre, position, randomPotentialDoorDirection);
			this.ec.ConnectCells(position, doorPlacement.dir);
			room.builtDoorPositions.Add(position);
			this.doorPlacements.Add(doorPlacement);
			otherPosition = position + randomPotentialDoorDirection.ToIntVector2();
			otherRoom = this.ec.CellFromPosition(otherPosition).room;
			otherRoom.builtDoorPositions.Add(otherPosition);
			return true;
		}
		return false;
	}

	// Token: 0x0600090E RID: 2318 RVA: 0x00030080 File Offset: 0x0002E280
	public Direction GetRandomPotentialDoorDirection(RoomController sourceRoom, IntVector2 position, bool onlyHalls, bool limitHalls, bool excludeHalls, bool forceDoor, Random rng)
	{
		List<Direction> list = new List<Direction>();
		for (int i = 0; i < 4; i++)
		{
			Cell cell = this.ec.CellFromPosition(position + ((Direction)i).ToIntVector2());
			if (!cell.Null && !cell.offLimits && cell.room != sourceRoom && !sourceRoom.connectedRooms.Contains(cell.room) && (cell.room.type == RoomType.Hall || cell.room.ContainsDoorPosition(position + ((Direction)i).ToIntVector2())))
			{
				if (this.ec.CellFromPosition(position + ((Direction)i).ToIntVector2()).room.type == RoomType.Hall)
				{
					if (!limitHalls || (!excludeHalls && (sourceRoom.roomsFromHall > 0 || this.AdditionalHallDoorPositionIsFarEnoughAway(sourceRoom, cell, this.ec.CellFromPosition(position)) || forceDoor)))
					{
						list.Add((Direction)i);
					}
				}
				else if (!onlyHalls)
				{
					list.Add((Direction)i);
				}
			}
		}
		if (list.Count > 0)
		{
			return list[rng.Next(0, list.Count)];
		}
		return Direction.Null;
	}

	// Token: 0x0600090F RID: 2319 RVA: 0x000301A4 File Offset: 0x0002E3A4
	public bool AdditionalHallDoorPositionIsFarEnoughAway(RoomController room, Cell outsideCell, Cell insideCell)
	{
		foreach (IntVector2 startPos in room.builtDoorPositions)
		{
			float num = (float)this.ec.NavigableDistance(startPos, insideCell.position, PathType.Const) * this.ld.additionalHallDoorRequirementMultiplier;
			int num2 = this.ec.NavigableDistance(startPos, outsideCell.position, PathType.Const);
			if (num > (float)num2)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06000910 RID: 2320 RVA: 0x00030230 File Offset: 0x0002E430
	public bool BuildDoorInDirection(RoomController room, IntVector2 position, Direction direction, bool ignoreDoorSpawns, out RoomController otherRoom)
	{
		otherRoom = null;
		if (this.ec.ContainsCoordinates(position + direction.ToIntVector2()) && !this.ec.CellFromPosition(position + direction.ToIntVector2()).Null)
		{
			Cell cell = this.ec.CellFromPosition(position + direction.ToIntVector2());
			if (ignoreDoorSpawns || cell.room.type == RoomType.Hall || cell.room.ContainsDoorPosition(position + direction.ToIntVector2()))
			{
				DoorPlacement item = new DoorPlacement(room, room.doorPre, position, direction);
				this.ec.ConnectCells(position, direction);
				this.doorPlacements.Add(item);
				otherRoom = cell.room;
				room.builtDoorPositions.Add(position);
				otherRoom.builtDoorPositions.Add(position + direction.ToIntVector2());
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000911 RID: 2321 RVA: 0x0003031C File Offset: 0x0002E51C
	public bool BuildWindowIfPossible(IntVector2 position, RoomController room, out RoomController otherRoom, out IntVector2 otherPosition)
	{
		otherRoom = null;
		otherPosition = default(IntVector2);
		this.controlledRNG.NextDouble();
		float num = (float)room.roomsFromHall / ((float)room.roomsFromHall + this.ld.hallPriorityDampening);
		Direction randomPotentialWindowDirection = this.GetRandomPotentialWindowDirection(room, position, this.controlledRNG);
		if (randomPotentialWindowDirection != Direction.Null)
		{
			this.ec.BuildWindow(this.ec.CellFromPosition(position), randomPotentialWindowDirection, room.windowObject);
			otherPosition = position + randomPotentialWindowDirection.ToIntVector2();
			otherRoom = this.ec.CellFromPosition(otherPosition).room;
			return true;
		}
		return false;
	}

	// Token: 0x06000912 RID: 2322 RVA: 0x000303BC File Offset: 0x0002E5BC
	public Direction GetRandomPotentialWindowDirection(RoomController sourceRoom, IntVector2 position, Random rng)
	{
		List<Direction> list = new List<Direction>();
		for (int i = 0; i < 4; i++)
		{
			Cell cell = this.ec.CellFromPosition(position + ((Direction)i).ToIntVector2());
			if (!cell.Null && cell.room != sourceRoom && !cell.WallHardCovered(((Direction)i).GetOpposite()))
			{
				list.Add((Direction)i);
			}
		}
		if (list.Count > 0)
		{
			return list[rng.Next(0, list.Count)];
		}
		return Direction.Null;
	}

	// Token: 0x170000C3 RID: 195
	// (get) Token: 0x06000913 RID: 2323 RVA: 0x0003043C File Offset: 0x0002E63C
	// (set) Token: 0x06000914 RID: 2324 RVA: 0x00030444 File Offset: 0x0002E644
	public bool EditorMode
	{
		get
		{
			return this.editorMode;
		}
		set
		{
			this.editorMode = value;
		}
	}

	// Token: 0x040009C8 RID: 2504
	protected int placeholderTileVal;

	// Token: 0x040009C9 RID: 2505
	public LevelObject ld;

	// Token: 0x040009CA RID: 2506
	public LevelAsset levelAsset;

	// Token: 0x040009CB RID: 2507
	public ExtraLevelDataAsset extraAsset;

	// Token: 0x040009CC RID: 2508
	public LevelDataContainer levelContainer;

	// Token: 0x040009CD RID: 2509
	public int seed;

	// Token: 0x040009CE RID: 2510
	public int seedOffset;

	// Token: 0x040009CF RID: 2511
	public bool useSeed;

	// Token: 0x040009D0 RID: 2512
	public bool generateOnStart;

	// Token: 0x040009D1 RID: 2513
	public float generationDelay;

	// Token: 0x040009D2 RID: 2514
	public float mapSize;

	// Token: 0x040009D3 RID: 2515
	protected DijkstraMap roomSpawnWeightDijkstraMap;

	// Token: 0x040009D4 RID: 2516
	protected List<IntVector2> previousRoomSpawnPositions = new List<IntVector2>();

	// Token: 0x040009D5 RID: 2517
	public Map mapPref;

	// Token: 0x040009D6 RID: 2518
	public MapTile mapTilePref;

	// Token: 0x040009D7 RID: 2519
	protected Map map;

	// Token: 0x040009D8 RID: 2520
	public Random controlledRNG;

	// Token: 0x040009D9 RID: 2521
	public EnvironmentController environmentControllerPre;

	// Token: 0x040009DA RID: 2522
	protected EnvironmentController ec;

	// Token: 0x040009DB RID: 2523
	public Transform navQuadPre;

	// Token: 0x040009DC RID: 2524
	public RoomController roomControllerPre;

	// Token: 0x040009DD RID: 2525
	public List<RoomController> plots;

	// Token: 0x040009DE RID: 2526
	public List<RoomController> halls;

	// Token: 0x040009DF RID: 2527
	public List<RoomController> specialRooms;

	// Token: 0x040009E0 RID: 2528
	protected List<WeightedRoomSpawn> potentialRoomSpawns = new List<WeightedRoomSpawn>();

	// Token: 0x040009E1 RID: 2529
	public List<Pickup> pickups;

	// Token: 0x040009E2 RID: 2530
	protected List<IntVector2> tilesToBlock = new List<IntVector2>();

	// Token: 0x040009E3 RID: 2531
	protected List<Direction> dirsToBlock = new List<Direction>();

	// Token: 0x040009E4 RID: 2532
	protected List<LightSourceData> lightsToGenerate = new List<LightSourceData>();

	// Token: 0x040009E5 RID: 2533
	protected List<PosterData> postersToBuild = new List<PosterData>();

	// Token: 0x040009E6 RID: 2534
	public Pickup pickupPre;

	// Token: 0x040009E7 RID: 2535
	public Notebook notebookPre;

	// Token: 0x040009E8 RID: 2536
	public Door nullDoorPre;

	// Token: 0x040009E9 RID: 2537
	protected List<DoorPlacement> doorPlacements = new List<DoorPlacement>();

	// Token: 0x040009EA RID: 2538
	protected List<Door> standardDoors = new List<Door>();

	// Token: 0x040009EB RID: 2539
	protected List<EnvironmentObject> environmentObjects = new List<EnvironmentObject>();

	// Token: 0x040009EC RID: 2540
	[SerializeField]
	private TicksAtBeginningOfFrame TicksAtBeginningOfFrame;

	// Token: 0x040009ED RID: 2541
	private long frameTickLimit = 100000L;

	// Token: 0x040009EE RID: 2542
	public IntVector2 levelSize;

	// Token: 0x040009EF RID: 2543
	private int framesSinceLastYield;

	// Token: 0x040009F0 RID: 2544
	public bool levelCreated;

	// Token: 0x040009F1 RID: 2545
	public bool levelInProgress;

	// Token: 0x040009F2 RID: 2546
	public bool generateAtlases = true;

	// Token: 0x040009F3 RID: 2547
	private bool error;

	// Token: 0x040009F4 RID: 2548
	protected bool editorMode;
}
