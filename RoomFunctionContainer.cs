using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001A2 RID: 418
public class RoomFunctionContainer : MonoBehaviour
{
	// Token: 0x06000984 RID: 2436 RVA: 0x00032EBC File Offset: 0x000310BC
	public void Initialize(RoomController room)
	{
		foreach (RoomFunction roomFunction in this.functions)
		{
			roomFunction.Initialize(room);
		}
	}

	// Token: 0x06000985 RID: 2437 RVA: 0x00032F10 File Offset: 0x00031110
	public void Build(LevelBuilder builder, Random rng)
	{
		foreach (RoomFunction roomFunction in this.functions)
		{
			roomFunction.Build(builder, rng);
		}
	}

	// Token: 0x06000986 RID: 2438 RVA: 0x00032F64 File Offset: 0x00031164
	public void OnGenerationFinished()
	{
		foreach (RoomFunction roomFunction in this.functions)
		{
			roomFunction.OnGenerationFinished();
		}
	}

	// Token: 0x06000987 RID: 2439 RVA: 0x00032FB4 File Offset: 0x000311B4
	public void AddFunction(RoomFunction function)
	{
		this.functions.Add(function);
	}

	// Token: 0x06000988 RID: 2440 RVA: 0x00032FC4 File Offset: 0x000311C4
	public void OnPlayerEnter(PlayerManager player)
	{
		foreach (RoomFunction roomFunction in this.functions)
		{
			roomFunction.OnPlayerEnter(player);
		}
	}

	// Token: 0x06000989 RID: 2441 RVA: 0x00033018 File Offset: 0x00031218
	public void OnPlayerStay(PlayerManager player)
	{
		foreach (RoomFunction roomFunction in this.functions)
		{
			roomFunction.OnPlayerStay(player);
		}
	}

	// Token: 0x0600098A RID: 2442 RVA: 0x0003306C File Offset: 0x0003126C
	public void OnPlayerExit(PlayerManager player)
	{
		foreach (RoomFunction roomFunction in this.functions)
		{
			roomFunction.OnPlayerExit(player);
		}
	}

	// Token: 0x0600098B RID: 2443 RVA: 0x000330C0 File Offset: 0x000312C0
	public void OnNpcEnter(NPC npc)
	{
		foreach (RoomFunction roomFunction in this.functions)
		{
			roomFunction.OnNpcEnter(npc);
		}
	}

	// Token: 0x0600098C RID: 2444 RVA: 0x00033114 File Offset: 0x00031314
	public void OnNpcStay(NPC npc)
	{
		foreach (RoomFunction roomFunction in this.functions)
		{
			roomFunction.OnNpcStay(npc);
		}
	}

	// Token: 0x0600098D RID: 2445 RVA: 0x00033168 File Offset: 0x00031368
	public void OnNpcExit(NPC npc)
	{
		foreach (RoomFunction roomFunction in this.functions)
		{
			roomFunction.OnNpcExit(npc);
		}
	}

	// Token: 0x0600098E RID: 2446 RVA: 0x000331BC File Offset: 0x000313BC
	public void OnEntityEnter(Entity entity)
	{
		foreach (RoomFunction roomFunction in this.functions)
		{
			roomFunction.OnEntityEnter(entity);
		}
	}

	// Token: 0x0600098F RID: 2447 RVA: 0x00033210 File Offset: 0x00031410
	public void OnEntityStay(Entity entity)
	{
		foreach (RoomFunction roomFunction in this.functions)
		{
			roomFunction.OnEntityStay(entity);
		}
	}

	// Token: 0x06000990 RID: 2448 RVA: 0x00033264 File Offset: 0x00031464
	public void OnEntityExit(Entity entity)
	{
		foreach (RoomFunction roomFunction in this.functions)
		{
			roomFunction.OnEntityExit(entity);
		}
	}

	// Token: 0x04000ACF RID: 2767
	[SerializeField]
	private List<RoomFunction> functions;
}
