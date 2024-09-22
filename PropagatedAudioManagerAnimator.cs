using System;
using UnityEngine;

// Token: 0x02000131 RID: 305
public class PropagatedAudioManagerAnimator : PropagatedAudioManager
{
	// Token: 0x06000741 RID: 1857 RVA: 0x000257F9 File Offset: 0x000239F9
	public override void PlayQueue()
	{
		if (this.soundQueue[0].hasAnimation)
		{
			this.animator.Play(this.soundQueue[0].soundClip.name, -1, 0f);
		}
		base.PlayQueue();
	}

	// Token: 0x06000742 RID: 1858 RVA: 0x00025833 File Offset: 0x00023A33
	public override void PlaySingle(SoundObject file)
	{
		base.PlaySingle(file);
		if (file.hasAnimation)
		{
			this.animator.Play(file.soundClip.name, -1, 0f);
		}
	}

	// Token: 0x040007FD RID: 2045
	[SerializeField]
	private Animator animator;
}
