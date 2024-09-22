using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200022A RID: 554
[ExecuteInEditMode]
[AddComponentMenu("Volumetric Audio/VA Zone")]
public class VA_Zone : MonoBehaviour
{
	// Token: 0x06000C89 RID: 3209 RVA: 0x00041DE4 File Offset: 0x0003FFE4
	protected virtual void Update()
	{
		Vector3 a = default(Vector3);
		if (VA_Helper.GetListenerPosition(ref a))
		{
			float target = 0f;
			if (Vector3.Distance(a, base.transform.position) <= this.Radius)
			{
				target = 1f;
			}
			this.Volume = VA_Helper.Dampen(this.Volume, target, this.VolumeDampening, Time.deltaTime, 0.1f);
			if (this.AudioSources != null)
			{
				for (int i = this.AudioSources.Count - 1; i >= 0; i--)
				{
					VA_AudioSource va_AudioSource = this.AudioSources[i];
					if (va_AudioSource != null)
					{
						va_AudioSource.Zone = this;
						if (this.Volume > 0f)
						{
							if (!va_AudioSource.gameObject.activeSelf)
							{
								va_AudioSource.gameObject.SetActive(true);
							}
							if (!va_AudioSource.enabled)
							{
								va_AudioSource.enabled = true;
							}
						}
						else if (this.DeactivateGameObjects)
						{
							if (va_AudioSource.gameObject.activeSelf)
							{
								va_AudioSource.gameObject.SetActive(false);
							}
						}
						else if (va_AudioSource.enabled)
						{
							va_AudioSource.enabled = false;
						}
					}
				}
			}
		}
	}

	// Token: 0x04000EE2 RID: 3810
	[Tooltip("The radius of this zone")]
	public float Radius = 1f;

	// Token: 0x04000EE3 RID: 3811
	[Tooltip("Should the ")]
	public bool DeactivateGameObjects;

	// Token: 0x04000EE4 RID: 3812
	[Tooltip("The speed at which the volume changes")]
	public float VolumeDampening = 10f;

	// Token: 0x04000EE5 RID: 3813
	public float Volume;

	// Token: 0x04000EE6 RID: 3814
	[Tooltip("The audio sources this zone is associated with")]
	public List<VA_AudioSource> AudioSources;
}
