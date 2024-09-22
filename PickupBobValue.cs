using System;
using UnityEngine;

// Token: 0x02000127 RID: 295
public class PickupBobValue : MonoBehaviour
{
	// Token: 0x06000720 RID: 1824 RVA: 0x00023DA3 File Offset: 0x00021FA3
	private void Update()
	{
		this.val += Time.deltaTime;
		PickupBobValue.bobVal = Mathf.Sin(this.val * this.speed) / 2f;
	}

	// Token: 0x0400075B RID: 1883
	public static float bobVal;

	// Token: 0x0400075C RID: 1884
	public float speed = 5f;

	// Token: 0x0400075D RID: 1885
	private float val;
}
