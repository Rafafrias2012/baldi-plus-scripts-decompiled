using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200021A RID: 538
[ExecuteInEditMode]
[AddComponentMenu("Volumetric Audio/VA Audio Listener")]
public class VA_AudioListener : MonoBehaviour
{
	// Token: 0x06000C09 RID: 3081 RVA: 0x0003F4F2 File Offset: 0x0003D6F2
	protected virtual void OnEnable()
	{
		VA_AudioListener.Instances.Add(this);
		if (VA_AudioListener.Instances.Count > 1)
		{
			Debug.LogWarning("Your scene already contains an active and enabled VA_AudioListener", VA_AudioListener.Instances[0]);
		}
	}

	// Token: 0x06000C0A RID: 3082 RVA: 0x0003F521 File Offset: 0x0003D721
	protected virtual void OnDisable()
	{
		VA_AudioListener.Instances.Remove(this);
	}

	// Token: 0x04000E93 RID: 3731
	public static List<VA_AudioListener> Instances = new List<VA_AudioListener>();
}
