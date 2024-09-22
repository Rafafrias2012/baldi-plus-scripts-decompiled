using System;
using UnityEngine;

// Token: 0x020000D0 RID: 208
public class DestroyOnAwake : MonoBehaviour
{
	// Token: 0x060004E9 RID: 1257 RVA: 0x000197AE File Offset: 0x000179AE
	private void Awake()
	{
		Object.Destroy(base.gameObject);
	}
}
