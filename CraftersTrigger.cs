using System;
using UnityEngine;

// Token: 0x02000012 RID: 18
public class CraftersTrigger : MonoBehaviour
{
	// Token: 0x06000062 RID: 98 RVA: 0x000048B2 File Offset: 0x00002AB2
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" && this.crafters != null && Random.value <= this.spawnChance)
		{
			this.crafters.SpawnAt(this.target);
		}
	}

	// Token: 0x04000094 RID: 148
	public ArtsAndCrafters crafters;

	// Token: 0x04000095 RID: 149
	public IntVector2 target;

	// Token: 0x04000096 RID: 150
	public float spawnChance;
}
