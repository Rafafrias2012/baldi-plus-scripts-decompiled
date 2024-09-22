using System;
using UnityEngine;

// Token: 0x02000194 RID: 404
public class MapTile : MonoBehaviour
{
	// Token: 0x06000930 RID: 2352 RVA: 0x0003103F File Offset: 0x0002F23F
	public void Reveal()
	{
		this.found = true;
		base.gameObject.SetActive(true);
	}

	// Token: 0x170000C7 RID: 199
	// (get) Token: 0x06000931 RID: 2353 RVA: 0x00031054 File Offset: 0x0002F254
	public SpriteRenderer SpriteRenderer
	{
		get
		{
			return this.spriteRenderer;
		}
	}

	// Token: 0x170000C8 RID: 200
	// (get) Token: 0x06000932 RID: 2354 RVA: 0x0003105C File Offset: 0x0002F25C
	public bool Found
	{
		get
		{
			return this.found;
		}
	}

	// Token: 0x04000A21 RID: 2593
	public IntVector2 position;

	// Token: 0x04000A22 RID: 2594
	[SerializeField]
	private SpriteRenderer spriteRenderer;

	// Token: 0x04000A23 RID: 2595
	private bool found;
}
