using System;
using UnityEngine;

// Token: 0x0200021C RID: 540
[ExecuteInEditMode]
[AddComponentMenu("Volumetric Audio/VA Box")]
public class VA_Box : VA_VolumetricShape
{
	// Token: 0x06000C15 RID: 3093 RVA: 0x0003FB14 File Offset: 0x0003DD14
	public void RebuildMatrix()
	{
		Vector3 t = base.transform.TransformPoint(this.Center);
		Quaternion rotation = base.transform.rotation;
		Vector3 lossyScale = base.transform.lossyScale;
		lossyScale.x *= this.Size.x;
		lossyScale.y *= this.Size.y;
		lossyScale.z *= this.Size.z;
		VA_Helper.MatrixTrs(t, rotation, lossyScale, ref this.cachedMatrix);
	}

	// Token: 0x06000C16 RID: 3094 RVA: 0x0003FB99 File Offset: 0x0003DD99
	public override bool LocalPointInShape(Vector3 localPoint)
	{
		return this.LocalPointInBox(localPoint);
	}

	// Token: 0x06000C17 RID: 3095 RVA: 0x0003FBA2 File Offset: 0x0003DDA2
	protected virtual void Reset()
	{
		this.BoxCollider = base.GetComponent<BoxCollider>();
	}

	// Token: 0x06000C18 RID: 3096 RVA: 0x0003FBB0 File Offset: 0x0003DDB0
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
			if (this.LocalPointInBox(vector3))
			{
				base.SetInnerPoint(vector2, true);
				vector3 = this.SnapLocalPoint(vector3);
				vector2 = this.cachedMatrix.MultiplyPoint(vector3);
				base.SetOuterPoint(vector2);
				return;
			}
			vector3 = this.ClipLocalPoint(vector3);
			vector2 = this.cachedMatrix.MultiplyPoint(vector3);
			base.SetInnerOuterPoint(vector2, false);
		}
	}

	// Token: 0x06000C19 RID: 3097 RVA: 0x0003FC6A File Offset: 0x0003DE6A
	private void UpdateFields()
	{
		if (this.BoxCollider != null)
		{
			this.Center = this.BoxCollider.center;
			this.Size = this.BoxCollider.size;
		}
	}

	// Token: 0x06000C1A RID: 3098 RVA: 0x0003FC9C File Offset: 0x0003DE9C
	private bool LocalPointInBox(Vector3 localPoint)
	{
		return localPoint.x >= -0.5f && localPoint.x <= 0.5f && localPoint.y >= -0.5f && localPoint.y <= 0.5f && localPoint.z >= -0.5f && localPoint.z <= 0.5f;
	}

	// Token: 0x06000C1B RID: 3099 RVA: 0x0003FD04 File Offset: 0x0003DF04
	private Vector3 SnapLocalPoint(Vector3 localPoint)
	{
		float num = Mathf.Abs(localPoint.x);
		float num2 = Mathf.Abs(localPoint.y);
		float num3 = Mathf.Abs(localPoint.z);
		if (num > num2 && num > num3)
		{
			localPoint *= VA_Helper.Reciprocal(num * 2f);
		}
		else if (num2 > num && num2 > num3)
		{
			localPoint *= VA_Helper.Reciprocal(num2 * 2f);
		}
		else
		{
			localPoint *= VA_Helper.Reciprocal(num3 * 2f);
		}
		return localPoint;
	}

	// Token: 0x06000C1C RID: 3100 RVA: 0x0003FD88 File Offset: 0x0003DF88
	private Vector3 ClipLocalPoint(Vector3 localPoint)
	{
		if (localPoint.x < -0.5f)
		{
			localPoint.x = -0.5f;
		}
		if (localPoint.x > 0.5f)
		{
			localPoint.x = 0.5f;
		}
		if (localPoint.y < -0.5f)
		{
			localPoint.y = -0.5f;
		}
		if (localPoint.y > 0.5f)
		{
			localPoint.y = 0.5f;
		}
		if (localPoint.z < -0.5f)
		{
			localPoint.z = -0.5f;
		}
		if (localPoint.z > 0.5f)
		{
			localPoint.z = 0.5f;
		}
		return localPoint;
	}

	// Token: 0x04000EAC RID: 3756
	[Tooltip("If you set this, then all shape settings will automatically be copied from the collider")]
	public BoxCollider BoxCollider;

	// Token: 0x04000EAD RID: 3757
	[Tooltip("The center of the box shape (if you set BoxCollider, this will be automatically overwritten)")]
	public Vector3 Center;

	// Token: 0x04000EAE RID: 3758
	[Tooltip("The size of the box shape (if you set BoxCollider, this will be automatically overwritten)")]
	public Vector3 Size = Vector3.one;

	// Token: 0x04000EAF RID: 3759
	private Matrix4x4 cachedMatrix = Matrix4x4.identity;
}
