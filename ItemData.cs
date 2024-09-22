using System;
using UnityEngine;

// Token: 0x02000180 RID: 384
[Serializable]
public class ItemData
{
	// Token: 0x060008DB RID: 2267 RVA: 0x0002D7B4 File Offset: 0x0002B9B4
	public ItemData GetNew()
	{
		return new ItemData
		{
			item = this.item,
			position = this.position
		};
	}

	// Token: 0x04000997 RID: 2455
	public ItemObject item;

	// Token: 0x04000998 RID: 2456
	public Vector2 position;
}
