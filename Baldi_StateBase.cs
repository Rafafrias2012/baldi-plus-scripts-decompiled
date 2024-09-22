using System;
using UnityEngine;

// Token: 0x02000014 RID: 20
public class Baldi_StateBase : NpcState
{
	// Token: 0x0600008B RID: 139 RVA: 0x0000511B File Offset: 0x0000331B
	public Baldi_StateBase(NPC npc, Baldi baldi) : base(npc)
	{
		this.baldi = baldi;
	}

	// Token: 0x0600008C RID: 140 RVA: 0x0000512B File Offset: 0x0000332B
	public override void Hear(Vector3 position, int value)
	{
		base.Hear(position, value);
		this.baldi.Hear(position, value, true);
	}

	// Token: 0x0600008D RID: 141 RVA: 0x00005143 File Offset: 0x00003343
	public override void PlayerInSight(PlayerManager player)
	{
		base.PlayerInSight(player);
		this.baldi.ClearSoundLocations();
		this.baldi.Hear(player.transform.position, 127, false);
	}

	// Token: 0x0600008E RID: 142 RVA: 0x00005170 File Offset: 0x00003370
	public override void DestinationEmpty()
	{
		base.DestinationEmpty();
		this.baldi.UpdateSoundTarget();
	}

	// Token: 0x0600008F RID: 143 RVA: 0x00005183 File Offset: 0x00003383
	protected virtual void ActivateSlapAnimation()
	{
		this.baldi.SlapNormal();
	}

	// Token: 0x06000090 RID: 144 RVA: 0x00005190 File Offset: 0x00003390
	public override void DoorHit(StandardDoor door)
	{
		if (door.locked)
		{
			door.Unlock();
			door.OpenTimed(5f, false);
			return;
		}
		base.DoorHit(door);
	}

	// Token: 0x040000B3 RID: 179
	protected Baldi baldi;
}
