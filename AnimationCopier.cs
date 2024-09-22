using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000203 RID: 515
public class AnimationCopier : MonoBehaviour
{
	// Token: 0x06000B6E RID: 2926 RVA: 0x0003C200 File Offset: 0x0003A400
	private void LateUpdate()
	{
		Image[] array = this.target;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].sprite = this.source.sprite;
		}
	}

	// Token: 0x04000DBC RID: 3516
	[SerializeField]
	private Image source;

	// Token: 0x04000DBD RID: 3517
	[SerializeField]
	private Image[] target;
}
