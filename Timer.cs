using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000140 RID: 320
public class Timer : MonoBehaviour
{
	// Token: 0x0600076C RID: 1900 RVA: 0x00026023 File Offset: 0x00024223
	private void Start()
	{
		if (this.beginOnStart)
		{
			this.StartTimer(this.startTime);
		}
	}

	// Token: 0x0600076D RID: 1901 RVA: 0x00026039 File Offset: 0x00024239
	public void StartTimer(float time)
	{
		this.time = time;
		base.StopAllCoroutines();
		base.StartCoroutine(this.TimerRunner());
	}

	// Token: 0x0600076E RID: 1902 RVA: 0x00026055 File Offset: 0x00024255
	private IEnumerator TimerRunner()
	{
		while (this.time > 0f)
		{
			if (!this.ignoreTimeScale)
			{
				this.time -= Time.deltaTime;
			}
			else
			{
				this.time -= Time.unscaledDeltaTime;
			}
			yield return null;
		}
		this.OnExpired.Invoke();
		yield break;
	}

	// Token: 0x04000821 RID: 2081
	private float time;

	// Token: 0x04000822 RID: 2082
	public float startTime;

	// Token: 0x04000823 RID: 2083
	public bool beginOnStart;

	// Token: 0x04000824 RID: 2084
	public bool ignoreTimeScale;

	// Token: 0x04000825 RID: 2085
	public UnityEvent OnExpired;
}
