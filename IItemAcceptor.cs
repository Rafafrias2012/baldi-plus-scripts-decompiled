using System;

// Token: 0x02000117 RID: 279
public interface IItemAcceptor
{
	// Token: 0x060006D7 RID: 1751
	void InsertItem(PlayerManager player, EnvironmentController ec);

	// Token: 0x060006D8 RID: 1752
	bool ItemFits(Items item);
}
