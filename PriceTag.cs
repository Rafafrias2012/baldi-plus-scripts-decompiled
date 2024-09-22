using System;
using TMPro;
using UnityEngine;

// Token: 0x020001E6 RID: 486
public class PriceTag : MonoBehaviour
{
	// Token: 0x06000AFE RID: 2814 RVA: 0x00039CCD File Offset: 0x00037ECD
	public void SetText(string val)
	{
		this.TMPtext.text = val;
	}

	// Token: 0x06000AFF RID: 2815 RVA: 0x00039CDB File Offset: 0x00037EDB
	public void SetSale(int basePrice, int salePrice)
	{
		this.TMPtext.text = string.Format("{0}\n <s> {1}  </s> {2}", Singleton<LocalizationManager>.Instance.GetLocalizedText("TAG_Sale"), basePrice.ToString(), salePrice.ToString());
	}

	// Token: 0x04000C90 RID: 3216
	[SerializeField]
	private TMP_Text TMPtext;
}
