using System;
using UnityEngine;

// Token: 0x020001E3 RID: 483
public class ClassicExit : Elevator
{
	// Token: 0x06000AF4 RID: 2804 RVA: 0x00039B60 File Offset: 0x00037D60
	public override void Close()
	{
		this.open = false;
		this.gateCollider.enabled = true;
		if (this.openedOnce)
		{
			this.audMan.PlaySingle(this.audGateClose);
			this.wallWithMap.SetActive(true);
		}
		else
		{
			this.wall.SetActive(true);
		}
		this.openedOnce = true;
	}

	// Token: 0x06000AF5 RID: 2805 RVA: 0x00039BBA File Offset: 0x00037DBA
	public override void Open()
	{
		this.open = true;
		this.gateCollider.enabled = false;
		this.wall.SetActive(false);
		this.wallWithMap.SetActive(false);
	}

	// Token: 0x04000C89 RID: 3209
	[SerializeField]
	private GameObject wall;

	// Token: 0x04000C8A RID: 3210
	[SerializeField]
	private GameObject wallWithMap;

	// Token: 0x04000C8B RID: 3211
	private bool openedOnce;
}
