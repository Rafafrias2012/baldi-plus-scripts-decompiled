using System;
using UnityEngine;

// Token: 0x0200013C RID: 316
public class TextureSlider : MonoBehaviour
{
	// Token: 0x06000761 RID: 1889 RVA: 0x00025ECC File Offset: 0x000240CC
	private void Update()
	{
		this.offset.x = Time.time * this.speed.x;
		this.offset.y = Time.time * this.speed.y;
		this.material.SetVector("_TextureOffset", this.offset);
	}

	// Token: 0x04000817 RID: 2071
	public Material material;

	// Token: 0x04000818 RID: 2072
	private Vector4 offset;

	// Token: 0x04000819 RID: 2073
	public Vector2 speed;
}
