using System;

// Token: 0x0200012F RID: 303
[Serializable]
public class InputData
{
	// Token: 0x06000736 RID: 1846 RVA: 0x00024F6A File Offset: 0x0002316A
	public InputData(string name, InputAction action)
	{
		this.name = name;
		this.action = action;
	}

	// Token: 0x040007ED RID: 2029
	public string name;

	// Token: 0x040007EE RID: 2030
	public InputAction action;
}
