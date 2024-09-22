using System;
using UnityEngine;

// Token: 0x0200020B RID: 523
public class GlobalCamCanvasAssigner : MonoBehaviour
{
	// Token: 0x06000B92 RID: 2962 RVA: 0x0003CAC3 File Offset: 0x0003ACC3
	private void Awake()
	{
		this.canvas.worldCamera = Singleton<GlobalCam>.Instance.Cam;
		this.canvas.planeDistance = this.planeDistance;
	}

	// Token: 0x04000DE6 RID: 3558
	[SerializeField]
	private Canvas canvas;

	// Token: 0x04000DE7 RID: 3559
	[SerializeField]
	private float planeDistance = 0.31f;
}
