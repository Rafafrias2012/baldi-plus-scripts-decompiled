using System;
using System.Collections;
using System.Collections.Generic;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000213 RID: 531
public class OptionsMenu : MonoBehaviour
{
	// Token: 0x06000BCA RID: 3018 RVA: 0x0003E104 File Offset: 0x0003C304
	private void Awake()
	{
		this.availableResolutions.Clear();
		Resolution item = default(Resolution);
		Resolution item2 = default(Resolution);
		Resolution item3 = default(Resolution);
		item.width = 480;
		item.height = 360;
		item2.width = Singleton<PlayerFileManager>.Instance.resolutionX;
		item2.height = Singleton<PlayerFileManager>.Instance.resolutionY;
		item3.width = Display.main.systemWidth;
		item3.height = Display.main.systemHeight;
		this.availableResolutions.Add(item);
		bool flag = true;
		bool flag2 = true;
		foreach (Resolution item4 in Screen.resolutions)
		{
			bool flag3 = false;
			foreach (Resolution resolution in this.availableResolutions)
			{
				if (resolution.width == item4.width && resolution.height == item4.height)
				{
					flag3 = true;
					break;
				}
			}
			if (!flag3)
			{
				this.availableResolutions.Add(item4);
			}
			if (item4.width == Singleton<PlayerFileManager>.Instance.resolutionX && item4.height == Singleton<PlayerFileManager>.Instance.resolutionY)
			{
				flag = false;
			}
			if (item4.width == item3.width && item4.height == item3.height)
			{
				flag2 = false;
			}
		}
		if (flag)
		{
			this.availableResolutions.Add(item2);
		}
		if (flag2)
		{
			this.availableResolutions.Add(item3);
		}
		for (int j = 0; j < this.availableResolutions.Count; j++)
		{
			if (this.availableResolutions[j].width < 480 || this.availableResolutions[j].height < 360)
			{
				this.availableResolutions.RemoveAt(j);
				j--;
			}
		}
		for (int k = 0; k < this.availableResolutions.Count; k++)
		{
			if (Singleton<PlayerFileManager>.Instance.resolutionX == this.availableResolutions[k].width && Singleton<PlayerFileManager>.Instance.resolutionY == this.availableResolutions[k].height)
			{
				this.currentResolution = k;
				break;
			}
		}
		this.ChangeResolution(0);
		this.UpdateNameText();
		for (int l = 0; l < this.soundAdj.Length; l++)
		{
			this.soundAdj[l].SetVal(Singleton<PlayerFileManager>.Instance.volume[l]);
		}
		this.sensitivityAdj[0].SetVal(Singleton<PlayerFileManager>.Instance.mouseCameraSensitivity);
		this.sensitivityAdj[1].SetVal(Singleton<PlayerFileManager>.Instance.mouseCursorSensitivity);
		this.sensitivityAdj[2].SetVal(Singleton<PlayerFileManager>.Instance.controllerCameraSensitivity);
		this.sensitivityAdj[3].SetVal(Singleton<PlayerFileManager>.Instance.controllerCursorSensitivity);
		this.subtitlesToggle.Set(Singleton<PlayerFileManager>.Instance.subtitles);
		this.fullScreenToggle.Set(Singleton<PlayerFileManager>.Instance.fullscreen);
		this.vsyncToggle.Set(Singleton<PlayerFileManager>.Instance.vsync);
		this.pixelFilterToggle.Set(Singleton<PlayerFileManager>.Instance.pixelFilter);
		this.flashToggle.Set(Singleton<PlayerFileManager>.Instance.reduceFlashing);
		this.rumbleToggle.Set(Singleton<PlayerFileManager>.Instance.rumble);
		if (Singleton<CoreGameManager>.Instance != null)
		{
			GameObject[] array = this.inGameDisabledCovers;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(true);
			}
		}
		this.ChangeCategory(0);
		if (SteamManager.Initialized && Singleton<InputManager>.Instance.SteamInputActive)
		{
			this.steamMapperButton.SetActive(true);
		}
	}

	// Token: 0x06000BCB RID: 3019 RVA: 0x0003E4F4 File Offset: 0x0003C6F4
	private void Update()
	{
		if (Singleton<InputManager>.Instance.RewiredControllerAvailable)
		{
			this.rewiredMapperButton.SetActive(true);
			return;
		}
		this.rewiredMapperButton.SetActive(false);
	}

	// Token: 0x06000BCC RID: 3020 RVA: 0x0003E51B File Offset: 0x0003C71B
	private void OnDisable()
	{
		this.Close();
	}

	// Token: 0x06000BCD RID: 3021 RVA: 0x0003E524 File Offset: 0x0003C724
	public void Close()
	{
		if (!this.saveDisabled)
		{
			Singleton<PlayerFileManager>.Instance.Save();
		}
		if (Singleton<MusicManager>.Instance.MidiPlaying)
		{
			this.PauseMidi();
		}
		this.dataMain.SetActive(true);
		this.confirmScreen.SetActive(false);
		this.continueScreen.SetActive(false);
	}

	// Token: 0x06000BCE RID: 3022 RVA: 0x0003E57C File Offset: 0x0003C77C
	public void ChangeCategory(int direction)
	{
		this.categories[this.currentCategory].SetActive(false);
		this.currentCategory += direction;
		if (this.currentCategory >= this.categories.Length)
		{
			this.currentCategory = 0;
		}
		else if (this.currentCategory < 0)
		{
			this.currentCategory = this.categories.Length - 1;
		}
		this.categories[this.currentCategory].SetActive(true);
		this.currentCategoryTmp.text = Singleton<LocalizationManager>.Instance.GetLocalizedText(this.categoryKeys[this.currentCategory]);
		int num = this.currentCategory - 1;
		int num2 = this.currentCategory + 1;
		if (num < 0)
		{
			num = this.categories.Length - 1;
		}
		if (num2 >= this.categories.Length)
		{
			num2 = 0;
		}
		this.previousCategoryTmp.text = Singleton<LocalizationManager>.Instance.GetLocalizedText(this.categoryKeys[num]);
		this.nextCategoryTmp.text = Singleton<LocalizationManager>.Instance.GetLocalizedText(this.categoryKeys[num2]);
	}

	// Token: 0x06000BCF RID: 3023 RVA: 0x0003E67C File Offset: 0x0003C87C
	public void VolumeChanged(int category)
	{
		Singleton<PlayerFileManager>.Instance.volume[category] = this.soundAdj[category].FloatVal;
		Singleton<PlayerFileManager>.Instance.UpdateVolume();
		this.audMan.FlushQueue(true);
		if (category == 0)
		{
			this.audMan.QueueAudio(this.audVoiceTest);
			return;
		}
		if (category == 1)
		{
			this.audMan.QueueAudio(this.audEffectTest);
			return;
		}
		if (category == 2)
		{
			if (!Singleton<MusicManager>.Instance.MidiPlaying)
			{
				Singleton<MusicManager>.Instance.PlayMidi(this.musicTest, false);
				return;
			}
			Singleton<MusicManager>.Instance.PauseMidi(false);
			this.midiStopperTime = 5f;
			if (!this.midiStopperRunning)
			{
				base.StartCoroutine(this.MidiPauser());
			}
		}
	}

	// Token: 0x06000BD0 RID: 3024 RVA: 0x0003E730 File Offset: 0x0003C930
	private IEnumerator MidiPauser()
	{
		this.midiStopperRunning = true;
		while (this.midiStopperTime > 0f)
		{
			this.midiStopperTime -= Time.unscaledDeltaTime;
			yield return null;
		}
		this.PauseMidi();
		yield break;
	}

	// Token: 0x06000BD1 RID: 3025 RVA: 0x0003E73F File Offset: 0x0003C93F
	private void PauseMidi()
	{
		Singleton<MusicManager>.Instance.PauseMidi(true);
		base.StopAllCoroutines();
		this.midiStopperTime = 0f;
		this.midiStopperRunning = false;
	}

	// Token: 0x06000BD2 RID: 3026 RVA: 0x0003E764 File Offset: 0x0003C964
	public void FlashingChanged()
	{
		Singleton<PlayerFileManager>.Instance.reduceFlashing = this.flashToggle.Value;
	}

	// Token: 0x06000BD3 RID: 3027 RVA: 0x0003E77B File Offset: 0x0003C97B
	public void SubtitlesChanged()
	{
		Singleton<PlayerFileManager>.Instance.subtitles = this.subtitlesToggle.Value;
	}

	// Token: 0x06000BD4 RID: 3028 RVA: 0x0003E792 File Offset: 0x0003C992
	public void RumbleChanged()
	{
		Singleton<PlayerFileManager>.Instance.rumble = this.rumbleToggle.Value;
	}

	// Token: 0x06000BD5 RID: 3029 RVA: 0x0003E7AC File Offset: 0x0003C9AC
	public void ChangeResolution(int direction)
	{
		this.currentResolution += direction;
		if (this.currentResolution >= this.availableResolutions.Count)
		{
			this.currentResolution = 0;
		}
		else if (this.currentResolution < 0)
		{
			this.currentResolution = this.availableResolutions.Count - 1;
		}
		this.resolutionTMP.text = this.availableResolutions[this.currentResolution].width.ToString() + "x" + this.availableResolutions[this.currentResolution].height.ToString();
	}

	// Token: 0x06000BD6 RID: 3030 RVA: 0x0003E858 File Offset: 0x0003CA58
	public void ApplyGraphics()
	{
		Singleton<PlayerFileManager>.Instance.resolutionX = this.availableResolutions[this.currentResolution].width;
		Singleton<PlayerFileManager>.Instance.resolutionY = this.availableResolutions[this.currentResolution].height;
		Singleton<PlayerFileManager>.Instance.fullscreen = this.fullScreenToggle.Value;
		Singleton<PlayerFileManager>.Instance.vsync = this.vsyncToggle.Value;
		Singleton<PlayerFileManager>.Instance.pixelFilter = this.pixelFilterToggle.Value;
		Singleton<PlayerFileManager>.Instance.UpdateResolution();
		if (Singleton<PlayerFileManager>.Instance.vsync)
		{
			QualitySettings.vSyncCount = 1;
			return;
		}
		QualitySettings.vSyncCount = 0;
	}

	// Token: 0x06000BD7 RID: 3031 RVA: 0x0003E910 File Offset: 0x0003CB10
	private void UpdateNameText()
	{
		TMP_Text[] array = this.nameTmp;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].text = Singleton<PlayerFileManager>.Instance.fileName;
		}
	}

	// Token: 0x06000BD8 RID: 3032 RVA: 0x0003E944 File Offset: 0x0003CB44
	public void SensitivityChanged(int type)
	{
		float floatVal = this.sensitivityAdj[type].FloatVal;
		switch (type)
		{
		case 0:
			Singleton<PlayerFileManager>.Instance.mouseCameraSensitivity = floatVal;
			return;
		case 1:
			Singleton<PlayerFileManager>.Instance.mouseCursorSensitivity = floatVal;
			return;
		case 2:
			Singleton<PlayerFileManager>.Instance.controllerCameraSensitivity = floatVal;
			return;
		case 3:
			Singleton<PlayerFileManager>.Instance.controllerCursorSensitivity = floatVal;
			return;
		default:
			return;
		}
	}

	// Token: 0x06000BD9 RID: 3033 RVA: 0x0003E9A5 File Offset: 0x0003CBA5
	public void ChangeSensitivityCategory()
	{
		this.sensitivityCategories[this.currentSensitivityCategory].gameObject.SetActive(false);
		this.currentSensitivityCategory = 1 - this.currentSensitivityCategory;
		this.sensitivityCategories[this.currentSensitivityCategory].gameObject.SetActive(true);
	}

	// Token: 0x06000BDA RID: 3034 RVA: 0x0003E9E8 File Offset: 0x0003CBE8
	public void ChangeControlCategory(int direction)
	{
		this.controlCategories[this.currentControlCategory].SetActive(false);
		this.currentControlCategory += direction;
		if (this.currentControlCategory >= this.controlCategories.Length)
		{
			this.currentControlCategory = 0;
		}
		else if (this.currentControlCategory < 0)
		{
			this.currentControlCategory = this.controlCategories.Length - 1;
		}
		this.controlCategories[this.currentControlCategory].SetActive(true);
	}

	// Token: 0x06000BDB RID: 3035 RVA: 0x0003EA5B File Offset: 0x0003CC5B
	public void OpenRewiredMapper(bool opening)
	{
		Singleton<InputManager>.Instance.FreezeInput(opening);
		if (!opening)
		{
			Singleton<InputManager>.Instance.CheckMouseMap();
		}
	}

	// Token: 0x06000BDC RID: 3036 RVA: 0x0003EA75 File Offset: 0x0003CC75
	public void OpenSteamMapper()
	{
		SteamInput.ShowBindingPanel(Singleton<InputManager>.Instance.LastUsedInputHandle);
	}

	// Token: 0x06000BDD RID: 3037 RVA: 0x0003EA88 File Offset: 0x0003CC88
	public void OptionConfirmed()
	{
		switch (this.currentConfirmAction)
		{
		case ConfirmAction.Null:
			return;
		case ConfirmAction.DeleteFile:
			this.DeleteFile();
			return;
		case ConfirmAction.ResetEndless:
			this.ResetEndless();
			return;
		case ConfirmAction.ResetTrip:
			this.ResetTrip();
			return;
		case ConfirmAction.ChangeName:
			this.ChangeName();
			return;
		default:
			return;
		}
	}

	// Token: 0x06000BDE RID: 3038 RVA: 0x0003EAD4 File Offset: 0x0003CCD4
	public void SetConfirmAction(int actionId)
	{
		this.currentConfirmAction = (ConfirmAction)actionId;
		switch (this.currentConfirmAction)
		{
		case ConfirmAction.Null:
			this.confirmTmp.text = "How did we get here?";
			return;
		case ConfirmAction.DeleteFile:
			this.confirmTmp.text = Singleton<LocalizationManager>.Instance.GetLocalizedText("Opt_ConfirmDelete");
			this.deleteName.SetActive(true);
			return;
		case ConfirmAction.ResetEndless:
			this.confirmTmp.text = Singleton<LocalizationManager>.Instance.GetLocalizedText("Opt_ConfirmEndlessReset");
			this.deleteName.SetActive(false);
			return;
		case ConfirmAction.ResetTrip:
			this.confirmTmp.text = Singleton<LocalizationManager>.Instance.GetLocalizedText("Opt_ConfirmTripReset");
			this.deleteName.SetActive(false);
			return;
		case ConfirmAction.ChangeName:
			this.confirmTmp.text = Singleton<LocalizationManager>.Instance.GetLocalizedText("Opt_ConfirmNameChange");
			this.deleteName.SetActive(false);
			return;
		default:
			return;
		}
	}

	// Token: 0x06000BDF RID: 3039 RVA: 0x0003EBB6 File Offset: 0x0003CDB6
	private void ResetEndless()
	{
		Singleton<HighScoreManager>.Instance.SetDefaultsEndless();
		Singleton<HighScoreManager>.Instance.Save();
		this.continueScreen.SetActive(true);
		this.continueTmp.text = Singleton<LocalizationManager>.Instance.GetLocalizedText("Opt_EndlessReset");
	}

	// Token: 0x06000BE0 RID: 3040 RVA: 0x0003EBF2 File Offset: 0x0003CDF2
	private void ResetTrip()
	{
		Singleton<HighScoreManager>.Instance.SetDefaultsTrips();
		Singleton<HighScoreManager>.Instance.Save();
		this.continueScreen.SetActive(true);
		this.continueTmp.text = Singleton<LocalizationManager>.Instance.GetLocalizedText("Opt_TripReset");
	}

	// Token: 0x06000BE1 RID: 3041 RVA: 0x0003EC30 File Offset: 0x0003CE30
	private void DeleteFile()
	{
		if (this.nameManager != null)
		{
			this.nameManager.DeleteName(0);
			this.saveDisabled = true;
		}
		this.deleteScreen.SetActive(true);
		Singleton<InputManager>.Instance.FreezeInput(true);
		base.StartCoroutine(this.DeleteAnimation());
	}

	// Token: 0x06000BE2 RID: 3042 RVA: 0x0003EC82 File Offset: 0x0003CE82
	private IEnumerator DeleteAnimation()
	{
		float time = 1f;
		while (time > 0f)
		{
			time -= Time.unscaledDeltaTime;
			yield return null;
		}
		this.audMan.PlaySingle(this.audDelete);
		Singleton<GlobalCam>.Instance.Transition(UiTransition.Dither, 0.25f);
		this.deleteCover.SetActive(true);
		while (Singleton<GlobalCam>.Instance.TransitionActive)
		{
			yield return null;
		}
		time = 1f;
		while (time > 0f)
		{
			time -= Time.unscaledDeltaTime;
			yield return null;
		}
		Singleton<GlobalStateManager>.Instance.skipNameEntry = false;
		Singleton<InputManager>.Instance.FreezeInput(false);
		SceneManager.LoadScene("MainMenu");
		yield break;
	}

	// Token: 0x06000BE3 RID: 3043 RVA: 0x0003EC91 File Offset: 0x0003CE91
	private void ChangeName()
	{
	}

	// Token: 0x04000E44 RID: 3652
	[SerializeField]
	private NameManager nameManager;

	// Token: 0x04000E45 RID: 3653
	[SerializeField]
	private AdjustmentBars[] soundAdj = new AdjustmentBars[0];

	// Token: 0x04000E46 RID: 3654
	[SerializeField]
	private AudioManager audMan;

	// Token: 0x04000E47 RID: 3655
	[SerializeField]
	private SoundObject audEffectTest;

	// Token: 0x04000E48 RID: 3656
	[SerializeField]
	private SoundObject audVoiceTest;

	// Token: 0x04000E49 RID: 3657
	[SerializeField]
	private SoundObject audDelete;

	// Token: 0x04000E4A RID: 3658
	[SerializeField]
	private string musicTest = "titleFixed";

	// Token: 0x04000E4B RID: 3659
	[SerializeField]
	private AdjustmentBars[] sensitivityAdj = new AdjustmentBars[0];

	// Token: 0x04000E4C RID: 3660
	[SerializeField]
	private GameObject[] categories = new GameObject[0];

	// Token: 0x04000E4D RID: 3661
	[SerializeField]
	private GameObject[] sensitivityCategories = new GameObject[0];

	// Token: 0x04000E4E RID: 3662
	[SerializeField]
	private GameObject[] controlCategories = new GameObject[0];

	// Token: 0x04000E4F RID: 3663
	[SerializeField]
	private GameObject[] inGameDisabledCovers = new GameObject[0];

	// Token: 0x04000E50 RID: 3664
	[SerializeField]
	private string[] categoryKeys = new string[0];

	// Token: 0x04000E51 RID: 3665
	[SerializeField]
	private GameObject steamMapperButton;

	// Token: 0x04000E52 RID: 3666
	[SerializeField]
	private GameObject rewiredMapperButton;

	// Token: 0x04000E53 RID: 3667
	[SerializeField]
	private GameObject deleteScreen;

	// Token: 0x04000E54 RID: 3668
	[SerializeField]
	private GameObject deleteCover;

	// Token: 0x04000E55 RID: 3669
	[SerializeField]
	private GameObject deleteName;

	// Token: 0x04000E56 RID: 3670
	[SerializeField]
	private GameObject confirmScreen;

	// Token: 0x04000E57 RID: 3671
	[SerializeField]
	private GameObject continueScreen;

	// Token: 0x04000E58 RID: 3672
	[SerializeField]
	private GameObject dataMain;

	// Token: 0x04000E59 RID: 3673
	[SerializeField]
	private MenuToggle subtitlesToggle;

	// Token: 0x04000E5A RID: 3674
	[SerializeField]
	private MenuToggle fullScreenToggle;

	// Token: 0x04000E5B RID: 3675
	[SerializeField]
	private MenuToggle vsyncToggle;

	// Token: 0x04000E5C RID: 3676
	[SerializeField]
	private MenuToggle pixelFilterToggle;

	// Token: 0x04000E5D RID: 3677
	[SerializeField]
	private MenuToggle flashToggle;

	// Token: 0x04000E5E RID: 3678
	[SerializeField]
	private MenuToggle rumbleToggle;

	// Token: 0x04000E5F RID: 3679
	[SerializeField]
	private TMP_Text[] nameTmp = new TMP_Text[0];

	// Token: 0x04000E60 RID: 3680
	[SerializeField]
	private TMP_Text currentCategoryTmp;

	// Token: 0x04000E61 RID: 3681
	[SerializeField]
	private TMP_Text previousCategoryTmp;

	// Token: 0x04000E62 RID: 3682
	[SerializeField]
	private TMP_Text nextCategoryTmp;

	// Token: 0x04000E63 RID: 3683
	[SerializeField]
	private TMP_Text resolutionTMP;

	// Token: 0x04000E64 RID: 3684
	[SerializeField]
	private TMP_Text confirmTmp;

	// Token: 0x04000E65 RID: 3685
	[SerializeField]
	private TMP_Text continueTmp;

	// Token: 0x04000E66 RID: 3686
	private List<Resolution> availableResolutions = new List<Resolution>();

	// Token: 0x04000E67 RID: 3687
	private ConfirmAction currentConfirmAction;

	// Token: 0x04000E68 RID: 3688
	private float midiStopperTime;

	// Token: 0x04000E69 RID: 3689
	private int currentCategory;

	// Token: 0x04000E6A RID: 3690
	private int currentResolution;

	// Token: 0x04000E6B RID: 3691
	private int currentSensitivityCategory;

	// Token: 0x04000E6C RID: 3692
	private int currentControlCategory;

	// Token: 0x04000E6D RID: 3693
	private bool midiStopperRunning;

	// Token: 0x04000E6E RID: 3694
	private bool saveDisabled;
}
