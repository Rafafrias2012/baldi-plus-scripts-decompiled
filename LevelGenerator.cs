using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x0200018D RID: 397
public class LevelGenerator : LevelBuilder
{
	// Token: 0x06000919 RID: 2329 RVA: 0x00030580 File Offset: 0x0002E780
	public override void StartGenerate()
	{
		base.StartGenerate();
		base.StartCoroutine(this.Generate());
	}

	// Token: 0x0600091A RID: 2330 RVA: 0x00030595 File Offset: 0x0002E795
	public IEnumerator Generate()
	{
		this.levelCreated = false;
		this.levelInProgress = true;
		Stopwatch sw = new Stopwatch();
		sw.Start();
		this.controlledRNG = new Random(Singleton<CoreGameManager>.Instance.Seed());
		Debug.Log(string.Concat(new string[]
		{
			"Generating level ",
			this.ec.name,
			this.seedOffset.ToString(),
			" with seed:",
			Singleton<CoreGameManager>.Instance.Seed().ToString()
		}));
		this.ec.SetTileInstantiation(false);
		this.generateAtlases = false;
		yield return null;
		yield return null;
		this.ec.npcsToSpawn = new List<NPC>();
		List<LevelObject> list = new List<LevelObject>();
		list.AddRange(this.ld.previousLevels);
		list.Add(this.ld);
		foreach (LevelObject levelObject in list)
		{
			foreach (NPC item in levelObject.forcedNpcs)
			{
				this.ec.npcsToSpawn.Add(item);
			}
			List<WeightedNPC> list2 = new List<WeightedNPC>(levelObject.potentialNPCs);
			foreach (NPC y in this.ec.npcsToSpawn)
			{
				for (int k = 0; k < list2.Count; k++)
				{
					if (list2[k].selection == y)
					{
						list2.RemoveAt(k);
						k--;
					}
				}
			}
			int num = 0;
			while (num < levelObject.additionalNPCs && list2.Count > 0)
			{
				NPC npc = WeightedSelection<NPC>.ControlledRandomSelectionList(WeightedNPC.Convert(list2), this.controlledRNG);
				this.ec.npcsToSpawn.Add(npc);
				for (int l = 0; l < list2.Count; l++)
				{
					if (list2[l].selection == npc)
					{
						list2.RemoveAt(l);
						l--;
					}
				}
				num++;
			}
		}
		if (this.ld.potentialBaldis.Length != 0)
		{
			List<NPC> npcsToSpawn = this.ec.npcsToSpawn;
			WeightedSelection<NPC>[] potentialBaldis = this.ld.potentialBaldis;
			npcsToSpawn.Add(WeightedSelection<NPC>.ControlledRandomSelection(potentialBaldis, this.controlledRNG));
		}
		this.controlledRNG = new Random(Singleton<CoreGameManager>.Instance.Seed() + this.seedOffset);
		this.levelSize = new IntVector2(this.controlledRNG.Next(this.ld.minSize.x, this.ld.maxSize.x + 1) + this.ld.outerEdgeBuffer * 2, this.controlledRNG.Next(this.ld.minSize.z, this.ld.maxSize.z + 1) + this.ld.outerEdgeBuffer * 2);
		if (this.levelSize.x > 256 || this.levelSize.z > 256)
		{
			Debug.LogWarning(string.Format("Level size in at least one dimension was greater than 256 at {0},{1}! Cancelling level generation", this.levelSize.x, this.levelSize.z));
			yield break;
		}
		if (this.controlledRNG.Next(0, 2) == 1)
		{
			int x2 = this.levelSize.x;
			this.levelSize.x = this.levelSize.z;
			this.levelSize.z = x2;
		}
		this.ec.InitializeCells(this.levelSize);
		if (base.FrameShouldEnd())
		{
			yield return null;
		}
		this.ec.levelSize = this.levelSize;
		this.ec.InitializeHeap();
		if (base.FrameShouldEnd())
		{
			yield return null;
		}
		this.ec.standardDarkLevel = this.ld.standardDarkLevel;
		this.ec.InitializeLighting();
		if (base.FrameShouldEnd())
		{
			yield return null;
		}
		base.CreateMap();
		if (base.FrameShouldEnd())
		{
			yield return null;
		}
		this.plots.Clear();
		this.halls.Clear();
		this.doorPlacements.Clear();
		this.pickups.Clear();
		int plotCount = this.controlledRNG.Next(this.ld.minPlots, this.ld.maxPlots + 1);
		int hallsToRemove = this.controlledRNG.Next(this.ld.minHallsToRemove, this.ld.maxHallsToRemove + 1);
		this.controlledRNG.Next(this.ld.minSideHallsToRemove, this.ld.maxSideHallsToRemove + 1);
		int hallsToAdd = this.controlledRNG.Next(this.ld.minReplacementHalls, this.ld.maxReplacementHalls + 1);
		List<ItemObject> lootTable = new List<ItemObject>();
		lootTable.AddRange(this.ld.forcedItems);
		WeightedItemObject[] array = new WeightedItemObject[this.ld.potentialItems.Length];
		for (int m = 0; m < this.ld.potentialItems.Length; m++)
		{
			array[m] = new WeightedItemObject();
			array[m].selection = this.ld.potentialItems[m].selection;
			array[m].weight = this.ld.potentialItems[m].weight;
		}
		int n = 0;
		while (n < this.ld.maxItemValue)
		{
			WeightedSelection<ItemObject>[] items = array;
			int num2 = WeightedSelection<ItemObject>.ControlledRandomIndex(items, this.controlledRNG);
			lootTable.Add(array[num2].selection);
			n += lootTable[lootTable.Count - 1].value;
			array[num2].weight = Mathf.Max(array[num2].weight - Mathf.RoundToInt((float)array[num2].weight * this.ld.duplicateItemWeightReduction), 1);
		}
		int eventCount = this.controlledRNG.Next(this.ld.minEvents, this.ld.maxEvents);
		List<WeightedRandomEvent> potentialEvents = new List<WeightedRandomEvent>(this.ld.randomEvents);
		List<RandomEvent> eventsToLaunch = new List<RandomEvent>();
		List<RandomEvent> events = new List<RandomEvent>();
		if (base.FrameShouldEnd())
		{
			yield return null;
		}
		int i = 0;
		int j;
		while (i < eventCount && potentialEvents.Count > 0)
		{
			RandomEvent randomEvent = WeightedSelection<RandomEvent>.ControlledRandomSelectionList(WeightedRandomEvent.Convert(potentialEvents), this.controlledRNG);
			eventsToLaunch.Add(randomEvent);
			for (int num3 = 0; num3 < potentialEvents.Count; num3++)
			{
				if (potentialEvents[num3].selection == randomEvent)
				{
					potentialEvents.RemoveAt(num3);
					num3--;
				}
			}
			if (base.FrameShouldEnd())
			{
				yield return null;
			}
			j = i;
			i = j + 1;
		}
		foreach (RandomEvent original in eventsToLaunch)
		{
			RandomEvent randomEvent2 = Object.Instantiate<RandomEvent>(original, this.ec.transform);
			randomEvent2.Initialize(this.ec, this.controlledRNG);
			randomEvent2.SetEventTime(new Random(Random.Range(0, int.MaxValue)));
			events.Add(randomEvent2);
			if (base.FrameShouldEnd())
			{
				yield return null;
			}
		}
		List<RandomEvent>.Enumerator enumerator3 = default(List<RandomEvent>.Enumerator);
		if (base.FrameShouldEnd())
		{
			yield return null;
		}
		WeightedSelection<Texture2D>[] items2 = this.ld.hallWallTexs;
		Texture2D hallWallTex = WeightedSelection<Texture2D>.ControlledRandomSelection(items2, this.controlledRNG);
		items2 = this.ld.hallFloorTexs;
		Texture2D hallFloorTex = WeightedSelection<Texture2D>.ControlledRandomSelection(items2, this.controlledRNG);
		items2 = this.ld.hallCeilingTexs;
		Texture2D hallCeilingTex = WeightedSelection<Texture2D>.ControlledRandomSelection(items2, this.controlledRNG);
		WeightedSelection<Transform>[] items3 = this.ld.hallLights;
		Transform hallLight = WeightedSelection<Transform>.ControlledRandomSelection(items3, this.controlledRNG);
		Texture2D[] groupWallTexture = new Texture2D[this.ld.roomGroup.Length];
		Texture2D[] groupFloorTexture = new Texture2D[this.ld.roomGroup.Length];
		Texture2D[] groupCeilingTexture = new Texture2D[this.ld.roomGroup.Length];
		Transform[] array2 = new Transform[this.ld.roomGroup.Length];
		for (int num4 = 0; num4 < this.ld.roomGroup.Length; num4++)
		{
			Texture2D[] array3 = groupWallTexture;
			int num5 = num4;
			items2 = this.ld.roomGroup[num4].wallTexture;
			array3[num5] = WeightedSelection<Texture2D>.ControlledRandomSelection(items2, this.controlledRNG);
			Texture2D[] array4 = groupFloorTexture;
			int num6 = num4;
			items2 = this.ld.roomGroup[num4].floorTexture;
			array4[num6] = WeightedSelection<Texture2D>.ControlledRandomSelection(items2, this.controlledRNG);
			Texture2D[] array5 = groupCeilingTexture;
			int num7 = num4;
			items2 = this.ld.roomGroup[num4].ceilingTexture;
			array5[num7] = WeightedSelection<Texture2D>.ControlledRandomSelection(items2, this.controlledRNG);
			Transform[] array6 = array2;
			int num8 = num4;
			items3 = this.ld.roomGroup[num4].light;
			array6[num8] = WeightedSelection<Transform>.ControlledRandomSelection(items3, this.controlledRNG);
		}
		int specialRoomsCount = this.controlledRNG.Next(this.ld.minSpecialRooms, this.ld.maxSpecialRooms + 1);
		if (base.FrameShouldEnd())
		{
			yield return null;
		}
		RoomController bufferTiles = Object.Instantiate<RoomController>(this.roomControllerPre, this.ec.transform);
		bufferTiles.transform.name = "Buffer Tiles";
		bufferTiles.ec = this.ec;
		if (this.ld.includeBuffers)
		{
			bufferTiles.type = RoomType.Room;
		}
		else
		{
			bufferTiles.type = RoomType.Null;
		}
		if (base.FrameShouldEnd())
		{
			yield return null;
		}
		int northEdgeBuffer = this.ld.outerEdgeBuffer;
		int eastEdgeBuffer = this.ld.outerEdgeBuffer;
		int southEdgeBuffer = this.ld.outerEdgeBuffer;
		int westEdgeBuffer = this.ld.outerEdgeBuffer;
		if (base.FrameShouldEnd())
		{
			yield return null;
		}
		for (i = 0; i < this.levelSize.x; i = j + 1)
		{
			for (int z = 0; z < this.levelSize.z; z = j + 1)
			{
				if (i < westEdgeBuffer || this.levelSize.x - i <= eastEdgeBuffer || z < southEdgeBuffer || this.levelSize.z - z <= northEdgeBuffer)
				{
					this.ec.CreateCell(this.placeholderTileVal, bufferTiles.transform, new IntVector2(i, z), bufferTiles);
				}
				if (base.FrameShouldEnd())
				{
					yield return null;
				}
				j = z;
			}
			j = i;
		}
		this.roomSpawnWeightDijkstraMap = new DijkstraMap(this.ec, PathType.Const, Array.Empty<Transform>());
		this.specialRooms = new List<RoomController>();
		for (i = 0; i < specialRoomsCount; i = j + 1)
		{
			base.UpdatePotentialSpawnsForPlots(this.ld.specialRoomsStickToEdge);
			WeightedSelection<RoomAsset>[] items4 = this.ld.potentialSpecialRooms;
			RoomController item2;
			this.RandomlyPlaceRoom(WeightedSelection<RoomAsset>.ControlledRandomSelection(items4, this.controlledRNG), false, out item2);
			this.specialRooms.Add(item2);
			if (base.FrameShouldEnd())
			{
				yield return null;
			}
			j = i;
		}
		this.halls.Add(null);
		base.CreateArea(this.halls, RoomType.Hall, RoomCategory.Hall, 0, new IntVector2(0, 0), new IntVector2(this.levelSize.x, this.levelSize.z), false);
		base.TextureArea(this.halls[0], hallWallTex, hallFloorTex, hallCeilingTex);
		this.halls[0].lightPre = hallLight;
		this.halls[0].acceptsExits = true;
		this.halls[0].doorMats = this.ld.standardDoorMat;
		this.ec.mainHall = this.halls[0];
		if (base.FrameShouldEnd())
		{
			yield return null;
		}
		if (this.ld.potentialPrePlotSpecialHalls.Length != 0)
		{
			i = 0;
			while (i < this.ld.maxPrePlotSpecialHalls && (i < this.ld.minPrePlotSpecialHalls || this.controlledRNG.NextDouble() < (double)this.ld.prePlotSpecialHallChance))
			{
				base.UpdatePotentialSpawnsForPlots(false);
				WeightedSelection<RoomAsset>[] items4 = this.ld.potentialPrePlotSpecialHalls;
				this.RandomlyPlaceRoom(WeightedSelection<RoomAsset>.ControlledRandomSelection(items4, this.controlledRNG), this.halls[0]);
				if (base.FrameShouldEnd())
				{
					yield return null;
				}
				j = i;
				i = j + 1;
			}
		}
		base.UpdatePotentialSpawnsForPlots(false);
		if (base.FrameShouldEnd())
		{
			yield return null;
		}
		i = 0;
		while (i < plotCount && this.potentialRoomSpawns.Count > 0)
		{
			this.plots.Add(null);
			base.CreateArea(this.plots, RoomType.Room, RoomCategory.Null, i, WeightedSelection<RoomSpawn>.ControlledRandomSelectionList(WeightedRoomSpawn.Convert(this.potentialRoomSpawns), this.controlledRNG).position, new IntVector2(this.levelSize.x, this.levelSize.z), true);
			base.UpdatePotentialSpawnsForPlots(false);
			if (base.FrameShouldEnd())
			{
				yield return null;
			}
			j = i;
			i = j + 1;
		}
		List<RoomController> plotsToExpand = new List<RoomController>(this.plots);
		while (plotsToExpand.Count > 0)
		{
			for (i = 0; i < plotsToExpand.Count; i = j + 1)
			{
				List<Direction> possibleDirections = base.GetPossibleDirections(plotsToExpand[i], 1);
				if (possibleDirections.Count > 0)
				{
					base.ExpandArea(plotsToExpand[i], possibleDirections[this.controlledRNG.Next(possibleDirections.Count)]);
				}
				else
				{
					plotsToExpand[i].expandable = false;
					plotsToExpand.RemoveAt(i);
					i--;
				}
				if (base.FrameShouldEnd())
				{
					yield return null;
				}
				j = i;
			}
		}
		for (i = 0; i < this.plots.Count; i = j + 1)
		{
			if (this.plots[i].size.x < this.ld.minPlotSize || this.plots[i].size.z < this.ld.minPlotSize)
			{
				base.DestroyRoom(this.plots[i]);
				this.plots.RemoveAt(i);
				i--;
			}
			if (base.FrameShouldEnd())
			{
				yield return null;
			}
			j = i;
		}
		List<List<Direction>> roomsNDirs = new List<List<Direction>>();
		List<RoomController> expandablePlots = new List<RoomController>(this.plots);
		for (int num9 = 0; num9 < this.plots.Count; num9++)
		{
			roomsNDirs.Add(new List<Direction>
			{
				Direction.North,
				Direction.East,
				Direction.South,
				Direction.West
			});
		}
		i = 0;
		while (i < hallsToRemove && expandablePlots.Count > 0)
		{
			int index = this.controlledRNG.Next(0, expandablePlots.Count);
			roomsNDirs[index] = base.GetPossibleDirections(expandablePlots[index], 0);
			if (roomsNDirs[index].Count > 0)
			{
				int index2 = this.controlledRNG.Next(0, roomsNDirs[index].Count);
				base.ExpandArea(expandablePlots[index], roomsNDirs[index][index2]);
				roomsNDirs[index].RemoveAt(index2);
			}
			else
			{
				roomsNDirs.RemoveAt(index);
				expandablePlots.RemoveAt(index);
				i--;
			}
			if (base.FrameShouldEnd())
			{
				yield return null;
			}
			j = i;
			i = j + 1;
		}
		for (i = 0; i < this.levelSize.x; i = j + 1)
		{
			for (int z = 0; z < this.levelSize.z; z = j + 1)
			{
				IntVector2 intVector = new IntVector2(i, z);
				if (this.ec.cells[intVector.x, intVector.z].Null && base.ProximityCheck(intVector, RoomType.Room, 2))
				{
					this.ec.CreateCell(this.placeholderTileVal, this.halls[0].transform, intVector, this.halls[0]);
				}
				if (base.FrameShouldEnd())
				{
					yield return null;
				}
				j = z;
			}
			j = i;
		}
		base.DestroyRoom(bufferTiles);
		if (base.FrameShouldEnd())
		{
			yield return null;
		}
		while (this.plots.Count > 0)
		{
			base.DestroyRoom(this.plots[0]);
			this.plots.RemoveAt(0);
			if (base.FrameShouldEnd())
			{
				yield return null;
			}
		}
		if (this.ld.potentialPostPlotSpecialHalls.Length != 0)
		{
			i = 0;
			while (i < this.ld.maxPostPlotSpecialHalls && (i < this.ld.minPostPlotSpecialHalls || this.controlledRNG.NextDouble() < (double)this.ld.postPlotSpecialHallChance))
			{
				base.UpdatePotentialSpawnsForRooms(true);
				WeightedSelection<RoomAsset>[] items4 = this.ld.potentialPostPlotSpecialHalls;
				this.RandomlyPlaceRoom(WeightedSelection<RoomAsset>.ControlledRandomSelection(items4, this.controlledRNG), this.halls[0]);
				if (base.FrameShouldEnd())
				{
					yield return null;
				}
				j = i;
				i = j + 1;
			}
		}
		bool tilesConnected = false;
		while (!tilesConnected)
		{
			i = -1;
			Queue<Cell> tileQueue = new Queue<Cell>();
			List<List<Cell>> tileGroups = new List<List<Cell>>();
			for (int z = 0; z < this.levelSize.x; z = j + 1)
			{
				for (int z2 = 0; z2 < this.levelSize.z; z2 = j + 1)
				{
					Cell cell = this.ec.cells[z, z2];
					if (!cell.Null && !cell.labeled && cell.room.type == RoomType.Hall)
					{
						i++;
						tileGroups.Add(new List<Cell>());
						cell.label = i;
						cell.labeled = true;
						tileQueue.Enqueue(cell);
						tileGroups[i].Add(cell);
						while (tileQueue.Count > 0)
						{
							List<Cell> list3 = new List<Cell>(base.MatchingAdjacentTiles(tileQueue.Dequeue()));
							for (int num10 = 0; num10 < list3.Count; num10++)
							{
								Cell cell2 = list3[num10];
								if (!cell2.labeled)
								{
									cell2.label = i;
									cell2.labeled = true;
									tileQueue.Enqueue(cell2);
									tileGroups[i].Add(cell2);
								}
							}
						}
					}
					if (base.FrameShouldEnd())
					{
						yield return null;
					}
					j = z2;
				}
				j = z;
			}
			if (base.FrameShouldEnd())
			{
				yield return null;
			}
			if (tileGroups.Count > 1)
			{
				for (int z = 0; z < tileGroups.Count; z = j + 1)
				{
					List<int> list4 = new List<int>();
					for (int num11 = 0; num11 < tileGroups.Count; num11++)
					{
						if (num11 != z)
						{
							list4.Add(num11);
						}
					}
					List<Cell> list5 = new List<Cell>(tileGroups[list4[this.controlledRNG.Next(0, list4.Count)]]);
					Cell startTile = tileGroups[z][this.controlledRNG.Next(0, tileGroups[z].Count)];
					Cell endTile = list5[this.controlledRNG.Next(0, list5.Count)];
					int z2 = 0;
					Queue<IntVector2> path = new Queue<IntVector2>();
					List<Direction> list6 = new List<Direction>(base.DirectionsToDestination(startTile.position, endTile.position));
					Direction curDir = list6[this.controlledRNG.Next(0, list6.Count)];
					IntVector2 currentPos = startTile.position;
					while (this.ec.cells[currentPos.x, currentPos.z].Null || this.ec.cells[currentPos.x, currentPos.z].label == startTile.label)
					{
						Cell cell3 = this.ec.cells[currentPos.x, currentPos.z];
						if (!cell3.Null && cell3.label == startTile.label)
						{
							path.Clear();
						}
						else
						{
							path.Enqueue(currentPos);
						}
						list6 = new List<Direction>(base.DirectionsToDestination(currentPos, endTile.position));
						if (list6.Contains(curDir))
						{
							if (list6.Count > 1 && z2 * this.ld.bridgeTurnChance > this.controlledRNG.Next(0, 100) + 1)
							{
								list6.Remove(curDir);
								curDir = list6[this.controlledRNG.Next(0, list6.Count)];
								z2 = 0;
							}
						}
						else
						{
							curDir = list6[this.controlledRNG.Next(0, list6.Count)];
						}
						currentPos += curDir.ToIntVector2();
						z2++;
						if (base.FrameShouldEnd())
						{
							yield return null;
						}
					}
					while (path.Count > 0)
					{
						this.ec.CreateCell(this.placeholderTileVal, startTile.room.transform, path.Dequeue(), startTile.room);
						if (base.FrameShouldEnd())
						{
							yield return null;
						}
					}
					if (base.FrameShouldEnd())
					{
						yield return null;
					}
					startTile = null;
					endTile = null;
					path = null;
					j = z;
				}
				for (int z = 0; z < this.levelSize.x; z = j + 1)
				{
					for (int z2 = 0; z2 < this.levelSize.z; z2 = j + 1)
					{
						Cell cell4 = this.ec.cells[z, z2];
						if (!cell4.Null)
						{
							cell4.labeled = false;
							cell4.label = 0;
						}
						if (base.FrameShouldEnd())
						{
							yield return null;
						}
						j = z2;
					}
					j = z;
				}
			}
			else
			{
				tilesConnected = true;
			}
			tileQueue = null;
			tileGroups = null;
		}
		if (base.FrameShouldEnd())
		{
			yield return null;
		}
		RoomController cornerBuffers = null;
		List<Cell> potentialStartingPoints = new List<Cell>();
		for (i = 0; i < hallsToAdd; i = j + 1)
		{
			if (cornerBuffers != null)
			{
				base.DestroyRoom(cornerBuffers);
			}
			cornerBuffers = Object.Instantiate<RoomController>(this.roomControllerPre, this.ec.transform);
			cornerBuffers.transform.name = "CornerBuffers";
			cornerBuffers.category = RoomCategory.Buffer;
			cornerBuffers.ec = this.ec;
			for (int z2 = 0; z2 < this.levelSize.x; z2 = j + 1)
			{
				for (int z3 = 0; z3 < this.levelSize.z; z3 = j + 1)
				{
					if (this.ec.cells[z2, z3].Null)
					{
						IntVector2 intVector2 = new IntVector2(z2, z3);
						if ((this.ec.TileInDirectionCheck(intVector2, Direction.North, this.ld.minRoomSize.x) || this.ec.TileInDirectionCheck(intVector2, Direction.South, this.ld.minRoomSize.x)) && (this.ec.TileInDirectionCheck(intVector2, Direction.East, this.ld.minRoomSize.x) || this.ec.TileInDirectionCheck(intVector2, Direction.West, this.ld.minRoomSize.x)))
						{
							this.ec.CreateCell(0, cornerBuffers.transform, intVector2, cornerBuffers);
						}
					}
					if (base.FrameShouldEnd())
					{
						yield return null;
					}
					j = z3;
				}
				j = z2;
			}
			potentialStartingPoints = new List<Cell>(this.halls[0].GetNewTileList());
			for (int z2 = 0; z2 < potentialStartingPoints.Count; z2 = j + 1)
			{
				if (this.ec.GetCellNeighbors(potentialStartingPoints[z2].position).Count >= 4)
				{
					potentialStartingPoints.RemoveAt(z2);
					z2--;
				}
				if (base.FrameShouldEnd())
				{
					yield return null;
				}
				j = z2;
			}
			int z = this.controlledRNG.Next(0, potentialStartingPoints.Count);
			while (potentialStartingPoints.Count > 0 && base.PotentialPathDirections(potentialStartingPoints[z].position).Count == 0)
			{
				potentialStartingPoints.RemoveAt(z);
				z = this.controlledRNG.Next(0, potentialStartingPoints.Count);
				if (base.FrameShouldEnd())
				{
					yield return null;
				}
			}
			if (potentialStartingPoints.Count > 0)
			{
				bool success = false;
				int z2 = 0;
				Queue<IntVector2> path = new Queue<IntVector2>();
				Cell endTile = null;
				Direction curDir = Direction.Null;
				while (!success && z2 < this.ld.maxHallAttempts)
				{
					path = base.GetRandomPath(potentialStartingPoints[z].position, this.ld.additionTurnChance, out success, out endTile, out curDir);
					z2++;
					if (base.FrameShouldEnd())
					{
						yield return null;
					}
				}
				foreach (IntVector2 position in path)
				{
					this.ec.CreateCell(this.placeholderTileVal, this.halls[0].transform, position, this.halls[0]);
					if (base.FrameShouldEnd())
					{
						yield return null;
					}
				}
				Queue<IntVector2>.Enumerator enumerator4 = default(Queue<IntVector2>.Enumerator);
				if (endTile != null && endTile.room != this.halls[0] && endTile.room.ContainsDoorPosition(endTile.position))
				{
					RoomController roomController;
					base.BuildDoorInDirection(endTile.room, endTile.position, curDir.GetOpposite(), false, out roomController);
				}
				if (base.FrameShouldEnd())
				{
					yield return null;
				}
				path = null;
				endTile = null;
			}
			j = i;
		}
		if (cornerBuffers != null)
		{
			base.DestroyRoom(cornerBuffers);
		}
		if (base.FrameShouldEnd())
		{
			yield return null;
		}
		List<Cell> deadEnds = new List<Cell>();
		for (i = 0; i < this.levelSize.x; i = j + 1)
		{
			for (int z = 0; z < this.levelSize.z; z = j + 1)
			{
				if (!this.ec.cells[i, z].Null && base.MatchingAdjacentTiles(this.ec.cells[i, z]).Count == 1)
				{
					deadEnds.Add(this.ec.cells[i, z]);
				}
				if (base.FrameShouldEnd())
				{
					yield return null;
				}
				j = z;
			}
			j = i;
		}
		foreach (Cell cell5 in deadEnds)
		{
			bool success = true;
			new Queue<Cell>();
			Cell cell6 = cell5;
			List<Cell> list7 = base.MatchingAdjacentTiles(cell6);
			List<Cell> list8 = new List<Cell>();
			list8.Add(cell6);
			List<Cell> potSpawns = new List<Cell>();
			potSpawns.Add(cell6);
			for (int num12 = 0; num12 < this.ld.deadEndBuffer; num12++)
			{
				if (list7.Count > 2)
				{
					success = false;
					break;
				}
				foreach (Cell item3 in list8)
				{
					list7.Remove(item3);
				}
				foreach (Cell cell7 in list7)
				{
					list7 = base.MatchingAdjacentTiles(cell7);
					potSpawns.Add(cell7);
					list8.Add(cell7);
				}
			}
			if (base.FrameShouldEnd())
			{
				yield return null;
			}
			if (success)
			{
				Cell endTile;
				Direction curDir;
				bool flag;
				Queue<IntVector2> queue = new Queue<IntVector2>(base.GetRandomPath(potSpawns[this.controlledRNG.Next(0, potSpawns.Count)].position, this.ld.additionTurnChance, out flag, out endTile, out curDir));
				foreach (IntVector2 position2 in queue)
				{
					this.ec.CreateCell(this.placeholderTileVal, this.halls[0].transform, position2, this.halls[0]);
					if (base.FrameShouldEnd())
					{
						yield return null;
					}
				}
				Queue<IntVector2>.Enumerator enumerator4 = default(Queue<IntVector2>.Enumerator);
				if (endTile != null && !endTile.Null && endTile.room != this.halls[0] && endTile.room.ContainsDoorPosition(endTile.position))
				{
					RoomController roomController2;
					base.BuildDoorInDirection(endTile.room, endTile.position, curDir.GetOpposite(), false, out roomController2);
				}
				endTile = null;
			}
			if (base.FrameShouldEnd())
			{
				yield return null;
			}
			potSpawns = null;
		}
		List<Cell>.Enumerator enumerator5 = default(List<Cell>.Enumerator);
		List<Direction> potentailExitDirections = Directions.All();
		int exitCount = this.ld.exitCount;
		for (i = 0; i < exitCount; i = j + 1)
		{
			int index3 = this.controlledRNG.Next(0, potentailExitDirections.Count);
			Direction curDir = potentailExitDirections[index3];
			potentailExitDirections.RemoveAt(index3);
			bool success = false;
			List<IntVector2> potentialSpawns = new List<IntVector2>();
			IntVector2 intVector3 = curDir.ToIntVector2();
			int num13 = Mathf.Max(0, (this.levelSize.x - 1) * intVector3.x);
			int z = Mathf.Max(0, (this.levelSize.x - 1) * (intVector3.x + Mathf.Abs(intVector3.z)));
			int z2 = Mathf.Max(0, (this.levelSize.z - 1) * intVector3.z);
			int z3 = Mathf.Max(0, (this.levelSize.z - 1) * (intVector3.z + Mathf.Abs(intVector3.x)));
			for (int x = num13; x <= z; x = j + 1)
			{
				for (int z4 = z2; z4 <= z3; z4 = j + 1)
				{
					IntVector2 currentPos = curDir.GetOpposite().ToIntVector2();
					IntVector2 offset = default(IntVector2);
					while (this.ec.ContainsCoordinates(new IntVector2(x + offset.x, z4 + offset.z)) && this.ec.cells[x + offset.x, z4 + offset.z].Null)
					{
						offset += currentPos;
						if (base.FrameShouldEnd())
						{
							yield return null;
						}
					}
					if (this.ec.ContainsCoordinates(new IntVector2(x + offset.x, z4 + offset.z)))
					{
						IntVector2 intVector4 = new IntVector2(x + offset.x, z4 + offset.z);
						if (!this.ec.cells[intVector4.x, intVector4.z].Null)
						{
							Cell cell8 = this.ec.cells[intVector4.x, intVector4.z];
							if (cell8.room.acceptsExits)
							{
								List<Direction> list9 = curDir.PerpendicularList();
								bool flag2 = true;
								foreach (Direction direction in list9)
								{
									if (!this.ec.ContainsCoordinates(intVector4 + direction.ToIntVector2()))
									{
										flag2 = false;
									}
									else if (this.ec.cells[intVector4.x + direction.ToIntVector2().x, intVector4.z + direction.ToIntVector2().z].Null)
									{
										flag2 = false;
									}
									else if (this.ec.cells[intVector4.x + direction.ToIntVector2().x, intVector4.z + direction.ToIntVector2().z].room != cell8.room)
									{
										flag2 = false;
									}
								}
								if (flag2)
								{
									if (base.RoomFits(this.ld.elevatorRoom, intVector4 + curDir.ToIntVector2(), this.ld.elevatorRoom.potentialDoorPositions[0], curDir.GetOpposite()))
									{
										potentialSpawns.Add(intVector4 + curDir.ToIntVector2());
									}
									if (cell8.room.type != RoomType.Hall)
									{
										success = true;
									}
								}
							}
						}
					}
					if (base.FrameShouldEnd())
					{
						yield return null;
					}
					j = z4;
				}
				j = x;
			}
			if (potentialSpawns.Count > 0)
			{
				if (success)
				{
					for (int num14 = 0; num14 < potentialSpawns.Count; num14++)
					{
						if (this.ec.CellFromPosition(potentialSpawns[num14] + curDir.GetOpposite().ToIntVector2()).room.type == RoomType.Hall)
						{
							potentialSpawns.RemoveAt(num14);
							num14--;
						}
					}
				}
				int index4 = this.controlledRNG.Next(0, potentialSpawns.Count);
				base.CreateElevator(potentialSpawns[index4], curDir.GetOpposite(), this.ld.elevatorPre, this.ld.elevatorRoom, this.ec.elevators.Count == 0);
				if (this.ec.CellFromPosition(potentialSpawns[index4]).room.type != RoomType.Hall)
				{
					this.ec.CellFromPosition(potentialSpawns[index4]).room.acceptsExits = false;
				}
			}
			else
			{
				Debug.LogWarning("No position for the exit on the " + curDir.ToString() + " side was found!\nReducing total number of exits.");
				i--;
				exitCount = Mathf.Min(exitCount, potentailExitDirections.Count);
			}
			if (base.FrameShouldEnd())
			{
				yield return null;
			}
			potentialSpawns = null;
			j = i;
		}
		if (base.FrameShouldEnd())
		{
			yield return null;
		}
		for (i = 0; i < this.levelSize.x; i = j + 1)
		{
			for (int z3 = 0; z3 < this.levelSize.z; z3 = j + 1)
			{
				IntVector2 intVector5 = new IntVector2(i, z3);
				if (!this.ec.cells[intVector5.x, intVector5.z].Null)
				{
					this.ec.UpdateCell(intVector5);
				}
				if (base.FrameShouldEnd())
				{
					yield return null;
				}
				j = z3;
			}
			j = i;
		}
		foreach (DoorPlacement doorPlacement in this.doorPlacements)
		{
			this.ec.ConnectCells(doorPlacement.position, doorPlacement.dir);
			if (base.FrameShouldEnd())
			{
				yield return null;
			}
		}
		List<DoorPlacement>.Enumerator enumerator8 = default(List<DoorPlacement>.Enumerator);
		this.roomSpawnWeightDijkstraMap = new DijkstraMap(this.ec, PathType.Const, Array.Empty<Transform>());
		int[] roomCount = new int[this.ld.roomGroup.Length];
		for (int num15 = 0; num15 < roomCount.Length; num15++)
		{
			roomCount[num15] = this.controlledRNG.Next(this.ld.roomGroup[num15].minRooms, this.ld.roomGroup[num15].maxRooms + 1);
		}
		if (base.FrameShouldEnd())
		{
			yield return null;
		}
		base.UpdatePotentialSpawnsForRooms(true);
		if (base.FrameShouldEnd())
		{
			yield return null;
		}
		this.ec.npcSpawnTile = new Cell[this.ec.npcsToSpawn.Count];
		for (i = 0; i < this.ec.npcsToSpawn.Count; i = j + 1)
		{
			if (this.ec.npcsToSpawn[i].potentialRoomAssets.Length != 0)
			{
				RoomController roomController3 = null;
				WeightedSelection<RoomAsset>[] items4 = this.ec.npcsToSpawn[i].potentialRoomAssets;
				if (this.RandomlyPlaceRoom(WeightedSelection<RoomAsset>.ControlledRandomSelection(items4, this.controlledRNG), true, out roomController3))
				{
					this.standardRooms.Add(roomController3);
					this.ec.npcSpawnTile[i] = roomController3.RandomEntitySafeCellNoGarbage();
				}
			}
			if (base.FrameShouldEnd())
			{
				yield return null;
			}
			base.UpdatePotentialSpawnsForRooms(true);
			if (base.FrameShouldEnd())
			{
				yield return null;
			}
			j = i;
		}
		foreach (RandomEvent randomEvent3 in events)
		{
			if (randomEvent3.PotentialRoomAssets.Length != 0)
			{
				RoomController roomController4 = null;
				WeightedSelection<RoomAsset>[] items4 = randomEvent3.PotentialRoomAssets;
				if (this.RandomlyPlaceRoom(WeightedSelection<RoomAsset>.ControlledRandomSelection(items4, this.controlledRNG), true, out roomController4))
				{
					this.standardRooms.Add(roomController4);
					randomEvent3.AssignRoom(roomController4);
				}
			}
			if (base.FrameShouldEnd())
			{
				yield return null;
			}
			base.UpdatePotentialSpawnsForRooms(true);
			if (base.FrameShouldEnd())
			{
				yield return null;
			}
		}
		enumerator3 = default(List<RandomEvent>.Enumerator);
		for (i = 0; i < this.ld.roomGroup.Length; i = j + 1)
		{
			RoomGroup group = this.ld.roomGroup[i];
			List<WeightedRoomAsset> potentialRoomsList = new List<WeightedRoomAsset>(group.potentialRooms);
			this.previousRoomSpawnPositions.Clear();
			bool stickToHalls = this.controlledRNG.NextDouble() < (double)group.stickToHallChance;
			base.UpdatePotentialSpawnsForRooms(stickToHalls);
			if (base.FrameShouldEnd())
			{
				yield return null;
			}
			int z3 = 0;
			while (z3 < roomCount[i] && potentialRoomsList.Count > 0)
			{
				WeightedSelection<RoomAsset>[] items4 = potentialRoomsList.ToArray();
				int index5 = WeightedSelection<RoomAsset>.ControlledRandomIndex(items4, this.controlledRNG);
				RoomController roomController5;
				if (!this.RandomlyPlaceRoom(potentialRoomsList[index5].selection, true, out roomController5))
				{
					potentialRoomsList.RemoveAt(index5);
					z3--;
				}
				else
				{
					this.standardRooms.Add(roomController5);
					stickToHalls = (this.controlledRNG.NextDouble() < (double)group.stickToHallChance);
					base.UpdatePotentialSpawnsForRooms(stickToHalls);
					if (!potentialRoomsList[index5].selection.keepTextures)
					{
						roomController5.florTex = groupFloorTexture[i];
						roomController5.wallTex = groupWallTexture[i];
						roomController5.ceilTex = groupCeilingTexture[i];
					}
				}
				if (base.FrameShouldEnd())
				{
					yield return null;
				}
				j = z3;
				z3 = j + 1;
			}
			group = null;
			potentialRoomsList = null;
			j = i;
		}
		using (List<RoomController>.Enumerator enumerator9 = this.standardRooms.GetEnumerator())
		{
			IL_3842:
			while (enumerator9.MoveNext())
			{
				RoomController room = enumerator9.Current;
				for (i = 0; i < room.forcedDoorPositions.Count; i = j + 1)
				{
					RoomController roomController6;
					IntVector2 position3;
					if (base.BuildDoorIfPossible(room.forcedDoorPositions[i], room, false, true, out roomController6, out position3))
					{
						room.forcedDoorPositions.RemoveAt(i);
						room.ConnectRooms(roomController6);
						roomController6.RemoveFromDoorPositions(position3);
						i--;
					}
					if (base.FrameShouldEnd())
					{
						yield return null;
					}
					j = i;
				}
				while (room.potentialDoorPositions.Count > 0)
				{
					bool success;
					if (this.controlledRNG.NextDouble() >= (double)this.ld.extraDoorChance)
					{
						IL_3825:
						while (room.potentialDoorPositions.Count > 0 && this.controlledRNG.NextDouble() < (double)room.windowChance)
						{
							success = false;
							while (room.potentialDoorPositions.Count > 0 && !success)
							{
								int index6 = this.controlledRNG.Next(0, room.potentialDoorPositions.Count);
								RoomController roomController7;
								IntVector2 position4;
								if (base.BuildWindowIfPossible(room.potentialDoorPositions[index6], room, out roomController7, out position4))
								{
									success = true;
									roomController7.RemoveFromDoorPositions(position4);
								}
								room.potentialDoorPositions.RemoveAt(index6);
								if (base.FrameShouldEnd())
								{
									yield return null;
								}
							}
						}
						room = null;
						goto IL_3842;
					}
					success = false;
					while (room.potentialDoorPositions.Count > 0 && !success)
					{
						int index7 = this.controlledRNG.Next(0, room.potentialDoorPositions.Count);
						RoomController roomController8;
						IntVector2 position5;
						if (base.BuildDoorIfPossible(room.potentialDoorPositions[index7], room, false, false, out roomController8, out position5))
						{
							success = true;
							room.ConnectRooms(roomController8);
							roomController8.RemoveFromDoorPositions(position5);
						}
						room.potentialDoorPositions.RemoveAt(index7);
						if (base.FrameShouldEnd())
						{
							yield return null;
						}
					}
				}
				goto IL_3825;
			}
		}
		List<RoomController>.Enumerator enumerator9 = default(List<RoomController>.Enumerator);
		this.ec.SetTileInstantiation(true);
		this.generateAtlases = true;
		if (base.FrameShouldEnd())
		{
			yield return null;
		}
		this.ec.mainHall.GenerateTextureAtlas();
		if (base.FrameShouldEnd())
		{
			yield return null;
		}
		foreach (RoomController roomController9 in this.ec.rooms)
		{
			roomController9.GenerateTextureAtlas();
			if (base.FrameShouldEnd())
			{
				yield return null;
			}
		}
		enumerator9 = default(List<RoomController>.Enumerator);
		for (i = 0; i < this.levelSize.x; i = j + 1)
		{
			for (int z3 = 0; z3 < this.levelSize.z; z3 = j + 1)
			{
				IntVector2 intVector6 = new IntVector2(i, z3);
				if (!this.ec.cells[intVector6.x, intVector6.z].Null)
				{
					this.ec.RefreshCell(intVector6);
				}
				if (base.FrameShouldEnd())
				{
					yield return null;
				}
				j = z3;
			}
			j = i;
		}
		foreach (PosterData posterData in this.postersToBuild)
		{
			this.ec.BuildPoster(posterData.poster, this.ec.CellFromPosition(posterData.position), posterData.direction);
			if (base.FrameShouldEnd())
			{
				yield return null;
			}
		}
		List<PosterData>.Enumerator enumerator10 = default(List<PosterData>.Enumerator);
		foreach (RoomController roomController10 in this.ec.rooms)
		{
			roomController10.functions.Build(this, this.controlledRNG);
			if (base.FrameShouldEnd())
			{
				yield return null;
			}
		}
		enumerator9 = default(List<RoomController>.Enumerator);
		foreach (DoorPlacement placement in this.doorPlacements)
		{
			base.BuildDoorPlacement(placement);
			if (base.FrameShouldEnd())
			{
				yield return null;
			}
		}
		enumerator8 = default(List<DoorPlacement>.Enumerator);
		this.doorPlacements.Clear();
		this.ec.lightMode = this.ld.lightMode;
		Cell[] openCheck = new Cell[4];
		List<Direction>[] openCheckDirs = new List<Direction>[]
		{
			new List<Direction>(),
			new List<Direction>(),
			new List<Direction>(),
			new List<Direction>()
		};
		openCheckDirs[0].Add(Direction.North);
		openCheckDirs[0].Add(Direction.East);
		openCheckDirs[1].Add(Direction.North);
		openCheckDirs[1].Add(Direction.West);
		openCheckDirs[2].Add(Direction.South);
		openCheckDirs[2].Add(Direction.East);
		openCheckDirs[3].Add(Direction.South);
		openCheckDirs[3].Add(Direction.West);
		List<Direction> currentOpenCheckDirs = new List<Direction>();
		for (i = 0; i < this.levelSize.x - 1; i = j + 1)
		{
			for (int z3 = 0; z3 < this.levelSize.z - 1; z3 = j + 1)
			{
				bool flag3 = true;
				bool @null = this.ec.cells[i, z3].Null;
				openCheck[0] = this.ec.cells[i, z3];
				openCheck[1] = this.ec.cells[i + 1, z3];
				openCheck[2] = this.ec.cells[i, z3 + 1];
				openCheck[3] = this.ec.cells[i + 1, z3 + 1];
				for (int num16 = 0; num16 < 4; num16++)
				{
					if (openCheck[num16].excludeFromOpen || openCheck[num16].Null != @null)
					{
						flag3 = false;
						break;
					}
					Directions.FillOpenDirectionsFromBin(currentOpenCheckDirs, openCheck[num16].NavBin);
					if (!currentOpenCheckDirs.Contains(openCheckDirs[num16][0]) || !currentOpenCheckDirs.Contains(openCheckDirs[num16][1]))
					{
						flag3 = false;
						break;
					}
				}
				if (flag3)
				{
					foreach (Cell cell9 in openCheck)
					{
						cell9.open = true;
						for (int num17 = 0; num17 < openCheck.Length; num17++)
						{
							if (!cell9.openTiles.Contains(openCheck[num17]) && cell9 != openCheck[num17])
							{
								cell9.openTiles.Add(openCheck[num17]);
							}
						}
					}
					Object.Instantiate<Transform>(this.navQuadPre, this.ec.navMeshTransform).transform.localPosition = new Vector3((float)i * 10f + 10f, 0f, (float)z3 * 10f + 10f);
				}
				if (base.FrameShouldEnd())
				{
					yield return null;
				}
				j = z3;
			}
			j = i;
		}
		List<OpenTileGroup> openTileGroups = new List<OpenTileGroup>();
		Queue<Cell> openTilesToCheck = new Queue<Cell>();
		for (i = 0; i < this.levelSize.x; i = j + 1)
		{
			for (int z3 = 0; z3 < this.levelSize.z; z3 = j + 1)
			{
				Cell cell10 = this.ec.cells[i, z3];
				if (cell10.open)
				{
					bool flag4 = false;
					foreach (OpenTileGroup openTileGroup in openTileGroups)
					{
						if (cell10.openTileGroup == openTileGroup)
						{
							flag4 = true;
						}
					}
					if (!flag4)
					{
						OpenTileGroup currentGroup = new OpenTileGroup();
						openTileGroups.Add(currentGroup);
						openTilesToCheck.Clear();
						openTilesToCheck.Enqueue(cell10);
						while (openTilesToCheck.Count > 0)
						{
							cell10 = openTilesToCheck.Dequeue();
							cell10.openTileGroup = currentGroup;
							currentGroup.cells.Add(cell10);
							foreach (Cell cell11 in cell10.openTiles)
							{
								if (cell11.openTileGroup != currentGroup && !openTilesToCheck.Contains(cell11))
								{
									openTilesToCheck.Enqueue(cell11);
								}
							}
							if (base.FrameShouldEnd())
							{
								yield return null;
							}
						}
						if (base.FrameShouldEnd())
						{
							yield return null;
						}
						foreach (Cell endTile in currentGroup.cells)
						{
							foreach (Direction direction2 in Directions.OpenDirectionsFromBin(endTile.ConstBin))
							{
								if (this.ec.ContainsCoordinates(endTile.position + direction2.ToIntVector2()) && !this.ec.CellFromPosition(endTile.position + direction2.ToIntVector2()).Null && !currentGroup.cells.Contains(this.ec.CellFromPosition(endTile.position + direction2.ToIntVector2())))
								{
									currentGroup.exits.Add(new OpenGroupExit(endTile, direction2));
								}
								if (base.FrameShouldEnd())
								{
									yield return null;
								}
							}
							List<Direction>.Enumerator enumerator12 = default(List<Direction>.Enumerator);
							endTile = null;
						}
						enumerator5 = default(List<Cell>.Enumerator);
						if (base.FrameShouldEnd())
						{
							yield return null;
						}
						currentGroup = null;
					}
				}
				if (base.FrameShouldEnd())
				{
					yield return null;
				}
				j = z3;
			}
			if (base.FrameShouldEnd())
			{
				yield return null;
			}
			j = i;
		}
		base.BlockTilesToBlock();
		if (base.FrameShouldEnd())
		{
			yield return null;
		}
		for (i = 0; i < this.ld.forcedSpecialHallBuilders.Length; i = j + 1)
		{
			ObjectBuilder objectBuilder = Object.Instantiate<ObjectBuilder>(this.ld.forcedSpecialHallBuilders[i], this.ec.transform);
			objectBuilder.Build(this.ec, this, this.halls[0], this.controlledRNG);
			this.ec.obstacles.Add(objectBuilder.obstacle);
			yield return null;
			j = i;
		}
		foreach (RandomEvent randomEvent4 in events)
		{
			randomEvent4.AfterUpdateSetup();
			yield return null;
		}
		enumerator3 = default(List<RandomEvent>.Enumerator);
		int specialBuilders = this.controlledRNG.Next(this.ld.minSpecialBuilders, this.ld.maxSpecialBuilders);
		i = 0;
		while (i < specialBuilders && this.ld.specialHallBuilders.Length != 0)
		{
			WeightedSelection<ObjectBuilder>[] specialHallBuilders = this.ld.specialHallBuilders;
			ObjectBuilder objectBuilder2 = Object.Instantiate<ObjectBuilder>(WeightedSelection<ObjectBuilder>.ControlledRandomSelection(specialHallBuilders, this.controlledRNG), this.ec.transform);
			objectBuilder2.Build(this.ec, this, this.halls[0], this.controlledRNG);
			this.ec.obstacles.Add(objectBuilder2.obstacle);
			yield return null;
			j = i;
			i = j + 1;
		}
		for (i = 0; i < this.levelSize.x; i = j + 1)
		{
			for (int z3 = 0; z3 < this.levelSize.z; z3 = j + 1)
			{
				if (!this.ec.cells[i, z3].Null)
				{
					Cell cell12 = this.ec.cells[i, z3];
					if (cell12.room.type == RoomType.Hall)
					{
						RandomSelection<HallBuilder>[] standardHallBuilders = this.ld.standardHallBuilders;
						List<HallBuilder> list10 = RandomSelection<HallBuilder>.ControlledRandomSelection(standardHallBuilders, this.controlledRNG);
						if (list10.Count > 0)
						{
							HallBuilder hallBuilder = Object.Instantiate<HallBuilder>(list10[this.controlledRNG.Next(0, list10.Count)], cell12.room.transform);
							hallBuilder.Build(cell12, this.ec, new Random(this.controlledRNG.Next()));
							Object.Destroy(hallBuilder.gameObject);
							yield return null;
						}
					}
				}
				if (base.FrameShouldEnd())
				{
					yield return null;
				}
				j = z3;
			}
			j = i;
		}
		CoreGameManager.lightMapPaused = true;
		if (base.FrameShouldEnd())
		{
			yield return null;
		}
		foreach (RoomController room in this.ec.rooms)
		{
			RoomController room;
			if (room.potentialPosters.Count > 0)
			{
				while (room.HasFreeWall && this.controlledRNG.NextDouble() < (double)room.posterChance)
				{
					this.ec.BuildPosterInRoom(room, WeightedSelection<PosterObject>.ControlledRandomSelectionList(WeightedPosterObject.Convert(room.potentialPosters), this.controlledRNG), this.controlledRNG);
					if (base.FrameShouldEnd())
					{
						yield return null;
					}
				}
			}
			room = null;
		}
		enumerator9 = default(List<RoomController>.Enumerator);
		for (i = 0; i < this.levelSize.x; i = j + 1)
		{
			for (int z3 = 0; z3 < this.levelSize.z; z3 = j + 1)
			{
				IntVector2 intVector7 = new IntVector2(i, z3);
				if (!this.ec.cells[intVector7.x, intVector7.z].Null)
				{
					Cell endTile = this.ec.cells[intVector7.x, intVector7.z];
					if (endTile.HasFreeWall && endTile.room.acceptsPosters && (double)this.ld.posterChance > this.controlledRNG.NextDouble() * 100.0)
					{
						Direction dir = endTile.RandomUncoveredDirection(this.controlledRNG);
						WeightedSelection<PosterObject>[] posters = this.ld.posters;
						PosterObject poster = WeightedSelection<PosterObject>.ControlledRandomSelection(posters, this.controlledRNG);
						this.ec.BuildPoster(poster, endTile, dir, this.controlledRNG);
					}
					if (endTile.room.type == RoomType.Hall && endTile.HardCoverageFits(CellCoverage.Up))
					{
						bool success = false;
						foreach (Cell cell13 in this.ec.lights)
						{
							if (!cell13.permanentLight && Mathf.Abs(endTile.position.x - cell13.position.x) + Mathf.Abs(endTile.position.z - cell13.position.z) < this.ld.maxLightDistance && this.ec.NavigableDistance(cell13, endTile, PathType.Const) < this.ld.maxLightDistance)
							{
								success = true;
								break;
							}
							if (base.FrameShouldEnd())
							{
								yield return null;
							}
						}
						enumerator5 = default(List<Cell>.Enumerator);
						if (!success)
						{
							endTile.room.ec.GenerateLight(endTile, this.ld.standardLightColor, this.ld.standardLightStrength);
							if (endTile.room.lightPre != null)
							{
								Object.Instantiate<Transform>(endTile.room.lightPre, endTile.TileTransform);
							}
							else
							{
								Object.Instantiate<Transform>(hallLight, endTile.TileTransform);
							}
						}
					}
					endTile = null;
				}
				if (base.FrameShouldEnd())
				{
					yield return null;
				}
				j = z3;
			}
			j = i;
		}
		foreach (RoomController room in this.ec.rooms)
		{
			RoomController room;
			foreach (IntVector2 position6 in room.standardLightCells)
			{
				this.ec.GenerateLight(this.ec.CellFromPosition(position6), this.ld.standardLightColor, this.ld.standardLightStrength);
				if (room.lightPre != null)
				{
					Object.Instantiate<Transform>(room.lightPre, this.ec.CellFromPosition(position6).TileTransform);
				}
				else
				{
					Object.Instantiate<Transform>(hallLight, this.ec.CellFromPosition(position6).TileTransform);
				}
				if (base.FrameShouldEnd())
				{
					yield return null;
				}
			}
			List<IntVector2>.Enumerator enumerator13 = default(List<IntVector2>.Enumerator);
			room = null;
		}
		enumerator9 = default(List<RoomController>.Enumerator);
		foreach (LightSourceData lightSourceData in this.lightsToGenerate)
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
		CoreGameManager.lightMapPaused = false;
		if (base.FrameShouldEnd())
		{
			yield return null;
		}
		this.ec.BuildNavMesh();
		if (base.FrameShouldEnd())
		{
			yield return null;
		}
		foreach (RoomController roomController11 in this.ec.rooms)
		{
			if (roomController11.type != RoomType.Hall && roomController11.spawnItems && roomController11.category != RoomCategory.Class)
			{
				int num18 = roomController11.maxItemValue;
				if (roomController11.doors.Count <= 1)
				{
					num18 += this.ld.singleEntranceItemVal;
				}
				if (roomController11.roomsFromHall > 0)
				{
					num18 += 10 * (int)Mathf.Pow(2f, (float)(roomController11.roomsFromHall + 1));
				}
				roomController11.maxItemValue = num18;
			}
			if (base.FrameShouldEnd())
			{
				yield return null;
			}
		}
		enumerator9 = default(List<RoomController>.Enumerator);
		lootTable.SortByValue();
		string str = "High End Loot:\n";
		List<ItemObject> highEndLoot = new List<ItemObject>();
		List<ItemObject> midEndLoot = new List<ItemObject>();
		List<ItemObject> lowEndLoot = new List<ItemObject>();
		int num19 = 0;
		while (lootTable.Count > 0 && num19 < this.ld.highEndCutoff)
		{
			str = str + lootTable[0].nameKey + "\n";
			highEndLoot.Add(lootTable[0]);
			num19 += lootTable[0].value;
			lootTable.RemoveAt(0);
		}
		List<ItemObject> emergencyHighEndLoot = new List<ItemObject>(highEndLoot);
		str += "Mid End Loot:\n";
		while (lootTable.Count > 0 && lootTable.Value() > this.ld.lowEndCutoff)
		{
			str = str + lootTable[0].nameKey + "\n";
			midEndLoot.Add(lootTable[0]);
			lootTable.RemoveAt(0);
		}
		lowEndLoot.AddRange(lootTable);
		str += "Low End Loot:\n";
		foreach (ItemObject itemObject in lootTable)
		{
			str = str + itemObject.nameKey + "\n";
		}
		if (base.FrameShouldEnd())
		{
			yield return null;
		}
		foreach (ItemObject item4 in highEndLoot)
		{
			base.PlaceItemInRandomRoom(item4, true, false, this.controlledRNG);
			if (base.FrameShouldEnd())
			{
				yield return null;
			}
		}
		List<ItemObject>.Enumerator enumerator16 = default(List<ItemObject>.Enumerator);
		foreach (ItemObject item5 in midEndLoot)
		{
			base.PlaceItemInRandomRoom(item5, false, false, this.controlledRNG);
			if (base.FrameShouldEnd())
			{
				yield return null;
			}
		}
		enumerator16 = default(List<ItemObject>.Enumerator);
		foreach (ItemObject item6 in lowEndLoot)
		{
			base.PlaceItemInRandomRoom(item6, false, true, this.controlledRNG);
			if (base.FrameShouldEnd())
			{
				yield return null;
			}
		}
		enumerator16 = default(List<ItemObject>.Enumerator);
		foreach (RoomController roomController12 in this.ec.rooms)
		{
			while (roomController12.maxItemValue - roomController12.currentItemValue > this.ld.maxAllowedUnusedValue && roomController12.itemSpawnPoints.Count > 0)
			{
				ItemObject item7 = emergencyHighEndLoot[this.controlledRNG.Next(0, emergencyHighEndLoot.Count)];
				base.PlaceItemInRoom(item7, roomController12, this.controlledRNG);
			}
			if (base.FrameShouldEnd())
			{
				yield return null;
			}
		}
		enumerator9 = default(List<RoomController>.Enumerator);
		foreach (RoomController room2 in this.ec.rooms)
		{
			base.UpdateRoomObjectPosition(room2);
			if (base.FrameShouldEnd())
			{
				yield return null;
			}
		}
		enumerator9 = default(List<RoomController>.Enumerator);
		if (base.FrameShouldEnd())
		{
			yield return null;
		}
		this.ec.map.UpdateIcons();
		this.ec.SetSpawn(this.ec.elevators[0].transform.position + -this.ec.elevators[0].transform.forward * 10f, this.ec.elevators[0].transform.rotation);
		this.ec.standardDoors = this.standardDoors;
		if (base.FrameShouldEnd())
		{
			yield return null;
		}
		float currentTime = this.ld.initialEventGap;
		List<RandomEvent> eventsToQueue = new List<RandomEvent>();
		eventsToQueue.AddRange(events);
		while (eventsToQueue.Count > 0)
		{
			RandomEvent randomEvent5 = eventsToQueue[Random.Range(0, eventsToQueue.Count)];
			currentTime += Random.value * (this.ld.maxEventGap - this.ld.minEventGap) + this.ld.minEventGap;
			this.ec.AddEvent(randomEvent5, currentTime);
			currentTime += randomEvent5.EventTime;
			eventsToQueue.Remove(randomEvent5);
			if (base.FrameShouldEnd())
			{
				yield return null;
			}
		}
		foreach (EnvironmentObject environmentObject in this.environmentObjects)
		{
			environmentObject.LoadingFinished();
			if (base.FrameShouldEnd())
			{
				yield return null;
			}
		}
		List<EnvironmentObject>.Enumerator enumerator17 = default(List<EnvironmentObject>.Enumerator);
		List<Cell> allTiles = new List<Cell>(this.ec.AllTilesNoGarbage(false, false));
		i = 0;
		while (i < this.ec.npcsToSpawn.Count && allTiles.Count > 0)
		{
			if (this.ec.npcSpawnTile[i] == null)
			{
				List<Cell> potSpawns = new List<Cell>();
				foreach (RoomController roomController13 in this.ec.rooms)
				{
					if (this.ec.npcsToSpawn[i].spawnableRooms.Contains(roomController13.category))
					{
						potSpawns.AddRange(roomController13.AllEventSafeCellsNoGarbage());
					}
					if (base.FrameShouldEnd())
					{
						yield return null;
					}
				}
				enumerator9 = default(List<RoomController>.Enumerator);
				foreach (RoomController roomController14 in this.halls)
				{
					if (this.ec.npcsToSpawn[i].spawnableRooms.Contains(roomController14.category))
					{
						potSpawns.AddRange(roomController14.AllTilesNoGarbage(false, true));
					}
					if (base.FrameShouldEnd())
					{
						yield return null;
					}
				}
				enumerator9 = default(List<RoomController>.Enumerator);
				if (potSpawns.Count > 0)
				{
					Cell cell14 = potSpawns[this.controlledRNG.Next(0, potSpawns.Count)];
					this.ec.npcSpawnTile[i] = cell14;
				}
				else
				{
					Cell cell14 = allTiles[this.controlledRNG.Next(0, allTiles.Count)];
					this.ec.npcSpawnTile[i] = cell14;
				}
				if (base.FrameShouldEnd())
				{
					yield return null;
				}
				potSpawns = null;
			}
			j = i;
			i = j + 1;
		}
		foreach (RoomController roomController15 in this.ec.rooms)
		{
			roomController15.functions.OnGenerationFinished();
			if (base.FrameShouldEnd())
			{
				yield return null;
			}
		}
		enumerator9 = default(List<RoomController>.Enumerator);
		if (base.FrameShouldEnd())
		{
			yield return null;
		}
		this.ec.CullingManager.PrepareOcclusionCalculations();
		if (base.FrameShouldEnd())
		{
			yield return null;
		}
		for (i = 0; i < this.ec.CullingManager.TotalChunks; i = j + 1)
		{
			this.ec.CullingManager.CalculateOcclusionCullingForChunk(i);
			if (base.FrameShouldEnd())
			{
				yield return null;
			}
			j = i;
		}
		this.ec.CullingManager.SetActive(true);
		if (base.FrameShouldEnd())
		{
			yield return null;
		}
		yield return null;
		yield return null;
		this.levelInProgress = false;
		this.levelCreated = true;
		if (Singleton<CoreGameManager>.Instance.GetCamera(0) != null)
		{
			Singleton<CoreGameManager>.Instance.GetCamera(0).StopRendering(false);
		}
		sw.Stop();
		Debug.Log("Level generator took " + sw.ElapsedMilliseconds.ToString() + " milliseconds to complete.");
		yield break;
		yield break;
	}

	// Token: 0x0600091B RID: 2331 RVA: 0x000305A4 File Offset: 0x0002E7A4
	private bool RandomlyPlaceRoom(RoomAsset room, RoomController existingRoom)
	{
		RoomController roomController = null;
		return this.RandomlyPlaceRoom(room, existingRoom, false, out roomController);
	}

	// Token: 0x0600091C RID: 2332 RVA: 0x000305BE File Offset: 0x0002E7BE
	private bool RandomlyPlaceRoom(RoomAsset room, bool addDoor, out RoomController roomController)
	{
		return this.RandomlyPlaceRoom(room, null, addDoor, out roomController);
	}

	// Token: 0x0600091D RID: 2333 RVA: 0x000305CC File Offset: 0x0002E7CC
	private bool RandomlyPlaceRoom(RoomAsset room, RoomController existingRoom, bool addDoor, out RoomController roomController)
	{
		bool flag = false;
		int index = 0;
		List<IntVector2> list = new List<IntVector2>();
		List<IntVector2> list2 = new List<IntVector2>(room.potentialDoorPositions);
		List<WeightedRoomSpawn> list3 = new List<WeightedRoomSpawn>(this.potentialRoomSpawns);
		if (room.requiredDoorPositions.Count > 0)
		{
			list2 = new List<IntVector2>(room.requiredDoorPositions);
		}
		else if (room.forcedDoorPositions.Count > 0)
		{
			list2 = new List<IntVector2>(room.forcedDoorPositions);
		}
		if (list2.Count > 0)
		{
			while (list2.Count > 0)
			{
				int index2 = this.controlledRNG.Next(0, list2.Count);
				list.Add(list2[index2]);
				list2.RemoveAt(index2);
			}
		}
		else
		{
			list.Add(default(IntVector2));
		}
		List<Direction> list4 = new List<Direction>();
		List<Direction> list5 = new List<Direction>(Directions.All());
		while (list5.Count > 0)
		{
			int index3 = this.controlledRNG.Next(0, list5.Count);
			list4.Add(list5[index3]);
			list5.RemoveAt(index3);
		}
		Direction direction = list4[0];
		IntVector2 intVector = list[0];
		while (!flag && list3.Count > 0)
		{
			index = WeightedSelection<RoomSpawn>.ControlledRandomIndexList(WeightedRoomSpawn.Convert(list3), this.controlledRNG);
			IntVector2 position = list3[index].selection.position;
			RoomSpawn selection = list3[index].selection;
			int num = 0;
			while (num < list.Count && !flag)
			{
				intVector = list[num];
				int num2 = 0;
				while (num2 < list4.Count && !flag)
				{
					direction = list4[num2];
					bool flag2 = true;
					if (!base.RoomFits(room, position, intVector, direction))
					{
						break;
					}
					if (flag2)
					{
						flag = true;
					}
					num2++;
				}
				num++;
			}
			if (!flag)
			{
				list3.RemoveAt(index);
			}
		}
		if (flag)
		{
			if (existingRoom == null)
			{
				roomController = base.LoadRoom(room, list3[index].selection.position, intVector, direction, true);
				this.previousRoomSpawnPositions.Add(list3[index].selection.position);
				RoomController roomController2;
				if (addDoor && base.BuildDoorInDirection(roomController, list3[index].selection.position, list3[index].selection.direction, false, out roomController2))
				{
					roomController.RemoveFromDoorPositions(list3[index].selection.position);
					roomController.ConnectRooms(roomController2);
					roomController2.RemoveFromDoorPositions(list3[index].selection.position + list3[index].selection.direction.ToIntVector2());
				}
			}
			else
			{
				roomController = existingRoom;
				base.LoadIntoExistingRoom(existingRoom, room, list3[index].selection.position, intVector, direction, false);
			}
			return true;
		}
		Debug.Log(string.Format("Could not find valid position for room {0} after several attempts.", room.name));
		roomController = null;
		return false;
	}

	// Token: 0x04000A06 RID: 2566
	private List<RoomController> standardRooms = new List<RoomController>();
}
