using System;
using UnityEngine;

// Token: 0x020001A1 RID: 417
public class RoomFunction : MonoBehaviour
{
	// Token: 0x06000976 RID: 2422 RVA: 0x00032E8A File Offset: 0x0003108A
	public virtual void Initialize(RoomController room)
	{
		this.room = room;
	}

	// Token: 0x06000977 RID: 2423 RVA: 0x00032E93 File Offset: 0x00031093
	public virtual void Build(LevelBuilder builder, Random rng)
	{
	}

	// Token: 0x06000978 RID: 2424 RVA: 0x00032E95 File Offset: 0x00031095
	public virtual void OnGenerationFinished()
	{
	}

	// Token: 0x06000979 RID: 2425 RVA: 0x00032E97 File Offset: 0x00031097
	public virtual void OnPlayerEnter(PlayerManager player)
	{
	}

	// Token: 0x0600097A RID: 2426 RVA: 0x00032E99 File Offset: 0x00031099
	public virtual void OnPlayerStay(PlayerManager player)
	{
	}

	// Token: 0x0600097B RID: 2427 RVA: 0x00032E9B File Offset: 0x0003109B
	public virtual void OnPlayerExit(PlayerManager player)
	{
	}

	// Token: 0x0600097C RID: 2428 RVA: 0x00032E9D File Offset: 0x0003109D
	public virtual void OnNpcEnter(NPC npc)
	{
	}

	// Token: 0x0600097D RID: 2429 RVA: 0x00032E9F File Offset: 0x0003109F
	public virtual void OnNpcStay(NPC npc)
	{
	}

	// Token: 0x0600097E RID: 2430 RVA: 0x00032EA1 File Offset: 0x000310A1
	public virtual void OnNpcExit(NPC npc)
	{
	}

	// Token: 0x0600097F RID: 2431 RVA: 0x00032EA3 File Offset: 0x000310A3
	public virtual void OnEntityEnter(Entity entity)
	{
	}

	// Token: 0x06000980 RID: 2432 RVA: 0x00032EA5 File Offset: 0x000310A5
	public virtual void OnEntityStay(Entity entity)
	{
	}

	// Token: 0x06000981 RID: 2433 RVA: 0x00032EA7 File Offset: 0x000310A7
	public virtual void OnEntityExit(Entity entity)
	{
	}

	// Token: 0x170000CF RID: 207
	// (get) Token: 0x06000982 RID: 2434 RVA: 0x00032EA9 File Offset: 0x000310A9
	public RoomController Room
	{
		get
		{
			return this.room;
		}
	}

	// Token: 0x04000ACE RID: 2766
	protected RoomController room;
}
