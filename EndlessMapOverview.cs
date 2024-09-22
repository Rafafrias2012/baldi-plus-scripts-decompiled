using System;
using TMPro;
using UnityEngine;

// Token: 0x0200020A RID: 522
public class EndlessMapOverview : MonoBehaviour
{
	// Token: 0x06000B8D RID: 2957 RVA: 0x0003C845 File Offset: 0x0003AA45
	public void AssignLevelKey(string key)
	{
		this.levelKey = key;
	}

	// Token: 0x06000B8E RID: 2958 RVA: 0x0003C84E File Offset: 0x0003AA4E
	public void AssignLevel(SceneObject level)
	{
		this.currentLevel = level;
	}

	// Token: 0x06000B8F RID: 2959 RVA: 0x0003C858 File Offset: 0x0003AA58
	public void LoadScores(string scoreKey)
	{
		if (scoreKey == "EndlessRandom_1")
		{
			this.seedInput.gameObject.SetActive(true);
		}
		else
		{
			this.seedInput.gameObject.SetActive(false);
		}
		this.levelName.text = Singleton<LocalizationManager>.Instance.GetLocalizedText(this.levelKey);
		for (int i = 0; i < this.listings.Length; i++)
		{
			if (scoreKey != null)
			{
				if (!(scoreKey == "EndlessRandom_1"))
				{
					if (!(scoreKey == "small"))
					{
						if (!(scoreKey == "EndlessMedium_1"))
						{
							if (scoreKey == "large")
							{
								this.listings[i].name.text = Singleton<HighScoreManager>.Instance.largeNames[i];
								this.listings[i].score.text = Singleton<HighScoreManager>.Instance.largeScores[i].ToString();
							}
						}
						else
						{
							this.listings[i].name.text = Singleton<HighScoreManager>.Instance.mediumNames[i];
							this.listings[i].score.text = Singleton<HighScoreManager>.Instance.mediumScores[i].ToString();
						}
					}
					else
					{
						this.listings[i].name.text = Singleton<HighScoreManager>.Instance.smallNames[i];
						this.listings[i].score.text = Singleton<HighScoreManager>.Instance.smallScores[i].ToString();
					}
				}
				else
				{
					this.listings[i].name.text = Singleton<HighScoreManager>.Instance.randomNames[i];
					this.listings[i].score.text = Singleton<HighScoreManager>.Instance.randomScores[i].ToString();
				}
			}
		}
	}

	// Token: 0x06000B90 RID: 2960 RVA: 0x0003CA30 File Offset: 0x0003AC30
	public void LoadLevel()
	{
		if (this.seedInput.UseSeed)
		{
			this.gameLoader.useSeed = true;
			this.gameLoader.seed = this.seedInput.Seed;
		}
		else
		{
			this.gameLoader.useSeed = false;
		}
		this.gameLoader.Initialize(0);
		this.gameLoader.SetMode(0);
		this.gameLoader.LoadLevel(this.currentLevel);
	}

	// Token: 0x04000DE0 RID: 3552
	public HighScoreListing[] listings = new HighScoreListing[10];

	// Token: 0x04000DE1 RID: 3553
	public GameLoader gameLoader;

	// Token: 0x04000DE2 RID: 3554
	public TMP_Text levelName;

	// Token: 0x04000DE3 RID: 3555
	public SeedInput seedInput;

	// Token: 0x04000DE4 RID: 3556
	private SceneObject currentLevel;

	// Token: 0x04000DE5 RID: 3557
	private string levelKey = "";
}
