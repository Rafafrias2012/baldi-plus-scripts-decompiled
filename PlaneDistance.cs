using System;
using UnityEngine;

// Token: 0x02000215 RID: 533
public class PlaneDistance : MonoBehaviour
{
	// Token: 0x06000BE7 RID: 3047 RVA: 0x0003EDA6 File Offset: 0x0003CFA6
	private void Awake()
	{
		base.GetComponent<Canvas>().planeDistance = this.planeDistance;
	}

	// Token: 0x04000E72 RID: 3698
	[SerializeField]
	private float planeDistance = 0.31f;
}
