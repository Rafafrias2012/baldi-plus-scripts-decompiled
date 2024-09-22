using System;
using UnityEngine;

// Token: 0x02000097 RID: 151
public class SpeedyChallengeManager : BaseGameManager
{
	// Token: 0x0600035B RID: 859 RVA: 0x00011A4C File Offset: 0x0000FC4C
	public override void Initialize()
	{
		base.Initialize();
		Singleton<CoreGameManager>.Instance.GetPlayer(0).plm.walkSpeed = 70f;
		Singleton<CoreGameManager>.Instance.GetPlayer(0).plm.runSpeed = 100f;
		this.ec.map.CompleteMap();
		this.notebookAngerVal = 0f;
	}

	// Token: 0x0600035C RID: 860 RVA: 0x00011AAE File Offset: 0x0000FCAE
	protected override void ExitedSpawn()
	{
		base.ExitedSpawn();
		this.ec.GetBaldi().GetAngry(42f);
	}

	// Token: 0x0600035D RID: 861 RVA: 0x00011ACB File Offset: 0x0000FCCB
	public override void LoadNextLevel()
	{
		AudioListener.pause = true;
		Time.timeScale = 0f;
		Singleton<CoreGameManager>.Instance.disablePause = true;
		this.winScreen.gameObject.SetActive(true);
	}

	// Token: 0x040003A2 RID: 930
	public ChallengeWin winScreen;
}
