using System;
using System.Collections;
using TMPro;
using UnityEngine;

// Token: 0x0200007C RID: 124
public class MinigameBase : MonoBehaviour
{
	// Token: 0x1700002C RID: 44
	// (get) Token: 0x060002D1 RID: 721 RVA: 0x0000EFF3 File Offset: 0x0000D1F3
	// (set) Token: 0x060002D2 RID: 722 RVA: 0x0000EFFB File Offset: 0x0000D1FB
	public float Time { get; private set; }

	// Token: 0x1700002D RID: 45
	// (get) Token: 0x060002D3 RID: 723 RVA: 0x0000F004 File Offset: 0x0000D204
	// (set) Token: 0x060002D4 RID: 724 RVA: 0x0000F00C File Offset: 0x0000D20C
	public int CurrentRound { get; private set; }

	// Token: 0x060002D5 RID: 725 RVA: 0x0000F018 File Offset: 0x0000D218
	public virtual void Initialize(FieldTripBaseRoomFunction hub, bool scoringMode)
	{
		this.hub = hub;
		this.minigameController.Initialize(this, scoringMode);
		this.titleObject.SetActive(!this.skipTitle);
		this.scoringMode = scoringMode;
		this.highScoresButton.SetActive(scoringMode);
		this.pauseQuitButton.SetActive(scoringMode);
		this.scoreTable.UpdateScores(this.minigame, this.minigameNameKey);
		Singleton<GlobalCam>.Instance.FadeIn(UiTransition.Dither, 0.01666667f);
	}

	// Token: 0x060002D6 RID: 726 RVA: 0x0000F094 File Offset: 0x0000D294
	private void Update()
	{
		if (this.minigameActive)
		{
			if (Singleton<InputManager>.Instance.GetDigitalInput("Pause", true))
			{
				this.Pause();
			}
			if (!this.paused && !Singleton<GlobalCam>.Instance.TransitionActive)
			{
				this.minigameController.VirtualUpdate();
			}
		}
		this.Time += this.DeltaTime;
		if (this.paused || Singleton<GlobalCam>.Instance.TransitionActive)
		{
			this.audioManager.pitchModifier = 0f;
			return;
		}
		this.audioManager.pitchModifier = 1f;
	}

	// Token: 0x060002D7 RID: 727 RVA: 0x0000F128 File Offset: 0x0000D328
	public void Pause()
	{
		if (!Singleton<GlobalCam>.Instance.TransitionActive)
		{
			this.paused = !this.paused;
			Singleton<GlobalCam>.Instance.Transition(UiTransition.Dither, 0.01666667f);
			this.pauseObject.SetActive(this.paused);
			GameObject[] array = this.disableOnPause;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(!this.paused);
			}
			if (!this.paused && this.useCursor)
			{
				this.cursorInitiator.Inititate();
			}
			Singleton<MusicManager>.Instance.PauseMidi(this.paused);
		}
	}

	// Token: 0x060002D8 RID: 728 RVA: 0x0000F1C5 File Offset: 0x0000D3C5
	public void ShowHighScores(bool val)
	{
		Singleton<GlobalCam>.Instance.Transition(UiTransition.Dither, 0.01666667f);
		this.highScoresObject.SetActive(val);
	}

	// Token: 0x060002D9 RID: 729 RVA: 0x0000F1E4 File Offset: 0x0000D3E4
	public void StartMinigame()
	{
		this.animator.Play("MinigameStart", -1, 0f);
		if (!this.useCursor)
		{
			this.cursorInitiator.enabled = false;
			Object.Destroy(CursorController.Instance.gameObject);
		}
		if (this.midiSong.Length <= 0)
		{
			Singleton<MusicManager>.Instance.QueueFile(this.musicObject, true);
		}
		else
		{
			Singleton<MusicManager>.Instance.PlayMidi(this.midiSong, true);
		}
		this.minigameController.Prepare();
		this.audioManager.PlaySingle(this.audStart);
	}

	// Token: 0x060002DA RID: 730 RVA: 0x0000F278 File Offset: 0x0000D478
	public void TitleSequenceFinished()
	{
		this.minigameActive = true;
		this.minigameController.VirtualStart();
	}

	// Token: 0x060002DB RID: 731 RVA: 0x0000F28C File Offset: 0x0000D48C
	public void AdvanceRound(float nextRoundDelay)
	{
		if (this.minigameActive)
		{
			this.CurrentRound++;
			this.roundTmp.text = string.Format(Singleton<LocalizationManager>.Instance.GetLocalizedText("Mgm_Round"), this.CurrentRound);
			this.roundIndicatorAnimator.Play("MinigameBaseRoundDisplay", -1, 0f);
			base.StartCoroutine(this.RoundAdvancer(nextRoundDelay));
		}
	}

	// Token: 0x060002DC RID: 732 RVA: 0x0000F2FD File Offset: 0x0000D4FD
	private IEnumerator RoundAdvancer(float delay)
	{
		delay += 1f;
		while (delay > 0f)
		{
			delay -= this.DeltaTime;
			yield return null;
		}
		this.minigameController.StartRound();
		yield break;
	}

	// Token: 0x060002DD RID: 733 RVA: 0x0000F313 File Offset: 0x0000D513
	public virtual void Win()
	{
		this.End(true);
	}

	// Token: 0x060002DE RID: 734 RVA: 0x0000F31C File Offset: 0x0000D51C
	public virtual void Lose()
	{
		this.End(false);
	}

	// Token: 0x060002DF RID: 735 RVA: 0x0000F328 File Offset: 0x0000D528
	public void ResultsAnimationFinished()
	{
		if (!this.result)
		{
			this.audioManager.PlaySingle(this.audLose);
			return;
		}
		if (this.scoringMode)
		{
			this.audioManager.PlaySingle(this.audHighScore);
			return;
		}
		this.audioManager.PlaySingle(this.audWin);
	}

	// Token: 0x060002E0 RID: 736 RVA: 0x0000F37C File Offset: 0x0000D57C
	public void SetScore(int score)
	{
		int num;
		Singleton<HighScoreManager>.Instance.AddTripScore(score, Singleton<PlayerFileManager>.Instance.fileName, this.minigame, true, out num);
		this.minigameActive = false;
		this.ShowResults(num >= 0, num, score);
		this.result = (num >= 0);
		Singleton<MusicManager>.Instance.SetFileLoop(false);
		Singleton<MusicManager>.Instance.SetLoop(false);
	}

	// Token: 0x060002E1 RID: 737 RVA: 0x0000F3E4 File Offset: 0x0000D5E4
	public virtual void AwardPerfectBonus()
	{
		if (Singleton<CoreGameManager>.Instance != null)
		{
			Singleton<CoreGameManager>.Instance.AddPoints(this.perfectBonus, 0, false);
		}
		this.bonusTmp.gameObject.SetActive(true);
		this.bonusTmp.text = string.Format(Singleton<LocalizationManager>.Instance.GetLocalizedText("Mgm_Base_Perfect"), this.perfectBonus);
	}

	// Token: 0x060002E2 RID: 738 RVA: 0x0000F44B File Offset: 0x0000D64B
	private void ShowResults(bool result)
	{
		this.ShowResults(result, 0, 0);
	}

	// Token: 0x060002E3 RID: 739 RVA: 0x0000F458 File Offset: 0x0000D658
	private void ShowResults(bool result, int rank, int score)
	{
		this.resultsObject.SetActive(true);
		if (!this.scoringMode)
		{
			if (result)
			{
				this.resultTmp.text = Singleton<LocalizationManager>.Instance.GetLocalizedText(this.winKey);
				return;
			}
			this.resultTmp.text = Singleton<LocalizationManager>.Instance.GetLocalizedText(this.loseKey);
			return;
		}
		else
		{
			if (result)
			{
				this.resultTmp.text = string.Format(Singleton<LocalizationManager>.Instance.GetLocalizedText("Mgm_FinalScore"), score) + string.Format(Singleton<LocalizationManager>.Instance.GetLocalizedText("Mgm_HighScore"), rank + 1);
				return;
			}
			this.resultTmp.text = string.Format(Singleton<LocalizationManager>.Instance.GetLocalizedText("Mgm_FinalScore"), score) + Singleton<LocalizationManager>.Instance.GetLocalizedText("Mgm_LoseEndless");
			return;
		}
	}

	// Token: 0x060002E4 RID: 740 RVA: 0x0000F537 File Offset: 0x0000D737
	protected virtual void End(bool result)
	{
		this.ShowResults(result);
		this.result = result;
		this.minigameActive = false;
		Singleton<MusicManager>.Instance.SetFileLoop(false);
		Singleton<MusicManager>.Instance.SetLoop(false);
	}

	// Token: 0x060002E5 RID: 741 RVA: 0x0000F564 File Offset: 0x0000D764
	public void ContinuePastResults()
	{
		if (!this.scoringMode)
		{
			this.Close(true);
			return;
		}
		Singleton<GlobalCam>.Instance.Transition(UiTransition.Dither, 0.01666667f);
		this.resultsObject.SetActive(false);
		this.animator.Play("MinigameBase", -1, 0f);
		this.minigameController.VirtualReset();
		this.cursorInitiator.enabled = false;
		this.cursorInitiator.enabled = true;
		this.scoreTable.UpdateScores(this.minigame, this.minigameNameKey);
		this.CurrentRound = 0;
	}

	// Token: 0x060002E6 RID: 742 RVA: 0x0000F5F4 File Offset: 0x0000D7F4
	public virtual void Close(bool finished)
	{
		Singleton<GlobalCam>.Instance.Transition(UiTransition.Dither, 0.01666667f);
		Singleton<MusicManager>.Instance.StopFile();
		if (!this.result || !finished)
		{
			Singleton<MusicManager>.Instance.StopMidi();
			Singleton<MusicManager>.Instance.SetSpeed(1f);
		}
		this.hub.EndMinigame(this.result, finished);
	}

	// Token: 0x1700002E RID: 46
	// (get) Token: 0x060002E7 RID: 743 RVA: 0x0000F651 File Offset: 0x0000D851
	public AudioManager AudioManager
	{
		get
		{
			return this.audioManager;
		}
	}

	// Token: 0x1700002F RID: 47
	// (get) Token: 0x060002E8 RID: 744 RVA: 0x0000F659 File Offset: 0x0000D859
	public float DeltaTime
	{
		get
		{
			if (this.paused || Singleton<GlobalCam>.Instance.TransitionActive || !this.minigameActive)
			{
				return 0f;
			}
			return UnityEngine.Time.deltaTime;
		}
	}

	// Token: 0x040002EF RID: 751
	[SerializeField]
	private string minigameNameKey = "Mgm_Name_CampfireFrenzy";

	// Token: 0x040002F0 RID: 752
	[SerializeField]
	private MinigameName minigame;

	// Token: 0x040002F1 RID: 753
	[SerializeField]
	private Minigame minigameController;

	// Token: 0x040002F2 RID: 754
	[SerializeField]
	private MinigameHighScoreTable scoreTable;

	// Token: 0x040002F3 RID: 755
	private FieldTripBaseRoomFunction hub;

	// Token: 0x040002F4 RID: 756
	[SerializeField]
	private GameObject[] disableOnPause = new GameObject[0];

	// Token: 0x040002F5 RID: 757
	[SerializeField]
	private GameObject gameCanvasObject;

	// Token: 0x040002F6 RID: 758
	[SerializeField]
	private GameObject resultsObject;

	// Token: 0x040002F7 RID: 759
	[SerializeField]
	private GameObject pauseObject;

	// Token: 0x040002F8 RID: 760
	[SerializeField]
	private GameObject titleObject;

	// Token: 0x040002F9 RID: 761
	[SerializeField]
	private GameObject highScoresObject;

	// Token: 0x040002FA RID: 762
	[SerializeField]
	private GameObject highScoresButton;

	// Token: 0x040002FB RID: 763
	[SerializeField]
	private GameObject pauseQuitButton;

	// Token: 0x040002FC RID: 764
	[SerializeField]
	private CursorInitiator cursorInitiator;

	// Token: 0x040002FD RID: 765
	[SerializeField]
	protected AudioManager audioManager;

	// Token: 0x040002FE RID: 766
	[SerializeField]
	private SoundObject audStart;

	// Token: 0x040002FF RID: 767
	[SerializeField]
	private SoundObject audWin;

	// Token: 0x04000300 RID: 768
	[SerializeField]
	private SoundObject audLose;

	// Token: 0x04000301 RID: 769
	[SerializeField]
	private SoundObject audHighScore;

	// Token: 0x04000302 RID: 770
	[SerializeField]
	private LoopingSoundObject musicObject;

	// Token: 0x04000303 RID: 771
	[SerializeField]
	private string midiSong = "";

	// Token: 0x04000304 RID: 772
	[SerializeField]
	private Animator animator;

	// Token: 0x04000305 RID: 773
	[SerializeField]
	private Animator roundIndicatorAnimator;

	// Token: 0x04000306 RID: 774
	[SerializeField]
	private TMP_Text resultTmp;

	// Token: 0x04000307 RID: 775
	[SerializeField]
	private TMP_Text bonusTmp;

	// Token: 0x04000308 RID: 776
	[SerializeField]
	private TMP_Text roundTmp;

	// Token: 0x04000309 RID: 777
	[SerializeField]
	private string winKey = "Mgm_Base_Win";

	// Token: 0x0400030A RID: 778
	[SerializeField]
	private string loseKey = "Mgm_Base_Lose";

	// Token: 0x0400030C RID: 780
	[SerializeField]
	private int perfectBonus = 500;

	// Token: 0x0400030E RID: 782
	[SerializeField]
	private bool useCursor;

	// Token: 0x0400030F RID: 783
	[SerializeField]
	private bool skipTitle;

	// Token: 0x04000310 RID: 784
	private bool result;

	// Token: 0x04000311 RID: 785
	private bool paused;

	// Token: 0x04000312 RID: 786
	private bool minigameActive;

	// Token: 0x04000313 RID: 787
	private bool scoringMode;
}
