using System;
using UnityEngine;

// Token: 0x020000D1 RID: 209
public class DirIntV2GarbageTest : MonoBehaviour
{
	// Token: 0x060004EB RID: 1259 RVA: 0x000197C4 File Offset: 0x000179C4
	private void Update()
	{
		for (int i = 0; i < 4; i++)
		{
			this.test = Directions.Vectors[i];
		}
	}

	// Token: 0x0400053E RID: 1342
	public IntVector2 test;
}
