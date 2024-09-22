using System;
using UnityEngine;

// Token: 0x02000094 RID: 148
public class ITM_YTPs : Item
{
	// Token: 0x06000352 RID: 850 RVA: 0x000118F1 File Offset: 0x0000FAF1
	public override bool Use(PlayerManager pm)
	{
		Singleton<CoreGameManager>.Instance.AddPoints(this.value, pm.playerNumber, true);
		return true;
	}

	// Token: 0x0400039D RID: 925
	[SerializeField]
	private int value = 25;
}
