using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200006E RID: 110
public class GravityEvent : RandomEvent
{
	// Token: 0x06000270 RID: 624 RVA: 0x0000D800 File Offset: 0x0000BA00
	public override void Begin()
	{
		base.Begin();
		this.tiles = new List<Cell>(this.ec.mainHall.AllTilesNoGarbage(false, true));
		PlayerDetour playerDetour = this.npcDetour;
		playerDetour.UseIfNeeded = (PlayerDetour.CheckIfNeeded)Delegate.Combine(playerDetour.UseIfNeeded, new PlayerDetour.CheckIfNeeded(this.CalculatePlayerDetour));
		foreach (NPC item in this.ec.Npcs)
		{
			this.npcs.Add(item);
			this.npcFlipped.Add(false);
		}
		foreach (NPC npc in this.npcs)
		{
			this.FlipNPC(npc);
		}
		this.FlipPlayer();
		base.StartCoroutine(this.InitialSpawnTimer(this.initialSpawnDelay));
	}

	// Token: 0x06000271 RID: 625 RVA: 0x0000D910 File Offset: 0x0000BB10
	public override void End()
	{
		while (this.flippers.Count > 0)
		{
			this.DestroyFlipper(this.flippers[0]);
		}
		if (this.playerFlipped)
		{
			this.FlipPlayer();
		}
		else if (!this.playerSpinning)
		{
			Singleton<CoreGameManager>.Instance.GetCamera(0).cameraModifiers.Remove(this.camMod);
			Singleton<CoreGameManager>.Instance.GetPlayer(0).camMods.Remove(this.camMod);
		}
		for (int i = 0; i < this.npcs.Count; i++)
		{
			if (this.npcFlipped[i])
			{
				this.FlipNPC(this.npcs[i]);
			}
		}
		foreach (NPC npc in this.npcs)
		{
			npc.Navigator.Entity != null;
		}
		base.End();
	}

	// Token: 0x06000272 RID: 626 RVA: 0x0000DA1C File Offset: 0x0000BC1C
	public override void ResetConditions()
	{
		base.ResetConditions();
		if (this.playerFlipped)
		{
			this.FlipPlayer();
		}
	}

	// Token: 0x06000273 RID: 627 RVA: 0x0000DA34 File Offset: 0x0000BC34
	public void FlipPlayer()
	{
		if (base.Active)
		{
			this.playerFlipped = !this.playerFlipped;
			Singleton<CoreGameManager>.Instance.GetPlayer(0).Reverse();
			Singleton<SubtitleManager>.Instance.Reverse();
			this.rotationLeft += 180f;
			this.UpdateCollision();
			if (!this.playerSpinning)
			{
				base.StartCoroutine(this.FlipAnimation());
			}
		}
	}

	// Token: 0x06000274 RID: 628 RVA: 0x0000DA9F File Offset: 0x0000BC9F
	private IEnumerator FlipAnimation()
	{
		this.playerSpinning = true;
		while (this.rotationLeft > 0f)
		{
			yield return null;
			this.playerRotation += Time.deltaTime * this.ec.EnvironmentTimeScale * this.flipSpeed;
			this.rotationLeft -= Time.deltaTime * this.ec.EnvironmentTimeScale * this.flipSpeed;
			this.camMod.rotOffset.z = this.playerRotation;
			Singleton<CoreGameManager>.Instance.GetPlayer(0).plm.Entity.SetBaseRotation(this.playerRotation);
		}
		if (this.playerFlipped)
		{
			this.playerRotation = 180f;
		}
		else
		{
			this.playerRotation = 0f;
		}
		this.camMod.rotOffset.z = this.playerRotation;
		Singleton<CoreGameManager>.Instance.GetPlayer(0).plm.Entity.SetBaseRotation(this.playerRotation);
		this.playerSpinning = false;
		if (!this.active)
		{
			Singleton<CoreGameManager>.Instance.GetCamera(0).cameraModifiers.Remove(this.camMod);
			Singleton<CoreGameManager>.Instance.GetPlayer(0).camMods.Remove(this.camMod);
			Singleton<CoreGameManager>.Instance.GetPlayer(0).plm.Entity.SetBaseRotation(0f);
		}
		yield break;
	}

	// Token: 0x06000275 RID: 629 RVA: 0x0000DAB0 File Offset: 0x0000BCB0
	public void FlipNPC(NPC npc)
	{
		if (base.Active)
		{
			int i = 0;
			while (i < this.npcs.Count)
			{
				if (this.npcs[i] == npc)
				{
					this.npcFlipped[i] = !this.npcFlipped[i];
					if (this.npcFlipped[i])
					{
						this.npcs[i].SetSpriteRotation(180f);
						Entity entity = this.npcs[i].Navigator.Entity;
						if (entity != null)
						{
							entity.SetBaseRotation(180f);
						}
					}
					else
					{
						this.npcs[i].SetSpriteRotation(0f);
						Entity entity2 = this.npcs[i].Navigator.Entity;
						if (entity2 != null)
						{
							entity2.SetBaseRotation(0f);
						}
					}
					if (npc.Navigator.Entity != null)
					{
						this.npcs[i].Navigator.Entity.IgnoreEntity(Singleton<CoreGameManager>.Instance.GetPlayer(0).plm.Entity, this.npcFlipped[i] != this.playerFlipped);
						return;
					}
					break;
				}
				else
				{
					i++;
				}
			}
		}
	}

	// Token: 0x06000276 RID: 630 RVA: 0x0000DBFC File Offset: 0x0000BDFC
	private bool CalculatePlayerDetour(NPC npc, NavigationState parentState, out NavigationDetourState state)
	{
		state = null;
		if (this.NpcOrientationMatchesPlayer(npc))
		{
			return false;
		}
		this._closestFlippers.Clear();
		foreach (GravityFlipper gravityFlipper in this.flippers)
		{
			float num = Vector3.Distance(npc.transform.position, gravityFlipper.transform.position);
			gravityFlipper._npcDistance = num;
			int num2 = 0;
			while (num2 < this._closestFlippers.Count && num > this._closestFlippers[num2]._npcDistance)
			{
				num2++;
			}
			this._closestFlippers.Insert(num2, gravityFlipper);
		}
		Vector3 position = Vector3.zero;
		bool flag = false;
		bool flag2 = false;
		int num3 = 0;
		int num4 = 0;
		while (num4 < 4 && num4 < this._closestFlippers.Count)
		{
			int num5 = this.ec.NavigableDistance(npc.transform.position, this._closestFlippers[num4].transform.position, PathType.Nav);
			if (num5 >= 0 && (num5 < num3 || !flag2))
			{
				num3 = num5;
				position = this._closestFlippers[num4].transform.position;
				state = new Detour_GravityEvent(this, npc, position, parentState);
				flag2 = true;
				flag = true;
			}
			num4++;
		}
		if (flag)
		{
			this.activeDetours.Add(state);
		}
		return flag;
	}

	// Token: 0x06000277 RID: 631 RVA: 0x0000DD78 File Offset: 0x0000BF78
	private void SpawnFlippers()
	{
		if (base.Active)
		{
			int num = this.crng.Next(this.minFlippers, this.maxFlippers);
			int num2 = 0;
			while (num2 < num && this.tiles.Count > 0)
			{
				int index = this.crng.Next(0, this.tiles.Count);
				this.SpawnFlipper(this.tiles[index]);
				this.tiles.RemoveAt(index);
				num2++;
			}
			base.StartCoroutine(this.RespawnTimer());
		}
	}

	// Token: 0x06000278 RID: 632 RVA: 0x0000DE04 File Offset: 0x0000C004
	private void SpawnFlipper(Cell tile)
	{
		GravityFlipper gravityFlipper = Object.Instantiate<GravityFlipper>(this.flipperPre[Random.Range(0, this.flipperPre.Length)], tile.TileTransform);
		gravityFlipper.transform.localPosition += Vector3.up * 5f;
		gravityFlipper.Initialize(this);
		this.flippers.Add(gravityFlipper);
	}

	// Token: 0x06000279 RID: 633 RVA: 0x0000DE6A File Offset: 0x0000C06A
	public void DestroyFlipper(GravityFlipper flipper)
	{
		this.flippers.Remove(flipper);
		Object.Destroy(flipper.gameObject);
	}

	// Token: 0x0600027A RID: 634 RVA: 0x0000DE84 File Offset: 0x0000C084
	private IEnumerator InitialSpawnTimer(float time)
	{
		while (time > 0f)
		{
			time -= Time.deltaTime * this.ec.EnvironmentTimeScale;
			yield return null;
		}
		this.SpawnFlippers();
		yield break;
	}

	// Token: 0x0600027B RID: 635 RVA: 0x0000DE9A File Offset: 0x0000C09A
	private IEnumerator RespawnTimer()
	{
		float time = (float)Random.Range(this.minRespawnTime, this.maxRespawnTime);
		while (time > 0f)
		{
			time -= Time.deltaTime * this.ec.EnvironmentTimeScale;
			yield return null;
		}
		this.SpawnFlippers();
		yield break;
	}

	// Token: 0x0600027C RID: 636 RVA: 0x0000DEAC File Offset: 0x0000C0AC
	private void UpdateCollision()
	{
		for (int i = 0; i < this.npcFlipped.Count; i++)
		{
			if (this.npcs[i].Navigator.Entity != null)
			{
				this.npcs[i].Navigator.Entity.IgnoreEntity(Singleton<CoreGameManager>.Instance.GetPlayer(0).plm.Entity, this.npcFlipped[i] != this.playerFlipped);
			}
			if (this.npcFlipped[i] != this.playerFlipped)
			{
				if (!this.npcs[i].ContainsDetour(this.npcDetour))
				{
					this.npcs[i].AddDetour(this.npcDetour);
				}
			}
			else
			{
				this.npcs[i].RemoveDetour(this.npcDetour);
			}
		}
		this._detoursToCheck.Clear();
		this._detoursToCheck.AddRange(this.activeDetours);
		foreach (NavigationDetourState navigationDetourState in this._detoursToCheck)
		{
		}
	}

	// Token: 0x0600027D RID: 637 RVA: 0x0000DFF4 File Offset: 0x0000C1F4
	public bool NpcOrientationMatchesPlayer(NPC npc)
	{
		for (int i = 0; i < this.npcFlipped.Count; i++)
		{
			if (this.npcs[i] == npc)
			{
				return this.npcFlipped[i] == this.playerFlipped;
			}
		}
		return false;
	}

	// Token: 0x0600027E RID: 638 RVA: 0x0000E041 File Offset: 0x0000C241
	public void RemoveDetour(Detour_GravityEvent detour)
	{
		this.activeDetours.Remove(detour);
	}

	// Token: 0x0400028B RID: 651
	[SerializeField]
	private GravityFlipper[] flipperPre = new GravityFlipper[0];

	// Token: 0x0400028C RID: 652
	private List<GravityFlipper> flippers = new List<GravityFlipper>();

	// Token: 0x0400028D RID: 653
	private List<GravityFlipper> _closestFlippers = new List<GravityFlipper>();

	// Token: 0x0400028E RID: 654
	[SerializeField]
	private CameraModifier camMod = new CameraModifier(Vector3.zero, Vector3.zero);

	// Token: 0x0400028F RID: 655
	private List<NPC> npcs = new List<NPC>();

	// Token: 0x04000290 RID: 656
	private List<Cell> tiles = new List<Cell>();

	// Token: 0x04000291 RID: 657
	[SerializeField]
	private float flipSpeed = 180f;

	// Token: 0x04000292 RID: 658
	[SerializeField]
	private float initialSpawnDelay = 10f;

	// Token: 0x04000293 RID: 659
	private float playerRotation;

	// Token: 0x04000294 RID: 660
	private float rotationLeft;

	// Token: 0x04000295 RID: 661
	[SerializeField]
	private int minFlippers = 15;

	// Token: 0x04000296 RID: 662
	[SerializeField]
	private int maxFlippers = 25;

	// Token: 0x04000297 RID: 663
	[SerializeField]
	private int minRespawnTime = 20;

	// Token: 0x04000298 RID: 664
	[SerializeField]
	private int maxRespawnTime = 40;

	// Token: 0x04000299 RID: 665
	private List<bool> npcFlipped = new List<bool>();

	// Token: 0x0400029A RID: 666
	private PlayerDetour npcDetour = new PlayerDetour(16);

	// Token: 0x0400029B RID: 667
	private List<NavigationDetourState> activeDetours = new List<NavigationDetourState>();

	// Token: 0x0400029C RID: 668
	private List<NavigationDetourState> _detoursToCheck = new List<NavigationDetourState>();

	// Token: 0x0400029D RID: 669
	private bool playerFlipped;

	// Token: 0x0400029E RID: 670
	private bool playerSpinning;
}
