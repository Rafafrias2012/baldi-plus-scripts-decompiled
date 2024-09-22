using System;
using TMPro;
using UnityEngine;

// Token: 0x02000102 RID: 258
public class EndlessGameManager : BaseGameManager
{
	// Token: 0x06000654 RID: 1620 RVA: 0x0001FE58 File Offset: 0x0001E058
	public override void Initialize()
	{
		base.Initialize();
		this.CreateHappyBaldi();
		Singleton<CoreGameManager>.Instance.AddMultiplier(2f);
	}

	// Token: 0x06000655 RID: 1621 RVA: 0x0001FE75 File Offset: 0x0001E075
	public override void BeginPlay()
	{
		base.BeginPlay();
		this.notebookAngerVal = 0f;
		Singleton<MusicManager>.Instance.PlayMidi("school", true);
		Singleton<MusicManager>.Instance.SetLoop(true);
	}

	// Token: 0x06000656 RID: 1622 RVA: 0x0001FEA3 File Offset: 0x0001E0A3
	public override void BeginSpoopMode()
	{
		base.BeginSpoopMode();
		this.ambience.Initialize(this.ec);
	}

	// Token: 0x06000657 RID: 1623 RVA: 0x0001FEBC File Offset: 0x0001E0BC
	private void CreateHappyBaldi()
	{
		HappyBaldi happyBaldi = Object.Instantiate<HappyBaldi>(this.happyBaldiPre, this.ec.transform);
		happyBaldi.Ec = this.ec;
		happyBaldi.transform.position = this.ec.spawnPoint + Singleton<CoreGameManager>.Instance.GetPlayer(0).transform.forward * 20f;
		this.bsi.Initialize(this.ec);
	}

	// Token: 0x06000658 RID: 1624 RVA: 0x0001FF35 File Offset: 0x0001E135
	public override void LoadNextLevel()
	{
		base.LoadNextLevel();
		this.elevatorScreen = Object.Instantiate<ElevatorScreen>(this.elevatorScreenPre);
		this.elevatorScreen.Initialize();
		this.elevatorScreen.ShowResults(this.time, 0);
	}

	// Token: 0x06000659 RID: 1625 RVA: 0x0001FF6B File Offset: 0x0001E16B
	public override void CollectNotebook(Notebook notebook)
	{
		base.CollectNotebook(notebook);
		notebook.activity.StartResetTimer(this.respawnTime);
	}

	// Token: 0x0600065A RID: 1626 RVA: 0x0001FF85 File Offset: 0x0001E185
	public override void CollectNotebooks(int count)
	{
		this.notebookAngerVal = 0f;
		base.CollectNotebooks(count);
		this.AngerBaldi((float)(-1 * count));
		Singleton<CoreGameManager>.Instance.GetHud(0).UpdateNotebookText(0, this.foundNotebooks.ToString(), count > 0);
	}

	// Token: 0x0600065B RID: 1627 RVA: 0x0001FFC3 File Offset: 0x0001E1C3
	public override void EndGame(Transform player, Baldi baldi)
	{
		Singleton<CoreGameManager>.Instance.SetLives(2);
		base.EndGame(player, baldi);
	}

	// Token: 0x0600065C RID: 1628 RVA: 0x0001FFD8 File Offset: 0x0001E1D8
	public override void RestartLevel()
	{
		int num = -1;
		this.resultsScreen.gameObject.SetActive(true);
		Singleton<HighScoreManager>.Instance.AddScore(base.FoundNotebooks, Singleton<PlayerFileManager>.Instance.fileName, this.endlessLevel, out num);
		this.scoreText.text = base.FoundNotebooks.ToString();
		if (num != -1)
		{
			this.congratsText.gameObject.SetActive(true);
			this.rankText.text = (num + 1).ToString();
		}
		Singleton<InputManager>.Instance.ActivateActionSet("Interface");
	}

	// Token: 0x0600065D RID: 1629 RVA: 0x0002006D File Offset: 0x0001E26D
	public void ReturnToMenu()
	{
		Singleton<CoreGameManager>.Instance.ReturnToMenu();
	}

	// Token: 0x0600065E RID: 1630 RVA: 0x00020079 File Offset: 0x0001E279
	protected override void AllNotebooks()
	{
	}

	// Token: 0x04000682 RID: 1666
	[SerializeField]
	private Canvas resultsScreen;

	// Token: 0x04000683 RID: 1667
	[SerializeField]
	private TMP_Text rankText;

	// Token: 0x04000684 RID: 1668
	[SerializeField]
	private TMP_Text congratsText;

	// Token: 0x04000685 RID: 1669
	[SerializeField]
	private TMP_Text scoreText;

	// Token: 0x04000686 RID: 1670
	[SerializeField]
	private HappyBaldi happyBaldiPre;

	// Token: 0x04000687 RID: 1671
	[SerializeField]
	private EndlessLevel endlessLevel;

	// Token: 0x04000688 RID: 1672
	[SerializeField]
	private BaldiSpeedIncreaser bsi;

	// Token: 0x04000689 RID: 1673
	[SerializeField]
	private float respawnTime = 99f;

	// Token: 0x0400068A RID: 1674
	[SerializeField]
	private Ambience ambience;
}
