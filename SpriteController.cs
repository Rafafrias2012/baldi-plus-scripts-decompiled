using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000216 RID: 534
public class SpriteController : MonoBehaviour
{
	// Token: 0x170000F8 RID: 248
	// (get) Token: 0x06000BE9 RID: 3049 RVA: 0x0003EDCC File Offset: 0x0003CFCC
	// (set) Token: 0x06000BEA RID: 3050 RVA: 0x0003EDD4 File Offset: 0x0003CFD4
	public RectTransform RectTransform { get; private set; }

	// Token: 0x170000F9 RID: 249
	// (get) Token: 0x06000BEB RID: 3051 RVA: 0x0003EDDD File Offset: 0x0003CFDD
	// (set) Token: 0x06000BEC RID: 3052 RVA: 0x0003EDE5 File Offset: 0x0003CFE5
	public Image Image { get; private set; }

	// Token: 0x170000FA RID: 250
	// (get) Token: 0x06000BED RID: 3053 RVA: 0x0003EDEE File Offset: 0x0003CFEE
	// (set) Token: 0x06000BEE RID: 3054 RVA: 0x0003EDF6 File Offset: 0x0003CFF6
	public Vector2 Position { get; private set; }

	// Token: 0x170000FB RID: 251
	// (get) Token: 0x06000BEF RID: 3055 RVA: 0x0003EDFF File Offset: 0x0003CFFF
	// (set) Token: 0x06000BF0 RID: 3056 RVA: 0x0003EE07 File Offset: 0x0003D007
	public Vector2 Scale { get; private set; }

	// Token: 0x06000BF1 RID: 3057 RVA: 0x0003EE10 File Offset: 0x0003D010
	private void Awake()
	{
		this.Image = base.GetComponent<Image>();
		this.RectTransform = base.GetComponent<RectTransform>();
	}

	// Token: 0x06000BF2 RID: 3058 RVA: 0x0003EE2C File Offset: 0x0003D02C
	public void SetPosition(Vector2 position)
	{
		this.Position = position;
		Vector2 anchoredPosition = new Vector2((float)Mathf.RoundToInt(position.x), (float)Mathf.RoundToInt(position.y));
		this.RectTransform.anchoredPosition = anchoredPosition;
	}

	// Token: 0x06000BF3 RID: 3059 RVA: 0x0003EE6B File Offset: 0x0003D06B
	public void SetScale(Vector2 scale)
	{
		this.Scale = scale;
		this.RectTransform.localScale = new Vector3(scale.x, scale.y, 1f);
	}

	// Token: 0x06000BF4 RID: 3060 RVA: 0x0003EE95 File Offset: 0x0003D095
	public void SetSprite(Sprite sprite)
	{
		this.Image.sprite = sprite;
	}

	// Token: 0x06000BF5 RID: 3061 RVA: 0x0003EEA3 File Offset: 0x0003D0A3
	public void SetColor(Color color)
	{
		this.Image.color = color;
	}

	// Token: 0x170000FC RID: 252
	// (get) Token: 0x06000BF6 RID: 3062 RVA: 0x0003EEB1 File Offset: 0x0003D0B1
	public Sprite Sprite
	{
		get
		{
			return this.Image.sprite;
		}
	}
}
