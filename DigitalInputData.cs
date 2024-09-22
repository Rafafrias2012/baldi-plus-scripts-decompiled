using System;
using Steamworks;

// Token: 0x02000112 RID: 274
public class DigitalInputData
{
	// Token: 0x060006C1 RID: 1729 RVA: 0x00022859 File Offset: 0x00020A59
	public DigitalInputData(InputDigitalActionHandle_t handle)
	{
		this.handle = handle;
	}

	// Token: 0x040006F9 RID: 1785
	public InputDigitalActionHandle_t handle;

	// Token: 0x040006FA RID: 1786
	public bool[] active = new bool[16];

	// Token: 0x040006FB RID: 1787
	public bool[] firstFrame = new bool[16];
}
