using System;
using UnityEngine;

// Token: 0x020001B3 RID: 435
public class SunlightRoomFunction : RoomFunction
{
	// Token: 0x060009D9 RID: 2521 RVA: 0x00034E70 File Offset: 0x00033070
	public override void Initialize(RoomController room)
	{
		base.Initialize(room);
		for (int i = 0; i < room.TileCount; i++)
		{
			room.TileAtIndex(i).permanentLight = true;
			room.ec.GenerateLight(room.TileAtIndex(i), this.color, 1);
		}
	}

	// Token: 0x04000B28 RID: 2856
	public Color color = new Color(1f, 1f, 1f, 1f);
}
