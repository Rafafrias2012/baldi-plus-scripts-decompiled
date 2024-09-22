using System;
using TMPro;
using UnityEngine;

// Token: 0x020001DF RID: 479
public class TooltipController : MonoBehaviour
{
	// Token: 0x06000ADF RID: 2783 RVA: 0x00039680 File Offset: 0x00037880
	private void Awake()
	{
		this.tooltipTmp.autoSizeTextContainer = true;
		this.UpdateTooltip("Tooltip");
		this.CloseTooltip();
	}

	// Token: 0x06000AE0 RID: 2784 RVA: 0x000396A0 File Offset: 0x000378A0
	private void Update()
	{
		if (this.useCursorPosition)
		{
			this._localPosition = CursorController.Instance.LocalPosition;
		}
		else
		{
			this._localPosition = this.screenPosition;
		}
		if (this.useCursorPosition)
		{
			this._localPosition.x = Mathf.Clamp(this._localPosition.x, this.xMin + this.tooltipBgRect.sizeDelta.x / 2f, this.xMax - this.tooltipBgRect.sizeDelta.x / 2f);
			this._localPosition.y = this._localPosition.y + (this.tooltipTmp.rectTransform.sizeDelta.y / 2f + (float)this.yBuffer + 8f);
		}
		this.tooltipRect.localPosition = this._localPosition;
	}

	// Token: 0x06000AE1 RID: 2785 RVA: 0x0003977C File Offset: 0x0003797C
	public void UpdateTooltip(string key)
	{
		this.stayOnCount++;
		this.ActualUpdateTooltip(key);
		this.ActualUpdateTooltip(key);
	}

	// Token: 0x06000AE2 RID: 2786 RVA: 0x0003979C File Offset: 0x0003799C
	public void ActualUpdateTooltip(string key)
	{
		this.tooltipTmp.text = Singleton<LocalizationManager>.Instance.GetLocalizedText(key);
		this.tooltipTmp.rectTransform.sizeDelta = this.tooltipTmp.GetPreferredValues();
		this._sizeDelta = this.tooltipTmp.rectTransform.sizeDelta;
		this._sizeDelta.x = this._sizeDelta.x + (float)this.xBuffer;
		this._sizeDelta.y = this._sizeDelta.y + (float)this.yBuffer;
		this.tooltipBgRect.sizeDelta = this._sizeDelta;
		this.tooltipTmp.gameObject.SetActive(true);
		this.tooltipBgRect.gameObject.SetActive(true);
	}

	// Token: 0x06000AE3 RID: 2787 RVA: 0x0003984F File Offset: 0x00037A4F
	public void SetPosition(Vector2 position)
	{
		this.screenPosition.x = position.x;
		this.screenPosition.y = position.y;
	}

	// Token: 0x06000AE4 RID: 2788 RVA: 0x00039874 File Offset: 0x00037A74
	public void CloseTooltip()
	{
		this.stayOnCount--;
		if (this.stayOnCount <= 0)
		{
			this.stayOnCount = 0;
			this.tooltipTmp.gameObject.SetActive(false);
			this.tooltipBgRect.gameObject.SetActive(false);
		}
	}

	// Token: 0x04000C6F RID: 3183
	[SerializeField]
	private TMP_Text tooltipTmp;

	// Token: 0x04000C70 RID: 3184
	[SerializeField]
	private RectTransform tooltipRect;

	// Token: 0x04000C71 RID: 3185
	[SerializeField]
	private RectTransform tooltipBgRect;

	// Token: 0x04000C72 RID: 3186
	[SerializeField]
	private Vector3 screenPosition;

	// Token: 0x04000C73 RID: 3187
	private Vector3 _localPosition;

	// Token: 0x04000C74 RID: 3188
	private Vector2 _sizeDelta;

	// Token: 0x04000C75 RID: 3189
	[SerializeField]
	private float xMin;

	// Token: 0x04000C76 RID: 3190
	[SerializeField]
	private float xMax;

	// Token: 0x04000C77 RID: 3191
	[SerializeField]
	private int xBuffer = 6;

	// Token: 0x04000C78 RID: 3192
	[SerializeField]
	private int yBuffer = 4;

	// Token: 0x04000C79 RID: 3193
	private int stayOnCount;

	// Token: 0x04000C7A RID: 3194
	[SerializeField]
	private bool useCursorPosition = true;
}
