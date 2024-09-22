using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001E1 RID: 481
public class AuthenticScreenManager : MonoBehaviour
{
	// Token: 0x06000AE8 RID: 2792 RVA: 0x000398F3 File Offset: 0x00037AF3
	public void ToggleLever()
	{
		this.leverOn = !this.leverOn;
		if (this.leverOn)
		{
			this.leverImage.sprite = this.leverOnSprite;
			return;
		}
		this.leverImage.sprite = this.leverOffSprite;
	}

	// Token: 0x06000AE9 RID: 2793 RVA: 0x0003992F File Offset: 0x00037B2F
	public void UseItem(int item)
	{
		Singleton<CoreGameManager>.Instance.GetPlayer(0).itm.selectedItem = item;
		Singleton<CoreGameManager>.Instance.GetPlayer(0).itm.UseItem();
	}

	// Token: 0x170000E3 RID: 227
	// (get) Token: 0x06000AEA RID: 2794 RVA: 0x0003995C File Offset: 0x00037B5C
	public bool LeverOn
	{
		get
		{
			return this.leverOn;
		}
	}

	// Token: 0x04000C7C RID: 3196
	[SerializeField]
	private Image leverImage;

	// Token: 0x04000C7D RID: 3197
	[SerializeField]
	private Sprite leverOffSprite;

	// Token: 0x04000C7E RID: 3198
	[SerializeField]
	private Sprite leverOnSprite;

	// Token: 0x04000C7F RID: 3199
	private bool leverOn;
}
