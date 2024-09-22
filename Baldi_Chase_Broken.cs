using System;
using UnityEngine;

// Token: 0x02000016 RID: 22
public class Baldi_Chase_Broken : Baldi_Chase
{
	// Token: 0x06000095 RID: 149 RVA: 0x000052FF File Offset: 0x000034FF
	public Baldi_Chase_Broken(NPC npc, Baldi baldi) : base(npc, baldi)
	{
	}

	// Token: 0x06000096 RID: 150 RVA: 0x00005309 File Offset: 0x00003509
	public override void Enter()
	{
	}

	// Token: 0x06000097 RID: 151 RVA: 0x0000530B File Offset: 0x0000350B
	public override void OnStateTriggerStay(Collider other)
	{
	}

	// Token: 0x06000098 RID: 152 RVA: 0x0000530D File Offset: 0x0000350D
	protected override void ActivateSlapAnimation()
	{
		if (!this.broken)
		{
			this.baldi.SlapBreak();
			this.broken = true;
			return;
		}
		this.baldi.SlapBroken();
	}

	// Token: 0x040000B5 RID: 181
	private bool broken;
}
