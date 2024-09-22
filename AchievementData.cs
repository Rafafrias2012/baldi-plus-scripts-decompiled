using System;

// Token: 0x020000E6 RID: 230
[Serializable]
public class AchievementData
{
	// Token: 0x06000567 RID: 1383 RVA: 0x0001B879 File Offset: 0x00019A79
	public AchievementData()
	{
		this.found = new bool[AchievementManager.totalAchievements];
	}

	// Token: 0x0400059A RID: 1434
	public bool[] found;
}
