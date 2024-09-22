using System;
using UnityEngine;

// Token: 0x020001BF RID: 447
public class SwingDoor : Door, IItemAcceptor
{
	// Token: 0x06000A23 RID: 2595 RVA: 0x00036094 File Offset: 0x00034294
	protected virtual void Start()
	{
		if (this.bOffset.x == 0 && this.bOffset.z == 0)
		{
			this.bOffset = this.direction.ToIntVector2();
		}
		this.bg[0] = base.aTile.room.wallTex;
		this.bg[1] = base.bTile.room.wallTex;
		this.UpdateTextures();
		this.aMapTile = this.ec.map.AddExtraTile(base.aTile.position);
		this.aMapTile.SpriteRenderer.sprite = this.mapUnlockedSprite;
		this.aMapTile.SpriteRenderer.color = base.aTile.room.color;
		this.aMapTile.transform.rotation = this.direction.ToUiRotation();
		this.bMapTile = this.ec.map.AddExtraTile(base.bTile.position);
		this.bMapTile.SpriteRenderer.sprite = this.mapUnlockedSprite;
		this.bMapTile.transform.rotation = this.direction.GetOpposite().ToUiRotation();
		this.bMapTile.SpriteRenderer.color = base.bTile.room.color;
	}

	// Token: 0x06000A24 RID: 2596 RVA: 0x000361EC File Offset: 0x000343EC
	public void UpdateTextures()
	{
		for (int i = 0; i < this.doors.Length; i++)
		{
			if (!this.locked)
			{
				MaterialModifier.ChangeHole(this.doors[i], this.mask[i], this.overlayShut[i]);
			}
			else
			{
				MaterialModifier.ChangeHole(this.doors[i], this.mask[i], this.overlayLocked[i]);
			}
			MaterialModifier.SetBase(this.doors[i], this.bg[i]);
		}
	}

	// Token: 0x06000A25 RID: 2597 RVA: 0x00036268 File Offset: 0x00034468
	private void OnTriggerStay(Collider other)
	{
		if ((other.tag == "Player" || other.tag == "NPC") && other.isTrigger && !this.locked)
		{
			if (other.tag == "Player")
			{
				this.OpenTimed(this.defaultTime, this.makesNoise);
				return;
			}
			if (other.isTrigger)
			{
				this.OpenTimed(this.defaultTime, false);
			}
		}
	}

	// Token: 0x06000A26 RID: 2598 RVA: 0x000362E4 File Offset: 0x000344E4
	public override void OpenTimed(float time, bool makeNoise)
	{
		if (!this.locked)
		{
			for (int i = 0; i < this.doors.Length; i++)
			{
				MaterialModifier.ChangeOverlay(this.doors[i], this.overlayOpen[i]);
			}
			if (!this.open && this.makesNoise)
			{
				this.audMan.PlaySingle(this.audDoorOpen);
			}
		}
		base.OpenTimed(time, makeNoise);
	}

	// Token: 0x06000A27 RID: 2599 RVA: 0x0003634C File Offset: 0x0003454C
	public override void Shut()
	{
		for (int i = 0; i < this.doors.Length; i++)
		{
			MaterialModifier.ChangeOverlay(this.doors[i], this.overlayShut[i]);
		}
		if (this.open && this.audDoorShut != null)
		{
			this.audMan.PlaySingle(this.audDoorShut);
		}
		base.Shut();
	}

	// Token: 0x06000A28 RID: 2600 RVA: 0x000363B0 File Offset: 0x000345B0
	public override void Lock(bool cancelTimer)
	{
		base.Lock(cancelTimer);
		this.Shut();
		for (int i = 0; i < this.doors.Length; i++)
		{
			this.colliders[i].enabled = true;
			MaterialModifier.ChangeOverlay(this.doors[i], this.overlayLocked[i]);
		}
		this.aMapTile.SpriteRenderer.sprite = this.mapLockedSprite;
		this.bMapTile.SpriteRenderer.sprite = this.mapLockedSprite;
	}

	// Token: 0x06000A29 RID: 2601 RVA: 0x0003642C File Offset: 0x0003462C
	public override void Unlock()
	{
		base.Unlock();
		if (!this.open)
		{
			for (int i = 0; i < this.doors.Length; i++)
			{
				this.colliders[i].enabled = false;
				MaterialModifier.ChangeOverlay(this.doors[i], this.overlayShut[i]);
			}
		}
		this.aMapTile.SpriteRenderer.sprite = this.mapUnlockedSprite;
		this.bMapTile.SpriteRenderer.sprite = this.mapUnlockedSprite;
	}

	// Token: 0x06000A2A RID: 2602 RVA: 0x000364A9 File Offset: 0x000346A9
	public void InsertItem(PlayerManager player, EnvironmentController ec)
	{
		this.audMan.PlaySingle(this.audLockItem);
		this.LockTimed(this.lockTime);
	}

	// Token: 0x06000A2B RID: 2603 RVA: 0x000364C8 File Offset: 0x000346C8
	public bool ItemFits(Items item)
	{
		return item == Items.DoorLock && !this.locked && this.acceptsLockItem;
	}

	// Token: 0x04000B81 RID: 2945
	[SerializeField]
	private float defaultTime = 2f;

	// Token: 0x04000B82 RID: 2946
	public AudioManager audMan;

	// Token: 0x04000B83 RID: 2947
	public MeshRenderer[] doors;

	// Token: 0x04000B84 RID: 2948
	public MeshCollider[] colliders;

	// Token: 0x04000B85 RID: 2949
	public Material[] mask;

	// Token: 0x04000B86 RID: 2950
	public Material[] overlayShut;

	// Token: 0x04000B87 RID: 2951
	public Material[] overlayOpen;

	// Token: 0x04000B88 RID: 2952
	public Material[] overlayLocked;

	// Token: 0x04000B89 RID: 2953
	private Texture[] bg = new Texture[2];

	// Token: 0x04000B8A RID: 2954
	public SoundObject audDoorOpen;

	// Token: 0x04000B8B RID: 2955
	public SoundObject audDoorShut;

	// Token: 0x04000B8C RID: 2956
	public SoundObject audLockItem;

	// Token: 0x04000B8D RID: 2957
	[SerializeField]
	protected Sprite mapUnlockedSprite;

	// Token: 0x04000B8E RID: 2958
	[SerializeField]
	protected Sprite mapLockedSprite;

	// Token: 0x04000B8F RID: 2959
	protected MapTile aMapTile;

	// Token: 0x04000B90 RID: 2960
	protected MapTile bMapTile;

	// Token: 0x04000B91 RID: 2961
	[SerializeField]
	private float lockTime = 30f;

	// Token: 0x04000B92 RID: 2962
	[SerializeField]
	private bool acceptsLockItem = true;
}
