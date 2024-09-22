using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020001DE RID: 478
public class StandardMenuButton : MenuButton
{
	// Token: 0x06000AD7 RID: 2775 RVA: 0x0003940C File Offset: 0x0003760C
	private void Update()
	{
		if (!this.highlighted && this.wasHighlighted)
		{
			this.wasHighlighted = false;
			this.UnHighilight();
		}
		this.highlighted = false;
	}

	// Token: 0x06000AD8 RID: 2776 RVA: 0x00039434 File Offset: 0x00037634
	public override void Press()
	{
		if (this.transitionOnPress)
		{
			Singleton<GlobalCam>.Instance.Transition(this.transitionType, this.transitionTime);
		}
		this.held = true;
		if (this.swapOnHold)
		{
			this.image.sprite = this.heldSprite;
		}
		base.Press();
	}

	// Token: 0x06000AD9 RID: 2777 RVA: 0x00039488 File Offset: 0x00037688
	private void UnHighilight()
	{
		if (this.swapOnHigh && (!this.held || !this.swapOnHold))
		{
			this.image.sprite = this.unhighlightedSprite;
		}
		if (this.eventOnHigh)
		{
			this.OffHighlight.Invoke();
		}
		if (this.underlineOnHigh)
		{
			this.text.fontStyle = FontStyles.Normal;
		}
		if (this.animateOnHigh)
		{
			if (this.unHighlightAnimation == "")
			{
				this.animator.Play(this.highlightAnimation, -1, 0f);
				this.animator.speed = 0f;
				return;
			}
			this.animator.Play(this.unHighlightAnimation, -1, 0f);
			this.animator.speed = 1f;
		}
	}

	// Token: 0x06000ADA RID: 2778 RVA: 0x00039550 File Offset: 0x00037750
	public override void UnHold()
	{
		this.held = false;
		this.OnRelease.Invoke();
		if (this.swapOnHold)
		{
			if (this.highlighted && this.swapOnHigh)
			{
				this.image.sprite = this.highlightedSprite;
				return;
			}
			this.image.sprite = this.unhighlightedSprite;
		}
	}

	// Token: 0x06000ADB RID: 2779 RVA: 0x000395AA File Offset: 0x000377AA
	private void OnEnable()
	{
		if (this.unhighlightOnEnable)
		{
			this.UnHighilight();
		}
	}

	// Token: 0x06000ADC RID: 2780 RVA: 0x000395BA File Offset: 0x000377BA
	private void OnDisable()
	{
		this.UnHighilight();
		this.wasHighlighted = false;
		this.highlighted = false;
	}

	// Token: 0x06000ADD RID: 2781 RVA: 0x000395D0 File Offset: 0x000377D0
	public override void Highlight()
	{
		base.Highlight();
		if (!this.wasHighlighted)
		{
			if (this.swapOnHigh && (!this.held || !this.swapOnHold))
			{
				this.image.sprite = this.highlightedSprite;
			}
			if (this.eventOnHigh)
			{
				this.OnHighlight.Invoke();
			}
			if (this.underlineOnHigh)
			{
				this.text.fontStyle = FontStyles.Underline;
			}
			if (this.animateOnHigh)
			{
				this.animator.Play(this.highlightAnimation, -1, 0f);
				this.animator.speed = 1f;
			}
		}
		this.wasHighlighted = true;
		this.highlighted = true;
	}

	// Token: 0x04000C5B RID: 3163
	public UnityEvent OnHighlight;

	// Token: 0x04000C5C RID: 3164
	public UnityEvent OffHighlight;

	// Token: 0x04000C5D RID: 3165
	public UnityEvent OnRelease;

	// Token: 0x04000C5E RID: 3166
	public Image image;

	// Token: 0x04000C5F RID: 3167
	public bool swapOnHigh;

	// Token: 0x04000C60 RID: 3168
	public bool swapOnHold;

	// Token: 0x04000C61 RID: 3169
	public bool animateOnHigh;

	// Token: 0x04000C62 RID: 3170
	public bool eventOnHigh;

	// Token: 0x04000C63 RID: 3171
	public bool underlineOnHigh;

	// Token: 0x04000C64 RID: 3172
	public bool unhighlightOnEnable;

	// Token: 0x04000C65 RID: 3173
	public bool transitionOnPress;

	// Token: 0x04000C66 RID: 3174
	public Sprite unhighlightedSprite;

	// Token: 0x04000C67 RID: 3175
	public Sprite highlightedSprite;

	// Token: 0x04000C68 RID: 3176
	public Sprite heldSprite;

	// Token: 0x04000C69 RID: 3177
	public UiTransition transitionType;

	// Token: 0x04000C6A RID: 3178
	public float transitionTime;

	// Token: 0x04000C6B RID: 3179
	public TMP_Text text;

	// Token: 0x04000C6C RID: 3180
	public Animator animator;

	// Token: 0x04000C6D RID: 3181
	public string highlightAnimation;

	// Token: 0x04000C6E RID: 3182
	public string unHighlightAnimation;
}
