using System;
using UnityEngine;

// Token: 0x0200013E RID: 318
public class Tile : MonoBehaviour
{
	// Token: 0x1700008E RID: 142
	// (get) Token: 0x06000766 RID: 1894 RVA: 0x00025F60 File Offset: 0x00024160
	public MeshFilter MeshFilter
	{
		get
		{
			return this.meshFilter;
		}
	}

	// Token: 0x1700008F RID: 143
	// (get) Token: 0x06000767 RID: 1895 RVA: 0x00025F68 File Offset: 0x00024168
	public MeshRenderer MeshRenderer
	{
		get
		{
			return this.meshRenderer;
		}
	}

	// Token: 0x06000768 RID: 1896 RVA: 0x00025F70 File Offset: 0x00024170
	public GameObject Collider(Direction dir)
	{
		return this.collider[(int)dir];
	}

	// Token: 0x0400081B RID: 2075
	[SerializeField]
	private MeshFilter meshFilter;

	// Token: 0x0400081C RID: 2076
	[SerializeField]
	private MeshRenderer meshRenderer;

	// Token: 0x0400081D RID: 2077
	[SerializeField]
	private GameObject[] collider = new GameObject[4];
}
