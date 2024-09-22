using System;
using UnityEngine;

// Token: 0x0200005B RID: 91
public class Playtime : NPC
{
	// Token: 0x06000205 RID: 517 RVA: 0x0000B8FA File Offset: 0x00009AFA
	public override void Initialize()
	{
		base.Initialize();
		this.behaviorStateMachine.ChangeState(new Playtime_Wandering(this, this));
		this.navigator.maxSpeed = this.normSpeed;
		this.navigator.SetSpeed(this.normSpeed);
	}

	// Token: 0x06000206 RID: 518 RVA: 0x0000B936 File Offset: 0x00009B36
	protected override void VirtualUpdate()
	{
		base.VirtualUpdate();
	}

	// Token: 0x06000207 RID: 519 RVA: 0x0000B93E File Offset: 0x00009B3E
	public void PersuePlayer(PlayerManager player)
	{
		this.behaviorStateMachine.CurrentNavigationState.UpdatePosition(player.transform.position);
		this.navigator.maxSpeed = this.runSpeed;
		this.navigator.SetSpeed(this.runSpeed);
	}

	// Token: 0x06000208 RID: 520 RVA: 0x0000B980 File Offset: 0x00009B80
	public void StartPersuingPlayer(PlayerManager player)
	{
		if (!this.audMan.audioDevice.isPlaying)
		{
			this.behaviorStateMachine.ChangeNavigationState(new NavigationState_TargetPlayer(this, 63, player.transform.position));
			this.audMan.PlaySingle(this.audLetsPlay);
		}
	}

	// Token: 0x06000209 RID: 521 RVA: 0x0000B9D0 File Offset: 0x00009BD0
	public void PlayerTurnAround(PlayerManager player)
	{
		Directions.ReverseList(this.navigator.currentDirs);
		this.behaviorStateMachine.ChangeNavigationState(new NavigationState_WanderRandom(this, 0));
		this.navigator.maxSpeed = this.normSpeed;
		this.navigator.SetSpeed(this.normSpeed);
	}

	// Token: 0x0600020A RID: 522 RVA: 0x0000BA21 File Offset: 0x00009C21
	public void JumpropeHit()
	{
		this.audMan.PlaySingle(this.audOops);
	}

	// Token: 0x0600020B RID: 523 RVA: 0x0000BA34 File Offset: 0x00009C34
	public void Count(int jumps)
	{
		this.audMan.PlaySingle(this.audCount[jumps - 1]);
	}

	// Token: 0x0600020C RID: 524 RVA: 0x0000BA4C File Offset: 0x00009C4C
	public void EndJumprope(bool won)
	{
		this.navigator.maxSpeed = this.normSpeed;
		this.navigator.SetSpeed(this.normSpeed);
		if (won)
		{
			this.audMan.PlaySingle(this.audCongrats);
		}
		else
		{
			this.audMan.FlushQueue(true);
			this.audMan.PlaySingle(this.audSad);
			this.animator.Play("PLAY_Sad", -1, 0f);
		}
		this.currentJumprope.Destroy();
		this.behaviorStateMachine.ChangeState(new Playtime_Cooldown(this, this, this.initialCooldown));
	}

	// Token: 0x0600020D RID: 525 RVA: 0x0000BAE7 File Offset: 0x00009CE7
	public void EndCooldown()
	{
		this.behaviorStateMachine.ChangeState(new Playtime_Wandering(this, this));
		this.animator.Play("PLAY_Jump");
	}

	// Token: 0x0600020E RID: 526 RVA: 0x0000BB0B File Offset: 0x00009D0B
	public void CalloutChance()
	{
		if (Random.value <= this.calloutChance)
		{
			this.RandomCallout();
		}
	}

	// Token: 0x0600020F RID: 527 RVA: 0x0000BB20 File Offset: 0x00009D20
	private void RandomCallout()
	{
		int num = Random.Range(0, this.audCalls.Length);
		if (!this.audMan.audioDevice.isPlaying)
		{
			this.audMan.PlaySingle(this.audCalls[num]);
		}
	}

	// Token: 0x06000210 RID: 528 RVA: 0x0000BB64 File Offset: 0x00009D64
	public void StartJumprope(PlayerManager player)
	{
		if (this.currentJumprope != null)
		{
			this.currentJumprope.Destroy();
		}
		this.currentJumprope = Object.Instantiate<Jumprope>(this.jumpropePre);
		this.currentJumprope.player = player;
		this.currentJumprope.playtime = this;
		this.navigator.maxSpeed = 0f;
		this.navigator.Entity.AddForce(new Force(base.transform.position - player.transform.position, 20f, -60f));
		this.audMan.PlaySingle(this.audGo);
		this.behaviorStateMachine.ChangeState(new Playtime_Playing(this, this));
	}

	// Token: 0x0400021C RID: 540
	[SerializeField]
	private Animator animator;

	// Token: 0x0400021D RID: 541
	[SerializeField]
	private AudioManager audMan;

	// Token: 0x0400021E RID: 542
	[SerializeField]
	private SoundObject[] audCount = new SoundObject[0];

	// Token: 0x0400021F RID: 543
	[SerializeField]
	private SoundObject[] audCalls = new SoundObject[0];

	// Token: 0x04000220 RID: 544
	[SerializeField]
	private SoundObject audLetsPlay;

	// Token: 0x04000221 RID: 545
	[SerializeField]
	private SoundObject audGo;

	// Token: 0x04000222 RID: 546
	[SerializeField]
	private SoundObject audCongrats;

	// Token: 0x04000223 RID: 547
	[SerializeField]
	private SoundObject audOops;

	// Token: 0x04000224 RID: 548
	[SerializeField]
	private SoundObject audSad;

	// Token: 0x04000225 RID: 549
	[SerializeField]
	private Jumprope jumpropePre;

	// Token: 0x04000226 RID: 550
	private Jumprope currentJumprope;

	// Token: 0x04000227 RID: 551
	[SerializeField]
	private float normSpeed = 12f;

	// Token: 0x04000228 RID: 552
	[SerializeField]
	private float runSpeed = 20f;

	// Token: 0x04000229 RID: 553
	[SerializeField]
	private float initialCooldown = 15f;

	// Token: 0x0400022A RID: 554
	[SerializeField]
	private float calloutChance = 0.05f;
}
