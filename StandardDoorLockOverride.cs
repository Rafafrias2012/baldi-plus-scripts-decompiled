using System;
using UnityEngine;

// Token: 0x02000075 RID: 117
public class StandardDoorLockOverride : MonoBehaviour, IClickable<int>
{
	// Token: 0x060002A7 RID: 679 RVA: 0x0000EA88 File Offset: 0x0000CC88
	public void Clicked(int player)
	{
		bool locked = this.door.locked;
		this.door.locked = false;
		this.door.Clicked(player);
		this.door.locked = locked;
	}

	// Token: 0x060002A8 RID: 680 RVA: 0x0000EAC5 File Offset: 0x0000CCC5
	public void ClickableSighted(int player)
	{
	}

	// Token: 0x060002A9 RID: 681 RVA: 0x0000EAC7 File Offset: 0x0000CCC7
	public void ClickableUnsighted(int player)
	{
	}

	// Token: 0x060002AA RID: 682 RVA: 0x0000EAC9 File Offset: 0x0000CCC9
	public bool ClickableHidden()
	{
		return false;
	}

	// Token: 0x060002AB RID: 683 RVA: 0x0000EACC File Offset: 0x0000CCCC
	public bool ClickableRequiresNormalHeight()
	{
		return false;
	}

	// Token: 0x040002C1 RID: 705
	[SerializeField]
	private StandardDoor door;
}
