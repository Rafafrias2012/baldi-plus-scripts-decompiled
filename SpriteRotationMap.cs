using System;
using UnityEngine;

// Token: 0x020000E9 RID: 233
[Serializable]
public class SpriteRotationMap
{
	// Token: 0x0600056C RID: 1388 RVA: 0x0001BAA0 File Offset: 0x00019CA0
	public Sprite Sprite(int id)
	{
		return this.spriteSheet[id];
	}

	// Token: 0x1700006A RID: 106
	// (get) Token: 0x0600056D RID: 1389 RVA: 0x0001BAAA File Offset: 0x00019CAA
	public int SpriteCount
	{
		get
		{
			return this.spriteSheet.Length;
		}
	}

	// Token: 0x040005A5 RID: 1445
	public int angleCount = 8;

	// Token: 0x040005A6 RID: 1446
	[SerializeField]
	private Sprite[] spriteSheet = new Sprite[0];
}
