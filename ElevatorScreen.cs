using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000B6 RID: 182
public class ElevatorScreen : Singleton<ElevatorScreen>
{
	// Token: 0x14000002 RID: 2
	// (add) Token: 0x0600040E RID: 1038 RVA: 0x000155C4 File Offset: 0x000137C4
	// (remove) Token: 0x0600040F RID: 1039 RVA: 0x000155FC File Offset: 0x000137FC
	public event ElevatorScreen.OnLoadReadyHandler OnLoadReady;

	// Token: 0x06000410 RID: 1040 RVA: 0x00015631 File Offset: 0x00013831
	protected override void AwakeFunction()
	{
		base.AwakeFunction();
		base.transform.localScale = Vector3.one * this.maxScale;
	}

	// Token: 0x06000411 RID: 1041 RVA: 0x00015654 File Offset: 0x00013854
	private void Start()
	{
		this.canvas.worldCamera = Singleton<GlobalCam>.Instance.Cam;
		this.seedText.text = Singleton<CoreGameManager>.Instance.Seed().ToString();
		this.UpdateFloorDisplay();
		this.UpdateLives();
		this.audMan.audioDevice.ignoreListenerPause = true;
		if (!this.initialized)
		{
			this.Initialize();
		}
	}

	// Token: 0x06000412 RID: 1042 RVA: 0x000156C0 File Offset: 0x000138C0
	public void Initialize()
	{
		this.QueueEnumerator(this.ZoomIntro());
		this.QueueEnumerator(this.Shut());
		base.StartCoroutine(this.LoadDelay(2f));
		this.ytpMultiplier = Singleton<CoreGameManager>.Instance.Lives + 1;
		this.initialized = true;
	}

	// Token: 0x06000413 RID: 1043 RVA: 0x00015710 File Offset: 0x00013910
	public void Reinit()
	{
		this.OnLoadReady = null;
		base.StartCoroutine(this.LoadDelay(2f));
		this.readyToStart = false;
	}

	// Token: 0x06000414 RID: 1044 RVA: 0x00015734 File Offset: 0x00013934
	private void Update()
	{
		if (this.queuedEnumerators.Count > 0 && !this.busy)
		{
			base.StartCoroutine(this.queuedEnumerators[0]);
			this.busy = true;
			this.queuedEnumerators.RemoveAt(0);
		}
		if (Singleton<CoreGameManager>.Instance.readyToStart && !this.playButton.activeSelf && !this.busy && !this.readyToStart)
		{
			this.buttonAnimator.gameObject.SetActive(true);
			bool saveEnabled = Singleton<CoreGameManager>.Instance.SaveEnabled;
			this.skipButton.SetActive(Singleton<CoreGameManager>.Instance.sceneObject.skippable);
			this.buttonAnimator.Play("ButtonRise", -1, 0f);
			this.buttonLabel.text = Singleton<LocalizationManager>.Instance.GetLocalizedText("But_Play!");
			this.readyToStart = true;
			this.audMan.PlaySingle(this.audBuzz);
			this.UpdateFloorDisplay();
			this.UpdateLives();
		}
		if (Singleton<CoreGameManager>.Instance.levelGenError && !this.error)
		{
			Image[] array = this.allImages;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].color = new Color(0.5f, 0f, 0f);
			}
			this.UpdateFloorDisplay();
			this.errorText.gameObject.SetActive(true);
			this.audMan.PlaySingle(this.audError);
			this.error = true;
		}
	}

	// Token: 0x06000415 RID: 1045 RVA: 0x000158B0 File Offset: 0x00013AB0
	private void QueueEnumerator(IEnumerator enumerator)
	{
		this.queuedEnumerators.Add(enumerator);
	}

	// Token: 0x06000416 RID: 1046 RVA: 0x000158BE File Offset: 0x00013ABE
	private void UpdateLives()
	{
		this.livesImage.sprite = this.lifeImages[Singleton<CoreGameManager>.Instance.Lives + 1];
	}

	// Token: 0x06000417 RID: 1047 RVA: 0x000158DE File Offset: 0x00013ADE
	public void UpdateFloorDisplay()
	{
		this.floorText.text = Singleton<CoreGameManager>.Instance.sceneObject.levelTitle;
	}

	// Token: 0x06000418 RID: 1048 RVA: 0x000158FA File Offset: 0x00013AFA
	public IEnumerator Shut()
	{
		yield return null;
		while (Singleton<GlobalCam>.Instance.TransitionActive)
		{
			yield return null;
		}
		this.animator.Play("ElvClose", -1, 0f);
		base.StartCoroutine(this.BusyDelay(1f));
		yield return null;
		yield break;
	}

	// Token: 0x06000419 RID: 1049 RVA: 0x00015909 File Offset: 0x00013B09
	public void Open()
	{
		this.animator.Play("ElvOpen", -1, 0f);
	}

	// Token: 0x0600041A RID: 1050 RVA: 0x00015921 File Offset: 0x00013B21
	public void ShowResults(float time, int timeBonus)
	{
		this.QueueEnumerator(this.Results(time, timeBonus));
	}

	// Token: 0x0600041B RID: 1051 RVA: 0x00015931 File Offset: 0x00013B31
	public void QueueShop()
	{
		this.QueueEnumerator(this.OpenShop());
	}

	// Token: 0x0600041C RID: 1052 RVA: 0x0001593F File Offset: 0x00013B3F
	private IEnumerator ZoomIntro()
	{
		float scale = this.maxScale;
		this.busy = true;
		while (scale > 1f)
		{
			scale -= this.scaleSpeed * Time.unscaledDeltaTime;
			base.transform.localScale = Vector3.one * scale;
			yield return null;
		}
		scale = 1f;
		base.transform.localScale = Vector3.one * scale;
		this.busy = false;
		this.cursorInitiator.enabled = true;
		Singleton<MusicManager>.Instance.PlayMidi("Elevator", true);
		yield break;
	}

	// Token: 0x0600041D RID: 1053 RVA: 0x0001594E File Offset: 0x00013B4E
	private IEnumerator ZoomExit()
	{
		float scale = 1f;
		this.busy = true;
		Object.Destroy(this.cursorInitiator.currentCursor.gameObject);
		while (scale < this.maxScale)
		{
			scale += this.scaleSpeed * Time.unscaledDeltaTime;
			base.transform.localScale = Vector3.one * scale;
			yield return null;
		}
		scale = this.maxScale;
		base.transform.localScale = Vector3.one * scale;
		this.busy = false;
		yield break;
	}

	// Token: 0x0600041E RID: 1054 RVA: 0x0001595D File Offset: 0x00013B5D
	private IEnumerator Results(float gameTime, int timeBonus)
	{
		yield return null;
		this.busy = true;
		this.bigScreen.animator.Play("SwingDown", -1, 0f);
		float time = 1.5f;
		while (time > 0f)
		{
			time -= Time.unscaledDeltaTime;
			yield return null;
		}
		this.bigScreen.resultsText.SetActive(true);
		TMP_Text toFill = this.bigScreen.time;
		string value = "";
		int num;
		for (int i = 0; i < 6; i = num + 1)
		{
			switch (i)
			{
			case 0:
				this.bigScreen.pointsText.SetActive(true);
				this.bigScreen.points.gameObject.SetActive(true);
				toFill = this.bigScreen.points;
				value = Singleton<CoreGameManager>.Instance.GetPointsThisLevel(0).ToString();
				break;
			case 1:
				this.bigScreen.multiplierText.SetActive(true);
				this.bigScreen.multiplier.gameObject.SetActive(true);
				toFill = this.bigScreen.multiplier;
				value = this.ytpMultiplier.ToString() + "X";
				break;
			case 2:
				this.bigScreen.totalText.SetActive(true);
				this.bigScreen.total.gameObject.SetActive(true);
				toFill = this.bigScreen.total;
				value = (Singleton<CoreGameManager>.Instance.GetPointsThisLevel(0) * this.ytpMultiplier).ToString();
				break;
			case 3:
				this.bigScreen.gradeText.SetActive(true);
				this.bigScreen.grade.gameObject.SetActive(true);
				toFill = this.bigScreen.grade;
				value = Singleton<CoreGameManager>.Instance.Grade;
				break;
			case 4:
			{
				this.bigScreen.timeText.SetActive(true);
				this.bigScreen.time.gameObject.SetActive(true);
				toFill = this.bigScreen.time;
				string str = Mathf.Floor(gameTime % 60f).ToString();
				if (Mathf.Floor(gameTime % 60f) < 10f)
				{
					str = "0" + Mathf.Floor(gameTime % 60f).ToString();
				}
				value = Mathf.Floor(gameTime / 60f).ToString() + ":" + str;
				break;
			}
			}
			time = 0.5f;
			while (time > 0f)
			{
				switch (i)
				{
				case 0:
					toFill.text = Random.Range(0, 9999).ToString();
					break;
				case 1:
					toFill.text = Random.Range(1, 4).ToString() + "X";
					break;
				case 2:
					toFill.text = Random.Range(0, 9999).ToString();
					break;
				case 3:
					toFill.text = this.bigScreen.grades[Random.Range(0, this.bigScreen.grades.Length)];
					break;
				case 4:
					toFill.text = Random.Range(0, 9999).ToString();
					break;
				}
				time -= Time.unscaledDeltaTime;
				yield return null;
			}
			toFill.text = value;
			num = i;
			if (num != 3)
			{
				if (num == 4 && timeBonus > 0)
				{
					this.bigScreen.timeBonusText.SetActive(true);
					this.bigScreen.timeBonus.gameObject.SetActive(true);
					this.bigScreen.timeBonus.text = "+" + timeBonus.ToString() + " YTPs";
				}
			}
			else if (CoreGameManager.gradeBonusVal[Singleton<CoreGameManager>.Instance.GradeVal] != 0)
			{
				this.bigScreen.gradeBonusText.SetActive(true);
				this.bigScreen.gradeBonus.gameObject.SetActive(true);
				this.bigScreen.gradeBonus.text = "+" + CoreGameManager.gradeBonusVal[Singleton<CoreGameManager>.Instance.GradeVal].ToString() + " YTPs";
			}
			time = 0.5f;
			while (time > 0f)
			{
				time -= Time.unscaledDeltaTime;
				yield return null;
			}
			num = i;
		}
		time = 2f;
		while (time > 0f)
		{
			time -= Time.unscaledDeltaTime;
			yield return null;
		}
		this.bigScreen.resultsText.SetActive(false);
		this.bigScreen.time.gameObject.SetActive(false);
		this.bigScreen.points.gameObject.SetActive(false);
		this.bigScreen.total.gameObject.SetActive(false);
		this.bigScreen.grade.gameObject.SetActive(false);
		this.bigScreen.timeText.SetActive(false);
		this.bigScreen.pointsText.SetActive(false);
		this.bigScreen.gradeText.SetActive(false);
		this.bigScreen.totalText.SetActive(false);
		this.bigScreen.gradeBonusText.SetActive(false);
		this.bigScreen.timeBonusText.SetActive(false);
		this.bigScreen.timeBonus.gameObject.SetActive(false);
		this.bigScreen.gradeBonus.gameObject.SetActive(false);
		this.bigScreen.multiplierText.gameObject.SetActive(false);
		this.bigScreen.multiplier.gameObject.SetActive(false);
		this.bigScreen.animator.Play("SwingUp", -1, 0f);
		time = 1.5f;
		while (time > 0f)
		{
			time -= Time.unscaledDeltaTime;
			yield return null;
		}
		this.busy = false;
		yield break;
	}

	// Token: 0x0600041F RID: 1055 RVA: 0x0001597A File Offset: 0x00013B7A
	private IEnumerator OpenShop()
	{
		this.Open();
		this.shopButton.SetActive(true);
		this.shopping = true;
		this.busy = true;
		this.buttonAnimator.gameObject.SetActive(true);
		this.buttonAnimator.Play("ButtonRise", -1, 0f);
		this.buttonLabel.text = "Continue";
		while (this.shopping)
		{
			yield return null;
		}
		this.busy = false;
		yield break;
	}

	// Token: 0x06000420 RID: 1056 RVA: 0x00015989 File Offset: 0x00013B89
	public void ExitShop()
	{
		this.shopping = false;
		this.QueueEnumerator(this.Shut());
		this.buttonAnimator.gameObject.SetActive(false);
	}

	// Token: 0x06000421 RID: 1057 RVA: 0x000159AF File Offset: 0x00013BAF
	public void StartGame()
	{
		this.fakeShopButton.SetActive(false);
		this.Open();
		Singleton<MusicManager>.Instance.StopMidi();
		base.StartCoroutine(this.StartDelay());
	}

	// Token: 0x06000422 RID: 1058 RVA: 0x000159DC File Offset: 0x00013BDC
	public void ButtonPressed(int val)
	{
		this.buttonAnimator.Play("ButtonDrop", -1, 0f);
		if (this.shopping)
		{
			this.shopButton.gameObject.SetActive(false);
			this.fakeShopButton.gameObject.SetActive(true);
			this.shopping = false;
			this.QueueEnumerator(this.Shut());
			return;
		}
		if (this.readyToStart)
		{
			this.StartGame();
		}
	}

	// Token: 0x06000423 RID: 1059 RVA: 0x00015A4B File Offset: 0x00013C4B
	public void SkipPitstop()
	{
		this.buttonAnimator.Play("ButtonDrop", -1, 0f);
		Singleton<BaseGameManager>.Instance.LoadNextLevel();
	}

	// Token: 0x06000424 RID: 1060 RVA: 0x00015A6D File Offset: 0x00013C6D
	private IEnumerator BusyDelay(float time)
	{
		this.busy = true;
		while (time > 0f)
		{
			time -= Time.unscaledDeltaTime;
			yield return null;
		}
		this.busy = false;
		yield break;
	}

	// Token: 0x06000425 RID: 1061 RVA: 0x00015A83 File Offset: 0x00013C83
	private IEnumerator LoadDelay(float time)
	{
		while (time > 0f)
		{
			time -= Time.unscaledDeltaTime;
			yield return null;
		}
		if (this.OnLoadReady != null)
		{
			this.OnLoadReady();
		}
		yield break;
	}

	// Token: 0x06000426 RID: 1062 RVA: 0x00015A99 File Offset: 0x00013C99
	private IEnumerator StartDelay()
	{
		float time = 1f;
		while (time > 0f)
		{
			time -= Time.fixedUnscaledDeltaTime;
			yield return null;
		}
		base.StartCoroutine(this.ZoomExit());
		yield return null;
		while (this.busy)
		{
			yield return null;
		}
		Singleton<BaseGameManager>.Instance.BeginPlay();
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x1700003C RID: 60
	// (get) Token: 0x06000427 RID: 1063 RVA: 0x00015AA8 File Offset: 0x00013CA8
	public Canvas Canvas
	{
		get
		{
			return this.canvas;
		}
	}

	// Token: 0x04000463 RID: 1123
	[SerializeField]
	private Canvas canvas;

	// Token: 0x04000464 RID: 1124
	[SerializeField]
	private Animator animator;

	// Token: 0x04000465 RID: 1125
	[SerializeField]
	private Animator buttonAnimator;

	// Token: 0x04000466 RID: 1126
	[SerializeField]
	private GameObject playButton;

	// Token: 0x04000467 RID: 1127
	[SerializeField]
	private GameObject shopButton;

	// Token: 0x04000468 RID: 1128
	[SerializeField]
	private GameObject fakeShopButton;

	// Token: 0x04000469 RID: 1129
	[SerializeField]
	private GameObject skipButton;

	// Token: 0x0400046A RID: 1130
	[SerializeField]
	private BigScreen bigScreen;

	// Token: 0x0400046B RID: 1131
	[SerializeField]
	private Image[] allImages = new Image[0];

	// Token: 0x0400046C RID: 1132
	[SerializeField]
	private Image livesImage;

	// Token: 0x0400046D RID: 1133
	[SerializeField]
	private Sprite[] lifeImages = new Sprite[0];

	// Token: 0x0400046E RID: 1134
	[SerializeField]
	private TMP_Text floorText;

	// Token: 0x0400046F RID: 1135
	[SerializeField]
	private TMP_Text seedText;

	// Token: 0x04000470 RID: 1136
	[SerializeField]
	private TMP_Text buttonLabel;

	// Token: 0x04000471 RID: 1137
	[SerializeField]
	private TMP_Text errorText;

	// Token: 0x04000472 RID: 1138
	[SerializeField]
	private CursorInitiator cursorInitiator;

	// Token: 0x04000473 RID: 1139
	[SerializeField]
	private AudioManager audMan;

	// Token: 0x04000474 RID: 1140
	[SerializeField]
	private SoundObject audBuzz;

	// Token: 0x04000475 RID: 1141
	[SerializeField]
	private SoundObject audError;

	// Token: 0x04000476 RID: 1142
	private List<IEnumerator> queuedEnumerators = new List<IEnumerator>();

	// Token: 0x04000477 RID: 1143
	[SerializeField]
	private float maxScale = 5f;

	// Token: 0x04000478 RID: 1144
	[SerializeField]
	private float scaleSpeed = 5f;

	// Token: 0x04000479 RID: 1145
	private int ytpMultiplier;

	// Token: 0x0400047A RID: 1146
	private bool starting;

	// Token: 0x0400047B RID: 1147
	private bool busy;

	// Token: 0x0400047C RID: 1148
	private bool readyToStart;

	// Token: 0x0400047D RID: 1149
	private bool shopping;

	// Token: 0x0400047E RID: 1150
	private bool initialized;

	// Token: 0x0400047F RID: 1151
	private bool error;

	// Token: 0x02000349 RID: 841
	// (Invoke) Token: 0x06001B42 RID: 6978
	public delegate void OnLoadReadyHandler();
}
