using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

// Token: 0x02000108 RID: 264
public class GlobalCam : Singleton<GlobalCam>
{
	// Token: 0x0600067F RID: 1663 RVA: 0x000209DB File Offset: 0x0001EBDB
	public void SetListener(bool val)
	{
		this.listener.enabled = val;
	}

	// Token: 0x17000080 RID: 128
	// (get) Token: 0x06000680 RID: 1664 RVA: 0x000209E9 File Offset: 0x0001EBE9
	public Camera Cam
	{
		get
		{
			return this.cam;
		}
	}

	// Token: 0x17000081 RID: 129
	// (get) Token: 0x06000681 RID: 1665 RVA: 0x000209F1 File Offset: 0x0001EBF1
	public Camera RenderTexCam
	{
		get
		{
			return this.renderTexCam;
		}
	}

	// Token: 0x06000682 RID: 1666 RVA: 0x000209F9 File Offset: 0x0001EBF9
	public void ChangeType(CameraRenderType type)
	{
		this.cameraData.renderType = type;
		this.listener.enabled = (type == CameraRenderType.Base);
	}

	// Token: 0x06000683 RID: 1667 RVA: 0x00020A18 File Offset: 0x0001EC18
	public void UpdateResolution(int resX, int resY)
	{
		if (this.currentRenderTexture == null)
		{
			this.currentRenderTexture = this.globalCamTexture;
		}
		this.currentRenderTexture = new RenderTexture(this.currentRenderTexture);
		if ((float)resX / (float)resY >= 1.3333f)
		{
			resX = Mathf.RoundToInt((float)resX / (float)resY * 360f);
			resY = 360;
			this.currentRenderTexture.width = resX;
			this.currentRenderTexture.height = resY;
		}
		else
		{
			resY = Mathf.RoundToInt((float)resY / (float)resX * 480f);
			resX = 480;
			this.currentRenderTexture.width = resX;
			this.currentRenderTexture.height = resY;
		}
		this.cam.targetTexture = this.currentRenderTexture;
		this.globalCamImage.texture = this.currentRenderTexture;
		this.transTexture = new Texture2D(this.currentRenderTexture.width, this.currentRenderTexture.height, TextureFormat.ARGB32, false);
		this.toCopyTexture = new Texture2D(this.currentRenderTexture.width, this.currentRenderTexture.height, TextureFormat.ARGB32, false);
		this.transBase = new Texture2D(this.currentRenderTexture.width, this.currentRenderTexture.height, TextureFormat.ARGB32, false);
		this.readPixelsRect = new Rect(0f, 0f, (float)this.currentRenderTexture.width, (float)this.currentRenderTexture.height);
		for (int i = 0; i < this.transBase.width; i++)
		{
			for (int j = 0; j < this.transBase.height; j++)
			{
				this.transBase.SetPixel(i, j, Color.clear);
			}
		}
		this.transBase.Apply();
		this._size.x = (float)resX;
		this._size.y = (float)resY;
		this.globalCamImage.rectTransform.sizeDelta = this._size;
		this.transImage.rectTransform.sizeDelta = this._size;
		if (this.transparentArray.Length != this.currentRenderTexture.width * this.currentRenderTexture.height)
		{
			this.transparentArray = new Color[this.currentRenderTexture.height * this.currentRenderTexture.width];
			for (int k = 0; k < this.transparentArray.Length; k++)
			{
				this.transparentArray[k] = Color.clear;
			}
		}
	}

	// Token: 0x06000684 RID: 1668 RVA: 0x00020C70 File Offset: 0x0001EE70
	public void Transition(UiTransition type, float duration)
	{
		if (CursorController.Instance != null)
		{
			CursorController.Instance.Hide(true);
		}
		this.cam.Render();
		RenderTexture.active = this.currentRenderTexture;
		this.transTexture.ReadPixels(this.readPixelsRect, 0, 0);
		this.transTexture.Apply();
		this.transImage.texture = this.transTexture;
		this.transImage.gameObject.SetActive(true);
		this.transImage.color = Color.white;
		if (this.transitionActive)
		{
			base.StopCoroutine(this.transitioner);
		}
		switch (type)
		{
		case UiTransition.Dither:
			this.transitioner = this.Dither(duration, this.toCopyTexture, true);
			break;
		case UiTransition.SwipeLeft:
			this.transitioner = this.SwipeLeft(duration);
			break;
		case UiTransition.SwipeRight:
			this.transitioner = this.SwipeRight(duration);
			break;
		}
		this.transitionActive = true;
		base.StartCoroutine(this.transitioner);
	}

	// Token: 0x06000685 RID: 1669 RVA: 0x00020D6C File Offset: 0x0001EF6C
	public void FadeIn(UiTransition type, float duration)
	{
		if (CursorController.Instance != null)
		{
			CursorController.Instance.Hide(true);
		}
		this.cam.Render();
		RenderTexture.active = this.currentRenderTexture;
		this.toCopyTexture.ReadPixels(this.readPixelsRect, 0, 0);
		this.toCopyTexture.Apply();
		Graphics.CopyTexture(this.transBase, this.transTexture);
		this.transTexture.Apply();
		this.transImage.texture = this.transTexture;
		this.transImage.gameObject.SetActive(true);
		this.globalCamImage.gameObject.SetActive(false);
		this.transImage.color = Color.white;
		if (this.transitionActive)
		{
			base.StopCoroutine(this.transitioner);
		}
		switch (type)
		{
		case UiTransition.Dither:
			this.transitioner = this.Dither(duration, this.toCopyTexture, false);
			break;
		case UiTransition.SwipeLeft:
			this.transitioner = this.SwipeLeft(duration);
			break;
		case UiTransition.SwipeRight:
			this.transitioner = this.SwipeRight(duration);
			break;
		}
		this.transitionActive = true;
		base.StartCoroutine(this.transitioner);
	}

	// Token: 0x06000686 RID: 1670 RVA: 0x00020E94 File Offset: 0x0001F094
	private void EndTransition()
	{
		this.transitionActive = false;
		this.transImage.gameObject.SetActive(false);
		this.globalCamImage.gameObject.SetActive(true);
		this.cam.enabled = true;
		if (CursorController.Instance != null)
		{
			CursorController.Instance.Hide(false);
		}
	}

	// Token: 0x06000687 RID: 1671 RVA: 0x00020EEE File Offset: 0x0001F0EE
	private IEnumerator SwipeLeft(float totalTime)
	{
		float time = totalTime;
		int clearedColumns = 0;
		yield return null;
		this.cam.enabled = false;
		while (time > 0f)
		{
			for (int i = this.transTexture.width - 1 - Mathf.RoundToInt((float)this.transTexture.width * (1f - time / totalTime)); i < this.transTexture.width - clearedColumns; i++)
			{
				for (int j = 0; j < this.transTexture.height; j++)
				{
					this.transTexture.SetPixel(i, j, Color.clear);
				}
			}
			this.transTexture.Apply();
			clearedColumns = Mathf.RoundToInt((float)this.transTexture.width * (1f - time / totalTime));
			time -= Time.unscaledDeltaTime;
			yield return null;
		}
		this.EndTransition();
		yield break;
	}

	// Token: 0x06000688 RID: 1672 RVA: 0x00020F04 File Offset: 0x0001F104
	private IEnumerator SwipeRight(float totalTime)
	{
		float time = totalTime;
		int clearedColumns = 0;
		yield return null;
		this.cam.enabled = false;
		while (time > 0f)
		{
			for (int i = clearedColumns; i < Mathf.RoundToInt((float)this.transTexture.width * (1f - time / totalTime)); i++)
			{
				for (int j = 0; j < this.transTexture.height; j++)
				{
					this.transTexture.SetPixel(i, j, Color.clear);
				}
			}
			this.transTexture.Apply();
			clearedColumns = Mathf.RoundToInt((float)this.transTexture.width * (1f - time / totalTime));
			time -= Time.unscaledDeltaTime;
			yield return null;
		}
		this.EndTransition();
		yield break;
	}

	// Token: 0x06000689 RID: 1673 RVA: 0x00020F1A File Offset: 0x0001F11A
	private IEnumerator Dither(float timeBetweenAdvance, Texture2D sourceTexture, bool updateToCopy)
	{
		float timeToNext = timeBetweenAdvance;
		int totalFrames = 0;
		yield return null;
		if (updateToCopy)
		{
			RenderTexture.active = this.currentRenderTexture;
			this.toCopyTexture.ReadPixels(this.readPixelsRect, 0, 0);
			this.toCopyTexture.Apply();
		}
		this.cam.enabled = false;
		while (totalFrames < 16)
		{
			if (timeToNext <= 0f)
			{
				for (int i = this.ditherOffset[totalFrames].x; i < this.transTexture.width; i += 4)
				{
					for (int j = this.ditherOffset[totalFrames].z; j < this.transTexture.height; j += 4)
					{
						this.transTexture.SetPixel(i, j, sourceTexture.GetPixel(i, j));
					}
				}
				this.transTexture.Apply();
				totalFrames++;
				timeToNext = timeBetweenAdvance;
			}
			else
			{
				timeToNext -= Time.unscaledDeltaTime;
			}
			yield return null;
		}
		this.EndTransition();
		yield break;
	}

	// Token: 0x17000082 RID: 130
	// (get) Token: 0x0600068A RID: 1674 RVA: 0x00020F3E File Offset: 0x0001F13E
	public bool TransitionActive
	{
		get
		{
			return this.transitionActive;
		}
	}

	// Token: 0x040006A3 RID: 1699
	[SerializeField]
	private Camera cam;

	// Token: 0x040006A4 RID: 1700
	[SerializeField]
	private Camera renderTexCam;

	// Token: 0x040006A5 RID: 1701
	[SerializeField]
	private RenderTexture globalCamTexture;

	// Token: 0x040006A6 RID: 1702
	private RenderTexture currentRenderTexture;

	// Token: 0x040006A7 RID: 1703
	[SerializeField]
	private RawImage globalCamImage;

	// Token: 0x040006A8 RID: 1704
	[SerializeField]
	private RawImage transImage;

	// Token: 0x040006A9 RID: 1705
	[SerializeField]
	private UniversalAdditionalCameraData cameraData;

	// Token: 0x040006AA RID: 1706
	[SerializeField]
	private AudioListener listener;

	// Token: 0x040006AB RID: 1707
	private IEnumerator transitioner;

	// Token: 0x040006AC RID: 1708
	private Texture2D transTexture;

	// Token: 0x040006AD RID: 1709
	private Texture2D toCopyTexture;

	// Token: 0x040006AE RID: 1710
	private Texture2D transBase;

	// Token: 0x040006AF RID: 1711
	private Color[] transparentArray = new Color[0];

	// Token: 0x040006B0 RID: 1712
	private Rect readPixelsRect;

	// Token: 0x040006B1 RID: 1713
	private Vector2 _size;

	// Token: 0x040006B2 RID: 1714
	private IntVector2[] ditherOffset = new IntVector2[]
	{
		new IntVector2(1, 1),
		new IntVector2(3, 3),
		new IntVector2(1, 3),
		new IntVector2(3, 1),
		new IntVector2(2, 0),
		new IntVector2(0, 2),
		new IntVector2(2, 2),
		new IntVector2(0, 0),
		new IntVector2(1, 2),
		new IntVector2(0, 3),
		new IntVector2(3, 2),
		new IntVector2(1, 0),
		new IntVector2(2, 3),
		new IntVector2(0, 1),
		new IntVector2(2, 1),
		new IntVector2(0, 3)
	};

	// Token: 0x040006B3 RID: 1715
	private bool transitionActive;
}
