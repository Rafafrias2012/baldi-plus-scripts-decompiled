using System;
using TMPro;
using UnityEngine;

// Token: 0x02000081 RID: 129
public class OneButtonTestMinigame : Minigame
{
	// Token: 0x06000306 RID: 774 RVA: 0x0000FD4E File Offset: 0x0000DF4E
	public override void Initialize(MinigameBase minigameBase, bool endlessMode)
	{
		base.Initialize(minigameBase, endlessMode);
		this.VirtualReset();
	}

	// Token: 0x06000307 RID: 775 RVA: 0x0000FD60 File Offset: 0x0000DF60
	public override void VirtualUpdate()
	{
		base.VirtualUpdate();
		if (Singleton<InputManager>.Instance.GetDigitalInput("MouseSubmit", true))
		{
			this.clicksToWin--;
		}
		this.countTmp.text = this.clicksToWin.ToString();
		if (this.clicksToWin <= 0)
		{
			if (this.scoringMode)
			{
				base.minigameBase.SetScore(Random.Range(0, 100));
				return;
			}
			base.minigameBase.Win();
		}
	}

	// Token: 0x06000308 RID: 776 RVA: 0x0000FDD9 File Offset: 0x0000DFD9
	public override void VirtualReset()
	{
		this.clicksToWin = this.setClicksToWin;
	}

	// Token: 0x04000331 RID: 817
	[SerializeField]
	private TMP_Text countTmp;

	// Token: 0x04000332 RID: 818
	public int setClicksToWin = 3;

	// Token: 0x04000333 RID: 819
	private int clicksToWin;
}
