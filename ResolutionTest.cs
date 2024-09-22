using System;
using UnityEngine;

// Token: 0x020000B8 RID: 184
public class ResolutionTest : MonoBehaviour
{
	// Token: 0x06000435 RID: 1077 RVA: 0x0001650D File Offset: 0x0001470D
	private void Start()
	{
		Screen.SetResolution(480, 360, FullScreenMode.Windowed, 60);
	}
}
