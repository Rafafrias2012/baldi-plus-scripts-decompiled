using System;
using UnityEngine;

// Token: 0x02000091 RID: 145
public class ITM_SwingDoorLock : Item
{
	// Token: 0x0600034A RID: 842 RVA: 0x0001170C File Offset: 0x0000F90C
	public override bool Use(PlayerManager pm)
	{
		if (Physics.Raycast(pm.transform.position, Singleton<CoreGameManager>.Instance.GetCamera(pm.playerNumber).transform.forward, out this.hit, pm.pc.reach, pm.pc.ClickLayers))
		{
			IItemAcceptor component = this.hit.transform.GetComponent<IItemAcceptor>();
			if (component != null && component.ItemFits(Items.DoorLock))
			{
				component.InsertItem(pm, pm.ec);
				Object.Destroy(base.gameObject);
				return true;
			}
		}
		Object.Destroy(base.gameObject);
		return false;
	}

	// Token: 0x04000399 RID: 921
	private RaycastHit hit;
}
