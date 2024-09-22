using System;
using UnityEngine;

// Token: 0x0200007F RID: 127
public class MinigameResults : MonoBehaviour
{
	// Token: 0x060002FC RID: 764 RVA: 0x0000F981 File Offset: 0x0000DB81
	public void PlayBang()
	{
		this.minigameBase.AudioManager.PlaySingle(this.audResultsBang);
	}

	// Token: 0x060002FD RID: 765 RVA: 0x0000F999 File Offset: 0x0000DB99
	public void PlayPerfect()
	{
		this.minigameBase.AudioManager.PlaySingle(this.audPerfectBonus);
	}

	// Token: 0x060002FE RID: 766 RVA: 0x0000F9B1 File Offset: 0x0000DBB1
	public void Finished()
	{
		this.minigameBase.ResultsAnimationFinished();
	}

	// Token: 0x0400031F RID: 799
	[SerializeField]
	private MinigameBase minigameBase;

	// Token: 0x04000320 RID: 800
	[SerializeField]
	private SoundObject audResultsBang;

	// Token: 0x04000321 RID: 801
	[SerializeField]
	private SoundObject audPerfectBonus;
}
