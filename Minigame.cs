using System;
using UnityEngine;

// Token: 0x0200007A RID: 122
public class Minigame : MonoBehaviour
{
	// Token: 0x1700002B RID: 43
	// (get) Token: 0x060002C7 RID: 711 RVA: 0x0000EFA2 File Offset: 0x0000D1A2
	// (set) Token: 0x060002C8 RID: 712 RVA: 0x0000EFAA File Offset: 0x0000D1AA
	public MinigameBase minigameBase { get; private set; }

	// Token: 0x060002C9 RID: 713 RVA: 0x0000EFB3 File Offset: 0x0000D1B3
	public virtual void Initialize(MinigameBase minigameBase, bool scoringMode)
	{
		this.minigameBase = minigameBase;
		this.scoringMode = scoringMode;
	}

	// Token: 0x060002CA RID: 714 RVA: 0x0000EFC3 File Offset: 0x0000D1C3
	public virtual void VirtualStart()
	{
	}

	// Token: 0x060002CB RID: 715 RVA: 0x0000EFC5 File Offset: 0x0000D1C5
	public virtual void Prepare()
	{
	}

	// Token: 0x060002CC RID: 716 RVA: 0x0000EFC7 File Offset: 0x0000D1C7
	public virtual void StartRound()
	{
	}

	// Token: 0x060002CD RID: 717 RVA: 0x0000EFC9 File Offset: 0x0000D1C9
	public virtual void VirtualUpdate()
	{
	}

	// Token: 0x060002CE RID: 718 RVA: 0x0000EFCB File Offset: 0x0000D1CB
	public virtual void VirtualReset()
	{
		this.score = 0;
	}

	// Token: 0x060002CF RID: 719 RVA: 0x0000EFD4 File Offset: 0x0000D1D4
	protected virtual void AddScore(int amount)
	{
		this.score += amount;
	}

	// Token: 0x040002E5 RID: 741
	protected int score;

	// Token: 0x040002E6 RID: 742
	protected bool scoringMode;

	// Token: 0x040002E7 RID: 743
	protected bool perfect = true;
}
