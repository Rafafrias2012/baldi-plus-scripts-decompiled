using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200020D RID: 525
public class HudManager : MonoBehaviour
{
	// Token: 0x06000B96 RID: 2966 RVA: 0x0003CB2B File Offset: 0x0003AD2B
	private void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		this.SetStaminaNeedle();
		this.baldiTv.Initialize();
	}

	// Token: 0x06000B97 RID: 2967 RVA: 0x0003CB49 File Offset: 0x0003AD49
	private void Update()
	{
		this.UpdateScaleFactor();
		this.UpdateStaminaNeedle();
		this.UpdateHudColor();
	}

	// Token: 0x06000B98 RID: 2968 RVA: 0x0003CB5D File Offset: 0x0003AD5D
	public void ReInit()
	{
		this.baldiTv.ReInit();
		this.SetStaminaValue(1f);
		this.SetStaminaNeedle();
		this.SetNotebookDisplay(true);
		this.UpdateReticle(false);
	}

	// Token: 0x06000B99 RID: 2969 RVA: 0x0003CB8C File Offset: 0x0003AD8C
	public void UpdateScaleFactor()
	{
		if (!Singleton<PlayerFileManager>.Instance.authenticMode)
		{
			if ((float)Singleton<PlayerFileManager>.Instance.resolutionX / (float)Singleton<PlayerFileManager>.Instance.resolutionY >= 1.3333f)
			{
				this.canvasScaler.scaleFactor = (float)Mathf.RoundToInt((float)Singleton<PlayerFileManager>.Instance.resolutionY / 360f);
				return;
			}
			this.canvasScaler.scaleFactor = (float)Mathf.FloorToInt((float)Singleton<PlayerFileManager>.Instance.resolutionY / 480f);
		}
	}

	// Token: 0x06000B9A RID: 2970 RVA: 0x0003CC08 File Offset: 0x0003AE08
	public void SetStaminaValue(float val)
	{
		this.needleTargetValue = val;
	}

	// Token: 0x06000B9B RID: 2971 RVA: 0x0003CC14 File Offset: 0x0003AE14
	private void UpdateStaminaNeedle()
	{
		float num = this.needleSpeed * Time.deltaTime;
		this.needleValue = Mathf.Max(this.needleValue - num, Mathf.Min(this.needleValue + num, this.needleTargetValue));
		if (this.staminaNeedle != null)
		{
			this._pos = this.staminaNeedle.anchoredPosition;
			if (this.needleValue > 1f)
			{
				this._pos.x = (float)Mathf.RoundToInt((float)this.staminaMaxPos + Mathf.Min((float)(this.staminaOverPos - this.staminaMaxPos) * (this.needleValue - 1f), (float)this.staminaOverPos));
			}
			else
			{
				this._pos.x = (float)Mathf.RoundToInt((float)this.staminaMinPos + (float)(this.staminaMaxPos - this.staminaMinPos) * this.needleValue);
			}
			this.staminaNeedle.anchoredPosition = this._pos;
		}
	}

	// Token: 0x06000B9C RID: 2972 RVA: 0x0003CD10 File Offset: 0x0003AF10
	private void SetStaminaNeedle()
	{
		this.needleValue = this.needleTargetValue;
		if (this.staminaNeedle != null)
		{
			this._pos = this.staminaNeedle.anchoredPosition;
			if (this.needleValue > 1f)
			{
				this._pos.x = (float)Mathf.RoundToInt((float)this.staminaMaxPos + Mathf.Min((float)(this.staminaOverPos - this.staminaMaxPos) * (this.needleValue - 1f), (float)this.staminaOverPos));
			}
			else
			{
				this._pos.x = (float)Mathf.RoundToInt((float)this.staminaMinPos + (float)(this.staminaMaxPos - this.staminaMinPos) * this.needleValue);
			}
			this.staminaNeedle.anchoredPosition = this._pos;
		}
	}

	// Token: 0x06000B9D RID: 2973 RVA: 0x0003CDE4 File Offset: 0x0003AFE4
	public void UpdateInventorySize(int size)
	{
		Vector3 v = this.inventory.rectTransform.anchoredPosition;
		v.x = (float)(40 * (9 - size));
		this.inventory.rectTransform.anchoredPosition = v;
		this.inventory.SetSize(size);
	}

	// Token: 0x06000B9E RID: 2974 RVA: 0x0003CE38 File Offset: 0x0003B038
	public void SetItemSelect(int value, string key)
	{
		if (this.itemBackgrounds[value] != null)
		{
			this.itemBackgrounds[this.previousSelectedItem].color = Color.white;
			this.itemBackgrounds[value].color = Color.red;
			this.previousSelectedItem = value;
			if (this.itemTitle != null)
			{
				this.itemTitle.text = Singleton<LocalizationManager>.Instance.GetLocalizedText(key);
			}
		}
	}

	// Token: 0x06000B9F RID: 2975 RVA: 0x0003CEA9 File Offset: 0x0003B0A9
	public void UpdateItemIcon(int value, Sprite sprite)
	{
		if (this.itemSprites[value] != null)
		{
			this.itemSprites[value].sprite = sprite;
		}
	}

	// Token: 0x06000BA0 RID: 2976 RVA: 0x0003CECC File Offset: 0x0003B0CC
	public void UpdateReticle(bool active)
	{
		if (this.reticle != null)
		{
			if (active && this.reticle.sprite != this.retOn)
			{
				this.reticle.sprite = this.retOn;
				return;
			}
			if (this.reticle.sprite != this.retOff)
			{
				this.reticle.sprite = this.retOff;
			}
		}
	}

	// Token: 0x06000BA1 RID: 2977 RVA: 0x0003CF3D File Offset: 0x0003B13D
	public void UpdateNotebookText(int textVal, string text, bool spin)
	{
		if (textVal < this.textBox.Length)
		{
			this.textBox[textVal].text = text;
		}
		if (spin)
		{
			this.notebookAnimator.Play("NotebookSpin", -1, 0f);
		}
	}

	// Token: 0x06000BA2 RID: 2978 RVA: 0x0003CF74 File Offset: 0x0003B174
	public void SetNotebookDisplay(bool val)
	{
		GameObject[] array = this.notebookDisplay;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(val);
		}
	}

	// Token: 0x06000BA3 RID: 2979 RVA: 0x0003CF9F File Offset: 0x0003B19F
	public Canvas Canvas()
	{
		return this.canvas;
	}

	// Token: 0x06000BA4 RID: 2980 RVA: 0x0003CFA7 File Offset: 0x0003B1A7
	public void ActivateBaldicator(bool coming)
	{
		if (!this.hidden)
		{
			if (coming)
			{
				this.animator.Play("Baldicator_Look", -1, 0f);
				return;
			}
			this.animator.Play("Baldicator_Think", -1, 0f);
		}
	}

	// Token: 0x06000BA5 RID: 2981 RVA: 0x0003CFE1 File Offset: 0x0003B1E1
	public void Hide(bool val)
	{
		this.hidden = val;
		this.canvas.enabled = !val;
	}

	// Token: 0x06000BA6 RID: 2982 RVA: 0x0003CFF9 File Offset: 0x0003B1F9
	public void Darken(bool val)
	{
		if (val)
		{
			this.colorTargetValue = 0f;
			return;
		}
		this.colorTargetValue = 1f;
	}

	// Token: 0x06000BA7 RID: 2983 RVA: 0x0003D018 File Offset: 0x0003B218
	private void UpdateHudColor()
	{
		if (this.colorValue != this.colorTargetValue)
		{
			float num = Time.deltaTime * 2f;
			this.colorValue = Mathf.Max(this.colorValue - num, Mathf.Min(this.colorValue + num, this.colorTargetValue));
			Image[] array = this.spritesToDarken;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].color = Color.Lerp(this.darkColor, Color.white, this.colorValue - Mathf.Repeat(this.colorValue, 0.2f));
			}
		}
	}

	// Token: 0x06000BA8 RID: 2984 RVA: 0x0003D0A9 File Offset: 0x0003B2A9
	public void SetTooltip(string key)
	{
		this.tooltip.UpdateTooltip(key);
	}

	// Token: 0x06000BA9 RID: 2985 RVA: 0x0003D0B7 File Offset: 0x0003B2B7
	public void CloseTooltip()
	{
		this.tooltip.CloseTooltip();
	}

	// Token: 0x170000F4 RID: 244
	// (get) Token: 0x06000BAA RID: 2986 RVA: 0x0003D0C4 File Offset: 0x0003B2C4
	public PointsAnimation PointsAnimator
	{
		get
		{
			return this.pointsAnimator;
		}
	}

	// Token: 0x170000F5 RID: 245
	// (get) Token: 0x06000BAB RID: 2987 RVA: 0x0003D0CC File Offset: 0x0003B2CC
	public BaldiTV BaldiTv
	{
		get
		{
			return this.baldiTv;
		}
	}

	// Token: 0x04000DEC RID: 3564
	[SerializeField]
	private BaldiTV baldiTv;

	// Token: 0x04000DED RID: 3565
	[SerializeField]
	private Canvas canvas;

	// Token: 0x04000DEE RID: 3566
	[SerializeField]
	private CanvasScaler canvasScaler;

	// Token: 0x04000DEF RID: 3567
	[SerializeField]
	private RectTransform staminaNeedle;

	// Token: 0x04000DF0 RID: 3568
	[SerializeField]
	private int staminaMinPos = 25;

	// Token: 0x04000DF1 RID: 3569
	[SerializeField]
	private int staminaMaxPos = 158;

	// Token: 0x04000DF2 RID: 3570
	[SerializeField]
	private int staminaOverPos = 166;

	// Token: 0x04000DF3 RID: 3571
	[SerializeField]
	public ItemSlotsManager inventory;

	// Token: 0x04000DF4 RID: 3572
	[SerializeField]
	private RawImage[] itemBackgrounds;

	// Token: 0x04000DF5 RID: 3573
	[SerializeField]
	private Image[] itemSprites;

	// Token: 0x04000DF6 RID: 3574
	[SerializeField]
	private Image[] spritesToDarken = new Image[0];

	// Token: 0x04000DF7 RID: 3575
	[SerializeField]
	private Image reticle;

	// Token: 0x04000DF8 RID: 3576
	[SerializeField]
	private TMP_Text itemTitle;

	// Token: 0x04000DF9 RID: 3577
	[SerializeField]
	private TMP_Text[] textBox;

	// Token: 0x04000DFA RID: 3578
	[SerializeField]
	private Sprite retOff;

	// Token: 0x04000DFB RID: 3579
	[SerializeField]
	private Sprite retOn;

	// Token: 0x04000DFC RID: 3580
	[SerializeField]
	private Animator animator;

	// Token: 0x04000DFD RID: 3581
	[SerializeField]
	private Animator notebookAnimator;

	// Token: 0x04000DFE RID: 3582
	[SerializeField]
	private GameObject[] notebookDisplay = new GameObject[0];

	// Token: 0x04000DFF RID: 3583
	[SerializeField]
	private PointsAnimation pointsAnimator;

	// Token: 0x04000E00 RID: 3584
	[SerializeField]
	private TooltipController tooltip;

	// Token: 0x04000E01 RID: 3585
	private Vector3 _pos;

	// Token: 0x04000E02 RID: 3586
	[SerializeField]
	private Color darkColor = new Color(0.25f, 0.25f, 0.25f, 1f);

	// Token: 0x04000E03 RID: 3587
	[SerializeField]
	private float needleSpeed = 1f;

	// Token: 0x04000E04 RID: 3588
	private float needleValue;

	// Token: 0x04000E05 RID: 3589
	private float needleTargetValue = 1f;

	// Token: 0x04000E06 RID: 3590
	private float colorValue = 1f;

	// Token: 0x04000E07 RID: 3591
	private float colorTargetValue = 1f;

	// Token: 0x04000E08 RID: 3592
	public int hudNum;

	// Token: 0x04000E09 RID: 3593
	private int previousSelectedItem;

	// Token: 0x04000E0A RID: 3594
	private bool hidden;
}
