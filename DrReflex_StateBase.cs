using System;

// Token: 0x02000034 RID: 52
public class DrReflex_StateBase : NpcState
{
	// Token: 0x0600014F RID: 335 RVA: 0x000088F1 File Offset: 0x00006AF1
	public DrReflex_StateBase(DrReflex drReflex) : base(drReflex)
	{
		this.drReflex = drReflex;
	}

	// Token: 0x06000150 RID: 336 RVA: 0x00008901 File Offset: 0x00006B01
	public override void Update()
	{
		base.Update();
		this.drReflex.RotateTowardsNextPoint();
	}

	// Token: 0x06000151 RID: 337 RVA: 0x00008914 File Offset: 0x00006B14
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

	// Token: 0x04000173 RID: 371
	protected DrReflex drReflex;
}
