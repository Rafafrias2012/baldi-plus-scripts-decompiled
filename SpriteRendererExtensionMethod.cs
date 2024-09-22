using System;
using UnityEngine;

// Token: 0x02000139 RID: 313
public static class SpriteRendererExtensionMethod
{
	// Token: 0x06000758 RID: 1880 RVA: 0x00025D14 File Offset: 0x00023F14
	public static void SetSpriteRotation(this SpriteRenderer renderer, float degrees)
	{
		renderer.GetPropertyBlock(SpriteRendererExtensionMethod._propertyBlock);
		SpriteRendererExtensionMethod._propertyBlock.SetFloat("_SpriteRotation", degrees);
		renderer.SetPropertyBlock(SpriteRendererExtensionMethod._propertyBlock);
	}

	// Token: 0x0400080E RID: 2062
	private static MaterialPropertyBlock _propertyBlock = new MaterialPropertyBlock();
}
