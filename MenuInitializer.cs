using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001D8 RID: 472
public class MenuInitializer : MonoBehaviour
{
	// Token: 0x06000AAC RID: 2732 RVA: 0x0003839C File Offset: 0x0003659C
	private void Start()
	{
		if (Singleton<GlobalStateManager>.Instance.skipNameEntry)
		{
			this.mainMenu.SetActive(true);
			this.nameManager.Load();
		}
		else
		{
			this.nameEntry.SetActive(true);
		}
		base.StartCoroutine(this.LoadFrameDelay());
	}

	// Token: 0x06000AAD RID: 2733 RVA: 0x000383DC File Offset: 0x000365DC
	private IEnumerator LoadFrameDelay()
	{
		yield return null;
		Singleton<GlobalCam>.Instance.Transition(UiTransition.Dither, 0.01666667f);
		GameObject[] array = this.covers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(false);
		}
		yield break;
	}

	// Token: 0x04000C39 RID: 3129
	[SerializeField]
	private NameManager nameManager;

	// Token: 0x04000C3A RID: 3130
	[SerializeField]
	private GameObject nameEntry;

	// Token: 0x04000C3B RID: 3131
	[SerializeField]
	private GameObject mainMenu;

	// Token: 0x04000C3C RID: 3132
	[SerializeField]
	private GameObject[] covers = new GameObject[0];
}
