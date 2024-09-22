using System;
using TMPro;
using UnityEngine;

// Token: 0x020001B7 RID: 439
[Serializable]
public class PosterTextData
{
	// Token: 0x060009E2 RID: 2530 RVA: 0x00035334 File Offset: 0x00033534
	public PosterTextData()
	{
		this.textKey = "pst_key";
		this.position = new IntVector2(32, 32);
		this.size = new IntVector2(192, 192);
		this.fontSize = 0;
		this.color = Color.white;
		this.style = FontStyles.Normal;
		this.alignment = TextAlignmentOptions.Center;
	}

	// Token: 0x04000B34 RID: 2868
	public string textKey;

	// Token: 0x04000B35 RID: 2869
	public IntVector2 position;

	// Token: 0x04000B36 RID: 2870
	public IntVector2 size;

	// Token: 0x04000B37 RID: 2871
	public TMP_FontAsset font;

	// Token: 0x04000B38 RID: 2872
	public int fontSize;

	// Token: 0x04000B39 RID: 2873
	public Color color;

	// Token: 0x04000B3A RID: 2874
	public FontStyles style;

	// Token: 0x04000B3B RID: 2875
	public TextAlignmentOptions alignment;
}
