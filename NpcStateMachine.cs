using System;

// Token: 0x020000BC RID: 188
public class NpcStateMachine
{
	// Token: 0x17000047 RID: 71
	// (get) Token: 0x0600046B RID: 1131 RVA: 0x000170BA File Offset: 0x000152BA
	public NpcState CurrentState
	{
		get
		{
			return this.currentState;
		}
	}

	// Token: 0x17000048 RID: 72
	// (get) Token: 0x0600046C RID: 1132 RVA: 0x000170C2 File Offset: 0x000152C2
	public NavigationState CurrentNavigationState
	{
		get
		{
			return this.currentState.CurrentNavigationState;
		}
	}

	// Token: 0x0600046D RID: 1133 RVA: 0x000170CF File Offset: 0x000152CF
	public void ChangeState(NpcState newState)
	{
		if (this.currentState != null)
		{
			this.currentState.Exit();
			newState.CurrentNavigationState = this.currentState.CurrentNavigationState;
		}
		this.currentState = newState;
		this.currentState.Enter();
	}

	// Token: 0x0600046E RID: 1134 RVA: 0x00017107 File Offset: 0x00015307
	public void Update()
	{
		if (this.currentState != null)
		{
			this.currentState.Update();
		}
	}

	// Token: 0x0600046F RID: 1135 RVA: 0x0001711C File Offset: 0x0001531C
	public void ChangeNavigationState(NavigationState state)
	{
		this.currentState.ChangeNavigationState(state);
	}

	// Token: 0x06000470 RID: 1136 RVA: 0x0001712A File Offset: 0x0001532A
	public void RestoreNavigationState()
	{
		this.currentState.RestoreNavigationState();
	}

	// Token: 0x040004E1 RID: 1249
	public NpcState currentState;
}
