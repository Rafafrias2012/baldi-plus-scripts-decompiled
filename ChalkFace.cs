using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000029 RID: 41
public class ChalkFace : NPC
{
	// Token: 0x060000F5 RID: 245 RVA: 0x00006A6C File Offset: 0x00004C6C
	public override void Initialize()
	{
		base.Initialize();
		this.state = new ChalkFace_Idle(this);
		this.behaviorStateMachine.ChangeState(this.state);
		this.behaviorStateMachine.ChangeNavigationState(new NavigationState_Disabled(this));
		List<RoomController> list = new List<RoomController>();
		List<RoomController> list2 = new List<RoomController>();
		foreach (RoomController roomController in this.ec.rooms)
		{
			if (roomController.category == RoomCategory.Class)
			{
				list.Add(roomController);
			}
			else if (roomController.category == RoomCategory.Faculty)
			{
				list2.Add(roomController);
			}
		}
		int num = Mathf.RoundToInt((float)list.Count * (this.classSpawnPercent / 100f));
		int num2 = Mathf.RoundToInt((float)list2.Count * (this.facultySpawnPercent / 100f));
		int num3 = 0;
		while (num3 < num && list.Count > 0)
		{
			int index = Random.Range(0, list.Count);
			this.SpawnBoard(list[index]);
			list.RemoveAt(index);
			num3++;
		}
		num3 = 0;
		while (num3 < num2 && list2.Count > 0)
		{
			int index2 = Random.Range(0, list2.Count);
			this.SpawnBoard(list2[index2]);
			list2.RemoveAt(index2);
			num3++;
		}
	}

	// Token: 0x060000F6 RID: 246 RVA: 0x00006BD4 File Offset: 0x00004DD4
	public bool AdvanceTimer()
	{
		this.charge += Time.deltaTime * this.ec.NpcTimeScale;
		if (this.setTime - this.charge <= 15f && !this.chargeSoundPlaying)
		{
			this.audMan.QueueAudio(this.audSpawn);
			this.audMan.audioDevice.time = this.setTime - (this.setTime - this.charge);
			this.chargeSoundPlaying = true;
		}
		this.UpdateSprite();
		return this.charge >= this.setTime;
	}

	// Token: 0x060000F7 RID: 247 RVA: 0x00006C70 File Offset: 0x00004E70
	public void DecreaseTimer()
	{
		this.charge = Mathf.Max(this.charge - this.unchargeRate * Time.deltaTime * this.ec.NpcTimeScale, 0f);
		this.audMan.FlushQueue(true);
		this.chargeSoundPlaying = false;
		this.UpdateSprite();
	}

	// Token: 0x060000F8 RID: 248 RVA: 0x00006CC5 File Offset: 0x00004EC5
	private void UpdateSprite()
	{
		this.spriteColor.a = this.charge / this.setTime;
		this.chalkRenderer.color = this.spriteColor;
	}

	// Token: 0x060000F9 RID: 249 RVA: 0x00006CF0 File Offset: 0x00004EF0
	public void Activate(RoomController room)
	{
		this.charge = 0f;
		this.chargeSoundPlaying = false;
		this.totalAcceleration = 0f;
		this.chalkRenderer.gameObject.SetActive(false);
		this.flyingRenderer.gameObject.SetActive(true);
		foreach (Door door in room.doors)
		{
			door.Shut();
			door.LockTimed(this.lockTime);
		}
		this.ec.MakeNoise(base.transform.position, this.noiseVal);
		this.audMan.QueueAudio(this.audLaugh);
		this.audMan.SetLoop(true);
	}

	// Token: 0x060000FA RID: 250 RVA: 0x00006DC4 File Offset: 0x00004FC4
	public void AdvanceLaughter(RoomController room, float acceleration)
	{
		this.totalAcceleration += acceleration * Time.deltaTime;
		if (acceleration > 0f)
		{
			base.transform.RotateAround(this.ec.RealRoomMid(room), Vector3.up, Time.deltaTime * (this.spinSpeed + this.totalAcceleration) * this.ec.NpcTimeScale);
			base.transform.position += (this.ec.RealRoomMid(room) + Vector3.up * 15f - base.transform.position).normalized * this.approachSpeed * Time.deltaTime * this.ec.NpcTimeScale;
			return;
		}
		base.transform.RotateAround(this.ec.RealRoomMid(room), Vector3.up, Time.deltaTime * this.spinSpeed * this.ec.NpcTimeScale);
		base.transform.position += (this.ec.RealRoomMid(room) + Vector3.up * 5f - base.transform.position).normalized * this.approachSpeed * Time.deltaTime * this.ec.NpcTimeScale;
	}

	// Token: 0x060000FB RID: 251 RVA: 0x00006F48 File Offset: 0x00005148
	private void SpawnBoard(RoomController room)
	{
		this._potentialSpawns = room.GetTilesOfShape(this.tileShapes, true);
		bool flag = false;
		while (!flag && this._potentialSpawns.Count > 0)
		{
			Cell cell = this._potentialSpawns[Random.Range(0, this._potentialSpawns.Count)];
			if (cell.HasFreeWall)
			{
				Object.Instantiate<Chalkboard>(this.chalkboardPre, cell.TileTransform).Spawn(cell, this);
				flag = true;
			}
			else
			{
				this._potentialSpawns.Remove(cell);
			}
		}
	}

	// Token: 0x060000FC RID: 252 RVA: 0x00006FD0 File Offset: 0x000051D0
	public void Teleport(Chalkboard chalkboard)
	{
		base.transform.position = chalkboard.Position + Vector3.up * 5f;
		base.transform.rotation = chalkboard.Rotation;
		this.chalkRenderer.gameObject.SetActive(true);
		this.flyingRenderer.gameObject.SetActive(false);
	}

	// Token: 0x060000FD RID: 253 RVA: 0x00007038 File Offset: 0x00005238
	public void Cancel()
	{
		this.chalkRenderer.gameObject.SetActive(false);
		this.flyingRenderer.gameObject.SetActive(false);
		this.audMan.FlushQueue(true);
		this.chargeSoundPlaying = false;
		this.chalkRenderer.color = Color.clear;
	}

	// Token: 0x17000012 RID: 18
	// (get) Token: 0x060000FE RID: 254 RVA: 0x0000708A File Offset: 0x0000528A
	public float LockTime
	{
		get
		{
			return this.lockTime;
		}
	}

	// Token: 0x17000013 RID: 19
	// (get) Token: 0x060000FF RID: 255 RVA: 0x00007092 File Offset: 0x00005292
	public float Acceleration
	{
		get
		{
			return this.acceleration;
		}
	}

	// Token: 0x04000108 RID: 264
	public ChalkFace_StateBase state;

	// Token: 0x04000109 RID: 265
	[SerializeField]
	private Chalkboard chalkboardPre;

	// Token: 0x0400010A RID: 266
	[SerializeField]
	private List<TileShape> tileShapes = new List<TileShape>();

	// Token: 0x0400010B RID: 267
	private List<Cell> _potentialSpawns = new List<Cell>();

	// Token: 0x0400010C RID: 268
	[SerializeField]
	private SpriteRenderer chalkRenderer;

	// Token: 0x0400010D RID: 269
	[SerializeField]
	private SpriteRenderer flyingRenderer;

	// Token: 0x0400010E RID: 270
	private Color spriteColor = Color.white;

	// Token: 0x0400010F RID: 271
	[SerializeField]
	private AudioManager audMan;

	// Token: 0x04000110 RID: 272
	[SerializeField]
	private SoundObject audLaugh;

	// Token: 0x04000111 RID: 273
	[SerializeField]
	private SoundObject audSpawn;

	// Token: 0x04000112 RID: 274
	[SerializeField]
	private float setTime = 15f;

	// Token: 0x04000113 RID: 275
	[SerializeField]
	private float unchargeRate = 0.25f;

	// Token: 0x04000114 RID: 276
	[SerializeField]
	private float lockTime = 15f;

	// Token: 0x04000115 RID: 277
	[SerializeField]
	private float classSpawnPercent = 70f;

	// Token: 0x04000116 RID: 278
	[SerializeField]
	private float facultySpawnPercent = 25f;

	// Token: 0x04000117 RID: 279
	[SerializeField]
	private float spinSpeed = 90f;

	// Token: 0x04000118 RID: 280
	[SerializeField]
	private float approachSpeed = 10f;

	// Token: 0x04000119 RID: 281
	[SerializeField]
	private float acceleration = 10f;

	// Token: 0x0400011A RID: 282
	private float charge;

	// Token: 0x0400011B RID: 283
	private float totalAcceleration;

	// Token: 0x0400011C RID: 284
	[SerializeField]
	[Range(0f, 127f)]
	private int noiseVal = 63;

	// Token: 0x0400011D RID: 285
	private bool chargeSoundPlaying;
}
