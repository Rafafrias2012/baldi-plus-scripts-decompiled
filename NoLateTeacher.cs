using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Token: 0x02000048 RID: 72
public class NoLateTeacher : NPC
{
	// Token: 0x060001A6 RID: 422 RVA: 0x00009B74 File Offset: 0x00007D74
	public override void Initialize()
	{
		base.Initialize();
		this.behaviorStateMachine.ChangeState(new NoLateTeacher_Wander(this));
		this.behaviorStateMachine.ChangeNavigationState(new NavigationState_WanderRounds(this, 0));
		this.toNextStomp = this.stompDistance;
		this.dijkstraMap = new DijkstraMap(this.ec, PathType.Nav, new Transform[]
		{
			base.transform
		});
		this.mapIconTarget = new GameObject();
		this.mapIconTarget.transform.SetParent(base.transform);
		this.navigator.Entity.OnTeleport += this.Teleport;
	}

	// Token: 0x060001A7 RID: 423 RVA: 0x00009C14 File Offset: 0x00007E14
	protected override void VirtualUpdate()
	{
		base.VirtualUpdate();
		if (Time.timeScale > 0f && !this.skipStompCheck && this.navigator.Velocity.magnitude > 0.1f * Time.deltaTime)
		{
			float num = this.navigator.Velocity.magnitude - this.toNextStomp;
			this.toNextStomp -= this.navigator.Velocity.magnitude;
			if (this.toNextStomp <= 0f)
			{
				this.toNextStomp = this.stompDistance - num;
				this.stompAudMan.PlaySingle(this.audStomp);
				this.animator.Play("Hop", -1, 0f);
			}
		}
		this.skipStompCheck = false;
	}

	// Token: 0x060001A8 RID: 424 RVA: 0x00009CE5 File Offset: 0x00007EE5
	private void Teleport(Vector3 position)
	{
		this.skipStompCheck = true;
	}

	// Token: 0x060001A9 RID: 425 RVA: 0x00009CEE File Offset: 0x00007EEE
	public void WanderSpeed()
	{
		this.navigator.SetSpeed(this.walkSpeed);
		this.navigator.maxSpeed = this.walkSpeed;
	}

	// Token: 0x060001AA RID: 426 RVA: 0x00009D12 File Offset: 0x00007F12
	public void ChaseSpeed()
	{
		this.navigator.maxSpeed = this.runSpeed;
		this.navigator.SetSpeed(this.runSpeed);
	}

	// Token: 0x060001AB RID: 427 RVA: 0x00009D36 File Offset: 0x00007F36
	public void AngrySpeed()
	{
		this.navigator.maxSpeed = this.angrySpeed;
		this.navigator.SetSpeed(this.angrySpeed);
	}

	// Token: 0x060001AC RID: 428 RVA: 0x00009D5A File Offset: 0x00007F5A
	public void CallPlayer()
	{
		this.audMan.PlaySingle(this.audSpot);
	}

	// Token: 0x060001AD RID: 429 RVA: 0x00009D70 File Offset: 0x00007F70
	public void PlayerCaught(PlayerManager player)
	{
		this.AssignClassRoom(player);
		player.plm.am.moveMods.Add(this.moveMod);
		this.moveMod.movementMultiplier = 0f;
		this.navigator.SetSpeed(0f);
		this.navigator.maxSpeed = 0f;
		this.targetedPlayer = player;
		this.ec.AddTimeScale(this.introModifier);
		for (int i = 0; i < this.classRoom.TileCount; i++)
		{
			this.ec.map.Find(this.classRoom.TileAtIndex(i).position.x, this.classRoom.TileAtIndex(i).position.z, this.classRoom.TileAtIndex(i).ConstBin, this.classRoom);
		}
		Cell cell = this.ec.CellFromPosition(IntVector2.GetGridPosition(this.ec.RealRoomMid(this.classRoom)));
		this.ec.map.Find(cell.position.x, cell.position.z, cell.ConstBin, this.classRoom);
		this.mapIconTarget.transform.parent = this.classRoom.objectObject.transform;
		this.mapIconTarget.transform.position = this.ec.RealRoomMid(this.classRoom);
		this.mapIcon = this.ec.map.AddIcon(this.mapIconPre, this.mapIconTarget.transform, Color.white).GetComponent<NoLateIcon>();
		this.mapIcon.timeText.text = Mathf.Floor(this.classTime / 60f).ToString("0") + ":" + (this.classTime % 60f).ToString("00");
		this.popupCanvas.worldCamera = Singleton<CoreGameManager>.Instance.GetCamera(this.targetedPlayer.playerNumber).canvasCam;
		this.popupCanvas.transform.SetParent(null);
		this.popupCanvas.gameObject.SetActive(true);
		this.audMan.FlushQueue(true);
		this.audMan.QueueAudio(this.audIntro);
		this.audMan.QueueAudio(this.audNumbers[Mathf.RoundToInt(this.classTime / 60f)]);
		this.audMan.QueueAudio(this.audMinutes);
		this.audMan.QueueAudio(this.audInsructions);
		this.behaviorStateMachine.ChangeState(new NoLateTeacher_Inform(this, player, 3f));
	}

	// Token: 0x1700001F RID: 31
	// (get) Token: 0x060001AE RID: 430 RVA: 0x0000A027 File Offset: 0x00008227
	public RoomController TargetClassRoom
	{
		get
		{
			return this.classRoom;
		}
	}

	// Token: 0x060001AF RID: 431 RVA: 0x0000A02F File Offset: 0x0000822F
	public void ReleasePlayer(PlayerManager player)
	{
		player.plm.am.moveMods.Remove(this.moveMod);
		this.ec.RemoveTimeScale(this.introModifier);
	}

	// Token: 0x17000020 RID: 32
	// (get) Token: 0x060001B0 RID: 432 RVA: 0x0000A05E File Offset: 0x0000825E
	public bool IsSpeaking
	{
		get
		{
			return this.audMan.QueuedAudioIsPlaying;
		}
	}

	// Token: 0x060001B1 RID: 433 RVA: 0x0000A06B File Offset: 0x0000826B
	public void HeadToClass(PlayerManager player)
	{
		this.navigator.SetSpeed(this.runSpeed);
		this.behaviorStateMachine.ChangeState(new NoLateTeacher_Waiting(this, player, this.classTime));
		this.UpdateTimer(this.classTime);
	}

	// Token: 0x060001B2 RID: 434 RVA: 0x0000A0A2 File Offset: 0x000082A2
	public Vector3 GetClassPosition()
	{
		return this.classRoom.RandomEventSafeCellNoGarbage().FloorWorldPosition;
	}

	// Token: 0x060001B3 RID: 435 RVA: 0x0000A0B4 File Offset: 0x000082B4
	public void UpdateTimer(float time)
	{
		if (Mathf.Floor(time) != (float)this.currentDisplayTime)
		{
			this.currentDisplayTime = Mathf.Max((int)Mathf.Floor(time), 0);
			this.mapIcon.timeText.text = Mathf.Floor((float)(this.currentDisplayTime / 60)).ToString("0") + ":" + (this.currentDisplayTime % 60).ToString("00");
			this.popupText.text = this.mapIcon.timeText.text;
			if (this.currentDisplayTime % 60 == 2)
			{
				this.popupAnimator.Play("TimerSlide", -1, 0f);
				Singleton<CoreGameManager>.Instance.audMan.QueueAudio(this.audNumbers[this.currentDisplayTime / 60]);
				Singleton<CoreGameManager>.Instance.audMan.QueueAudio(this.audMinutesLeft);
			}
		}
	}

	// Token: 0x060001B4 RID: 436 RVA: 0x0000A1A4 File Offset: 0x000083A4
	public void InTime()
	{
		this.behaviorStateMachine.ChangeState(new NoLateTeacher_Cooldown(this, this.Cooldown));
		this.behaviorStateMachine.ChangeNavigationState(new NavigationState_WanderRounds(this, 0));
		this.audMan.PlaySingle(this.audInTime);
		this.Dismiss();
		Singleton<CoreGameManager>.Instance.AddPoints(this.successPoints, this.targetedPlayer.playerNumber, true);
	}

	// Token: 0x060001B5 RID: 437 RVA: 0x0000A20D File Offset: 0x0000840D
	public void Dismiss()
	{
		this.sprite.sprite = this.normalSprite;
		this.audMan.QueueAudio(this.audDismissed);
		this.mapIcon.gameObject.SetActive(false);
	}

	// Token: 0x060001B6 RID: 438 RVA: 0x0000A244 File Offset: 0x00008444
	public void TimeOut(PlayerManager player)
	{
		this.sprite.gameObject.SetActive(false);
		this.navigator.SetSpeed(this.angrySpeed);
		this.trigger.enabled = false;
		this.mapIcon.gameObject.SetActive(false);
		Singleton<CoreGameManager>.Instance.audMan.PlaySingle(this.audTimesUp);
		this.behaviorStateMachine.ChangeState(new NoLateTeacher_TimeOut(this, player));
	}

	// Token: 0x060001B7 RID: 439 RVA: 0x0000A2B8 File Offset: 0x000084B8
	public void Attack(PlayerManager player)
	{
		this.navigator.SetSpeed(this.runSpeed);
		this.sprite.gameObject.SetActive(true);
		this.trigger.enabled = true;
		this.targetedPlayer.Am.moveMods.Add(this.moveMod);
		this.moveMod.movementAddend = Vector3.zero;
		this.moveMod.movementMultiplier = 1f;
		this.ec.MakeNoise(this.ec.RealRoomMid(this.classRoom), this.noiseVal);
		this.sprite.sprite = this.angrySprite;
		this.audMan.PlaySingle(this.audScream);
		this.behaviorStateMachine.ChangeState(new NoLateTeacher_AttackDelay(this, player, this.attackDelay));
	}

	// Token: 0x060001B8 RID: 440 RVA: 0x0000A38C File Offset: 0x0000858C
	public bool Drag(PlayerManager player)
	{
		float num = Mathf.Abs((base.transform.position - this.targetedPlayer.transform.position).magnitude);
		if (num > this.dragBreakDistance)
		{
			this.moveMod.movementAddend = Vector3.zero;
			this.moveMod.movementMultiplier = 1f;
			return false;
		}
		this.moveMod.movementAddend = (base.transform.position - this.targetedPlayer.transform.position).normalized * (this.dragSpeed + num) * base.TimeScale;
		this.moveMod.movementMultiplier = this.dragMultiplier;
		return true;
	}

	// Token: 0x060001B9 RID: 441 RVA: 0x0000A450 File Offset: 0x00008650
	public void ReturnToPlayerCheck(PlayerManager player)
	{
		if (Mathf.Abs((base.transform.position - this.targetedPlayer.transform.position).magnitude) > this.maxDragDistance)
		{
			this.behaviorStateMachine.ChangeState(new NoLateTeacher_Returning(this, player));
		}
	}

	// Token: 0x060001BA RID: 442 RVA: 0x0000A4A4 File Offset: 0x000086A4
	public bool CanDrag(PlayerManager player)
	{
		return Mathf.Abs((base.transform.position - player.transform.position).magnitude) <= this.minDragDistance;
	}

	// Token: 0x060001BB RID: 443 RVA: 0x0000A4E4 File Offset: 0x000086E4
	public void Teach(PlayerManager player)
	{
		this.moveMod.movementAddend = Vector3.zero;
		this.moveMod.movementMultiplier = 1f;
		player.Am.moveMods.Remove(this.moveMod);
		foreach (Door door in this.classRoom.doors)
		{
			door.Shut();
			door.LockTimed(this.angryTeachTime);
		}
		this.ec.MakeNoise(this.ec.RealRoomMid(this.classRoom), this.noiseVal);
		this.behaviorStateMachine.ChangeState(new NoLateTeacher_AngryTeach(this, this.angryTeachTime));
	}

	// Token: 0x060001BC RID: 444 RVA: 0x0000A5B8 File Offset: 0x000087B8
	private void AssignClassRoom(PlayerManager player)
	{
		this._potentialClassRooms.Clear();
		this.dijkstraMap.Calculate();
		foreach (RoomController roomController in this.ec.rooms)
		{
			if (roomController.category == RoomCategory.Class && roomController != this.ec.CellFromPosition(base.transform.position).room && roomController != this.ec.CellFromPosition(player.transform.position).room)
			{
				int num = 0;
				foreach (Door door in roomController.doors)
				{
					if (this.dijkstraMap.Value(door.aTile.position) > num)
					{
						num = this.dijkstraMap.Value(door.aTile.position);
					}
				}
				if (num > 0)
				{
					this._potentialClassRooms.Add(new WeightedRoomController());
					this._potentialClassRooms[this._potentialClassRooms.Count - 1].selection = roomController;
					this._potentialClassRooms[this._potentialClassRooms.Count - 1].weight = num;
				}
			}
		}
		this.classRoom = WeightedSelection<RoomController>.ControlledRandomSelectionList(WeightedRoomController.Convert(this._potentialClassRooms), new Random());
	}

	// Token: 0x17000021 RID: 33
	// (get) Token: 0x060001BD RID: 445 RVA: 0x0000A770 File Offset: 0x00008970
	public float Cooldown
	{
		get
		{
			return this.resetDelay;
		}
	}

	// Token: 0x060001BE RID: 446 RVA: 0x0000A778 File Offset: 0x00008978
	private void OnDestroy()
	{
		if (this.targetedPlayer != null)
		{
			this.targetedPlayer.plm.am.moveMods.Remove(this.moveMod);
		}
		this.ec.RemoveTimeScale(this.introModifier);
	}

	// Token: 0x040001AC RID: 428
	private RoomController classRoom;

	// Token: 0x040001AD RID: 429
	private RoomController targetedRoom;

	// Token: 0x040001AE RID: 430
	private PlayerManager targetedPlayer;

	// Token: 0x040001AF RID: 431
	private List<WeightedRoomController> _potentialClassRooms = new List<WeightedRoomController>();

	// Token: 0x040001B0 RID: 432
	private List<Door> _potentialDoors = new List<Door>();

	// Token: 0x040001B1 RID: 433
	private Door targetedDoor;

	// Token: 0x040001B2 RID: 434
	private Door _currentDoor;

	// Token: 0x040001B3 RID: 435
	[SerializeField]
	private MovementModifier moveMod;

	// Token: 0x040001B4 RID: 436
	[SerializeField]
	private TimeScaleModifier introModifier = new TimeScaleModifier();

	// Token: 0x040001B5 RID: 437
	[SerializeField]
	private AudioManager audMan;

	// Token: 0x040001B6 RID: 438
	[SerializeField]
	private AudioManager stompAudMan;

	// Token: 0x040001B7 RID: 439
	[SerializeField]
	private SoundObject audIntro;

	// Token: 0x040001B8 RID: 440
	[SerializeField]
	private SoundObject audInsructions;

	// Token: 0x040001B9 RID: 441
	[SerializeField]
	private SoundObject audMinutes;

	// Token: 0x040001BA RID: 442
	[SerializeField]
	private SoundObject audMinutesLeft;

	// Token: 0x040001BB RID: 443
	[SerializeField]
	private SoundObject audTimesUp;

	// Token: 0x040001BC RID: 444
	[SerializeField]
	private SoundObject audInTime;

	// Token: 0x040001BD RID: 445
	[SerializeField]
	private SoundObject audStomp;

	// Token: 0x040001BE RID: 446
	[SerializeField]
	private SoundObject audScream;

	// Token: 0x040001BF RID: 447
	[SerializeField]
	private SoundObject audDismissed;

	// Token: 0x040001C0 RID: 448
	[SerializeField]
	private SoundObject audSpot;

	// Token: 0x040001C1 RID: 449
	[SerializeField]
	private SoundObject[] audNumbers = new SoundObject[10];

	// Token: 0x040001C2 RID: 450
	[SerializeField]
	private MapIcon mapIconPre;

	// Token: 0x040001C3 RID: 451
	private NoLateIcon mapIcon;

	// Token: 0x040001C4 RID: 452
	private GameObject mapIconTarget;

	// Token: 0x040001C5 RID: 453
	[SerializeField]
	private TMP_Text popupText;

	// Token: 0x040001C6 RID: 454
	[SerializeField]
	private Canvas popupCanvas;

	// Token: 0x040001C7 RID: 455
	[SerializeField]
	private Animator animator;

	// Token: 0x040001C8 RID: 456
	[SerializeField]
	private Animator popupAnimator;

	// Token: 0x040001C9 RID: 457
	[SerializeField]
	private SpriteRenderer sprite;

	// Token: 0x040001CA RID: 458
	[SerializeField]
	private Sprite normalSprite;

	// Token: 0x040001CB RID: 459
	[SerializeField]
	private Sprite angrySprite;

	// Token: 0x040001CC RID: 460
	[SerializeField]
	private Collider trigger;

	// Token: 0x040001CD RID: 461
	private DijkstraMap dijkstraMap;

	// Token: 0x040001CE RID: 462
	private IntVector2 _playerPosition;

	// Token: 0x040001CF RID: 463
	[SerializeField]
	private float walkSpeed = 10f;

	// Token: 0x040001D0 RID: 464
	[SerializeField]
	private float runSpeed = 30f;

	// Token: 0x040001D1 RID: 465
	[SerializeField]
	private float angrySpeed = 75f;

	// Token: 0x040001D2 RID: 466
	[SerializeField]
	private float classTime = 300f;

	// Token: 0x040001D3 RID: 467
	[SerializeField]
	private float doorChance = 0.95f;

	// Token: 0x040001D4 RID: 468
	[SerializeField]
	private float stompDistance = 5f;

	// Token: 0x040001D5 RID: 469
	[SerializeField]
	private float dragSpeed = 50f;

	// Token: 0x040001D6 RID: 470
	[SerializeField]
	private float dragMultiplier = 0.25f;

	// Token: 0x040001D7 RID: 471
	[SerializeField]
	private float teachTime = 3f;

	// Token: 0x040001D8 RID: 472
	[SerializeField]
	private float angryTeachTime = 15f;

	// Token: 0x040001D9 RID: 473
	[SerializeField]
	private float attackDelay = 0.25f;

	// Token: 0x040001DA RID: 474
	[SerializeField]
	private float maxDragDistance = 50f;

	// Token: 0x040001DB RID: 475
	[SerializeField]
	private float minDragDistance = 2f;

	// Token: 0x040001DC RID: 476
	[SerializeField]
	private float dragBreakDistance = 10f;

	// Token: 0x040001DD RID: 477
	[SerializeField]
	private float resetDelay = 120f;

	// Token: 0x040001DE RID: 478
	private float toNextStomp;

	// Token: 0x040001DF RID: 479
	[SerializeField]
	private int successPoints = 100;

	// Token: 0x040001E0 RID: 480
	[SerializeField]
	[Range(0f, 127f)]
	private int noiseVal = 126;

	// Token: 0x040001E1 RID: 481
	private int currentDisplayTime;

	// Token: 0x040001E2 RID: 482
	private bool skipStompCheck;
}
