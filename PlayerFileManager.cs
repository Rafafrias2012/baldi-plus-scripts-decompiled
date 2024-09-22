using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityCipher;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x02000129 RID: 297
public class PlayerFileManager : Singleton<PlayerFileManager>
{
	// Token: 0x06000722 RID: 1826 RVA: 0x00023DE8 File Offset: 0x00021FE8
	public void Start()
	{
		this.SetDefaults();
		Application.targetFrameRate = -1;
		QualitySettings.vSyncCount = 1;
		if (PlayerPrefs.GetString("lastFile") != "" && File.Exists(Application.persistentDataPath + "/PlayerFile_" + PlayerPrefs.GetString("lastFile") + ".sav"))
		{
			this.fileName = PlayerPrefs.GetString("lastFile");
		}
		else
		{
			this.fileName = "!UnassignedFile";
		}
		if (PlayerPrefs.GetInt("ControlMapVersion") < 2)
		{
			this.SetDefaultControls();
		}
		PlayerPrefs.SetInt("ControlMapVersion", 2);
		if (this.loadOnAwake)
		{
			this.Load();
		}
	}

	// Token: 0x06000723 RID: 1827 RVA: 0x00023E8C File Offset: 0x0002208C
	public bool Load()
	{
		string path = Application.persistentDataPath + "/PlayerFile_" + this.fileName + ".sav";
		if (!File.Exists(path))
		{
			Debug.Log("Save data for " + this.fileName + " does not exist. Setting defaults.");
			this.ResetSaveData();
			this.SetDefaults();
			this.UpdateResolution();
			this.Save();
			return false;
		}
		bool flag = true;
		bool flag2 = true;
		PlayerPrefs.SetString("lastFile", this.fileName);
		PlayerFileData playerFileData;
		try
		{
			playerFileData = JsonUtility.FromJson<PlayerFileData>(RijndaelEncryption.Decrypt(File.ReadAllText(path), this.fileName));
		}
		catch
		{
			playerFileData = new PlayerFileData();
			flag = false;
		}
		if (!flag)
		{
			this.SetDefaults();
			this.ResetSaveData();
			this.UpdateResolution();
			Debug.Log("Loading save data for file " + this.fileName + " unsuccessful!");
			return false;
		}
		Debug.Log("Save data for file " + this.fileName + " loaded successfully!");
		Debug.Log("Save file version = " + playerFileData.saveVersion.ToString());
		this.foundChars = playerFileData.foundChars;
		this.foundEvnts = playerFileData.foundEvnts;
		this.foundItems = playerFileData.foundItems;
		this.foundObstcls = playerFileData.foundObstcls;
		this.foundTrips = playerFileData.foundTrips;
		this.wonChallenges = playerFileData.wonChallenges;
		this.clearedLevels = playerFileData.clearedLevels;
		this.reduceFlashing = playerFileData.reduceFlashing;
		Singleton<AchievementManager>.Instance.Load(playerFileData.achievementData);
		this.savedGameData = playerFileData.savedGameData;
		path = Application.persistentDataPath + "/PlayerConfig_" + this.fileName + ".cfg";
		if (Mathf.RoundToInt(playerFileData.saveVersion) <= 1)
		{
			Debug.Log("Loading settings data from data file, as it's from an older version. Settings will be saved and loaded from separate player config file from now on.");
			this.SetDefaults();
			this.Save();
		}
		else if (File.Exists(path))
		{
			FileConfigData fileConfigData;
			try
			{
				fileConfigData = JsonUtility.FromJson<FileConfigData>(File.ReadAllText(path));
			}
			catch
			{
				fileConfigData = new FileConfigData();
				flag2 = false;
				Debug.Log("Loading config data for file " + this.fileName + " unsuccessful!");
			}
			if (flag2)
			{
				Debug.Log("Config for file " + this.fileName + " loaded successfully!");
				this.currentLanguage = fileConfigData.language;
				this.mouseCameraSensitivity = fileConfigData.mouseCameraSensitivity;
				this.mouseCursorSensitivity = fileConfigData.mouseCursorSensitivity;
				this.controllerCameraSensitivity = fileConfigData.controllerCameraSensitivity;
				this.controllerCursorSensitivity = fileConfigData.controllerCursorSensitivity;
				this.volume[0] = fileConfigData.volume[0];
				this.volume[1] = fileConfigData.volume[1];
				this.volume[2] = fileConfigData.volume[2];
				this.resolutionX = fileConfigData.resolutionX;
				this.resolutionY = fileConfigData.resolutionY;
				if (this.resolutionX < 480 || this.resolutionY < 360)
				{
					this.resolutionX = Mathf.Max(Display.main.systemWidth, 480);
					this.resolutionY = Mathf.Max(Display.main.systemHeight, 360);
				}
				this.rumble = fileConfigData.rumble;
				this.analogMovement = fileConfigData.analogMovement;
				this.antiAnnoyance = fileConfigData.antiAnnoyance;
				this.subtitles = fileConfigData.subtitles;
				this.crtFilter = fileConfigData.crtFilter;
				this.fullscreen = fileConfigData.fullscreen;
				this.vsync = fileConfigData.vsync;
				this.pixelFilter = fileConfigData.pixelFilter;
				this.controllers = fileConfigData.controllers;
			}
			else
			{
				this.Save();
			}
		}
		else
		{
			Debug.Log("No config file for " + this.fileName + " found. A new one will be created on save. Settings will be reset to default.");
			this.Save();
		}
		this.UpdateResolution();
		if (this.vsync)
		{
			QualitySettings.vSyncCount = 1;
		}
		else
		{
			QualitySettings.vSyncCount = 0;
		}
		this.UpdateVolume();
		return true;
	}

	// Token: 0x06000724 RID: 1828 RVA: 0x0002426C File Offset: 0x0002246C
	public void UpdateOldFile(string fileName)
	{
		string path = Application.persistentDataPath + "/PlayerFile_" + fileName + ".dat";
		if (File.Exists(path))
		{
			bool flag = true;
			PlayerPrefs.SetString("lastFile", fileName);
			using (FileStream fileStream = File.OpenRead(path))
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				PlayerFileData playerFileData;
				try
				{
					playerFileData = (PlayerFileData)binaryFormatter.Deserialize(fileStream);
				}
				catch
				{
					playerFileData = new PlayerFileData();
					flag = false;
				}
				fileStream.Close();
				if (flag)
				{
					this.subLanguage = playerFileData.subLanguage;
					this.textureLanguage = playerFileData.textureLanguage;
					this.speechLanguage = playerFileData.speechLangauge;
					this.mouseCameraSensitivity = playerFileData.mouseCameraSensitivity;
					this.volume[0] = playerFileData.volume[0];
					this.volume[1] = playerFileData.volume[1];
					this.volume[2] = playerFileData.volume[2];
					this.resolutionX = playerFileData.resolutionX;
					this.resolutionY = playerFileData.resolutionY;
					if (this.resolutionX == 0)
					{
						this.resolutionX = Mathf.Max(Display.main.systemWidth, 480);
						this.resolutionY = Mathf.Max(Display.main.systemHeight, 360);
					}
					this.rumble = playerFileData.rumble;
					this.analogMovement = playerFileData.analogMovement;
					this.antiAnnoyance = playerFileData.antiAnnoyance;
					this.subtitles = playerFileData.subtitles;
					this.crtFilter = playerFileData.crtFilter;
					this.fullscreen = playerFileData.fullscreen;
					this.foundChars = playerFileData.foundChars;
					this.foundEvnts = playerFileData.foundEvnts;
					this.foundItems = playerFileData.foundItems;
					this.foundObstcls = playerFileData.foundObstcls;
					this.foundTrips = playerFileData.foundTrips;
					this.wonChallenges = playerFileData.wonChallenges;
					this.clearedLevels = playerFileData.clearedLevels;
					this.savedGameData = new SavedGameData();
					this.fileName = fileName;
					this.Save(0f);
				}
			}
		}
	}

	// Token: 0x06000725 RID: 1829 RVA: 0x000244A4 File Offset: 0x000226A4
	public void ResetSaveData()
	{
		for (int i = 0; i < this.foundChars.Length; i++)
		{
			this.foundChars[i] = false;
		}
		for (int j = 0; j < this.foundEvnts.Length; j++)
		{
			this.foundEvnts[j] = false;
		}
		for (int k = 0; k < this.foundItems.Length; k++)
		{
			this.foundItems[k] = false;
		}
		for (int l = 0; l < this.foundObstcls.Length; l++)
		{
			this.foundObstcls[l] = false;
		}
		for (int m = 0; m < this.foundTrips.Length; m++)
		{
			this.foundTrips[m] = false;
		}
		for (int n = 0; n < this.wonChallenges.Length; n++)
		{
			this.wonChallenges[n] = false;
		}
		for (int num = 0; num < this.clearedLevels.Length; num++)
		{
			this.clearedLevels[num] = false;
		}
		for (int num2 = 0; num2 < this.foundChars.Length; num2++)
		{
			this.foundChars[num2] = false;
		}
		this.reduceFlashing = false;
		this.authenticMode = false;
		Singleton<AchievementManager>.Instance.Reset();
		this.savedGameData = new SavedGameData();
	}

	// Token: 0x06000726 RID: 1830 RVA: 0x000245C8 File Offset: 0x000227C8
	public void SetDefaults()
	{
		this.subLanguage = Language.English;
		this.textureLanguage = Language.English;
		this.speechLanguage = Language.English;
		this.mouseCameraSensitivity = 0.29f;
		this.mouseCursorSensitivity = 1f;
		this.controllerCameraSensitivity = 400f;
		this.controllerCursorSensitivity = 400f;
		this.volume[0] = 1f;
		this.volume[1] = 1f;
		this.volume[2] = 0.8f;
		this.UpdateVolume();
		this.rumble = true;
		this.analogMovement = true;
		this.antiAnnoyance = false;
		this.subtitles = false;
		this.crtFilter = false;
		this.pixelFilter = true;
		this.resolutionX = Mathf.Max(Display.main.systemWidth, 480);
		this.resolutionY = Mathf.Max(Display.main.systemHeight, 360);
		this.fullscreen = true;
		this.vsync = true;
		this.reduceFlashing = false;
		this.authenticMode = false;
	}

	// Token: 0x06000727 RID: 1831 RVA: 0x000246BD File Offset: 0x000228BD
	public void Save()
	{
		this.Save(3f);
	}

	// Token: 0x06000728 RID: 1832 RVA: 0x000246CC File Offset: 0x000228CC
	public void Save(float version)
	{
		string path = Application.persistentDataPath + "/PlayerFile_" + this.fileName + ".sav";
		if (File.Exists(path))
		{
			File.Delete(path);
		}
		File.WriteAllText(path, RijndaelEncryption.Encrypt(JsonUtility.ToJson(new PlayerFileData
		{
			saveVersion = version,
			foundChars = this.foundChars,
			foundEvnts = this.foundEvnts,
			foundItems = this.foundItems,
			foundObstcls = this.foundObstcls,
			foundTrips = this.foundTrips,
			clearedLevels = this.clearedLevels,
			reduceFlashing = this.reduceFlashing,
			achievementData = Singleton<AchievementManager>.Instance.Save(),
			savedGameData = this.savedGameData
		}), this.fileName));
		PlayerPrefs.SetString("lastFile", this.fileName);
		Debug.Log("File progress for " + this.fileName + " saved");
		path = Application.persistentDataPath + "/PlayerConfig_" + this.fileName + ".cfg";
		if (File.Exists(path))
		{
			File.Delete(path);
		}
		FileConfigData fileConfigData = new FileConfigData();
		fileConfigData.language = this.currentLanguage;
		fileConfigData.mouseCameraSensitivity = this.mouseCameraSensitivity;
		fileConfigData.mouseCursorSensitivity = this.mouseCursorSensitivity;
		fileConfigData.controllerCameraSensitivity = this.controllerCameraSensitivity;
		fileConfigData.controllerCursorSensitivity = this.controllerCursorSensitivity;
		fileConfigData.volume[0] = this.volume[0];
		fileConfigData.volume[1] = this.volume[1];
		fileConfigData.volume[2] = this.volume[2];
		fileConfigData.resolutionX = this.resolutionX;
		fileConfigData.resolutionY = this.resolutionY;
		fileConfigData.rumble = this.rumble;
		fileConfigData.analogMovement = this.analogMovement;
		fileConfigData.antiAnnoyance = this.antiAnnoyance;
		fileConfigData.subtitles = this.subtitles;
		fileConfigData.crtFilter = this.crtFilter;
		fileConfigData.fullscreen = this.fullscreen;
		fileConfigData.vsync = this.vsync;
		fileConfigData.pixelFilter = this.pixelFilter;
		fileConfigData.controllers = this.controllers;
		File.WriteAllText(path, JsonUtility.ToJson(fileConfigData));
		Debug.Log("Config for " + this.fileName + " saved");
	}

	// Token: 0x06000729 RID: 1833 RVA: 0x00024908 File Offset: 0x00022B08
	public void Delete(string deleteName)
	{
		if (File.Exists(Application.persistentDataPath + "/PlayerFile_" + deleteName + ".sav"))
		{
			File.Delete(Application.persistentDataPath + "/PlayerFile_" + deleteName + ".sav");
			File.Delete(Application.persistentDataPath + "/PlayerConfig_" + this.fileName + ".cfg");
			Debug.Log("File for " + deleteName + " successfully deleted");
			return;
		}
		Debug.Log("Delete attempt failed. File for " + deleteName + " does not exist.");
	}

	// Token: 0x0600072A RID: 1834 RVA: 0x00024998 File Offset: 0x00022B98
	public void UpdateLocalization()
	{
		if (this.subLanguage == Language.English)
		{
			Singleton<LocalizationManager>.Instance.LoadLocalizedText("Subtitles_En.json", Language.English);
		}
		else if (this.subLanguage == Language.French)
		{
			Singleton<LocalizationManager>.Instance.LoadLocalizedText("Subtitles_Fr.json", Language.French);
		}
		else
		{
			Singleton<LocalizationManager>.Instance.LoadLocalizedText("Subtitles_En.json", Language.English);
		}
		Singleton<LocalizationManager>.Instance.UpdateCurrentLanguage(this.speechLanguage);
		Singleton<LocalizationManager>.Instance.UpdateCurrentTextureLanguage(this.textureLanguage);
	}

	// Token: 0x0600072B RID: 1835 RVA: 0x00024A0A File Offset: 0x00022C0A
	public void Find(bool[] type, int value)
	{
		if (!type[value])
		{
			type[value] = true;
			this.Save();
		}
	}

	// Token: 0x0600072C RID: 1836 RVA: 0x00024A1C File Offset: 0x00022C1C
	public void UpdateResolution()
	{
		Screen.SetResolution(this.resolutionX, this.resolutionY, this.fullscreen, 0);
		if (Singleton<GlobalCam>.Instance != null)
		{
			Singleton<GlobalCam>.Instance.UpdateResolution(this.resolutionX, this.resolutionY);
		}
		this.UpdateTpp();
	}

	// Token: 0x0600072D RID: 1837 RVA: 0x00024A6C File Offset: 0x00022C6C
	public void UpdateTpp()
	{
		if (!this.pixelFilter)
		{
			Shader.SetGlobalFloat("_texelsPerPixel", 0f);
			return;
		}
		if ((float)this.resolutionX / (float)this.resolutionY >= 1.3333f)
		{
			Shader.SetGlobalFloat("_texelsPerPixel", 360f / (float)this.resolutionY);
			return;
		}
		Shader.SetGlobalFloat("_texelsPerPixel", 480f / (float)this.resolutionX);
	}

	// Token: 0x0600072E RID: 1838 RVA: 0x00024AD8 File Offset: 0x00022CD8
	public void UpdateVolume()
	{
		if (this.volume[0] != 0f)
		{
			this.mixer[0].audioMixer.SetFloat("voiceVol", Mathf.Log10(Mathf.Pow(this.volume[0], 2f)) * 20f);
		}
		else
		{
			this.mixer[0].audioMixer.SetFloat("voiceVol", -80f);
		}
		if (this.volume[1] != 0f)
		{
			this.mixer[1].audioMixer.SetFloat("effectVol", Mathf.Log10(Mathf.Pow(this.volume[1], 2f)) * 20f);
		}
		else
		{
			this.mixer[1].audioMixer.SetFloat("effectVol", -80f);
		}
		if (this.volume[2] != 0f)
		{
			this.mixer[2].audioMixer.SetFloat("musicVol", Mathf.Log10(Mathf.Pow(this.volume[2], 2f)) * 20f);
			return;
		}
		this.mixer[2].audioMixer.SetFloat("musicVol", -80f);
	}

	// Token: 0x0600072F RID: 1839 RVA: 0x00024C0D File Offset: 0x00022E0D
	public void SetDefaultControls()
	{
		Singleton<InputManager>.Instance.ResetControlMaps();
	}

	// Token: 0x06000730 RID: 1840 RVA: 0x00024C1C File Offset: 0x00022E1C
	public void GetInputStringsForAction(InputAction action, string controllerId, out List<string> output)
	{
		output = new List<string>();
		for (int i = 0; i < this.controllers.Count; i++)
		{
			if (this.controllers[i].id == controllerId)
			{
				for (int j = 0; j < this.controllers[i].inputs.Count; j++)
				{
					if (output.Count >= 3)
					{
						return;
					}
					if (this.controllers[i].inputs[j].action == action)
					{
						output.Add(this.controllers[i].inputs[j].name);
					}
				}
				break;
			}
		}
	}

	// Token: 0x04000762 RID: 1890
	public const float saveVersion = 3f;

	// Token: 0x04000763 RID: 1891
	public bool debugMode;

	// Token: 0x04000764 RID: 1892
	public bool loadOnAwake;

	// Token: 0x04000765 RID: 1893
	public bool crtFilter;

	// Token: 0x04000766 RID: 1894
	public string fileName;

	// Token: 0x04000767 RID: 1895
	public Language currentLanguage;

	// Token: 0x04000768 RID: 1896
	public Language subLanguage;

	// Token: 0x04000769 RID: 1897
	public Language textureLanguage;

	// Token: 0x0400076A RID: 1898
	public Language speechLanguage;

	// Token: 0x0400076B RID: 1899
	public float mouseCameraSensitivity;

	// Token: 0x0400076C RID: 1900
	public float mouseCursorSensitivity;

	// Token: 0x0400076D RID: 1901
	public float controllerCameraSensitivity;

	// Token: 0x0400076E RID: 1902
	public float controllerCursorSensitivity;

	// Token: 0x0400076F RID: 1903
	public float[] volume = new float[3];

	// Token: 0x04000770 RID: 1904
	public AudioMixerGroup[] mixer = new AudioMixerGroup[3];

	// Token: 0x04000771 RID: 1905
	public int resolutionX;

	// Token: 0x04000772 RID: 1906
	public int resolutionY;

	// Token: 0x04000773 RID: 1907
	public bool rumble;

	// Token: 0x04000774 RID: 1908
	public bool analogMovement;

	// Token: 0x04000775 RID: 1909
	public bool antiAnnoyance;

	// Token: 0x04000776 RID: 1910
	public bool subtitles;

	// Token: 0x04000777 RID: 1911
	public bool fullscreen;

	// Token: 0x04000778 RID: 1912
	public bool vsync;

	// Token: 0x04000779 RID: 1913
	public bool pixelFilter;

	// Token: 0x0400077A RID: 1914
	public bool reduceFlashing;

	// Token: 0x0400077B RID: 1915
	public bool authenticMode;

	// Token: 0x0400077C RID: 1916
	public List<ControllerData> controllers;

	// Token: 0x0400077D RID: 1917
	public bool[] foundChars = new bool[256];

	// Token: 0x0400077E RID: 1918
	public bool[] foundEvnts = new bool[256];

	// Token: 0x0400077F RID: 1919
	public bool[] foundItems = new bool[256];

	// Token: 0x04000780 RID: 1920
	public bool[] foundObstcls = new bool[256];

	// Token: 0x04000781 RID: 1921
	public bool[] foundTrips = new bool[256];

	// Token: 0x04000782 RID: 1922
	public bool[] wonChallenges = new bool[256];

	// Token: 0x04000783 RID: 1923
	public bool[] clearedLevels = new bool[32];

	// Token: 0x04000784 RID: 1924
	public SavedGameData savedGameData = new SavedGameData();

	// Token: 0x04000785 RID: 1925
	public List<ItemObject> itemObjects;
}
