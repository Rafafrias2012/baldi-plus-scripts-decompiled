using System;
using System.Collections.Generic;
using System.IO;
using UnityCipher;
using UnityEngine;

// Token: 0x020001CF RID: 463
public class LocalizationManager : Singleton<LocalizationManager>
{
	// Token: 0x06000A6E RID: 2670 RVA: 0x0003746C File Offset: 0x0003566C
	private void Start()
	{
		this.LoadLocalizedText("Subtitles_En.json", Language.English);
	}

	// Token: 0x06000A6F RID: 2671 RVA: 0x0003747A File Offset: 0x0003567A
	public void UpdateCurrentLanguage(Language language)
	{
		this.currentAudLang = language;
	}

	// Token: 0x06000A70 RID: 2672 RVA: 0x00037483 File Offset: 0x00035683
	public void UpdateCurrentTextureLanguage(Language language)
	{
		this.currentTextureLang = language;
	}

	// Token: 0x06000A71 RID: 2673 RVA: 0x0003748C File Offset: 0x0003568C
	public void LoadLocalizedText(string fileName, Language language)
	{
		this.currentSubLang = language;
		this.localizedText = new Dictionary<string, string>();
		string path = Path.Combine(Application.streamingAssetsPath, fileName);
		if (File.Exists(path))
		{
			LocalizationData localizationData = JsonUtility.FromJson<LocalizationData>(File.ReadAllText(path));
			for (int i = 0; i < localizationData.items.Length; i++)
			{
				this.localizedText.Add(localizationData.items[i].key, localizationData.items[i].value);
			}
			return;
		}
		Debug.LogError("Cannot find file!");
	}

	// Token: 0x06000A72 RID: 2674 RVA: 0x0003750E File Offset: 0x0003570E
	public string GetLocalizedText(string key)
	{
		return this.GetLocalizedText(key, false);
	}

	// Token: 0x06000A73 RID: 2675 RVA: 0x00037518 File Offset: 0x00035718
	public string GetLocalizedText(string key, bool encrypted)
	{
		string result = key;
		if (this.localizedText.ContainsKey(key))
		{
			if (!encrypted)
			{
				result = this.localizedText[key];
			}
			else
			{
				result = RijndaelEncryption.Decrypt(this.localizedText[key], "baldi");
			}
		}
		return result;
	}

	// Token: 0x06000A74 RID: 2676 RVA: 0x00037560 File Offset: 0x00035760
	public AudioClip GetLocalizedAudioClip(SoundObject sound)
	{
		AudioClip audioClip;
		switch (this.currentAudLang)
		{
		case Language.English:
			audioClip = sound.soundClip;
			break;
		case Language.Test:
			audioClip = sound.testLanguageClip;
			break;
		case Language.French:
			audioClip = sound.frenchClip;
			break;
		case Language.Japanese:
			audioClip = sound.japaneseClip;
			break;
		default:
			audioClip = sound.soundClip;
			break;
		}
		if (audioClip == null)
		{
			audioClip = sound.soundClip;
		}
		return audioClip;
	}

	// Token: 0x06000A75 RID: 2677 RVA: 0x000375CC File Offset: 0x000357CC
	public Material[] GetLocalizedPoster(PosterObject poster)
	{
		Material[] result;
		switch (this.currentTextureLang)
		{
		case Language.English:
			result = poster.material;
			break;
		case Language.Test:
			result = poster.testMaterial;
			break;
		case Language.French:
			result = poster.frenchMaterial;
			break;
		case Language.Japanese:
			result = poster.japaneseMaterial;
			break;
		default:
			result = poster.material;
			break;
		}
		return result;
	}

	// Token: 0x04000BF0 RID: 3056
	private Dictionary<string, string> localizedText;

	// Token: 0x04000BF1 RID: 3057
	private Language currentSubLang;

	// Token: 0x04000BF2 RID: 3058
	private Language currentAudLang;

	// Token: 0x04000BF3 RID: 3059
	private Language currentTextureLang;
}
