using System;
using TMPro;
using UnityEngine;

// Token: 0x020001D9 RID: 473
public class NameButton : MonoBehaviour
{
	// Token: 0x06000AAF RID: 2735 RVA: 0x000383FF File Offset: 0x000365FF
	private void Start()
	{
		this.UpdateState();
	}

	// Token: 0x06000AB0 RID: 2736 RVA: 0x00038407 File Offset: 0x00036607
	public void UpdateState()
	{
		this.text.text = NameManager.nm.nameList[this.fileNo];
	}

	// Token: 0x06000AB1 RID: 2737 RVA: 0x00038425 File Offset: 0x00036625
	public void Highlight()
	{
		this.text.color = Color.green;
	}

	// Token: 0x06000AB2 RID: 2738 RVA: 0x00038437 File Offset: 0x00036637
	public void Unhighlight()
	{
		this.text.color = Color.black;
	}

	// Token: 0x04000C3D RID: 3133
	public int fileNo;

	// Token: 0x04000C3E RID: 3134
	public TMP_Text text;
}
