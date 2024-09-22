using System;
using UnityEngine;

// Token: 0x0200001B RID: 27
public class Beans : NPC
{
	// Token: 0x060000A7 RID: 167 RVA: 0x0000545C File Offset: 0x0000365C
	public override void Initialize()
	{
		base.Initialize();
		this.behaviorStateMachine.ChangeState(new Beans_NewWandering(this));
		this.speed = this.navigator.maxSpeed;
		this.gum = Object.Instantiate<Gum>(this.gumPre);
		this.gum.Initialize(this.ec, this);
		this.gumFleeMap = new DijkstraMap(this.ec, PathType.Const, new Transform[]
		{
			this.gum.transform
		});
	}

	// Token: 0x060000A8 RID: 168 RVA: 0x000054DA File Offset: 0x000036DA
	protected override void VirtualUpdate()
	{
		base.VirtualUpdate();
	}

	// Token: 0x1700000D RID: 13
	// (get) Token: 0x060000A9 RID: 169 RVA: 0x000054E2 File Offset: 0x000036E2
	public float SprintWait
	{
		get
		{
			return Random.Range(this.minSprintWait, this.maxSprintWait);
		}
	}

	// Token: 0x1700000E RID: 14
	// (get) Token: 0x060000AA RID: 170 RVA: 0x000054F5 File Offset: 0x000036F5
	public float SprintTime
	{
		get
		{
			return Random.Range(this.minSprint, this.maxSprint);
		}
	}

	// Token: 0x060000AB RID: 171 RVA: 0x00005508 File Offset: 0x00003708
	public void Sprint()
	{
		this.animator.SetBool("sprinting", true);
		this.navigator.SetSpeed(this.sprintSpeed);
		this.navigator.maxSpeed = this.sprintSpeed;
		this.animator.Play("Beans_Sprint", -1, 0f);
		if (!this.audMan.QueuedAudioIsPlaying && Random.value <= this.sprintAudChance)
		{
			this.audMan.QueueAudio(this.audSkipSounds[Random.Range(0, this.audSkipSounds.Length)]);
		}
	}

	// Token: 0x060000AC RID: 172 RVA: 0x00005598 File Offset: 0x00003798
	public void StopSprint()
	{
		this.animator.SetBool("sprinting", false);
		this.navigator.SetSpeed(this.normSpeed);
		this.navigator.maxSpeed = this.normSpeed;
		this.animator.Play("Beans_Idle", -1, 0f);
	}

	// Token: 0x060000AD RID: 173 RVA: 0x000055EE File Offset: 0x000037EE
	public void Stop()
	{
		this.animator.SetBool("sprinting", false);
		this.animator.Play("Beans_Idle", -1, 0f);
	}

	// Token: 0x060000AE RID: 174 RVA: 0x00005618 File Offset: 0x00003818
	public void ChewPrep(PlayerManager player)
	{
		this.navigator.FindPath(base.transform.position, player.transform.position);
		this.behaviorStateMachine.ChangeState(new Beans_ChewPrep(this, player));
		this.audMan.FlushQueue(true);
		this.audMan.QueueAudio(this.audTargetSounds[Random.Range(0, this.audTargetSounds.Length)]);
	}

	// Token: 0x060000AF RID: 175 RVA: 0x00005684 File Offset: 0x00003884
	public void Chew()
	{
		this.animator.Play("Beans_Chew", -1, 0f);
	}

	// Token: 0x060000B0 RID: 176 RVA: 0x0000569C File Offset: 0x0000389C
	public void Blow()
	{
		this.audMan.PlaySingle(this.audBlow);
	}

	// Token: 0x060000B1 RID: 177 RVA: 0x000056B0 File Offset: 0x000038B0
	public void Spit(PlayerManager player)
	{
		this.gum.transform.rotation = Directions.DirFromVector3(player.transform.position - base.transform.position, 45f).ToRotation();
		this.gum.transform.position = base.transform.position + this.gum.transform.forward * 4f;
		this.gum.Reset();
		this.animator.Play("Beans_Spit", -1, 0f);
		this.audMan.PlaySingle(this.audSpit);
		this.audMan.QueueAudio(this.audSpitSounds[Random.Range(0, this.audSpitSounds.Length)]);
	}

	// Token: 0x060000B2 RID: 178 RVA: 0x00005783 File Offset: 0x00003983
	public void HitPlayer()
	{
		this.audMan.QueueAudio(this.audPlayerHitSounds[Random.Range(0, this.audPlayerHitSounds.Length)]);
	}

	// Token: 0x060000B3 RID: 179 RVA: 0x000057A5 File Offset: 0x000039A5
	public void HitNPC()
	{
		this.audMan.QueueAudio(this.audNPCHitSounds[Random.Range(0, this.audNPCHitSounds.Length)]);
	}

	// Token: 0x060000B4 RID: 180 RVA: 0x000057C8 File Offset: 0x000039C8
	public void GumHit(Gum gum, bool hitSelf)
	{
		this.gumFleeMap.Activate();
		this.gumFleeMap.QueueUpdate();
		if (!hitSelf)
		{
			this.behaviorStateMachine.ChangeState(new Beans_Flee(this, this.cooldownTime, this.gumFleeMap));
			return;
		}
		this.behaviorStateMachine.ChangeState(new Beans_NewWandering(this));
	}

	// Token: 0x1700000F RID: 15
	// (get) Token: 0x060000B5 RID: 181 RVA: 0x0000581D File Offset: 0x00003A1D
	public float ChewTime
	{
		get
		{
			return this.chewTime;
		}
	}

	// Token: 0x17000010 RID: 16
	// (get) Token: 0x060000B6 RID: 182 RVA: 0x00005825 File Offset: 0x00003A25
	public float CooldownTime
	{
		get
		{
			return this.cooldownTime;
		}
	}

	// Token: 0x040000B9 RID: 185
	[SerializeField]
	private Gum gumPre;

	// Token: 0x040000BA RID: 186
	public Gum gum;

	// Token: 0x040000BB RID: 187
	[SerializeField]
	private Animator animator;

	// Token: 0x040000BC RID: 188
	[SerializeField]
	private AudioManager audMan;

	// Token: 0x040000BD RID: 189
	[SerializeField]
	private SoundObject[] audSkipSounds = new SoundObject[0];

	// Token: 0x040000BE RID: 190
	[SerializeField]
	private SoundObject[] audTargetSounds = new SoundObject[0];

	// Token: 0x040000BF RID: 191
	[SerializeField]
	private SoundObject[] audSpitSounds = new SoundObject[0];

	// Token: 0x040000C0 RID: 192
	[SerializeField]
	private SoundObject[] audPlayerHitSounds = new SoundObject[0];

	// Token: 0x040000C1 RID: 193
	[SerializeField]
	private SoundObject[] audNPCHitSounds = new SoundObject[0];

	// Token: 0x040000C2 RID: 194
	[SerializeField]
	private SoundObject audSpit;

	// Token: 0x040000C3 RID: 195
	[SerializeField]
	private SoundObject audBlow;

	// Token: 0x040000C4 RID: 196
	[SerializeField]
	private Collider trigger;

	// Token: 0x040000C5 RID: 197
	private DijkstraMap gumFleeMap;

	// Token: 0x040000C6 RID: 198
	[SerializeField]
	private float cooldownTime = 15f;

	// Token: 0x040000C7 RID: 199
	[SerializeField]
	private float normSpeed = 5f;

	// Token: 0x040000C8 RID: 200
	[SerializeField]
	private float sprintSpeed = 25f;

	// Token: 0x040000C9 RID: 201
	[SerializeField]
	private float minSprint = 0.25f;

	// Token: 0x040000CA RID: 202
	[SerializeField]
	private float maxSprint = 2f;

	// Token: 0x040000CB RID: 203
	[SerializeField]
	private float minSprintWait = 2f;

	// Token: 0x040000CC RID: 204
	[SerializeField]
	private float maxSprintWait = 10f;

	// Token: 0x040000CD RID: 205
	[SerializeField]
	private float sprintAudChance = 0.25f;

	// Token: 0x040000CE RID: 206
	[SerializeField]
	private float chewTime = 3f;

	// Token: 0x040000CF RID: 207
	private float cooldown;

	// Token: 0x040000D0 RID: 208
	private float speed;
}
