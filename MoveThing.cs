using System;
using UnityEngine;

// Token: 0x020000D6 RID: 214
public class MoveThing : MonoBehaviour
{
	// Token: 0x060004F8 RID: 1272 RVA: 0x00019B38 File Offset: 0x00017D38
	private void Update()
	{
		base.transform.position = base.transform.position + Vector3.forward * this.distance + Vector3.right * (this.distance / 10f);
	}

	// Token: 0x04000542 RID: 1346
	public float distance;
}
