using System;
using UnityEngine;

// Token: 0x0200015C RID: 348
public class BalloonSpawner : RoomFunction
{
	// Token: 0x060007A7 RID: 1959 RVA: 0x00026AFC File Offset: 0x00024CFC
	public override void Initialize(RoomController room)
	{
		int num = Random.Range(this.min, this.max);
		for (int i = 0; i < num; i++)
		{
			Object.Instantiate<Balloon>(this.balloonPre[Random.Range(0, this.balloonPre.Length)], base.transform).Initialize(room);
		}
	}

	// Token: 0x0400084E RID: 2126
	[SerializeField]
	private Balloon[] balloonPre = new Balloon[0];

	// Token: 0x0400084F RID: 2127
	[SerializeField]
	private int min = 4;

	// Token: 0x04000850 RID: 2128
	[SerializeField]
	private int max = 8;
}
