using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000083 RID: 131
public class ITM_AlarmClock : Item, IClickable<int>
{
	// Token: 0x0600030D RID: 781 RVA: 0x0000FEC0 File Offset: 0x0000E0C0
	public override bool Use(PlayerManager pm)
	{
		this.ec = pm.ec;
		base.transform.position = pm.transform.position;
		this.entity.Initialize(this.ec, base.transform.position);
		base.StartCoroutine(this.Timer(this.setTime[this.initSetTime]));
		return true;
	}

	// Token: 0x0600030E RID: 782 RVA: 0x0000FF26 File Offset: 0x0000E126
	private IEnumerator Timer(float initTime)
	{
		this.time = initTime;
		while (this.time > 0f)
		{
			this.time -= Time.deltaTime * this.ec.EnvironmentTimeScale;
			if (this.time <= this.setTime[0])
			{
				this.spriteRenderer.sprite = this.clockSprite[0];
			}
			else if (this.time <= this.setTime[1])
			{
				this.spriteRenderer.sprite = this.clockSprite[1];
			}
			else if (this.time <= this.setTime[2])
			{
				this.spriteRenderer.sprite = this.clockSprite[2];
			}
			else
			{
				this.spriteRenderer.sprite = this.clockSprite[3];
			}
			yield return null;
		}
		this.ec.MakeNoise(base.transform.position, this.noiseVal);
		this.audMan.FlushQueue(true);
		this.audMan.PlaySingle(this.audRing);
		this.finished = true;
		this.spriteRenderer.sprite = this.clockSprite[3];
		while (this.audMan.QueuedAudioIsPlaying)
		{
			yield return null;
		}
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x0600030F RID: 783 RVA: 0x0000FF3C File Offset: 0x0000E13C
	public void Clicked(int playerNumber)
	{
		if (!this.finished)
		{
			this.audMan.PlaySingle(this.audWind);
			if (this.time <= this.setTime[0])
			{
				this.time = this.setTime[1];
				return;
			}
			if (this.time <= this.setTime[1])
			{
				this.time = this.setTime[2];
				return;
			}
			if (this.time <= this.setTime[2])
			{
				this.time = this.setTime[3];
				return;
			}
			this.time = this.setTime[0];
		}
	}

	// Token: 0x06000310 RID: 784 RVA: 0x0000FFCD File Offset: 0x0000E1CD
	public void ClickableSighted(int player)
	{
	}

	// Token: 0x06000311 RID: 785 RVA: 0x0000FFCF File Offset: 0x0000E1CF
	public void ClickableUnsighted(int player)
	{
	}

	// Token: 0x06000312 RID: 786 RVA: 0x0000FFD1 File Offset: 0x0000E1D1
	public bool ClickableHidden()
	{
		return false;
	}

	// Token: 0x06000313 RID: 787 RVA: 0x0000FFD4 File Offset: 0x0000E1D4
	public bool ClickableRequiresNormalHeight()
	{
		return true;
	}

	// Token: 0x04000336 RID: 822
	private EnvironmentController ec;

	// Token: 0x04000337 RID: 823
	[SerializeField]
	private Entity entity;

	// Token: 0x04000338 RID: 824
	[SerializeField]
	private AudioManager audMan;

	// Token: 0x04000339 RID: 825
	[SerializeField]
	private SoundObject audRing;

	// Token: 0x0400033A RID: 826
	[SerializeField]
	private SoundObject audWind;

	// Token: 0x0400033B RID: 827
	[SerializeField]
	private SpriteRenderer spriteRenderer;

	// Token: 0x0400033C RID: 828
	[SerializeField]
	private Sprite[] clockSprite = new Sprite[4];

	// Token: 0x0400033D RID: 829
	[SerializeField]
	private float[] setTime = new float[]
	{
		15f,
		30f,
		45f,
		60f
	};

	// Token: 0x0400033E RID: 830
	private float time;

	// Token: 0x0400033F RID: 831
	[SerializeField]
	[Range(0f, 3f)]
	private int initSetTime = 1;

	// Token: 0x04000340 RID: 832
	[SerializeField]
	[Range(0f, 127f)]
	private int noiseVal = 112;

	// Token: 0x04000341 RID: 833
	private bool finished;
}
