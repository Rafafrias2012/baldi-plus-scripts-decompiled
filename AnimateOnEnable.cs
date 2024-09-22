using System;
using UnityEngine;

// Token: 0x020000E7 RID: 231
public class AnimateOnEnable : MonoBehaviour
{
	// Token: 0x06000568 RID: 1384 RVA: 0x0001B891 File Offset: 0x00019A91
	private void OnEnable()
	{
		this.animator.Play(this.name, -1, 0f);
	}

	// Token: 0x0400059B RID: 1435
	public Animator animator;

	// Token: 0x0400059C RID: 1436
	public new string name;
}
