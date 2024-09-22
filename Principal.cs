using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000060 RID: 96
public class Principal : NPC
{
	// Token: 0x06000221 RID: 545 RVA: 0x0000BE5B File Offset: 0x0000A05B
	public override void Initialize()
	{
		base.Initialize();
		this.timeInSight = new float[this.players.Count];
		this.defaultSpeed = this.navigator.maxSpeed;
		this.behaviorStateMachine.ChangeState(new Principal_Wandering(this));
	}

	// Token: 0x06000222 RID: 546 RVA: 0x0000BE9C File Offset: 0x0000A09C
	public void ObservePlayer(PlayerManager player)
	{
		if (player.Disobeying && !player.Tagged)
		{
			this.timeInSight[player.playerNumber] += Time.deltaTime * base.TimeScale;
			if (this.timeInSight[player.playerNumber] >= player.GuiltySensitivity)
			{
				if (!this.allKnowing)
				{
					this.behaviorStateMachine.ChangeState(new Principal_ChasingPlayer(this, player));
				}
				else
				{
					this.behaviorStateMachine.ChangeState(new Principal_ChasingPlayer_AllKnowing(this, player));
				}
				this.targetedPlayer = player;
				this.Scold(player.ruleBreak);
			}
		}
	}

	// Token: 0x06000223 RID: 547 RVA: 0x0000BF30 File Offset: 0x0000A130
	public void LoseTrackOfPlayer(PlayerManager player)
	{
		this.timeInSight[player.playerNumber] = 0f;
	}

	// Token: 0x06000224 RID: 548 RVA: 0x0000BF44 File Offset: 0x0000A144
	public void FacultyDoorHit(StandardDoor door, Cell otherSide)
	{
		if (this.lastKnockedDoor != door)
		{
			this.KnockOnDoor(door, otherSide);
		}
		else
		{
			door.OpenTimedWithKey(door.DefaultTime, false);
		}
		this.lastKnockedDoor = door;
	}

	// Token: 0x06000225 RID: 549 RVA: 0x0000BF74 File Offset: 0x0000A174
	public void KnockOnDoor(StandardDoor door, Cell otherSide)
	{
		door.Knock();
		NavigationState_DoNothing navigationState_DoNothing = new NavigationState_DoNothing(this, 0);
		this.navigationStateMachine.ChangeState(navigationState_DoNothing);
		navigationState_DoNothing.priority = -1;
		base.StopAllCoroutines();
		base.StartCoroutine(this.UnpauseAfterKnock(door, this.knockPauseTime, otherSide));
	}

	// Token: 0x06000226 RID: 550 RVA: 0x0000BFBD File Offset: 0x0000A1BD
	private IEnumerator UnpauseAfterKnock(StandardDoor door, float time, Cell otherSide)
	{
		while (time > 0f)
		{
			time -= Time.deltaTime * base.TimeScale;
			yield return null;
		}
		this.navigationStateMachine.ChangeState(new NavigationState_TargetPosition(this, -1, otherSide.FloorWorldPosition));
		if (Vector3.Distance(base.transform.position, door.CenteredPosition) <= 5f)
		{
			door.OpenTimedWithKey(door.DefaultTime, false);
		}
		yield break;
	}

	// Token: 0x06000227 RID: 551 RVA: 0x0000BFE4 File Offset: 0x0000A1E4
	public void SendToDetention()
	{
		if (this.ec.offices.Count > 0)
		{
			int index = Random.Range(0, this.ec.offices.Count);
			this.targetedPlayer.Teleport(this.ec.RealRoomMid(this.ec.offices[index]));
			this.targetedPlayer.ClearGuilt();
			base.transform.position = this.targetedPlayer.transform.position + this.targetedPlayer.transform.forward * 10f;
			float time = this.detentionInit + this.detentionInc * (float)this.detentionLevel;
			if (this.detentionLevel >= this.audTimes.Length - 1)
			{
				time = 99f;
			}
			this.ec.offices[index].functionObject.GetComponent<DetentionRoomFunction>().Activate(time, this.ec);
			this.audMan.QueueAudio(this.audTimes[this.detentionLevel]);
			this.audMan.QueueAudio(this.audDetention);
			this.audMan.QueueAudio(this.audScolds[Random.Range(0, this.audScolds.Length)]);
			this.timeInSight[this.targetedPlayer.playerNumber] = 0f;
			if (this.detentionUi != null)
			{
				Object.Destroy(this.detentionUi.gameObject);
			}
			this.detentionUi = Object.Instantiate<DetentionUi>(this.detentionUiPre);
			this.detentionUi.Initialize(Singleton<CoreGameManager>.Instance.GetCamera(this.targetedPlayer.playerNumber).canvasCam, time, this.ec);
			this.detentionLevel = Mathf.Min(this.detentionLevel + 1, this.audTimes.Length - 1);
			Baldi baldi = this.ec.GetBaldi();
			if (baldi != null)
			{
				baldi.ClearSoundLocations();
			}
			this.ec.MakeNoise(this.targetedPlayer.transform.position, this.detentionNoise);
			this.behaviorStateMachine.ChangeState(new Principal_Detention(this, 3f));
		}
	}

	// Token: 0x06000228 RID: 552 RVA: 0x0000C204 File Offset: 0x0000A404
	public void Scold(string brokenRule)
	{
		this.audMan.FlushQueue(true);
		if (brokenRule != null)
		{
			uint num = <PrivateImplementationDetails>.ComputeStringHash(brokenRule);
			if (num <= 1160324588U)
			{
				if (num != 658687313U)
				{
					if (num != 799624914U)
					{
						if (num != 1160324588U)
						{
							return;
						}
						if (!(brokenRule == "Running"))
						{
							return;
						}
						this.audMan.QueueAudio(this.audNoRunning);
						return;
					}
					else
					{
						if (!(brokenRule == "Lockers"))
						{
							return;
						}
						this.audMan.QueueAudio(this.audNoLockers);
						return;
					}
				}
				else
				{
					if (!(brokenRule == "Escaping"))
					{
						return;
					}
					this.audMan.QueueAudio(this.audNoEscaping);
					return;
				}
			}
			else if (num <= 3081129568U)
			{
				if (num != 2186282543U)
				{
					if (num != 3081129568U)
					{
						return;
					}
					if (!(brokenRule == "AfterHours"))
					{
						return;
					}
					this.audMan.QueueAudio(this.audNoAfterHours);
					return;
				}
				else
				{
					if (!(brokenRule == "Drinking"))
					{
						return;
					}
					this.audMan.QueueAudio(this.audNoDrinking);
					return;
				}
			}
			else if (num != 4131415285U)
			{
				if (num != 4144467295U)
				{
					return;
				}
				if (!(brokenRule == "Faculty"))
				{
					return;
				}
				this.audMan.QueueAudio(this.audNoFaculty);
				return;
			}
			else
			{
				if (!(brokenRule == "Bullying"))
				{
					return;
				}
				this.audMan.QueueAudio(this.audNoBullying);
			}
		}
	}

	// Token: 0x06000229 RID: 553 RVA: 0x0000C354 File Offset: 0x0000A554
	public void WhistleReact(Vector3 target)
	{
		this.behaviorStateMachine.ChangeState(new Principal_WhistleApproach(this, this.behaviorStateMachine.currentState, target));
		this.navigator.SetSpeed(this.whistleSpeed);
		this.audMan.FlushQueue(true);
		this.audMan.PlaySingle(this.audComing);
	}

	// Token: 0x0600022A RID: 554 RVA: 0x0000C3AC File Offset: 0x0000A5AC
	public void WhistleReached()
	{
		this.navigator.maxSpeed = this.defaultSpeed;
	}

	// Token: 0x0600022B RID: 555 RVA: 0x0000C3BF File Offset: 0x0000A5BF
	public void WhistleChance()
	{
		if (this.whistleRng.NextDouble() < (double)this.whistleChance && !this.audMan.QueuedAudioIsPlaying)
		{
			this.audMan.PlaySingle(this.audWhistle);
		}
	}

	// Token: 0x0400022E RID: 558
	private PlayerManager targetedPlayer;

	// Token: 0x0400022F RID: 559
	[SerializeField]
	private DetentionUi detentionUiPre;

	// Token: 0x04000230 RID: 560
	private DetentionUi detentionUi;

	// Token: 0x04000231 RID: 561
	[SerializeField]
	private AudioManager audMan;

	// Token: 0x04000232 RID: 562
	[SerializeField]
	private SoundObject[] audTimes = new SoundObject[0];

	// Token: 0x04000233 RID: 563
	[SerializeField]
	private SoundObject[] audScolds = new SoundObject[0];

	// Token: 0x04000234 RID: 564
	[SerializeField]
	private SoundObject audNoRunning;

	// Token: 0x04000235 RID: 565
	[SerializeField]
	private SoundObject audNoDrinking;

	// Token: 0x04000236 RID: 566
	[SerializeField]
	private SoundObject audNoEating;

	// Token: 0x04000237 RID: 567
	[SerializeField]
	private SoundObject audNoFaculty;

	// Token: 0x04000238 RID: 568
	[SerializeField]
	private SoundObject audNoStabbing;

	// Token: 0x04000239 RID: 569
	[SerializeField]
	private SoundObject audNoBullying;

	// Token: 0x0400023A RID: 570
	[SerializeField]
	private SoundObject audNoEscaping;

	// Token: 0x0400023B RID: 571
	[SerializeField]
	private SoundObject audNoLockers;

	// Token: 0x0400023C RID: 572
	[SerializeField]
	private SoundObject audNoAfterHours;

	// Token: 0x0400023D RID: 573
	[SerializeField]
	private SoundObject audDetention;

	// Token: 0x0400023E RID: 574
	[SerializeField]
	private SoundObject audWhistle;

	// Token: 0x0400023F RID: 575
	[SerializeField]
	private SoundObject audComing;

	// Token: 0x04000240 RID: 576
	private Random whistleRng = new Random();

	// Token: 0x04000241 RID: 577
	[SerializeField]
	private float detentionInit = 15f;

	// Token: 0x04000242 RID: 578
	[SerializeField]
	private float detentionInc = 5f;

	// Token: 0x04000243 RID: 579
	[SerializeField]
	private float whistleChance = 2f;

	// Token: 0x04000244 RID: 580
	[SerializeField]
	private float whistleSpeed = 200f;

	// Token: 0x04000245 RID: 581
	[SerializeField]
	private float knockPauseTime = 3f;

	// Token: 0x04000246 RID: 582
	private float defaultSpeed;

	// Token: 0x04000247 RID: 583
	private float[] timeInSight = new float[0];

	// Token: 0x04000248 RID: 584
	[SerializeField]
	[Range(0f, 127f)]
	private int detentionNoise = 95;

	// Token: 0x04000249 RID: 585
	private int detentionLevel;

	// Token: 0x0400024A RID: 586
	[SerializeField]
	private bool allKnowing;

	// Token: 0x0400024B RID: 587
	private StandardDoor lastKnockedDoor;
}
