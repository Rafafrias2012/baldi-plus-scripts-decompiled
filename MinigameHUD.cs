using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200007D RID: 125
public class MinigameHUD : MonoBehaviour
{
	// Token: 0x17000030 RID: 48
	// (get) Token: 0x060002EA RID: 746 RVA: 0x0000F6DA File Offset: 0x0000D8DA
	// (set) Token: 0x060002EB RID: 747 RVA: 0x0000F6E2 File Offset: 0x0000D8E2
	public int livesLost { get; private set; }

	// Token: 0x17000031 RID: 49
	// (get) Token: 0x060002EC RID: 748 RVA: 0x0000F6EB File Offset: 0x0000D8EB
	// (set) Token: 0x060002ED RID: 749 RVA: 0x0000F6F3 File Offset: 0x0000D8F3
	public int currentScore { get; private set; }

	// Token: 0x060002EE RID: 750 RVA: 0x0000F6FC File Offset: 0x0000D8FC
	private void Awake()
	{
		this.scoreIndicatorAnimator.Play("Indicate", -1, 1f);
	}

	// Token: 0x060002EF RID: 751 RVA: 0x0000F714 File Offset: 0x0000D914
	private void Start()
	{
		this.SetScore(this.currentScore);
	}

	// Token: 0x060002F0 RID: 752 RVA: 0x0000F724 File Offset: 0x0000D924
	public void Reinitialize()
	{
		Image[] array = this.lifeIndicator;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].gameObject.SetActive(false);
		}
		this.SetScore(0);
		this.livesLost = 0;
	}

	// Token: 0x060002F1 RID: 753 RVA: 0x0000F762 File Offset: 0x0000D962
	public void HideScore()
	{
		this.scoreTmp.gameObject.SetActive(false);
	}

	// Token: 0x060002F2 RID: 754 RVA: 0x0000F775 File Offset: 0x0000D975
	public void AddScore(int points)
	{
		if (!this.adderActive)
		{
			this.adderActive = true;
			base.StartCoroutine(this.Adder(this.currentScore));
		}
		this.currentScore = points;
	}

	// Token: 0x060002F3 RID: 755 RVA: 0x0000F7A0 File Offset: 0x0000D9A0
	public void SetScore(int points)
	{
		this.scoreTmp.text = string.Format(Singleton<LocalizationManager>.Instance.GetLocalizedText("Mgm_Score"), points);
		base.StopAllCoroutines();
		this.currentScore = points;
	}

	// Token: 0x060002F4 RID: 756 RVA: 0x0000F7D4 File Offset: 0x0000D9D4
	public void IndicateScore(int value, Vector2 position)
	{
		this.IndicateText(string.Format("+{0}", value), position, Color.green);
	}

	// Token: 0x060002F5 RID: 757 RVA: 0x0000F7F4 File Offset: 0x0000D9F4
	public void IndicateText(string text, Vector2 position, Color color)
	{
		position.x = Mathf.Round(position.x);
		position.y = Mathf.Round(position.y);
		this.scoreIndicatorBase.anchoredPosition = position;
		this.scoreIndicatorTmp.text = text;
		this.scoreIndicatorTmp.color = color;
		this.scoreIndicatorAnimator.Play("Indicate", -1, 0f);
	}

	// Token: 0x060002F6 RID: 758 RVA: 0x0000F85F File Offset: 0x0000DA5F
	private IEnumerator Adder(int value)
	{
		yield return null;
		while (value != this.currentScore)
		{
			int num = 5;
			value = Mathf.Max(value - num, Mathf.Min(value + num, this.currentScore));
			this.scoreTmp.text = string.Format(Singleton<LocalizationManager>.Instance.GetLocalizedText("Mgm_Score"), value);
			yield return null;
		}
		this.adderActive = false;
		yield break;
	}

	// Token: 0x060002F7 RID: 759 RVA: 0x0000F875 File Offset: 0x0000DA75
	public void LoseLife()
	{
		this.lifeIndicator[this.livesLost].gameObject.SetActive(true);
		this.livesLost++;
	}

	// Token: 0x060002F8 RID: 760 RVA: 0x0000F89D File Offset: 0x0000DA9D
	public void LoseLife(Sprite indicatorImage)
	{
		this.lifeIndicator[this.livesLost].sprite = indicatorImage;
		this.LoseLife();
	}

	// Token: 0x04000314 RID: 788
	[SerializeField]
	private TMP_Text scoreTmp;

	// Token: 0x04000315 RID: 789
	[SerializeField]
	private TMP_Text scoreIndicatorTmp;

	// Token: 0x04000316 RID: 790
	[SerializeField]
	private Animator scoreIndicatorAnimator;

	// Token: 0x04000317 RID: 791
	[SerializeField]
	private Image[] lifeIndicator = new Image[3];

	// Token: 0x04000318 RID: 792
	[SerializeField]
	private RectTransform scoreIndicatorBase;

	// Token: 0x0400031B RID: 795
	private bool adderActive;
}
