using System;
using UnityEngine;

// Token: 0x02000227 RID: 551
[ExecuteInEditMode]
[AddComponentMenu("Volumetric Audio/VA Sphere")]
public class VA_Sphere : VA_VolumetricShape
{
	// Token: 0x06000C6F RID: 3183 RVA: 0x0004186C File Offset: 0x0003FA6C
	public void RebuildMatrix()
	{
		Vector3 t = base.transform.TransformPoint(this.Center);
		Quaternion rotation = base.transform.rotation;
		Vector3 lossyScale = base.transform.lossyScale;
		lossyScale.x = (lossyScale.y = (lossyScale.z = Mathf.Max(Mathf.Max(lossyScale.x, lossyScale.y), lossyScale.z)));
		VA_Helper.MatrixTrs(t, rotation, lossyScale, ref this.cachedMatrix);
	}

	// Token: 0x06000C70 RID: 3184 RVA: 0x000418E6 File Offset: 0x0003FAE6
	public override bool LocalPointInShape(Vector3 localPoint)
	{
		return this.LocalPointInSphere(localPoint);
	}

	// Token: 0x06000C71 RID: 3185 RVA: 0x000418EF File Offset: 0x0003FAEF
	protected virtual void Awake()
	{
		this.SphereCollider = base.GetComponent<SphereCollider>();
	}

	// Token: 0x06000C72 RID: 3186 RVA: 0x00041900 File Offset: 0x0003FB00
	protected override void LateUpdate()
	{
		base.LateUpdate();
		Vector3 vector = default(Vector3);
		if (VA_Helper.GetListenerPosition(ref vector))
		{
			this.UpdateFields();
			this.RebuildMatrix();
			Vector3 vector2 = vector;
			Vector3 vector3 = this.cachedMatrix.inverse.MultiplyPoint(vector2);
			if (this.IsHollow)
			{
				vector3 = this.SnapLocalPoint(vector3);
				vector2 = this.cachedMatrix.MultiplyPoint(vector3);
				base.SetOuterPoint(vector2);
				return;
			}
			if (this.LocalPointInSphere(vector3))
			{
				base.SetInnerPoint(vector2, true);
				vector3 = this.SnapLocalPoint(vector3);
				vector2 = this.cachedMatrix.MultiplyPoint(vector3);
				base.SetOuterPoint(vector2);
				return;
			}
			vector3 = this.SnapLocalPoint(vector3);
			vector2 = this.cachedMatrix.MultiplyPoint(vector3);
			base.SetInnerOuterPoint(vector2, false);
		}
	}

	// Token: 0x06000C73 RID: 3187 RVA: 0x000419BA File Offset: 0x0003FBBA
	private void UpdateFields()
	{
		if (this.SphereCollider != null)
		{
			this.Center = this.SphereCollider.center;
			this.Radius = this.SphereCollider.radius;
		}
	}

	// Token: 0x06000C74 RID: 3188 RVA: 0x000419EC File Offset: 0x0003FBEC
	private bool LocalPointInSphere(Vector3 localPoint)
	{
		return localPoint.sqrMagnitude < this.Radius * this.Radius;
	}

	// Token: 0x06000C75 RID: 3189 RVA: 0x00041A04 File Offset: 0x0003FC04
	private Vector3 SnapLocalPoint(Vector3 localPoint)
	{
		return localPoint.normalized * this.Radius;
	}

	// Token: 0x04000ED2 RID: 3794
	[Tooltip("If you set this, then all shape settings will automatically be copied from the collider")]
	public SphereCollider SphereCollider;

	// Token: 0x04000ED3 RID: 3795
	[Tooltip("The center of the sphere shape (if you set SphereCollider, this will be automatically overwritten)")]
	public Vector3 Center;

	// Token: 0x04000ED4 RID: 3796
	[Tooltip("The radius of the sphere shape (if you set SphereCollider, this will be automatically overwritten)")]
	public float Radius = 1f;

	// Token: 0x04000ED5 RID: 3797
	private Matrix4x4 cachedMatrix = Matrix4x4.identity;
}
