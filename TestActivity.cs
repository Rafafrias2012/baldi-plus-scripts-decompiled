using System;

// Token: 0x020000A1 RID: 161
public class TestActivity : Activity, IClickable<int>
{
	// Token: 0x060003B5 RID: 949 RVA: 0x000133E6 File Offset: 0x000115E6
	public void Clicked(int playerNumber)
	{
		this.Completed(playerNumber);
		base.gameObject.SetActive(false);
	}

	// Token: 0x060003B6 RID: 950 RVA: 0x000133FB File Offset: 0x000115FB
	public void ClickableSighted(int player)
	{
	}

	// Token: 0x060003B7 RID: 951 RVA: 0x000133FD File Offset: 0x000115FD
	public void ClickableUnsighted(int player)
	{
	}

	// Token: 0x060003B8 RID: 952 RVA: 0x000133FF File Offset: 0x000115FF
	public bool ClickableHidden()
	{
		return false;
	}

	// Token: 0x060003B9 RID: 953 RVA: 0x00013402 File Offset: 0x00011602
	public bool ClickableRequiresNormalHeight()
	{
		return false;
	}
}
