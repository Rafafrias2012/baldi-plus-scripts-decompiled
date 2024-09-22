using System;
using UnityEngine;

// Token: 0x020001B5 RID: 437
public class SpecialManagerFunctionTrigger : MonoBehaviour
{
	// Token: 0x060009DD RID: 2525 RVA: 0x00034F88 File Offset: 0x00033188
	private void OnTriggerEnter(Collider other)
	{
		if ((!this.triggered || !this.triggerOnce) && other.CompareTag("Player"))
		{
			Singleton<BaseGameManager>.Instance.CallSpecialManagerFunction(this.functionIndexVal, base.gameObject);
			this.triggered = true;
		}
	}

	// Token: 0x04000B29 RID: 2857
	[SerializeField]
	private int functionIndexVal;

	// Token: 0x04000B2A RID: 2858
	[SerializeField]
	private bool triggerOnce = true;

	// Token: 0x04000B2B RID: 2859
	private bool triggered;
}
