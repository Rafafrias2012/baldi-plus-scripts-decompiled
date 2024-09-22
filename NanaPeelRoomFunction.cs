using System;
using UnityEngine;

// Token: 0x020001AD RID: 429
public class NanaPeelRoomFunction : RoomFunction
{
	// Token: 0x060009B9 RID: 2489 RVA: 0x00033EC8 File Offset: 0x000320C8
	public override void OnGenerationFinished()
	{
		base.OnGenerationFinished();
		int num = Random.Range(this.minBananas, this.maxBananas + 1);
		for (int i = 0; i < num; i++)
		{
			Object.Instantiate<ITM_NanaPeel>(this.bananaPrefab).Spawn(this.room.ec, this.room.ec.RealRoomRand(this.room), Vector3.forward, 0f);
		}
	}

	// Token: 0x04000AF5 RID: 2805
	[SerializeField]
	private ITM_NanaPeel bananaPrefab;

	// Token: 0x04000AF6 RID: 2806
	[SerializeField]
	private int minBananas;

	// Token: 0x04000AF7 RID: 2807
	[SerializeField]
	private int maxBananas = 4;
}
