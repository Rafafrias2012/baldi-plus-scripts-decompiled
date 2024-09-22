using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200002F RID: 47
public class Cumulo : NPC
{
	// Token: 0x06000115 RID: 277 RVA: 0x000074E8 File Offset: 0x000056E8
	public override void Initialize()
	{
		base.Initialize();
		this.windManager.transform.parent = this.ec.transform;
		this.windManager.Initialize();
		MeshRenderer[] array = this.windGraphics;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].sharedMaterial = this.windManager.newMaterial;
		}
		this.behaviorStateMachine.ChangeState(new Cumulo_Moving(this));
		this.windSpeed = this.windManager.Speed;
	}

	// Token: 0x06000116 RID: 278 RVA: 0x0000756C File Offset: 0x0000576C
	protected override void VirtualUpdate()
	{
		base.VirtualUpdate();
		if (base.Navigator.Entity.Squished)
		{
			this.windGraphicsParent.localScale = new Vector3(0.5f, 0.5f, 1f);
			this.windManager.SetSpeed(0f);
			return;
		}
		this.windGraphicsParent.localScale = new Vector3(1f, 1f, 1f);
		this.windManager.SetSpeed(this.windSpeed);
	}

	// Token: 0x06000117 RID: 279 RVA: 0x000075F4 File Offset: 0x000057F4
	public void FindDestination()
	{
		this.halls = this.ec.FindHallways();
		for (int i = 0; i < this.halls.Count; i++)
		{
			if (this.halls[i].Count < this.minHallLength)
			{
				this.halls.RemoveAt(i);
				i--;
			}
		}
		if (this.halls.Count > 0)
		{
			this.currentHall = this.halls[Random.Range(0, this.halls.Count)];
			if (Random.Range(0, 2) == 0)
			{
				this._tile = this.currentHall[0];
				this._oppositeTile = this.currentHall[this.currentHall.Count - 1];
			}
			else
			{
				this._tile = this.currentHall[this.currentHall.Count - 1];
				this._oppositeTile = this.currentHall[0];
			}
			if ((this._tile.position - this._oppositeTile.position).x == 0)
			{
				if ((this._tile.position - this._oppositeTile.position).z > 0)
				{
					this.dir = Direction.South;
				}
				else
				{
					this.dir = Direction.North;
				}
			}
			else if ((this._tile.position - this._oppositeTile.position).x > 0)
			{
				this.dir = Direction.West;
			}
			else
			{
				this.dir = Direction.East;
			}
			this.TargetPosition(this._tile.FloorWorldPosition);
			return;
		}
		Debug.LogWarning("Cumulo was unable to find a hallway big enough to blow in.");
	}

	// Token: 0x06000118 RID: 280 RVA: 0x00007798 File Offset: 0x00005998
	public void Blow()
	{
		this.windManager.SetDirection(this.dir);
		IntVector2 intVector = this._tile.position - this._oppositeTile.position;
		this.windManager.transform.position = new Vector3(((float)intVector.x / 2f + (float)this._oppositeTile.position.x) * 10f + 5f, 5f, ((float)intVector.z / 2f + (float)this._oppositeTile.position.z) * 10f + 5f);
		this.windManager.BoxCollider.size = new Vector3(Mathf.Abs((float)intVector.x * 10f) + 10f, 10f, Mathf.Abs((float)intVector.z * 10f) + 10f);
		foreach (MeshRenderer meshRenderer in this.windGraphics)
		{
			meshRenderer.transform.localScale = Vector3.zero + Vector3.forward + Vector3.right * 10f;
			meshRenderer.transform.localScale = meshRenderer.transform.localScale + Vector3.up * (float)(Mathf.Abs(intVector.x + intVector.z) + 1) * 10f;
		}
		this.windManager.newMaterial.SetVector("_Tiling", Vector2.one + Vector2.up * (float)Mathf.Abs(intVector.x + intVector.z));
		this.windGraphicsParent.rotation = this.dir.ToRotation();
		this.windManager.gameObject.SetActive(true);
		this.audMan.SetLoop(true);
		this.audMan.maintainLoop = true;
		this.audMan.QueueAudio(this.audBlowing);
	}

	// Token: 0x17000016 RID: 22
	// (get) Token: 0x06000119 RID: 281 RVA: 0x000079A8 File Offset: 0x00005BA8
	public float RandomBlowTime
	{
		get
		{
			return Random.Range(this.minStay, this.maxStay);
		}
	}

	// Token: 0x0600011A RID: 282 RVA: 0x000079BB File Offset: 0x00005BBB
	public void StopBlowing()
	{
		this.windManager.gameObject.SetActive(false);
		this.audMan.FlushQueue(true);
	}

	// Token: 0x04000125 RID: 293
	[SerializeField]
	private BeltManager windManager;

	// Token: 0x04000126 RID: 294
	private List<List<Cell>> halls = new List<List<Cell>>();

	// Token: 0x04000127 RID: 295
	private List<Cell> currentHall = new List<Cell>();

	// Token: 0x04000128 RID: 296
	private Cell _tile;

	// Token: 0x04000129 RID: 297
	private Cell _oppositeTile;

	// Token: 0x0400012A RID: 298
	[SerializeField]
	private Transform windGraphicsParent;

	// Token: 0x0400012B RID: 299
	[SerializeField]
	private MeshRenderer[] windGraphics = new MeshRenderer[0];

	// Token: 0x0400012C RID: 300
	[SerializeField]
	private AudioManager audMan;

	// Token: 0x0400012D RID: 301
	[SerializeField]
	private SoundObject audBlowing;

	// Token: 0x0400012E RID: 302
	private Direction dir;

	// Token: 0x0400012F RID: 303
	[SerializeField]
	private float minStay = 10f;

	// Token: 0x04000130 RID: 304
	[SerializeField]
	private float maxStay = 30f;

	// Token: 0x04000131 RID: 305
	private float windSpeed;

	// Token: 0x04000132 RID: 306
	[SerializeField]
	private int minHallLength = 5;
}
