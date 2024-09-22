using System;
using UnityEngine;

// Token: 0x020000DB RID: 219
public class SelfDestruct : MonoBehaviour
{
	// Token: 0x060004FF RID: 1279 RVA: 0x00019E62 File Offset: 0x00018062
	public void Boom()
	{
		Object.Destroy(base.gameObject);
	}
}
