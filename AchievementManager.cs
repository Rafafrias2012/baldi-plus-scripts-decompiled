using System;
using System.Collections;
using Steamworks;
using UnityEngine;

// Token: 0x020000E4 RID: 228
public class AchievementManager : Singleton<AchievementManager>
{
	// Token: 0x0600055F RID: 1375 RVA: 0x0001B6F0 File Offset: 0x000198F0
	public void Load(AchievementData loadData)
	{
		for (int i = 0; i < this.achievementStatus.Length; i++)
		{
			if (i < loadData.found.Length)
			{
				this.achievementStatus[i] = loadData.found[i];
			}
		}
		if (SteamManager.Initialized)
		{
			this.m_UserStatsReceived = Callback<UserStatsReceived_t>.Create(new Callback<UserStatsReceived_t>.DispatchDelegate(this.OnUserStatsReceived));
			if (SteamUserStats.RequestCurrentStats())
			{
				base.StartCoroutine(this.SteamRequestDelay());
			}
		}
	}

	// Token: 0x06000560 RID: 1376 RVA: 0x0001B75D File Offset: 0x0001995D
	public AchievementData Save()
	{
		return new AchievementData
		{
			found = this.achievementStatus
		};
	}

	// Token: 0x06000561 RID: 1377 RVA: 0x0001B770 File Offset: 0x00019970
	public void Reset()
	{
		for (int i = 0; i < this.achievementStatus.Length; i++)
		{
			this.achievementStatus[i] = false;
		}
	}

	// Token: 0x06000562 RID: 1378 RVA: 0x0001B79C File Offset: 0x0001999C
	public void Set(Achievement achievement, bool status)
	{
		if (this.achievementStatus[(int)achievement] != status)
		{
			this.achievementStatus[(int)achievement] = status;
			Singleton<PlayerFileManager>.Instance.Save();
		}
		if (SteamManager.Initialized && SteamUserStats.GetAchievement(this.achievementId[(int)achievement], out this._unlocked) && status && !this._unlocked)
		{
			SteamUserStats.SetAchievement(this.achievementId[(int)achievement]);
			SteamUserStats.StoreStats();
		}
	}

	// Token: 0x06000563 RID: 1379 RVA: 0x0001B802 File Offset: 0x00019A02
	private void OnUserStatsReceived(UserStatsReceived_t pCallback)
	{
		if (pCallback.m_eResult == EResult.k_EResultOK)
		{
			this.steamStatsFound = true;
			return;
		}
		Debug.LogWarning("Steamworks user stats request failed with error " + pCallback.m_eResult.ToString());
	}

	// Token: 0x06000564 RID: 1380 RVA: 0x0001B836 File Offset: 0x00019A36
	private IEnumerator SteamRequestDelay()
	{
		if (SteamManager.Initialized)
		{
			while (!this.steamStatsFound)
			{
				yield return null;
			}
			for (int i = 0; i < AchievementManager.totalAchievements; i++)
			{
				if (this.achievementStatus[i])
				{
					this.Set((Achievement)i, true);
				}
			}
		}
		yield break;
	}

	// Token: 0x04000592 RID: 1426
	private Callback<UserStatsReceived_t> m_UserStatsReceived;

	// Token: 0x04000593 RID: 1427
	public static int totalAchievements = 1;

	// Token: 0x04000594 RID: 1428
	private bool[] achievementStatus = new bool[AchievementManager.totalAchievements];

	// Token: 0x04000595 RID: 1429
	private string[] achievementId = new string[]
	{
		"ACH_TEST"
	};

	// Token: 0x04000596 RID: 1430
	private bool _unlocked;

	// Token: 0x04000597 RID: 1431
	private bool steamStatsFound;
}
