using System;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x02000120 RID: 288
public class MusicFileData
{
	// Token: 0x06000706 RID: 1798 RVA: 0x000237E8 File Offset: 0x000219E8
	public MusicFileData(AudioClip clip, AudioMixerGroup mixer)
	{
		this.clip = clip;
		this.mixer = mixer;
	}

	// Token: 0x04000744 RID: 1860
	public AudioClip clip;

	// Token: 0x04000745 RID: 1861
	public AudioMixerGroup mixer;
}
