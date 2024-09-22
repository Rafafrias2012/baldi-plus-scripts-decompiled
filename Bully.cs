using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x02000025 RID: 37
public class Bully : NPC
{
	// Token: 0x060000DD RID: 221 RVA: 0x0000624C File Offset: 0x0000444C
	public override void Initialize()
	{
		base.Initialize();
		EnvironmentController ec = this.ec;
		ec.tempOpenBully = (EnvironmentController.TempObstacleManagement)Delegate.Combine(ec.tempOpenBully, new EnvironmentController.TempObstacleManagement(this.TempOpen));
		EnvironmentController ec2 = this.ec;
		ec2.tempCloseBully = (EnvironmentController.TempObstacleManagement)Delegate.Combine(ec2.tempCloseBully, new EnvironmentController.TempObstacleManagement(this.TempClose));
		this.behaviorStateMachine.ChangeState(new Bully_Wait(this, this, Random.Range(this.minDelay, this.maxDelay)));
		this.navigator.Entity.SetFrozen(true);
		this.Hide();
		this.raycastBlocker.transform.SetParent(base.transform.parent);
		this.looker.IgnoreTransform(this.raycastBlocker.transform);
	}

	// Token: 0x060000DE RID: 222 RVA: 0x00006318 File Offset: 0x00004518
	protected override void VirtualUpdate()
	{
		base.VirtualUpdate();
		this.raycastBlocker.transform.position = base.transform.position;
	}

	// Token: 0x060000DF RID: 223 RVA: 0x0000633B File Offset: 0x0000453B
	public void Taunt()
	{
		this.audMan.PlaySingle(this.callouts[Random.Range(0, this.callouts.Length)]);
	}

	// Token: 0x060000E0 RID: 224 RVA: 0x0000635D File Offset: 0x0000455D
	public void SetGuilty()
	{
		base.SetGuilt(10f, "Bullying");
	}

	// Token: 0x060000E1 RID: 225 RVA: 0x0000636F File Offset: 0x0000456F
	public override void SentToDetention()
	{
		base.SentToDetention();
		this.Hide();
	}

	// Token: 0x060000E2 RID: 226 RVA: 0x00006380 File Offset: 0x00004580
	public void Spawn()
	{
		this.tiles.Clear();
		for (int i = 0; i < this.ec.levelSize.x; i++)
		{
			for (int j = 0; j < this.ec.levelSize.z; j++)
			{
				if (!this.ec.cells[i, j].Null && this.ec.cells[i, j].room.type == RoomType.Hall && !this.ec.cells[i, j].open && !this.ec.cells[i, j].HasAnyHardCoverage)
				{
					this.tiles.Add(this.ec.cells[i, j]);
				}
			}
		}
		this.spawn = this.tiles[Random.Range(0, this.tiles.Count)];
		bool flag = false;
		while (!flag)
		{
			this.spawn = this.tiles[Random.Range(0, this.tiles.Count)];
			flag = true;
			int num = 0;
			while (num < Singleton<CoreGameManager>.Instance.setPlayers && flag)
			{
				if ((this.spawn.FloorWorldPosition + Vector3.up * 5f - Singleton<CoreGameManager>.Instance.GetPlayer(num).transform.position).magnitude <= this.playerBuffer || this.ec.TrapCheck(this.spawn))
				{
					flag = false;
				}
				num++;
			}
		}
		base.transform.position = this.spawn.FloorWorldPosition + Vector3.up * 5f;
		this.BlockSurroundingCells(true);
		this.SetComponents(true);
		this.behaviorStateMachine.ChangeState(new Bully_Active(this, this, this.maxStay));
		Physics.SyncTransforms();
		this.hidden = false;
	}

	// Token: 0x060000E3 RID: 227 RVA: 0x00006588 File Offset: 0x00004788
	public void Hide()
	{
		base.ClearGuilt();
		if (this.spawn != null)
		{
			this.BlockSurroundingCells(false);
		}
		this.SetComponents(false);
		this.behaviorStateMachine.ChangeState(new Bully_Wait(this, this, Random.Range(this.minDelay, this.maxDelay)));
		Physics.SyncTransforms();
		this.hidden = true;
	}

	// Token: 0x060000E4 RID: 228 RVA: 0x000065E0 File Offset: 0x000047E0
	private void BlockSurroundingCells(bool block)
	{
		for (int i = 0; i < 4; i++)
		{
			if (this.spawn.ConstNavigable((Direction)i))
			{
				this.ec.CellFromPosition(this.spawn.position + ((Direction)i).ToIntVector2()).Block(((Direction)i).GetOpposite(), block);
			}
		}
	}

	// Token: 0x060000E5 RID: 229 RVA: 0x00006634 File Offset: 0x00004834
	private void TempOpen()
	{
		if (!this.hidden)
		{
			this.ec.FreezeNavigationUpdates(true);
			this.BlockSurroundingCells(false);
			this.ec.FreezeNavigationUpdates(false);
		}
	}

	// Token: 0x060000E6 RID: 230 RVA: 0x0000665D File Offset: 0x0000485D
	private void TempClose()
	{
		if (!this.hidden)
		{
			this.ec.FreezeNavigationUpdates(true);
			this.BlockSurroundingCells(true);
			this.ec.FreezeNavigationUpdates(false);
		}
	}

	// Token: 0x060000E7 RID: 231 RVA: 0x00006688 File Offset: 0x00004888
	public void SetComponents(bool visible)
	{
		Collider[] array = this.collidersToHide;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = visible;
		}
		this.spriteToHide.enabled = visible;
		this.obstacleToHide.enabled = visible;
	}

	// Token: 0x060000E8 RID: 232 RVA: 0x000066CC File Offset: 0x000048CC
	public void StealItem(PlayerManager pm)
	{
		this.slotsToSteal.Clear();
		for (int i = 0; i < 6; i++)
		{
			if (!this.itemsToReject.Contains(pm.itm.items[i].itemType))
			{
				this.slotsToSteal.Add(i);
			}
		}
		if (this.slotsToSteal.Count > 0)
		{
			pm.itm.RemoveItem(this.slotsToSteal[Random.Range(0, this.slotsToSteal.Count)]);
			this.Hide();
			this.audMan.PlaySingle(this.takeouts[Random.Range(0, this.takeouts.Length)]);
			return;
		}
		this.Push(pm.plm.Entity);
		this.audMan.PlaySingle(this.noItems);
	}

	// Token: 0x060000E9 RID: 233 RVA: 0x0000679C File Offset: 0x0000499C
	public void Push(Entity entity)
	{
		entity.AddForce(new Force((entity.transform.position - base.transform.position).normalized, this.pushSpeed, this.pushAcceleration));
	}

	// Token: 0x060000EA RID: 234 RVA: 0x000067E3 File Offset: 0x000049E3
	public void ExpressBoredom()
	{
		this.audMan.PlaySingle(this.bored);
	}

	// Token: 0x060000EB RID: 235 RVA: 0x000067F8 File Offset: 0x000049F8
	private void OnDestroy()
	{
		EnvironmentController ec = this.ec;
		ec.tempOpenBully = (EnvironmentController.TempObstacleManagement)Delegate.Remove(ec.tempOpenBully, new EnvironmentController.TempObstacleManagement(this.TempOpen));
		EnvironmentController ec2 = this.ec;
		ec2.tempCloseBully = (EnvironmentController.TempObstacleManagement)Delegate.Remove(ec2.tempCloseBully, new EnvironmentController.TempObstacleManagement(this.TempClose));
	}

	// Token: 0x040000F0 RID: 240
	[SerializeField]
	private AudioManager audMan;

	// Token: 0x040000F1 RID: 241
	[SerializeField]
	private Collider[] collidersToHide = new Collider[0];

	// Token: 0x040000F2 RID: 242
	[SerializeField]
	private SpriteRenderer spriteToHide = new SpriteRenderer();

	// Token: 0x040000F3 RID: 243
	[SerializeField]
	private NavMeshObstacle obstacleToHide = new NavMeshObstacle();

	// Token: 0x040000F4 RID: 244
	[SerializeField]
	private SoundObject[] callouts = new SoundObject[0];

	// Token: 0x040000F5 RID: 245
	[SerializeField]
	private SoundObject[] takeouts = new SoundObject[0];

	// Token: 0x040000F6 RID: 246
	[SerializeField]
	private SoundObject noItems;

	// Token: 0x040000F7 RID: 247
	[SerializeField]
	private SoundObject bored;

	// Token: 0x040000F8 RID: 248
	private List<Cell> tiles = new List<Cell>();

	// Token: 0x040000F9 RID: 249
	private Cell spawn;

	// Token: 0x040000FA RID: 250
	[SerializeField]
	private GameObject raycastBlocker;

	// Token: 0x040000FB RID: 251
	[SerializeField]
	private List<Items> itemsToReject = new List<Items>();

	// Token: 0x040000FC RID: 252
	private List<int> slotsToSteal = new List<int>();

	// Token: 0x040000FD RID: 253
	[SerializeField]
	private float minDelay = 60f;

	// Token: 0x040000FE RID: 254
	[SerializeField]
	private float maxDelay = 180f;

	// Token: 0x040000FF RID: 255
	[SerializeField]
	private float maxStay = 180f;

	// Token: 0x04000100 RID: 256
	[SerializeField]
	private float playerBuffer = 20f;

	// Token: 0x04000101 RID: 257
	[SerializeField]
	private float pushSpeed = 5f;

	// Token: 0x04000102 RID: 258
	[SerializeField]
	private float pushAcceleration = -5f;

	// Token: 0x04000103 RID: 259
	private bool hidden;
}
