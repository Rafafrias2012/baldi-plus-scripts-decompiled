using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001EB RID: 491
public class PlayerManager : MonoBehaviour
{
	// Token: 0x06000B22 RID: 2850 RVA: 0x0003AD85 File Offset: 0x00038F85
	private void Start()
	{
		this.dijkstraMap = new DijkstraMap(this.ec, PathType.Nav, new Transform[]
		{
			base.transform
		});
		this.dijkstraMap.Activate();
		this.dijkstraMap.QueueUpdate();
	}

	// Token: 0x06000B23 RID: 2851 RVA: 0x0003ADC0 File Offset: 0x00038FC0
	private void Update()
	{
		if (this.guiltTime > 0f)
		{
			this.guiltTime -= Time.deltaTime * this.playerTimeScale;
		}
		else if (this.disobeying)
		{
			this.disobeying = false;
			this.guiltTime = 0f;
		}
		if (this.ec.CellFromPosition(base.transform.position).room != this.currentRoom)
		{
			if (this.currentRoom != null)
			{
				this.currentRoom.functions.OnPlayerExit(this);
			}
			this.currentRoom = this.ec.CellFromPosition(base.transform.position).room;
			if (this.currentRoom != null)
			{
				this.currentRoom.functions.OnPlayerEnter(this);
			}
		}
		if (this.currentRoom != null)
		{
			this.currentRoom.functions.OnPlayerStay(this);
		}
		if (this.ec.ContainsCoordinates(IntVector2.GetGridPosition(base.transform.position)) && this.ec.LightLevel(base.transform.position) <= this.maxHideableLightLevel)
		{
			if (!this.hiddenByLight)
			{
				this.hiddenByLight = true;
				this.SetInvisible(true);
				return;
			}
		}
		else if (this.hiddenByLight)
		{
			this.hiddenByLight = false;
			this.SetInvisible(false);
		}
	}

	// Token: 0x06000B24 RID: 2852 RVA: 0x0003AF1D File Offset: 0x0003911D
	public void RuleBreak(string rule, float linger, float sensitivity)
	{
		if (linger >= this.guiltTime)
		{
			this.ruleBreak = rule;
			this.disobeying = true;
			this.guiltTime = linger;
			this.guiltSensitivity = sensitivity;
		}
	}

	// Token: 0x06000B25 RID: 2853 RVA: 0x0003AF44 File Offset: 0x00039144
	public void RuleBreak(string rule, float linger)
	{
		this.RuleBreak(rule, linger, 1f);
	}

	// Token: 0x06000B26 RID: 2854 RVA: 0x0003AF53 File Offset: 0x00039153
	public void ClearGuilt()
	{
		this.ruleBreak = "";
		this.disobeying = false;
		this.guiltTime = 0f;
	}

	// Token: 0x06000B27 RID: 2855 RVA: 0x0003AF74 File Offset: 0x00039174
	public void SetInvisible(bool value)
	{
		if (value)
		{
			this.invisibles++;
			this.invisible = value;
			Singleton<CoreGameManager>.Instance.GetHud(this.playerNumber).Darken(true);
			return;
		}
		this.invisibles--;
		if (this.invisibles <= 0)
		{
			if (this.invisibles < 0)
			{
				this.invisibles = 0;
			}
			this.invisible = value;
			Singleton<CoreGameManager>.Instance.GetHud(this.playerNumber).Darken(false);
		}
	}

	// Token: 0x06000B28 RID: 2856 RVA: 0x0003AFF4 File Offset: 0x000391F4
	public void Teleport(Vector3 pos)
	{
		this.plm.Entity.Teleport(pos);
		PlayerManager.PlayerTeleportedFunction playerTeleportedFunction = this.onPlayerTeleport;
		if (playerTeleportedFunction == null)
		{
			return;
		}
		playerTeleportedFunction(this, pos, pos - base.transform.position);
	}

	// Token: 0x06000B29 RID: 2857 RVA: 0x0003B02A File Offset: 0x0003922A
	public void AddTimeScale(TimeScaleModifier scale)
	{
		if (!this.timeScaleModifiers.Contains(scale))
		{
			this.timeScaleModifiers.Add(scale);
		}
	}

	// Token: 0x06000B2A RID: 2858 RVA: 0x0003B046 File Offset: 0x00039246
	public void RemoveTimeScale(TimeScaleModifier scale)
	{
		this.timeScaleModifiers.Remove(scale);
	}

	// Token: 0x170000E8 RID: 232
	// (get) Token: 0x06000B2B RID: 2859 RVA: 0x0003B058 File Offset: 0x00039258
	public float PlayerTimeScale
	{
		get
		{
			float num = 1f;
			for (int i = 0; i < this.timeScaleModifiers.Count; i++)
			{
				num *= this.timeScaleModifiers[i].playerTimeScale;
			}
			return num * this.ec.PlayerTimeScale;
		}
	}

	// Token: 0x170000E9 RID: 233
	// (get) Token: 0x06000B2C RID: 2860 RVA: 0x0003B0A4 File Offset: 0x000392A4
	public float GuiltySensitivity
	{
		get
		{
			return this.guiltSensitivity;
		}
	}

	// Token: 0x170000EA RID: 234
	// (get) Token: 0x06000B2D RID: 2861 RVA: 0x0003B0AC File Offset: 0x000392AC
	public bool Disobeying
	{
		get
		{
			return this.disobeying;
		}
	}

	// Token: 0x170000EB RID: 235
	// (get) Token: 0x06000B2E RID: 2862 RVA: 0x0003B0B4 File Offset: 0x000392B4
	public ActivityModifier Am
	{
		get
		{
			return this.am;
		}
	}

	// Token: 0x06000B2F RID: 2863 RVA: 0x0003B0BC File Offset: 0x000392BC
	public void SetNametag(bool state)
	{
		if (state)
		{
			this.tagged = true;
			this.tags++;
			return;
		}
		this.tags--;
		if (this.tags <= 0)
		{
			this.tagged = false;
			if (this.tags < 0)
			{
				this.tags = 0;
			}
		}
	}

	// Token: 0x06000B30 RID: 2864 RVA: 0x0003B110 File Offset: 0x00039310
	public void Reverse()
	{
		this.reversed = !this.reversed;
	}

	// Token: 0x170000EC RID: 236
	// (get) Token: 0x06000B31 RID: 2865 RVA: 0x0003B121 File Offset: 0x00039321
	public DijkstraMap DijkstraMap
	{
		get
		{
			return this.dijkstraMap;
		}
	}

	// Token: 0x170000ED RID: 237
	// (get) Token: 0x06000B32 RID: 2866 RVA: 0x0003B129 File Offset: 0x00039329
	public bool Tagged
	{
		get
		{
			return this.tagged;
		}
	}

	// Token: 0x170000EE RID: 238
	// (get) Token: 0x06000B33 RID: 2867 RVA: 0x0003B131 File Offset: 0x00039331
	public bool Invisible
	{
		get
		{
			return this.invisible;
		}
	}

	// Token: 0x04000CB4 RID: 3252
	public PlayerManager.PlayerTeleportedFunction onPlayerTeleport;

	// Token: 0x04000CB5 RID: 3253
	public EnvironmentController ec;

	// Token: 0x04000CB6 RID: 3254
	public ItemManager itm;

	// Token: 0x04000CB7 RID: 3255
	public PlayerMovement plm;

	// Token: 0x04000CB8 RID: 3256
	public PlayerClick pc;

	// Token: 0x04000CB9 RID: 3257
	public Transform cameraBase;

	// Token: 0x04000CBA RID: 3258
	[SerializeField]
	private ActivityModifier am;

	// Token: 0x04000CBB RID: 3259
	public List<Jumprope> jumpropes = new List<Jumprope>();

	// Token: 0x04000CBC RID: 3260
	public List<CameraModifier> camMods = new List<CameraModifier>();

	// Token: 0x04000CBD RID: 3261
	private RoomController currentRoom;

	// Token: 0x04000CBE RID: 3262
	private DijkstraMap dijkstraMap;

	// Token: 0x04000CBF RID: 3263
	[SerializeField]
	private float maxHideableLightLevel = 0.3f;

	// Token: 0x04000CC0 RID: 3264
	private float playerTimeScale = 1f;

	// Token: 0x04000CC1 RID: 3265
	public static int playerCount;

	// Token: 0x04000CC2 RID: 3266
	private int tags;

	// Token: 0x04000CC3 RID: 3267
	private int invisibles;

	// Token: 0x04000CC4 RID: 3268
	public int playerNumber;

	// Token: 0x04000CC5 RID: 3269
	public string ruleBreak;

	// Token: 0x04000CC6 RID: 3270
	private List<TimeScaleModifier> timeScaleModifiers = new List<TimeScaleModifier>();

	// Token: 0x04000CC7 RID: 3271
	private float guiltTime;

	// Token: 0x04000CC8 RID: 3272
	private float guiltSensitivity;

	// Token: 0x04000CC9 RID: 3273
	public bool disobeying;

	// Token: 0x04000CCA RID: 3274
	public bool invincible;

	// Token: 0x04000CCB RID: 3275
	public bool tagged;

	// Token: 0x04000CCC RID: 3276
	public bool reversed;

	// Token: 0x04000CCD RID: 3277
	private bool invisible;

	// Token: 0x04000CCE RID: 3278
	private bool hiddenByLight;

	// Token: 0x02000391 RID: 913
	// (Invoke) Token: 0x06001CFE RID: 7422
	public delegate void PlayerTeleportedFunction(PlayerManager player, Vector3 pos, Vector3 positionDelta);
}
