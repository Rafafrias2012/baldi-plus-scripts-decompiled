using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000033 RID: 51
public class DrReflex : NPC
{
	// Token: 0x06000124 RID: 292 RVA: 0x00007B52 File Offset: 0x00005D52
	public override void Initialize()
	{
		base.Initialize();
		this.home = base.transform.position;
		this.behaviorStateMachine.ChangeState(new DrReflex_Wandering(this, 0f));
	}

	// Token: 0x06000125 RID: 293 RVA: 0x00007B84 File Offset: 0x00005D84
	protected override void VirtualUpdate()
	{
		base.VirtualUpdate();
		if (this.navigator.Speed > 0f && this.navigator.Am.Multiplier > 0f)
		{
			this.animator.SetBool("Walking", true);
			return;
		}
		this.animator.SetBool("Walking", false);
	}

	// Token: 0x06000126 RID: 294 RVA: 0x00007BE4 File Offset: 0x00005DE4
	public void Reset()
	{
		this.testComplete = false;
		this.navigator.SetSpeed(this.wanderSpeed);
		this.navigator.maxSpeed = this.wanderSpeed;
		this.navigator.SetRoomAvoidance(true);
		this.audioManager.pitchModifier = 1f;
		this.currentPitchValue = -1.5f;
		this.ResetAnimationTriggers();
		this.animator.SetTrigger("StartWalking");
	}

	// Token: 0x06000127 RID: 295 RVA: 0x00007C58 File Offset: 0x00005E58
	public void ResetAnimationTriggers()
	{
		foreach (AnimatorControllerParameter animatorControllerParameter in this.animator.parameters)
		{
			if (animatorControllerParameter.type == AnimatorControllerParameterType.Trigger)
			{
				this.animator.ResetTrigger(animatorControllerParameter.name);
			}
		}
	}

	// Token: 0x06000128 RID: 296 RVA: 0x00007CA0 File Offset: 0x00005EA0
	public void UpdateRoomWandering()
	{
		Random random = new Random();
		this.navigator.SetRoomAvoidance(random.NextDouble() >= (double)this.roomWanderChance);
	}

	// Token: 0x06000129 RID: 297 RVA: 0x00007CD0 File Offset: 0x00005ED0
	public bool FacingNextPoint()
	{
		if (this.turning)
		{
			return true;
		}
		if ((double)Vector3.Angle(base.transform.forward, this.navigator.NextPoint - base.transform.position) <= 22.5)
		{
			Vector3 lhs = this.navigator.NextPoint - base.transform.position;
			if (lhs != Vector3.zero)
			{
				base.transform.rotation = Quaternion.LookRotation(lhs.normalized, Vector3.up);
			}
			return true;
		}
		return false;
	}

	// Token: 0x0600012A RID: 298 RVA: 0x00007D66 File Offset: 0x00005F66
	public bool FacingPlayer(PlayerManager player)
	{
		return (double)Vector3.Angle(base.transform.forward, player.transform.position - base.transform.position) <= 22.5;
	}

	// Token: 0x0600012B RID: 299 RVA: 0x00007DA2 File Offset: 0x00005FA2
	public void PauseAndTurn(PlayerManager player)
	{
		if (!this.turning)
		{
			this.turning = true;
			this.navigator.ClearRemainingDistanceThisFrame();
			base.StartCoroutine(this.PauseAndTurner(player));
		}
	}

	// Token: 0x0600012C RID: 300 RVA: 0x00007DCC File Offset: 0x00005FCC
	public void PauseAndTurn()
	{
		this.PauseAndTurn(null);
	}

	// Token: 0x0600012D RID: 301 RVA: 0x00007DD5 File Offset: 0x00005FD5
	private IEnumerator PauseAndTurner(PlayerManager player)
	{
		Vector3 target = this.navigator.NextPoint;
		if (player != null)
		{
			target = player.transform.position;
		}
		this.navigator.Am.moveMods.Add(this.moveMod);
		float time = 1f;
		while (time > 0f)
		{
			time -= Time.deltaTime * base.TimeScale;
			yield return null;
		}
		time = 1f;
		Vector3 vector = default(Vector3);
		while (time > 0f || (double)Vector3.Angle(base.transform.forward, target - base.transform.position) > 22.5)
		{
			if (player != null)
			{
				target = player.transform.position;
			}
			vector = Vector3.RotateTowards(base.transform.forward, (target - base.transform.position).normalized, Time.deltaTime * 2f * 3.1415927f * this.turnSpeed, 0f);
			if (vector != Vector3.zero)
			{
				base.transform.rotation = Quaternion.LookRotation(vector, Vector3.up);
			}
			time -= Time.deltaTime;
			yield return null;
		}
		base.transform.rotation = Quaternion.LookRotation((target - base.transform.position).normalized, Vector3.up);
		this.turning = false;
		this.navigator.Am.moveMods.Remove(this.moveMod);
		yield break;
	}

	// Token: 0x0600012E RID: 302 RVA: 0x00007DEC File Offset: 0x00005FEC
	public void RotateTowardsNextPoint()
	{
		if (!this.turning)
		{
			Vector3 vector = default(Vector3);
			vector = Vector3.RotateTowards(base.transform.forward, (this.navigator.NextPoint - base.transform.position).normalized, Time.deltaTime * 2f * 3.1415927f * this.turnSpeed, 0f);
			if (vector != Vector3.zero)
			{
				base.transform.rotation = Quaternion.LookRotation(vector, Vector3.up);
			}
		}
	}

	// Token: 0x0600012F RID: 303 RVA: 0x00007E80 File Offset: 0x00006080
	public void RotateTowardsPlayer(PlayerManager player)
	{
		if (!this.turning)
		{
			Vector3 vector = default(Vector3);
			vector = Vector3.RotateTowards(base.transform.forward, (player.transform.position - base.transform.position).normalized, Time.deltaTime * 2f * 3.1415927f * this.turnSpeed, 0f);
			if (vector != Vector3.zero)
			{
				base.transform.rotation = Quaternion.LookRotation(vector, Vector3.up);
			}
		}
	}

	// Token: 0x06000130 RID: 304 RVA: 0x00007F14 File Offset: 0x00006114
	public void FaceEntity(Entity entity)
	{
		Vector3 vector = (entity.transform.position - base.transform.position).normalized.ZeroOutY();
		if (vector != Vector3.zero)
		{
			base.transform.rotation = Quaternion.LookRotation(vector, Vector3.up);
		}
	}

	// Token: 0x06000131 RID: 305 RVA: 0x00007F70 File Offset: 0x00006170
	public void StartCharge()
	{
		this.navigator.SetSpeed(this.chargeSpeed);
		this.navigator.maxSpeed = this.chargeSpeed;
		this.audioManager.QueueAudio(this.audThinkIntro);
		this.audioManager.QueueAudio(this.audThinkLoop);
		this.audioManager.SetLoop(true);
		this.animator.SetTrigger("StartChasing");
	}

	// Token: 0x06000132 RID: 306 RVA: 0x00007FDD File Offset: 0x000061DD
	public void IncreasePitch()
	{
		this.currentPitchValue += Time.deltaTime;
		this.audioManager.pitchModifier = Mathf.Log((this.currentPitchValue + this.pitchIncreaseSpeedModifier) / this.pitchIncreaseSpeedModifier * this.pitchIncreaseMaxModifier);
	}

	// Token: 0x06000133 RID: 307 RVA: 0x0000801C File Offset: 0x0000621C
	public void StartTest(PlayerManager player)
	{
		this.navigator.SetSpeed(0f);
		this.navigator.maxSpeed = 0f;
		this.leftHotSpot.SetActive(false);
		this.rightHotSpot.SetActive(false);
		this.currentYtps = this.initialYtps;
		this.ResetTest();
		base.StartCoroutine(this.TurnPlayer(player, 1f));
		this.testCoroutine = this.TestSequence();
		base.StartCoroutine(this.testCoroutine);
		this.audioManager.SetLoop(false);
		this.audioManager.QueueAudio(this.audThinkEnd);
	}

	// Token: 0x06000134 RID: 308 RVA: 0x000080BC File Offset: 0x000062BC
	public void ResetTest()
	{
		this.leftHotSpot.SetActive(false);
		this.rightHotSpot.SetActive(false);
		if (Random.Range(0, 2) == 0)
		{
			this.correctSide = Direction.East;
			return;
		}
		this.correctSide = Direction.West;
	}

	// Token: 0x06000135 RID: 309 RVA: 0x000080EE File Offset: 0x000062EE
	private IEnumerator TestSequence()
	{
		float minTestTime;
		float maxTestTime;
		float totalResponseTime;
		if (this.level >= 4)
		{
			minTestTime = this.minTestTimeLvl4;
			maxTestTime = this.minTestTimeLvl4;
			totalResponseTime = this.responseTimeLvl4;
		}
		else if (this.level > 0)
		{
			minTestTime = this.minTestTimeLvl1;
			maxTestTime = this.minTestTimeLvl1;
			totalResponseTime = this.responseTimeLvl1;
		}
		else
		{
			minTestTime = this.minTestTimeLvl0;
			maxTestTime = this.minTestTimeLvl0;
			totalResponseTime = this.responseTimeLvl0;
		}
		float waitTime = Random.Range(minTestTime, maxTestTime) + this.initialDelay;
		for (;;)
		{
			if (!this.audioManager.QueuedAudioIsPlaying)
			{
				while (waitTime > 0f)
				{
					waitTime -= Time.deltaTime * base.TimeScale;
					yield return null;
				}
				this.leftHotSpot.SetActive(true);
				this.rightHotSpot.SetActive(true);
				this.animator.ResetTrigger("CloseHands");
				if (this.correctSide == Direction.East)
				{
					this.animator.SetTrigger("OpenHandsRight");
				}
				else
				{
					this.animator.SetTrigger("OpenHandsLeft");
				}
				this.audioManager.FlushQueue(true);
				this.audioManager.QueueAudio(this.audFast);
				float responseTime = totalResponseTime;
				while (responseTime > 0f)
				{
					responseTime -= Time.deltaTime * base.TimeScale;
					yield return null;
				}
				this.audioManager.pitchModifier = 1f;
				this.currentYtps -= this.ytpPenalty;
				this.ResetTest();
				waitTime = Random.Range(minTestTime, maxTestTime);
				this.animator.SetTrigger("CloseHands");
				if (!this.audioManager.QueuedUp)
				{
					this.audioManager.QueueRandomAudio(this.audIncorrect);
					this.audioManager.QueueRandomAudio(this.audTooSlow);
					this.audioManager.QueueAudio(this.audTryAgain);
				}
			}
			else
			{
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x06000136 RID: 310 RVA: 0x000080FD File Offset: 0x000062FD
	public override void SetSpriteRotation(float degrees)
	{
		base.SetSpriteRotation(degrees);
		if (Mathf.Abs(degrees) > 90f)
		{
			this.reverseTest = true;
			return;
		}
		this.reverseTest = false;
	}

	// Token: 0x06000137 RID: 311 RVA: 0x00008124 File Offset: 0x00006324
	public void HotspotClicked(Direction side)
	{
		if ((side == this.correctSide && !this.reverseTest) || (side != this.correctSide && this.reverseTest))
		{
			this.WinTest();
		}
		else
		{
			this.LoseTest();
		}
		this.audioManager.PlaySingle(this.audHit);
	}

	// Token: 0x06000138 RID: 312 RVA: 0x00008174 File Offset: 0x00006374
	private void WinTest()
	{
		this.EndTest(true, null);
		base.StartCoroutine(this.ResetDelay(2f, this.cooldown));
		this.audioManager.FlushQueue(true);
		if (this.currentYtps == this.initialYtps && Random.Range(0, 4) == 0)
		{
			this.audioManager.QueueAudio(this.audFirstTry);
		}
		else
		{
			this.audioManager.QueueRandomAudio(this.audCorrect);
		}
		this.audioManager.QueueRandomAudio(this.audResult);
		this.animator.SetTrigger("CloseHands");
	}

	// Token: 0x06000139 RID: 313 RVA: 0x00008209 File Offset: 0x00006409
	private IEnumerator ResetDelay(float delay, float cooldown)
	{
		while (delay > 0f)
		{
			delay -= Time.deltaTime * base.TimeScale;
			yield return null;
		}
		this.behaviorStateMachine.ChangeState(new DrReflex_Wandering(this, cooldown));
		yield break;
	}

	// Token: 0x0600013A RID: 314 RVA: 0x00008228 File Offset: 0x00006428
	private void LoseTest()
	{
		this.audioManager.pitchModifier = 1f;
		this.ResetTest();
		this.currentYtps -= this.ytpPenalty;
		this.audioManager.FlushQueue(true);
		this.audioManager.QueueRandomAudio(this.audIncorrect);
		this.audioManager.QueueRandomAudio(this.audHitCircle);
		this.audioManager.QueueAudio(this.audTryAgain);
		this.animator.SetTrigger("CloseHands");
	}

	// Token: 0x0600013B RID: 315 RVA: 0x000082B0 File Offset: 0x000064B0
	public void EndTest(bool success, PlayerManager player)
	{
		this.leftHotSpot.SetActive(false);
		this.rightHotSpot.SetActive(false);
		this.leftHighlight.SetActive(false);
		this.rightHighlight.SetActive(false);
		base.StopCoroutine(this.testCoroutine);
		this.testComplete = true;
		if (success)
		{
			Singleton<CoreGameManager>.Instance.AddPoints(Mathf.Max(this.currentYtps, 0), 0, this.currentYtps > 0);
			this.animator.SetTrigger("CloseHands");
		}
		else
		{
			base.StartCoroutine(this.LoseDelay(2f, player));
			this.animator.SetTrigger("Failed");
			this.audioManager.FlushQueue(true);
			this.audioManager.QueueAudio(this.audSession);
		}
		this.level++;
		this.audioManager.pitchModifier = 1f;
	}

	// Token: 0x0600013C RID: 316 RVA: 0x00008393 File Offset: 0x00006593
	private IEnumerator LoseDelay(float time, PlayerManager player)
	{
		while (time > 0f)
		{
			time -= Time.deltaTime * base.TimeScale;
			yield return null;
		}
		this.behaviorStateMachine.ChangeState(new DrReflex_Angry(this, player));
		yield break;
	}

	// Token: 0x0600013D RID: 317 RVA: 0x000083B0 File Offset: 0x000065B0
	private IEnumerator TurnPlayer(PlayerManager player, float speed)
	{
		float time = 0.5f;
		Vector3 vector = default(Vector3);
		player.plm.am.moveMods.Add(this.moveMod);
		while (time > 0f)
		{
			vector = Vector3.RotateTowards(player.transform.forward.ZeroOutY(), (base.transform.position.ZeroOutY() - player.transform.position.ZeroOutY()).normalized, Time.deltaTime * 2f * 3.1415927f * speed, 0f);
			Debug.DrawRay(player.transform.position, vector, Color.yellow);
			player.transform.rotation = Quaternion.LookRotation(vector, Vector3.up);
			time -= Time.deltaTime;
			yield return null;
		}
		player.plm.am.moveMods.Remove(this.moveMod);
		yield break;
	}

	// Token: 0x0600013E RID: 318 RVA: 0x000083CD File Offset: 0x000065CD
	public void HeadToOffice()
	{
		this.behaviorStateMachine.ChangeNavigationState(new NavigationState_TargetPosition(this, 63, this.home));
		this.navigator.SetSpeed(this.huntSpeed);
		this.navigator.maxSpeed = this.huntSpeed;
	}

	// Token: 0x0600013F RID: 319 RVA: 0x0000840A File Offset: 0x0000660A
	public void GetHammer()
	{
		this.animator.SetTrigger("HammerAquired");
	}

	// Token: 0x06000140 RID: 320 RVA: 0x0000841C File Offset: 0x0000661C
	public void HammerCheck(PlayerManager targetPlayer)
	{
		int num = Physics.OverlapSphereNonAlloc(base.transform.position + base.transform.forward * 5f, 4f, this.hammerColliders, this.hammerMask, QueryTriggerInteraction.Collide);
		for (int i = 0; i < num; i++)
		{
			if (this.hammerColliders[i].isTrigger && this.hammerColliders[i].transform != base.transform)
			{
				Entity component = this.hammerColliders[i].GetComponent<Entity>();
				if (component != null && !component.Squished && !this.navigator.Entity.IsIgnoring(component))
				{
					bool flag;
					this.looker.Raycast(this.hammerColliders[i].transform, Vector3.Magnitude(base.transform.position - this.hammerColliders[i].transform.position), this.hammerRaycastMask, out flag);
					if (flag)
					{
						this.animator.SetBool("RapidSwing", true);
						this.Hammer(component);
						base.StartCoroutine(this.RapidHammer(component, this.hammerColliders[i].transform == targetPlayer.transform));
						if (this.hammerColliders[i].transform == targetPlayer.transform)
						{
							base.StartCoroutine(this.TurnPlayer(targetPlayer, 3f));
						}
					}
				}
			}
		}
	}

	// Token: 0x06000141 RID: 321 RVA: 0x0000859C File Offset: 0x0000679C
	public void Hammer(Entity entity)
	{
		this.animator.SetTrigger("Swing");
		if (entity != null)
		{
			this.animator.SetBool("Chill", false);
			entity.Squish(this.squishTime);
		}
		float num = Vector3.Distance(base.transform.position, Singleton<CoreGameManager>.Instance.GetPlayer(0).transform.position);
		if (num < 200f)
		{
			float signedAngle = Vector3.SignedAngle(base.transform.position - Singleton<CoreGameManager>.Instance.GetPlayer(0).transform.position, Singleton<CoreGameManager>.Instance.GetPlayer(0).transform.forward, Vector3.up);
			Singleton<InputManager>.Instance.Rumble(1f - num / 200f, 0.5f, signedAngle);
		}
	}

	// Token: 0x06000142 RID: 322 RVA: 0x0000866F File Offset: 0x0000686F
	public void HammerSound()
	{
		this.audioManager.PlaySingle(this.audHammer);
	}

	// Token: 0x06000143 RID: 323 RVA: 0x00008682 File Offset: 0x00006882
	private IEnumerator RapidHammer(Entity entity, bool final)
	{
		float time = Random.Range(0.5f, 1.5f);
		this.navigator.Am.moveMods.Add(this.moveMod);
		entity.ExternalActivity.moveMods.Add(this.moveMod);
		while (time > 0f)
		{
			time -= Time.deltaTime;
			this.animator.SetBool("RapidSwing", true);
			this.FaceEntity(entity);
			yield return null;
		}
		this.animator.SetBool("RapidSwing", false);
		entity.ExternalActivity.moveMods.Remove(this.moveMod);
		if (final)
		{
			this.animator.SetBool("Chill", true);
			float delay = 1f;
			while (delay > 0f)
			{
				delay -= Time.deltaTime;
				this.FaceEntity(entity);
				yield return null;
			}
			this.animator.SetBool("Chill", false);
			this.navigator.Am.moveMods.Remove(this.moveMod);
			this.behaviorStateMachine.ChangeState(new DrReflex_Wandering(this, this.cooldown + this.squishTime));
			this.audioManager.QueueAudio(this.audNotBad);
		}
		else
		{
			this.navigator.Am.moveMods.Remove(this.moveMod);
		}
		yield break;
	}

	// Token: 0x06000144 RID: 324 RVA: 0x0000869F File Offset: 0x0000689F
	public override void PlayerInSight(PlayerManager player)
	{
		base.PlayerInSight(player);
		this.lastSightedPlayerLocation = player.transform.position;
	}

	// Token: 0x06000145 RID: 325 RVA: 0x000086B9 File Offset: 0x000068B9
	public override void Hear(Vector3 position, int value)
	{
		base.Hear(position, value);
		if (!this.looker.PlayerInSight())
		{
			this.lastSightedPlayerLocation = position;
		}
	}

	// Token: 0x06000146 RID: 326 RVA: 0x000086D7 File Offset: 0x000068D7
	public bool CalloutChance(bool happy)
	{
		if (Random.value <= this.calloutChance)
		{
			this.RandomCallout(happy);
			return true;
		}
		return false;
	}

	// Token: 0x06000147 RID: 327 RVA: 0x000086F0 File Offset: 0x000068F0
	private void RandomCallout(bool happy)
	{
		if (happy)
		{
			if (!this.audioManager.QueuedAudioIsPlaying)
			{
				this.audioManager.PlayRandomAudio(this.audHappy);
				return;
			}
		}
		else
		{
			this.audioManager.PlayRandomAudio(this.audAngry);
			this.animator.SetBool("Chill", true);
		}
	}

	// Token: 0x17000017 RID: 23
	// (get) Token: 0x06000148 RID: 328 RVA: 0x00008741 File Offset: 0x00006941
	public float RoomWanderCycle
	{
		get
		{
			return this.roomWanderCycle;
		}
	}

	// Token: 0x17000018 RID: 24
	// (get) Token: 0x06000149 RID: 329 RVA: 0x00008749 File Offset: 0x00006949
	public AudioManager AudioManager
	{
		get
		{
			return this.audioManager;
		}
	}

	// Token: 0x17000019 RID: 25
	// (get) Token: 0x0600014A RID: 330 RVA: 0x00008751 File Offset: 0x00006951
	public Animator Animator
	{
		get
		{
			return this.animator;
		}
	}

	// Token: 0x1700001A RID: 26
	// (get) Token: 0x0600014B RID: 331 RVA: 0x00008759 File Offset: 0x00006959
	public Vector3 LastSightedPlayerLocation
	{
		get
		{
			return this.lastSightedPlayerLocation;
		}
	}

	// Token: 0x0600014C RID: 332 RVA: 0x00008761 File Offset: 0x00006961
	public bool PlayerLeft(PlayerManager player)
	{
		return Vector3.Distance(player.transform.position, base.transform.position) > this.maxTestDistance && !this.testComplete;
	}

	// Token: 0x1700001B RID: 27
	// (get) Token: 0x0600014D RID: 333 RVA: 0x00008791 File Offset: 0x00006991
	public bool Turning
	{
		get
		{
			return this.turning;
		}
	}

	// Token: 0x04000136 RID: 310
	private Vector3 home;

	// Token: 0x04000137 RID: 311
	[SerializeField]
	private AudioManager audioManager;

	// Token: 0x04000138 RID: 312
	[SerializeField]
	private SoundObject[] audCorrect;

	// Token: 0x04000139 RID: 313
	[SerializeField]
	private SoundObject[] audIncorrect;

	// Token: 0x0400013A RID: 314
	[SerializeField]
	private SoundObject[] audHappy;

	// Token: 0x0400013B RID: 315
	[SerializeField]
	private SoundObject[] audAngry;

	// Token: 0x0400013C RID: 316
	[SerializeField]
	private SoundObject[] audResult;

	// Token: 0x0400013D RID: 317
	[SerializeField]
	private SoundObject[] audTooSlow;

	// Token: 0x0400013E RID: 318
	[SerializeField]
	private SoundObject[] audHitCircle;

	// Token: 0x0400013F RID: 319
	[SerializeField]
	private SoundObject audThinkIntro;

	// Token: 0x04000140 RID: 320
	[SerializeField]
	private SoundObject audThinkLoop;

	// Token: 0x04000141 RID: 321
	[SerializeField]
	private SoundObject audThinkEnd;

	// Token: 0x04000142 RID: 322
	[SerializeField]
	private SoundObject audHammer;

	// Token: 0x04000143 RID: 323
	[SerializeField]
	private SoundObject audFast;

	// Token: 0x04000144 RID: 324
	[SerializeField]
	private SoundObject audFirstTry;

	// Token: 0x04000145 RID: 325
	[SerializeField]
	private SoundObject audSession;

	// Token: 0x04000146 RID: 326
	[SerializeField]
	private SoundObject audTryAgain;

	// Token: 0x04000147 RID: 327
	[SerializeField]
	private SoundObject audNotBad;

	// Token: 0x04000148 RID: 328
	[SerializeField]
	private SoundObject audHit;

	// Token: 0x04000149 RID: 329
	private Collider[] hammerColliders = new Collider[16];

	// Token: 0x0400014A RID: 330
	[SerializeField]
	private GameObject leftHotSpot;

	// Token: 0x0400014B RID: 331
	[SerializeField]
	private GameObject rightHotSpot;

	// Token: 0x0400014C RID: 332
	[SerializeField]
	private GameObject leftHighlight;

	// Token: 0x0400014D RID: 333
	[SerializeField]
	private GameObject rightHighlight;

	// Token: 0x0400014E RID: 334
	[SerializeField]
	private LayerMask hammerMask = 0;

	// Token: 0x0400014F RID: 335
	[SerializeField]
	private LayerMask hammerRaycastMask = 0;

	// Token: 0x04000150 RID: 336
	[SerializeField]
	private Animator animator;

	// Token: 0x04000151 RID: 337
	private IEnumerator testCoroutine;

	// Token: 0x04000152 RID: 338
	private Direction correctSide = Direction.East;

	// Token: 0x04000153 RID: 339
	private MovementModifier moveMod = new MovementModifier(Vector3.zero, 0f);

	// Token: 0x04000154 RID: 340
	private Vector3 lastSightedPlayerLocation;

	// Token: 0x04000155 RID: 341
	[SerializeField]
	private float wanderSpeed = 16f;

	// Token: 0x04000156 RID: 342
	[SerializeField]
	private float chargeSpeed = 30f;

	// Token: 0x04000157 RID: 343
	[SerializeField]
	private float huntSpeed = 26f;

	// Token: 0x04000158 RID: 344
	[SerializeField]
	private float turnSpeed = 1f;

	// Token: 0x04000159 RID: 345
	[SerializeField]
	private float maxTestDistance = 20f;

	// Token: 0x0400015A RID: 346
	[SerializeField]
	private float cooldown = 30f;

	// Token: 0x0400015B RID: 347
	[SerializeField]
	private float squishTime = 30f;

	// Token: 0x0400015C RID: 348
	[SerializeField]
	private float pitchIncreaseMaxModifier = 4f;

	// Token: 0x0400015D RID: 349
	[SerializeField]
	private float pitchIncreaseSpeedModifier = 10f;

	// Token: 0x0400015E RID: 350
	[SerializeField]
	private float roomWanderCycle = 5f;

	// Token: 0x0400015F RID: 351
	[SerializeField]
	private float roomWanderChance = 0.1f;

	// Token: 0x04000160 RID: 352
	[SerializeField]
	private float calloutChance = 0.05f;

	// Token: 0x04000161 RID: 353
	[SerializeField]
	private float initialDelay = 2f;

	// Token: 0x04000162 RID: 354
	[SerializeField]
	private float minTestTimeLvl0 = 0.5f;

	// Token: 0x04000163 RID: 355
	[SerializeField]
	private float maxTestTimeLvl0 = 3f;

	// Token: 0x04000164 RID: 356
	[SerializeField]
	private float responseTimeLvl0 = 1f;

	// Token: 0x04000165 RID: 357
	[SerializeField]
	private float minTestTimeLvl1 = 2f;

	// Token: 0x04000166 RID: 358
	[SerializeField]
	private float maxTestTimeLvl1 = 5f;

	// Token: 0x04000167 RID: 359
	[SerializeField]
	private float responseTimeLvl1 = 0.75f;

	// Token: 0x04000168 RID: 360
	[SerializeField]
	private float minTestTimeLvl4 = 3f;

	// Token: 0x04000169 RID: 361
	[SerializeField]
	private float maxTestTimeLvl4 = 8f;

	// Token: 0x0400016A RID: 362
	[SerializeField]
	private float responseTimeLvl4 = 0.5f;

	// Token: 0x0400016B RID: 363
	private float currentPitchValue;

	// Token: 0x0400016C RID: 364
	[SerializeField]
	private int initialYtps = 25;

	// Token: 0x0400016D RID: 365
	[SerializeField]
	private int ytpPenalty = 5;

	// Token: 0x0400016E RID: 366
	private int level;

	// Token: 0x0400016F RID: 367
	private int currentYtps;

	// Token: 0x04000170 RID: 368
	private bool turning;

	// Token: 0x04000171 RID: 369
	private bool reverseTest;

	// Token: 0x04000172 RID: 370
	private bool testComplete;
}
