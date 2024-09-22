using System;
using UnityEngine;

// Token: 0x0200016D RID: 365
public class EnvironmentLightingLateUpdater : MonoBehaviour
{
	// Token: 0x060008B6 RID: 2230 RVA: 0x0002CB58 File Offset: 0x0002AD58
	private void LateUpdate()
	{
		this.ec.UpdateQueuedLightChanges();
		Singleton<CoreGameManager>.Instance.UpdateLightMap();
	}

	// Token: 0x04000936 RID: 2358
	[SerializeField]
	private EnvironmentController ec;
}
