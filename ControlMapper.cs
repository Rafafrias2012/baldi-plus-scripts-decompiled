using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Token: 0x02000207 RID: 519
public class ControlMapper : MonoBehaviour
{
	// Token: 0x06000B85 RID: 2949 RVA: 0x0003C621 File Offset: 0x0003A821
	private void Start()
	{
		this.Load(this.currentActionPage);
	}

	// Token: 0x06000B86 RID: 2950 RVA: 0x0003C630 File Offset: 0x0003A830
	public void Load(int page)
	{
		this._current = this.actionPages[page];
		List<string> inputs = new List<string>();
		this.pageLabel.text = this._current.name;
		for (int i = 0; i < this.controlMaps.Length; i++)
		{
			if (i < this._current.input.Length)
			{
				this.controlMaps[i].gameObject.SetActive(true);
				InputAction action = this._current.input[i];
				Singleton<PlayerFileManager>.Instance.GetInputStringsForAction(action, "Test", out inputs);
				this.controlMaps[i].Load(this._current.input[i], this._current.inputName[i], inputs);
			}
			else
			{
				this.controlMaps[i].gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06000B87 RID: 2951 RVA: 0x0003C704 File Offset: 0x0003A904
	public void ChangePage(int dir)
	{
		this.currentActionPage += dir;
		if (this.currentActionPage >= this.actionPages.Count)
		{
			this.currentActionPage = 0;
		}
		else if (this.currentActionPage < 0)
		{
			this.currentActionPage = this.actionPages.Count - 1;
		}
		this.Load(this.currentActionPage);
	}

	// Token: 0x04000DD3 RID: 3539
	public TMP_Text pageLabel;

	// Token: 0x04000DD4 RID: 3540
	[SerializeField]
	private ControlMap[] controlMaps = new ControlMap[0];

	// Token: 0x04000DD5 RID: 3541
	public List<InputPage> actionPages = new List<InputPage>();

	// Token: 0x04000DD6 RID: 3542
	private InputPage _current;

	// Token: 0x04000DD7 RID: 3543
	private int currentActionPage;
}
