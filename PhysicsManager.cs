using System;
using UnityEngine;

// Token: 0x02000124 RID: 292
public class PhysicsManager : MonoBehaviour
{
	// Token: 0x0600070C RID: 1804 RVA: 0x00023878 File Offset: 0x00021A78
	private void Update()
	{
		if (Time.timeScale > 0f && Time.deltaTime > 0f)
		{
			Entity.UpdateAllEntities();
			if (!Physics.autoSimulation)
			{
				Entity.UpdateAllEntityTriggerStates();
				if (Time.deltaTime > 0f)
				{
					Physics.Simulate(Time.deltaTime);
				}
			}
			Entity.UpdateAllEntityStates();
			if (!Physics.autoSyncTransforms)
			{
				Physics.SyncTransforms();
			}
		}
	}
}
