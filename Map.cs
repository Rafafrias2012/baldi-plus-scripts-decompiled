using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000210 RID: 528
public class Map : MonoBehaviour
{
	// Token: 0x06000BB4 RID: 2996 RVA: 0x0003D3A0 File Offset: 0x0003B5A0
	private void Update()
	{
		Shader.SetGlobalFloat("_MapUnscaledTime", Time.unscaledTime);
		for (int i = 0; i < this.arrowTargets.Count; i++)
		{
			if (this.arrowTargets[i] != null)
			{
				this._arrowPosition.x = this.arrowTargets[i].position.x / 10f - 0.5f;
				this._arrowPosition.y = this.arrowTargets[i].position.z / 10f - 0.5f;
				this._arrowPosition.z = -1f;
				this._arrowRotation.z = this.arrowTargets[i].rotation.eulerAngles.y * -1f;
				this.arrows[i].transform.position = this._arrowPosition;
				this.arrows[i].transform.localEulerAngles = this._arrowRotation;
			}
			else
			{
				this.arrowTargets.RemoveAt(i);
				Object.Destroy(this.arrows[i].gameObject);
				this.arrows.RemoveAt(i);
				i--;
			}
		}
		if (!this.advancedMode)
		{
			for (int j = 0; j < this.cams.Count; j++)
			{
				if (j >= this.targets.Count)
				{
					return;
				}
				this._position.x = this.targets[j].position.x / 10f - 0.5f;
				this._position.y = this.targets[j].position.z / 10f - 0.5f;
				this._position.z = -10f;
				this._rotation.z = this.targets[j].rotation.eulerAngles.y * -1f;
				foreach (MapIcon mapIcon in this.icons)
				{
					mapIcon.transform.localEulerAngles = this._rotation;
				}
				this.cams[j].transform.position = this._position;
				this.cams[j].transform.localEulerAngles = this._rotation;
				this.cams[j].orthographicSize = this.defaultZoom;
				this._gridPosition = IntVector2.GetGridPosition(this.targets[j].position);
				if (this.ContainsCoordinates(this._gridPosition) && !this.ec.cells[this._gridPosition.x, this._gridPosition.z].Null && !this.tiles[this._gridPosition.x, this._gridPosition.z].Found && !this.ec.cells[this._gridPosition.x, this._gridPosition.z].hideFromMap && this.ec.cells[this._gridPosition.x, this._gridPosition.z] != null)
				{
					if (this.ec.cells[this._gridPosition.x, this._gridPosition.z].open)
					{
						using (List<Cell>.Enumerator enumerator2 = this.ec.cells[this._gridPosition.x, this._gridPosition.z].openTileGroup.cells.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								Cell cell = enumerator2.Current;
								if (!cell.hideFromMap)
								{
									this.Find(cell.position.x, cell.position.z, cell.ConstBin, cell.room);
								}
							}
							goto IL_497;
						}
					}
					this.Find(this._gridPosition.x, this._gridPosition.z, this.ec.cells[this._gridPosition.x, this._gridPosition.z].ConstBin, this.ec.cells[this._gridPosition.x, this._gridPosition.z].room);
				}
				IL_497:;
			}
		}
		else
		{
			Singleton<InputManager>.Instance.GetAnalogInput(this.mapAnalogData, out this._absoluteVector, out this._deltaVector, 0.1f);
			foreach (MapIcon mapIcon2 in this.icons)
			{
				mapIcon2.transform.rotation = Quaternion.identity;
			}
			this._position.x = this._position.x + (this._absoluteVector.x * Time.unscaledDeltaTime * this.scrollSpeed + this._deltaVector.x);
			this._position.y = this._position.y + (this._absoluteVector.y * Time.unscaledDeltaTime * this.scrollSpeed + this._deltaVector.y);
			if (this._position.x < 0f)
			{
				this._position.x = 0f;
			}
			if (this._position.y < 0f)
			{
				this._position.y = 0f;
			}
			if (this._position.x > (float)this.size.x)
			{
				this._position.x = (float)this.size.x;
			}
			if (this._position.y > (float)this.size.z)
			{
				this._position.y = (float)this.size.z;
			}
			if (Singleton<InputManager>.Instance.GetDigitalInput("MapZoomPos", false))
			{
				this.zoom -= this.zoomSpeed * Time.unscaledDeltaTime;
				if (this.zoom < this.zoomMin)
				{
					this.zoom = this.zoomMin;
				}
			}
			else if (Singleton<InputManager>.Instance.GetDigitalInput("MapZoomNeg", false))
			{
				this.zoom += this.zoomSpeed * Time.unscaledDeltaTime;
				if (this.zoom > this.zoomMax)
				{
					this.zoom = this.zoomMax;
				}
			}
			this._position.z = -10f;
			this._rotation.z = 0f;
			this.cams[0].orthographicSize = this.zoom;
			this.cams[0].transform.position = this._position;
			this.cams[0].transform.localEulerAngles = this._rotation;
		}
	}

	// Token: 0x06000BB5 RID: 2997 RVA: 0x0003DAF8 File Offset: 0x0003BCF8
	public void Find(int posX, int posZ, int bin, RoomController room)
	{
		if (this.tiles[posX, posZ] != null)
		{
			this.tiles[posX, posZ].SpriteRenderer.sprite = this.mapTileSprite[bin];
			this.tiles[posX, posZ].SpriteRenderer.color = room.color;
			if (room.mapMaterial != null)
			{
				this.tiles[posX, posZ].SpriteRenderer.sharedMaterial = room.mapMaterial;
			}
			this.tiles[posX, posZ].Reveal();
			this.foundTiles[posX, posZ] = true;
		}
	}

	// Token: 0x06000BB6 RID: 2998 RVA: 0x0003DBA8 File Offset: 0x0003BDA8
	public void CompleteMap()
	{
		for (int i = 0; i < this.size.x; i++)
		{
			for (int j = 0; j < this.size.z; j++)
			{
				if (!this.ec.cells[i, j].Null && !this.ec.cells[i, j].hideFromMap)
				{
					this.Find(i, j, this.ec.cells[i, j].ConstBin, this.ec.cells[i, j].room);
				}
			}
		}
		Singleton<CoreGameManager>.Instance.saveMapPurchased = false;
	}

	// Token: 0x170000F6 RID: 246
	// (set) Token: 0x06000BB7 RID: 2999 RVA: 0x0003DC5A File Offset: 0x0003BE5A
	public EnvironmentController Ec
	{
		set
		{
			this.ec = value;
		}
	}

	// Token: 0x06000BB8 RID: 3000 RVA: 0x0003DC63 File Offset: 0x0003BE63
	public MapTile AddExtraTile(IntVector2 position)
	{
		MapTile mapTile = Object.Instantiate<MapTile>(this.mapExtraTilePref, this.tiles[position.x, position.z].transform);
		mapTile.gameObject.SetActive(true);
		return mapTile;
	}

	// Token: 0x06000BB9 RID: 3001 RVA: 0x0003DC98 File Offset: 0x0003BE98
	public void AddArrow(Transform target, Color color)
	{
		this.arrows.Add(Object.Instantiate<MapIcon>(this.arrowPre, base.transform));
		this.arrows[this.arrows.Count - 1].spriteRenderer.color = color;
		this.arrowTargets.Add(target);
	}

	// Token: 0x06000BBA RID: 3002 RVA: 0x0003DCF0 File Offset: 0x0003BEF0
	public MapIcon AddIcon(MapIcon iconPre, Transform target, Color color)
	{
		MapIcon mapIcon = Object.Instantiate<MapIcon>(iconPre);
		mapIcon.target = target;
		mapIcon.UpdatePosition(this);
		mapIcon.spriteRenderer.color = color;
		this.icons.Add(mapIcon);
		return mapIcon;
	}

	// Token: 0x06000BBB RID: 3003 RVA: 0x0003DD2C File Offset: 0x0003BF2C
	public MapTile ClosestTileFromPosition(Vector3 position)
	{
		return this.tiles[Mathf.Max(Mathf.RoundToInt(position.x / 10f - 0.5f), 0), Mathf.Max(Mathf.RoundToInt(position.z / 10f - 0.5f), 0)];
	}

	// Token: 0x06000BBC RID: 3004 RVA: 0x0003DD80 File Offset: 0x0003BF80
	public void OpenMap()
	{
		this.advancedMode = true;
		this._position.x = this.targets[0].position.x / 10f - 0.5f;
		this._position.y = this.targets[0].position.z / 10f - 0.5f;
		this.padCanvas.gameObject.SetActive(true);
		this.bg.SetActive(true);
		Shader.EnableKeyword("_KEYMAPSHOWBACKGROUND");
		Singleton<GlobalCam>.Instance.FadeIn(UiTransition.Dither, 0.01666667f);
	}

	// Token: 0x06000BBD RID: 3005 RVA: 0x0003DE25 File Offset: 0x0003C025
	public void CloseMap()
	{
		this.padCanvas.gameObject.SetActive(false);
		this.advancedMode = false;
		this.bg.SetActive(false);
		Shader.DisableKeyword("_KEYMAPSHOWBACKGROUND");
	}

	// Token: 0x06000BBE RID: 3006 RVA: 0x0003DE55 File Offset: 0x0003C055
	public void TurnOn()
	{
		this.cover.SetActive(false);
	}

	// Token: 0x06000BBF RID: 3007 RVA: 0x0003DE63 File Offset: 0x0003C063
	public void TurnOff()
	{
		this.cover.SetActive(true);
	}

	// Token: 0x06000BC0 RID: 3008 RVA: 0x0003DE71 File Offset: 0x0003C071
	private bool ContainsCoordinates(IntVector2 pos)
	{
		return pos.x >= 0 && pos.x < this.size.x && pos.z >= 0 && pos.z < this.size.z;
	}

	// Token: 0x06000BC1 RID: 3009 RVA: 0x0003DEB0 File Offset: 0x0003C0B0
	public void Initialize(EnvironmentController ec, IntVector2 size)
	{
		this.tiles = new MapTile[size.x, size.z];
		for (int i = 0; i < size.x; i++)
		{
			for (int j = 0; j < size.z; j++)
			{
				this.tiles[i, j] = Object.Instantiate<MapTile>(this.mapTilePref, this.tilesObject.transform);
				this.tiles[i, j].transform.position = new Vector3((float)i, (float)j, 0f);
			}
		}
		this.foundTiles = new bool[size.x, size.z];
		this.size.x = size.x;
		this.size.z = size.z;
		this.ec = ec;
		Shader.DisableKeyword("_KEYMAPSHOWBACKGROUND");
	}

	// Token: 0x06000BC2 RID: 3010 RVA: 0x0003DF88 File Offset: 0x0003C188
	public void UpdateIcons()
	{
		foreach (MapIcon mapIcon in this.icons)
		{
			mapIcon.UpdatePosition(this);
		}
	}

	// Token: 0x04000E19 RID: 3609
	[SerializeField]
	private EnvironmentController ec;

	// Token: 0x04000E1A RID: 3610
	public List<Camera> cams = new List<Camera>();

	// Token: 0x04000E1B RID: 3611
	public List<Transform> targets = new List<Transform>();

	// Token: 0x04000E1C RID: 3612
	public List<Transform> arrowTargets = new List<Transform>();

	// Token: 0x04000E1D RID: 3613
	[SerializeField]
	private MapTile mapTilePref;

	// Token: 0x04000E1E RID: 3614
	[SerializeField]
	private MapTile mapExtraTilePref;

	// Token: 0x04000E1F RID: 3615
	public MapTile[,] tiles = new MapTile[0, 0];

	// Token: 0x04000E20 RID: 3616
	[SerializeField]
	private Sprite[] mapTileSprite = new Sprite[16];

	// Token: 0x04000E21 RID: 3617
	[SerializeField]
	private MapIcon arrowPre;

	// Token: 0x04000E22 RID: 3618
	private List<MapIcon> arrows = new List<MapIcon>();

	// Token: 0x04000E23 RID: 3619
	private List<MapIcon> icons = new List<MapIcon>();

	// Token: 0x04000E24 RID: 3620
	[SerializeField]
	private Canvas padCanvas;

	// Token: 0x04000E25 RID: 3621
	[SerializeField]
	private GameObject bg;

	// Token: 0x04000E26 RID: 3622
	[SerializeField]
	private GameObject cover;

	// Token: 0x04000E27 RID: 3623
	[SerializeField]
	private GameObject tilesObject;

	// Token: 0x04000E28 RID: 3624
	public AnalogInputData mapAnalogData;

	// Token: 0x04000E29 RID: 3625
	public bool[,] foundTiles = new bool[0, 0];

	// Token: 0x04000E2A RID: 3626
	private Vector3 _position;

	// Token: 0x04000E2B RID: 3627
	private Vector3 _rotation;

	// Token: 0x04000E2C RID: 3628
	private Vector3 _arrowPosition;

	// Token: 0x04000E2D RID: 3629
	private Vector3 _arrowRotation;

	// Token: 0x04000E2E RID: 3630
	private Vector2 _absoluteVector;

	// Token: 0x04000E2F RID: 3631
	private Vector2 _deltaVector;

	// Token: 0x04000E30 RID: 3632
	private IntVector2 _gridPosition;

	// Token: 0x04000E31 RID: 3633
	public IntVector2 size;

	// Token: 0x04000E32 RID: 3634
	[SerializeField]
	private float scrollSpeed = 0.25f;

	// Token: 0x04000E33 RID: 3635
	[SerializeField]
	private float zoomSpeed = 4f;

	// Token: 0x04000E34 RID: 3636
	[SerializeField]
	private float zoomMin = 5f;

	// Token: 0x04000E35 RID: 3637
	[SerializeField]
	private float zoomMax = 24f;

	// Token: 0x04000E36 RID: 3638
	[SerializeField]
	private float defaultZoom = 10f;

	// Token: 0x04000E37 RID: 3639
	private float zoom = 10f;

	// Token: 0x04000E38 RID: 3640
	private bool advancedMode;
}
