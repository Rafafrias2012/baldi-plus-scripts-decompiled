using System;
using TMPro;
using UnityEngine;

// Token: 0x0200013B RID: 315
public class TextLocalizer : MonoBehaviour
{
	// Token: 0x0600075D RID: 1885 RVA: 0x00025E5D File Offset: 0x0002405D
	private void Awake()
	{
		this.textBox = base.GetComponent<TMP_Text>();
	}

	// Token: 0x0600075E RID: 1886 RVA: 0x00025E6B File Offset: 0x0002406B
	private void Start()
	{
		if ((!this.onlySetIfBlank || this.textBox.text.Length == 0) && this.key != "")
		{
			this.GetLocalizedText(this.key);
		}
	}

	// Token: 0x0600075F RID: 1887 RVA: 0x00025EA5 File Offset: 0x000240A5
	public void GetLocalizedText(string key)
	{
		this.textBox.text = Singleton<LocalizationManager>.Instance.GetLocalizedText(key, this.encrypted);
	}

	// Token: 0x04000813 RID: 2067
	private TMP_Text textBox;

	// Token: 0x04000814 RID: 2068
	public string key;

	// Token: 0x04000815 RID: 2069
	public bool encrypted;

	// Token: 0x04000816 RID: 2070
	public bool onlySetIfBlank;
}
