using System;
using UnityEngine;

// Token: 0x02000046 RID: 70
public class LookAtGuy_Blinding : LookAtGuy_BaseState
{
	// Token: 0x060001A1 RID: 417 RVA: 0x00009B1A File Offset: 0x00007D1A
	public LookAtGuy_Blinding(LookAtGuy theTest, float time) : base(theTest)
	{
		this.time = time;
	}

	// Token: 0x060001A2 RID: 418 RVA: 0x00009B2A File Offset: 0x00007D2A
	public override void Update()
	{
		base.Update();
		this.time -= Time.deltaTime * this.npc.TimeScale;
		if (this.time <= 0f)
		{
			this.theTest.Respawn();
		}
	}

	// Token: 0x060001A3 RID: 419 RVA: 0x00009B68 File Offset: 0x00007D68
	public override void Sighted()
	{
	}

	// Token: 0x060001A4 RID: 420 RVA: 0x00009B6A File Offset: 0x00007D6A
	public override void Unsighted()
	{
	}

	// Token: 0x040001AA RID: 426
	protected float time;
}
