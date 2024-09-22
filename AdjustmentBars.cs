using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000202 RID: 514
public class AdjustmentBars : MonoBehaviour
{
	// Token: 0x06000B68 RID: 2920 RVA: 0x0003C0BF File Offset: 0x0003A2BF
	private void Awake()
	{
		this.UpdateBars();
	}

	// Token: 0x06000B69 RID: 2921 RVA: 0x0003C0C8 File Offset: 0x0003A2C8
	public void Adjust(int dir)
	{
		this.val += dir;
		if (this.val > this.bars.Length)
		{
			this.val = this.bars.Length;
		}
		else if (this.val < 0)
		{
			this.val = 0;
		}
		this.UpdateBars();
		this.onValueChanged.Invoke();
	}

	// Token: 0x06000B6A RID: 2922 RVA: 0x0003C124 File Offset: 0x0003A324
	public void SetVal(float setVal)
	{
		for (int i = 0; i <= this.bars.Length; i++)
		{
			this.val = i;
			if (this.FloatVal >= setVal)
			{
				break;
			}
		}
		this.UpdateBars();
	}

	// Token: 0x06000B6B RID: 2923 RVA: 0x0003C15C File Offset: 0x0003A35C
	private void UpdateBars()
	{
		for (int i = 0; i < this.bars.Length; i++)
		{
			if (i < this.val)
			{
				this.bars[i].sprite = this.highlighted;
			}
			else
			{
				this.bars[i].sprite = this.unhighlighted;
			}
		}
	}

	// Token: 0x170000F3 RID: 243
	// (get) Token: 0x06000B6C RID: 2924 RVA: 0x0003C1AD File Offset: 0x0003A3AD
	public float FloatVal
	{
		get
		{
			return (float)Math.Round((double)this.valueCurve.Evaluate((float)this.val / (float)this.bars.Length), this.decimals);
		}
	}

	// Token: 0x04000DB5 RID: 3509
	public UnityEvent onValueChanged;

	// Token: 0x04000DB6 RID: 3510
	[SerializeField]
	private Image[] bars = new Image[0];

	// Token: 0x04000DB7 RID: 3511
	[SerializeField]
	private Sprite highlighted;

	// Token: 0x04000DB8 RID: 3512
	[SerializeField]
	private Sprite unhighlighted;

	// Token: 0x04000DB9 RID: 3513
	[SerializeField]
	private AnimationCurve valueCurve = new AnimationCurve();

	// Token: 0x04000DBA RID: 3514
	[SerializeField]
	private int decimals = 1;

	// Token: 0x04000DBB RID: 3515
	private int val;
}
