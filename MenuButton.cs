using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020001D7 RID: 471
public class MenuButton : MonoBehaviour
{
	// Token: 0x06000AA7 RID: 2727 RVA: 0x0003837B File Offset: 0x0003657B
	public virtual void Press()
	{
		this.OnPress.Invoke();
	}

	// Token: 0x06000AA8 RID: 2728 RVA: 0x00038388 File Offset: 0x00036588
	public virtual void Highlight()
	{
	}

	// Token: 0x06000AA9 RID: 2729 RVA: 0x0003838A File Offset: 0x0003658A
	public virtual void UnHold()
	{
	}

	// Token: 0x170000E0 RID: 224
	// (get) Token: 0x06000AAA RID: 2730 RVA: 0x0003838C File Offset: 0x0003658C
	public bool WasHighlighted
	{
		get
		{
			return this.wasHighlighted;
		}
	}

	// Token: 0x04000C33 RID: 3123
	public UnityEvent OnPress;

	// Token: 0x04000C34 RID: 3124
	public SoundObject audHighlightOverride;

	// Token: 0x04000C35 RID: 3125
	public SoundObject audConfirmOverride;

	// Token: 0x04000C36 RID: 3126
	protected bool highlighted;

	// Token: 0x04000C37 RID: 3127
	protected bool wasHighlighted;

	// Token: 0x04000C38 RID: 3128
	protected bool held;
}
