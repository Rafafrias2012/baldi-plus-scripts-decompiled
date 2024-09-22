using System;
using UnityEngine;

// Token: 0x020001D6 RID: 470
public class MainModeButtonController : MonoBehaviour
{
	// Token: 0x06000AA5 RID: 2725 RVA: 0x00038348 File Offset: 0x00036548
	private void OnEnable()
	{
		if (Singleton<PlayerFileManager>.Instance.savedGameData.saveAvailable)
		{
			this.mainNew.SetActive(false);
			this.mainContinue.SetActive(true);
		}
	}

	// Token: 0x04000C31 RID: 3121
	public GameObject mainNew;

	// Token: 0x04000C32 RID: 3122
	public GameObject mainContinue;
}
