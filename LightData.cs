using System;

// Token: 0x02000192 RID: 402
[Serializable]
public class LightData
{
	// Token: 0x0600092C RID: 2348 RVA: 0x00030F92 File Offset: 0x0002F192
	public LightData(Cell source, int distance)
	{
		this.source = source;
		this.distance = distance;
	}

	// Token: 0x04000A1C RID: 2588
	public Cell source;

	// Token: 0x04000A1D RID: 2589
	public int distance;
}
