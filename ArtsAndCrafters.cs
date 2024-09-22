using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200000A RID: 10
public class ArtsAndCrafters : NPC
{
	// Token: 0x06000036 RID: 54 RVA: 0x000039FC File Offset: 0x00001BFC
	public override void Initialize()
	{
		base.Initialize();
		this.runTime = Random.Range(this.minSightTime, this.maxSightTime);
		for (int i = 0; i < this.ec.levelSize.x; i++)
		{
			for (int j = 0; j < this.ec.levelSize.z; j++)
			{
				if (this.ec.cells[i, j] != null)
				{
					Cell cell = this.ec.cells[i, j];
					if ((cell.shape == TileShape.Corner || cell.shape == TileShape.Single || cell.shape == TileShape.End) && !cell.open)
					{
						this.spawnTiles.Add(cell);
					}
				}
			}
		}
		foreach (Cell cell2 in this.spawnTiles)
		{
			IntVector2 position = default(IntVector2);
			int num = 0;
			if (cell2.shape != TileShape.Single)
			{
				for (int k = 0; k < Directions.OpenDirectionsFromBin(cell2.NavBin).Count; k++)
				{
					Direction dir = Directions.OpenDirectionsFromBin(cell2.NavBin)[k];
					this.ec.CountConsecutiveTiles(cell2.position, dir, out num, out position);
					if (num >= this.minHallLength)
					{
						this.CreateTrigger(cell2.position, this.ec.CellFromPosition(position).FloorWorldPosition);
					}
				}
			}
			else
			{
				this.ec.CountConsecutiveTiles(cell2.position, Directions.ClosedDirectionsFromBin(cell2.NavBin)[0].GetOpposite(), out num, out position);
				if (num >= this.minHallLength)
				{
					this.CreateTrigger(cell2.position, this.ec.CellFromPosition(position).FloorWorldPosition);
				}
			}
		}
		this.state = new ArtsAndCrafters_Waiting(this);
		this.behaviorStateMachine.ChangeState(this.state);
		this.behaviorStateMachine.ChangeNavigationState(new NavigationState_DoNothing(this, 0));
	}

	// Token: 0x06000037 RID: 55 RVA: 0x00003C10 File Offset: 0x00001E10
	protected override void VirtualUpdate()
	{
		base.VirtualUpdate();
	}

	// Token: 0x06000038 RID: 56 RVA: 0x00003C18 File Offset: 0x00001E18
	public void SpawnAt(IntVector2 position)
	{
		this.state.SpawnAt(position);
	}

	// Token: 0x06000039 RID: 57 RVA: 0x00003C26 File Offset: 0x00001E26
	private void CreateTrigger(IntVector2 target, Vector3 position)
	{
		CraftersTrigger craftersTrigger = Object.Instantiate<CraftersTrigger>(this.triggerPre, this.ec.transform);
		craftersTrigger.target = target;
		craftersTrigger.crafters = this;
		craftersTrigger.spawnChance = this.spawnChance;
		craftersTrigger.transform.position = position;
	}

	// Token: 0x0600003A RID: 58 RVA: 0x00003C64 File Offset: 0x00001E64
	public void GetAngry(PlayerManager player)
	{
		this.angry = true;
		this.navigator.maxSpeed = this.angryMaxSpeed;
		this.navigator.SetSpeed(0f);
		this.navigator.accel = this.angryAccel;
		this.Hide(false);
		this.visibleRenderer.sprite = this.angrySprite;
		this.audMan.QueueAudio(this.audIntro);
		this.audMan.QueueAudio(this.audLoop);
		this.audMan.loop = true;
		this.state = new ArtsAndCrafters_Chasing(this, player);
		this.behaviorStateMachine.ChangeState(this.state);
	}

	// Token: 0x0600003B RID: 59 RVA: 0x00003D0E File Offset: 0x00001F0E
	public void RunAway()
	{
		this.running = true;
		this.navigator.SetSpeed(this.normSpeed);
		this.runTime = Random.Range(this.minSightTime, this.maxSightTime);
	}

	// Token: 0x0600003C RID: 60 RVA: 0x00003D3F File Offset: 0x00001F3F
	public void Hide(bool val)
	{
		this.hidden = val;
		this.visibleRenderer.enabled = !val;
		this.baseTrigger[0].enabled = !val;
	}

	// Token: 0x0600003D RID: 61 RVA: 0x00003D68 File Offset: 0x00001F68
	public void Teleport(IntVector2 position)
	{
		base.transform.position = this.ec.CellFromPosition(position).FloorWorldPosition + Vector3.up * 5f;
	}

	// Token: 0x0600003E RID: 62 RVA: 0x00003D9A File Offset: 0x00001F9A
	private IEnumerator Teleporter(IntVector2 position)
	{
		while (this.visibleRenderer.isVisible)
		{
			yield return null;
		}
		this.Hide(true);
		base.transform.position = this.ec.CellFromPosition(position).FloorWorldPosition + Vector3.up * 5f;
		yield return null;
		while (this.visibleRenderer.isVisible)
		{
			yield return null;
		}
		this.Hide(false);
		yield break;
	}

	// Token: 0x0600003F RID: 63 RVA: 0x00003DB0 File Offset: 0x00001FB0
	public void UpdateTeleportAnimation(PlayerManager player, Vector3 currentAngle, float echoDistance)
	{
		Vector3 a = default(Vector3);
		base.transform.position = player.transform.position + currentAngle * this.spinDistance;
		for (int i = 0; i < this.echo.Length; i++)
		{
			a = Quaternion.AngleAxis(echoDistance * (float)i * -1f, Vector3.up) * currentAngle;
			this.echo[i].transform.position = player.transform.position + a * (this.spinDistance + (float)i + 1f);
		}
	}

	// Token: 0x06000040 RID: 64 RVA: 0x00003E54 File Offset: 0x00002054
	public void Attack(PlayerManager player)
	{
		this.navigator.SetSpeed(0f);
		this.navigator.ClearDestination();
		this.visibleRenderer.gameObject.layer = 29;
		this.attacking = true;
		base.transform.position = player.transform.position + player.transform.forward * 8f;
		Transform[] array = this.echo;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].gameObject.SetActive(true);
		}
		this.audMan.FlushQueue(true);
		this.attackAudMan.SetLoop(true);
		this.attackAudMan.maintainLoop = true;
		this.attackAudMan.QueueAudio(this.audLoop);
	}

	// Token: 0x06000041 RID: 65 RVA: 0x00003F20 File Offset: 0x00002120
	public void TeleportPlayer(PlayerManager player)
	{
		List<IntVector2> list = new List<IntVector2>();
		foreach (Elevator elevator in this.ec.elevators)
		{
			if (elevator.IsOpen)
			{
				list.Add(elevator.Door.position);
			}
		}
		if (list.Count <= 0)
		{
			foreach (Elevator elevator2 in this.ec.elevators)
			{
				list.Add(elevator2.Door.position);
			}
		}
		bool flag = false;
		List<Cell> list2 = new List<Cell>();
		IntVector2 position = list[Random.Range(0, list.Count)];
		int num = 0;
		if (list.Count > 0)
		{
			while (!flag && num < 32)
			{
				bool flag2;
				this.ec.FindPath(this.ec.CellFromPosition(position), this.ec.mainHall.TileAtIndex(Random.Range(0, this.ec.mainHall.TileCount)), PathType.Nav, out list2, out flag2);
				if (flag2 && list2.Count > this.baldiSpawnDistance)
				{
					flag = true;
					list2 = new List<Cell>(list2);
				}
				num++;
			}
		}
		if (flag)
		{
			Baldi baldi = this.ec.GetBaldi();
			if (baldi != null)
			{
				baldi.ClearSoundLocations();
			}
			if (this.ec.GetBaldi() != null)
			{
				this.ec.GetBaldi().transform.position = list2[this.baldiSpawnDistance].FloorWorldPosition + Vector3.up * 5f;
			}
			player.Teleport(list2[this.playerSpawnDistance].CenterWorldPosition);
			this.ec.MakeNoise(list2[this.playerSpawnDistance].CenterWorldPosition, this.noiseValue);
		}
		this.audMan.FlushQueue(true);
		this.navigator.Entity.SetActive(false);
	}

	// Token: 0x17000001 RID: 1
	// (get) Token: 0x06000042 RID: 66 RVA: 0x00004148 File Offset: 0x00002348
	public float RunTime
	{
		get
		{
			return this.runTime;
		}
	}

	// Token: 0x17000002 RID: 2
	// (get) Token: 0x06000043 RID: 67 RVA: 0x00004150 File Offset: 0x00002350
	public float AngryTime
	{
		get
		{
			return this.angerTime;
		}
	}

	// Token: 0x17000003 RID: 3
	// (get) Token: 0x06000044 RID: 68 RVA: 0x00004158 File Offset: 0x00002358
	public float AttackTime
	{
		get
		{
			return this.attackTime;
		}
	}

	// Token: 0x17000004 RID: 4
	// (get) Token: 0x06000045 RID: 69 RVA: 0x00004160 File Offset: 0x00002360
	public float AttackSpinSpeed
	{
		get
		{
			return this.attackSpinSpeed;
		}
	}

	// Token: 0x17000005 RID: 5
	// (get) Token: 0x06000046 RID: 70 RVA: 0x00004168 File Offset: 0x00002368
	public float AttackSpinAccel
	{
		get
		{
			return this.attackSpinAccel;
		}
	}

	// Token: 0x17000006 RID: 6
	// (get) Token: 0x06000047 RID: 71 RVA: 0x00004170 File Offset: 0x00002370
	public float EchoIncrease
	{
		get
		{
			return this.echoIncrease;
		}
	}

	// Token: 0x17000007 RID: 7
	// (get) Token: 0x06000048 RID: 72 RVA: 0x00004178 File Offset: 0x00002378
	public float MaxEchoDistance
	{
		get
		{
			return this.maxEchoDistance;
		}
	}

	// Token: 0x17000008 RID: 8
	// (get) Token: 0x06000049 RID: 73 RVA: 0x00004180 File Offset: 0x00002380
	public bool Jealous
	{
		get
		{
			return Singleton<BaseGameManager>.Instance.FoundNotebooks >= Singleton<BaseGameManager>.Instance.NotebookTotal;
		}
	}

	// Token: 0x04000060 RID: 96
	public ArtsAndCrafters_StateBase state;

	// Token: 0x04000061 RID: 97
	[SerializeField]
	private SpriteRenderer visibleRenderer;

	// Token: 0x04000062 RID: 98
	[SerializeField]
	private AudioManager audMan;

	// Token: 0x04000063 RID: 99
	[SerializeField]
	private AudioManager attackAudMan;

	// Token: 0x04000064 RID: 100
	[SerializeField]
	private SoundObject audIntro;

	// Token: 0x04000065 RID: 101
	[SerializeField]
	private SoundObject audLoop;

	// Token: 0x04000066 RID: 102
	[SerializeField]
	private Sprite angrySprite;

	// Token: 0x04000067 RID: 103
	[SerializeField]
	private Transform[] echo = new Transform[0];

	// Token: 0x04000068 RID: 104
	private Transform target;

	// Token: 0x04000069 RID: 105
	[SerializeField]
	private float minSightTime = 1f;

	// Token: 0x0400006A RID: 106
	[SerializeField]
	private float maxSightTime = 6f;

	// Token: 0x0400006B RID: 107
	[SerializeField]
	private float angerTime = 1f;

	// Token: 0x0400006C RID: 108
	[SerializeField]
	private float normSpeed = 50f;

	// Token: 0x0400006D RID: 109
	[SerializeField]
	private float angryAccel = 10f;

	// Token: 0x0400006E RID: 110
	[SerializeField]
	private float angryMaxSpeed = 1000f;

	// Token: 0x0400006F RID: 111
	[SerializeField]
	private float attackSpinSpeed = 10f;

	// Token: 0x04000070 RID: 112
	[SerializeField]
	private float attackSpinAccel = 25f;

	// Token: 0x04000071 RID: 113
	[SerializeField]
	private float attackTime = 5f;

	// Token: 0x04000072 RID: 114
	[SerializeField]
	private float spawnChance = 0.2f;

	// Token: 0x04000073 RID: 115
	[SerializeField]
	private float spinDistance = 10f;

	// Token: 0x04000074 RID: 116
	[SerializeField]
	private float echoIncrease;

	// Token: 0x04000075 RID: 117
	[SerializeField]
	private float maxEchoDistance = 45f;

	// Token: 0x04000076 RID: 118
	private float timeInView;

	// Token: 0x04000077 RID: 119
	private float runTime;

	// Token: 0x04000078 RID: 120
	private float timeLooked;

	// Token: 0x04000079 RID: 121
	[SerializeField]
	private int minHallLength = 10;

	// Token: 0x0400007A RID: 122
	[SerializeField]
	private int playerSpawnDistance = 10;

	// Token: 0x0400007B RID: 123
	[SerializeField]
	private int baldiSpawnDistance = 18;

	// Token: 0x0400007C RID: 124
	[SerializeField]
	private int noiseValue = 64;

	// Token: 0x0400007D RID: 125
	[SerializeField]
	private CraftersTrigger triggerPre;

	// Token: 0x0400007E RID: 126
	private IEnumerator teleporter;

	// Token: 0x0400007F RID: 127
	private List<Cell> spawnTiles = new List<Cell>();

	// Token: 0x04000080 RID: 128
	private Dictionary<IntVector2, IntVector2> triggersToPositions = new Dictionary<IntVector2, IntVector2>();

	// Token: 0x04000081 RID: 129
	private List<IntVector2> triggerPositions = new List<IntVector2>();

	// Token: 0x04000082 RID: 130
	private bool angry;

	// Token: 0x04000083 RID: 131
	private bool attacking;

	// Token: 0x04000084 RID: 132
	private bool running;

	// Token: 0x04000085 RID: 133
	private bool hidden;
}
