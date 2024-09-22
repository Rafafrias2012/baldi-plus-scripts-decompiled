using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200015B RID: 347
public class BaldiSpeedIncreaser : MonoBehaviour
{
	// Token: 0x060007A3 RID: 1955 RVA: 0x00026A9F File Offset: 0x00024C9F
	public void Initialize(EnvironmentController ec)
	{
		this.ec = ec;
		base.StartCoroutine(this.BaldiWait());
	}

	// Token: 0x060007A4 RID: 1956 RVA: 0x00026AB5 File Offset: 0x00024CB5
	private IEnumerator BaldiWait()
	{
		while (this.ec.GetBaldi() == null)
		{
			yield return null;
		}
		base.StartCoroutine(this.SpeedIncreaser());
		yield break;
	}

	// Token: 0x060007A5 RID: 1957 RVA: 0x00026AC4 File Offset: 0x00024CC4
	private IEnumerator SpeedIncreaser()
	{
		float time = this.speedIncreaseRate;
		for (;;)
		{
			if (time <= 0f)
			{
				time = this.speedIncreaseRate;
				Singleton<BaseGameManager>.Instance.AngerBaldi(this.angerRate);
				this.angerRate += this.angerRateRate;
			}
			else
			{
				time -= Time.deltaTime * this.ec.NpcTimeScale;
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x0400084A RID: 2122
	private EnvironmentController ec;

	// Token: 0x0400084B RID: 2123
	[SerializeField]
	private float speedIncreaseRate = 1f;

	// Token: 0x0400084C RID: 2124
	[SerializeField]
	private float angerRate = 0.01f;

	// Token: 0x0400084D RID: 2125
	[SerializeField]
	private float angerRateRate = 0.00025f;
}
