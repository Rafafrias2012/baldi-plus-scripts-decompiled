using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020001E0 RID: 480
public class AnimationEventTriggerCallFunctionInParentObjectWhyIsThisNecessaryHahaha : MonoBehaviour
{
	// Token: 0x06000AE6 RID: 2790 RVA: 0x000398DE File Offset: 0x00037ADE
	public void AnimationEvent()
	{
		this.OnAnimationEvent.Invoke();
	}

	// Token: 0x04000C7B RID: 3195
	public UnityEvent OnAnimationEvent;
}
