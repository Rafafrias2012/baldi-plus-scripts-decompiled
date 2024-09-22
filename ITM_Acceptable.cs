using System;
using UnityEngine;

// Token: 0x02000088 RID: 136
public class ITM_Acceptable : Item
{
	// Token: 0x0600032F RID: 815 RVA: 0x000109B0 File Offset: 0x0000EBB0
	public override bool Use(PlayerManager pm)
	{
		if (Physics.Raycast(pm.transform.position, Singleton<CoreGameManager>.Instance.GetCamera(pm.playerNumber).transform.forward, out this.hit, pm.pc.reach, pm.pc.ClickLayers))
		{
			foreach (IItemAcceptor itemAcceptor in this.hit.transform.GetComponents<IItemAcceptor>())
			{
				if (itemAcceptor != null && itemAcceptor.ItemFits(this.item))
				{
					itemAcceptor.InsertItem(pm, pm.ec);
					Object.Destroy(base.gameObject);
					if (this.audUse != null)
					{
						Singleton<CoreGameManager>.Instance.audMan.PlaySingle(this.audUse);
					}
					return true;
				}
			}
		}
		Object.Destroy(base.gameObject);
		return false;
	}

	// Token: 0x0400036C RID: 876
	[SerializeField]
	private Items item;

	// Token: 0x0400036D RID: 877
	private RaycastHit hit;

	// Token: 0x0400036E RID: 878
	[SerializeField]
	private SoundObject audUse;
}
