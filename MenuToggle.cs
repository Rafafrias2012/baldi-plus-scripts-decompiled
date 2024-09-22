using System;
using UnityEngine;

// Token: 0x02000211 RID: 529
public class MenuToggle : MonoBehaviour
{
	// Token: 0x06000BC4 RID: 3012 RVA: 0x0003E08F File Offset: 0x0003C28F
	public void Toggle()
	{
		this.val = !this.val;
		this.UpdateCheck();
	}

	// Token: 0x06000BC5 RID: 3013 RVA: 0x0003E0A6 File Offset: 0x0003C2A6
	public void Set(bool set)
	{
		if (!this.disabled)
		{
			this.val = set;
			this.UpdateCheck();
		}
	}

	// Token: 0x06000BC6 RID: 3014 RVA: 0x0003E0BD File Offset: 0x0003C2BD
	private void UpdateCheck()
	{
		this.checkmark.SetActive(this.val);
	}

	// Token: 0x06000BC7 RID: 3015 RVA: 0x0003E0D0 File Offset: 0x0003C2D0
	public void Disable(bool val)
	{
		this.disabled = val;
		this.disableCover.SetActive(val);
		this.hotspot.SetActive(!val);
	}

	// Token: 0x170000F7 RID: 247
	// (get) Token: 0x06000BC8 RID: 3016 RVA: 0x0003E0F4 File Offset: 0x0003C2F4
	public bool Value
	{
		get
		{
			return this.val;
		}
	}

	// Token: 0x04000E39 RID: 3641
	private bool val;

	// Token: 0x04000E3A RID: 3642
	[SerializeField]
	private GameObject checkmark;

	// Token: 0x04000E3B RID: 3643
	[SerializeField]
	private GameObject disableCover;

	// Token: 0x04000E3C RID: 3644
	[SerializeField]
	private GameObject hotspot;

	// Token: 0x04000E3D RID: 3645
	private bool disabled;
}
