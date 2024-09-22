using System;
using UnityEngine;

// Token: 0x020001F1 RID: 497
[CreateAssetMenu(fileName = "LayerMask", menuName = "Custom Assets/Layer Mask", order = 13)]
public class LayerMaskObject : ScriptableObject
{
	// Token: 0x06000B42 RID: 2882 RVA: 0x0003B80C File Offset: 0x00039A0C
	public bool Contains(int layer)
	{
		return (this.mask & 1 << layer) != 0;
	}

	// Token: 0x04000CFB RID: 3323
	public LayerMask mask;
}
