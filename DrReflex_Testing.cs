using System;

// Token: 0x02000038 RID: 56
public class DrReflex_Testing : DrReflex_StateBase
{
	// Token: 0x06000162 RID: 354 RVA: 0x00008DB8 File Offset: 0x00006FB8
	public DrReflex_Testing(DrReflex drReflex, PlayerManager player) : base(drReflex)
	{
		this.player = player;
	}

	// Token: 0x06000163 RID: 355 RVA: 0x00008DC8 File Offset: 0x00006FC8
	public override void Enter()
	{
		base.Enter();
		this.drReflex.StartTest(this.player);
		base.ChangeNavigationState(new NavigationState_DoNothing(this.drReflex, 127));
	}

	// Token: 0x06000164 RID: 356 RVA: 0x00008DF4 File Offset: 0x00006FF4
	public override void Update()
	{
		this.drReflex.RotateTowardsPlayer(this.player);
		if (this.drReflex.PlayerLeft(this.player))
		{
			this.drReflex.EndTest(false, this.player);
		}
	}

	// Token: 0x0400017C RID: 380
	protected PlayerManager player;
}
