using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200006B RID: 107
public class FloodEvent : RandomEvent
{
	// Token: 0x06000253 RID: 595 RVA: 0x0000CBE4 File Offset: 0x0000ADE4
	public override void Initialize(EnvironmentController controller, Random rng)
	{
		base.Initialize(controller, rng);
		this.water = Object.Instantiate<Transform>(this.waterPre, base.transform);
		this.water.gameObject.SetActive(false);
		this.underwaterFog.color = Color.cyan;
		this.underwaterFog.startDist = 0f;
		this.underwaterFog.maxDist = 30f;
		this.underwaterFog.strength = 1f;
		this.underwaterFog.priority = 63;
	}

	// Token: 0x06000254 RID: 596 RVA: 0x0000CC70 File Offset: 0x0000AE70
	private void Update()
	{
		if (this.water.gameObject.activeSelf)
		{
			this.ChangeUnderwateState(Singleton<CoreGameManager>.Instance.GetCamera(0).transform.position.y < this.water.transform.position.y);
		}
	}

	// Token: 0x06000255 RID: 597 RVA: 0x0000CCC8 File Offset: 0x0000AEC8
	private void ChangeUnderwateState(bool underwater)
	{
		if (underwater)
		{
			if (!this.cameraUnderWater)
			{
				Singleton<CoreGameManager>.Instance.GetPlayer(0).SetInvisible(true);
				this.ec.AddFog(this.underwaterFog);
				this.audMan.FlushQueue(true);
				this.audMan.QueueAudio(this.audUnderwater);
				this.audMan.SetLoop(true);
				this.cameraUnderWater = true;
				return;
			}
		}
		else if (this.cameraUnderWater)
		{
			Singleton<CoreGameManager>.Instance.GetPlayer(0).SetInvisible(false);
			this.ec.RemoveFog(this.underwaterFog);
			this.audMan.FlushQueue(false);
			this.audMan.QueueAudio(this.audWater);
			this.audMan.SetLoop(true);
			this.cameraUnderWater = false;
		}
	}

	// Token: 0x06000256 RID: 598 RVA: 0x0000CD90 File Offset: 0x0000AF90
	public override void Begin()
	{
		base.Begin();
		this.water.gameObject.SetActive(true);
		base.StartCoroutine(this.Move());
		for (int i = 0; i < Singleton<CoreGameManager>.Instance.setPlayers; i++)
		{
			Singleton<CoreGameManager>.Instance.GetPlayer(i).Am.moveMods.Add(this.moveMod);
		}
		foreach (NPC npc in this.ec.Npcs)
		{
			npc.Navigator.Am.moveMods.Add(this.moveMod);
		}
		this.audMan.QueueAudio(this.audWater);
		this.audMan.SetLoop(true);
		this.audMan.audioDevice.volume = 0f;
	}

	// Token: 0x06000257 RID: 599 RVA: 0x0000CE88 File Offset: 0x0000B088
	private void SpawnWhirlpool(Cell cell)
	{
		Whirlpool whirlpool = Object.Instantiate<Whirlpool>(this.whirlpoolPre);
		whirlpool.ec = this.ec;
		whirlpool.transform.position = cell.FloorWorldPosition;
		whirlpool.whirlpoolPre = this.whirlpoolPre;
		whirlpool.flood = this;
		this.whirlpools.Add(whirlpool);
	}

	// Token: 0x06000258 RID: 600 RVA: 0x0000CEE0 File Offset: 0x0000B0E0
	public override void Pause()
	{
		for (int i = 0; i < Singleton<CoreGameManager>.Instance.setPlayers; i++)
		{
			Singleton<CoreGameManager>.Instance.GetPlayer(i).Am.moveMods.Remove(this.moveMod);
		}
		this.ChangeUnderwateState(false);
		base.Pause();
		this.audMan.Pause(true);
	}

	// Token: 0x06000259 RID: 601 RVA: 0x0000CF3C File Offset: 0x0000B13C
	public override void Unpause()
	{
		for (int i = 0; i < Singleton<CoreGameManager>.Instance.setPlayers; i++)
		{
			Singleton<CoreGameManager>.Instance.GetPlayer(i).Am.moveMods.Add(this.moveMod);
		}
		base.Unpause();
		this.audMan.Pause(false);
	}

	// Token: 0x0600025A RID: 602 RVA: 0x0000CF90 File Offset: 0x0000B190
	private IEnumerator Move()
	{
		this.water.transform.position = Vector3.zero;
		Vector3 position = this.water.transform.position;
		bool settled = false;
		float timeBetweenDoors = 0.5f;
		float timeUntilDoor = timeBetweenDoors;
		int currentDoor = 0;
		this.volume = this.audWater.volumeMultiplier;
		using (List<RoomController>.Enumerator enumerator = this.ec.rooms.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				RoomController roomController = enumerator.Current;
				foreach (Door item in roomController.doors)
				{
					if (!this.doors.Contains(item))
					{
						this.doors.Add(item);
					}
				}
			}
			goto IL_31C;
		}
		IL_107:
		this.MoveWater();
		if (this.water.transform.position.y < this.height)
		{
			this.water.transform.position += Vector3.up * this.riseSpeed * Time.deltaTime * this.ec.EnvironmentTimeScale;
			this.audMan.audioDevice.volume = this.water.transform.position.y / this.height * this.volume;
		}
		else if (!settled)
		{
			position = this.water.transform.position;
			position.y = this.height;
			this.water.transform.position = position;
			this.audMan.audioDevice.volume = this.volume;
			int num = this.crng.Next(this.minWhirlpools, this.maxWhirlpools);
			List<Cell> list = this.ec.AllTilesNoGarbage(false, true);
			list.ConvertEntityUnsafeCells();
			int num2 = 0;
			while (num2 < num && list.Count > 0)
			{
				int index = this.crng.Next(0, list.Count);
				this.SpawnWhirlpool(list[index]);
				IntVector2 position2 = list[index].position;
				list.RemoveAt(index);
				list.RemoveDuplicateCells(position2);
				num2++;
			}
			settled = true;
		}
		if (timeUntilDoor > 0f)
		{
			timeUntilDoor -= Time.deltaTime * this.ec.EnvironmentTimeScale;
		}
		else if (currentDoor < this.doors.Count)
		{
			this.doors[currentDoor].OpenTimed(base.EventTime, false);
			currentDoor++;
			timeUntilDoor = timeBetweenDoors;
		}
		yield return null;
		IL_31C:
		if (this.active)
		{
			goto IL_107;
		}
		for (int i = 0; i < Singleton<CoreGameManager>.Instance.setPlayers; i++)
		{
			Singleton<CoreGameManager>.Instance.GetPlayer(i).Am.moveMods.Remove(this.moveMod);
		}
		foreach (NPC npc in this.ec.Npcs)
		{
			npc.Navigator.Am.moveMods.Remove(this.moveMod);
		}
		using (List<Whirlpool>.Enumerator enumerator4 = this.whirlpools.GetEnumerator())
		{
			while (enumerator4.MoveNext())
			{
				Whirlpool whirlpool = enumerator4.Current;
				if (whirlpool != null)
				{
					whirlpool.ClearMoveMods();
					Object.Destroy(whirlpool.gameObject);
				}
			}
			goto IL_487;
		}
		IL_402:
		this.MoveWater();
		this.water.transform.position -= Vector3.up * this.riseSpeed * Time.deltaTime;
		this.audMan.audioDevice.volume = this.water.transform.position.y / this.height * this.volume;
		yield return null;
		IL_487:
		if (this.water.transform.position.y <= 0f)
		{
			this.audMan.FlushQueue(true);
			this.water.gameObject.SetActive(false);
			this.ChangeUnderwateState(false);
			yield break;
		}
		goto IL_402;
	}

	// Token: 0x0600025B RID: 603 RVA: 0x0000CFA0 File Offset: 0x0000B1A0
	private void MoveWater()
	{
		this.water.transform.position += this.speed * Time.deltaTime * this.ec.EnvironmentTimeScale;
		if (this.water.transform.position.x > this.limit.x)
		{
			this.water.transform.position -= Vector3.right * this.limit.x;
		}
		if (this.water.transform.position.z > this.limit.y)
		{
			this.water.transform.position -= Vector3.forward * this.limit.y;
		}
	}

	// Token: 0x04000259 RID: 601
	[SerializeField]
	private Whirlpool whirlpoolPre;

	// Token: 0x0400025A RID: 602
	public List<Whirlpool> whirlpools = new List<Whirlpool>();

	// Token: 0x0400025B RID: 603
	[SerializeField]
	private AudioManager audMan;

	// Token: 0x0400025C RID: 604
	[SerializeField]
	private SoundObject audWater;

	// Token: 0x0400025D RID: 605
	[SerializeField]
	private SoundObject audUnderwater;

	// Token: 0x0400025E RID: 606
	[SerializeField]
	private Transform waterPre;

	// Token: 0x0400025F RID: 607
	[SerializeField]
	private Transform water;

	// Token: 0x04000260 RID: 608
	private Fog underwaterFog = new Fog();

	// Token: 0x04000261 RID: 609
	[SerializeField]
	private MovementModifier moveMod;

	// Token: 0x04000262 RID: 610
	[SerializeField]
	private List<Door> doors = new List<Door>();

	// Token: 0x04000263 RID: 611
	[SerializeField]
	private Vector3 speed = new Vector3(5f, 0f, 5f);

	// Token: 0x04000264 RID: 612
	[SerializeField]
	private Vector2 limit = new Vector2(25f, 25f);

	// Token: 0x04000265 RID: 613
	[SerializeField]
	private float height = 3.1f;

	// Token: 0x04000266 RID: 614
	[SerializeField]
	private float riseSpeed = 1f;

	// Token: 0x04000267 RID: 615
	private float volume = 1f;

	// Token: 0x04000268 RID: 616
	[SerializeField]
	private int minWhirlpools = 2;

	// Token: 0x04000269 RID: 617
	[SerializeField]
	private int maxWhirlpools = 6;

	// Token: 0x0400026A RID: 618
	private bool cameraUnderWater;
}
