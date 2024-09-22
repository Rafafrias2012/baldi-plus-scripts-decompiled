using System;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x020001F8 RID: 504
[CreateAssetMenu(fileName = "Sound", menuName = "Custom Assets/Sound Data Object", order = 1)]
public class SoundObject : ScriptableObject
{
	// Token: 0x04000D8A RID: 3466
	public AudioClip soundClip;

	// Token: 0x04000D8B RID: 3467
	public string soundKey;

	// Token: 0x04000D8C RID: 3468
	public SubtitleTimedKey[] additionalKeys = new SubtitleTimedKey[0];

	// Token: 0x04000D8D RID: 3469
	public SoundType soundType;

	// Token: 0x04000D8E RID: 3470
	public AudioMixerGroup mixerOverride;

	// Token: 0x04000D8F RID: 3471
	public float subDuration;

	// Token: 0x04000D90 RID: 3472
	public float volumeMultiplier = 1f;

	// Token: 0x04000D91 RID: 3473
	public Color color = Color.white;

	// Token: 0x04000D92 RID: 3474
	public bool lockSettings;

	// Token: 0x04000D93 RID: 3475
	public bool subtitle = true;

	// Token: 0x04000D94 RID: 3476
	public bool encrypted;

	// Token: 0x04000D95 RID: 3477
	public AudioClip testLanguageClip;

	// Token: 0x04000D96 RID: 3478
	public AudioClip frenchClip;

	// Token: 0x04000D97 RID: 3479
	public AudioClip japaneseClip;

	// Token: 0x04000D98 RID: 3480
	public bool hasAnimation;

	// Token: 0x04000D99 RID: 3481
	public bool addToMemory = true;
}
