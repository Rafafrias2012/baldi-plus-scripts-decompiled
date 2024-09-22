using System;
using UnityEngine;

// Token: 0x02000041 RID: 65
public class LookAtGuy : NPC
{
	// Token: 0x0600018C RID: 396 RVA: 0x00009596 File Offset: 0x00007796
	public override void Initialize()
	{
		base.Initialize();
		this.behaviorStateMachine.ChangeState(new LookAtGuy_Inactive(this));
	}

	// Token: 0x0600018D RID: 397 RVA: 0x000095AF File Offset: 0x000077AF
	protected override void VirtualUpdate()
	{
		base.VirtualUpdate();
	}

	// Token: 0x0600018E RID: 398 RVA: 0x000095B7 File Offset: 0x000077B7
	public void Activate()
	{
		this.animator.Play("Rise", -1, 0f);
		this.rumbleAudMan.PlaySingle(this.audActivate);
		this.audMan.PlaySingle(this.audLoop);
	}

	// Token: 0x0600018F RID: 399 RVA: 0x000095F4 File Offset: 0x000077F4
	public void ChargePlayer(PlayerManager player)
	{
		this.behaviorStateMachine.CurrentNavigationState.priority = 0;
		this.behaviorStateMachine.ChangeNavigationState(new NavigationState_TargetPlayer(this, 127, player.transform.position, true));
		if (this.currentSpeedLevel >= this.speedLevels.Length)
		{
			base.transform.position = this.players[0].transform.position;
			return;
		}
		this.navigator.SetSpeed(this.speedLevels[this.currentSpeedLevel]);
		this.moveSpeed = this.speedLevels[this.currentSpeedLevel];
	}

	// Token: 0x06000190 RID: 400 RVA: 0x00009690 File Offset: 0x00007890
	public void Stop()
	{
		this.behaviorStateMachine.CurrentNavigationState.priority = 0;
		this.behaviorStateMachine.ChangeNavigationState(new NavigationState_DoNothing(this, 127, true));
		this.currentSpeedLevel++;
		this.navigator.maxSpeed = 0f;
	}

	// Token: 0x06000191 RID: 401 RVA: 0x000096E0 File Offset: 0x000078E0
	public void Blind()
	{
		this.FreezeNPCs(false);
		this.navigator.maxSpeed = 0f;
		this.ec.AddFog(this.fog);
		this.sprite.enabled = false;
		this.ogLayer = base.gameObject.layer;
		base.gameObject.layer = 20;
		this.animator.Play("Reset", -1, 0f);
		this.blindAudMan.QueueAudio(this.audBlindStart);
		this.blindAudMan.QueueAudio(this.audBlindLoop);
		this.blindAudMan.SetLoop(true);
		this.behaviorStateMachine.ChangeState(new LookAtGuy_Blinding(this, this.fogTime));
	}

	// Token: 0x06000192 RID: 402 RVA: 0x0000979C File Offset: 0x0000799C
	public void Respawn()
	{
		base.transform.position = this.ec.mainHall.TileAtIndex(Random.Range(0, this.ec.mainHall.TileCount)).FloorWorldPosition + Vector3.up * 5f;
		this.animator.Play("Reset", -1, 0f);
		this.currentSpeedLevel = 2;
		this.ec.RemoveFog(this.fog);
		this.sprite.enabled = true;
		this.blindAudMan.FlushQueue(true);
		base.gameObject.layer = this.ogLayer;
		this.behaviorStateMachine.ChangeState(new LookAtGuy_Inactive(this));
	}

	// Token: 0x06000193 RID: 403 RVA: 0x0000985C File Offset: 0x00007A5C
	public void FreezeNPCs(bool freeze)
	{
		if (freeze)
		{
			if (!this.freezing)
			{
				this.ec.AddTimeScale(this.timeScale);
				this.freezing = true;
				this.audMan.SetLoop(true);
				this.audMan.maintainLoop = true;
				this.audMan.QueueAudio(this.audLoop);
				this.rumbleAudMan.PlaySingle(this.audSighted);
				this.ec.FlickerLights(true);
				return;
			}
		}
		else if (this.freezing)
		{
			this.ec.RemoveTimeScale(this.timeScale);
			this.freezing = false;
			this.audMan.FlushQueue(true);
			this.ec.FlickerLights(false);
		}
	}

	// Token: 0x04000195 RID: 405
	[SerializeField]
	private float moveSpeed;

	// Token: 0x04000196 RID: 406
	[SerializeField]
	private SpriteRenderer sprite;

	// Token: 0x04000197 RID: 407
	[SerializeField]
	private Animator animator;

	// Token: 0x04000198 RID: 408
	[SerializeField]
	private AudioManager audMan;

	// Token: 0x04000199 RID: 409
	[SerializeField]
	private AudioManager blindAudMan;

	// Token: 0x0400019A RID: 410
	[SerializeField]
	private AudioManager rumbleAudMan;

	// Token: 0x0400019B RID: 411
	[SerializeField]
	private SoundObject audActivate;

	// Token: 0x0400019C RID: 412
	[SerializeField]
	private SoundObject audSighted;

	// Token: 0x0400019D RID: 413
	[SerializeField]
	private SoundObject audLoop;

	// Token: 0x0400019E RID: 414
	[SerializeField]
	private SoundObject audBlindStart;

	// Token: 0x0400019F RID: 415
	[SerializeField]
	private SoundObject audBlindLoop;

	// Token: 0x040001A0 RID: 416
	public TimeScaleModifier timeScale = new TimeScaleModifier();

	// Token: 0x040001A1 RID: 417
	public Fog fog;

	// Token: 0x040001A2 RID: 418
	[SerializeField]
	private float[] speedLevels = new float[8];

	// Token: 0x040001A3 RID: 419
	[SerializeField]
	private float fogTime = 120f;

	// Token: 0x040001A4 RID: 420
	private int ogLayer;

	// Token: 0x040001A5 RID: 421
	private int currentSpeedLevel;

	// Token: 0x040001A6 RID: 422
	private bool freezing;
}
