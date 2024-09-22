using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000069 RID: 105
public class DemoWinScene : MonoBehaviour
{
	// Token: 0x0600024D RID: 589 RVA: 0x0000CABA File Offset: 0x0000ACBA
	private void Update()
	{
		if (!this.source.isPlaying)
		{
			this.animator.enabled = false;
			this.image.sprite = this.sprite;
			base.StartCoroutine(this.Delay());
		}
	}

	// Token: 0x0600024E RID: 590 RVA: 0x0000CAF3 File Offset: 0x0000ACF3
	private IEnumerator Delay()
	{
		yield return null;
		Application.Quit();
		yield break;
	}

	// Token: 0x04000255 RID: 597
	public AudioSource source;

	// Token: 0x04000256 RID: 598
	public Animator animator;

	// Token: 0x04000257 RID: 599
	public Image image;

	// Token: 0x04000258 RID: 600
	public Sprite sprite;
}
