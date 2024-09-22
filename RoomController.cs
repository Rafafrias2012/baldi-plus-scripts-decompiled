using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

// Token: 0x020001A0 RID: 416
public class RoomController : MonoBehaviour
{
	// Token: 0x06000959 RID: 2393 RVA: 0x00032396 File Offset: 0x00030596
	private void Awake()
	{
		this.baseMat = new Material(this.defaultMat);
		this.posterMat = new Material(this.defaultPosterMat);
	}

	// Token: 0x0600095A RID: 2394 RVA: 0x000323BC File Offset: 0x000305BC
	public void ConnectRooms(RoomController newRoom)
	{
		if (newRoom.type != RoomType.Hall && !this.connectedRooms.Contains(newRoom))
		{
			this.connectedRooms.Add(newRoom);
		}
		if (this.type != RoomType.Hall && !newRoom.connectedRooms.Contains(this))
		{
			newRoom.connectedRooms.Add(this);
		}
		if (newRoom.type == RoomType.Hall)
		{
			this.roomsFromHall = 0;
		}
		else if (this.roomsFromHall > newRoom.roomsFromHall + 1 && newRoom.roomsFromHall != 2147483647)
		{
			this.roomsFromHall = newRoom.roomsFromHall + 1;
		}
		foreach (RoomController roomController in this.connectedRooms)
		{
			roomController.UpdateHallDistance(this.roomsFromHall);
		}
	}

	// Token: 0x0600095B RID: 2395 RVA: 0x00032498 File Offset: 0x00030698
	public void UpdateHallDistance(int distance)
	{
		if (this.roomsFromHall > distance + 1 && distance != 2147483647)
		{
			this.roomsFromHall = distance + 1;
			foreach (RoomController roomController in this.connectedRooms)
			{
				roomController.UpdateHallDistance(this.roomsFromHall);
			}
		}
	}

	// Token: 0x0600095C RID: 2396 RVA: 0x0003250C File Offset: 0x0003070C
	public void AddTile(Cell tile)
	{
		this.cells.Add(tile);
	}

	// Token: 0x0600095D RID: 2397 RVA: 0x0003251A File Offset: 0x0003071A
	public void RemoveTile(Cell tile)
	{
		this.cells.Remove(tile);
	}

	// Token: 0x0600095E RID: 2398 RVA: 0x00032529 File Offset: 0x00030729
	public void GenerateTextureAtlas()
	{
		this.GenerateTextureAtlas(256);
	}

	// Token: 0x0600095F RID: 2399 RVA: 0x00032538 File Offset: 0x00030738
	public void GenerateTextureAtlas(int size)
	{
		bool flag;
		Texture2D matchingTextureAtlas = this.ec.GetMatchingTextureAtlas(this.florTex, this.wallTex, this.ceilTex, out flag);
		if (flag)
		{
			this.textureAtlas = matchingTextureAtlas;
		}
		else
		{
			this.textureAtlas = new Texture2D(size * 2, size * 2, TextureFormat.RGBA32, false);
			this.textureAtlas.filterMode = FilterMode.Point;
			Color[] colors = new Color[0];
			colors = MaterialModifier.GetColorsForTileTexture(this.florTex, size);
			this.textureAtlas.SetPixels(0, 0, size, size, colors);
			colors = MaterialModifier.GetColorsForTileTexture(this.wallTex, size);
			this.textureAtlas.SetPixels(size, size, size, size, colors);
			colors = MaterialModifier.GetColorsForTileTexture(this.ceilTex, size);
			this.textureAtlas.SetPixels(0, size, size, size, colors);
			this.textureAtlas.Apply();
			this.ec.generatedTextureAtlases.Add(new RoomTextureAtlas(this.florTex, this.wallTex, this.ceilTex, this.textureAtlas));
		}
		if (GraphicsFormatUtility.HasAlphaChannel(this.florTex.graphicsFormat) || GraphicsFormatUtility.HasAlphaChannel(this.wallTex.graphicsFormat) || GraphicsFormatUtility.HasAlphaChannel(this.ceilTex.graphicsFormat))
		{
			this.baseMat = new Material(this.defaultAlphaMat);
			this.posterMat = new Material(this.defaultAlphaPosterMap);
		}
		this.baseMat.SetTexture("_MainTex", this.textureAtlas);
		this.posterMat.SetTexture("_MainTex", this.textureAtlas);
	}

	// Token: 0x06000960 RID: 2400 RVA: 0x000326B0 File Offset: 0x000308B0
	public void UpdatePosition()
	{
		IntVector2 intVector = new IntVector2(this.ec.levelSize.x, this.ec.levelSize.z);
		IntVector2 intVector2 = default(IntVector2);
		for (int i = 0; i < this.ec.levelSize.x; i++)
		{
			for (int j = 0; j < this.ec.levelSize.z; j++)
			{
				if (this.ec.cells[i, j].room == this)
				{
					if (i < intVector.x)
					{
						intVector.x = i;
					}
					if (i > intVector2.x)
					{
						intVector2.x = i;
					}
					if (j < intVector.z)
					{
						intVector.z = j;
					}
					if (j > intVector2.z)
					{
						intVector2.z = j;
					}
				}
			}
		}
		this.position = intVector;
		this.size.x = intVector2.x - intVector.x + 1;
		this.size.z = intVector2.z - intVector.z + 1;
	}

	// Token: 0x06000961 RID: 2401 RVA: 0x000327C6 File Offset: 0x000309C6
	public void RemoveFromDoorPositions(IntVector2 position)
	{
		this.potentialDoorPositions.Remove(position);
		this.forcedDoorPositions.Remove(position);
	}

	// Token: 0x170000CB RID: 203
	// (get) Token: 0x06000962 RID: 2402 RVA: 0x000327E2 File Offset: 0x000309E2
	public bool HasDoorPositions
	{
		get
		{
			return this.potentialDoorPositions.Count > 0 || this.forcedDoorPositions.Count > 0;
		}
	}

	// Token: 0x06000963 RID: 2403 RVA: 0x00032802 File Offset: 0x00030A02
	public bool ContainsDoorPosition(IntVector2 position)
	{
		return this.potentialDoorPositions.Contains(position) || this.forcedDoorPositions.Contains(position);
	}

	// Token: 0x06000964 RID: 2404 RVA: 0x00032820 File Offset: 0x00030A20
	public bool GetRandomConnectionPoint(Random rng, out IntVector2 position, out Direction direction)
	{
		this._potentialConnectionPoints.Clear();
		this._potentialConnectionDirections.Clear();
		for (int i = 0; i < this.connectionPoints.Count; i++)
		{
			Cell cell = this.ec.CellFromPosition(this.connectionPoints[i]);
			if (cell.HasHardFreeWall)
			{
				for (int j = 0; j < 4; j++)
				{
					if (cell.HasWallInDirection((Direction)j) && !cell.WallHardCovered((Direction)j) && this.ec.ContainsCoordinates(cell.position + ((Direction)j).ToIntVector2()) && this.ec.CellFromPosition(cell.position + ((Direction)j).ToIntVector2()).Null)
					{
						this._potentialConnectionPoints.Add(cell.position);
						this._potentialConnectionDirections.Add((Direction)j);
					}
				}
			}
		}
		if (this._potentialConnectionPoints.Count > 0)
		{
			int index = rng.Next(0, this._potentialConnectionPoints.Count);
			position = this._potentialConnectionPoints[index];
			direction = this._potentialConnectionDirections[index];
			return true;
		}
		position = default(IntVector2);
		direction = Direction.Null;
		return false;
	}

	// Token: 0x170000CC RID: 204
	// (get) Token: 0x06000965 RID: 2405 RVA: 0x00032949 File Offset: 0x00030B49
	public Texture2D TextureAtlas
	{
		get
		{
			return this.textureAtlas;
		}
	}

	// Token: 0x06000966 RID: 2406 RVA: 0x00032951 File Offset: 0x00030B51
	public List<Cell> GetNewTileList()
	{
		return new List<Cell>(this.cells);
	}

	// Token: 0x06000967 RID: 2407 RVA: 0x00032960 File Offset: 0x00030B60
	public List<Cell> AllTilesNoGarbage(bool includeOffLimits, bool includeWithHardCoverage)
	{
		this._allTiles.Clear();
		for (int i = 0; i < this.cells.Count; i++)
		{
			if ((includeOffLimits || !this.cells[i].offLimits) && (includeWithHardCoverage || !this.cells[i].HasAnyHardCoverage))
			{
				this._allTiles.Add(this.cells[i]);
			}
		}
		return this._allTiles;
	}

	// Token: 0x06000968 RID: 2408 RVA: 0x000329D8 File Offset: 0x00030BD8
	public List<Cell> AllEntitySafeCellsNoGarbage()
	{
		this._allTiles.Clear();
		for (int i = 0; i < this.entitySafeCells.Count; i++)
		{
			this._allTiles.Add(this.ec.CellFromPosition(this.entitySafeCells[i]));
		}
		return this._allTiles;
	}

	// Token: 0x06000969 RID: 2409 RVA: 0x00032A30 File Offset: 0x00030C30
	public Cell RandomEntitySafeCellNoGarbage()
	{
		if (this.entitySafeCells.Count > 0)
		{
			return this.ec.CellFromPosition(this.entitySafeCells[Random.Range(0, this.entitySafeCells.Count)]);
		}
		return this.cells[0];
	}

	// Token: 0x0600096A RID: 2410 RVA: 0x00032A80 File Offset: 0x00030C80
	public Cell ControlledRandomEntitySafeCellNoGarbage(Random rng)
	{
		if (this.entitySafeCells.Count > 0)
		{
			return this.ec.CellFromPosition(this.entitySafeCells[rng.Next(0, this.entitySafeCells.Count)]);
		}
		return this.cells[0];
	}

	// Token: 0x0600096B RID: 2411 RVA: 0x00032AD0 File Offset: 0x00030CD0
	public List<Cell> AllEventSafeCellsNoGarbage()
	{
		this._allTiles.Clear();
		for (int i = 0; i < this.eventSafeCells.Count; i++)
		{
			this._allTiles.Add(this.ec.CellFromPosition(this.eventSafeCells[i]));
		}
		return this._allTiles;
	}

	// Token: 0x0600096C RID: 2412 RVA: 0x00032B28 File Offset: 0x00030D28
	public Cell RandomEventSafeCellNoGarbage()
	{
		if (this.eventSafeCells.Count > 0)
		{
			return this.ec.CellFromPosition(this.eventSafeCells[Random.Range(0, this.eventSafeCells.Count)]);
		}
		return this.cells[0];
	}

	// Token: 0x0600096D RID: 2413 RVA: 0x00032B78 File Offset: 0x00030D78
	public Cell ControlledRandomEventSafeCellNoGarbage(Random rng)
	{
		if (this.eventSafeCells.Count > 0)
		{
			return this.ec.CellFromPosition(this.eventSafeCells[rng.Next(0, this.eventSafeCells.Count)]);
		}
		return this.cells[0];
	}

	// Token: 0x0600096E RID: 2414 RVA: 0x00032BC8 File Offset: 0x00030DC8
	public List<Cell> GetTilesOfShape(List<TileShape> shapes, CellCoverage coverageToFit, bool includeOpen)
	{
		List<Cell> newTileList = this.GetNewTileList();
		for (int i = 0; i < newTileList.Count; i++)
		{
			if (!shapes.Contains(newTileList[i].shape) || newTileList[i].HasAnyHardCoverage || (newTileList[i].open && !includeOpen) || !newTileList[i].HardCoverageFitsInAnyDirection(coverageToFit))
			{
				newTileList.RemoveAt(i);
				i--;
			}
		}
		return newTileList;
	}

	// Token: 0x0600096F RID: 2415 RVA: 0x00032C3B File Offset: 0x00030E3B
	public List<Cell> GetTilesOfShape(List<TileShape> shapes, bool includeOpen)
	{
		return this.GetTilesOfShape(shapes, CellCoverage.None, includeOpen);
	}

	// Token: 0x06000970 RID: 2416 RVA: 0x00032C46 File Offset: 0x00030E46
	public Cell TileAtIndex(int index)
	{
		return this.cells[index];
	}

	// Token: 0x170000CD RID: 205
	// (get) Token: 0x06000971 RID: 2417 RVA: 0x00032C54 File Offset: 0x00030E54
	public int TileCount
	{
		get
		{
			return this.cells.Count;
		}
	}

	// Token: 0x06000972 RID: 2418 RVA: 0x00032C64 File Offset: 0x00030E64
	public bool ContainsCoordinates(IntVector2 pos)
	{
		return pos.x >= this.position.x && pos.x < this.position.x + this.size.x && pos.z >= this.position.z && pos.z < this.position.z + this.size.z;
	}

	// Token: 0x06000973 RID: 2419 RVA: 0x00032CD8 File Offset: 0x00030ED8
	public bool containsPosition(Vector3 position)
	{
		return position.x <= this.ec.RealRoomMax(this).x && position.x >= this.ec.RealRoomMin(this).x && position.z <= this.ec.RealRoomMax(this).z && position.z >= this.ec.RealRoomMin(this).z;
	}

	// Token: 0x170000CE RID: 206
	// (get) Token: 0x06000974 RID: 2420 RVA: 0x00032D4C File Offset: 0x00030F4C
	public bool HasFreeWall
	{
		get
		{
			using (List<Cell>.Enumerator enumerator = this.cells.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.HasFreeWall)
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	// Token: 0x04000A91 RID: 2705
	public EnvironmentController ec;

	// Token: 0x04000A92 RID: 2706
	public RoomCategory category;

	// Token: 0x04000A93 RID: 2707
	public RoomType type;

	// Token: 0x04000A94 RID: 2708
	public Direction dir;

	// Token: 0x04000A95 RID: 2709
	public Color color = Color.white;

	// Token: 0x04000A96 RID: 2710
	public Material mapMaterial;

	// Token: 0x04000A97 RID: 2711
	public Texture2D florTex;

	// Token: 0x04000A98 RID: 2712
	public Texture2D wallTex;

	// Token: 0x04000A99 RID: 2713
	public Texture2D ceilTex;

	// Token: 0x04000A9A RID: 2714
	public StandardDoorMats doorMats;

	// Token: 0x04000A9B RID: 2715
	public WindowObject windowObject;

	// Token: 0x04000A9C RID: 2716
	public IntVector2 position;

	// Token: 0x04000A9D RID: 2717
	public IntVector2 size;

	// Token: 0x04000A9E RID: 2718
	public IntVector2 maxSize;

	// Token: 0x04000A9F RID: 2719
	public int spawnWeight = 100;

	// Token: 0x04000AA0 RID: 2720
	public int nearbyRooms;

	// Token: 0x04000AA1 RID: 2721
	public int roomsFromHall = int.MaxValue;

	// Token: 0x04000AA2 RID: 2722
	public List<WeightedItemObject> itemList = new List<WeightedItemObject>();

	// Token: 0x04000AA3 RID: 2723
	public int minItemValue;

	// Token: 0x04000AA4 RID: 2724
	public int maxItemValue = 100;

	// Token: 0x04000AA5 RID: 2725
	public int currentItemValue;

	// Token: 0x04000AA6 RID: 2726
	public bool expandable;

	// Token: 0x04000AA7 RID: 2727
	public bool acceptsExits;

	// Token: 0x04000AA8 RID: 2728
	public bool acceptsPosters = true;

	// Token: 0x04000AA9 RID: 2729
	public List<Cell> cells = new List<Cell>();

	// Token: 0x04000AAA RID: 2730
	private List<Cell> _allTiles = new List<Cell>();

	// Token: 0x04000AAB RID: 2731
	public List<IntVector2> builtDoorPositions = new List<IntVector2>();

	// Token: 0x04000AAC RID: 2732
	public List<IntVector2> potentialDoorPositions = new List<IntVector2>();

	// Token: 0x04000AAD RID: 2733
	public List<IntVector2> forcedDoorPositions = new List<IntVector2>();

	// Token: 0x04000AAE RID: 2734
	public List<IntVector2> connectionPoints = new List<IntVector2>();

	// Token: 0x04000AAF RID: 2735
	public List<IntVector2> entitySafeCells = new List<IntVector2>();

	// Token: 0x04000AB0 RID: 2736
	public List<IntVector2> eventSafeCells = new List<IntVector2>();

	// Token: 0x04000AB1 RID: 2737
	public List<IntVector2> standardLightCells = new List<IntVector2>();

	// Token: 0x04000AB2 RID: 2738
	public List<RoomController> connectedRooms;

	// Token: 0x04000AB3 RID: 2739
	public RoomFunctionContainer functions;

	// Token: 0x04000AB4 RID: 2740
	public GameObject functionObject;

	// Token: 0x04000AB5 RID: 2741
	public GameObject objectObject;

	// Token: 0x04000AB6 RID: 2742
	public Door doorPre;

	// Token: 0x04000AB7 RID: 2743
	public Transform lightPre;

	// Token: 0x04000AB8 RID: 2744
	public List<Door> doors;

	// Token: 0x04000AB9 RID: 2745
	public List<Direction> doorDirs;

	// Token: 0x04000ABA RID: 2746
	public List<ItemSpawnPoint> itemSpawnPoints = new List<ItemSpawnPoint>();

	// Token: 0x04000ABB RID: 2747
	public WeightedTransform[] decorations = new WeightedTransform[0];

	// Token: 0x04000ABC RID: 2748
	public List<WeightedPosterObject> potentialPosters;

	// Token: 0x04000ABD RID: 2749
	public float windowChance;

	// Token: 0x04000ABE RID: 2750
	public float posterChance;

	// Token: 0x04000ABF RID: 2751
	public bool spawnItems = true;

	// Token: 0x04000AC0 RID: 2752
	public bool offLimits;

	// Token: 0x04000AC1 RID: 2753
	public Material defaultMat;

	// Token: 0x04000AC2 RID: 2754
	public Material defaultAlphaMat;

	// Token: 0x04000AC3 RID: 2755
	public Material defaultPosterMat;

	// Token: 0x04000AC4 RID: 2756
	public Material defaultAlphaPosterMap;

	// Token: 0x04000AC5 RID: 2757
	public Material baseMat;

	// Token: 0x04000AC6 RID: 2758
	public Material posterMat;

	// Token: 0x04000AC7 RID: 2759
	public Material wallMat;

	// Token: 0x04000AC8 RID: 2760
	public Material floorMat;

	// Token: 0x04000AC9 RID: 2761
	public Material ceilingMat;

	// Token: 0x04000ACA RID: 2762
	public Texture2D textureAtlas;

	// Token: 0x04000ACB RID: 2763
	public Texture2D defaultPoster;

	// Token: 0x04000ACC RID: 2764
	private List<IntVector2> _potentialConnectionPoints = new List<IntVector2>();

	// Token: 0x04000ACD RID: 2765
	private List<Direction> _potentialConnectionDirections = new List<Direction>();
}
