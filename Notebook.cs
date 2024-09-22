using System;
using UnityEngine;

// Token: 0x020001C6 RID: 454
public class Notebook : MonoBehaviour, IClickable<int>
{
	// Token: 0x06000A4E RID: 2638 RVA: 0x00036F45 File Offset: 0x00035145
	private void Start()
	{
		this.sprite.sprite = this.sprites[Random.Range(0, this.sprites.Length)];
	}

	// Token: 0x06000A4F RID: 2639 RVA: 0x00036F68 File Offset: 0x00035168
	public void Clicked(int player)
	{
		this.Hide(true);
		Singleton<BaseGameManager>.Instance.CollectNotebook(this);
		Singleton<CoreGameManager>.Instance.GetPlayer(player).plm.AddStamina(Singleton<CoreGameManager>.Instance.GetPlayer(player).plm.staminaMax, true);
		Singleton<CoreGameManager>.Instance.audMan.PlaySingle(this.audPickup);
	}

	// Token: 0x06000A50 RID: 2640 RVA: 0x00036FC7 File Offset: 0x000351C7
	public void ClickableSighted(int player)
	{
	}

	// Token: 0x06000A51 RID: 2641 RVA: 0x00036FC9 File Offset: 0x000351C9
	public void ClickableUnsighted(int player)
	{
	}

	// Token: 0x06000A52 RID: 2642 RVA: 0x00036FCB File Offset: 0x000351CB
	public bool ClickableHidden()
	{
		return false;
	}

	// Token: 0x06000A53 RID: 2643 RVA: 0x00036FCE File Offset: 0x000351CE
	public bool ClickableRequiresNormalHeight()
	{
		return true;
	}

	// Token: 0x06000A54 RID: 2644 RVA: 0x00036FD4 File Offset: 0x000351D4
	public void Hide(bool hide)
	{
		if (hide)
		{
			this.sprite.gameObject.SetActive(false);
			this.sphereCollider.enabled = false;
			if (this.icon != null)
			{
				this.icon.spriteRenderer.enabled = false;
				return;
			}
		}
		else
		{
			this.sprite.gameObject.SetActive(true);
			this.sphereCollider.enabled = true;
			if (this.icon != null)
			{
				this.icon.spriteRenderer.enabled = true;
			}
		}
	}

	// Token: 0x04000BBE RID: 3006
	[SerializeField]
	private SpriteRenderer sprite;

	// Token: 0x04000BBF RID: 3007
	[SerializeField]
	private SphereCollider sphereCollider;

	// Token: 0x04000BC0 RID: 3008
	[SerializeField]
	private Sprite[] sprites = new Sprite[0];

	// Token: 0x04000BC1 RID: 3009
	public Activity activity;

	// Token: 0x04000BC2 RID: 3010
	public MapIcon iconPre;

	// Token: 0x04000BC3 RID: 3011
	public MapIcon icon;

	// Token: 0x04000BC4 RID: 3012
	public SoundObject audPickup;
}
