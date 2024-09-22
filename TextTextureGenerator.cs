using System;
using TMPro;
using UnityEngine;

// Token: 0x020001B6 RID: 438
public class TextTextureGenerator : MonoBehaviour
{
	// Token: 0x060009DF RID: 2527 RVA: 0x00034FD4 File Offset: 0x000331D4
	public Texture2D GenerateTextTexture(PosterObject poster)
	{
		if (this.renderTexture == null)
		{
			this.renderTexture = new RenderTexture(256, 256, 32, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
			this.renderCamera.targetTexture = this.renderTexture;
		}
		Texture2D texture2D = new Texture2D(256, 256, TextureFormat.ARGB32, false);
		Texture2D texture2D2 = new Texture2D(256, 256, TextureFormat.ARGB32, false);
		texture2D.filterMode = FilterMode.Point;
		texture2D2.filterMode = FilterMode.Point;
		Color[] pixels = poster.baseTexture.GetPixels();
		Color[] array = new Color[0];
		this.LoadPosterData(poster);
		this.renderCamera.Render();
		RenderTexture.active = this.renderTexture;
		texture2D2.ReadPixels(this._readRect, 0, 0);
		texture2D2.Apply();
		array = texture2D2.GetPixels();
		for (int i = 0; i < pixels.Length; i++)
		{
			pixels[i].r = Mathf.Lerp(pixels[i].r, array[i].r, array[i].a);
			pixels[i].g = Mathf.Lerp(pixels[i].g, array[i].g, array[i].a);
			pixels[i].b = Mathf.Lerp(pixels[i].b, array[i].b, array[i].a);
		}
		texture2D.SetPixels(pixels);
		texture2D.Apply();
		return texture2D;
	}

	// Token: 0x060009E0 RID: 2528 RVA: 0x00035158 File Offset: 0x00033358
	public void LoadPosterData(PosterObject poster)
	{
		for (int i = 0; i < this.textureTMPPre.Length; i++)
		{
			this.textureTMPPre[i].gameObject.SetActive(false);
		}
		for (int j = 0; j < poster.textData.Length; j++)
		{
			this._textMesh = this.textureTMPPre[j];
			this._data = poster.textData[j];
			this._pos.x = (float)this._data.position.x;
			this._pos.y = (float)this._data.position.z;
			this._size.x = (float)this._data.size.x;
			this._size.y = (float)this._data.size.z;
			this._textMesh.gameObject.SetActive(true);
			this._textMesh.rectTransform.sizeDelta = this._size;
			this._textMesh.rectTransform.anchoredPosition = this._pos;
			this._textMesh.text = Singleton<LocalizationManager>.Instance.GetLocalizedText(this._data.textKey);
			this._textMesh.font = this._data.font;
			this._textMesh.fontSize = (float)this._data.fontSize;
			this._textMesh.color = this._data.color;
			this._textMesh.fontStyle = this._data.style;
			this._textMesh.alignment = this._data.alignment;
		}
	}

	// Token: 0x04000B2C RID: 2860
	private RenderTexture renderTexture;

	// Token: 0x04000B2D RID: 2861
	[SerializeField]
	private Camera renderCamera;

	// Token: 0x04000B2E RID: 2862
	public TMP_Text[] textureTMPPre = new TMP_Text[4];

	// Token: 0x04000B2F RID: 2863
	private TMP_Text _textMesh;

	// Token: 0x04000B30 RID: 2864
	private PosterTextData _data;

	// Token: 0x04000B31 RID: 2865
	private Rect _readRect = new Rect(0f, 0f, 256f, 256f);

	// Token: 0x04000B32 RID: 2866
	private Vector3 _pos;

	// Token: 0x04000B33 RID: 2867
	private Vector2 _size;
}
