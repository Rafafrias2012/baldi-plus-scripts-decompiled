using System;
using UnityEngine;

// Token: 0x0200010A RID: 266
public class GlobalSystemInitializer : MonoBehaviour
{
	// Token: 0x0600068D RID: 1677 RVA: 0x00021063 File Offset: 0x0001F263
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	public static void LoadMain()
	{
		Object.DontDestroyOnLoad(Object.Instantiate(Resources.Load("Main")) as GameObject);
	}
}
