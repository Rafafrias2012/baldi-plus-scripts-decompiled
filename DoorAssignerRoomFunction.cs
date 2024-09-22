using System;
using UnityEngine;

// Token: 0x020001A9 RID: 425
public class DoorAssignerRoomFunction : RoomFunction
{
	// Token: 0x060009A6 RID: 2470 RVA: 0x0003378C File Offset: 0x0003198C
	public override void Initialize(RoomController room)
	{
		base.Initialize(room);
		room.doorPre = this.doorPre;
	}

	// Token: 0x04000AD7 RID: 2775
	[SerializeField]
	private Door doorPre;
}
