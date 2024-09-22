using System;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x020000CC RID: 204
public class AgentTest : MonoBehaviour
{
	// Token: 0x060004E1 RID: 1249 RVA: 0x00019674 File Offset: 0x00017874
	private void Start()
	{
		this.agent.Warp(new Vector3(-5f, 0f, 5f));
	}

	// Token: 0x060004E2 RID: 1250 RVA: 0x00019696 File Offset: 0x00017896
	private void Update()
	{
		this.agent.SetDestination(new Vector3(5f, 5f, 5f));
	}

	// Token: 0x04000529 RID: 1321
	public NavMeshAgent agent;
}
