using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200009D RID: 157
public class Activity : MonoBehaviour
{
	// Token: 0x06000382 RID: 898 RVA: 0x000126DE File Offset: 0x000108DE
	public virtual void Completed(int player, bool correct, Activity activity)
	{
		this.Completed(player);
		this.completed = true;
		Singleton<BaseGameManager>.Instance.ActivityCompleted(correct, activity);
	}

	// Token: 0x06000383 RID: 899 RVA: 0x000126FA File Offset: 0x000108FA
	public virtual void Completed(int player)
	{
		if (!this.bonusMode)
		{
			this.notebook.gameObject.SetActive(true);
			this.notebook.Hide(false);
		}
	}

	// Token: 0x06000384 RID: 900 RVA: 0x00012721 File Offset: 0x00010921
	public void SetNotebook(Notebook val)
	{
		this.notebook = val;
		val.activity = this;
	}

	// Token: 0x06000385 RID: 901 RVA: 0x00012731 File Offset: 0x00010931
	public virtual void ReInit()
	{
		if (this.notebook.icon != null && !this.bonusMode)
		{
			this.notebook.icon.spriteRenderer.enabled = true;
		}
		this.completed = false;
	}

	// Token: 0x06000386 RID: 902 RVA: 0x0001276B File Offset: 0x0001096B
	public virtual void Corrupt(bool val)
	{
		this.corrupted = val;
	}

	// Token: 0x06000387 RID: 903 RVA: 0x00012774 File Offset: 0x00010974
	public virtual void SetBonusMode(bool val)
	{
		this.bonusMode = val;
	}

	// Token: 0x06000388 RID: 904 RVA: 0x0001277D File Offset: 0x0001097D
	public virtual void StartResetTimer(float time)
	{
		base.StartCoroutine(this.ResetTimer(time));
	}

	// Token: 0x06000389 RID: 905 RVA: 0x0001278D File Offset: 0x0001098D
	private IEnumerator ResetTimer(float time)
	{
		while (time > 0f)
		{
			if (!this.trigger.HasPlayer)
			{
				time -= Time.deltaTime * this.room.ec.EnvironmentTimeScale;
			}
			yield return null;
		}
		this.ReInit();
		this.audMan.PlaySingle(this.audRespawn);
		yield break;
	}

	// Token: 0x17000032 RID: 50
	// (get) Token: 0x0600038A RID: 906 RVA: 0x000127A3 File Offset: 0x000109A3
	public bool IsCompleted
	{
		get
		{
			return this.completed;
		}
	}

	// Token: 0x17000033 RID: 51
	// (get) Token: 0x0600038B RID: 907 RVA: 0x000127AB File Offset: 0x000109AB
	public bool Corrupted
	{
		get
		{
			return this.corrupted;
		}
	}

	// Token: 0x17000034 RID: 52
	// (get) Token: 0x0600038C RID: 908 RVA: 0x000127B3 File Offset: 0x000109B3
	public bool InBonusMode
	{
		get
		{
			return this.bonusMode;
		}
	}

	// Token: 0x040003AE RID: 942
	[SerializeField]
	protected Notebook notebook;

	// Token: 0x040003AF RID: 943
	public RoomController room;

	// Token: 0x040003B0 RID: 944
	[SerializeField]
	protected ColliderGroup trigger;

	// Token: 0x040003B1 RID: 945
	[SerializeField]
	protected AudioManager audMan;

	// Token: 0x040003B2 RID: 946
	[SerializeField]
	protected SoundObject audRespawn;

	// Token: 0x040003B3 RID: 947
	[SerializeField]
	protected float baldiPause = 1f;

	// Token: 0x040003B4 RID: 948
	protected bool completed;

	// Token: 0x040003B5 RID: 949
	protected bool corrupted;

	// Token: 0x040003B6 RID: 950
	protected bool bonusMode;
}
