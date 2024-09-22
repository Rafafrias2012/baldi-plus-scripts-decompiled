using System;
using UnityEngine;

// Token: 0x02000116 RID: 278
public class Item : MonoBehaviour
{
	// Token: 0x060006D5 RID: 1749 RVA: 0x00022C5E File Offset: 0x00020E5E
	public virtual bool Use(PlayerManager pm)
	{
		Object.Destroy(base.gameObject);
		return false;
	}

	// Token: 0x04000716 RID: 1814
	protected PlayerManager pm;
}
