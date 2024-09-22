using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000008 RID: 8
public class FirstPrize_Active : FirstPrize_StateBase
{
	// Token: 0x06000022 RID: 34 RVA: 0x00002BCB File Offset: 0x00000DCB
	public FirstPrize_Active(FirstPrize firstPrize) : base(firstPrize)
	{
	}

	// Token: 0x06000023 RID: 35 RVA: 0x00002BEA File Offset: 0x00000DEA
	public override void Enter()
	{
		base.Enter();
		this.targetState = new NavigationState_TargetPosition(this.npc, 63, this.npc.transform.position);
	}

	// Token: 0x06000024 RID: 36 RVA: 0x00002C18 File Offset: 0x00000E18
	public override void Update()
	{
		this.playerInSight = (this.unseenDelay > 0f);
		if (this.nextTarget != Vector3.zero)
		{
			this.TargetPosition(this.nextTarget);
		}
		if (!this.npc.Navigator.HasDestination && this.npc.Navigator.Speed <= this.firstPrize.wanderSpeed + 1f)
		{
			base.ChangeNavigationState(new NavigationState_WanderRandom(this.npc, 0));
		}
		if (this.npc.Navigator.NextPoint != this.npc.transform.position)
		{
			this._angleDiff = Mathf.DeltaAngle(this.npc.transform.eulerAngles.y, Mathf.Atan2(this.npc.Navigator.NextPoint.x - this.npc.transform.position.x, this.npc.Navigator.NextPoint.z - this.npc.transform.position.z) * 57.29578f);
		}
		else
		{
			this._angleDiff = 0f;
		}
		if (Mathf.Abs(this._angleDiff) > this.firstPrize.angleRange)
		{
			this.npc.Navigator.maxSpeed = 0f;
			this.npc.Navigator.SetSpeed(0f);
			this.pushing = false;
		}
		else if (this.unseenDelay > 0f && (this.nextTarget == Vector3.zero || IntVector2.GetGridPosition(this.nextTarget) == IntVector2.GetGridPosition(this.npc.Navigator.CurrentDestination)))
		{
			this.pushing = true;
			this.npc.Navigator.maxSpeed = this.firstPrize.chaseSpeed;
		}
		else
		{
			this.pushing = false;
			this.npc.Navigator.maxSpeed = this.firstPrize.wanderSpeed;
		}
		if (this.npc.Navigator.Speed < this.firstPrize.minPushSpeed || Mathf.Abs(this._angleDiff) <= this.firstPrize.angleRange)
		{
			this._rotation = this.npc.transform.eulerAngles;
			this._rotation.y = this._rotation.y + Mathf.Min(this.firstPrize.turnSpeed * Time.deltaTime * this.npc.TimeScale, Mathf.Abs(this._angleDiff)) * Mathf.Sign(this._angleDiff);
			this.npc.transform.eulerAngles = this._rotation;
		}
		this.UpdateMoveModStatus();
		if (!this.npc.looker.PlayerInSight() && this.unseenDelay > 0f)
		{
			this.unseenDelay -= Time.deltaTime * this.npc.TimeScale;
			if (this.unseenDelay <= 0f)
			{
				this.LostPlayer();
			}
		}
		if (this.npc.Navigator.speed >= this.firstPrize.slamSpeed)
		{
			if (!this.atSlamSpeed)
			{
				this.npc.Navigator.passableObstacles.Add(PassableObstacle.Window);
				this.atSlamSpeed = true;
			}
			if (Physics.Raycast(this.npc.transform.position, this.npc.transform.forward, out this.raycastHit, 5f, 2097152, QueryTriggerInteraction.Collide) && this.raycastHit.transform.CompareTag("Window"))
			{
				this.raycastHit.transform.GetComponent<Window>().Break(true);
			}
		}
		else if (this.atSlamSpeed)
		{
			this.npc.Navigator.passableObstacles.Remove(PassableObstacle.Window);
			this.atSlamSpeed = false;
		}
		if (this.npc.Navigator.speed == 0f && this.previousFrameSpeed >= this.firstPrize.slamSpeed)
		{
			this.firstPrize.audMan.PlaySingle(this.firstPrize.audBang);
			this.npc.ec.MakeNoise(this.npc.transform.position, this.firstPrize.slamNoiseValue);
		}
		this.previousFrameSpeed = this.npc.Navigator.Speed;
	}

	// Token: 0x06000025 RID: 37 RVA: 0x00003088 File Offset: 0x00001288
	private void UpdateMoveModStatus()
	{
		if (this.pushing && this.npc.Navigator.Velocity.magnitude >= this.firstPrize.minPushSpeed * Time.deltaTime)
		{
			this.hugEnabled = true;
			this.UpdateMoveMods();
			return;
		}
		this.hugEnabled = false;
		for (int i = 0; i < this.moveModsMan.Count; i++)
		{
			this.moveModsMan.moveMod(i).movementAddend = Vector3.zero;
			this.moveModsMan.moveMod(i).movementMultiplier = 1f;
			this.moveModsMan.moveMod(i).forceTrigger = false;
		}
	}

	// Token: 0x06000026 RID: 38 RVA: 0x00003134 File Offset: 0x00001334
	private void UpdateMoveMods()
	{
		Vector3 a = this.npc.transform.position + this.npc.transform.forward * 2f;
		for (int i = 0; i < this.moveModsMan.Count; i++)
		{
			float f = Mathf.Abs((a - this.moveModsMan.trans(i).position).magnitude);
			Vector3 a2 = (a - this.moveModsMan.trans(i).position).normalized * 2f * Mathf.Pow(f, 2f);
			Vector3 b = this.npc.transform.forward * (this.npc.Navigator.Speed + this.npc.Navigator.Acceleration);
			this.moveModsMan.moveMod(i).movementAddend = a2 + b;
			this.moveModsMan.moveMod(i).movementMultiplier = 0.1f;
			this.moveModsMan.moveMod(i).forceTrigger = true;
		}
	}

	// Token: 0x06000027 RID: 39 RVA: 0x00003268 File Offset: 0x00001468
	public void TargetPosition(Vector3 target)
	{
		if (this.npc.Navigator.Speed > this.firstPrize.wanderSpeed + 1f)
		{
			this.nextTarget = target;
			return;
		}
		base.ChangeNavigationState(this.targetState);
		this.targetState.UpdatePosition(target);
		this.npc.Navigator.SkipCurrentDestinationPoint();
		this.nextTarget = Vector3.zero;
	}

	// Token: 0x06000028 RID: 40 RVA: 0x000032D4 File Offset: 0x000014D4
	public override void DestinationEmpty()
	{
		if (!this.npc.Navigator.Wandering)
		{
			this.npc.Navigator.maxSpeed = 0f;
			this.npc.Navigator.SetSpeed(0f);
			this.pushing = false;
		}
		base.DestinationEmpty();
		this.currentStandardTargetPos.x = 0;
		this.currentStandardTargetPos.z = 0;
		this.currentWindowTargetPos.x = 0;
		this.currentWindowTargetPos.z = 0;
		if (this.npc.Navigator.Speed <= this.firstPrize.wanderSpeed + 1f)
		{
			base.ChangeNavigationState(new NavigationState_WanderRandom(this.npc, 0));
			if (Random.value * 100f <= this.firstPrize.randomAudioChance && !this.firstPrize.audMan.QueuedAudioIsPlaying)
			{
				this.firstPrize.audMan.QueueAudio(this.firstPrize.audRand[Random.Range(0, this.firstPrize.audRand.Length)]);
			}
		}
	}

	// Token: 0x06000029 RID: 41 RVA: 0x000033E8 File Offset: 0x000015E8
	public override void PlayerInSight(PlayerManager player)
	{
		if (!player.Tagged)
		{
			this.ChargeTarget(player.transform.position, false);
			this.unseenDelay = 1f;
		}
	}

	// Token: 0x0600002A RID: 42 RVA: 0x00003410 File Offset: 0x00001610
	public void ChargeTarget(Vector3 pos, bool forceCurrent)
	{
		Direction dir = Directions.DirFromVector3(pos - this.npc.transform.position, 45f);
		this.standardTargetPos = IntVector2.GetGridPosition(pos);
		this.windowTargetPos = IntVector2.GetGridPosition(pos);
		this.npc.ec.FurthestTileInDir(IntVector2.GetGridPosition(this.npc.transform.position), dir, out this.standardTargetPos);
		this.npc.ec.TempOpenWindows();
		this.npc.ec.FurthestTileInDir(IntVector2.GetGridPosition(this.npc.transform.position), dir, out this.windowTargetPos);
		this.npc.ec.TempCloseWindows();
		if (forceCurrent)
		{
			this.currentStandardTargetPos = this.standardTargetPos;
			this.currentWindowTargetPos = this.windowTargetPos;
		}
		if (this.currentStandardTargetPos != this.standardTargetPos && this.currentWindowTargetPos != this.windowTargetPos)
		{
			this.TargetPosition(this.npc.ec.CellFromPosition(this.standardTargetPos).FloorWorldPosition);
			this.targetingWindow = false;
			if (this.npc.Navigator.Speed <= this.firstPrize.wanderSpeed + 1f)
			{
				this.currentStandardTargetPos = this.standardTargetPos;
				this.currentWindowTargetPos = this.windowTargetPos;
				return;
			}
		}
		else if (this.atSlamSpeed)
		{
			if (!this.targetingWindow)
			{
				this.targetState.UpdatePosition(this.npc.ec.CellFromPosition(this.currentWindowTargetPos).FloorWorldPosition);
				base.ChangeNavigationState(this.targetState);
				this.targetingWindow = true;
				return;
			}
		}
		else if (this.targetingWindow)
		{
			this.targetState.UpdatePosition(this.npc.ec.CellFromPosition(this.currentStandardTargetPos).FloorWorldPosition);
			base.ChangeNavigationState(this.targetState);
			this.targetingWindow = false;
		}
	}

	// Token: 0x0600002B RID: 43 RVA: 0x00003604 File Offset: 0x00001804
	private void LostPlayer()
	{
		this.currentStandardTargetPos.x = 0;
		this.currentStandardTargetPos.z = 0;
		this.currentWindowTargetPos.x = 0;
		this.currentWindowTargetPos.z = 0;
		if (this.npc.Navigator.speed <= this.firstPrize.wanderSpeed + 1f)
		{
			base.ChangeNavigationState(new NavigationState_WanderRandom(this.npc, 0));
		}
		this.npc.Navigator.maxSpeed = this.firstPrize.wanderSpeed;
		if (!this.firstPrize.audMan.QueuedAudioIsPlaying)
		{
			this.firstPrize.audMan.QueueAudio(this.firstPrize.audLose[Random.Range(0, this.firstPrize.audLose.Length)]);
		}
	}

	// Token: 0x0600002C RID: 44 RVA: 0x000036D4 File Offset: 0x000018D4
	public override void PlayerSighted(PlayerManager player)
	{
		if (!this.firstPrize.audMan.QueuedAudioIsPlaying)
		{
			this.firstPrize.audMan.QueueAudio(this.firstPrize.audSee[Random.Range(0, this.firstPrize.audSee.Length)]);
		}
	}

	// Token: 0x0600002D RID: 45 RVA: 0x00003724 File Offset: 0x00001924
	public override void OnStateTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			this.moveModsMan.AddMoveMod(other.transform);
			this.currentPlayer = other.GetComponent<PlayerManager>();
			PlayerManager playerManager = this.currentPlayer;
			playerManager.onPlayerTeleport = (PlayerManager.PlayerTeleportedFunction)Delegate.Combine(playerManager.onPlayerTeleport, new PlayerManager.PlayerTeleportedFunction(this.PlayerTeleported));
			if (this.hugEnabled)
			{
				other.transform.position = this.npc.transform.position + this.npc.transform.forward * 2f;
			}
			if (!this.firstPrize.audMan.QueuedAudioIsPlaying)
			{
				this.firstPrize.audMan.QueueAudio(this.firstPrize.audHug[Random.Range(0, this.firstPrize.audHug.Length)]);
			}
		}
	}

	// Token: 0x0600002E RID: 46 RVA: 0x00003808 File Offset: 0x00001A08
	public override void OnStateTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			this.PlayerExitedTrigger(other.transform);
		}
	}

	// Token: 0x0600002F RID: 47 RVA: 0x00003828 File Offset: 0x00001A28
	private void PlayerExitedTrigger(Transform playerTrans)
	{
		this.moveModsMan.Remove(playerTrans);
		if (this.currentPlayer != null)
		{
			PlayerManager playerManager = this.currentPlayer;
			playerManager.onPlayerTeleport = (PlayerManager.PlayerTeleportedFunction)Delegate.Remove(playerManager.onPlayerTeleport, new PlayerManager.PlayerTeleportedFunction(this.PlayerTeleported));
			this.currentPlayer = null;
		}
	}

	// Token: 0x06000030 RID: 48 RVA: 0x0000387D File Offset: 0x00001A7D
	public void PlayerTeleported(PlayerManager player, Vector3 pos, Vector3 positionDelta)
	{
		this.pushing = false;
		this.UpdateMoveModStatus();
		this.PlayerExitedTrigger(player.transform);
	}

	// Token: 0x06000031 RID: 49 RVA: 0x00003898 File Offset: 0x00001A98
	public override void Exit()
	{
		base.Exit();
		if (this.currentPlayer != null)
		{
			PlayerManager playerManager = this.currentPlayer;
			playerManager.onPlayerTeleport = (PlayerManager.PlayerTeleportedFunction)Delegate.Remove(playerManager.onPlayerTeleport, new PlayerManager.PlayerTeleportedFunction(this.PlayerTeleported));
		}
		this.moveModsMan.RemoveAll();
	}

	// Token: 0x04000044 RID: 68
	private NavigationState targetState;

	// Token: 0x04000045 RID: 69
	private List<ActivityModifier> actMods = new List<ActivityModifier>();

	// Token: 0x04000046 RID: 70
	private MoveModsManager moveModsMan = new MoveModsManager();

	// Token: 0x04000047 RID: 71
	private PlayerManager currentPlayer;

	// Token: 0x04000048 RID: 72
	private RaycastHit raycastHit;

	// Token: 0x04000049 RID: 73
	private Vector3 _rotation;

	// Token: 0x0400004A RID: 74
	private Vector3 _nextPoint;

	// Token: 0x0400004B RID: 75
	private Vector3 nextTarget;

	// Token: 0x0400004C RID: 76
	private IntVector2 standardTargetPos;

	// Token: 0x0400004D RID: 77
	private IntVector2 windowTargetPos;

	// Token: 0x0400004E RID: 78
	private IntVector2 currentStandardTargetPos;

	// Token: 0x0400004F RID: 79
	private IntVector2 currentWindowTargetPos;

	// Token: 0x04000050 RID: 80
	private Cell _currentTile;

	// Token: 0x04000051 RID: 81
	private float _xRangeMin;

	// Token: 0x04000052 RID: 82
	private float _xRangeMax;

	// Token: 0x04000053 RID: 83
	private float _zRangeMin;

	// Token: 0x04000054 RID: 84
	private float _zRangeMax;

	// Token: 0x04000055 RID: 85
	private float previousFrameSpeed;

	// Token: 0x04000056 RID: 86
	private Vector3 _position;

	// Token: 0x04000057 RID: 87
	private float cutTimeLeft;

	// Token: 0x04000058 RID: 88
	private float unseenDelay;

	// Token: 0x04000059 RID: 89
	private float _angleDiff;

	// Token: 0x0400005A RID: 90
	private bool pushing;

	// Token: 0x0400005B RID: 91
	private bool atSlamSpeed;

	// Token: 0x0400005C RID: 92
	private bool targetingWindow;

	// Token: 0x0400005D RID: 93
	private bool hugEnabled;

	// Token: 0x0400005E RID: 94
	private bool playerInSight;
}
