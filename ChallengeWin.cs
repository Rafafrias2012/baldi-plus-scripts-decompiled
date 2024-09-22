using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000162 RID: 354
public class ChallengeWin : MonoBehaviour
{
	// Token: 0x060007FF RID: 2047 RVA: 0x00027C1E File Offset: 0x00025E1E
	private void Start()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		this.bg.SetActive(true);
		this.audMan.QueueAudio(this.sound);
		base.StartCoroutine(this.Delay());
	}

	// Token: 0x06000800 RID: 2048 RVA: 0x00027C55 File Offset: 0x00025E55
	private IEnumerator SelfDestruct()
	{
		yield return null;
		while (Time.timeScale == 0f)
		{
			yield return null;
		}
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06000801 RID: 2049 RVA: 0x00027C64 File Offset: 0x00025E64
	private IEnumerator Delay()
	{
		float time = 5f;
		while (time > 0f)
		{
			time -= Time.unscaledDeltaTime;
			yield return null;
		}
		this.bg.SetActive(false);
		time = 2f;
		while (time > 0f)
		{
			time -= Time.unscaledDeltaTime;
			yield return null;
		}
		Singleton<CoreGameManager>.Instance.Quit();
		yield return null;
		AudioListener.pause = false;
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x0400088F RID: 2191
	[SerializeField]
	private AudioManager audMan;

	// Token: 0x04000890 RID: 2192
	[SerializeField]
	private SoundObject sound;

	// Token: 0x04000891 RID: 2193
	[SerializeField]
	private GameObject bg;
}
