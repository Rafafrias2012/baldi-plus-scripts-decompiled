using System;
using UnityEngine;

// Token: 0x02000096 RID: 150
public class GrappleChallengeManager : BaseGameManager
{
	// Token: 0x06000356 RID: 854 RVA: 0x00011964 File Offset: 0x0000FB64
	public override void Initialize()
	{
		base.Initialize();
		Singleton<CoreGameManager>.Instance.GetPlayer(0).plm.walkSpeed = 0f;
		Singleton<CoreGameManager>.Instance.GetPlayer(0).plm.runSpeed = 0f;
		this.ec.map.CompleteMap();
	}

	// Token: 0x06000357 RID: 855 RVA: 0x000119BB File Offset: 0x0000FBBB
	private void Update()
	{
		if (this.playStarted)
		{
			Singleton<CoreGameManager>.Instance.GetPlayer(0).itm.SetItem(this.hook, 0);
			Singleton<CoreGameManager>.Instance.GetPlayer(0).itm.LockSlot(0, true);
		}
	}

	// Token: 0x06000358 RID: 856 RVA: 0x000119F8 File Offset: 0x0000FBF8
	public override void AngerBaldi(float val)
	{
		base.AngerBaldi(val * this.bookMultiplier);
	}

	// Token: 0x06000359 RID: 857 RVA: 0x00011A08 File Offset: 0x0000FC08
	public override void LoadNextLevel()
	{
		AudioListener.pause = true;
		Time.timeScale = 0f;
		Singleton<CoreGameManager>.Instance.disablePause = true;
		this.winScreen.gameObject.SetActive(true);
	}

	// Token: 0x0400039F RID: 927
	[SerializeField]
	private ItemObject hook;

	// Token: 0x040003A0 RID: 928
	[SerializeField]
	private float bookMultiplier = 2f;

	// Token: 0x040003A1 RID: 929
	public ChallengeWin winScreen;
}
