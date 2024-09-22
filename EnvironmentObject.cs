using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200016E RID: 366
public class EnvironmentObject : MonoBehaviour
{
	// Token: 0x170000C1 RID: 193
	// (get) Token: 0x060008B8 RID: 2232 RVA: 0x0002CB77 File Offset: 0x0002AD77
	// (set) Token: 0x060008B9 RID: 2233 RVA: 0x0002CB7F File Offset: 0x0002AD7F
	public EnvironmentController Ec
	{
		get
		{
			return this.ec;
		}
		set
		{
			this.ec = value;
		}
	}

	// Token: 0x060008BA RID: 2234 RVA: 0x0002CB88 File Offset: 0x0002AD88
	public virtual void LoadingFinished()
	{
	}

	// Token: 0x060008BB RID: 2235 RVA: 0x0002CB8C File Offset: 0x0002AD8C
	public static void GiveController(Transform currentObject, EnvironmentController currentController, List<EnvironmentObject> levelBuilderEnvironmentObjectList)
	{
		EnvironmentObject[] components = currentObject.GetComponents<EnvironmentObject>();
		if (currentObject != null)
		{
			for (int i = 0; i < components.Length; i++)
			{
				components[i].Ec = currentController;
				levelBuilderEnvironmentObjectList.Add(components[i]);
			}
		}
	}

	// Token: 0x04000937 RID: 2359
	[SerializeField]
	protected EnvironmentController ec;
}
