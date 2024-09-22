using System;

// Token: 0x0200006A RID: 106
public class RulerEvent : RandomEvent
{
	// Token: 0x06000250 RID: 592 RVA: 0x0000CB04 File Offset: 0x0000AD04
	public override void Begin()
	{
		base.Begin();
		foreach (NPC npc in this.ec.Npcs)
		{
			if (npc.Character == Character.Baldi)
			{
				npc.GetComponent<Baldi>().BreakRuler();
			}
		}
	}

	// Token: 0x06000251 RID: 593 RVA: 0x0000CB70 File Offset: 0x0000AD70
	public override void End()
	{
		base.End();
		foreach (NPC npc in this.ec.Npcs)
		{
			if (npc.Character == Character.Baldi)
			{
				npc.GetComponent<Baldi>().RestoreRuler();
			}
		}
	}
}
