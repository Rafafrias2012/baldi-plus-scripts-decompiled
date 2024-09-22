using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000205 RID: 517
public class BaldiTV : MonoBehaviour
{
	// Token: 0x06000B74 RID: 2932 RVA: 0x0003C33A File Offset: 0x0003A53A
	public void Initialize()
	{
	}

	// Token: 0x06000B75 RID: 2933 RVA: 0x0003C33C File Offset: 0x0003A53C
	private void Update()
	{
		if (this.queuedEnumerators.Count > 0 && !this.busy)
		{
			base.StartCoroutine(this.queuedEnumerators[0]);
			this.baldiTvAnimator.SetBool("Active", this.moves);
			this.busy = true;
			this.queuedEnumerators.RemoveAt(0);
		}
		if (this.staticImage.sprite == this.static1)
		{
			this.staticImage.sprite = this.static2;
			return;
		}
		this.staticImage.sprite = this.static1;
	}

	// Token: 0x06000B76 RID: 2934 RVA: 0x0003C3D8 File Offset: 0x0003A5D8
	public void ReInit()
	{
		base.StopAllCoroutines();
		this.queuedEnumerators.Clear();
		this.baldiTvAudioManager.FlushQueue(true);
		this.baldiTvAnimator.SetBool("Active", false);
		this.baldiTvAnimator.Play("BaldiTV_Idle", -1, 0f);
		this.ResetScreen();
		this.busy = false;
	}

	// Token: 0x06000B77 RID: 2935 RVA: 0x0003C436 File Offset: 0x0003A636
	private void QueueCheck()
	{
		if (this.queuedEnumerators.Count == 0)
		{
			this.baldiTvAnimator.SetBool("Active", false);
		}
	}

	// Token: 0x06000B78 RID: 2936 RVA: 0x0003C456 File Offset: 0x0003A656
	private void ResetScreen()
	{
		this.baldiImage.enabled = false;
		this.staticObject.SetActive(false);
		this.exclamationObject.SetActive(false);
	}

	// Token: 0x06000B79 RID: 2937 RVA: 0x0003C47C File Offset: 0x0003A67C
	private void QueueEnumerator(IEnumerator enumerator)
	{
		this.queuedEnumerators.Add(enumerator);
	}

	// Token: 0x06000B7A RID: 2938 RVA: 0x0003C48C File Offset: 0x0003A68C
	public void Speak(SoundObject sound)
	{
		if (!this.busy)
		{
			this.QueueEnumerator(this.Delay(1f));
		}
		this.QueueEnumerator(this.Static(0.25f));
		this.QueueEnumerator(this.BaldiSpeaks(sound));
		this.QueueEnumerator(this.Static(0.25f));
	}

	// Token: 0x06000B7B RID: 2939 RVA: 0x0003C4E4 File Offset: 0x0003A6E4
	public void AnnounceEvent(SoundObject sound)
	{
		if (!this.busy)
		{
			this.QueueEnumerator(this.Exclamation(2.5f));
		}
		this.QueueEnumerator(this.Static(0.25f));
		this.QueueEnumerator(this.BaldiSpeaks(sound));
		this.QueueEnumerator(this.Static(0.25f));
	}

	// Token: 0x06000B7C RID: 2940 RVA: 0x0003C539 File Offset: 0x0003A739
	private IEnumerator Delay(float duration)
	{
		this.ResetScreen();
		while (duration > 0f)
		{
			duration -= Time.deltaTime;
			yield return null;
		}
		this.busy = false;
		this.QueueCheck();
		yield break;
	}

	// Token: 0x06000B7D RID: 2941 RVA: 0x0003C54F File Offset: 0x0003A74F
	private IEnumerator Exclamation(float duration)
	{
		this.ResetScreen();
		this.exclamationObject.SetActive(true);
		while (duration > 0f)
		{
			duration -= Time.deltaTime;
			yield return null;
		}
		this.busy = false;
		this.QueueCheck();
		yield break;
	}

	// Token: 0x06000B7E RID: 2942 RVA: 0x0003C565 File Offset: 0x0003A765
	private IEnumerator BaldiSpeaks(SoundObject sound)
	{
		this.ResetScreen();
		this.baldiImage.enabled = true;
		this.baldiTvAudioManager.FlushQueue(true);
		this.baldiTvAudioManager.QueueAudio(sound);
		yield return null;
		while (this.baldiTvAudioManager.QueuedAudioIsPlaying)
		{
			yield return null;
		}
		this.busy = false;
		this.QueueCheck();
		yield break;
	}

	// Token: 0x06000B7F RID: 2943 RVA: 0x0003C57B File Offset: 0x0003A77B
	private IEnumerator Static(float duration)
	{
		this.ResetScreen();
		this.staticObject.SetActive(true);
		while (duration > 0f)
		{
			duration -= Time.deltaTime;
			yield return null;
		}
		this.staticObject.SetActive(false);
		this.busy = false;
		this.QueueCheck();
		yield break;
	}

	// Token: 0x04000DC4 RID: 3524
	[SerializeField]
	private GameObject staticObject;

	// Token: 0x04000DC5 RID: 3525
	[SerializeField]
	private GameObject exclamationObject;

	// Token: 0x04000DC6 RID: 3526
	[SerializeField]
	private Animator baldiTvAnimator;

	// Token: 0x04000DC7 RID: 3527
	[SerializeField]
	private AudioManager baldiTvAudioManager;

	// Token: 0x04000DC8 RID: 3528
	[SerializeField]
	private Image baldiImage;

	// Token: 0x04000DC9 RID: 3529
	[SerializeField]
	private Image staticImage;

	// Token: 0x04000DCA RID: 3530
	[SerializeField]
	private Sprite static1;

	// Token: 0x04000DCB RID: 3531
	[SerializeField]
	private Sprite static2;

	// Token: 0x04000DCC RID: 3532
	private List<IEnumerator> queuedEnumerators = new List<IEnumerator>();

	// Token: 0x04000DCD RID: 3533
	[SerializeField]
	private bool moves;

	// Token: 0x04000DCE RID: 3534
	private bool busy;
}
