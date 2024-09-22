using System;
using UnityEngine;

// Token: 0x020000DC RID: 220
public class TileDebugScript : MonoBehaviour
{
	// Token: 0x06000501 RID: 1281 RVA: 0x00019E77 File Offset: 0x00018077
	public void AddPoster()
	{
	}

	// Token: 0x06000502 RID: 1282 RVA: 0x00019E79 File Offset: 0x00018079
	private void OnGUI()
	{
		if (GUI.Button(new Rect(200f, 100f, 100f, 30f), "Add Poster"))
		{
			this.AddPoster();
		}
	}

	// Token: 0x04000548 RID: 1352
	public Material poster;

	// Token: 0x04000549 RID: 1353
	public Material windowMask;

	// Token: 0x0400054A RID: 1354
	public Material windowCover;

	// Token: 0x0400054B RID: 1355
	public Cell tile;
}
