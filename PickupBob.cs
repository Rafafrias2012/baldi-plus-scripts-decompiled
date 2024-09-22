using System;
using UnityEngine;

// Token: 0x02000126 RID: 294
public class PickupBob : MonoBehaviour
{
	// Token: 0x0600071D RID: 1821 RVA: 0x00023D59 File Offset: 0x00021F59
	private void Update()
	{
		base.transform.localPosition = new Vector3(0f, PickupBobValue.bobVal, 0f);
	}

	// Token: 0x0600071E RID: 1822 RVA: 0x00023D7A File Offset: 0x00021F7A
	private void OnEnable()
	{
		base.transform.localPosition = new Vector3(0f, PickupBobValue.bobVal, 0f);
	}
}
