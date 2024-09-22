using System;

// Token: 0x020000BE RID: 190
public class NavigationStateMachine
{
	// Token: 0x0600048A RID: 1162 RVA: 0x000171F0 File Offset: 0x000153F0
	public void ChangeState(NavigationState newState)
	{
		if (this.currentState != newState)
		{
			if (this.currentState != null)
			{
				if (this.currentState.priority <= newState.priority)
				{
					this.currentState.Exit();
					this.currentState = newState;
					this.currentState.Enter();
					return;
				}
			}
			else
			{
				this.currentState = newState;
				this.currentState.Enter();
			}
		}
	}

	// Token: 0x0600048B RID: 1163 RVA: 0x00017251 File Offset: 0x00015451
	public void DestinationEmpty()
	{
		this.currentState.DestinationEmpty();
	}

	// Token: 0x040004E5 RID: 1253
	private NavigationState currentState;
}
