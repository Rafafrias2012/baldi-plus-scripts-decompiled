using System;
using UnityEngine;

// Token: 0x02000080 RID: 128
public class MinigameTutorial : Minigame
{
	// Token: 0x06000300 RID: 768 RVA: 0x0000F9C6 File Offset: 0x0000DBC6
	public override void Initialize(MinigameBase minigameBase, bool scoringMode)
	{
		base.Initialize(minigameBase, scoringMode);
		this.ChangePage(0);
		Singleton<MusicManager>.Instance.PlayMidi(this.music, true);
		Singleton<MusicManager>.Instance.SetSpeed(0.5f);
	}

	// Token: 0x06000301 RID: 769 RVA: 0x0000F9F8 File Offset: 0x0000DBF8
	private void Update()
	{
		base.VirtualUpdate();
		this.transitionProgress += Time.unscaledDeltaTime;
		this.tutorBaldi.SetPosition(Vector2.Lerp(this.tutorBaldi.Position, this.targetPosition, this.transitionProgress));
		this.tutorBaldi.SetScale(Vector2.Lerp(this.tutorBaldi.Scale, this.targetScale, this.transitionProgress));
	}

	// Token: 0x06000302 RID: 770 RVA: 0x0000FA6C File Offset: 0x0000DC6C
	public void ChangePage(int advance)
	{
		this.screen[this.page].SetActive(false);
		if (!base.minigameBase.AudioManager.AnyAudioIsPlaying)
		{
			this.StartRandomTutorBaldiAnimation();
		}
		base.minigameBase.AudioManager.FlushQueue(true);
		this.page = Mathf.Clamp(this.page + advance, 0, this.screen.Length - 1);
		this.backButton.SetActive(this.page != 0);
		this.forwardButton.SetActive(this.page != this.screen.Length - 1);
		this.screen[this.page].SetActive(true);
		base.minigameBase.AudioManager.QueueAudio(this.audVoice[this.page]);
		this.targetPosition.x = this.positionAndScale[this.page].x;
		this.targetPosition.y = this.positionAndScale[this.page].y;
		this.transitionProgress = 0f;
		this.tutorBaldi.SetScale(new Vector2(Mathf.Sign(this.positionAndScale[this.page].z) * Mathf.Abs(this.tutorBaldi.Scale.x), this.tutorBaldi.Scale.y));
		this.targetScale = new Vector2(this.positionAndScale[this.page].z, Mathf.Abs(this.positionAndScale[this.page].z));
		this.tooltipController.CloseTooltip();
		if (this.tooltipKey[this.page].Length > 0)
		{
			this.tooltipController.UpdateTooltip(this.tooltipKey[this.page]);
		}
		if (this.targetPosition.y > 0f)
		{
			this.tooltipController.SetPosition(new Vector2(240f, 64f));
			return;
		}
		this.tooltipController.SetPosition(new Vector2(240f, 296f));
	}

	// Token: 0x06000303 RID: 771 RVA: 0x0000FC90 File Offset: 0x0000DE90
	public void StartRandomTutorBaldiAnimation()
	{
		this.tutorBaldiAnimator.Play(this.animation[Random.Range(0, this.animation.Length)], -1, 0f);
	}

	// Token: 0x06000304 RID: 772 RVA: 0x0000FCB8 File Offset: 0x0000DEB8
	public void RandomTutorBaldiAnimation()
	{
		if (base.minigameBase.AudioManager.AnyAudioIsPlaying)
		{
			this.tutorBaldiAnimator.Play(this.animation[Random.Range(0, this.animation.Length)], -1, 0f);
		}
	}

	// Token: 0x04000322 RID: 802
	[SerializeField]
	private GameObject[] screen = new GameObject[0];

	// Token: 0x04000323 RID: 803
	[SerializeField]
	private string[] tooltipKey = new string[0];

	// Token: 0x04000324 RID: 804
	[SerializeField]
	private SoundObject[] audVoice = new SoundObject[0];

	// Token: 0x04000325 RID: 805
	[SerializeField]
	private Vector3[] positionAndScale = new Vector3[0];

	// Token: 0x04000326 RID: 806
	[SerializeField]
	private string[] animation = new string[0];

	// Token: 0x04000327 RID: 807
	[SerializeField]
	private SpriteController tutorBaldi;

	// Token: 0x04000328 RID: 808
	[SerializeField]
	private Animator tutorBaldiAnimator;

	// Token: 0x04000329 RID: 809
	[SerializeField]
	private TooltipController tooltipController;

	// Token: 0x0400032A RID: 810
	[SerializeField]
	private GameObject backButton;

	// Token: 0x0400032B RID: 811
	[SerializeField]
	private GameObject forwardButton;

	// Token: 0x0400032C RID: 812
	private Vector2 targetPosition;

	// Token: 0x0400032D RID: 813
	private Vector2 targetScale;

	// Token: 0x0400032E RID: 814
	public string music;

	// Token: 0x0400032F RID: 815
	private float transitionProgress = 1f;

	// Token: 0x04000330 RID: 816
	private int page;
}
