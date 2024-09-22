using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000141 RID: 321
public class UIInvert : MonoBehaviour
{
	// Token: 0x06000770 RID: 1904 RVA: 0x0002606C File Offset: 0x0002426C
	public void InvertColors(bool invert)
	{
		if (invert)
		{
			this.image.material.SetInt("_InvertColors", 1);
			return;
		}
		this.image.material.SetInt("_InvertColors", 0);
	}

	// Token: 0x04000826 RID: 2086
	public Image image;
}
