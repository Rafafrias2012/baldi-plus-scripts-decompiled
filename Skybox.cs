using System;
using UnityEngine;

// Token: 0x020001C7 RID: 455
public class Skybox : MonoBehaviour
{
	// Token: 0x06000A56 RID: 2646 RVA: 0x00037074 File Offset: 0x00035274
	public void InitializeMaterials(Texture tex, int sizeX, int sizeZ)
	{
		this.verticalMat = new Material(this.skyboxMat);
		this.verticalMat.SetTexture("_MainTex", tex);
		this.verticalMat.SetFloat("_Tiling", (float)sizeZ);
		this.horizontalMat = new Material(this.verticalMat);
		this.horizontalMat.SetFloat("_Tiling", (float)sizeX);
		MeshRenderer[] array = this.verticalMesh;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].sharedMaterial = this.verticalMat;
		}
		array = this.horizontalMesh;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].sharedMaterial = this.horizontalMat;
		}
	}

	// Token: 0x04000BC5 RID: 3013
	public Material skyboxMat;

	// Token: 0x04000BC6 RID: 3014
	private Material verticalMat;

	// Token: 0x04000BC7 RID: 3015
	private Material horizontalMat;

	// Token: 0x04000BC8 RID: 3016
	[SerializeField]
	private MeshRenderer[] verticalMesh;

	// Token: 0x04000BC9 RID: 3017
	[SerializeField]
	private MeshRenderer[] horizontalMesh;
}
