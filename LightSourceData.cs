using System;
using UnityEngine;

// Token: 0x0200017B RID: 379
[Serializable]
public class LightSourceData
{
	// Token: 0x060008D4 RID: 2260 RVA: 0x0002D6B4 File Offset: 0x0002B8B4
	public LightSourceData GetNew()
	{
		return new LightSourceData
		{
			prefab = this.prefab,
			position = new IntVector2(this.position.x, this.position.z),
			color = new Color(this.color.r, this.color.g, this.color.b, this.color.a),
			strength = this.strength
		};
	}

	// Token: 0x04000987 RID: 2439
	public Transform prefab;

	// Token: 0x04000988 RID: 2440
	public IntVector2 position;

	// Token: 0x04000989 RID: 2441
	public Color color;

	// Token: 0x0400098A RID: 2442
	public int strength;
}
