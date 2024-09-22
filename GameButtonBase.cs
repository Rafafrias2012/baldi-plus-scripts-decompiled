using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001C3 RID: 451
public class GameButtonBase : MonoBehaviour, IClickable<int>
{
	// Token: 0x06000A41 RID: 2625 RVA: 0x00036E0D File Offset: 0x0003500D
	public void SetUp(IButtonReceiver newReceiver)
	{
		this.buttonReceivers.Add(newReceiver);
	}

	// Token: 0x06000A42 RID: 2626 RVA: 0x00036E1B File Offset: 0x0003501B
	public void Clicked(int playerNumber)
	{
		this.Pressed(playerNumber);
	}

	// Token: 0x06000A43 RID: 2627 RVA: 0x00036E24 File Offset: 0x00035024
	public void ClickableSighted(int player)
	{
	}

	// Token: 0x06000A44 RID: 2628 RVA: 0x00036E26 File Offset: 0x00035026
	public void ClickableUnsighted(int player)
	{
	}

	// Token: 0x06000A45 RID: 2629 RVA: 0x00036E28 File Offset: 0x00035028
	public bool ClickableHidden()
	{
		return false;
	}

	// Token: 0x06000A46 RID: 2630 RVA: 0x00036E2B File Offset: 0x0003502B
	public bool ClickableRequiresNormalHeight()
	{
		return true;
	}

	// Token: 0x06000A47 RID: 2631 RVA: 0x00036E2E File Offset: 0x0003502E
	protected virtual void Pressed(int playerNumber)
	{
		if (this.triggerSpecialManagerFunction)
		{
			Singleton<BaseGameManager>.Instance.CallSpecialManagerFunction(this.functionToTrigger, base.gameObject);
		}
	}

	// Token: 0x04000BB3 RID: 2995
	protected List<IButtonReceiver> buttonReceivers = new List<IButtonReceiver>();

	// Token: 0x04000BB4 RID: 2996
	[SerializeField]
	private bool triggerSpecialManagerFunction;

	// Token: 0x04000BB5 RID: 2997
	[SerializeField]
	private int functionToTrigger;
}
