using System;
using UnityEngine;

// Token: 0x02000104 RID: 260
public class MainGameManager : BaseGameManager
{
	// Token: 0x06000664 RID: 1636 RVA: 0x00020110 File Offset: 0x0001E310
	public override void Initialize()
	{
		if (this.levelNo > Singleton<CoreGameManager>.Instance.lastLevelNumber)
		{
			Singleton<CoreGameManager>.Instance.SetLives(this.defaultLives);
			Singleton<CoreGameManager>.Instance.lastLevelNumber = this.levelNo;
		}
		base.Initialize();
		this.CreateHappyBaldi();
	}

	// Token: 0x06000665 RID: 1637 RVA: 0x00020150 File Offset: 0x0001E350
	public override void BeginPlay()
	{
		base.BeginPlay();
		Singleton<MusicManager>.Instance.PlayMidi("school", true);
		Singleton<MusicManager>.Instance.SetLoop(true);
	}

	// Token: 0x06000666 RID: 1638 RVA: 0x00020173 File Offset: 0x0001E373
	public override void BeginSpoopMode()
	{
		base.BeginSpoopMode();
		this.ambience.Initialize(this.ec);
	}

	// Token: 0x06000667 RID: 1639 RVA: 0x0002018C File Offset: 0x0001E38C
	private void CreateHappyBaldi()
	{
		HappyBaldi happyBaldi = Object.Instantiate<HappyBaldi>(this.happyBaldiPre, this.ec.transform);
		happyBaldi.Ec = this.ec;
		happyBaldi.transform.position = this.ec.spawnPoint + Singleton<CoreGameManager>.Instance.GetPlayer(0).transform.forward * 20f;
	}

	// Token: 0x06000668 RID: 1640 RVA: 0x000201F4 File Offset: 0x0001E3F4
	public override void LoadNextLevel()
	{
		Singleton<CoreGameManager>.Instance.saveMapAvailable = false;
		for (int i = 0; i < Singleton<CoreGameManager>.Instance.Lives; i++)
		{
			Singleton<CoreGameManager>.Instance.AddPoints(Singleton<CoreGameManager>.Instance.GetPointsThisLevel(0), 0, false, false);
		}
		base.PrepareToLoad();
		this.elevatorScreen = Object.Instantiate<ElevatorScreen>(this.elevatorScreenPre);
		this.elevatorScreen.OnLoadReady += base.LoadNextLevel;
		this.elevatorScreen.Initialize();
		int num = 0;
		if (this.time <= this.levelObject.timeBonusLimit)
		{
			num = this.levelObject.timeBonusVal;
		}
		Singleton<CoreGameManager>.Instance.AddPoints(num, 0, false, false);
		Singleton<CoreGameManager>.Instance.AwardGradeBonus();
		this.elevatorScreen.ShowResults(this.time, num);
		if (Singleton<CoreGameManager>.Instance.GetPoints(0) > 0)
		{
			bool finalLevel = this.levelObject.finalLevel;
		}
	}

	// Token: 0x06000669 RID: 1641 RVA: 0x000202D7 File Offset: 0x0001E4D7
	protected override void LoadSceneObject(SceneObject sceneObject, bool restarting)
	{
		Singleton<CoreGameManager>.Instance.nextLevel = sceneObject;
		if (!this.levelObject.finalLevel || restarting)
		{
			base.LoadSceneObject(this.pitstop, restarting);
			return;
		}
		base.LoadSceneObject(sceneObject, restarting);
	}

	// Token: 0x0600066A RID: 1642 RVA: 0x0002030C File Offset: 0x0001E50C
	public override void RestartLevel()
	{
		Singleton<CoreGameManager>.Instance.saveMapAvailable = true;
		base.PrepareToLoad();
		this.elevatorScreen = Object.Instantiate<ElevatorScreen>(this.elevatorScreenPre);
		this.elevatorScreen.OnLoadReady += base.RestartLevel;
		this.elevatorScreen.Initialize();
		Singleton<CoreGameManager>.Instance.GetPoints(0);
	}

	// Token: 0x0600066B RID: 1643 RVA: 0x0002036B File Offset: 0x0001E56B
	public override void CollectNotebooks(int count)
	{
		base.CollectNotebooks(count);
	}

	// Token: 0x0600066C RID: 1644 RVA: 0x00020374 File Offset: 0x0001E574
	public override void CollectNotebook(Notebook notebook)
	{
		base.CollectNotebook(notebook);
		if (notebook.activity.GetType().Name == "NoActivity")
		{
			Singleton<CoreGameManager>.Instance.AddPoints(10, 0, true);
		}
	}

	// Token: 0x0600066D RID: 1645 RVA: 0x000203A8 File Offset: 0x0001E5A8
	protected override void AllNotebooks()
	{
		base.AllNotebooks();
		foreach (Activity activity in this.ec.activities)
		{
			if (activity != this.lastActivity)
			{
				activity.Corrupt(false);
				activity.SetBonusMode(true);
			}
		}
		Singleton<CoreGameManager>.Instance.GetHud(0).BaldiTv.Speak(this.allNotebooksNotification);
	}

	// Token: 0x0400068B RID: 1675
	[SerializeField]
	private SceneObject pitstop;

	// Token: 0x0400068C RID: 1676
	[SerializeField]
	private HappyBaldi happyBaldiPre;

	// Token: 0x0400068D RID: 1677
	[SerializeField]
	private Ambience ambience;

	// Token: 0x0400068E RID: 1678
	[SerializeField]
	private SoundObject allNotebooksNotification;
}
