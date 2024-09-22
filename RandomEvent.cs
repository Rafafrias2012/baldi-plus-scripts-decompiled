using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000079 RID: 121
public class RandomEvent : MonoBehaviour
{
	// Token: 0x060002B6 RID: 694 RVA: 0x0000EE4F File Offset: 0x0000D04F
	public virtual void Initialize(EnvironmentController controller, Random rng)
	{
		this.ec = controller;
		this.crng = new Random(rng.Next());
	}

	// Token: 0x060002B7 RID: 695 RVA: 0x0000EE69 File Offset: 0x0000D069
	public virtual void AfterUpdateSetup()
	{
	}

	// Token: 0x060002B8 RID: 696 RVA: 0x0000EE6B File Offset: 0x0000D06B
	public virtual void PremadeSetup()
	{
	}

	// Token: 0x060002B9 RID: 697 RVA: 0x0000EE6D File Offset: 0x0000D06D
	public virtual void AssignRoom(RoomController room)
	{
		this.room = room;
	}

	// Token: 0x060002BA RID: 698 RVA: 0x0000EE78 File Offset: 0x0000D078
	public virtual void Begin()
	{
		this.active = true;
		this.eventTimer = this.Timer(this.eventTime);
		base.StartCoroutine(this.eventTimer);
		if (Singleton<CoreGameManager>.Instance.currentMode == Mode.Main)
		{
			Singleton<PlayerFileManager>.Instance.Find(Singleton<PlayerFileManager>.Instance.foundEvnts, (int)this.eventType);
		}
	}

	// Token: 0x060002BB RID: 699 RVA: 0x0000EED1 File Offset: 0x0000D0D1
	public virtual void End()
	{
		this.active = false;
		this.ec.EventOver(this);
	}

	// Token: 0x060002BC RID: 700 RVA: 0x0000EEE6 File Offset: 0x0000D0E6
	public virtual void ResetConditions()
	{
	}

	// Token: 0x060002BD RID: 701 RVA: 0x0000EEE8 File Offset: 0x0000D0E8
	public virtual void Pause()
	{
	}

	// Token: 0x060002BE RID: 702 RVA: 0x0000EEEA File Offset: 0x0000D0EA
	public virtual void Unpause()
	{
	}

	// Token: 0x060002BF RID: 703 RVA: 0x0000EEEC File Offset: 0x0000D0EC
	public void SetEventTime(Random rng)
	{
		this.eventTime = (float)rng.NextDouble() * (this.maxEventTime - this.minEventTime) + this.minEventTime;
	}

	// Token: 0x17000026 RID: 38
	// (get) Token: 0x060002C0 RID: 704 RVA: 0x0000EF10 File Offset: 0x0000D110
	public float EventTime
	{
		get
		{
			return this.eventTime;
		}
	}

	// Token: 0x060002C1 RID: 705 RVA: 0x0000EF18 File Offset: 0x0000D118
	private IEnumerator Timer(float time)
	{
		this.remainingTime = time;
		while (this.remainingTime > 0f)
		{
			this.remainingTime -= Time.deltaTime * this.ec.EnvironmentTimeScale;
			yield return null;
		}
		this.End();
		yield break;
	}

	// Token: 0x17000027 RID: 39
	// (get) Token: 0x060002C2 RID: 706 RVA: 0x0000EF2E File Offset: 0x0000D12E
	public bool Active
	{
		get
		{
			return this.active;
		}
	}

	// Token: 0x17000028 RID: 40
	// (get) Token: 0x060002C3 RID: 707 RVA: 0x0000EF36 File Offset: 0x0000D136
	public RandomEventType Type
	{
		get
		{
			return this.eventType;
		}
	}

	// Token: 0x17000029 RID: 41
	// (get) Token: 0x060002C4 RID: 708 RVA: 0x0000EF3E File Offset: 0x0000D13E
	public WeightedRoomAsset[] PotentialRoomAssets
	{
		get
		{
			return this.potentialRoomAssets;
		}
	}

	// Token: 0x1700002A RID: 42
	// (get) Token: 0x060002C5 RID: 709 RVA: 0x0000EF46 File Offset: 0x0000D146
	public SoundObject EventIntro
	{
		get
		{
			return this.eventIntro;
		}
	}

	// Token: 0x040002D7 RID: 727
	protected IEnumerator eventTimer;

	// Token: 0x040002D8 RID: 728
	[SerializeField]
	protected RandomEventType eventType = RandomEventType.Fog;

	// Token: 0x040002D9 RID: 729
	[SerializeField]
	protected EnvironmentController ec;

	// Token: 0x040002DA RID: 730
	[SerializeField]
	protected Random crng = new Random();

	// Token: 0x040002DB RID: 731
	[SerializeField]
	protected SoundObject eventIntro;

	// Token: 0x040002DC RID: 732
	protected RoomController room;

	// Token: 0x040002DD RID: 733
	[SerializeField]
	protected WeightedRoomAsset[] potentialRoomAssets = new WeightedRoomAsset[0];

	// Token: 0x040002DE RID: 734
	[SerializeField]
	private float eventTime;

	// Token: 0x040002DF RID: 735
	[SerializeField]
	protected float minEventTime = 30f;

	// Token: 0x040002E0 RID: 736
	[SerializeField]
	protected float maxEventTime = 60f;

	// Token: 0x040002E1 RID: 737
	[SerializeField]
	protected float descriptionTime = 5f;

	// Token: 0x040002E2 RID: 738
	protected float remainingTime;

	// Token: 0x040002E3 RID: 739
	protected bool active;
}
