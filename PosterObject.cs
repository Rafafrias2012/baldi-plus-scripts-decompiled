using System;
using UnityEngine;

// Token: 0x020001F6 RID: 502
[CreateAssetMenu(fileName = "PosterSet", menuName = "Custom Assets/Poster Set", order = 5)]
public class PosterObject : ScriptableObject
{
	// Token: 0x06000B48 RID: 2888 RVA: 0x0003BB30 File Offset: 0x00039D30
	public Material GetMaterial(int val)
	{
		if (!this.useLocalVersion)
		{
			return this.material[val];
		}
		Material[] localizedPoster = this.material;
		if (Singleton<LocalizationManager>.Instance != null)
		{
			localizedPoster = Singleton<LocalizationManager>.Instance.GetLocalizedPoster(this);
		}
		if (localizedPoster != null && val < localizedPoster.Length)
		{
			return localizedPoster[val];
		}
		Debug.LogWarning(string.Concat(new string[]
		{
			"No localized version of ",
			this.material[0].ToString(),
			" poster was found for language ",
			Singleton<PlayerFileManager>.Instance.textureLanguage.ToString(),
			". Returning default material"
		}));
		return this.material[val];
	}

	// Token: 0x04000D71 RID: 3441
	public bool useLocalVersion;

	// Token: 0x04000D72 RID: 3442
	public Material[] material = new Material[0];

	// Token: 0x04000D73 RID: 3443
	public Material[] testMaterial = new Material[0];

	// Token: 0x04000D74 RID: 3444
	public Material[] frenchMaterial = new Material[0];

	// Token: 0x04000D75 RID: 3445
	public Material[] japaneseMaterial = new Material[0];

	// Token: 0x04000D76 RID: 3446
	public Texture2D baseTexture;

	// Token: 0x04000D77 RID: 3447
	public PosterTextData[] textData = new PosterTextData[0];

	// Token: 0x04000D78 RID: 3448
	public PosterObject[] multiPosterArray = new PosterObject[0];

	// Token: 0x04000D79 RID: 3449
	public bool loop;
}
