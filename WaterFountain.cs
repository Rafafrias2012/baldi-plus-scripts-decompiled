using System;
using UnityEngine;

// Token: 0x020001CB RID: 459
public class WaterFountain : EnvironmentObject, IClickable<int>
{
	// Token: 0x06000A66 RID: 2662 RVA: 0x0003740C File Offset: 0x0003560C
	public void Clicked(int playerNumber)
	{
		Singleton<CoreGameManager>.Instance.GetPlayer(playerNumber).plm.AddStamina(Singleton<CoreGameManager>.Instance.GetPlayer(playerNumber).plm.staminaMax, true);
		this.audMan.PlaySingle(this.audSip);
	}

	// Token: 0x06000A67 RID: 2663 RVA: 0x0003744A File Offset: 0x0003564A
	public void ClickableSighted(int player)
	{
	}

	// Token: 0x06000A68 RID: 2664 RVA: 0x0003744C File Offset: 0x0003564C
	public void ClickableUnsighted(int player)
	{
	}

	// Token: 0x06000A69 RID: 2665 RVA: 0x0003744E File Offset: 0x0003564E
	public bool ClickableHidden()
	{
		return false;
	}

	// Token: 0x06000A6A RID: 2666 RVA: 0x00037451 File Offset: 0x00035651
	public bool ClickableRequiresNormalHeight()
	{
		return true;
	}

	// Token: 0x04000BE6 RID: 3046
	[SerializeField]
	private AudioManager audMan;

	// Token: 0x04000BE7 RID: 3047
	[SerializeField]
	private SoundObject audSip;
}
