using System;
using UnityEngine;

// Token: 0x02000098 RID: 152
public class StealthyChallengeManager : BaseGameManager
{
	// Token: 0x0600035F RID: 863 RVA: 0x00011B04 File Offset: 0x0000FD04
	public override void Initialize()
	{
		base.Initialize();
		Singleton<CoreGameManager>.Instance.GetPlayer(0).RuleBreak("AfterHours", 1000000f);
		this.ec.map.CompleteMap();
		Singleton<CoreGameManager>.Instance.GetPlayer(0).itm.AddItem(this.chalkEraser);
		Singleton<CoreGameManager>.Instance.GetPlayer(0).itm.AddItem(this.chalkEraser);
		Singleton<CoreGameManager>.Instance.GetPlayer(0).itm.AddItem(this.chalkEraser);
		Singleton<CoreGameManager>.Instance.GetPlayer(0).itm.AddItem(this.chalkEraser);
		Singleton<CoreGameManager>.Instance.GetPlayer(0).itm.AddItem(this.chalkEraser);
	}

	// Token: 0x06000360 RID: 864 RVA: 0x00011BC8 File Offset: 0x0000FDC8
	protected override void ExitedSpawn()
	{
		base.ExitedSpawn();
		foreach (NPC npc in this.ec.Npcs)
		{
			if (npc.Character == Character.Principal)
			{
				this.ec.map.AddArrow(npc.transform, Color.gray);
			}
		}
		this.ec.GetBaldi().GetAngry(10f);
	}

	// Token: 0x06000361 RID: 865 RVA: 0x00011C58 File Offset: 0x0000FE58
	public override void LoadNextLevel()
	{
		AudioListener.pause = true;
		Time.timeScale = 0f;
		Singleton<CoreGameManager>.Instance.disablePause = true;
		this.winScreen.gameObject.SetActive(true);
	}

	// Token: 0x040003A3 RID: 931
	[SerializeField]
	private ItemObject chalkEraser;

	// Token: 0x040003A4 RID: 932
	public ChallengeWin winScreen;
}
