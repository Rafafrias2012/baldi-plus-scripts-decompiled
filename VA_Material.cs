using System;
using UnityEngine;

// Token: 0x0200021F RID: 543
public class VA_Material : MonoBehaviour
{
	// Token: 0x04000EBB RID: 3771
	[Tooltip("The volume multiplier when this material is blocking the VA_AudioSource")]
	[Range(0f, 1f)]
	public float OcclusionVolume = 0.1f;
}
