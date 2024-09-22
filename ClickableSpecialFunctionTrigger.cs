using System;
using UnityEngine;

// Token: 0x020000F5 RID: 245
public class ClickableSpecialFunctionTrigger : MonoBehaviour, IClickable<int>
{
	// Token: 0x060005AF RID: 1455 RVA: 0x0001C63B File Offset: 0x0001A83B
	public void Clicked(int player)
	{
		if (Singleton<BaseGameManager>.Instance != null)
		{
			Singleton<BaseGameManager>.Instance.CallSpecialManagerFunction(this.functionToTrigger, base.gameObject);
		}
	}

	// Token: 0x060005B0 RID: 1456 RVA: 0x0001C660 File Offset: 0x0001A860
	public void ClickableSighted(int player)
	{
	}

	// Token: 0x060005B1 RID: 1457 RVA: 0x0001C662 File Offset: 0x0001A862
	public void ClickableUnsighted(int player)
	{
	}

	// Token: 0x060005B2 RID: 1458 RVA: 0x0001C664 File Offset: 0x0001A864
	public bool ClickableHidden()
	{
		return false;
	}

	// Token: 0x060005B3 RID: 1459 RVA: 0x0001C667 File Offset: 0x0001A867
	public bool ClickableRequiresNormalHeight()
	{
		return true;
	}

	// Token: 0x040005D2 RID: 1490
	[SerializeField]
	private int functionToTrigger;
}
