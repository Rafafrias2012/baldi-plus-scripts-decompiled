using System;
using TMPro;
using UnityEngine;

// Token: 0x02000214 RID: 532
public class PauseReset : MonoBehaviour
{
	// Token: 0x06000BE5 RID: 3045 RVA: 0x0003ED20 File Offset: 0x0003CF20
	private void OnEnable()
	{
		GameObject[] array = this.main;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(true);
		}
		this.seedText.text = Singleton<CoreGameManager>.Instance.Seed().ToString();
		array = this.close;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(false);
		}
	}

	// Token: 0x04000E6F RID: 3695
	[SerializeField]
	private GameObject[] main = new GameObject[0];

	// Token: 0x04000E70 RID: 3696
	[SerializeField]
	private GameObject[] close = new GameObject[0];

	// Token: 0x04000E71 RID: 3697
	[SerializeField]
	private TMP_Text seedText;
}
