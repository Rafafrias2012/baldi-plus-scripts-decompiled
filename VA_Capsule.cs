using System;
using UnityEngine;

// Token: 0x0200021D RID: 541
[ExecuteInEditMode]
[AddComponentMenu("Volumetric Audio/VA Capsule")]
public class VA_Capsule : VA_VolumetricShape
{
	// Token: 0x06000C1E RID: 3102 RVA: 0x0003FE4C File Offset: 0x0003E04C
	public void RebuildMatrix()
	{
		Vector3 t = base.transform.TransformPoint(this.Center);
		Quaternion quaternion = base.transform.rotation;
		Vector3 lossyScale = base.transform.lossyScale;
		switch (this.Direction)
		{
		case 0:
			quaternion *= VA_Capsule.RotationX;
			break;
		case 1:
			quaternion *= VA_Capsule.RotationY;
			break;
		case 2:
			quaternion *= VA_Capsule.RotationZ;
			break;
		}
		lossyScale.x = (lossyScale.y = (lossyScale.z = Mathf.Max(lossyScale.x, lossyScale.z)));
		VA_Helper.MatrixTrs(t, quaternion, lossyScale, ref this.cachedMatrix);
	}

	// Token: 0x06000C1F RID: 3103 RVA: 0x0003FF04 File Offset: 0x0003E104
	public override bool LocalPointInShape(Vector3 localPoint)
	{
		float halfHeight = Mathf.Max(0f, this.Height * 0.5f - this.Radius);
		return this.LocalPointInCapsule(localPoint, halfHeight);
	}

	// Token: 0x06000C20 RID: 3104 RVA: 0x0003FF37 File Offset: 0x0003E137
	protected virtual void Awake()
	{
		this.CapsuleCollider = base.GetComponent<CapsuleCollider>();
	}

	// Token: 0x06000C21 RID: 3105 RVA: 0x0003FF48 File Offset: 0x0003E148
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
			Vector3 lossyScale = base.transform.lossyScale;
			float num = VA_Helper.Divide(lossyScale.y, Mathf.Max(lossyScale.x, lossyScale.z));
			float halfHeight = Mathf.Max(0f, this.Height * num * 0.5f - this.Radius);
			if (this.IsHollow)
			{
				vector3 = this.SnapLocalPoint(vector3, halfHeight);
				vector2 = this.cachedMatrix.MultiplyPoint(vector3);
				base.SetOuterPoint(vector2);
				return;
			}
			if (this.LocalPointInCapsule(vector3, halfHeight))
			{
				base.SetInnerPoint(vector2, true);
				vector3 = this.SnapLocalPoint(vector3, halfHeight);
				vector2 = this.cachedMatrix.MultiplyPoint(vector3);
				base.SetOuterPoint(vector2);
				return;
			}
			vector3 = this.SnapLocalPoint(vector3, halfHeight);
			vector2 = this.cachedMatrix.MultiplyPoint(vector3);
			base.SetInnerOuterPoint(vector2, false);
		}
	}

	// Token: 0x06000C22 RID: 3106 RVA: 0x00040058 File Offset: 0x0003E258
	private void UpdateFields()
	{
		if (this.CapsuleCollider != null)
		{
			this.Center = this.CapsuleCollider.center;
			this.Radius = this.CapsuleCollider.radius;
			this.Height = this.CapsuleCollider.height;
			this.Direction = this.CapsuleCollider.direction;
		}
	}

	// Token: 0x06000C23 RID: 3107 RVA: 0x000400B8 File Offset: 0x0003E2B8
	private bool LocalPointInCapsule(Vector3 localPoint, float halfHeight)
	{
		if (localPoint.y > halfHeight)
		{
			localPoint.y -= halfHeight;
			return localPoint.sqrMagnitude < this.Radius * this.Radius;
		}
		if (localPoint.y < -halfHeight)
		{
			localPoint.y += halfHeight;
			return localPoint.sqrMagnitude < this.Radius * this.Radius;
		}
		localPoint.y = 0f;
		return localPoint.sqrMagnitude < this.Radius * this.Radius;
	}

	// Token: 0x06000C24 RID: 3108 RVA: 0x00040140 File Offset: 0x0003E340
	private Vector3 SnapLocalPoint(Vector3 localPoint, float halfHeight)
	{
		if (localPoint.y > halfHeight)
		{
			localPoint.y -= halfHeight;
			localPoint = localPoint.normalized * this.Radius;
			localPoint.y += halfHeight;
		}
		else if (localPoint.y < -halfHeight)
		{
			localPoint.y += halfHeight;
			localPoint = localPoint.normalized * this.Radius;
			localPoint.y -= halfHeight;
		}
		else
		{
			float y = localPoint.y;
			localPoint.y = 0f;
			localPoint = localPoint.normalized * this.Radius;
			localPoint.y = y;
		}
		return localPoint;
	}

	// Token: 0x04000EB0 RID: 3760
	[Tooltip("If you set this, then all shape settings will automatically be copied from the collider")]
	public CapsuleCollider CapsuleCollider;

	// Token: 0x04000EB1 RID: 3761
	[Tooltip("The center of the capsule shape (if you set CapsuleCollider, this will be automatically overwritten)")]
	public Vector3 Center;

	// Token: 0x04000EB2 RID: 3762
	[Tooltip("The radius of the capsule shape (if you set CapsuleCollider, this will be automatically overwritten)")]
	public float Radius = 1f;

	// Token: 0x04000EB3 RID: 3763
	[Tooltip("The height of the capsule shape (if you set CapsuleCollider, this will be automatically overwritten)")]
	public float Height = 2f;

	// Token: 0x04000EB4 RID: 3764
	[Tooltip("The direction of the capsule shape (if you set CapsuleCollider, this will be automatically overwritten)")]
	[VA_Popup(new string[]
	{
		"X-Axis",
		"Y-Axis",
		"Z-Axis"
	})]
	public int Direction = 1;

	// Token: 0x04000EB5 RID: 3765
	private static Quaternion RotationX = Quaternion.Euler(0f, 0f, 90f);

	// Token: 0x04000EB6 RID: 3766
	private static Quaternion RotationY = Quaternion.identity;

	// Token: 0x04000EB7 RID: 3767
	private static Quaternion RotationZ = Quaternion.Euler(90f, 0f, 0f);

	// Token: 0x04000EB8 RID: 3768
	private Matrix4x4 cachedMatrix = Matrix4x4.identity;
}
