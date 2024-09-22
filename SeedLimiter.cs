using System;
using TMPro;
using UnityEngine;

// Token: 0x020001DD RID: 477
public class SeedLimiter : MonoBehaviour
{
	// Token: 0x06000AD5 RID: 2773 RVA: 0x0003936C File Offset: 0x0003756C
	public void CheckValue()
	{
		if (this.input.text != "" && this.input.text != "-" && (Convert.ToInt64(this.input.text) > 2147483615L || Convert.ToInt64(this.input.text) < -2147483615L))
		{
			this.input.text = this.input.text.Remove(this.input.text.Length - 1);
		}
	}

	// Token: 0x04000C5A RID: 3162
	public TMP_InputField input;
}
