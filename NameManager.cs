using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Steamworks;
using UnityEngine;

// Token: 0x020001DA RID: 474
public class NameManager : MonoBehaviour
{
	// Token: 0x06000AB4 RID: 2740 RVA: 0x00038454 File Offset: 0x00036654
	private void Awake()
	{
		if (NameManager.nm == null)
		{
			NameManager.nm = this;
		}
		else if (NameManager.nm != this)
		{
			Object.Destroy(base.gameObject);
		}
		this.Load();
		if (SteamManager.Initialized)
		{
			this.m_TextInputDismissed = Callback<GamepadTextInputDismissed_t>.Create(new Callback<GamepadTextInputDismissed_t>.DispatchDelegate(this.OnGamepadTextInputDismissed_t));
		}
	}

	// Token: 0x06000AB5 RID: 2741 RVA: 0x000384B4 File Offset: 0x000366B4
	private void Update()
	{
		if (!this.loadingFile && !this.virtualKeyboardActive)
		{
			if (!this.errorOpen)
			{
				if (Input.anyKeyDown && Input.inputString.Length > 0 && char.IsLetter(Input.inputString, 0))
				{
					if (this.nameCount < 8)
					{
						this.enteringNewName = true;
						this.newFileButton.SetActive(false);
					}
					if (this.newName.Length < 8)
					{
						this.newName += Input.inputString[0].ToString();
					}
				}
				if (this.enteringNewName)
				{
					if (Input.GetKeyDown(KeyCode.Backspace))
					{
						this.newName = this.newName.Remove(this.newName.Length - 1);
						if (this.newName.Length <= 0)
						{
							this.enteringNewName = false;
							if (SteamManager.Initialized && (SteamUtils.IsSteamInBigPictureMode() || SteamUtils.IsSteamRunningOnSteamDeck()))
							{
								this.newFileButton.SetActive(true);
							}
						}
					}
					this.buttons[this.nameCount].text.text = this.newName;
					if (Input.GetKeyDown(KeyCode.Return))
					{
						this.NameClicked(this.nameCount);
						return;
					}
				}
			}
			else if (Input.anyKeyDown)
			{
				this.CheckForSaves();
				this.Save();
				this.UpdateState();
				this.CloseError();
			}
		}
	}

	// Token: 0x06000AB6 RID: 2742 RVA: 0x0003860C File Offset: 0x0003680C
	public void AddName()
	{
		this.nameCount++;
		for (int i = this.nameCount - 1; i > 0; i--)
		{
			this.nameList[i] = this.nameList[i - 1];
		}
		this.nameList[0] = this.newName;
		this.Save();
		Singleton<PlayerFileManager>.Instance.fileName = this.nameList[0];
		Singleton<PlayerFileManager>.Instance.Load();
		this.LoadName(0);
		this.enteringNewName = false;
		this.audSource.clip = this.audWelcome;
		this.audSource.Play();
	}

	// Token: 0x06000AB7 RID: 2743 RVA: 0x000386A8 File Offset: 0x000368A8
	public void NameClicked(int fileNo)
	{
		if (!this.loadingFile && !this.errorOpen)
		{
			if (fileNo < this.nameCount)
			{
				this.LoadName(fileNo);
				return;
			}
			if (fileNo == this.nameCount && this.enteringNewName)
			{
				if (!this.NameExists(this.newName))
				{
					this.AddName();
					return;
				}
				Debug.Log("Name exists");
			}
		}
	}

	// Token: 0x06000AB8 RID: 2744 RVA: 0x00038708 File Offset: 0x00036908
	private bool NameExists(string name)
	{
		bool result = false;
		for (int i = 0; i < this.nameCount; i++)
		{
			Debug.Log("Dupe name check " + i.ToString());
			if (string.Equals(name, this.nameList[i], StringComparison.OrdinalIgnoreCase))
			{
				result = true;
				break;
			}
		}
		return result;
	}

	// Token: 0x06000AB9 RID: 2745 RVA: 0x00038754 File Offset: 0x00036954
	public void DeleteName(int fileNo)
	{
		Singleton<PlayerFileManager>.Instance.Delete(this.nameList[fileNo]);
		for (int i = fileNo; i < this.nameCount - 1; i++)
		{
			this.nameList[i] = this.nameList[i + 1];
		}
		this.nameList[this.nameCount - 1] = "";
		this.nameCount--;
		this.UpdateState();
		this.Save();
	}

	// Token: 0x06000ABA RID: 2746 RVA: 0x000387C8 File Offset: 0x000369C8
	public void LoadName(int fileNo)
	{
		this.loadingFile = true;
		this.audSource.Stop();
		string text = this.nameList[fileNo];
		for (int i = fileNo; i > 0; i--)
		{
			this.nameList[i] = this.nameList[i - 1];
		}
		this.nameList[0] = text;
		this.saveComplete = false;
		this.Save();
		base.StartCoroutine(this.LoadDelay(text));
	}

	// Token: 0x06000ABB RID: 2747 RVA: 0x00038832 File Offset: 0x00036A32
	private IEnumerator LoadDelay(string selectedName)
	{
		yield return null;
		while (this.audSource.isPlaying)
		{
			yield return null;
		}
		Singleton<PlayerFileManager>.Instance.fileName = selectedName;
		if (!Singleton<PlayerFileManager>.Instance.Load())
		{
			this.DeleteName(0);
			this.OpenError();
			this.loadingFile = false;
			yield break;
		}
		while (!this.saveComplete)
		{
			yield return null;
		}
		Singleton<InputManager>.Instance.FreezeInput(false);
		Singleton<GlobalCam>.Instance.Transition(UiTransition.Dither, 0.01666667f);
		this.canvas.SetActive(false);
		this.mainMenuCanvas.SetActive(true);
		Singleton<GlobalStateManager>.Instance.skipNameEntry = true;
		yield break;
	}

	// Token: 0x06000ABC RID: 2748 RVA: 0x00038848 File Offset: 0x00036A48
	private void OpenError()
	{
		this.errorScreen.SetActive(true);
		this.errorOpen = true;
	}

	// Token: 0x06000ABD RID: 2749 RVA: 0x0003885D File Offset: 0x00036A5D
	private void CloseError()
	{
		this.errorScreen.SetActive(false);
		this.errorOpen = false;
	}

	// Token: 0x06000ABE RID: 2750 RVA: 0x00038874 File Offset: 0x00036A74
	public void UpdateState()
	{
		for (int i = 0; i < this.buttons.Length; i++)
		{
			this.buttons[i].UpdateState();
		}
	}

	// Token: 0x06000ABF RID: 2751 RVA: 0x000388A4 File Offset: 0x00036AA4
	public void Save()
	{
		string path = Application.persistentDataPath + "/NameList.lst";
		if (File.Exists(path))
		{
			File.Delete(path);
		}
		NameListData nameListData = new NameListData();
		nameListData.nameList = this.nameList;
		nameListData.nameCount = this.nameCount;
		nameListData.saveVersion = this.saveVersion;
		File.WriteAllText(path, JsonUtility.ToJson(nameListData));
		Debug.Log("Saved JSON: " + JsonUtility.ToJson(nameListData));
		this.saveComplete = true;
		Debug.Log("Names saved");
	}

	// Token: 0x06000AC0 RID: 2752 RVA: 0x0003892C File Offset: 0x00036B2C
	public void Load()
	{
		string path = Application.persistentDataPath + "/NameList.dat";
		if (File.Exists(path) && !File.Exists(Application.persistentDataPath + "/NameList.lst"))
		{
			Debug.Log("Converting old saves to new format!");
			bool flag = true;
			Debug.Log("Name file exists");
			using (FileStream fileStream = File.OpenRead(path))
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				NameListData nameListData;
				try
				{
					nameListData = (NameListData)binaryFormatter.Deserialize(fileStream);
				}
				catch
				{
					nameListData = new NameListData();
					flag = false;
				}
				fileStream.Close();
				if (flag)
				{
					this.nameList = nameListData.nameList;
					this.nameCount = nameListData.nameCount;
				}
			}
			File.Delete(path);
			this.Save();
			for (int i = 0; i < this.nameCount; i++)
			{
				if (File.Exists(Application.persistentDataPath + "/PlayerFile_" + this.nameList[i] + ".dat"))
				{
					Singleton<PlayerFileManager>.Instance.UpdateOldFile(this.nameList[i]);
					File.Delete(Application.persistentDataPath + "/PlayerFile_" + this.nameList[i] + ".dat");
				}
			}
			if (File.Exists(Application.persistentDataPath + "/HighScores.dat"))
			{
				File.Delete(Application.persistentDataPath + "/HighScores.dat");
			}
			if (File.Exists(Application.persistentDataPath + "/PlayerFile_!UnassignedFile.dat"))
			{
				File.Delete(Application.persistentDataPath + "/PlayerFile_!UnassignedFile.dat");
			}
			Singleton<PlayerFileManager>.Instance.fileName = "!UnassignedFile";
		}
		path = Application.persistentDataPath + "/NameList.lst";
		if (File.Exists(path))
		{
			bool flag2 = true;
			Debug.Log("Name file exists");
			NameListData nameListData2;
			try
			{
				nameListData2 = JsonUtility.FromJson<NameListData>(File.ReadAllText(path));
			}
			catch
			{
				nameListData2 = new NameListData();
				flag2 = false;
			}
			if (nameListData2 == null || nameListData2.nameList == null || nameListData2.nameList.Length == 0 || nameListData2.nameList.Length != 8)
			{
				nameListData2 = new NameListData();
				flag2 = false;
			}
			if (!flag2)
			{
				Debug.LogWarning("NameList file was not loaded successfully! Baldi's Basics Plus will look for existing save files and create a new NameList.");
				this.CheckForSaves();
				this.Save();
			}
			else
			{
				this.nameList = nameListData2.nameList;
				this.nameCount = nameListData2.nameCount;
			}
		}
		else
		{
			Debug.Log("Name file does not exist. Setting defaults/Checking for existing saves.");
			this.CheckForSaves();
			this.Save();
		}
		bool flag3 = false;
		if (SteamManager.Initialized && (SteamUtils.IsSteamInBigPictureMode() || SteamUtils.IsSteamRunningOnSteamDeck()) && this.nameCount < this.nameList.Length)
		{
			this.newFileButton.SetActive(true);
			this.newFileButton.transform.position = this.buttons[this.nameCount].transform.position;
			flag3 = true;
		}
		if (this.nameCount <= 0 && !flag3)
		{
			this.instructions.SetActive(true);
		}
	}

	// Token: 0x06000AC1 RID: 2753 RVA: 0x00038C18 File Offset: 0x00036E18
	private void CheckForSaves()
	{
		Debug.Log("Checking for existing saves.");
		this.nameCount = 0;
		FileInfo[] files = new DirectoryInfo(Application.persistentDataPath).GetFiles("*.sav*");
		Debug.Log("Found " + files.Length.ToString() + " .sav files in save directory.");
		int num = 0;
		while (num < files.Length && this.nameCount < 8)
		{
			if (files[num].Name.StartsWith("PlayerFile_") && files[num].Name.Length < 24 && files[num].Name.Length > 15)
			{
				Debug.Log("File '" + files[num].Name + "' appears to be a valid PlayerFile.");
				string text = files[num].Name.Remove(0, 11);
				text = text.Replace(".sav", "");
				bool flag = true;
				for (int i = 0; i < text.Length; i++)
				{
					if (!char.IsLetter(text[i]))
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					Debug.Log("File '" + text + "' had all letters and so is valid and will be added to the name list.");
					this.nameList[this.nameCount] = text;
					this.nameCount++;
				}
				else
				{
					Debug.Log("File '" + text + "' did not have all letters and so will not be added to the name list.");
				}
			}
			else
			{
				Debug.Log("File '" + files[num].Name + "' does not appear to be a valid PlayerFile.");
			}
			num++;
		}
		Debug.Log("Save file check complete!");
	}

	// Token: 0x06000AC2 RID: 2754 RVA: 0x00038DA4 File Offset: 0x00036FA4
	public void NewFileButtonPressed()
	{
		if (SteamManager.Initialized && (SteamUtils.IsSteamInBigPictureMode() || SteamUtils.IsSteamRunningOnSteamDeck()) && SteamUtils.ShowGamepadTextInput(EGamepadTextInputMode.k_EGamepadTextInputModeNormal, EGamepadTextInputLineMode.k_EGamepadTextInputLineModeSingleLine, Singleton<LocalizationManager>.Instance.GetLocalizedText("Steam_NewFile"), 8U, ""))
		{
			this.virtualKeyboardActive = true;
			Singleton<InputManager>.Instance.FreezeInput(true);
		}
	}

	// Token: 0x06000AC3 RID: 2755 RVA: 0x00038DF8 File Offset: 0x00036FF8
	private void OnGamepadTextInputDismissed_t(GamepadTextInputDismissed_t pCallback)
	{
		if (pCallback.m_bSubmitted)
		{
			string text = "";
			SteamUtils.GetEnteredGamepadTextInput(out text, pCallback.m_unSubmittedText);
			this.newName = "";
			int num = 0;
			while (num < text.Length && this.newName.Length < 8)
			{
				if (char.IsLetter(text[num]))
				{
					this.newName += text[num].ToString();
				}
				num++;
			}
			if (this.newName.Length > 0)
			{
				if (!this.NameExists(this.newName))
				{
					this.enteringNewName = true;
					this.newFileButton.SetActive(false);
					this.NameClicked(this.nameCount);
					this.virtualKeyboardActive = false;
					return;
				}
				base.StartCoroutine(this.KeyboardReset());
				return;
			}
		}
		else
		{
			Singleton<InputManager>.Instance.FreezeInput(false);
			this.virtualKeyboardActive = false;
		}
	}

	// Token: 0x06000AC4 RID: 2756 RVA: 0x00038EDF File Offset: 0x000370DF
	private IEnumerator KeyboardReset()
	{
		float time = 1f;
		while (time > 0f)
		{
			time -= Time.unscaledDeltaTime;
			yield return null;
		}
		SteamUtils.ShowGamepadTextInput(EGamepadTextInputMode.k_EGamepadTextInputModeNormal, EGamepadTextInputLineMode.k_EGamepadTextInputLineModeSingleLine, Singleton<LocalizationManager>.Instance.GetLocalizedText("Steam_DupeFile"), 8U, "");
		yield break;
	}

	// Token: 0x04000C3F RID: 3135
	public static NameManager nm;

	// Token: 0x04000C40 RID: 3136
	public NameButton[] buttons;

	// Token: 0x04000C41 RID: 3137
	public GameObject errorScreen;

	// Token: 0x04000C42 RID: 3138
	public GameObject instructions;

	// Token: 0x04000C43 RID: 3139
	public GameObject newFileButton;

	// Token: 0x04000C44 RID: 3140
	public GameObject canvas;

	// Token: 0x04000C45 RID: 3141
	public GameObject mainMenuCanvas;

	// Token: 0x04000C46 RID: 3142
	private Callback<GamepadTextInputDismissed_t> m_TextInputDismissed;

	// Token: 0x04000C47 RID: 3143
	public string[] nameList = new string[8];

	// Token: 0x04000C48 RID: 3144
	private string newName = "";

	// Token: 0x04000C49 RID: 3145
	public int saveVersion = 1;

	// Token: 0x04000C4A RID: 3146
	public int nameCount;

	// Token: 0x04000C4B RID: 3147
	private bool enteringNewName;

	// Token: 0x04000C4C RID: 3148
	private bool loadingFile;

	// Token: 0x04000C4D RID: 3149
	private bool saveComplete;

	// Token: 0x04000C4E RID: 3150
	private bool errorOpen;

	// Token: 0x04000C4F RID: 3151
	private bool virtualKeyboardActive;

	// Token: 0x04000C50 RID: 3152
	[SerializeField]
	private AudioSource audSource;

	// Token: 0x04000C51 RID: 3153
	[SerializeField]
	private AudioClip audWelcome;
}
