using System;
using UnityEngine;

// Token: 0x020001FB RID: 507
[CreateAssetMenu(fileName = "WindowSet", menuName = "Custom Assets/Window Set", order = 5)]
public class WindowObject : ScriptableObject
{
	// Token: 0x04000D9F RID: 3487
	public Window windowPre;

	// Token: 0x04000DA0 RID: 3488
	public Material mask;

	// Token: 0x04000DA1 RID: 3489
	public Material[] overlay = new Material[2];

	// Token: 0x04000DA2 RID: 3490
	public Material[] open = new Material[2];
}
