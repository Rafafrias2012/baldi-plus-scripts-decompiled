using System;
using UnityEngine;

// Token: 0x0200000D RID: 13
public class ArtsAndCrafters_Ready : ArtsAndCrafters_StateBase
{
	// Token: 0x06000050 RID: 80 RVA: 0x00004315 File Offset: 0x00002515
	public ArtsAndCrafters_Ready(ArtsAndCrafters crafters) : base(crafters)
	{
	}

	// Token: 0x06000051 RID: 81 RVA: 0x0000432C File Offset: 0x0000252C
	public override void Update()
	{
		base.Update();
		if (!this.npc.looker.PlayerInSight())
		{
			this.time -= Time.deltaTime * this.npc.TimeScale;
			if (this.time <= 0f)
			{
				this.crafters.state = new ArtsAndCrafters_Waiting(this.crafters);
				this.npc.behaviorStateMachine.ChangeState(this.crafters.state);
				return;
			}
		}
		else
		{
			this.time = 1f;
		}
		if (!this.npc.looker.IsVisible)
		{
			this.crafters.state = new ArtsAndCrafters_Stalking(this.crafters);
			this.npc.behaviorStateMachine.ChangeState(this.crafters.state);
			return;
		}
	}

	// Token: 0x04000087 RID: 135
	private float time = 1f;
}
