using System;
using UnityEngine;

// Token: 0x02000225 RID: 549
public class VA_PopupAttribute : PropertyAttribute
{
	// Token: 0x06000C68 RID: 3176 RVA: 0x000417F6 File Offset: 0x0003F9F6
	public VA_PopupAttribute(params string[] newNames)
	{
		this.Names = newNames;
	}

	// Token: 0x04000ECE RID: 3790
	public string[] Names;
}
