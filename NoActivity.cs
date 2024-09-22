using System;

// Token: 0x020000A0 RID: 160
public class NoActivity : Activity
{
	// Token: 0x060003B2 RID: 946 RVA: 0x000133CD File Offset: 0x000115CD
	private void Start()
	{
		this.ReInit();
	}

	// Token: 0x060003B3 RID: 947 RVA: 0x000133D5 File Offset: 0x000115D5
	public override void ReInit()
	{
		this.Completed(0);
	}
}
