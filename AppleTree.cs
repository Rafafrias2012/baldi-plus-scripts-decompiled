using System;
using UnityEngine;

// Token: 0x020000B3 RID: 179
public class AppleTree : MonoBehaviour
{
	// Token: 0x06000403 RID: 1027 RVA: 0x000152CE File Offset: 0x000134CE
	private void Start()
	{
	}

	// Token: 0x06000404 RID: 1028 RVA: 0x000152D0 File Offset: 0x000134D0
	private void OnTriggerEnter(Collider other)
	{
		if (!this.dropped && other.gameObject.CompareTag("GrapplingHook"))
		{
			this.apple.transform.SetParent(null, true);
			this.apple.transform.position -= Vector3.up * (this.apple.transform.position.y - 5f);
			this.dropped = true;
		}
	}

	// Token: 0x04000444 RID: 1092
	[SerializeField]
	private Pickup apple;

	// Token: 0x04000445 RID: 1093
	private bool dropped;
}
