using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200003C RID: 60
public class GottaSweep : NPC
{
	// Token: 0x06000176 RID: 374 RVA: 0x000090E6 File Offset: 0x000072E6
	public override void Initialize()
	{
		base.Initialize();
		this.behaviorStateMachine.ChangeState(new GottaSweep_Wait(this, this));
	}

	// Token: 0x06000177 RID: 375 RVA: 0x00009100 File Offset: 0x00007300
	private void Start()
	{
		this.home = base.transform.position;
		this.homeCell = this.ec.CellFromPosition(this.home);
	}

	// Token: 0x06000178 RID: 376 RVA: 0x0000912C File Offset: 0x0000732C
	protected override void VirtualUpdate()
	{
		base.VirtualUpdate();
		this.moveMod.movementAddend = this.navigator.Velocity.normalized * this.navigator.speed * this.moveModMultiplier * this.navigator.Am.Multiplier;
	}

	// Token: 0x1700001C RID: 28
	// (get) Token: 0x06000179 RID: 377 RVA: 0x0000918D File Offset: 0x0000738D
	public float GetRandomDelay
	{
		get
		{
			return Random.Range(this.minDelay, this.maxDelay);
		}
	}

	// Token: 0x1700001D RID: 29
	// (get) Token: 0x0600017A RID: 378 RVA: 0x000091A0 File Offset: 0x000073A0
	public float GetRandomSweepTime
	{
		get
		{
			return Random.Range(this.minActive, this.maxActive);
		}
	}

	// Token: 0x1700001E RID: 30
	// (get) Token: 0x0600017B RID: 379 RVA: 0x000091B3 File Offset: 0x000073B3
	public bool IsHome
	{
		get
		{
			return this.ec.CellFromPosition(base.transform.position) == this.homeCell;
		}
	}

	// Token: 0x0600017C RID: 380 RVA: 0x000091D3 File Offset: 0x000073D3
	public void StartSweeping()
	{
		this.audMan.PlaySingle(this.audIntro);
		this.navigator.SetSpeed(this.speed);
		this.moveMod.forceTrigger = true;
	}

	// Token: 0x0600017D RID: 381 RVA: 0x00009203 File Offset: 0x00007403
	public void StopSweeping()
	{
		this.moveMod.forceTrigger = false;
		this.navigator.SetSpeed(0f);
		this.navigator.maxSpeed = 0f;
	}

	// Token: 0x0600017E RID: 382 RVA: 0x00009234 File Offset: 0x00007434
	protected override void VirtualOnTriggerEnter(Collider other)
	{
		if (other.isTrigger)
		{
			Entity component = other.GetComponent<Entity>();
			if (component != null)
			{
				this.audMan.PlaySingle(this.audSweep);
				ActivityModifier externalActivity = component.ExternalActivity;
				if (!externalActivity.moveMods.Contains(this.moveMod))
				{
					externalActivity.moveMods.Add(this.moveMod);
					this.actMods.Add(externalActivity);
				}
			}
		}
	}

	// Token: 0x0600017F RID: 383 RVA: 0x000092A4 File Offset: 0x000074A4
	protected override void VirtualOnTriggerExit(Collider other)
	{
		if (other.isTrigger)
		{
			Entity component = other.GetComponent<Entity>();
			if (component != null)
			{
				ActivityModifier externalActivity = component.ExternalActivity;
				externalActivity.moveMods.Remove(this.moveMod);
				this.actMods.Remove(externalActivity);
			}
		}
	}

	// Token: 0x06000180 RID: 384 RVA: 0x000092F0 File Offset: 0x000074F0
	public override void Despawn()
	{
		foreach (ActivityModifier activityModifier in this.actMods)
		{
			activityModifier.moveMods.Remove(this.moveMod);
		}
		base.Despawn();
	}

	// Token: 0x04000185 RID: 389
	public Vector3 home;

	// Token: 0x04000186 RID: 390
	private Cell homeCell;

	// Token: 0x04000187 RID: 391
	[SerializeField]
	private MovementModifier moveMod;

	// Token: 0x04000188 RID: 392
	private List<ActivityModifier> actMods = new List<ActivityModifier>();

	// Token: 0x04000189 RID: 393
	[SerializeField]
	private float minDelay = 60f;

	// Token: 0x0400018A RID: 394
	[SerializeField]
	private float maxDelay = 180f;

	// Token: 0x0400018B RID: 395
	[SerializeField]
	private float minActive = 30f;

	// Token: 0x0400018C RID: 396
	[SerializeField]
	private float maxActive = 60f;

	// Token: 0x0400018D RID: 397
	[SerializeField]
	private float speed = 40f;

	// Token: 0x0400018E RID: 398
	[SerializeField]
	private float moveModMultiplier = 0.9f;

	// Token: 0x0400018F RID: 399
	[SerializeField]
	private AudioManager audMan;

	// Token: 0x04000190 RID: 400
	[SerializeField]
	private SoundObject audIntro;

	// Token: 0x04000191 RID: 401
	[SerializeField]
	private SoundObject audSweep;
}
