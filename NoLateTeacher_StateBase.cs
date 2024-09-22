using System;

// Token: 0x02000049 RID: 73
public class NoLateTeacher_StateBase : NpcState
{
	// Token: 0x060001C0 RID: 448 RVA: 0x0000A8BE File Offset: 0x00008ABE
	public NoLateTeacher_StateBase(NoLateTeacher pomp) : base(pomp)
	{
		this.pomp = pomp;
	}

	// Token: 0x060001C1 RID: 449 RVA: 0x0000A8CE File Offset: 0x00008ACE
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

	// Token: 0x040001E3 RID: 483
	protected NoLateTeacher pomp;
}
