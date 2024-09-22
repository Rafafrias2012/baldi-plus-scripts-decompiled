using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000121 RID: 289
public class MusicPlayer : MonoBehaviour
{
	// Token: 0x06000707 RID: 1799 RVA: 0x000237FE File Offset: 0x000219FE
	private void Start()
	{
		if (this.playOnStart)
		{
			base.StartCoroutine(this.StartDelay());
		}
	}

	// Token: 0x06000708 RID: 1800 RVA: 0x00023815 File Offset: 0x00021A15
	private IEnumerator StartDelay()
	{
		yield return null;
		Singleton<MusicManager>.Instance.PlayMidi(this.track, true);
		Singleton<MusicManager>.Instance.SetLoop(this.loop);
		yield break;
	}

	// Token: 0x04000746 RID: 1862
	[SerializeField]
	private string track = "title";

	// Token: 0x04000747 RID: 1863
	[SerializeField]
	private bool playOnStart;

	// Token: 0x04000748 RID: 1864
	[SerializeField]
	private bool loop = true;
}
