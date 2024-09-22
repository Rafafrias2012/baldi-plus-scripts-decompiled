using System;

// Token: 0x0200010F RID: 271
[Serializable]
internal class HighScoreData
{
	// Token: 0x040006C8 RID: 1736
	public int[] randomScores = new int[10];

	// Token: 0x040006C9 RID: 1737
	public int[] randomSeeds = new int[10];

	// Token: 0x040006CA RID: 1738
	public int[] smallScores = new int[10];

	// Token: 0x040006CB RID: 1739
	public int[] mediumScores = new int[10];

	// Token: 0x040006CC RID: 1740
	public int[] largeScores = new int[10];

	// Token: 0x040006CD RID: 1741
	public string[] randomNames = new string[10];

	// Token: 0x040006CE RID: 1742
	public string[] smallNames = new string[10];

	// Token: 0x040006CF RID: 1743
	public string[] mediumNames = new string[10];

	// Token: 0x040006D0 RID: 1744
	public string[] largeNames = new string[10];

	// Token: 0x040006D1 RID: 1745
	public int[] tripScores = new int[0];

	// Token: 0x040006D2 RID: 1746
	public string[] tripNames = new string[0];

	// Token: 0x040006D3 RID: 1747
	public int highScoresVersion;
}
