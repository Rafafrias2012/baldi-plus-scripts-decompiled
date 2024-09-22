using System;
using UnityEngine;

// Token: 0x020000F0 RID: 240
public class BillboardManager : MonoBehaviour
{
	// Token: 0x06000599 RID: 1433 RVA: 0x0001C4A0 File Offset: 0x0001A6A0
	private void LateUpdate()
	{
		if (this.camMain == null && Camera.main != null)
		{
			this.camMain = Camera.main;
		}
		if (this.camMain != null && this.camMain.transform.parent != null)
		{
			BillboardUpdater.camRot.eulerAngles = Vector3.up * this.camMain.transform.parent.rotation.eulerAngles.y + Vector3.right * this.camMain.transform.parent.rotation.eulerAngles.x;
		}
	}

	// Token: 0x040005CB RID: 1483
	private Camera cam;

	// Token: 0x040005CC RID: 1484
	private Camera camMain;
}
