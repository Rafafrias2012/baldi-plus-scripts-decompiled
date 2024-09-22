using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200015A RID: 346
public class Ambience : MonoBehaviour
{
	// Token: 0x060007A0 RID: 1952 RVA: 0x00026A28 File Offset: 0x00024C28
	public void Initialize(EnvironmentController ec)
	{
		this.ec = ec;
		base.StartCoroutine(this.Timer());
	}

	// Token: 0x060007A1 RID: 1953 RVA: 0x00026A3E File Offset: 0x00024C3E
	private IEnumerator Timer()
	{
		float time = Random.Range(this.minDelay, this.maxDelay);
		for (;;)
		{
			if (time <= 0f)
			{
				time = Random.Range(this.minDelay, this.maxDelay);
				this.pos.x = (float)(Random.Range(0, this.ec.levelSize.x * 10) + 5);
				this.pos.z = (float)(Random.Range(0, this.ec.levelSize.z * 10) + 5);
				base.transform.position = this.pos;
				this.audMan.PlayRandomAudio(this.sounds);
				yield return null;
				while (this.audMan.QueuedAudioIsPlaying)
				{
					yield return null;
				}
			}
			else
			{
				time -= Time.deltaTime * this.ec.EnvironmentTimeScale;
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x04000844 RID: 2116
	private EnvironmentController ec;

	// Token: 0x04000845 RID: 2117
	public AudioManager audMan;

	// Token: 0x04000846 RID: 2118
	public SoundObject[] sounds = new SoundObject[0];

	// Token: 0x04000847 RID: 2119
	private Vector3 pos = new Vector3(0f, 5f, 0f);

	// Token: 0x04000848 RID: 2120
	public float minDelay = 90f;

	// Token: 0x04000849 RID: 2121
	public float maxDelay = 300f;
}
