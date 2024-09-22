using System;
using UnityEngine;

// Token: 0x020000D4 RID: 212
public class LightingDebug : MonoBehaviour
{
	// Token: 0x060004F0 RID: 1264 RVA: 0x00019844 File Offset: 0x00017A44
	private void OnGUI()
	{
		if (GUI.Button(new Rect(10f, 100f, 100f, 30f), "All On"))
		{
			this.LightsOn();
		}
		if (GUI.Button(new Rect(10f, 140f, 100f, 30f), "All Off"))
		{
			this.LightsOff();
		}
		if (GUI.Button(new Rect(10f, 180f, 100f, 30f), "Gradient"))
		{
			this.LightsGradient();
		}
		if (GUI.Button(new Rect(10f, 220f, 100f, 30f), "Checker"))
		{
			this.LightsChecker();
		}
		if (GUI.Button(new Rect(10f, 260f, 100f, 30f), "Rave"))
		{
			this.LightsRave();
		}
	}

	// Token: 0x060004F1 RID: 1265 RVA: 0x00019928 File Offset: 0x00017B28
	private void LightsOn()
	{
		for (int i = 0; i < this.ec.levelSize.x; i++)
		{
			for (int j = 0; j < this.ec.levelSize.z; j++)
			{
				this.ec.cells[i, j];
			}
		}
	}

	// Token: 0x060004F2 RID: 1266 RVA: 0x00019980 File Offset: 0x00017B80
	private void LightsOff()
	{
		for (int i = 0; i < this.ec.levelSize.x; i++)
		{
			for (int j = 0; j < this.ec.levelSize.z; j++)
			{
				this.ec.cells[i, j];
			}
		}
	}

	// Token: 0x060004F3 RID: 1267 RVA: 0x000199D8 File Offset: 0x00017BD8
	private void LightsGradient()
	{
		for (int i = 0; i < this.ec.levelSize.x; i++)
		{
			for (int j = 0; j < this.ec.levelSize.z; j++)
			{
				if (this.ec.cells[i, j] != null)
				{
					Mathf.Max((float)i / (float)this.ec.levelSize.x, (float)j / (float)this.ec.levelSize.z);
				}
			}
		}
	}

	// Token: 0x060004F4 RID: 1268 RVA: 0x00019A60 File Offset: 0x00017C60
	private void LightsChecker()
	{
		for (int i = 0; i < this.ec.levelSize.x; i++)
		{
			for (int j = 0; j < this.ec.levelSize.z; j++)
			{
				if (this.ec.cells[i, j] != null && (i % 2 != 0 || j % 2 != 0) && i % 2 == 1)
				{
					int num = j % 2;
				}
			}
		}
	}

	// Token: 0x060004F5 RID: 1269 RVA: 0x00019AD0 File Offset: 0x00017CD0
	private void LightsRave()
	{
		for (int i = 0; i < this.ec.levelSize.x; i++)
		{
			for (int j = 0; j < this.ec.levelSize.z; j++)
			{
				this.ec.cells[i, j];
			}
		}
	}

	// Token: 0x04000541 RID: 1345
	public EnvironmentController ec;
}
