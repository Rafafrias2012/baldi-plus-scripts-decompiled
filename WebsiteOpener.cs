using System;
using UnityEngine;

// Token: 0x02000146 RID: 326
public class WebsiteOpener : MonoBehaviour
{
	// Token: 0x0600077D RID: 1917 RVA: 0x000264EE File Offset: 0x000246EE
	public void OpenPage()
	{
		Application.OpenURL(this.url);
	}

	// Token: 0x0600077E RID: 1918 RVA: 0x000264FB File Offset: 0x000246FB
	public void OpenSave()
	{
		Debug.Log(Application.persistentDataPath);
		Application.OpenURL(Application.persistentDataPath);
	}

	// Token: 0x04000841 RID: 2113
	public string url;
}
