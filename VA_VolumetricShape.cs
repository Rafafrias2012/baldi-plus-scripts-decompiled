using System;
using UnityEngine;

// Token: 0x02000229 RID: 553
public abstract class VA_VolumetricShape : VA_Shape
{
	// Token: 0x17000109 RID: 265
	// (get) Token: 0x06000C80 RID: 3200 RVA: 0x00041D10 File Offset: 0x0003FF10
	public override bool FinalPointSet
	{
		get
		{
			if (!this.IsHollow)
			{
				return this.InnerPointSet;
			}
			return this.OuterPointSet;
		}
	}

	// Token: 0x1700010A RID: 266
	// (get) Token: 0x06000C81 RID: 3201 RVA: 0x00041D27 File Offset: 0x0003FF27
	public override Vector3 FinalPoint
	{
		get
		{
			if (!this.IsHollow)
			{
				return this.InnerPoint;
			}
			return this.OuterPoint;
		}
	}

	// Token: 0x1700010B RID: 267
	// (get) Token: 0x06000C82 RID: 3202 RVA: 0x00041D3E File Offset: 0x0003FF3E
	public override float FinalPointDistance
	{
		get
		{
			if (!this.IsHollow)
			{
				return this.InnerPointDistance;
			}
			return this.OuterPointDistance;
		}
	}

	// Token: 0x06000C83 RID: 3203 RVA: 0x00041D58 File Offset: 0x0003FF58
	public bool PointInShape(Vector3 worldPoint)
	{
		Vector3 localPoint = base.transform.InverseTransformPoint(worldPoint);
		return this.LocalPointInShape(localPoint);
	}

	// Token: 0x06000C84 RID: 3204
	public abstract bool LocalPointInShape(Vector3 localPoint);

	// Token: 0x06000C85 RID: 3205 RVA: 0x00041D7C File Offset: 0x0003FF7C
	public void SetInnerPoint(Vector3 newInnerPoint, bool inside)
	{
		Vector3 a = default(Vector3);
		if (VA_Helper.GetListenerPosition(ref a))
		{
			this.InnerPointSet = true;
			this.InnerPoint = newInnerPoint;
			this.InnerPointDistance = Vector3.Distance(a, newInnerPoint);
			this.InnerPointInside = inside;
		}
	}

	// Token: 0x06000C86 RID: 3206 RVA: 0x00041DBC File Offset: 0x0003FFBC
	public void SetInnerOuterPoint(Vector3 newInnerOuterPoint, bool inside)
	{
		this.SetInnerPoint(newInnerOuterPoint, inside);
		base.SetOuterPoint(newInnerOuterPoint);
	}

	// Token: 0x06000C87 RID: 3207 RVA: 0x00041DCD File Offset: 0x0003FFCD
	protected override void LateUpdate()
	{
		base.LateUpdate();
		this.InnerPointSet = false;
	}

	// Token: 0x04000EDD RID: 3805
	[Tooltip("If you set this, then sound will only emit from the thin shell around the shape, else it will emit from inside too")]
	public bool IsHollow;

	// Token: 0x04000EDE RID: 3806
	public bool InnerPointSet;

	// Token: 0x04000EDF RID: 3807
	public Vector3 InnerPoint;

	// Token: 0x04000EE0 RID: 3808
	public float InnerPointDistance;

	// Token: 0x04000EE1 RID: 3809
	public bool InnerPointInside;
}
