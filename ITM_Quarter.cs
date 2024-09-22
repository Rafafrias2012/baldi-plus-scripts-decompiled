using System;
using UnityEngine;

// Token: 0x0200008F RID: 143
public class ITM_Quarter : Item
{
	// Token: 0x06000346 RID: 838 RVA: 0x000114B8 File Offset: 0x0000F6B8
	public override bool Use(PlayerManager pm)
	{
		if (Physics.Raycast(pm.transform.position, Singleton<CoreGameManager>.Instance.GetCamera(pm.playerNumber).transform.forward, out this.hit, pm.pc.reach, pm.pc.ClickLayers))
		{
			IItemAcceptor component = this.hit.transform.GetComponent<IItemAcceptor>();
			if (component != null && component.ItemFits(Items.Quarter))
			{
				component.InsertItem(pm, pm.ec);
				Object.Destroy(base.gameObject);
				return true;
			}
		}
		Object.Destroy(base.gameObject);
		return false;
	}

	// Token: 0x04000395 RID: 917
	private RaycastHit hit;
}
