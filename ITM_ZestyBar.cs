using System;
using UnityEngine;

// Token: 0x02000095 RID: 149
public class ITM_ZestyBar : Item
{
	// Token: 0x06000354 RID: 852 RVA: 0x0001191B File Offset: 0x0000FB1B
	public override bool Use(PlayerManager pm)
	{
		pm.plm.stamina = pm.plm.staminaMax * 2f;
		Singleton<CoreGameManager>.Instance.audMan.PlaySingle(this.audEat);
		Object.Destroy(base.gameObject);
		return true;
	}

	// Token: 0x0400039E RID: 926
	[SerializeField]
	private SoundObject audEat;
}
