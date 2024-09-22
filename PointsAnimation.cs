using System;
using System.Collections;
using TMPro;
using UnityEngine;

// Token: 0x020000B4 RID: 180
public class PointsAnimation : MonoBehaviour
{
	// Token: 0x06000406 RID: 1030 RVA: 0x00015358 File Offset: 0x00013558
	private void Awake()
	{
		this.totalTmp.text = Singleton<CoreGameManager>.Instance.GetPoints(0).ToString();
	}

	// Token: 0x06000407 RID: 1031 RVA: 0x00015384 File Offset: 0x00013584
	public void AddScore(int points, int current, int player)
	{
		if (!this.additionVisible)
		{
			this.additionValue = 0;
		}
		this.additionVisible = true;
		this.additionValue += points;
		if (this.additionValue > 0)
		{
			this.additionTmp.color = Color.green;
			this.additionTmp.text = "+" + this.additionValue.ToString();
		}
		else
		{
			this.additionTmp.color = Color.red;
			this.additionTmp.text = this.additionValue.ToString();
		}
		this.additionAnimator.SetTrigger("Add");
		if (!this.adderActive)
		{
			this.adderActive = true;
			this.ShowDisplay(true);
			base.StartCoroutine(this.Adder(current));
		}
	}

	// Token: 0x06000408 RID: 1032 RVA: 0x0001544A File Offset: 0x0001364A
	public void UpdateScore(int points)
	{
		if (this.displayShows <= 0)
		{
			this.totalTmp.text = points.ToString();
		}
	}

	// Token: 0x06000409 RID: 1033 RVA: 0x00015468 File Offset: 0x00013668
	public void ShowDisplay(bool val)
	{
		if (val)
		{
			this.displayAnimator.SetBool("Adding", true);
			this.displayShows++;
			return;
		}
		this.displayShows--;
		if (this.displayShows <= 0)
		{
			this.displayAnimator.SetBool("Adding", false);
		}
	}

	// Token: 0x0600040A RID: 1034 RVA: 0x000154C0 File Offset: 0x000136C0
	private IEnumerator Adder(int value)
	{
		while (this.additionVisible)
		{
			yield return null;
		}
		while (value != Singleton<CoreGameManager>.Instance.GetPoints(0))
		{
			int num = 5;
			value = Mathf.Max(value - num, Mathf.Min(value + num, Singleton<CoreGameManager>.Instance.GetPoints(0)));
			this.totalTmp.text = value.ToString();
			yield return null;
		}
		float time = 1f;
		while (time > 0f)
		{
			time -= Time.deltaTime;
			yield return null;
		}
		if (!this.additionVisible && value == Singleton<CoreGameManager>.Instance.GetPoints(0))
		{
			this.ShowDisplay(false);
			this.adderActive = false;
		}
		else
		{
			base.StartCoroutine(this.Adder(value));
		}
		yield break;
	}

	// Token: 0x0600040B RID: 1035 RVA: 0x000154D6 File Offset: 0x000136D6
	public void AdditionFinished()
	{
		this.additionVisible = false;
		if (this.additionValue > 0)
		{
			Singleton<CoreGameManager>.Instance.audMan.PlaySingle(this.audCashregister);
		}
	}

	// Token: 0x04000446 RID: 1094
	public Animator additionAnimator;

	// Token: 0x04000447 RID: 1095
	public Animator displayAnimator;

	// Token: 0x04000448 RID: 1096
	public TMP_Text totalTmp;

	// Token: 0x04000449 RID: 1097
	public TMP_Text additionTmp;

	// Token: 0x0400044A RID: 1098
	[SerializeField]
	private SoundObject audCashregister;

	// Token: 0x0400044B RID: 1099
	private int additionValue;

	// Token: 0x0400044C RID: 1100
	private int currentValue;

	// Token: 0x0400044D RID: 1101
	private int totalValue;

	// Token: 0x0400044E RID: 1102
	private int displayShows;

	// Token: 0x0400044F RID: 1103
	private bool additionVisible;

	// Token: 0x04000450 RID: 1104
	private bool adderActive;
}
