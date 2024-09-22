using System;
using System.IO;
using UnityCipher;
using UnityEngine;

// Token: 0x0200010E RID: 270
public class HighScoreManager : Singleton<HighScoreManager>
{
	// Token: 0x0600069B RID: 1691 RVA: 0x00021329 File Offset: 0x0001F529
	private void Start()
	{
		this.Load();
	}

	// Token: 0x0600069C RID: 1692 RVA: 0x00021334 File Offset: 0x0001F534
	public void Load()
	{
		string path = Application.persistentDataPath + "/HighScores.hgh";
		this.SetDefaultsEndless();
		this.SetDefaultsTrips();
		if (!File.Exists(path))
		{
			Debug.Log("High scores do not exist. Setting defaults.");
			this.Save();
			return;
		}
		bool flag = true;
		HighScoreData highScoreData = new HighScoreData();
		try
		{
			highScoreData = JsonUtility.FromJson<HighScoreData>(RijndaelEncryption.Decrypt(File.ReadAllText(path), "99"));
		}
		catch
		{
			flag = false;
		}
		if (!flag)
		{
			Debug.LogWarning("High scores file is corrupted. Setting defaults.");
			this.Save();
			return;
		}
		this.randomScores = highScoreData.randomScores;
		this.smallScores = highScoreData.smallScores;
		this.mediumScores = highScoreData.mediumScores;
		this.largeScores = highScoreData.largeScores;
		this.randomNames = highScoreData.randomNames;
		this.smallNames = highScoreData.smallNames;
		this.mediumNames = highScoreData.mediumNames;
		this.largeNames = highScoreData.largeNames;
		this.randomSeeds = highScoreData.randomSeeds;
		if (highScoreData.highScoresVersion < 4)
		{
			this.SetDefaultsTrips();
			return;
		}
		this.tripScores = highScoreData.tripScores.ConvertTo2d(16, 5);
		this.tripNames = highScoreData.tripNames.ConvertTo2d(16, 5);
	}

	// Token: 0x0600069D RID: 1693 RVA: 0x0002146C File Offset: 0x0001F66C
	public void Save()
	{
		string path = Application.persistentDataPath + "/HighScores.hgh";
		if (File.Exists(path))
		{
			File.Delete(path);
		}
		File.WriteAllText(path, RijndaelEncryption.Encrypt(JsonUtility.ToJson(new HighScoreData
		{
			randomScores = this.randomScores,
			smallScores = this.smallScores,
			mediumScores = this.mediumScores,
			largeScores = this.largeScores,
			randomNames = this.randomNames,
			smallNames = this.smallNames,
			mediumNames = this.mediumNames,
			largeNames = this.largeNames,
			randomSeeds = this.randomSeeds,
			tripScores = this.tripScores.ConvertTo1d(16, 5),
			tripNames = this.tripNames.ConvertTo1d(16, 5),
			highScoresVersion = HighScoreManager.highScoresVersion
		}), "99"));
		Debug.Log("High scores saved.");
	}

	// Token: 0x0600069E RID: 1694 RVA: 0x0002155C File Offset: 0x0001F75C
	public void SetDefaultsEndless()
	{
		for (int i = 0; i < this.randomScores.Length; i++)
		{
			this.randomScores[i] = 10 - i;
			this.smallScores[i] = 10 - i;
			this.mediumScores[i] = 10 - i;
			this.largeScores[i] = 10 - i;
			this.randomNames[i] = "Baldi";
			this.smallNames[i] = "Baldi";
			this.mediumNames[i] = "Baldi";
			this.largeNames[i] = "Baldi";
			this.randomSeeds[i] = 99 * Random.Range(1, 19999999);
		}
	}

	// Token: 0x0600069F RID: 1695 RVA: 0x000215FC File Offset: 0x0001F7FC
	public void SetDefaultsTrips()
	{
		for (int i = 0; i < 5; i++)
		{
			for (int j = 0; j < 16; j++)
			{
				this.tripScores[j, i] = 0;
				this.tripNames[j, i] = "Baldi";
			}
		}
	}

	// Token: 0x060006A0 RID: 1696 RVA: 0x00021644 File Offset: 0x0001F844
	public void AddScore(int score, string name, EndlessLevel level, out int rank)
	{
		bool flag = false;
		rank = -1;
		int num = 0;
		while (num < 10 && !flag)
		{
			if (level != EndlessLevel.Random)
			{
				if (level == EndlessLevel.Medium)
				{
					if (score > this.mediumScores[num])
					{
						for (int i = 9; i > num; i--)
						{
							this.mediumScores[i] = this.mediumScores[i - 1];
							this.mediumNames[i] = this.mediumNames[i - 1];
						}
						this.mediumScores[num] = score;
						this.mediumNames[num] = name;
						rank = num;
						flag = true;
					}
				}
			}
			else if (score > this.randomScores[num])
			{
				for (int j = 9; j > num; j--)
				{
					this.randomScores[j] = this.randomScores[j - 1];
					this.randomNames[j] = this.randomNames[j - 1];
					this.randomSeeds[j] = this.randomSeeds[j - 1];
				}
				this.randomScores[num] = score;
				this.randomNames[num] = name;
				this.randomSeeds[num] = Singleton<CoreGameManager>.Instance.Seed();
				rank = num;
				flag = true;
			}
			num++;
		}
		if (flag)
		{
			this.Save();
		}
	}

	// Token: 0x060006A1 RID: 1697 RVA: 0x00021758 File Offset: 0x0001F958
	public void AddTripScore(int score, string name, MinigameName minigame, bool greaterWins, out int rank)
	{
		bool flag = false;
		rank = -1;
		if (greaterWins)
		{
			for (int i = 0; i < 5; i++)
			{
				if (flag)
				{
					break;
				}
				if (score > this.tripScores[(int)minigame, i])
				{
					for (int j = 4; j > i; j--)
					{
						this.tripScores[(int)minigame, j] = this.tripScores[(int)minigame, j - 1];
						this.tripNames[(int)minigame, j] = this.tripNames[(int)minigame, j - 1];
					}
					this.tripScores[(int)minigame, i] = score;
					this.tripNames[(int)minigame, i] = name;
					rank = i;
					flag = true;
				}
			}
		}
		else
		{
			int num = 0;
			while (num < 5 && !flag)
			{
				if (score < this.tripScores[(int)minigame, num])
				{
					for (int k = 4; k > num; k--)
					{
						this.tripScores[(int)minigame, k] = this.tripScores[(int)minigame, k - 1];
						this.tripNames[(int)minigame, k] = this.tripNames[(int)minigame, k - 1];
					}
					this.tripScores[(int)minigame, num] = score;
					this.tripNames[(int)minigame, num] = name;
					rank = num;
					flag = true;
				}
				num++;
			}
		}
		if (flag)
		{
			this.Save();
		}
	}

	// Token: 0x040006BC RID: 1724
	public int[] randomScores = new int[10];

	// Token: 0x040006BD RID: 1725
	public int[] randomSeeds = new int[10];

	// Token: 0x040006BE RID: 1726
	public int[] smallScores = new int[10];

	// Token: 0x040006BF RID: 1727
	public int[] mediumScores = new int[10];

	// Token: 0x040006C0 RID: 1728
	public int[] largeScores = new int[10];

	// Token: 0x040006C1 RID: 1729
	public string[] randomNames = new string[10];

	// Token: 0x040006C2 RID: 1730
	public string[] smallNames = new string[10];

	// Token: 0x040006C3 RID: 1731
	public string[] mediumNames = new string[10];

	// Token: 0x040006C4 RID: 1732
	public string[] largeNames = new string[10];

	// Token: 0x040006C5 RID: 1733
	public int[,] tripScores = new int[16, 5];

	// Token: 0x040006C6 RID: 1734
	public string[,] tripNames = new string[16, 5];

	// Token: 0x040006C7 RID: 1735
	private static int highScoresVersion = 4;
}
