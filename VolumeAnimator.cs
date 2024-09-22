using System;
using UnityEngine;

// Token: 0x02000144 RID: 324
public class VolumeAnimator : MonoBehaviour
{
	// Token: 0x06000776 RID: 1910 RVA: 0x00026244 File Offset: 0x00024444
	private void Update()
	{
		if (this.audioSource == null)
		{
			this.audioSource = this.fallbackAudioManager.audioDevice;
		}
		if (this.audioSource.isPlaying)
		{
			if (this.audioSource.clip != this.currentClip)
			{
				this.currentClip = this.audioSource.clip;
				this.clipData = new float[this.currentClip.samples * this.currentClip.channels];
				this.currentClip.GetData(this.clipData, 0);
				this.lastSample = 0;
				this.sampleBuffer = Mathf.RoundToInt((float)this.currentClip.samples / this.currentClip.length * this.bufferTime);
			}
			this.volume = 0f;
			int num = Mathf.Max(this.lastSample - this.sampleBuffer, 0);
			while (num < this.audioSource.timeSamples * this.currentClip.channels && num < this.clipData.Length)
			{
				this.potentialVolume = this.sensitivity.Evaluate(Mathf.Abs(this.clipData[num]));
				if (this.potentialVolume > this.volume)
				{
					this.volume = this.potentialVolume;
				}
				num++;
			}
			this.lastSample = this.audioSource.timeSamples * this.currentClip.channels;
			this.animator.Play(this.animationName, -1, Mathf.Max(new float[]
			{
				this.volume
			}));
			return;
		}
		this.animator.Play(this.animationName, -1, 0f);
	}

	// Token: 0x04000831 RID: 2097
	public string animationName;

	// Token: 0x04000832 RID: 2098
	public AnimationCurve sensitivity;

	// Token: 0x04000833 RID: 2099
	[SerializeField]
	private Animator animator;

	// Token: 0x04000834 RID: 2100
	[SerializeField]
	private AudioManager fallbackAudioManager;

	// Token: 0x04000835 RID: 2101
	[SerializeField]
	private AudioSource audioSource;

	// Token: 0x04000836 RID: 2102
	private AudioClip currentClip;

	// Token: 0x04000837 RID: 2103
	public float bufferTime = 0.1f;

	// Token: 0x04000838 RID: 2104
	private float[] clipData;

	// Token: 0x04000839 RID: 2105
	private float volume;

	// Token: 0x0400083A RID: 2106
	private float potentialVolume;

	// Token: 0x0400083B RID: 2107
	private int lastSample;

	// Token: 0x0400083C RID: 2108
	private int sampleBuffer;
}
