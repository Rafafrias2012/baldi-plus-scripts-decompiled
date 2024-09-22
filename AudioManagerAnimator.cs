using System;
using UnityEngine;

// Token: 0x020000EE RID: 238
public class AudioManagerAnimator : AudioManager
{
	// Token: 0x06000593 RID: 1427 RVA: 0x0001C3D2 File Offset: 0x0001A5D2
	public override void PlayQueue()
	{
		if (this.soundQueue[0].hasAnimation)
		{
			this.animator.Play(this.soundQueue[0].soundClip.name, -1, 0f);
		}
		base.PlayQueue();
	}

	// Token: 0x06000594 RID: 1428 RVA: 0x0001C40C File Offset: 0x0001A60C
	public override void PlaySingle(SoundObject file)
	{
		base.PlaySingle(file);
		if (file.hasAnimation)
		{
			this.animator.Play(file.soundClip.name, -1, 0f);
		}
	}

	// Token: 0x040005C6 RID: 1478
	[SerializeField]
	private Animator animator;
}
