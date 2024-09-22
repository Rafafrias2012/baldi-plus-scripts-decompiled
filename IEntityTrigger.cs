using System;
using UnityEngine;

// Token: 0x020000E2 RID: 226
public interface IEntityTrigger
{
	// Token: 0x06000559 RID: 1369
	void EntityTriggerEnter(Collider other);

	// Token: 0x0600055A RID: 1370
	void EntityTriggerStay(Collider other);

	// Token: 0x0600055B RID: 1371
	void EntityTriggerExit(Collider other);
}
