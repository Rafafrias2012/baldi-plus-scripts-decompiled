using System;
using UnityEngine;

// Token: 0x02000200 RID: 512
public class TextureCopyTest : MonoBehaviour
{
	// Token: 0x06000B63 RID: 2915 RVA: 0x0003BFBC File Offset: 0x0003A1BC
	private void Start()
	{
		Texture2D texture2D = new Texture2D(256, 256, TextureFormat.RGB24, false, false);
		texture2D.filterMode = FilterMode.Point;
		Color[] pixels = this.toCopy.GetPixels();
		texture2D.SetPixels(0, 0, 128, 128, pixels);
		texture2D.Apply();
		base.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", texture2D);
	}

	// Token: 0x04000DB2 RID: 3506
	[SerializeField]
	private Texture2D toCopy;
}
