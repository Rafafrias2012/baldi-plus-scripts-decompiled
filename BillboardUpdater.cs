using System;
using UnityEngine;

// Token: 0x020000F1 RID: 241
public class BillboardUpdater : MonoBehaviour
{
	// Token: 0x0600059B RID: 1435 RVA: 0x0001C568 File Offset: 0x0001A768
	private void LateUpdate()
	{
		base.transform.LookAt(base.transform.position + BillboardUpdater.camRot * Vector3.forward, base.transform.parent.up);
	}

	// Token: 0x040005CD RID: 1485
	public static Quaternion camRot;
}
