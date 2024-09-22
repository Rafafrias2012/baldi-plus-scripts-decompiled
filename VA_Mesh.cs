using System;
using UnityEngine;

// Token: 0x02000220 RID: 544
[ExecuteInEditMode]
[AddComponentMenu("Volumetric Audio/VA Mesh")]
public class VA_Mesh : VA_VolumetricShape
{
	// Token: 0x170000FF RID: 255
	// (get) Token: 0x06000C43 RID: 3139 RVA: 0x0004082C File Offset: 0x0003EA2C
	public bool IsBaked
	{
		get
		{
			return this.tree != null && this.tree.Nodes != null && this.tree.Nodes.Count > 0;
		}
	}

	// Token: 0x06000C44 RID: 3140 RVA: 0x00040858 File Offset: 0x0003EA58
	public void ClearBake()
	{
		if (this.tree != null)
		{
			this.tree.Clear();
		}
	}

	// Token: 0x06000C45 RID: 3141 RVA: 0x0004086D File Offset: 0x0003EA6D
	public void Bake()
	{
		if (this.tree == null)
		{
			this.tree = new VA_MeshTree();
		}
		this.tree.Update(this.Mesh);
		if (this.linear != null)
		{
			this.linear.Clear();
		}
	}

	// Token: 0x06000C46 RID: 3142 RVA: 0x000408A8 File Offset: 0x0003EAA8
	public override bool LocalPointInShape(Vector3 localPoint)
	{
		Vector3 worldPoint = base.transform.TransformPoint(localPoint);
		return this.PointInMesh(localPoint, worldPoint);
	}

	// Token: 0x06000C47 RID: 3143 RVA: 0x000408CA File Offset: 0x0003EACA
	protected virtual void Reset()
	{
		this.IsHollow = true;
		this.MeshCollider = base.GetComponent<MeshCollider>();
		this.MeshFilter = base.GetComponent<MeshFilter>();
	}

	// Token: 0x06000C48 RID: 3144 RVA: 0x000408EC File Offset: 0x0003EAEC
	protected override void LateUpdate()
	{
		base.LateUpdate();
		if (this.meshUpdateCooldown > this.MeshUpdateInterval)
		{
			this.meshUpdateCooldown = this.MeshUpdateInterval;
		}
		if (this.MeshUpdateInterval >= 0f)
		{
			this.meshUpdateCooldown -= Time.deltaTime;
			if (this.meshUpdateCooldown <= 0f)
			{
				this.meshUpdateCooldown = this.MeshUpdateInterval;
				if (this.IsBaked)
				{
					if (this.tree != null)
					{
						this.tree.Update(this.Mesh);
					}
				}
				else if (this.linear != null)
				{
					this.linear.Update(this.Mesh);
				}
			}
		}
		Vector3 vector = default(Vector3);
		if (VA_Helper.GetListenerPosition(ref vector))
		{
			this.UpdateFields();
			Vector3 vector2 = vector;
			Vector3 vector3 = base.transform.InverseTransformPoint(vector2);
			if (this.Mesh != null)
			{
				if (this.IsHollow)
				{
					vector3 = this.SnapLocalPoint(vector3);
					vector2 = base.transform.TransformPoint(vector3);
					base.SetOuterPoint(vector2);
					return;
				}
				if (this.PointInMesh(vector3, vector2))
				{
					base.SetInnerPoint(vector2, true);
					vector3 = this.SnapLocalPoint(vector3);
					vector2 = base.transform.TransformPoint(vector3);
					base.SetOuterPoint(vector2);
					return;
				}
				vector3 = this.SnapLocalPoint(vector3);
				vector2 = base.transform.TransformPoint(vector3);
				base.SetInnerOuterPoint(vector2, false);
			}
		}
	}

	// Token: 0x06000C49 RID: 3145 RVA: 0x00040A38 File Offset: 0x0003EC38
	private Vector3 FindClosestLocalPoint(Vector3 localPoint)
	{
		if (this.IsBaked)
		{
			return this.tree.FindClosestPoint(localPoint);
		}
		if (this.linear == null)
		{
			this.linear = new VA_MeshLinear();
		}
		if (!this.linear.HasTriangles)
		{
			this.linear.Update(this.Mesh);
		}
		return this.linear.FindClosestPoint(localPoint);
	}

	// Token: 0x06000C4A RID: 3146 RVA: 0x00040A98 File Offset: 0x0003EC98
	private void UpdateFields()
	{
		if (this.MeshCollider != null)
		{
			this.Mesh = this.MeshCollider.sharedMesh;
			return;
		}
		if (this.MeshFilter != null)
		{
			this.Mesh = this.MeshFilter.sharedMesh;
		}
	}

	// Token: 0x06000C4B RID: 3147 RVA: 0x00040AE4 File Offset: 0x0003ECE4
	private int RaycastHitCount(Vector3 origin, Vector3 direction, float separation)
	{
		int num = 0;
		if (this.MeshCollider != null && separation > 0f)
		{
			float num2 = Vector3.Magnitude(this.MeshCollider.bounds.size);
			float num3 = num2;
			float num4 = num2;
			Ray ray = new Ray(origin, direction);
			Ray ray2 = new Ray(origin + direction * num2, -direction);
			RaycastHit raycastHit = default(RaycastHit);
			int num5 = 0;
			while (num5 < 50 && this.MeshCollider.Raycast(ray, out raycastHit, num3))
			{
				num3 -= raycastHit.distance + separation;
				ray.origin = raycastHit.point + ray.direction * separation;
				num++;
				num5++;
			}
			int num6 = 0;
			while (num6 < 50 && this.MeshCollider.Raycast(ray2, out raycastHit, num4))
			{
				num4 -= raycastHit.distance + separation;
				ray2.origin = raycastHit.point + ray2.direction * separation;
				num++;
				num6++;
			}
		}
		return num;
	}

	// Token: 0x06000C4C RID: 3148 RVA: 0x00040C00 File Offset: 0x0003EE00
	private bool PointInMesh(Vector3 localPoint, Vector3 worldPoint)
	{
		if (!this.Mesh.bounds.Contains(localPoint))
		{
			return false;
		}
		int num = this.RaycastHitCount(worldPoint, Vector3.up, this.RaySeparation);
		return num != 0 && num % 2 != 0;
	}

	// Token: 0x06000C4D RID: 3149 RVA: 0x00040C43 File Offset: 0x0003EE43
	private Vector3 SnapLocalPoint(Vector3 localPoint)
	{
		return this.FindClosestLocalPoint(localPoint);
	}

	// Token: 0x04000EBC RID: 3772
	[Tooltip("If you set this, then all shape settings will automatically be copied from the collider")]
	public MeshCollider MeshCollider;

	// Token: 0x04000EBD RID: 3773
	[Tooltip("If you set this, then all shape settings will automatically be copied from the collider")]
	public MeshFilter MeshFilter;

	// Token: 0x04000EBE RID: 3774
	[Tooltip("The mesh of the mesh shape (if you set MeshCollider or MeshFilter, this will be automatically overwritten)")]
	public Mesh Mesh;

	// Token: 0x04000EBF RID: 3775
	[Tooltip("The interval between each mesh update in seconds (-1 = no updates)")]
	public float MeshUpdateInterval = -1f;

	// Token: 0x04000EC0 RID: 3776
	[Tooltip("How far apart each volume checking ray should be separated to avoid miscalculations. This value should be based on the size of your mesh, but be kept quite low")]
	public float RaySeparation = 0.1f;

	// Token: 0x04000EC1 RID: 3777
	[SerializeField]
	private VA_MeshTree tree;

	// Token: 0x04000EC2 RID: 3778
	[NonSerialized]
	private VA_MeshLinear linear;

	// Token: 0x04000EC3 RID: 3779
	[NonSerialized]
	private float meshUpdateCooldown;
}
