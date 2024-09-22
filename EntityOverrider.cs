using System;

// Token: 0x020000E1 RID: 225
public class EntityOverrider
{
	// Token: 0x17000065 RID: 101
	// (get) Token: 0x0600054B RID: 1355 RVA: 0x0001B505 File Offset: 0x00019705
	// (set) Token: 0x0600054C RID: 1356 RVA: 0x0001B50D File Offset: 0x0001970D
	public Entity entity { get; private set; }

	// Token: 0x17000066 RID: 102
	// (get) Token: 0x0600054D RID: 1357 RVA: 0x0001B516 File Offset: 0x00019716
	// (set) Token: 0x0600054E RID: 1358 RVA: 0x0001B51E File Offset: 0x0001971E
	public float height { get; private set; }

	// Token: 0x17000067 RID: 103
	// (get) Token: 0x0600054F RID: 1359 RVA: 0x0001B527 File Offset: 0x00019727
	// (set) Token: 0x06000550 RID: 1360 RVA: 0x0001B52F File Offset: 0x0001972F
	public bool active { get; private set; } = true;

	// Token: 0x06000551 RID: 1361 RVA: 0x0001B538 File Offset: 0x00019738
	public EntityOverrider()
	{
		this.entity = null;
		this.height = 0f;
		this.freezes = 0;
		this.interactionDisables = 0;
		this.active = false;
	}

	// Token: 0x06000552 RID: 1362 RVA: 0x0001B56E File Offset: 0x0001976E
	public void Override(Entity entity)
	{
		this.entity = entity;
		this.active = true;
	}

	// Token: 0x06000553 RID: 1363 RVA: 0x0001B580 File Offset: 0x00019780
	public void Release()
	{
		if (this.active)
		{
			Entity entity = this.entity;
			if (entity != null)
			{
				entity.Release();
			}
			Entity entity2 = this.entity;
			if (entity2 != null)
			{
				entity2.UpdateHeightAndScale();
			}
			Entity entity3 = this.entity;
			if (entity3 != null)
			{
				entity3.UpdateLayer();
			}
			this.active = false;
		}
	}

	// Token: 0x06000554 RID: 1364 RVA: 0x0001B5CF File Offset: 0x000197CF
	public void SetHeight(float height)
	{
		this.height = height;
		Entity entity = this.entity;
		if (entity == null)
		{
			return;
		}
		entity.UpdateHeightAndScale();
	}

	// Token: 0x06000555 RID: 1365 RVA: 0x0001B5E8 File Offset: 0x000197E8
	public void SetFrozen(bool value)
	{
		if (value)
		{
			this.freezes++;
			return;
		}
		this.freezes--;
		if (this.freezes <= 0)
		{
			this.freezes = 0;
		}
	}

	// Token: 0x17000068 RID: 104
	// (get) Token: 0x06000556 RID: 1366 RVA: 0x0001B61A File Offset: 0x0001981A
	public bool Frozen
	{
		get
		{
			return this.freezes > 0;
		}
	}

	// Token: 0x06000557 RID: 1367 RVA: 0x0001B628 File Offset: 0x00019828
	public void SetInteractionState(bool value)
	{
		if (!value)
		{
			this.interactionDisables++;
		}
		else if (this.interactionDisables > 0)
		{
			this.interactionDisables--;
		}
		if (this.active)
		{
			Entity entity = this.entity;
			if (entity == null)
			{
				return;
			}
			entity.UpdateLayer();
		}
	}

	// Token: 0x17000069 RID: 105
	// (get) Token: 0x06000558 RID: 1368 RVA: 0x0001B677 File Offset: 0x00019877
	public bool InteractionDisabled
	{
		get
		{
			return this.interactionDisables > 0;
		}
	}

	// Token: 0x0400058E RID: 1422
	private int freezes;

	// Token: 0x0400058F RID: 1423
	private int interactionDisables;
}
