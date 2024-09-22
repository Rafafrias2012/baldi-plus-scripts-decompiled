using System;
using UnityEngine;

// Token: 0x020001C0 RID: 448
public class Window : Door
{
	// Token: 0x06000A2D RID: 2605 RVA: 0x00036514 File Offset: 0x00034714
	private void Start()
	{
		this.startingLayer = this.windows[0].gameObject.layer;
		EnvironmentController ec = this.ec;
		ec.tempOpenWindows = (EnvironmentController.TempObstacleManagement)Delegate.Combine(ec.tempOpenWindows, new EnvironmentController.TempObstacleManagement(this.TempOpen));
		EnvironmentController ec2 = this.ec;
		ec2.tempCloseWindows = (EnvironmentController.TempObstacleManagement)Delegate.Combine(ec2.tempCloseWindows, new EnvironmentController.TempObstacleManagement(this.TempClose));
	}

	// Token: 0x06000A2E RID: 2606 RVA: 0x00036588 File Offset: 0x00034788
	public void Initialize(EnvironmentController ec, IntVector2 pos, Direction dir, WindowObject wObject)
	{
		this.ec = ec;
		this.position = pos;
		base.transform.position = base.aTile.FloorWorldPosition;
		this.direction = dir;
		this.bOffset = dir.ToIntVector2();
		base.transform.rotation = dir.ToRotation();
		this.windowObject = wObject;
		this.UpdateTextures();
		if (this.openOnStart)
		{
			this.Open(true, false);
		}
		else
		{
			base.aTile.Mute(dir, true);
			base.bTile.Mute(dir.GetOpposite(), true);
			this.Shut();
		}
		this.aMapTile = ec.map.AddExtraTile(base.aTile.position);
		this.aMapTile.SpriteRenderer.sprite = this.mapClosedSprite;
		this.aMapTile.SpriteRenderer.color = base.aTile.room.color;
		this.aMapTile.transform.rotation = this.direction.ToUiRotation();
		this.bMapTile = ec.map.AddExtraTile(base.bTile.position);
		this.bMapTile.SpriteRenderer.sprite = this.mapClosedSprite;
		this.bMapTile.transform.rotation = this.direction.GetOpposite().ToUiRotation();
		this.bMapTile.SpriteRenderer.color = base.bTile.room.color;
	}

	// Token: 0x06000A2F RID: 2607 RVA: 0x00036704 File Offset: 0x00034904
	public void UpdateTextures()
	{
		for (int i = 0; i < this.windows.Length; i++)
		{
			MaterialModifier.ChangeHole(this.windows[i], this.windowObject.mask, this.windowObject.overlay[i]);
			if (i == 0)
			{
				MaterialModifier.SetBase(this.windows[i], base.aTile.room.wallTex);
			}
			else
			{
				MaterialModifier.SetBase(this.windows[i], base.bTile.room.wallTex);
			}
		}
	}

	// Token: 0x06000A30 RID: 2608 RVA: 0x00036788 File Offset: 0x00034988
	public override void Shut()
	{
		base.Shut();
		for (int i = 0; i < this.windows.Length; i++)
		{
			MaterialModifier.ChangeOverlay(this.windows[i], this.windowObject.overlay[i]);
		}
		MeshCollider[] array = this.colliders;
		for (int j = 0; j < array.Length; j++)
		{
			array[j].enabled = true;
		}
	}

	// Token: 0x06000A31 RID: 2609 RVA: 0x000367E8 File Offset: 0x000349E8
	public override void Open(bool cancelTimer, bool makeNoise)
	{
		base.Open(cancelTimer, makeNoise);
		for (int i = 0; i < this.windows.Length; i++)
		{
			MaterialModifier.ChangeOverlay(this.windows[i], this.windowObject.open[i]);
		}
		MeshCollider[] array = this.colliders;
		for (int j = 0; j < array.Length; j++)
		{
			array[j].enabled = false;
		}
	}

	// Token: 0x06000A32 RID: 2610 RVA: 0x00036848 File Offset: 0x00034A48
	public void Break(bool makeNoise)
	{
		if (!this.broken)
		{
			this.Open(true, false);
			this.audMan.PlaySingle(this.audWindowBreak);
			if (makeNoise)
			{
				this.ec.MakeNoise(base.transform.position + base.transform.forward * this.positionOffset, this.noiseValue);
			}
			this.broken = true;
			base.aTile.Mute(this.direction, false);
			base.bTile.Mute(this.direction.GetOpposite(), false);
			this.aMapTile.SpriteRenderer.sprite = this.mapOpenSprite;
			this.bMapTile.SpriteRenderer.sprite = this.mapOpenSprite;
		}
	}

	// Token: 0x06000A33 RID: 2611 RVA: 0x0003690F File Offset: 0x00034B0F
	private void TempOpen()
	{
		this.ec.FreezeNavigationUpdates(true);
		this.Block(false);
		this.ec.FreezeNavigationUpdates(false);
	}

	// Token: 0x06000A34 RID: 2612 RVA: 0x00036930 File Offset: 0x00034B30
	private void TempClose()
	{
		this.ec.FreezeNavigationUpdates(true);
		this.Block(!this.open);
		this.ec.FreezeNavigationUpdates(false);
	}

	// Token: 0x06000A35 RID: 2613 RVA: 0x0003695C File Offset: 0x00034B5C
	private void OnDestroy()
	{
		EnvironmentController ec = this.ec;
		ec.tempOpenWindows = (EnvironmentController.TempObstacleManagement)Delegate.Remove(ec.tempOpenWindows, new EnvironmentController.TempObstacleManagement(this.TempOpen));
		EnvironmentController ec2 = this.ec;
		ec2.tempCloseWindows = (EnvironmentController.TempObstacleManagement)Delegate.Remove(ec2.tempCloseWindows, new EnvironmentController.TempObstacleManagement(this.TempClose));
	}

	// Token: 0x04000B93 RID: 2963
	public AudioManager audMan;

	// Token: 0x04000B94 RID: 2964
	public MeshRenderer[] windows;

	// Token: 0x04000B95 RID: 2965
	public MeshCollider[] colliders;

	// Token: 0x04000B96 RID: 2966
	private WindowObject windowObject;

	// Token: 0x04000B97 RID: 2967
	public SoundObject audWindowOpen;

	// Token: 0x04000B98 RID: 2968
	public SoundObject audWindowBreak;

	// Token: 0x04000B99 RID: 2969
	[SerializeField]
	private Sprite mapClosedSprite;

	// Token: 0x04000B9A RID: 2970
	[SerializeField]
	private Sprite mapOpenSprite;

	// Token: 0x04000B9B RID: 2971
	private MapTile aMapTile;

	// Token: 0x04000B9C RID: 2972
	private MapTile bMapTile;

	// Token: 0x04000B9D RID: 2973
	[Range(0f, 127f)]
	public int breakNoiseValue;

	// Token: 0x04000B9E RID: 2974
	private int startingLayer;

	// Token: 0x04000B9F RID: 2975
	public bool openOnStart;

	// Token: 0x04000BA0 RID: 2976
	private bool broken;
}
