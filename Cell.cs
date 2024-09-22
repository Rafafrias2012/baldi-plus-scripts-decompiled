using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200015F RID: 351
public class Cell : IHeapItem<Cell>, IComparable<Cell>
{
	// Token: 0x060007AB RID: 1963 RVA: 0x00026D45 File Offset: 0x00024F45
	public void Initialize()
	{
		this.initalized = true;
		this.doorDirs = new List<Direction>();
		this.doors = new List<Door>();
		this.doorDirsSpace = new List<Direction>();
		this._openDirs = new List<Direction>();
		this.openTiles = new List<Cell>();
	}

	// Token: 0x060007AC RID: 1964 RVA: 0x00026D88 File Offset: 0x00024F88
	public void Uninitialize()
	{
		this.initalized = false;
		this.doorDirs.Clear();
		this.doors.Clear();
		this.doorDirsSpace.Clear();
		this._openDirs.Clear();
		this.openTiles.Clear();
		this.hasChunk = false;
		this.chunk = null;
		if (this.tileLoaded)
		{
			Object.Destroy(this.tile.gameObject);
		}
		this.tileLoaded = false;
	}

	// Token: 0x060007AD RID: 1965 RVA: 0x00026E00 File Offset: 0x00025000
	public void LoadTile()
	{
		this.tile = Object.Instantiate<Tile>(this.room.ec.TilePre, this.room.transform);
		this.tile.transform.localPosition = new Vector3((float)(this.position.x * 10 + 5), 0f, (float)(this.position.z * 10 + 5));
		this.tileLoaded = true;
		this.LoadTileMesh();
		this.HideTile(this.tileHidden);
	}

	// Token: 0x060007AE RID: 1966 RVA: 0x00026E88 File Offset: 0x00025088
	private void LoadTileMesh()
	{
		if (this.tileLoaded)
		{
			this.tile.MeshFilter.sharedMesh = this.room.ec.TileMesh(this.constBin);
			for (int i = 0; i < 4; i++)
			{
				this.tile.Collider((Direction)i).SetActive(this.constBin.ContainsDirection((Direction)i));
			}
		}
	}

	// Token: 0x060007AF RID: 1967 RVA: 0x00026EEC File Offset: 0x000250EC
	public void SetShape(int bin, TileShape shape)
	{
		this.constBin = bin;
		this.navBin = bin;
		this.soundBin = bin;
		this.shape = shape;
		if (this.tileLoaded)
		{
			this.LoadTileMesh();
		}
		this.room.ec.RecalculateNavigation();
		GameCamera.RecalculateDijkstraMap();
	}

	// Token: 0x060007B0 RID: 1968 RVA: 0x00026F38 File Offset: 0x00025138
	public void HideTile(bool value)
	{
		if (this.tileLoaded)
		{
			this.tile.MeshRenderer.enabled = !value;
		}
		this.tileHidden = value;
	}

	// Token: 0x060007B1 RID: 1969 RVA: 0x00026F5D File Offset: 0x0002515D
	public void SetBase(Material mat)
	{
		if (this.tileLoaded)
		{
			this.tile.MeshRenderer.sharedMaterial = mat;
		}
	}

	// Token: 0x060007B2 RID: 1970 RVA: 0x00026F78 File Offset: 0x00025178
	public void PrepareForPoster()
	{
		if (this.tile.MeshRenderer.sharedMaterial.shader.name != "Shader Graphs/TileStandardWPoster")
		{
			MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
			this.tile.MeshRenderer.sharedMaterial = this.room.posterMat;
			this.tile.MeshRenderer.GetPropertyBlock(materialPropertyBlock);
			materialPropertyBlock.SetTexture("_PosterN", this.room.defaultPoster);
			materialPropertyBlock.SetTexture("_PosterE", this.room.defaultPoster);
			materialPropertyBlock.SetTexture("_PosterS", this.room.defaultPoster);
			materialPropertyBlock.SetTexture("_PosterW", this.room.defaultPoster);
			this.tile.MeshRenderer.SetPropertyBlock(materialPropertyBlock);
		}
	}

	// Token: 0x060007B3 RID: 1971 RVA: 0x0002704C File Offset: 0x0002524C
	public void AddPoster(Direction dir, Texture overlay)
	{
		this.PrepareForPoster();
		MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
		this.tile.MeshRenderer.GetPropertyBlock(materialPropertyBlock);
		switch (dir)
		{
		case Direction.North:
			materialPropertyBlock.SetTexture("_PosterN", overlay);
			break;
		case Direction.East:
			materialPropertyBlock.SetTexture("_PosterE", overlay);
			break;
		case Direction.South:
			materialPropertyBlock.SetTexture("_PosterS", overlay);
			break;
		case Direction.West:
			materialPropertyBlock.SetTexture("_PosterW", overlay);
			break;
		}
		this.tile.MeshRenderer.SetPropertyBlock(materialPropertyBlock);
		this.SoftCoverWall(dir);
	}

	// Token: 0x060007B4 RID: 1972 RVA: 0x000270DC File Offset: 0x000252DC
	public void AddRenderer(Renderer renderer)
	{
		this.renderers.Add(renderer);
	}

	// Token: 0x060007B5 RID: 1973 RVA: 0x000270EA File Offset: 0x000252EA
	public bool TileMatches(RoomController checkRoom)
	{
		return this.room == checkRoom;
	}

	// Token: 0x060007B6 RID: 1974 RVA: 0x000270F8 File Offset: 0x000252F8
	private Direction RandomOccupiedDirection(int bin, Random rng)
	{
		int num = 0;
		for (int i = 0; i < 4; i++)
		{
			if ((bin & 1 << i) > 0)
			{
				Cell._potentialDirs[num] = i;
				num++;
			}
		}
		if (num > 0)
		{
			return (Direction)Cell._potentialDirs[rng.Next(0, num)];
		}
		return Direction.Null;
	}

	// Token: 0x060007B7 RID: 1975 RVA: 0x00027140 File Offset: 0x00025340
	private Direction RandomUnoccupiedDirection(int bin, Random rng)
	{
		int num = 0;
		for (int i = 0; i < 4; i++)
		{
			if ((bin & 1 << i) == 0)
			{
				Cell._potentialDirs[num] = i;
				num++;
			}
		}
		if (num > 0)
		{
			return (Direction)Cell._potentialDirs[rng.Next(0, num)];
		}
		return Direction.Null;
	}

	// Token: 0x060007B8 RID: 1976 RVA: 0x00027185 File Offset: 0x00025385
	public Direction RandomConstDirection(Random rng)
	{
		return this.RandomOccupiedDirection(this.ConstBin, rng);
	}

	// Token: 0x060007B9 RID: 1977 RVA: 0x00027194 File Offset: 0x00025394
	public Direction RandomUncoveredDirection(Random rng)
	{
		return this.RandomUnoccupiedDirection(this.SoftWallAvailabilityBin, rng);
	}

	// Token: 0x17000090 RID: 144
	// (get) Token: 0x060007BA RID: 1978 RVA: 0x000271A4 File Offset: 0x000253A4
	public List<Direction> AllWallDirections
	{
		get
		{
			List<Direction> list = new List<Direction>();
			for (int i = 0; i < 4; i++)
			{
				if ((this.ConstBin & 1 << i) > 0)
				{
					list.Add((Direction)i);
				}
			}
			return list;
		}
	}

	// Token: 0x17000091 RID: 145
	// (get) Token: 0x060007BB RID: 1979 RVA: 0x000271DB File Offset: 0x000253DB
	public List<Direction> AllOpenNavDirections
	{
		get
		{
			Directions.FillOpenDirectionsFromBin(this._openDirs, this.navBin);
			return this._openDirs;
		}
	}

	// Token: 0x17000092 RID: 146
	// (get) Token: 0x060007BC RID: 1980 RVA: 0x000271F4 File Offset: 0x000253F4
	public int ConstBin
	{
		get
		{
			return this.constBin;
		}
	}

	// Token: 0x17000093 RID: 147
	// (get) Token: 0x060007BD RID: 1981 RVA: 0x000271FC File Offset: 0x000253FC
	// (set) Token: 0x060007BE RID: 1982 RVA: 0x00027204 File Offset: 0x00025404
	public int NavBin
	{
		get
		{
			return this.navBin;
		}
		set
		{
			this.navBin = value;
		}
	}

	// Token: 0x17000094 RID: 148
	// (get) Token: 0x060007BF RID: 1983 RVA: 0x0002720D File Offset: 0x0002540D
	public int SoundBin
	{
		get
		{
			return this.soundBin;
		}
	}

	// Token: 0x17000095 RID: 149
	// (get) Token: 0x060007C0 RID: 1984 RVA: 0x00027215 File Offset: 0x00025415
	public int HardCoverageBin
	{
		get
		{
			return (int)this.hardCoverage;
		}
	}

	// Token: 0x17000096 RID: 150
	// (get) Token: 0x060007C1 RID: 1985 RVA: 0x0002721D File Offset: 0x0002541D
	public int HardWallAvailabilityBin
	{
		get
		{
			return ~(this.ConstBin ^ (int)this.hardCoverage) | (int)this.hardCoverage;
		}
	}

	// Token: 0x060007C2 RID: 1986 RVA: 0x00027234 File Offset: 0x00025434
	public bool HardCoverageFits(CellCoverage coverage)
	{
		return (coverage & (CellCoverage)this.HardCoverageBin) == CellCoverage.None;
	}

	// Token: 0x060007C3 RID: 1987 RVA: 0x00027244 File Offset: 0x00025444
	public bool HardCoverageFitsInAnyDirection(CellCoverage coverage)
	{
		for (int i = 0; i < 4; i++)
		{
			if (this.HardCoverageFits(coverage.Rotated((Direction)i)))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x17000097 RID: 151
	// (get) Token: 0x060007C4 RID: 1988 RVA: 0x0002726F File Offset: 0x0002546F
	public bool HasAnyHardCoverage
	{
		get
		{
			return this.hardCoverage > CellCoverage.None;
		}
	}

	// Token: 0x060007C5 RID: 1989 RVA: 0x0002727A File Offset: 0x0002547A
	public bool WallHardCovered(Direction dir)
	{
		return (this.hardCoverage & (CellCoverage)(1 << (int)dir)) > CellCoverage.None;
	}

	// Token: 0x060007C6 RID: 1990 RVA: 0x0002728C File Offset: 0x0002548C
	public void FilterDirectionsThroughHardCoverage(List<Direction> list, bool debug)
	{
		for (int i = 0; i < list.Count; i++)
		{
			if (debug)
			{
				Debug.Log(string.Format("wall covered {0} = {1}", list[i], this.WallHardCovered(list[i])));
			}
			if (this.WallHardCovered(list[i]))
			{
				list.RemoveAt(i);
				i--;
			}
		}
	}

	// Token: 0x17000098 RID: 152
	// (get) Token: 0x060007C7 RID: 1991 RVA: 0x000272F4 File Offset: 0x000254F4
	public int SoftCoverageBin
	{
		get
		{
			return (int)this.softCoverage;
		}
	}

	// Token: 0x17000099 RID: 153
	// (get) Token: 0x060007C8 RID: 1992 RVA: 0x000272FC File Offset: 0x000254FC
	public int SoftWallAvailabilityBin
	{
		get
		{
			return ~(this.ConstBin ^ (int)this.softCoverage) | (int)this.softCoverage;
		}
	}

	// Token: 0x060007C9 RID: 1993 RVA: 0x00027313 File Offset: 0x00025513
	public bool WallSoftCovered(Direction dir)
	{
		return (this.softCoverage & (CellCoverage)(1 << (int)dir)) > CellCoverage.None;
	}

	// Token: 0x060007CA RID: 1994 RVA: 0x00027328 File Offset: 0x00025528
	public void FilterDirectionsThroughSoftCoverage(List<Direction> list)
	{
		for (int i = 0; i < list.Count; i++)
		{
			if (this.WallSoftCovered(list[i]))
			{
				list.RemoveAt(i);
				i--;
			}
		}
	}

	// Token: 0x1700009A RID: 154
	// (get) Token: 0x060007CB RID: 1995 RVA: 0x00027360 File Offset: 0x00025560
	public int fCost
	{
		get
		{
			return this.gCost + this.hCost;
		}
	}

	// Token: 0x1700009B RID: 155
	// (get) Token: 0x060007CC RID: 1996 RVA: 0x0002736F File Offset: 0x0002556F
	// (set) Token: 0x060007CD RID: 1997 RVA: 0x00027377 File Offset: 0x00025577
	public int HeapIndex
	{
		get
		{
			return this.heapIndex;
		}
		set
		{
			this.heapIndex = value;
		}
	}

	// Token: 0x060007CE RID: 1998 RVA: 0x00027380 File Offset: 0x00025580
	public int CompareTo(Cell tileToCompare)
	{
		int num = this.fCost.CompareTo(tileToCompare.fCost);
		if (num == 0)
		{
			num = this.hCost.CompareTo(tileToCompare.hCost);
		}
		return -num;
	}

	// Token: 0x060007CF RID: 1999 RVA: 0x000273B9 File Offset: 0x000255B9
	public bool Navigable(Direction dir, PathType pathType)
	{
		switch (pathType)
		{
		case PathType.Const:
			return this.ConstNavigable(dir);
		case PathType.Nav:
			return this.NavNavigable(dir);
		case PathType.Sound:
			return this.SoundNavigable(dir);
		default:
			return false;
		}
	}

	// Token: 0x060007D0 RID: 2000 RVA: 0x000273E8 File Offset: 0x000255E8
	public bool ConstNavigable(Direction dir)
	{
		return (this.ConstBin & 1 << (int)dir) == 0;
	}

	// Token: 0x060007D1 RID: 2001 RVA: 0x000273FA File Offset: 0x000255FA
	public bool NavNavigable(Direction dir)
	{
		return (this.NavBin & 1 << (int)dir) == 0;
	}

	// Token: 0x060007D2 RID: 2002 RVA: 0x0002740C File Offset: 0x0002560C
	public bool SoundNavigable(Direction dir)
	{
		return (this.SoundBin & 1 << (int)dir) == 0;
	}

	// Token: 0x1700009C RID: 156
	// (get) Token: 0x060007D3 RID: 2003 RVA: 0x00027420 File Offset: 0x00025620
	public int WallCount
	{
		get
		{
			int num = 0;
			for (int i = 0; i < 4; i++)
			{
				if ((this.ConstBin & 1 << i) > 0)
				{
					num++;
				}
			}
			return num;
		}
	}

	// Token: 0x060007D4 RID: 2004 RVA: 0x00027450 File Offset: 0x00025650
	public bool HasWallInDirection(Direction dir)
	{
		return (this.ConstBin & 1 << (int)dir) > 0;
	}

	// Token: 0x060007D5 RID: 2005 RVA: 0x00027462 File Offset: 0x00025662
	public void HardCover(CellCoverage coverage)
	{
		this.hardCoverage |= coverage;
		this.SoftCover(coverage);
	}

	// Token: 0x060007D6 RID: 2006 RVA: 0x00027479 File Offset: 0x00025679
	public void HardCoverEntirely()
	{
		this.HardCover(CellCoverage.North | CellCoverage.East | CellCoverage.South | CellCoverage.West | CellCoverage.Up | CellCoverage.Down | CellCoverage.Center);
	}

	// Token: 0x060007D7 RID: 2007 RVA: 0x00027483 File Offset: 0x00025683
	public void HardCoverWall(Direction dir, bool covered)
	{
		if ((this.hardCoverage & (CellCoverage)(1 << (int)dir)) == CellCoverage.None)
		{
			this.hardCoverage |= (CellCoverage)(1 << (int)dir);
		}
		this.SoftCoverWall(dir);
	}

	// Token: 0x060007D8 RID: 2008 RVA: 0x000274AE File Offset: 0x000256AE
	public void SoftCoverWall(Direction dir)
	{
		if ((this.softCoverage & (CellCoverage)(1 << (int)dir)) == CellCoverage.None)
		{
			this.softCoverage |= (CellCoverage)(1 << (int)dir);
		}
	}

	// Token: 0x060007D9 RID: 2009 RVA: 0x000274D2 File Offset: 0x000256D2
	public void SoftCover(CellCoverage coverage)
	{
		this.softCoverage |= coverage;
	}

	// Token: 0x060007DA RID: 2010 RVA: 0x000274E4 File Offset: 0x000256E4
	public void Block(Direction dir, bool block)
	{
		if (block)
		{
			this.blocks[(int)dir]++;
			if ((this.navBin & 1 << (int)dir) == 0)
			{
				this.navBin += 1 << (int)dir;
				this.room.ec.RecalculateNavigation();
				return;
			}
		}
		else
		{
			this.blocks[(int)dir]--;
			if (this.blocks[(int)dir] < 0)
			{
				this.blocks[(int)dir] = 0;
			}
			if ((this.navBin & 1 << (int)dir) > 0 && this.blocks[(int)dir] <= 0)
			{
				this.navBin -= 1 << (int)dir;
				this.room.ec.RecalculateNavigation();
			}
		}
	}

	// Token: 0x060007DB RID: 2011 RVA: 0x000275A0 File Offset: 0x000257A0
	public void Mute(Direction dir, bool block)
	{
		if (block)
		{
			this.mutes[(int)dir]++;
			if ((this.soundBin & 1 << (int)dir) == 0)
			{
				this.soundBin += 1 << (int)dir;
				GameCamera.RecalculateDijkstraMap();
				return;
			}
		}
		else
		{
			this.mutes[(int)dir]--;
			if (this.mutes[(int)dir] < 0)
			{
				this.mutes[(int)dir] = 0;
			}
			if ((this.soundBin & 1 << (int)dir) > 0 && this.mutes[(int)dir] <= 0)
			{
				this.soundBin -= 1 << (int)dir;
				GameCamera.RecalculateDijkstraMap();
			}
		}
	}

	// Token: 0x060007DC RID: 2012 RVA: 0x00027644 File Offset: 0x00025844
	public void SilentBlock(Direction dir, bool block)
	{
		if (block)
		{
			if ((this.navBin & 1 << (int)dir) == 0)
			{
				this.navBin += 1 << (int)dir;
				return;
			}
		}
		else if ((this.navBin & 1 << (int)dir) > 0)
		{
			this.navBin -= 1 << (int)dir;
		}
	}

	// Token: 0x060007DD RID: 2013 RVA: 0x0002769C File Offset: 0x0002589C
	public void RefreshNavBin()
	{
		for (int i = 0; i < 4; i++)
		{
			if (this.blocks[i] > 0)
			{
				this.navBin |= 1 << i;
			}
		}
	}

	// Token: 0x1700009D RID: 157
	// (get) Token: 0x060007DE RID: 2014 RVA: 0x000276D4 File Offset: 0x000258D4
	public bool HasFreeWall
	{
		get
		{
			for (int i = 0; i < 4; i++)
			{
				if ((this.SoftWallAvailabilityBin & 1 << i) == 0)
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x1700009E RID: 158
	// (get) Token: 0x060007DF RID: 2015 RVA: 0x00027700 File Offset: 0x00025900
	public bool HasHardFreeWall
	{
		get
		{
			for (int i = 0; i < 4; i++)
			{
				if ((this.HardWallAvailabilityBin & 1 << i) == 0)
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x060007E0 RID: 2016 RVA: 0x0002772B File Offset: 0x0002592B
	public void SetSilence(bool value)
	{
		if (value)
		{
			this.silences++;
			this.silent = true;
			return;
		}
		this.silences--;
		if (this.silences <= 0)
		{
			this.silent = false;
		}
	}

	// Token: 0x1700009F RID: 159
	// (get) Token: 0x060007E1 RID: 2017 RVA: 0x00027764 File Offset: 0x00025964
	public bool Silent
	{
		get
		{
			return this.silent;
		}
	}

	// Token: 0x060007E2 RID: 2018 RVA: 0x0002776C File Offset: 0x0002596C
	public void SetLight(bool on)
	{
		if (this.hasLight)
		{
			this.lightOn = on;
			this.room.ec.UpdateLightingAtCell(this);
		}
	}

	// Token: 0x060007E3 RID: 2019 RVA: 0x00027790 File Offset: 0x00025990
	public void SetPower(bool power)
	{
		if (!power)
		{
			this.lightPowerRemovals++;
		}
		else
		{
			this.lightPowerRemovals--;
			if (this.lightPowerRemovals <= 0)
			{
				this.lightPowerRemovals = 0;
			}
		}
		this.room.ec.UpdateLightingAtCell(this);
	}

	// Token: 0x060007E4 RID: 2020 RVA: 0x000277DF File Offset: 0x000259DF
	public void Flicker()
	{
		this.SetPower(this.lightPowerFlickered);
		this.lightPowerFlickered = !this.lightPowerFlickered;
	}

	// Token: 0x060007E5 RID: 2021 RVA: 0x000277FC File Offset: 0x000259FC
	public void FixFlicker()
	{
		if (this.lightPowerFlickered)
		{
			this.SetPower(true);
		}
	}

	// Token: 0x170000A0 RID: 160
	// (get) Token: 0x060007E6 RID: 2022 RVA: 0x0002780D File Offset: 0x00025A0D
	public Color CurrentColor
	{
		get
		{
			if (this.lightOn && this.lightPowerRemovals <= 0)
			{
				return this.lightColor;
			}
			return Color.black;
		}
	}

	// Token: 0x170000A1 RID: 161
	// (get) Token: 0x060007E7 RID: 2023 RVA: 0x0002782C File Offset: 0x00025A2C
	public bool HasObjectBase
	{
		get
		{
			return this.objectBase != null;
		}
	}

	// Token: 0x170000A2 RID: 162
	// (get) Token: 0x060007E8 RID: 2024 RVA: 0x0002783A File Offset: 0x00025A3A
	public Vector3 FloorWorldPosition
	{
		get
		{
			return new Vector3((float)this.position.x * 10f + 5f, 0f, (float)this.position.z * 10f + 5f);
		}
	}

	// Token: 0x170000A3 RID: 163
	// (get) Token: 0x060007E9 RID: 2025 RVA: 0x00027876 File Offset: 0x00025A76
	public Vector3 CenterWorldPosition
	{
		get
		{
			return new Vector3((float)this.position.x * 10f + 5f, 5f, (float)this.position.z * 10f + 5f);
		}
	}

	// Token: 0x170000A4 RID: 164
	// (get) Token: 0x060007EA RID: 2026 RVA: 0x000278B2 File Offset: 0x00025AB2
	public Transform TileTransform
	{
		get
		{
			if (this.tile == null)
			{
				return null;
			}
			return this.tile.transform;
		}
	}

	// Token: 0x170000A5 RID: 165
	// (get) Token: 0x060007EB RID: 2027 RVA: 0x000278CF File Offset: 0x00025ACF
	// (set) Token: 0x060007EC RID: 2028 RVA: 0x000278D7 File Offset: 0x00025AD7
	public Tile Tile
	{
		get
		{
			return this.tile;
		}
		set
		{
			this.tile = value;
		}
	}

	// Token: 0x060007ED RID: 2029 RVA: 0x000278E0 File Offset: 0x00025AE0
	public int Mutes(Direction direction)
	{
		return this.mutes[(int)direction];
	}

	// Token: 0x170000A6 RID: 166
	// (get) Token: 0x060007EE RID: 2030 RVA: 0x000278EA File Offset: 0x00025AEA
	public bool Null
	{
		get
		{
			return !this.initalized;
		}
	}

	// Token: 0x170000A7 RID: 167
	// (get) Token: 0x060007EF RID: 2031 RVA: 0x000278F5 File Offset: 0x00025AF5
	public bool Hidden
	{
		get
		{
			return this.tileHidden;
		}
	}

	// Token: 0x170000A8 RID: 168
	// (get) Token: 0x060007F0 RID: 2032 RVA: 0x00027900 File Offset: 0x00025B00
	// (set) Token: 0x060007F1 RID: 2033 RVA: 0x00027958 File Offset: 0x00025B58
	public Transform ObjectBase
	{
		get
		{
			if (this.objectBase == null)
			{
				this.objectBase = new GameObject().transform;
				this.objectBase.name = "ObjectBase";
				this.objectBase.transform.SetParent(this.TileTransform, false);
			}
			return this.objectBase;
		}
		set
		{
			this.objectBase = value;
		}
	}

	// Token: 0x170000A9 RID: 169
	// (get) Token: 0x060007F2 RID: 2034 RVA: 0x00027961 File Offset: 0x00025B61
	public bool TileLoaded
	{
		get
		{
			return this.tileLoaded;
		}
	}

	// Token: 0x060007F3 RID: 2035 RVA: 0x00027969 File Offset: 0x00025B69
	public void SetChunk(Chunk chunk)
	{
		this.chunk = chunk;
		this.hasChunk = true;
	}

	// Token: 0x060007F4 RID: 2036 RVA: 0x00027979 File Offset: 0x00025B79
	public void RemoveChunk()
	{
		this.hasChunk = false;
	}

	// Token: 0x170000AA RID: 170
	// (get) Token: 0x060007F5 RID: 2037 RVA: 0x00027982 File Offset: 0x00025B82
	public bool HasChunk
	{
		get
		{
			return this.hasChunk;
		}
	}

	// Token: 0x170000AB RID: 171
	// (get) Token: 0x060007F6 RID: 2038 RVA: 0x0002798A File Offset: 0x00025B8A
	public Chunk Chunk
	{
		get
		{
			return this.chunk;
		}
	}

	// Token: 0x0400085E RID: 2142
	private const CellCoverage fullCoverage = CellCoverage.North | CellCoverage.East | CellCoverage.South | CellCoverage.West | CellCoverage.Up | CellCoverage.Down | CellCoverage.Center;

	// Token: 0x0400085F RID: 2143
	public RoomController room;

	// Token: 0x04000860 RID: 2144
	public IntVector2 position;

	// Token: 0x04000861 RID: 2145
	public Color lightColor;

	// Token: 0x04000862 RID: 2146
	public int label;

	// Token: 0x04000863 RID: 2147
	public int lightStrength;

	// Token: 0x04000864 RID: 2148
	private int lightPowerRemovals;

	// Token: 0x04000865 RID: 2149
	private int silences;

	// Token: 0x04000866 RID: 2150
	public bool labeled;

	// Token: 0x04000867 RID: 2151
	public bool open;

	// Token: 0x04000868 RID: 2152
	public bool locked;

	// Token: 0x04000869 RID: 2153
	public bool hasLight;

	// Token: 0x0400086A RID: 2154
	public bool lightOn;

	// Token: 0x0400086B RID: 2155
	public bool lightHasPower = true;

	// Token: 0x0400086C RID: 2156
	public bool permanentLight;

	// Token: 0x0400086D RID: 2157
	public bool excludeFromOpen;

	// Token: 0x0400086E RID: 2158
	public bool offLimits;

	// Token: 0x0400086F RID: 2159
	public bool hideFromMap;

	// Token: 0x04000870 RID: 2160
	private bool initalized;

	// Token: 0x04000871 RID: 2161
	private bool tileLoaded;

	// Token: 0x04000872 RID: 2162
	private bool tileHidden;

	// Token: 0x04000873 RID: 2163
	private bool silent;

	// Token: 0x04000874 RID: 2164
	private bool lightPowerFlickered;

	// Token: 0x04000875 RID: 2165
	private CellCoverage hardCoverage;

	// Token: 0x04000876 RID: 2166
	private CellCoverage softCoverage;

	// Token: 0x04000877 RID: 2167
	private Tile tile;

	// Token: 0x04000878 RID: 2168
	private Transform objectBase;

	// Token: 0x04000879 RID: 2169
	public List<Renderer> renderers = new List<Renderer>();

	// Token: 0x0400087A RID: 2170
	public bool doorHere;

	// Token: 0x0400087B RID: 2171
	public List<Direction> doorDirs;

	// Token: 0x0400087C RID: 2172
	public List<Door> doors;

	// Token: 0x0400087D RID: 2173
	public List<Direction> doorDirsSpace;

	// Token: 0x0400087E RID: 2174
	private int constBin;

	// Token: 0x0400087F RID: 2175
	private int navBin;

	// Token: 0x04000880 RID: 2176
	private int soundBin;

	// Token: 0x04000881 RID: 2177
	private int heapIndex;

	// Token: 0x04000882 RID: 2178
	private int[] blocks = new int[4];

	// Token: 0x04000883 RID: 2179
	private int[] mutes = new int[4];

	// Token: 0x04000884 RID: 2180
	public TileShape shape;

	// Token: 0x04000885 RID: 2181
	private static int[] _potentialDirs = new int[4];

	// Token: 0x04000886 RID: 2182
	private Chunk chunk;

	// Token: 0x04000887 RID: 2183
	private bool hasChunk;

	// Token: 0x04000888 RID: 2184
	public Cell parent;

	// Token: 0x04000889 RID: 2185
	private List<Direction> _openDirs = new List<Direction>();

	// Token: 0x0400088A RID: 2186
	public List<Cell> openTiles = new List<Cell>();

	// Token: 0x0400088B RID: 2187
	public List<Cell> lightAffectingCells = new List<Cell>();

	// Token: 0x0400088C RID: 2188
	public OpenTileGroup openTileGroup;

	// Token: 0x0400088D RID: 2189
	public int gCost;

	// Token: 0x0400088E RID: 2190
	public int hCost;
}
