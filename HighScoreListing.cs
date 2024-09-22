using System;
using TMPro;
using UnityEngine;

// Token: 0x0200020C RID: 524
public class HighScoreListing : MonoBehaviour
{
	// Token: 0x06000B94 RID: 2964 RVA: 0x0003CAFE File Offset: 0x0003ACFE
	public void UseSeed()
	{
		this.screen.seedInput.SetValue(Singleton<HighScoreManager>.Instance.randomSeeds[this.rank - 1]);
	}

	// Token: 0x04000DE8 RID: 3560
	public int rank;

	// Token: 0x04000DE9 RID: 3561
	public EndlessMapOverview screen;

	// Token: 0x04000DEA RID: 3562
	public new TMP_Text name;

	// Token: 0x04000DEB RID: 3563
	public TMP_Text score;
}
