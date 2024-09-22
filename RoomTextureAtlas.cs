using System;
using UnityEngine;

// Token: 0x020001B8 RID: 440
public class RoomTextureAtlas
{
	// Token: 0x060009E3 RID: 2531 RVA: 0x0003539A File Offset: 0x0003359A
	public RoomTextureAtlas(Texture2D floor, Texture2D wall, Texture2D ceiling, Texture2D atlas)
	{
		this.floor = floor;
		this.wall = wall;
		this.ceiling = ceiling;
		this.atlas = atlas;
	}

	// Token: 0x04000B3C RID: 2876
	public Texture2D floor;

	// Token: 0x04000B3D RID: 2877
	public Texture2D wall;

	// Token: 0x04000B3E RID: 2878
	public Texture2D ceiling;

	// Token: 0x04000B3F RID: 2879
	public Texture2D atlas;
}
