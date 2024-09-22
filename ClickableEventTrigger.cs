using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000F3 RID: 243
public class ClickableEventTrigger : MonoBehaviour, IClickable<int>
{
	// Token: 0x060005A2 RID: 1442 RVA: 0x0001C5AC File Offset: 0x0001A7AC
	public void Clicked(int playerNumber)
	{
		UnityEvent onPress = this.OnPress;
		if (onPress == null)
		{
			return;
		}
		onPress.Invoke();
	}

	// Token: 0x060005A3 RID: 1443 RVA: 0x0001C5BE File Offset: 0x0001A7BE
	public void ClickableSighted(int player)
	{
	}

	// Token: 0x060005A4 RID: 1444 RVA: 0x0001C5C0 File Offset: 0x0001A7C0
	public void ClickableUnsighted(int player)
	{
	}

	// Token: 0x060005A5 RID: 1445 RVA: 0x0001C5C2 File Offset: 0x0001A7C2
	public bool ClickableHidden()
	{
		return false;
	}

	// Token: 0x060005A6 RID: 1446 RVA: 0x0001C5C5 File Offset: 0x0001A7C5
	public bool ClickableRequiresNormalHeight()
	{
		return this.requiresNormalHeight;
	}

	// Token: 0x040005CE RID: 1486
	[SerializeField]
	private UnityEvent OnPress;

	// Token: 0x040005CF RID: 1487
	[SerializeField]
	private bool requiresNormalHeight = true;
}
