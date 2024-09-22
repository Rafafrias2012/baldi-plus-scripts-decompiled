using System;
using UnityEngine;

// Token: 0x02000090 RID: 144
public class ITM_Scissors : Item
{
	// Token: 0x06000348 RID: 840 RVA: 0x00011560 File Offset: 0x0000F760
	public override bool Use(PlayerManager pm)
	{
		bool flag = false;
		if (pm.jumpropes.Count > 0)
		{
			while (pm.jumpropes.Count > 0)
			{
				pm.jumpropes[0].End(false);
			}
			flag = true;
		}
		if (Gum.playerGum.Count > 0)
		{
			foreach (Gum gum in Gum.playerGum)
			{
				gum.Cut();
			}
			flag = true;
		}
		if (Physics.Raycast(pm.transform.position, Singleton<CoreGameManager>.Instance.GetCamera(pm.playerNumber).transform.forward, out this.hit, pm.pc.reach, this.clickMask))
		{
			IItemAcceptor component = this.hit.transform.GetComponent<IItemAcceptor>();
			if (component != null && component.ItemFits(Items.Scissors))
			{
				component.InsertItem(pm, pm.ec);
				flag = true;
			}
		}
		else
		{
			Collider[] array = new Collider[16];
			int num = Physics.OverlapSphereNonAlloc(pm.transform.position, 4f, array, 131072, QueryTriggerInteraction.Collide);
			for (int i = 0; i < num; i++)
			{
				if (array[i].isTrigger && array[i].CompareTag("NPC"))
				{
					FirstPrize component2 = array[i].GetComponent<FirstPrize>();
					if (component2 != null)
					{
						component2.CutWires();
						flag = true;
					}
				}
			}
		}
		Object.Destroy(base.gameObject);
		if (flag)
		{
			Singleton<CoreGameManager>.Instance.audMan.PlaySingle(this.audSnip);
		}
		return flag;
	}

	// Token: 0x04000396 RID: 918
	private RaycastHit hit;

	// Token: 0x04000397 RID: 919
	[SerializeField]
	private LayerMask clickMask;

	// Token: 0x04000398 RID: 920
	[SerializeField]
	private SoundObject audSnip;
}
