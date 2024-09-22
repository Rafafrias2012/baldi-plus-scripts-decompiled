using System;
using MidiPlayerTK;
using UnityEngine;

// Token: 0x02000013 RID: 19
public class Baldi : NPC
{
	// Token: 0x14000001 RID: 1
	// (add) Token: 0x06000064 RID: 100 RVA: 0x000048FC File Offset: 0x00002AFC
	// (remove) Token: 0x06000065 RID: 101 RVA: 0x00004930 File Offset: 0x00002B30
	public static event Baldi.BaldiSlapDelegate OnBaldiSlap;

	// Token: 0x06000066 RID: 102 RVA: 0x00004963 File Offset: 0x00002B63
	public override void Initialize()
	{
		base.Initialize();
		this.behaviorStateMachine.ChangeState(new Baldi_Chase(this, this));
		this.behaviorStateMachine.ChangeNavigationState(new NavigationState_WanderRandom(this, 0));
	}

	// Token: 0x06000067 RID: 103 RVA: 0x0000498F File Offset: 0x00002B8F
	private void Start()
	{
		this.GetAngry(this.baseAnger);
	}

	// Token: 0x06000068 RID: 104 RVA: 0x0000499D File Offset: 0x00002B9D
	private void OnEnable()
	{
		MusicManager.OnMidiEvent += this.MidiEvent;
	}

	// Token: 0x06000069 RID: 105 RVA: 0x000049B0 File Offset: 0x00002BB0
	private void OnDisable()
	{
		MusicManager.OnMidiEvent -= this.MidiEvent;
	}

	// Token: 0x0600006A RID: 106 RVA: 0x000049C3 File Offset: 0x00002BC3
	protected override void VirtualUpdate()
	{
		base.VirtualUpdate();
	}

	// Token: 0x0600006B RID: 107 RVA: 0x000049CC File Offset: 0x00002BCC
	public void Hear(Vector3 position, int value, bool indicator)
	{
		this.soundLocations[value] = position;
		if (value >= this.currentSoundVal)
		{
			if (indicator)
			{
				for (int i = 0; i < Singleton<CoreGameManager>.Instance.setPlayers; i++)
				{
					Singleton<CoreGameManager>.Instance.GetHud(i).ActivateBaldicator(true);
				}
			}
			this.UpdateSoundTarget();
			return;
		}
		if (indicator)
		{
			for (int j = 0; j < Singleton<CoreGameManager>.Instance.setPlayers; j++)
			{
				Singleton<CoreGameManager>.Instance.GetHud(j).ActivateBaldicator(false);
			}
		}
	}

	// Token: 0x0600006C RID: 108 RVA: 0x00004A48 File Offset: 0x00002C48
	public void Praise(float time)
	{
		AudioManager audioManager = this.audMan;
		WeightedSelection<SoundObject>[] items = this.correctSounds;
		audioManager.PlaySingle(WeightedSelection<SoundObject>.RandomSelection(items));
		this.behaviorStateMachine.ChangeState(new Baldi_Praise(this, this, this.behaviorStateMachine.CurrentState, time));
	}

	// Token: 0x0600006D RID: 109 RVA: 0x00004A8B File Offset: 0x00002C8B
	public void PraiseAnimation()
	{
		this.animator.Play("BAL_Smile", -1, 0f);
	}

	// Token: 0x0600006E RID: 110 RVA: 0x00004AA3 File Offset: 0x00002CA3
	public virtual void GetAngry(float value)
	{
		this.anger += value;
		if ((double)this.anger <= 0.1)
		{
			this.anger = 0.1f;
		}
	}

	// Token: 0x17000009 RID: 9
	// (get) Token: 0x0600006F RID: 111 RVA: 0x00004AD0 File Offset: 0x00002CD0
	public float Delay
	{
		get
		{
			return this.slapCurve.Evaluate(this.anger + this.extraAnger);
		}
	}

	// Token: 0x1700000A RID: 10
	// (get) Token: 0x06000070 RID: 112 RVA: 0x00004AEA File Offset: 0x00002CEA
	public float Speed
	{
		get
		{
			return this.speedCurve.Evaluate(this.anger) + this.baseSpeed + this.extraAnger;
		}
	}

	// Token: 0x1700000B RID: 11
	// (get) Token: 0x06000071 RID: 113 RVA: 0x00004B0B File Offset: 0x00002D0B
	public float MovementPortion
	{
		get
		{
			return Mathf.Max(0.1f, this.anger / (this.anger + 50f));
		}
	}

	// Token: 0x06000072 RID: 114 RVA: 0x00004B2A File Offset: 0x00002D2A
	public virtual void SetAnger(float value)
	{
		this.anger = value;
		this.GetAngry(0f);
	}

	// Token: 0x06000073 RID: 115 RVA: 0x00004B3E File Offset: 0x00002D3E
	public void GetExtraAnger(float value)
	{
		this.extraAnger += value;
	}

	// Token: 0x06000074 RID: 116 RVA: 0x00004B50 File Offset: 0x00002D50
	public void UpdateSoundTarget()
	{
		for (int i = this.soundLocations.Length - 1; i >= 0; i--)
		{
			if (this.soundLocations[i] != Vector3.zero)
			{
				if (i == 127)
				{
					this.behaviorStateMachine.ChangeNavigationState(new NavigationState_TargetPlayer(this, 63, this.soundLocations[i]));
				}
				else
				{
					this.behaviorStateMachine.ChangeNavigationState(new NavigationState_TargetPosition(this, 63, this.soundLocations[i]));
				}
				this.currentSoundVal = i;
				this.soundLocations[i] = Vector3.zero;
				return;
			}
		}
		this.behaviorStateMachine.ChangeNavigationState(new NavigationState_WanderRandom(this, 0));
		this.currentSoundVal = 0;
	}

	// Token: 0x06000075 RID: 117 RVA: 0x00004C04 File Offset: 0x00002E04
	public void ClearSoundLocations()
	{
		for (int i = 0; i < this.soundLocations.Length; i++)
		{
			this.soundLocations[i] = Vector3.zero;
		}
		this.currentSoundVal = 0;
	}

	// Token: 0x06000076 RID: 118 RVA: 0x00004C3C File Offset: 0x00002E3C
	public void Distract()
	{
		this.ClearSoundLocations();
		this.behaviorStateMachine.ChangeNavigationState(new NavigationState_WanderRandom(this, 0));
		base.Navigator.ClearDestination();
	}

	// Token: 0x06000077 RID: 119 RVA: 0x00004C64 File Offset: 0x00002E64
	public override float DistanceCheck(float val)
	{
		if (this.navigator.Am.Multiplier == 0f)
		{
			this.slapTotal = float.PositiveInfinity;
			return 0f;
		}
		float num = val * (1f / this.navigator.Am.Multiplier);
		if (this.slapTotal + num <= this.slapDistance)
		{
			this.slapTotal += num;
			this.totalDistance += val;
			return val;
		}
		if (this.slapTotal < this.slapDistance)
		{
			float num2 = (this.slapDistance - this.slapTotal) * this.navigator.Am.Multiplier;
			this.slapTotal += num;
			this.totalDistance += num2;
			return num2;
		}
		this.slapTotal += num;
		this.EndSlap();
		return 0f;
	}

	// Token: 0x06000078 RID: 120 RVA: 0x00004D48 File Offset: 0x00002F48
	private bool HasSoundLocation()
	{
		for (int i = 0; i < this.soundLocations.Length; i++)
		{
			if (this.soundLocations[i] != Vector3.zero)
			{
				return true;
			}
		}
		this.currentSoundVal = 0;
		return false;
	}

	// Token: 0x06000079 RID: 121 RVA: 0x00004D8A File Offset: 0x00002F8A
	private void MidiEvent(MPTKEvent midiEvent)
	{
		if (midiEvent.Channel == 15 && midiEvent.Command == MPTKCommand.NoteOn)
		{
			this.Slap();
		}
	}

	// Token: 0x0600007A RID: 122 RVA: 0x00004DA9 File Offset: 0x00002FA9
	public void UpdateSlapDistance()
	{
		this.nextSlapDistance += this.Speed * Time.deltaTime * base.TimeScale;
	}

	// Token: 0x0600007B RID: 123 RVA: 0x00004DCB File Offset: 0x00002FCB
	public void ResetSlapDistance()
	{
		this.nextSlapDistance = 0f;
	}

	// Token: 0x0600007C RID: 124 RVA: 0x00004DD8 File Offset: 0x00002FD8
	public void ResetSprite()
	{
		this.animator.Play("BAL_Slap", -1, 1f);
	}

	// Token: 0x0600007D RID: 125 RVA: 0x00004DF0 File Offset: 0x00002FF0
	public virtual void Slap()
	{
		Baldi.OnBaldiSlap(this);
		this.slapTotal = 0f;
		this.slapDistance = this.nextSlapDistance;
		this.nextSlapDistance = 0f;
		this.navigator.SetSpeed(this.slapDistance / (this.Delay * this.MovementPortion));
		if (this.breakRuler)
		{
			this.behaviorStateMachine.ChangeState(new Baldi_Chase_Broken(this, this));
			this.breakRuler = false;
			return;
		}
		if (this.restoreRuler)
		{
			this.behaviorStateMachine.ChangeState(new Baldi_Chase(this, this));
			this.restoreRuler = false;
		}
	}

	// Token: 0x0600007E RID: 126 RVA: 0x00004E8C File Offset: 0x0000308C
	public void SlapNormal()
	{
		this.animator.Play("BAL_Slap", -1, 0f);
		this.audMan.PlaySingle(this.slap);
		this.SlapRumble();
	}

	// Token: 0x0600007F RID: 127 RVA: 0x00004EBB File Offset: 0x000030BB
	public void SlapBreak()
	{
		this.animator.Play("BAL_Slap_Broken", -1, 0f);
		this.audMan.PlaySingle(this.rulerBreak);
		this.SlapRumble();
	}

	// Token: 0x06000080 RID: 128 RVA: 0x00004EEA File Offset: 0x000030EA
	public void SlapBroken()
	{
		this.animator.Play("BAL_Slap_Broken", -1, 0f);
	}

	// Token: 0x06000081 RID: 129 RVA: 0x00004F04 File Offset: 0x00003104
	public void SlapRumble()
	{
		float num = Vector3.Distance(base.transform.position, Singleton<CoreGameManager>.Instance.GetPlayer(0).transform.position);
		if (num < 100f)
		{
			float signedAngle = Vector3.SignedAngle(base.transform.position - Singleton<CoreGameManager>.Instance.GetPlayer(0).transform.position, Singleton<CoreGameManager>.Instance.GetPlayer(0).transform.forward, Vector3.up);
			Singleton<InputManager>.Instance.Rumble(1f - num / 100f, 0.2f, signedAngle);
		}
	}

	// Token: 0x06000082 RID: 130 RVA: 0x00004FA1 File Offset: 0x000031A1
	public void EndSlap()
	{
		this.slapDistance = 0f;
	}

	// Token: 0x1700000C RID: 12
	// (get) Token: 0x06000083 RID: 131 RVA: 0x00004FAE File Offset: 0x000031AE
	public AudioManager AudMan
	{
		get
		{
			return this.audMan;
		}
	}

	// Token: 0x06000084 RID: 132 RVA: 0x00004FB6 File Offset: 0x000031B6
	public void BreakRuler()
	{
		this.breakRuler = true;
	}

	// Token: 0x06000085 RID: 133 RVA: 0x00004FBF File Offset: 0x000031BF
	public void RestoreRuler()
	{
		this.restoreRuler = true;
	}

	// Token: 0x06000086 RID: 134 RVA: 0x00004FC8 File Offset: 0x000031C8
	public virtual void CaughtPlayer(PlayerManager player)
	{
		this.behaviorStateMachine.ChangeState(new Baldi_Attack(this, this));
		if (Singleton<BaseGameManager>.Instance != null)
		{
			Singleton<BaseGameManager>.Instance.EndGame(player.transform, this);
			return;
		}
		Singleton<CoreGameManager>.Instance.EndGame(player.transform, this);
	}

	// Token: 0x06000087 RID: 135 RVA: 0x00005018 File Offset: 0x00003218
	public void TakeApple()
	{
		this.behaviorStateMachine.ChangeState(new Baldi_Apple(this, this, this.behaviorStateMachine.CurrentState));
		base.StopAllCoroutines();
		this.navigator.SetSpeed(0f);
		this.audMan.FlushQueue(true);
		this.audMan.PlaySingle(this.audAppleThanks);
		this.animator.Play("BAL_AppleIntro", -1, 0f);
	}

	// Token: 0x06000088 RID: 136 RVA: 0x0000508B File Offset: 0x0000328B
	public void ResumeApple()
	{
		this.animator.Play("BAL_AppleLoop", -1, 0f);
	}

	// Token: 0x06000089 RID: 137 RVA: 0x000050A4 File Offset: 0x000032A4
	public void EatSound()
	{
		AudioManager audioManager = this.audMan;
		WeightedSelection<SoundObject>[] items = this.eatSounds;
		audioManager.PlaySingle(WeightedSelection<SoundObject>.RandomSelection(items));
	}

	// Token: 0x04000098 RID: 152
	[SerializeField]
	private Animator animator;

	// Token: 0x04000099 RID: 153
	[SerializeField]
	private AudioManager audMan;

	// Token: 0x0400009A RID: 154
	[SerializeField]
	private SoundObject slap;

	// Token: 0x0400009B RID: 155
	[SerializeField]
	private SoundObject rulerBreak;

	// Token: 0x0400009C RID: 156
	[SerializeField]
	private SoundObject audAppleThanks;

	// Token: 0x0400009D RID: 157
	[SerializeField]
	private WeightedSoundObject[] eatSounds;

	// Token: 0x0400009E RID: 158
	[SerializeField]
	private WeightedSoundObject[] correctSounds;

	// Token: 0x0400009F RID: 159
	public WeightedSoundObject[] loseSounds;

	// Token: 0x040000A0 RID: 160
	[SerializeField]
	private AnimationCurve speedCurve = new AnimationCurve();

	// Token: 0x040000A1 RID: 161
	[SerializeField]
	private AnimationCurve slapCurve = new AnimationCurve();

	// Token: 0x040000A2 RID: 162
	private Vector3[] soundLocations = new Vector3[128];

	// Token: 0x040000A3 RID: 163
	public float baseAnger;

	// Token: 0x040000A4 RID: 164
	public float baseSpeed;

	// Token: 0x040000A5 RID: 165
	public float speedMultiplier = 1.652f;

	// Token: 0x040000A6 RID: 166
	public float slapSpeedScale = 1f;

	// Token: 0x040000A7 RID: 167
	public float extraAngerDrain;

	// Token: 0x040000A8 RID: 168
	public float totalDistance;

	// Token: 0x040000A9 RID: 169
	public float appleTime;

	// Token: 0x040000AA RID: 170
	protected float slapTotal;

	// Token: 0x040000AB RID: 171
	protected float anger;

	// Token: 0x040000AC RID: 172
	protected float extraAnger;

	// Token: 0x040000AD RID: 173
	protected float slapDistance;

	// Token: 0x040000AE RID: 174
	protected float nextSlapDistance;

	// Token: 0x040000AF RID: 175
	protected float pauseTime;

	// Token: 0x040000B0 RID: 176
	private int currentSoundVal;

	// Token: 0x040000B1 RID: 177
	private bool breakRuler;

	// Token: 0x040000B2 RID: 178
	private bool restoreRuler;

	// Token: 0x02000305 RID: 773
	// (Invoke) Token: 0x060019B3 RID: 6579
	public delegate void BaldiSlapDelegate(Baldi baldi);
}
