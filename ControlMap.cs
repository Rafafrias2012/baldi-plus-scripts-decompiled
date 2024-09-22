using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Token: 0x02000206 RID: 518
public class ControlMap : MonoBehaviour
{
	// Token: 0x06000B81 RID: 2945 RVA: 0x0003C5A4 File Offset: 0x0003A7A4
	public void Load(InputAction action, string name, List<string> inputs)
	{
		this.action = action;
		this.label.text = name;
		string text = "";
		for (int i = 0; i < inputs.Count; i++)
		{
			if (i < inputs.Count - 1)
			{
				text = text + inputs[i] + ", ";
			}
			else
			{
				text += inputs[i];
			}
		}
		this.inputsLabel.text = text;
	}

	// Token: 0x06000B82 RID: 2946 RVA: 0x0003C615 File Offset: 0x0003A815
	public void AssignInput()
	{
	}

	// Token: 0x06000B83 RID: 2947 RVA: 0x0003C617 File Offset: 0x0003A817
	public void ResetToDefault()
	{
	}

	// Token: 0x04000DCF RID: 3535
	public ControlMapper controlMapper;

	// Token: 0x04000DD0 RID: 3536
	public InputAction action;

	// Token: 0x04000DD1 RID: 3537
	public TMP_Text label;

	// Token: 0x04000DD2 RID: 3538
	public TMP_Text inputsLabel;
}
