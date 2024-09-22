using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000118 RID: 280
public class ItemAcceptor : MonoBehaviour, IItemAcceptor
{
	// Token: 0x060006D9 RID: 1753 RVA: 0x00022C74 File Offset: 0x00020E74
	public void InsertItem(PlayerManager player, EnvironmentController ec)
	{
		this.OnInsert.Invoke();
	}

	// Token: 0x060006DA RID: 1754 RVA: 0x00022C81 File Offset: 0x00020E81
	public bool ItemFits(Items item)
	{
		return this.acceptibleItems.Contains(item);
	}

	// Token: 0x04000717 RID: 1815
	public UnityEvent OnInsert;

	// Token: 0x04000718 RID: 1816
	[SerializeField]
	private List<Items> acceptibleItems = new List<Items>();
}
