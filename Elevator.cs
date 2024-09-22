using System;
using UnityEngine;

// Token: 0x020001BB RID: 443
public class Elevator : MonoBehaviour
{
	// Token: 0x060009F0 RID: 2544 RVA: 0x00035630 File Offset: 0x00033830
	public void Initialize(EnvironmentController ec)
	{
		this.mapIcon = ec.map.AddIcon(this.mapIconPre, base.transform, Color.white);
		this.mapIcon.spriteRenderer.sprite = this.openIconSprite;
		ec.elevators.Add(this);
	}

	// Token: 0x170000D1 RID: 209
	// (get) Token: 0x060009F1 RID: 2545 RVA: 0x00035681 File Offset: 0x00033881
	public ColliderGroup ColliderGroup
	{
		get
		{
			return this.colliderGroup;
		}
	}

	// Token: 0x170000D2 RID: 210
	// (get) Token: 0x060009F2 RID: 2546 RVA: 0x00035689 File Offset: 0x00033889
	public ColliderGroup InsideCollider
	{
		get
		{
			return this.insideCollider;
		}
	}

	// Token: 0x170000D3 RID: 211
	// (get) Token: 0x060009F3 RID: 2547 RVA: 0x00035691 File Offset: 0x00033891
	public Door Door
	{
		get
		{
			return this.door;
		}
	}

	// Token: 0x060009F4 RID: 2548 RVA: 0x00035699 File Offset: 0x00033899
	public void PrepareToClose()
	{
		this.gateCollider.enabled = true;
		this.mapIcon.spriteRenderer.color = Color.green;
	}

	// Token: 0x060009F5 RID: 2549 RVA: 0x000356BC File Offset: 0x000338BC
	public void PrepareForExit()
	{
		this.gateCollider.enabled = false;
	}

	// Token: 0x060009F6 RID: 2550 RVA: 0x000356CC File Offset: 0x000338CC
	public virtual void Close()
	{
		this.open = false;
		this.gateCollider.enabled = true;
		this.animator.Play("Close");
		this.audMan.PlaySingle(this.audGateClose);
		this.mapIcon.spriteRenderer.sprite = this.lockedIconSprite;
		this.mapIcon.spriteRenderer.color = Color.red;
	}

	// Token: 0x060009F7 RID: 2551 RVA: 0x00035738 File Offset: 0x00033938
	public virtual void Open()
	{
		this.open = true;
		this.gateCollider.enabled = false;
	}

	// Token: 0x170000D4 RID: 212
	// (get) Token: 0x060009F8 RID: 2552 RVA: 0x0003574D File Offset: 0x0003394D
	public bool IsOpen
	{
		get
		{
			return this.open;
		}
	}

	// Token: 0x170000D5 RID: 213
	// (get) Token: 0x060009F9 RID: 2553 RVA: 0x00035755 File Offset: 0x00033955
	// (set) Token: 0x060009FA RID: 2554 RVA: 0x0003575D File Offset: 0x0003395D
	public bool IsSpawn
	{
		get
		{
			return this.isSpawn;
		}
		set
		{
			this.isSpawn = value;
		}
	}

	// Token: 0x170000D6 RID: 214
	// (get) Token: 0x060009FB RID: 2555 RVA: 0x00035766 File Offset: 0x00033966
	public bool LobbyBlocksNpcs
	{
		get
		{
			return this.lobbyBlocksNpcs;
		}
	}

	// Token: 0x04000B4F RID: 2895
	[SerializeField]
	protected Door door;

	// Token: 0x04000B50 RID: 2896
	[SerializeField]
	protected ColliderGroup colliderGroup;

	// Token: 0x04000B51 RID: 2897
	[SerializeField]
	protected ColliderGroup insideCollider;

	// Token: 0x04000B52 RID: 2898
	[SerializeField]
	protected AudioManager audMan;

	// Token: 0x04000B53 RID: 2899
	[SerializeField]
	protected SoundObject audGateClose;

	// Token: 0x04000B54 RID: 2900
	[SerializeField]
	protected Animator animator;

	// Token: 0x04000B55 RID: 2901
	[SerializeField]
	protected MeshCollider gateCollider;

	// Token: 0x04000B56 RID: 2902
	[SerializeField]
	protected MapIcon mapIconPre;

	// Token: 0x04000B57 RID: 2903
	protected MapIcon mapIcon;

	// Token: 0x04000B58 RID: 2904
	[SerializeField]
	protected Sprite lockedIconSprite;

	// Token: 0x04000B59 RID: 2905
	[SerializeField]
	protected Sprite openIconSprite;

	// Token: 0x04000B5A RID: 2906
	[SerializeField]
	protected bool lobbyBlocksNpcs = true;

	// Token: 0x04000B5B RID: 2907
	protected bool open = true;

	// Token: 0x04000B5C RID: 2908
	protected bool isSpawn;
}
