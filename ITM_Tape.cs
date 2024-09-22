using System;
using UnityEngine;

// Token: 0x02000092 RID: 146
public class ITM_Tape : Item
{
	// Token: 0x0600034C RID: 844 RVA: 0x000117B4 File Offset: 0x0000F9B4
	public override bool Use(PlayerManager pm)
	{
		if (Physics.Raycast(pm.transform.position, Singleton<CoreGameManager>.Instance.GetCamera(pm.playerNumber).transform.forward, out this.hit, pm.pc.reach, pm.pc.ClickLayers))
		{
			IItemAcceptor component = this.hit.transform.GetComponent<IItemAcceptor>();
			if (component != null && component.ItemFits(Items.Tape))
			{
				component.InsertItem(pm, pm.ec);
				Object.Destroy(base.gameObject);
				return true;
			}
		}
		Object.Destroy(base.gameObject);
		return false;
	}

	// Token: 0x0400039A RID: 922
	private RaycastHit hit;
}
