using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200018F RID: 399
public class LevelLoader : LevelBuilder
{
	// Token: 0x170000C4 RID: 196
	// (set) Token: 0x06000920 RID: 2336 RVA: 0x00030971 File Offset: 0x0002EB71
	public LevelData LevelData
	{
		set
		{
			this.levelData = value;
		}
	}

	// Token: 0x06000921 RID: 2337 RVA: 0x0003097C File Offset: 0x0002EB7C
	public override void StartGenerate()
	{
		this.levelCreated = false;
		this.levelInProgress = true;
		this.controlledRNG = new Random(Singleton<CoreGameManager>.Instance.Seed());
		base.StartGenerate();
		LevelData levelData = new LevelData();
		if (this.levelData == null)
		{
			if (this.levelAsset != null)
			{
				levelData = LevelData.ConvertFromAsset(this.levelAsset);
				if (this.extraAsset != null)
				{
					levelData.extraData = ExtraLevelData.ConvertFromAsset(this.extraAsset);
				}
				else
				{
					levelData.extraData = new ExtraLevelData();
				}
				this.dataName = this.levelAsset.name;
			}
			else if (this.levelContainer != null)
			{
				levelData = LevelData.ConvertFromContainer(this.levelContainer);
				if (this.extraAsset != null)
				{
					levelData.extraData = ExtraLevelData.ConvertFromAsset(this.extraAsset);
				}
				else
				{
					levelData.extraData = new ExtraLevelData();
				}
				this.dataName = this.levelContainer.name;
			}
		}
		else
		{
			levelData = this.levelData;
		}
		if (!this.editorMode)
		{
			Singleton<BaseGameManager>.Instance.PrepareLevelData(levelData);
		}
		base.StartCoroutine(this.Load(levelData));
	}

	// Token: 0x06000922 RID: 2338 RVA: 0x00030A9D File Offset: 0x0002EC9D
	private IEnumerator Load(LevelData data)
	{
		Debug.Log("Level loader loading " + this.dataName + ".");
		if (base.FrameShouldEnd())
		{
			yield return null;
		}
		Debug.Log("Level loader initializing level.");
		this.ec.spawnPoint = data.spawnPoint;
		this.ec.spawnRotation = data.spawnDirection.ToRotation();
		this.ec.levelSize = data.levelSize;
		this.ec.InitializeCells(this.ec.levelSize);
		this.levelSize = this.ec.levelSize;
		if (!this.editorMode)
		{
			this.ec.standardDarkLevel = data.extraData.minLightColor;
			this.ec.lightMode = data.extraData.lightMode;
			this.ec.InitializeLighting();
		}
		if (base.FrameShouldEnd())
		{
			yield return null;
		}
		base.CreateMap();
		if (!this.editorMode)
		{
			Debug.Log("Level loader initializing heap.");
			this.ec.InitializeHeap();
			Debug.Log("Heap initialized.");
		}
		if (base.FrameShouldEnd())
		{
			yield return null;
		}
		Debug.Log("Level loader setting up rooms.");
		foreach (RoomData roomData in data.rooms)
		{
			this.ec.rooms.Add(null);
			base.CreateArea(this.ec.rooms, roomData.type, roomData.category, this.ec.rooms.Count - 1, new IntVector2(0, 0), new IntVector2(0, 0), false);
			RoomController roomController = this.ec.rooms[this.ec.rooms.Count - 1];
			roomController.transform.name = "Room" + (this.ec.rooms.Count - 1).ToString() + "_" + roomData.name;
			roomController.florTex = roomData.florTex;
			roomController.wallTex = roomData.wallTex;
			roomController.ceilTex = roomData.ceilTex;
			roomController.GenerateTextureAtlas();
			roomController.doorMats = roomData.doorMats;
			roomController.offLimits = roomData.offLimits;
			roomController.entitySafeCells = roomData.entitySafeCells;
			roomController.eventSafeCells = roomData.eventSafeCells;
			roomController.itemSpawnPoints = roomData.itemSpawnPoints;
			roomController.minItemValue = roomData.minItemValue;
			roomController.maxItemValue = roomData.maxItemValue;
			roomController.itemList = roomData.itemList;
			roomController.color = roomData.color;
			roomController.mapMaterial = roomData.mapMaterial;
			if (roomController.category == RoomCategory.Office)
			{
				this.ec.offices.Add(roomController);
			}
			if (roomController.type == RoomType.Hall)
			{
				this.ec.mainHall = roomController;
			}
			if (base.FrameShouldEnd())
			{
				yield return null;
			}
		}
		List<RoomData>.Enumerator enumerator = default(List<RoomData>.Enumerator);
		if (base.FrameShouldEnd())
		{
			yield return null;
		}
		Debug.Log("Level loader loading tile data.");
		foreach (CellData cellData in data.tile)
		{
			int num = 0;
			int num2 = 64;
			if (cellData.type != 16)
			{
				this.ec.CreateCell(cellData.type, this.ec.rooms[cellData.roomId].transform, cellData.pos, this.ec.rooms[cellData.roomId]);
			}
			if (num + 1 >= num2 && base.FrameShouldEnd())
			{
				yield return null;
			}
		}
		CellData[] array = null;
		Debug.Log("Tile data loaded.");
		if (base.FrameShouldEnd())
		{
			yield return null;
		}
		if (!this.editorMode)
		{
			Debug.Log("Level loader loading additional room assets.");
			foreach (RoomAssetPlacementData roomAssetPlacementData in data.roomAssetsPlacements)
			{
				base.LoadRoom(roomAssetPlacementData.room, roomAssetPlacementData.position, roomAssetPlacementData.room.potentialDoorPositions[roomAssetPlacementData.doorSpawnId], roomAssetPlacementData.direction, false);
			}
			if (base.FrameShouldEnd())
			{
				yield return null;
			}
		}
		Debug.Log("Level loader setting room size and position data.");
		int j;
		for (int i = 0; i < this.ec.rooms.Count; i = j + 1)
		{
			RoomController roomController2 = this.ec.rooms[i];
			roomController2.UpdatePosition();
			base.UpdateRoomObjectPosition(roomController2);
			if (base.FrameShouldEnd())
			{
				yield return null;
			}
			j = i;
		}
		Debug.Log("Level loader creating exits.");
		if (!this.editorMode)
		{
			foreach (ExitData exitData in data.exits)
			{
				base.CreateElevator(exitData.position, exitData.direction, exitData.prefab, exitData.room, exitData.spawn);
				if (exitData.spawn)
				{
					this.ec.spawnPoint = this.ec.elevators[this.ec.elevators.Count - 1].transform.position + Vector3.up * 5f + -this.ec.elevators[this.ec.elevators.Count - 1].transform.forward * 10f;
					this.ec.spawnRotation = this.ec.elevators[this.ec.elevators.Count - 1].transform.rotation;
				}
				if (base.FrameShouldEnd())
				{
					yield return null;
				}
			}
			List<ExitData>.Enumerator enumerator3 = default(List<ExitData>.Enumerator);
		}
		Debug.Log("Level loader building doors.");
		if (!this.editorMode)
		{
			foreach (DoorData doorData in data.doors)
			{
				DoorPlacement placement = new DoorPlacement(this.ec.rooms[doorData.roomId], doorData.doorPre, doorData.position, doorData.dir);
				this.ec.ConnectCells(doorData.position, doorData.dir);
				base.BuildDoorPlacement(placement);
			}
		}
		if (base.FrameShouldEnd())
		{
			yield return null;
		}
		Debug.Log("Level loader building windows.");
		foreach (WindowData windowData in data.windows)
		{
			if (this.ec.CellFromPosition(windowData.position) != null)
			{
				this.ec.BuildWindow(this.ec.CellFromPosition(windowData.position), windowData.direction, windowData.window, this.editorMode);
			}
		}
		if (base.FrameShouldEnd())
		{
			yield return null;
		}
		Debug.Log("Level loader setting up room functions.");
		if (!this.editorMode)
		{
			for (int k = 0; k < this.ec.rooms.Count; k++)
			{
				if (k < data.rooms.Count && data.rooms[k].roomFunctionContainer != null)
				{
					if (this.ec.rooms[k].functionObject != null)
					{
						Object.Destroy(this.ec.rooms[k].functionObject);
					}
					RoomFunctionContainer roomFunctionContainer = Object.Instantiate<RoomFunctionContainer>(data.rooms[k].roomFunctionContainer, this.ec.rooms[k].transform);
					roomFunctionContainer.Initialize(this.ec.rooms[k]);
					this.ec.rooms[k].functionObject = roomFunctionContainer.gameObject;
					this.ec.rooms[k].functions = roomFunctionContainer;
				}
			}
		}
		if (base.FrameShouldEnd())
		{
			yield return null;
		}
		Debug.Log("Level loader calculating open areas.");
		if (!this.editorMode)
		{
			Cell[] array2 = new Cell[4];
			List<Direction>[] array3 = new List<Direction>[]
			{
				new List<Direction>(),
				new List<Direction>(),
				new List<Direction>(),
				new List<Direction>()
			};
			array3[0].Add(Direction.North);
			array3[0].Add(Direction.East);
			array3[1].Add(Direction.North);
			array3[1].Add(Direction.West);
			array3[2].Add(Direction.South);
			array3[2].Add(Direction.East);
			array3[3].Add(Direction.South);
			array3[3].Add(Direction.West);
			List<Direction> list = new List<Direction>();
			for (int l = 0; l < this.levelSize.x - 1; l++)
			{
				for (int m = 0; m < this.levelSize.z - 1; m++)
				{
					bool flag = true;
					bool @null = this.ec.cells[l, m].Null;
					array2[0] = this.ec.cells[l, m];
					array2[1] = this.ec.cells[l + 1, m];
					array2[2] = this.ec.cells[l, m + 1];
					array2[3] = this.ec.cells[l + 1, m + 1];
					for (int n = 0; n < 4; n++)
					{
						if (array2[n].excludeFromOpen || array2[n].Null != @null)
						{
							flag = false;
							break;
						}
						Directions.FillOpenDirectionsFromBin(list, array2[n].NavBin);
						if (!list.Contains(array3[n][0]) || !list.Contains(array3[n][1]))
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						foreach (Cell cell in array2)
						{
							cell.open = true;
							for (int num3 = 0; num3 < array2.Length; num3++)
							{
								if (!cell.openTiles.Contains(array2[num3]) && cell != array2[num3])
								{
									cell.openTiles.Add(array2[num3]);
								}
							}
						}
						Object.Instantiate<Transform>(this.navQuadPre, this.ec.navMeshTransform).transform.localPosition = new Vector3((float)l * 10f + 10f, 0f, (float)m * 10f + 10f);
					}
				}
			}
			List<OpenTileGroup> openTileGroups = new List<OpenTileGroup>();
			Queue<Cell> openTilesToCheck = new Queue<Cell>();
			for (int i = 0; i < this.levelSize.x; i = j + 1)
			{
				for (int z = 0; z < this.levelSize.z; z = j + 1)
				{
					Cell cell2 = this.ec.cells[i, z];
					if (cell2.open)
					{
						bool flag2 = false;
						foreach (OpenTileGroup openTileGroup in openTileGroups)
						{
							if (cell2.openTileGroup == openTileGroup)
							{
								flag2 = true;
							}
						}
						if (!flag2)
						{
							OpenTileGroup currentGroup = new OpenTileGroup();
							openTileGroups.Add(currentGroup);
							openTilesToCheck.Clear();
							openTilesToCheck.Enqueue(cell2);
							while (openTilesToCheck.Count > 0)
							{
								cell2 = openTilesToCheck.Dequeue();
								cell2.openTileGroup = currentGroup;
								currentGroup.cells.Add(cell2);
								foreach (Cell cell3 in cell2.openTiles)
								{
									if (cell3.openTileGroup != currentGroup && !openTilesToCheck.Contains(cell3))
									{
										openTilesToCheck.Enqueue(cell3);
									}
								}
							}
							if (base.FrameShouldEnd())
							{
								yield return null;
							}
							foreach (Cell cell4 in currentGroup.cells)
							{
								foreach (Direction direction in Directions.OpenDirectionsFromBin(cell4.ConstBin))
								{
									if (this.ec.ContainsCoordinates(cell4.position + direction.ToIntVector2()) && !this.ec.CellFromPosition(cell4.position + direction.ToIntVector2()).Null && !currentGroup.cells.Contains(this.ec.CellFromPosition(cell4.position + direction.ToIntVector2())))
									{
										currentGroup.exits.Add(new OpenGroupExit(cell4, direction));
									}
								}
							}
							if (base.FrameShouldEnd())
							{
								yield return null;
							}
							currentGroup = null;
						}
					}
					j = z;
				}
				if (base.FrameShouldEnd())
				{
					yield return null;
				}
				j = i;
			}
			this.ec.BuildNavMesh();
			openTileGroups = null;
			openTilesToCheck = null;
		}
		if (base.FrameShouldEnd())
		{
			yield return null;
		}
		Debug.Log("Level loader blocking tiles marked to be blocked.");
		base.BlockTilesToBlock();
		Debug.Log("Level loader building buttons.");
		List<GameButtonBase> buttons = new List<GameButtonBase>();
		foreach (ButtonData buttonData in data.buttons)
		{
			GameButtonBase gameButtonBase = Object.Instantiate<GameButtonBase>(buttonData.prefab);
			if (this.ec.CellFromPosition(buttonData.position) != null)
			{
				gameButtonBase.transform.parent = this.ec.CellFromPosition(buttonData.position).TileTransform;
				gameButtonBase.transform.localPosition = Vector3.zero;
			}
			else
			{
				gameButtonBase.transform.parent = this.ec.transform;
				gameButtonBase.transform.position = new Vector3((float)buttonData.position.x * 10f + 5f, 0f, (float)buttonData.position.z * 10f + 5f);
			}
			gameButtonBase.transform.rotation = buttonData.direction.ToRotation();
			buttons.Add(gameButtonBase);
		}
		if (base.FrameShouldEnd())
		{
			yield return null;
		}
		Debug.Log("Level loader loading room objects.");
		if (!this.editorMode)
		{
			for (int num4 = 0; num4 < data.rooms.Count; num4++)
			{
				for (int num5 = 0; num5 < data.rooms[num4].basicObjects.Count; num5++)
				{
					BasicObjectData basicObjectData = data.rooms[num4].basicObjects[num5];
					if (basicObjectData.prefab != null)
					{
						Transform transform = Object.Instantiate<Transform>(basicObjectData.prefab, this.ec.rooms[num4].objectObject.transform);
						transform.position = basicObjectData.position;
						transform.rotation = basicObjectData.rotation;
						EnvironmentObject component = transform.GetComponent<EnvironmentObject>();
						if (component != null)
						{
							component.Ec = this.ec;
							this.environmentObjects.Add(component);
						}
						for (int num6 = 0; num6 < data.buttons.Count; num6++)
						{
							foreach (ButtonReceiverData buttonReceiverData in data.buttons[num6].receivers)
							{
								if (buttonReceiverData.type == ButtonReceiverType.Basic && buttonReceiverData.receiverRoom == num4 && buttonReceiverData.receiverIndex == num5)
								{
									buttons[num6].SetUp(transform.GetComponent<IButtonReceiver>());
									break;
								}
							}
						}
					}
				}
			}
		}
		if (base.FrameShouldEnd())
		{
			yield return null;
		}
		Debug.Log("Level loader placing items.");
		if (!this.editorMode)
		{
			for (int num7 = 0; num7 < data.rooms.Count; num7++)
			{
				if (data.extraData.potentialItems.Length != 0 && data.rooms[num7].maxItemValue > 0)
				{
					base.CreateRandomItemsInRoom(this.ec.rooms[num7], data.extraData.potentialItems, this.controlledRNG);
				}
				foreach (ItemData itemData in data.rooms[num7].items)
				{
					base.CreateItem(this.ec.rooms[num7], itemData.item, itemData.position);
				}
			}
		}
		if (base.FrameShouldEnd())
		{
			yield return null;
		}
		Debug.Log("Level loader loading tile based objects.");
		for (int num8 = 0; num8 < data.tbos.Count; num8++)
		{
			TileBasedObjectData tileBasedObjectData = data.tbos[num8];
			if (this.ec.CellFromPosition(tileBasedObjectData.position) != null)
			{
				TileBasedObject tileBasedObject = Object.Instantiate<TileBasedObject>(tileBasedObjectData.prefab, this.ec.CellFromPosition(tileBasedObjectData.position).TileTransform);
				tileBasedObject.transform.rotation = tileBasedObjectData.direction.ToRotation();
				tileBasedObject.direction = tileBasedObjectData.direction;
				tileBasedObject.position = tileBasedObjectData.position;
				tileBasedObject.bOffset = tileBasedObjectData.direction.ToIntVector2();
				tileBasedObject.ec = this.ec;
				for (int num9 = 0; num9 < data.buttons.Count; num9++)
				{
					foreach (ButtonReceiverData buttonReceiverData2 in data.buttons[num9].receivers)
					{
						if (buttonReceiverData2.type == ButtonReceiverType.TileBased && buttonReceiverData2.receiverIndex == num8)
						{
							buttons[num9].SetUp(tileBasedObject.GetComponent<IButtonReceiver>());
							break;
						}
					}
				}
			}
		}
		if (base.FrameShouldEnd())
		{
			yield return null;
		}
		Debug.Log("Level loader instantiating object builders.");
		foreach (ObjectBuilderData objectBuilderData in data.builders)
		{
			Object.Instantiate<ObjectBuilder>(objectBuilderData.builder, this.ec.transform).Load(this.ec, objectBuilderData.pos, objectBuilderData.dir);
		}
		if (base.FrameShouldEnd())
		{
			yield return null;
		}
		Debug.Log("Level loader loading and generating posters.");
		foreach (PosterData posterData in data.posters)
		{
			if (this.ec.CellFromPosition(posterData.position) != null && this.ec.BuildPoster(posterData.poster, this.ec.CellFromPosition(posterData.position), posterData.direction) && base.FrameShouldEnd())
			{
				yield return null;
			}
		}
		List<PosterData>.Enumerator enumerator13 = default(List<PosterData>.Enumerator);
		if (!this.editorMode)
		{
			foreach (PosterData posterData2 in this.postersToBuild)
			{
				this.ec.BuildPoster(posterData2.poster, this.ec.CellFromPosition(posterData2.position), posterData2.direction);
				if (base.FrameShouldEnd())
				{
					yield return null;
				}
			}
			enumerator13 = default(List<PosterData>.Enumerator);
		}
		if (base.FrameShouldEnd())
		{
			yield return null;
		}
		Debug.Log("Level loader loading and generating light sources.");
		if (!this.editorMode)
		{
			foreach (LightSourceData lightSourceData in data.lights)
			{
				this.ec.GenerateLight(this.ec.CellFromPosition(lightSourceData.position), lightSourceData.color, lightSourceData.strength);
				if (lightSourceData.prefab != null)
				{
					Object.Instantiate<Transform>(lightSourceData.prefab, this.ec.CellFromPosition(lightSourceData.position).TileTransform);
				}
				if (base.FrameShouldEnd())
				{
					yield return null;
				}
			}
			List<LightSourceData>.Enumerator enumerator14 = default(List<LightSourceData>.Enumerator);
		}
		if (!this.editorMode)
		{
			foreach (LightSourceData lightSourceData2 in this.lightsToGenerate)
			{
				this.ec.GenerateLight(this.ec.CellFromPosition(lightSourceData2.position), lightSourceData2.color, lightSourceData2.strength);
				if (lightSourceData2.prefab != null)
				{
					Object.Instantiate<Transform>(lightSourceData2.prefab, this.ec.CellFromPosition(lightSourceData2.position).TileTransform);
				}
				if (base.FrameShouldEnd())
				{
					yield return null;
				}
			}
			List<LightSourceData>.Enumerator enumerator14 = default(List<LightSourceData>.Enumerator);
		}
		if (base.FrameShouldEnd())
		{
			yield return null;
		}
		Debug.Log("Level loader loading activities.");
		if (!this.editorMode)
		{
			for (int num10 = 0; num10 < data.rooms.Count; num10++)
			{
				if (data.rooms[num10].hasActivity)
				{
					base.GenerateActivity(this.ec.rooms[num10], data.rooms[num10].activity);
				}
			}
		}
		if (base.FrameShouldEnd())
		{
			yield return null;
		}
		Debug.Log("Level loader setting up events.");
		if (!this.editorMode)
		{
			float num11 = data.extraData.initialEventGap;
			List<RandomEvent> list2 = new List<RandomEvent>(data.events);
			List<RandomEvent> list3 = new List<RandomEvent>();
			while (list2.Count > 0)
			{
				int index = Random.Range(0, list2.Count);
				list3.Add(list2[index]);
				list2.RemoveAt(index);
			}
			foreach (RandomEvent original in list3)
			{
				num11 += Random.Range(data.extraData.minEventGap, data.extraData.maxEventGap);
				RandomEvent randomEvent = Object.Instantiate<RandomEvent>(original, this.ec.transform);
				randomEvent.Initialize(this.ec, this.controlledRNG);
				randomEvent.PremadeSetup();
				randomEvent.SetEventTime(this.controlledRNG);
				this.ec.AddEvent(randomEvent, num11);
				num11 += randomEvent.EventTime;
			}
		}
		if (base.FrameShouldEnd())
		{
			yield return null;
		}
		Debug.Log("Level loader letting EnvironmentObjects know level generation has complete.");
		foreach (EnvironmentObject environmentObject in this.environmentObjects)
		{
			environmentObject.LoadingFinished();
			if (base.FrameShouldEnd())
			{
				yield return null;
			}
		}
		List<EnvironmentObject>.Enumerator enumerator16 = default(List<EnvironmentObject>.Enumerator);
		foreach (RoomController roomController3 in this.ec.rooms)
		{
			roomController3.functions.OnGenerationFinished();
			if (base.FrameShouldEnd())
			{
				yield return null;
			}
		}
		List<RoomController>.Enumerator enumerator17 = default(List<RoomController>.Enumerator);
		if (base.FrameShouldEnd())
		{
			yield return null;
		}
		Debug.Log("Level loader setting up NPCs to spawn.");
		if (!this.editorMode)
		{
			this.ec.npcSpawnTile = new Cell[data.extraData.npcSpawnPoints.Count + data.extraData.totalPotentialNpcsToSpawn];
			for (int num12 = 0; num12 < data.extraData.npcsToSpawn.Count; num12++)
			{
				this.ec.npcsToSpawn.Add(data.extraData.npcsToSpawn[num12]);
				this.ec.npcSpawnTile[num12] = this.ec.CellFromPosition(data.extraData.npcSpawnPoints[num12]);
			}
			List<WeightedNPC> list4 = new List<WeightedNPC>(data.extraData.potentialNpcs);
			foreach (NPC y in this.ec.npcsToSpawn)
			{
				for (int num13 = 0; num13 < list4.Count; num13++)
				{
					if (list4[num13].selection == y)
					{
						list4.RemoveAt(num13);
						num13--;
					}
				}
			}
			int num14 = 0;
			while (num14 < data.extraData.totalPotentialNpcsToSpawn && list4.Count > 0)
			{
				NPC npc = WeightedSelection<NPC>.ControlledRandomSelectionList(WeightedNPC.Convert(list4), this.controlledRNG);
				this.ec.npcsToSpawn.Add(npc);
				for (int num15 = 0; num15 < list4.Count; num15++)
				{
					if (list4[num15].selection == npc)
					{
						list4.RemoveAt(num15);
						num15--;
					}
				}
				num14++;
			}
		}
		if (base.FrameShouldEnd())
		{
			yield return null;
		}
		Debug.Log("Level loader setting NPC spawn points.");
		for (int i = 0; i < this.ec.npcsToSpawn.Count; i = j + 1)
		{
			if (this.ec.npcSpawnTile[i] == null)
			{
				List<Cell> list5 = new List<Cell>();
				foreach (RoomController roomController4 in this.ec.rooms)
				{
					if (this.ec.npcsToSpawn[i].spawnableRooms.Contains(roomController4.category))
					{
						list5.AddRange(roomController4.AllTilesNoGarbage(false, true));
					}
				}
				foreach (RoomController roomController5 in this.halls)
				{
					if (this.ec.npcsToSpawn[i].spawnableRooms.Contains(roomController5.category))
					{
						list5.AddRange(roomController5.AllTilesNoGarbage(false, true));
					}
				}
				if (list5.Count > 0)
				{
					Cell cell5 = list5[this.controlledRNG.Next(0, list5.Count)];
					this.ec.npcSpawnTile[i] = cell5;
				}
				if (base.FrameShouldEnd())
				{
					yield return null;
				}
			}
			j = i;
		}
		if (base.FrameShouldEnd())
		{
			yield return null;
		}
		if (!this.editorMode && Singleton<CoreGameManager>.Instance.GetCamera(0) != null)
		{
			Singleton<CoreGameManager>.Instance.GetCamera(0).StopRendering(false);
		}
		if (base.FrameShouldEnd())
		{
			yield return null;
		}
		if (!this.editorMode)
		{
			this.ec.CullingManager.PrepareOcclusionCalculations();
		}
		if (base.FrameShouldEnd())
		{
			yield return null;
		}
		if (!this.editorMode)
		{
			for (int i = 0; i < this.ec.CullingManager.TotalChunks; i = j + 1)
			{
				this.ec.CullingManager.CalculateOcclusionCullingForChunk(i);
				if (base.FrameShouldEnd())
				{
					yield return null;
				}
				j = i;
			}
		}
		if (!this.editorMode)
		{
			this.ec.CullingManager.SetActive(true);
		}
		yield return null;
		yield return null;
		this.levelInProgress = false;
		this.levelCreated = true;
		Debug.Log("Level loader finished loading!");
		bool editorMode = this.editorMode;
		yield break;
		yield break;
	}

	// Token: 0x04000A10 RID: 2576
	private LevelData levelData;

	// Token: 0x04000A11 RID: 2577
	private string dataName = "Unassigned";

	// Token: 0x04000A12 RID: 2578
	[SerializeField]
	private bool loadOnStart;
}
