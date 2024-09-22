using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000217 RID: 535
public class SubtitleController : MonoBehaviour
{
	// Token: 0x06000BF8 RID: 3064 RVA: 0x0003EEC8 File Offset: 0x0003D0C8
	public void Initialize()
	{
		this.text.text = this.contents;
		this.text.color = this.color;
		if (this.hasPosition)
		{
			this.PositionSub();
		}
		else if (this.sourceAudMan.overrideCaptionPosition)
		{
			this.SetCustomPosition();
		}
		else
		{
			this.PositionInQueue();
		}
		if (!this.loop)
		{
			base.StartCoroutine("Die");
		}
		if (!Singleton<PlayerFileManager>.Instance.subtitles || (this.sourceAudMan.positional && !this.hasPosition))
		{
			this.Hide(true);
		}
	}

	// Token: 0x06000BF9 RID: 3065 RVA: 0x0003EF60 File Offset: 0x0003D160
	private void Update()
	{
		if (this.sourceAudMan == null)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		if ((AudioListener.pause && !this.sourceAudMan.ignoreListenerPause) || !Singleton<PlayerFileManager>.Instance.subtitles || !this.sourceAudMan.gameObject.activeInHierarchy)
		{
			this.Hide(true);
		}
		else
		{
			this.Hide(false);
		}
		if (this.hasPosition)
		{
			this.PositionSub();
			return;
		}
		if (this.sourceAudMan.overrideCaptionPosition)
		{
			this.SetCustomPosition();
			return;
		}
		this.PositionInQueue();
		if (this.sourceAudMan.positional)
		{
			this.Hide(true);
			if (!this.hasPosition && Singleton<CoreGameManager>.Instance != null && Singleton<CoreGameManager>.Instance.GetCamera(0) != null)
			{
				this.camTran = Singleton<CoreGameManager>.Instance.GetCamera(0).transform;
				this.hasPosition = true;
			}
		}
	}

	// Token: 0x06000BFA RID: 3066 RVA: 0x0003F04C File Offset: 0x0003D24C
	private void Hide(bool hide)
	{
		if (hide && !this.hidden)
		{
			this.text.enabled = false;
			this.bg.enabled = false;
			this.hidden = true;
			return;
		}
		if (!hide && this.hidden)
		{
			this.text.enabled = true;
			this.bg.enabled = true;
			this.hidden = false;
		}
	}

	// Token: 0x06000BFB RID: 3067 RVA: 0x0003F0B0 File Offset: 0x0003D2B0
	private void PositionSub()
	{
		if (this.soundTran != null && this.camTran != null)
		{
			this.reverseVal = 1f;
			if (this.subMan.Reversed)
			{
				this.reverseVal = -1f;
			}
			float num = Mathf.Atan2(this.camTran.position.z - this.soundTran.position.z, this.camTran.position.x - this.soundTran.position.x) * 57.29578f + this.camTran.eulerAngles.y + 180f;
			this.anchoredPos.x = Mathf.Cos(num * 0.017453292f) * this.radius * this.reverseVal;
			this.anchoredPos.y = Mathf.Sin(num * 0.017453292f) * this.radius;
			this.anchoredPos.z = 0f;
			this.rectTransform.anchoredPosition = this.anchoredPos;
			float subtitleScale = this.sourceAudMan.GetSubtitleScale(this.camTran);
			this.localScale.x = subtitleScale;
			this.localScale.y = subtitleScale;
			this.localScale.z = 1f;
			this.rectTransform.localScale = this.localScale;
		}
	}

	// Token: 0x06000BFC RID: 3068 RVA: 0x0003F21C File Offset: 0x0003D41C
	private void SetCustomPosition()
	{
		this.rectTransform.anchorMin = this.sourceAudMan.captionAnchor;
		this.rectTransform.anchorMax = this.sourceAudMan.captionAnchor;
		this.anchoredPos = this.sourceAudMan.captionPosition;
		this.rectTransform.anchoredPosition = this.anchoredPos;
		this.rectTransform.localScale = Vector3.one;
	}

	// Token: 0x06000BFD RID: 3069 RVA: 0x0003F28C File Offset: 0x0003D48C
	private void PositionInQueue()
	{
		this.anchoredPos.x = 0f;
		this.anchoredPos.y = -100f;
		this.anchoredPos.z = 0f;
		this.rectTransform.anchoredPosition = this.anchoredPos;
		this.rectTransform.localScale = Vector3.one;
	}

	// Token: 0x06000BFE RID: 3070 RVA: 0x0003F2EF File Offset: 0x0003D4EF
	private IEnumerator Die()
	{
		while (this.duration > 0f && this.sourceAudMan != null)
		{
			if (this.sourceAudMan.useUnscaledPitch || this.sourceAudMan.ignoreListenerPause)
			{
				this.duration -= Time.unscaledDeltaTime;
			}
			else
			{
				this.duration -= Time.deltaTime;
			}
			int i = this.soundObject.additionalKeys.Length - 1;
			while (i >= 0)
			{
				if (this.soundObject.subDuration - this.duration > this.soundObject.additionalKeys[i].time)
				{
					if (this.currentKey != this.soundObject.additionalKeys[i].key)
					{
						this.contents = Singleton<LocalizationManager>.Instance.GetLocalizedText(this.soundObject.additionalKeys[i].key, this.soundObject.additionalKeys[i].encrypted);
						this.currentKey = this.soundObject.additionalKeys[i].key;
						this.text.text = this.contents;
						break;
					}
					break;
				}
				else
				{
					i--;
				}
			}
			yield return null;
		}
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x04000E77 RID: 3703
	public RectTransform rectTransform;

	// Token: 0x04000E78 RID: 3704
	public SubtitleManager subMan;

	// Token: 0x04000E79 RID: 3705
	public AudioManager sourceAudMan;

	// Token: 0x04000E7A RID: 3706
	public SoundObject soundObject;

	// Token: 0x04000E7B RID: 3707
	public Transform soundTran;

	// Token: 0x04000E7C RID: 3708
	public Transform camTran;

	// Token: 0x04000E7D RID: 3709
	public TMP_Text text;

	// Token: 0x04000E7E RID: 3710
	public Image bg;

	// Token: 0x04000E7F RID: 3711
	public Color color;

	// Token: 0x04000E80 RID: 3712
	private Vector3 anchoredPos;

	// Token: 0x04000E81 RID: 3713
	private Vector3 localScale;

	// Token: 0x04000E82 RID: 3714
	public float duration;

	// Token: 0x04000E83 RID: 3715
	public float distance;

	// Token: 0x04000E84 RID: 3716
	public float radius;

	// Token: 0x04000E85 RID: 3717
	public float reverseVal;

	// Token: 0x04000E86 RID: 3718
	public string contents;

	// Token: 0x04000E87 RID: 3719
	public string currentKey;

	// Token: 0x04000E88 RID: 3720
	public bool loop;

	// Token: 0x04000E89 RID: 3721
	public bool hasPosition;

	// Token: 0x04000E8A RID: 3722
	private bool hidden;
}
