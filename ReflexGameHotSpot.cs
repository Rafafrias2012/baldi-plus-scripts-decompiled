using System;
using UnityEngine;

// Token: 0x0200003B RID: 59
public class ReflexGameHotSpot : MonoBehaviour, IClickable<int>
{
	// Token: 0x06000170 RID: 368 RVA: 0x000090BA File Offset: 0x000072BA
	public void Clicked(int playerNumber)
	{
		this.drReflex.HotspotClicked(this.side);
	}

	// Token: 0x06000171 RID: 369 RVA: 0x000090CD File Offset: 0x000072CD
	public void ClickableSighted(int player)
	{
	}

	// Token: 0x06000172 RID: 370 RVA: 0x000090CF File Offset: 0x000072CF
	public void ClickableUnsighted(int player)
	{
	}

	// Token: 0x06000173 RID: 371 RVA: 0x000090D1 File Offset: 0x000072D1
	public bool ClickableHidden()
	{
		return false;
	}

	// Token: 0x06000174 RID: 372 RVA: 0x000090D4 File Offset: 0x000072D4
	public bool ClickableRequiresNormalHeight()
	{
		return true;
	}

	// Token: 0x04000183 RID: 387
	[SerializeField]
	private DrReflex drReflex;

	// Token: 0x04000184 RID: 388
	[SerializeField]
	private Direction side = Direction.East;
}
