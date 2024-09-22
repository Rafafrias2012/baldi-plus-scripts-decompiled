using System;
using UnityEngine;

// Token: 0x02000165 RID: 357
public class ColliderGroup : MonoBehaviour
{
	// Token: 0x0600080E RID: 2062 RVA: 0x00027E37 File Offset: 0x00026037
	public void Enable(bool enable)
	{
		if (enable)
		{
			base.gameObject.SetActive(true);
			return;
		}
		base.gameObject.SetActive(false);
		this.ResetValues();
	}

	// Token: 0x0600080F RID: 2063 RVA: 0x00027E5C File Offset: 0x0002605C
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			this.playerCount++;
		}
		else if (other.tag == "NPC" && other.isTrigger)
		{
			this.npcCount++;
		}
		this.UpdateBools();
	}

	// Token: 0x06000810 RID: 2064 RVA: 0x00027EBC File Offset: 0x000260BC
	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			this.playerCount--;
		}
		else if (other.tag == "NPC" && other.isTrigger)
		{
			this.npcCount--;
		}
		this.UpdateBools();
	}

	// Token: 0x06000811 RID: 2065 RVA: 0x00027F19 File Offset: 0x00026119
	private void UpdateBools()
	{
		if (this.playerCount > 0)
		{
			this.hasPlayer = true;
		}
		else
		{
			this.hasPlayer = false;
		}
		if (this.npcCount > 0)
		{
			this.hasNPC = true;
			return;
		}
		this.hasNPC = false;
	}

	// Token: 0x06000812 RID: 2066 RVA: 0x00027F4C File Offset: 0x0002614C
	public void ResetValues()
	{
		this.playerCount = 0;
		this.npcCount = 0;
		this.UpdateBools();
	}

	// Token: 0x170000B0 RID: 176
	// (get) Token: 0x06000813 RID: 2067 RVA: 0x00027F62 File Offset: 0x00026162
	public bool HasPlayer
	{
		get
		{
			return this.hasPlayer;
		}
	}

	// Token: 0x170000B1 RID: 177
	// (get) Token: 0x06000814 RID: 2068 RVA: 0x00027F6A File Offset: 0x0002616A
	public bool HasNPC
	{
		get
		{
			return this.hasNPC;
		}
	}

	// Token: 0x0400089C RID: 2204
	private int playerCount;

	// Token: 0x0400089D RID: 2205
	private int npcCount;

	// Token: 0x0400089E RID: 2206
	private bool hasPlayer;

	// Token: 0x0400089F RID: 2207
	private bool hasNPC;
}
