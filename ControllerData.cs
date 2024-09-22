using System;
using System.Collections.Generic;

// Token: 0x0200012E RID: 302
[Serializable]
public class ControllerData
{
	// Token: 0x06000735 RID: 1845 RVA: 0x00024F50 File Offset: 0x00023150
	public ControllerData(string id)
	{
		this.id = id;
	}

	// Token: 0x040007EB RID: 2027
	public string id;

	// Token: 0x040007EC RID: 2028
	public List<InputData> inputs = new List<InputData>();
}
