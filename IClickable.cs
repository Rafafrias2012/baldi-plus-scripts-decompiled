using System;

// Token: 0x020000F2 RID: 242
public interface IClickable<T>
{
	// Token: 0x0600059D RID: 1437
	void Clicked(T player);

	// Token: 0x0600059E RID: 1438
	void ClickableSighted(T player);

	// Token: 0x0600059F RID: 1439
	void ClickableUnsighted(T player);

	// Token: 0x060005A0 RID: 1440
	bool ClickableHidden();

	// Token: 0x060005A1 RID: 1441
	bool ClickableRequiresNormalHeight();
}
