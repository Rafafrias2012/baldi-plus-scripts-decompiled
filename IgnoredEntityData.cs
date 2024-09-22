using System;

// Token: 0x020000DF RID: 223
[Serializable]
public class IgnoredEntityData
{
	// Token: 0x06000544 RID: 1348 RVA: 0x0001B457 File Offset: 0x00019657
	public IgnoredEntityData()
	{
		this.ignoredEntity = null;
		this.ignores = 0;
	}

	// Token: 0x06000545 RID: 1349 RVA: 0x0001B46D File Offset: 0x0001966D
	public IgnoredEntityData(Entity ignoredEntity, int ignores)
	{
		this.ignoredEntity = ignoredEntity;
		this.ignores = ignores;
	}

	// Token: 0x04000588 RID: 1416
	public Entity ignoredEntity;

	// Token: 0x04000589 RID: 1417
	public int ignores;
}
