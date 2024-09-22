using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x0200021B RID: 539
[ExecuteInEditMode]
[AddComponentMenu("Volumetric Audio/VA Audio Source")]
public class VA_AudioSource : MonoBehaviour
{
	// Token: 0x170000FE RID: 254
	// (get) Token: 0x06000C0D RID: 3085 RVA: 0x0003F544 File Offset: 0x0003D744
	public bool HasVolumetricShape
	{
		get
		{
			for (int i = this.Shapes.Count - 1; i >= 0; i--)
			{
				VA_Shape va_Shape = this.Shapes[i];
				if (va_Shape != null)
				{
					VA_Sphere va_Sphere = va_Shape as VA_Sphere;
					if (va_Sphere != null && !va_Sphere.IsHollow)
					{
						return true;
					}
					VA_Box va_Box = va_Shape as VA_Box;
					if (va_Box != null && !va_Box.IsHollow)
					{
						return true;
					}
					VA_Capsule va_Capsule = va_Shape as VA_Capsule;
					if (va_Capsule != null && !va_Capsule.IsHollow)
					{
						return true;
					}
					VA_Mesh va_Mesh = va_Shape as VA_Mesh;
					if (va_Mesh != null && !va_Mesh.IsHollow)
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	// Token: 0x06000C0E RID: 3086 RVA: 0x0003F5F4 File Offset: 0x0003D7F4
	protected virtual void Start()
	{
		if (this.BlendCurve == null)
		{
			this.BlendCurve = new AnimationCurve(VA_AudioSource.defaultBlendCurveKeys);
		}
		if (this.FadeCurve == null)
		{
			this.FadeCurve = new AnimationCurve(VA_AudioSource.defaultVolumeCurveKeys);
		}
	}

	// Token: 0x06000C0F RID: 3087 RVA: 0x0003F628 File Offset: 0x0003D828
	protected virtual void LateUpdate()
	{
		Vector3 vector = default(Vector3);
		if (VA_Helper.GetListenerPosition(ref vector))
		{
			if (this.Position)
			{
				float num = float.PositiveInfinity;
				VA_Shape x = null;
				Vector3 vector2 = default(Vector3);
				if (this.Shapes != null)
				{
					for (int i = this.Shapes.Count - 1; i >= 0; i--)
					{
						VA_Shape va_Shape = this.Shapes[i];
						if (VA_Helper.Enabled(va_Shape) && va_Shape.FinalPointSet && va_Shape.FinalPointDistance < num)
						{
							num = va_Shape.FinalPointDistance;
							vector2 = va_Shape.FinalPoint;
							x = va_Shape;
						}
					}
				}
				if (this.ExcludedShapes != null)
				{
					for (int j = this.ExcludedShapes.Count - 1; j >= 0; j--)
					{
						VA_VolumetricShape va_VolumetricShape = this.ExcludedShapes[j];
						if (VA_Helper.Enabled(va_VolumetricShape) && !va_VolumetricShape.IsHollow && va_VolumetricShape.InnerPointInside && va_VolumetricShape.OuterPointSet && va_VolumetricShape.OuterPointDistance > num)
						{
							num = va_VolumetricShape.OuterPointDistance;
							vector2 = va_VolumetricShape.OuterPoint;
							x = va_VolumetricShape;
							break;
						}
					}
				}
				if (x != null)
				{
					if (this.PositionDampening <= 0f)
					{
						base.transform.position = vector2;
					}
					else
					{
						base.transform.position = VA_Helper.Dampen3(base.transform.position, vector2, this.PositionDampening, Time.deltaTime, 0f);
					}
				}
				else
				{
					vector2 = base.transform.position;
					num = Vector3.Distance(vector2, vector);
				}
			}
			if (this.Blend)
			{
				float value = Vector3.Distance(base.transform.position, vector);
				float time = Mathf.InverseLerp(this.BlendMinDistance, this.BlendMaxDistance, value);
				this.SetPanLevel(this.BlendCurve.Evaluate(time));
			}
			if (this.Volume)
			{
				float num2 = this.BaseVolume;
				if (this.Zone != null)
				{
					num2 *= this.Zone.Volume;
				}
				if (this.Fade)
				{
					float value2 = Vector3.Distance(base.transform.position, vector);
					float time2 = Mathf.InverseLerp(this.FadeMinDistance, this.FadeMaxDistance, value2);
					num2 *= this.FadeCurve.Evaluate(time2);
				}
				if (this.Occlude)
				{
					Vector3 direction = vector - base.transform.position;
					float num3 = 1f;
					if (this.OccludeGroups != null)
					{
						for (int k = this.OccludeGroups.Count - 1; k >= 0; k--)
						{
							VA_AudioSource.OccludeGroup occludeGroup = this.OccludeGroups[k];
							VA_AudioSource.OccludeType occludeMethod = this.OccludeMethod;
							if (occludeMethod != VA_AudioSource.OccludeType.Raycast)
							{
								if (occludeMethod == VA_AudioSource.OccludeType.RaycastAll)
								{
									RaycastHit[] array = Physics.RaycastAll(base.transform.position, direction, direction.magnitude, occludeGroup.Layers);
									for (int l = array.Length - 1; l >= 0; l--)
									{
										num3 *= this.GetOcclusionVolume(occludeGroup, array[l]);
									}
								}
							}
							else
							{
								RaycastHit hit = default(RaycastHit);
								if (Physics.Raycast(base.transform.position, direction, out hit, direction.magnitude, occludeGroup.Layers))
								{
									num3 *= this.GetOcclusionVolume(occludeGroup, hit);
								}
							}
						}
					}
					this.OccludeAmount = VA_Helper.Dampen(this.OccludeAmount, num3, this.OccludeDampening, Time.deltaTime, 0.1f);
					num2 *= this.OccludeAmount;
				}
				this.SetVolume(num2);
			}
		}
	}

	// Token: 0x06000C10 RID: 3088 RVA: 0x0003F99C File Offset: 0x0003DB9C
	private float GetOcclusionVolume(VA_AudioSource.OccludeGroup group, RaycastHit hit)
	{
		if (this.OccludeMaterial)
		{
			VA_Material componentInParent = hit.collider.GetComponentInParent<VA_Material>();
			if (componentInParent != null)
			{
				return componentInParent.OcclusionVolume;
			}
		}
		return group.Volume;
	}

	// Token: 0x06000C11 RID: 3089 RVA: 0x0003F9D4 File Offset: 0x0003DBD4
	protected virtual void SetPanLevel(float newPanLevel)
	{
		if (this.audioSource == null)
		{
			this.audioSource = base.GetComponent<AudioSource>();
		}
		if (this.audioSource != null)
		{
			this.audioSource.spatialBlend = newPanLevel;
		}
	}

	// Token: 0x06000C12 RID: 3090 RVA: 0x0003FA0A File Offset: 0x0003DC0A
	protected virtual void SetVolume(float newVolume)
	{
		if (this.audioSource == null)
		{
			this.audioSource = base.GetComponent<AudioSource>();
		}
		if (this.audioSource != null)
		{
			this.audioSource.volume = newVolume;
		}
	}

	// Token: 0x04000E94 RID: 3732
	[Tooltip("Should this sound have its position update?")]
	public bool Position = true;

	// Token: 0x04000E95 RID: 3733
	[Tooltip("The speed at which the sound position changes (0 = instant)")]
	public float PositionDampening;

	// Token: 0x04000E96 RID: 3734
	[Tooltip("The shapes you want the audio source to emit from")]
	public List<VA_Shape> Shapes;

	// Token: 0x04000E97 RID: 3735
	[Tooltip("The shapes you want the audio source to be excluded from")]
	public List<VA_VolumetricShape> ExcludedShapes;

	// Token: 0x04000E98 RID: 3736
	[Tooltip("Should this sound have its Spatial Blend update?")]
	public bool Blend;

	// Token: 0x04000E99 RID: 3737
	[Tooltip("The distance at which the sound becomes fuly mono")]
	public float BlendMinDistance;

	// Token: 0x04000E9A RID: 3738
	[Tooltip("The distance at which the sound becomes fuly stereo")]
	public float BlendMaxDistance = 5f;

	// Token: 0x04000E9B RID: 3739
	[Tooltip("The distribution of the mono to stereo ratio")]
	public AnimationCurve BlendCurve;

	// Token: 0x04000E9C RID: 3740
	[Tooltip("Should this sound have its volume update?")]
	public bool Volume = true;

	// Token: 0x04000E9D RID: 3741
	[Tooltip("The base volume of the audio source")]
	[Range(0f, 1f)]
	public float BaseVolume = 1f;

	// Token: 0x04000E9E RID: 3742
	[Tooltip("The zone this sound is associated with")]
	public VA_Zone Zone;

	// Token: 0x04000E9F RID: 3743
	[Tooltip("Should the volume fade based on distance?")]
	[FormerlySerializedAs("Volume")]
	public bool Fade;

	// Token: 0x04000EA0 RID: 3744
	[Tooltip("The distance at which the sound fades to maximum volume")]
	[FormerlySerializedAs("VolumeMinDistance")]
	public float FadeMinDistance;

	// Token: 0x04000EA1 RID: 3745
	[Tooltip("The distance at which the sound fades to minimum volume")]
	[FormerlySerializedAs("VolumeMaxDistance")]
	public float FadeMaxDistance = 5f;

	// Token: 0x04000EA2 RID: 3746
	[Tooltip("The distribution of volume based on its scaled distance")]
	[FormerlySerializedAs("VolumeCurve")]
	public AnimationCurve FadeCurve;

	// Token: 0x04000EA3 RID: 3747
	[Tooltip("Should this sound be blocked when behind other objects?")]
	public bool Occlude;

	// Token: 0x04000EA4 RID: 3748
	[Tooltip("The raycast style against the occlusion groups")]
	public VA_AudioSource.OccludeType OccludeMethod;

	// Token: 0x04000EA5 RID: 3749
	[Tooltip("Check for VA_Material instances attached to the occlusion object")]
	public bool OccludeMaterial;

	// Token: 0x04000EA6 RID: 3750
	[Tooltip("How quickly the sound fades in/out when behind an object")]
	[FormerlySerializedAs("OccludeSpeed")]
	public float OccludeDampening = 5f;

	// Token: 0x04000EA7 RID: 3751
	[Tooltip("The amount of occlusion checks")]
	public List<VA_AudioSource.OccludeGroup> OccludeGroups;

	// Token: 0x04000EA8 RID: 3752
	public float OccludeAmount = 1f;

	// Token: 0x04000EA9 RID: 3753
	[NonSerialized]
	private AudioSource audioSource;

	// Token: 0x04000EAA RID: 3754
	private static Keyframe[] defaultBlendCurveKeys = new Keyframe[]
	{
		new Keyframe(0f, 0f),
		new Keyframe(1f, 1f)
	};

	// Token: 0x04000EAB RID: 3755
	private static Keyframe[] defaultVolumeCurveKeys = new Keyframe[]
	{
		new Keyframe(0f, 1f),
		new Keyframe(1f, 0f)
	};

	// Token: 0x0200039A RID: 922
	public enum OccludeType
	{
		// Token: 0x04001CDF RID: 7391
		Raycast,
		// Token: 0x04001CE0 RID: 7392
		RaycastAll
	}

	// Token: 0x0200039B RID: 923
	[Serializable]
	public class OccludeGroup
	{
		// Token: 0x04001CE1 RID: 7393
		public LayerMask Layers;

		// Token: 0x04001CE2 RID: 7394
		[Range(0f, 1f)]
		public float Volume;
	}
}
