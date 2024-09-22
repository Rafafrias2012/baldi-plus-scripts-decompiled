using System;
using UnityEngine;

// Token: 0x02000195 RID: 405
public class MaterialModifier : MonoBehaviour
{
	// Token: 0x06000934 RID: 2356 RVA: 0x0003106C File Offset: 0x0002F26C
	public static void ChangeMaterial(MeshRenderer mesh, Material[] material)
	{
		mesh.sharedMaterials = material;
	}

	// Token: 0x06000935 RID: 2357 RVA: 0x00031078 File Offset: 0x0002F278
	public static void SetBase(MeshRenderer mesh, Texture tex)
	{
		MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
		mesh.GetPropertyBlock(materialPropertyBlock, 0);
		materialPropertyBlock.SetTexture("_MainTex", tex);
		mesh.SetPropertyBlock(materialPropertyBlock, 0);
	}

	// Token: 0x06000936 RID: 2358 RVA: 0x000310A8 File Offset: 0x0002F2A8
	public static void ChangeOverlay(MeshRenderer mesh, Material overlay)
	{
		MaterialModifier.ChangeMaterial(mesh, new Material[]
		{
			mesh.sharedMaterials[0],
			overlay
		});
	}

	// Token: 0x06000937 RID: 2359 RVA: 0x000310D4 File Offset: 0x0002F2D4
	public static void ChangeHole(MeshRenderer mesh, Material mask, Material overlay)
	{
		Material material = mesh.sharedMaterials[0];
		Texture texture = null;
		if (material.HasProperty("_MainTex"))
		{
			texture = material.GetTexture("_MainTex");
		}
		MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
		if (texture == null)
		{
			mesh.GetPropertyBlock(materialPropertyBlock, 0);
			texture = materialPropertyBlock.GetTexture("_MainTex");
		}
		MaterialModifier.ChangeMaterial(mesh, new Material[]
		{
			mask,
			overlay
		});
		mesh.GetPropertyBlock(materialPropertyBlock, 0);
		if (texture != null)
		{
			materialPropertyBlock.SetTexture("_MainTex", texture);
		}
		if (material.shader.name == "Shader Graphs/StandardTransparent")
		{
			materialPropertyBlock.SetFloat("_RenderSky", material.GetFloat("_RenderSky"));
		}
		else if (material.shader.name == "Shader Graphs/MaskedStandard")
		{
			materialPropertyBlock.SetFloat("_RenderSky", materialPropertyBlock.GetFloat("_RenderSky"));
		}
		mesh.SetPropertyBlock(materialPropertyBlock, 0);
	}

	// Token: 0x06000938 RID: 2360 RVA: 0x000311C0 File Offset: 0x0002F3C0
	public static void ChangeHoleBackground(MeshRenderer mesh, Material background)
	{
		MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
		mesh.GetPropertyBlock(materialPropertyBlock, 0);
		materialPropertyBlock.SetTexture("_MainTex", background.GetTexture("_MainTex"));
		if (background.shader.name == "Shader Graphs/StandardTransparent" || background.shader.name == "Shader Graphs/MaskedStandard")
		{
			materialPropertyBlock.SetFloat("_RenderSky", background.GetFloat("_RenderSky"));
		}
		mesh.SetPropertyBlock(materialPropertyBlock, 0);
	}

	// Token: 0x06000939 RID: 2361 RVA: 0x0003123D File Offset: 0x0002F43D
	public static void ChangeColor(MeshRenderer mesh, Color color, int[] layers)
	{
	}

	// Token: 0x0600093A RID: 2362 RVA: 0x00031240 File Offset: 0x0002F440
	public static Color[] GetColorsForTileTexture(Texture2D toCopy, int size)
	{
		Color[] array = new Color[size * size];
		float num = (float)toCopy.width / (float)size;
		for (int i = 0; i < size; i++)
		{
			for (int j = 0; j < size; j++)
			{
				int num2 = i * size + j;
				array[num2] = toCopy.GetPixel(Mathf.FloorToInt((float)j * num), Mathf.FloorToInt((float)i * num));
			}
		}
		return array;
	}
}
