using System;

// Token: 0x0200013F RID: 319
[Serializable]
public class TimeScaleModifier
{
	// Token: 0x0600076A RID: 1898 RVA: 0x00025F90 File Offset: 0x00024190
	public TimeScaleModifier()
	{
		this.npcTimeScale = 1f;
		this.environmentTimeScale = 1f;
		this.playerTimeScale = 1f;
	}

	// Token: 0x0600076B RID: 1899 RVA: 0x00025FE5 File Offset: 0x000241E5
	public TimeScaleModifier(float npcTimeScale, float environmentTimeScale, float playerTimeScale)
	{
		this.npcTimeScale = npcTimeScale;
		this.environmentTimeScale = environmentTimeScale;
		this.playerTimeScale = playerTimeScale;
	}

	// Token: 0x0400081E RID: 2078
	public float npcTimeScale = 1f;

	// Token: 0x0400081F RID: 2079
	public float environmentTimeScale = 1f;

	// Token: 0x04000820 RID: 2080
	public float playerTimeScale = 1f;
}
