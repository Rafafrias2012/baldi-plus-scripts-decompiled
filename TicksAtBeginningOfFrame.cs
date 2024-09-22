using System;
using UnityEngine;

// Token: 0x0200013D RID: 317
public class TicksAtBeginningOfFrame : MonoBehaviour
{
	// Token: 0x06000763 RID: 1891 RVA: 0x00025F30 File Offset: 0x00024130
	private void Update()
	{
		this.ticks = DateTime.Now.Ticks;
	}

	// Token: 0x1700008D RID: 141
	// (get) Token: 0x06000764 RID: 1892 RVA: 0x00025F50 File Offset: 0x00024150
	public long Ticks
	{
		get
		{
			return this.ticks;
		}
	}

	// Token: 0x0400081A RID: 2074
	private long ticks;
}
