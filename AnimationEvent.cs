using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000EA RID: 234
public class AnimationEvent : MonoBehaviour
{
	// Token: 0x0600056F RID: 1391 RVA: 0x0001BACF File Offset: 0x00019CCF
	public void EventTriggered()
	{
		this.OnAnimationEvent.Invoke();
	}

	// Token: 0x040005A7 RID: 1447
	public UnityEvent OnAnimationEvent;
}
