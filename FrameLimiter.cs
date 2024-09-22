using System;
using UnityEngine;

// Token: 0x020000D2 RID: 210
public class FrameLimiter : MonoBehaviour
{
	// Token: 0x060004ED RID: 1261 RVA: 0x000197F8 File Offset: 0x000179F8
	private void Awake()
	{
		Debug.Log("MaxDeltaTime = " + Time.maximumDeltaTime.ToString());
		if (this.on)
		{
			Application.targetFrameRate = this.rate;
		}
	}

	// Token: 0x0400053F RID: 1343
	public int rate;

	// Token: 0x04000540 RID: 1344
	public bool on;
}
