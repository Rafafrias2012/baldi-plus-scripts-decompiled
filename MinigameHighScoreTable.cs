using System;
using TMPro;
using UnityEngine;

// Token: 0x0200007E RID: 126
public class MinigameHighScoreTable : MonoBehaviour
{
	// Token: 0x060002FA RID: 762 RVA: 0x0000F8CC File Offset: 0x0000DACC
	public void UpdateScores(MinigameName minigame, string nameKey)
	{
		for (int i = 0; i < this.nameTmp.Length; i++)
		{
			this.nameTmp[i].text = Singleton<HighScoreManager>.Instance.tripNames[(int)minigame, i].ToString();
			this.valueTmp[i].text = Singleton<HighScoreManager>.Instance.tripScores[(int)minigame, i].ToString();
		}
		this.titleTmp.text = Singleton<LocalizationManager>.Instance.GetLocalizedText(nameKey) + " " + Singleton<LocalizationManager>.Instance.GetLocalizedText("Mgm_Leaderboard");
	}

	// Token: 0x0400031C RID: 796
	[SerializeField]
	private TMP_Text[] nameTmp = new TMP_Text[0];

	// Token: 0x0400031D RID: 797
	[SerializeField]
	private TMP_Text[] valueTmp = new TMP_Text[0];

	// Token: 0x0400031E RID: 798
	[SerializeField]
	private TMP_Text titleTmp;
}
