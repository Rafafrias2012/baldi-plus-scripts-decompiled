using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000BB RID: 187
public class NPC : MonoBehaviour, IEntityTrigger
{
	// Token: 0x06000441 RID: 1089 RVA: 0x00016C7F File Offset: 0x00014E7F
	private void Awake()
	{
		this.normalLayer = base.gameObject.layer;
	}

	// Token: 0x1700003E RID: 62
	// (get) Token: 0x06000442 RID: 1090 RVA: 0x00016C92 File Offset: 0x00014E92
	public Navigator Navigator
	{
		get
		{
			return this.navigator;
		}
	}

	// Token: 0x06000443 RID: 1091 RVA: 0x00016C9A File Offset: 0x00014E9A
	public virtual void Initialize()
	{
		this.navigator.Initialize(this.ec);
	}

	// Token: 0x06000444 RID: 1092 RVA: 0x00016CB0 File Offset: 0x00014EB0
	protected void Update()
	{
		this.behaviorStateMachine.Update();
		if (this.ec.CellFromPosition(base.transform.position).room != this.currentRoom)
		{
			if (this.currentRoom != null)
			{
				this.currentRoom.functions.OnNpcExit(this);
			}
			this.currentRoom = this.ec.CellFromPosition(base.transform.position).room;
			if (this.currentRoom != null)
			{
				this.currentRoom.functions.OnNpcEnter(this);
			}
		}
		if (this.currentRoom != null)
		{
			this.currentRoom.functions.OnNpcStay(this);
		}
		this.VirtualUpdate();
	}

	// Token: 0x06000445 RID: 1093 RVA: 0x00016D74 File Offset: 0x00014F74
	protected virtual void VirtualUpdate()
	{
	}

	// Token: 0x06000446 RID: 1094 RVA: 0x00016D76 File Offset: 0x00014F76
	public virtual float DistanceCheck(float val)
	{
		return val;
	}

	// Token: 0x06000447 RID: 1095 RVA: 0x00016D79 File Offset: 0x00014F79
	public virtual void DestinationEmpty()
	{
	}

	// Token: 0x06000448 RID: 1096 RVA: 0x00016D7B File Offset: 0x00014F7B
	public virtual void Hear(Vector3 position, int value)
	{
	}

	// Token: 0x06000449 RID: 1097 RVA: 0x00016D7D File Offset: 0x00014F7D
	public virtual void PlayerInSight(PlayerManager player)
	{
	}

	// Token: 0x0600044A RID: 1098 RVA: 0x00016D7F File Offset: 0x00014F7F
	public virtual void PlayerSighted(PlayerManager player)
	{
	}

	// Token: 0x0600044B RID: 1099 RVA: 0x00016D81 File Offset: 0x00014F81
	public virtual void PlayerLost(PlayerManager player)
	{
	}

	// Token: 0x0600044C RID: 1100 RVA: 0x00016D83 File Offset: 0x00014F83
	public void EntityTriggerEnter(Collider other)
	{
		this.behaviorStateMachine.CurrentState.OnStateTriggerEnter(other);
		this.VirtualOnTriggerEnter(other);
	}

	// Token: 0x0600044D RID: 1101 RVA: 0x00016D9D File Offset: 0x00014F9D
	protected virtual void VirtualOnTriggerEnter(Collider other)
	{
	}

	// Token: 0x0600044E RID: 1102 RVA: 0x00016D9F File Offset: 0x00014F9F
	public void EntityTriggerStay(Collider other)
	{
		this.behaviorStateMachine.CurrentState.OnStateTriggerStay(other);
		this.VirtualOnTriggerStay(other);
	}

	// Token: 0x0600044F RID: 1103 RVA: 0x00016DB9 File Offset: 0x00014FB9
	protected virtual void VirtualOnTriggerStay(Collider other)
	{
	}

	// Token: 0x06000450 RID: 1104 RVA: 0x00016DBB File Offset: 0x00014FBB
	public void EntityTriggerExit(Collider other)
	{
		this.behaviorStateMachine.CurrentState.OnStateTriggerExit(other);
		this.VirtualOnTriggerExit(other);
	}

	// Token: 0x06000451 RID: 1105 RVA: 0x00016DD5 File Offset: 0x00014FD5
	protected virtual void VirtualOnTriggerExit(Collider other)
	{
	}

	// Token: 0x06000452 RID: 1106 RVA: 0x00016DD7 File Offset: 0x00014FD7
	public void DoorHit(StandardDoor door)
	{
		this.behaviorStateMachine.CurrentState.DoorHit(door);
	}

	// Token: 0x06000453 RID: 1107 RVA: 0x00016DEA File Offset: 0x00014FEA
	public virtual void TargetPosition(Vector3 target)
	{
		this.behaviorStateMachine.ChangeNavigationState(new NavigationState_TargetPosition(this, 63, target));
	}

	// Token: 0x06000454 RID: 1108 RVA: 0x00016E00 File Offset: 0x00015000
	protected void SetGuilt(float time, string rule)
	{
		if (this.disobeying)
		{
			this.guiltTime = time;
			this.brokenRule = rule;
			return;
		}
		this.disobeying = true;
		this.guiltTime = time;
		this.brokenRule = rule;
		this.guiltTimer = this.GuiltTimer();
		base.StartCoroutine(this.guiltTimer);
	}

	// Token: 0x06000455 RID: 1109 RVA: 0x00016E52 File Offset: 0x00015052
	private IEnumerator GuiltTimer()
	{
		while (this.guiltTime > 0f)
		{
			this.guiltTime -= Time.deltaTime * this.ec.NpcTimeScale;
			yield return null;
		}
		this.ClearGuilt();
		yield break;
	}

	// Token: 0x06000456 RID: 1110 RVA: 0x00016E61 File Offset: 0x00015061
	public void ClearGuilt()
	{
		this.disobeying = false;
	}

	// Token: 0x06000457 RID: 1111 RVA: 0x00016E6A File Offset: 0x0001506A
	public virtual void SentToDetention()
	{
	}

	// Token: 0x06000458 RID: 1112 RVA: 0x00016E6C File Offset: 0x0001506C
	public void Sighted()
	{
		this.behaviorStateMachine.currentState.Sighted();
	}

	// Token: 0x06000459 RID: 1113 RVA: 0x00016E7E File Offset: 0x0001507E
	public void InPlayerSight(PlayerManager player)
	{
		this.behaviorStateMachine.currentState.InPlayerSight(player);
	}

	// Token: 0x0600045A RID: 1114 RVA: 0x00016E91 File Offset: 0x00015091
	public virtual void Unsighted()
	{
		this.behaviorStateMachine.currentState.Unsighted();
	}

	// Token: 0x0600045B RID: 1115 RVA: 0x00016EA3 File Offset: 0x000150A3
	public void MadeNavigationDecision()
	{
		this.behaviorStateMachine.currentState.MadeNavigationDecision();
	}

	// Token: 0x1700003F RID: 63
	// (get) Token: 0x0600045C RID: 1116 RVA: 0x00016EB5 File Offset: 0x000150B5
	public Character Character
	{
		get
		{
			return this.character;
		}
	}

	// Token: 0x0600045D RID: 1117 RVA: 0x00016EBD File Offset: 0x000150BD
	public virtual void Despawn()
	{
		this.ec.Npcs.Remove(this);
		Object.Destroy(base.gameObject);
	}

	// Token: 0x0600045E RID: 1118 RVA: 0x00016EDC File Offset: 0x000150DC
	public virtual void SetSpriteRotation(float degrees)
	{
		if (this._propertyBlock == null)
		{
			this._propertyBlock = new MaterialPropertyBlock();
		}
		foreach (SpriteRenderer spriteRenderer in this.spriteRenderer)
		{
			spriteRenderer.GetPropertyBlock(this._propertyBlock);
			this._propertyBlock.SetFloat("_SpriteRotation", degrees);
			spriteRenderer.SetPropertyBlock(this._propertyBlock);
		}
	}

	// Token: 0x0600045F RID: 1119 RVA: 0x00016F3C File Offset: 0x0001513C
	public void AddDetour(PlayerDetour detour)
	{
		int num = 0;
		while (num < this.playerDetours.Count && detour.priority < this.playerDetours[num].priority)
		{
			num++;
		}
		this.playerDetours.Insert(num, detour);
	}

	// Token: 0x06000460 RID: 1120 RVA: 0x00016F88 File Offset: 0x00015188
	public bool ActivateDetour(NavigationState parentState, out NavigationDetourState state)
	{
		state = null;
		for (int i = 0; i < this.playerDetours.Count; i++)
		{
			if (this.playerDetours[i].UseIfNeeded(this, parentState, out state))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000461 RID: 1121 RVA: 0x00016FCC File Offset: 0x000151CC
	public void RemoveDetour(PlayerDetour detour)
	{
		this.playerDetours.Remove(detour);
	}

	// Token: 0x17000040 RID: 64
	// (get) Token: 0x06000462 RID: 1122 RVA: 0x00016FDB File Offset: 0x000151DB
	public bool HasDetour
	{
		get
		{
			return this.playerDetours.Count > 0;
		}
	}

	// Token: 0x06000463 RID: 1123 RVA: 0x00016FEB File Offset: 0x000151EB
	public bool ContainsDetour(PlayerDetour detour)
	{
		return this.playerDetours.Contains(detour);
	}

	// Token: 0x17000041 RID: 65
	// (get) Token: 0x06000464 RID: 1124 RVA: 0x00016FF9 File Offset: 0x000151F9
	public PosterObject Poster
	{
		get
		{
			return this.poster;
		}
	}

	// Token: 0x17000042 RID: 66
	// (get) Token: 0x06000465 RID: 1125 RVA: 0x00017001 File Offset: 0x00015201
	public bool Disobeying
	{
		get
		{
			return this.disobeying;
		}
	}

	// Token: 0x17000043 RID: 67
	// (get) Token: 0x06000466 RID: 1126 RVA: 0x00017009 File Offset: 0x00015209
	public string BrokenRule
	{
		get
		{
			return this.brokenRule;
		}
	}

	// Token: 0x17000044 RID: 68
	// (get) Token: 0x06000467 RID: 1127 RVA: 0x00017011 File Offset: 0x00015211
	public bool IgnorePlayerOnSpawn
	{
		get
		{
			return this.ignorePlayerOnSpawn;
		}
	}

	// Token: 0x17000045 RID: 69
	// (get) Token: 0x06000468 RID: 1128 RVA: 0x00017019 File Offset: 0x00015219
	public bool IgnoreBelts
	{
		get
		{
			return this.ignoreBelts;
		}
	}

	// Token: 0x17000046 RID: 70
	// (get) Token: 0x06000469 RID: 1129 RVA: 0x00017021 File Offset: 0x00015221
	public float TimeScale
	{
		get
		{
			return this.ec.NpcTimeScale;
		}
	}

	// Token: 0x040004C9 RID: 1225
	[SerializeField]
	private Character character;

	// Token: 0x040004CA RID: 1226
	[SerializeField]
	protected Navigator navigator;

	// Token: 0x040004CB RID: 1227
	[SerializeField]
	private PosterObject poster;

	// Token: 0x040004CC RID: 1228
	public EnvironmentController ec;

	// Token: 0x040004CD RID: 1229
	public Looker looker;

	// Token: 0x040004CE RID: 1230
	public Collider[] baseTrigger = new Collider[0];

	// Token: 0x040004CF RID: 1231
	public GameObject spriteBase;

	// Token: 0x040004D0 RID: 1232
	public SpriteRenderer[] spriteRenderer = new SpriteRenderer[0];

	// Token: 0x040004D1 RID: 1233
	public List<PlayerManager> players = new List<PlayerManager>();

	// Token: 0x040004D2 RID: 1234
	public List<RoomCategory> spawnableRooms = new List<RoomCategory>
	{
		RoomCategory.Class,
		RoomCategory.Faculty,
		RoomCategory.Hall,
		RoomCategory.Office
	};

	// Token: 0x040004D3 RID: 1235
	public WeightedRoomAsset[] potentialRoomAssets = new WeightedRoomAsset[0];

	// Token: 0x040004D4 RID: 1236
	private RoomController currentRoom;

	// Token: 0x040004D5 RID: 1237
	private IEnumerator guiltTimer;

	// Token: 0x040004D6 RID: 1238
	private MaterialPropertyBlock _propertyBlock;

	// Token: 0x040004D7 RID: 1239
	private List<PlayerDetour> playerDetours = new List<PlayerDetour>();

	// Token: 0x040004D8 RID: 1240
	private string brokenRule;

	// Token: 0x040004D9 RID: 1241
	private float guiltTime;

	// Token: 0x040004DA RID: 1242
	private int disables;

	// Token: 0x040004DB RID: 1243
	private int normalLayer;

	// Token: 0x040004DC RID: 1244
	[SerializeField]
	protected bool ignorePlayerOnSpawn;

	// Token: 0x040004DD RID: 1245
	[SerializeField]
	protected bool ignoreBelts;

	// Token: 0x040004DE RID: 1246
	protected bool disobeying;

	// Token: 0x040004DF RID: 1247
	public NpcStateMachine behaviorStateMachine = new NpcStateMachine();

	// Token: 0x040004E0 RID: 1248
	public NavigationStateMachine navigationStateMachine = new NavigationStateMachine();
}
