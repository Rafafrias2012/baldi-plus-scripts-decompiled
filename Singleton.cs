using System;
using UnityEngine;

// Token: 0x02000136 RID: 310
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	// Token: 0x0600074D RID: 1869 RVA: 0x00025A38 File Offset: 0x00023C38
	private void Awake()
	{
		if (Singleton<T>.m_Instance == null)
		{
			Singleton<T>.m_Instance = (T)((object)Object.FindObjectOfType(typeof(T)));
			if (!this.destroyOnLoad)
			{
				Object.DontDestroyOnLoad(base.gameObject);
			}
		}
		else if (Singleton<T>.m_Instance != this)
		{
			Object.Destroy(base.gameObject);
		}
		this.AwakeFunction();
	}

	// Token: 0x0600074E RID: 1870 RVA: 0x00025AA8 File Offset: 0x00023CA8
	protected virtual void AwakeFunction()
	{
	}

	// Token: 0x1700008B RID: 139
	// (get) Token: 0x0600074F RID: 1871 RVA: 0x00025AAA File Offset: 0x00023CAA
	public static T Instance
	{
		get
		{
			return Singleton<T>.m_Instance;
		}
	}

	// Token: 0x04000803 RID: 2051
	private static T m_Instance;

	// Token: 0x04000804 RID: 2052
	[SerializeField]
	private bool destroyOnLoad;
}
