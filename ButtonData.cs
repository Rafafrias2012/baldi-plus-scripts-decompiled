using System;
using System.Collections.Generic;

// Token: 0x02000182 RID: 386
[Serializable]
public class ButtonData
{
	// Token: 0x0400099C RID: 2460
	public GameButtonBase prefab;

	// Token: 0x0400099D RID: 2461
	public IntVector2 position;

	// Token: 0x0400099E RID: 2462
	public Direction direction;

	// Token: 0x0400099F RID: 2463
	public List<ButtonReceiverData> receivers = new List<ButtonReceiverData>();
}
