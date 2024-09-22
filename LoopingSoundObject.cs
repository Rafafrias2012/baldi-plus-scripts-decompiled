using System;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x020001F4 RID: 500
[CreateAssetMenu(fileName = "Music Object", menuName = "Custom Assets/Music Data Object", order = 8)]
public class LoopingSoundObject : ScriptableObject
{
	// Token: 0x04000D6E RID: 3438
	public AudioClip[] clips = new AudioClip[0];

	// Token: 0x04000D6F RID: 3439
	public AudioMixerGroup mixer;
}
