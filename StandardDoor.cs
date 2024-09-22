using System;
using UnityEngine;

// Token: 0x020001BE RID: 446
public class StandardDoor : Door, IClickable<int>, IItemAcceptor
{
	// Token: 0x1400000A RID: 10
	// (add) Token: 0x06000A0E RID: 2574 RVA: 0x00035AFC File Offset: 0x00033CFC
	// (remove) Token: 0x06000A0F RID: 2575 RVA: 0x00035B34 File Offset: 0x00033D34
	public event StandardDoor.OnPlayerOpenHandler OnPlayerOpen;

	// Token: 0x06000A10 RID: 2576 RVA: 0x00035B6C File Offset: 0x00033D6C
	private void Start()
	{
		this.bg[0] = base.aTile.room.wallTex;
		this.bg[1] = base.bTile.room.wallTex;
		this.overlayShut[1] = base.aTile.room.doorMats.shut;
		this.overlayShut[0] = base.bTile.room.doorMats.shut;
		this.overlayOpen[1] = base.aTile.room.doorMats.open;
		this.overlayOpen[0] = base.bTile.room.doorMats.open;
		this.UpdateTextures();
		this.startingLayer = this.doors[0].gameObject.layer;
		this.aMapTile = this.ec.map.AddExtraTile(base.aTile.position);
		this.aMapTile.SpriteRenderer.sprite = this.mapUnlockedSprite;
		this.aMapTile.SpriteRenderer.color = base.aTile.room.color;
		this.aMapTile.transform.rotation = this.direction.ToUiRotation();
		this.bMapTile = this.ec.map.AddExtraTile(base.bTile.position);
		this.bMapTile.SpriteRenderer.sprite = this.mapUnlockedSprite;
		this.bMapTile.transform.rotation = this.direction.GetOpposite().ToUiRotation();
		this.bMapTile.SpriteRenderer.color = base.bTile.room.color;
	}

	// Token: 0x06000A11 RID: 2577 RVA: 0x00035D24 File Offset: 0x00033F24
	public void Clicked(int player)
	{
		bool open = this.open;
		this.OpenTimed(this.defaultTime, this.makesNoise);
		if (this.locked)
		{
			this.audMan.PlaySingle(this.audDoorLocked);
			return;
		}
		if (!open)
		{
			StandardDoor.OnPlayerOpenHandler onPlayerOpen = this.OnPlayerOpen;
			if (onPlayerOpen == null)
			{
				return;
			}
			onPlayerOpen();
		}
	}

	// Token: 0x06000A12 RID: 2578 RVA: 0x00035D77 File Offset: 0x00033F77
	public void ClickableSighted(int player)
	{
	}

	// Token: 0x06000A13 RID: 2579 RVA: 0x00035D79 File Offset: 0x00033F79
	public void ClickableUnsighted(int player)
	{
	}

	// Token: 0x06000A14 RID: 2580 RVA: 0x00035D7B File Offset: 0x00033F7B
	public bool ClickableHidden()
	{
		return false;
	}

	// Token: 0x06000A15 RID: 2581 RVA: 0x00035D7E File Offset: 0x00033F7E
	public bool ClickableRequiresNormalHeight()
	{
		return false;
	}

	// Token: 0x06000A16 RID: 2582 RVA: 0x00035D84 File Offset: 0x00033F84
	public void UpdateTextures()
	{
		for (int i = 0; i < this.doors.Length; i++)
		{
			MaterialModifier.ChangeHole(this.doors[i], this.mask[i], this.overlayShut[i]);
			MaterialModifier.SetBase(this.doors[i], this.bg[i]);
		}
	}

	// Token: 0x06000A17 RID: 2583 RVA: 0x00035DD8 File Offset: 0x00033FD8
	public override void OpenTimed(float time, bool makeNoise)
	{
		if (!this.locked)
		{
			for (int i = 0; i < this.doors.Length; i++)
			{
				this.colliders[i].enabled = false;
				MaterialModifier.ChangeOverlay(this.doors[i], this.overlayOpen[i]);
			}
			MeshRenderer[] array = this.doors;
			for (int j = 0; j < array.Length; j++)
			{
				array[j].gameObject.layer = 2;
			}
			if (!this.open && this.makesNoise)
			{
				this.audMan.PlaySingle(this.audDoorOpen);
			}
		}
		base.OpenTimed(time, makeNoise);
	}

	// Token: 0x06000A18 RID: 2584 RVA: 0x00035E6F File Offset: 0x0003406F
	public void OpenTimedWithKey(float time, bool makeNoise)
	{
		if (this.locked)
		{
			this.Unlock();
			this.OpenTimed(time, makeNoise);
			return;
		}
		this.OpenTimed(time, makeNoise);
	}

	// Token: 0x06000A19 RID: 2585 RVA: 0x00035E90 File Offset: 0x00034090
	public override void Shut()
	{
		for (int i = 0; i < this.doors.Length; i++)
		{
			this.colliders[i].enabled = true;
			MaterialModifier.ChangeOverlay(this.doors[i], this.overlayShut[i]);
		}
		MeshRenderer[] array = this.doors;
		for (int j = 0; j < array.Length; j++)
		{
			array[j].gameObject.layer = this.startingLayer;
		}
		if (this.open && this.makesNoise)
		{
			this.audMan.PlaySingle(this.audDoorShut);
		}
		base.Shut();
	}

	// Token: 0x06000A1A RID: 2586 RVA: 0x00035F24 File Offset: 0x00034124
	public override void Lock(bool cancelTimer)
	{
		if (!this.locked)
		{
			this.audMan.PlaySingle(this.audDoorLock);
		}
		this.aMapTile.SpriteRenderer.sprite = this.mapLockedSprite;
		this.bMapTile.SpriteRenderer.sprite = this.mapLockedSprite;
		base.Lock(cancelTimer);
	}

	// Token: 0x06000A1B RID: 2587 RVA: 0x00035F80 File Offset: 0x00034180
	public override void Unlock()
	{
		if (this.locked)
		{
			this.audMan.PlaySingle(this.audDoorUnlock);
		}
		this.aMapTile.SpriteRenderer.sprite = this.mapUnlockedSprite;
		this.bMapTile.SpriteRenderer.sprite = this.mapUnlockedSprite;
		base.Unlock();
	}

	// Token: 0x06000A1C RID: 2588 RVA: 0x00035FD8 File Offset: 0x000341D8
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("NPC") && other.isTrigger)
		{
			other.GetComponent<NPC>().DoorHit(this);
		}
		if (other.CompareTag("Player") && other.GetComponent<ActivityModifier>().ForceTrigger)
		{
			this.OpenTimed(this.defaultTime, false);
		}
	}

	// Token: 0x06000A1D RID: 2589 RVA: 0x0003602D File Offset: 0x0003422D
	public void InsertItem(PlayerManager player, EnvironmentController ec)
	{
		this.Unlock();
	}

	// Token: 0x170000DA RID: 218
	// (get) Token: 0x06000A1E RID: 2590 RVA: 0x00036035 File Offset: 0x00034235
	public Vector3 CenteredPosition
	{
		get
		{
			return this.doors[0].transform.position;
		}
	}

	// Token: 0x06000A1F RID: 2591 RVA: 0x00036049 File Offset: 0x00034249
	public bool ItemFits(Items item)
	{
		return item == Items.DetentionKey && this.locked;
	}

	// Token: 0x06000A20 RID: 2592 RVA: 0x0003605A File Offset: 0x0003425A
	public void Knock()
	{
		this.audMan.PlaySingle(this.audDoorKnock);
	}

	// Token: 0x170000DB RID: 219
	// (get) Token: 0x06000A21 RID: 2593 RVA: 0x0003606D File Offset: 0x0003426D
	public float DefaultTime
	{
		get
		{
			return this.defaultTime;
		}
	}

	// Token: 0x04000B6E RID: 2926
	[SerializeField]
	private float defaultTime = 3f;

	// Token: 0x04000B6F RID: 2927
	public AudioManager audMan;

	// Token: 0x04000B70 RID: 2928
	public MeshRenderer[] doors;

	// Token: 0x04000B71 RID: 2929
	public MeshCollider[] colliders;

	// Token: 0x04000B72 RID: 2930
	private Texture2D[] bg = new Texture2D[2];

	// Token: 0x04000B73 RID: 2931
	public Material[] mask;

	// Token: 0x04000B74 RID: 2932
	public Material[] overlayShut;

	// Token: 0x04000B75 RID: 2933
	public Material[] overlayOpen;

	// Token: 0x04000B76 RID: 2934
	public SoundObject audDoorOpen;

	// Token: 0x04000B77 RID: 2935
	public SoundObject audDoorShut;

	// Token: 0x04000B78 RID: 2936
	public SoundObject audDoorLocked;

	// Token: 0x04000B79 RID: 2937
	public SoundObject audDoorLock;

	// Token: 0x04000B7A RID: 2938
	public SoundObject audDoorUnlock;

	// Token: 0x04000B7B RID: 2939
	public SoundObject audDoorKnock;

	// Token: 0x04000B7C RID: 2940
	[SerializeField]
	private Sprite mapUnlockedSprite;

	// Token: 0x04000B7D RID: 2941
	[SerializeField]
	private Sprite mapLockedSprite;

	// Token: 0x04000B7E RID: 2942
	private MapTile aMapTile;

	// Token: 0x04000B7F RID: 2943
	private MapTile bMapTile;

	// Token: 0x04000B80 RID: 2944
	private int startingLayer;

	// Token: 0x02000383 RID: 899
	// (Invoke) Token: 0x06001CAC RID: 7340
	public delegate void OnPlayerOpenHandler();
}
