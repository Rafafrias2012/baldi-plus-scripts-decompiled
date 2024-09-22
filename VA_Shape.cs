using System;
using UnityEngine;

// Token: 0x02000226 RID: 550
public abstract class VA_Shape : MonoBehaviour
{
	// Token: 0x17000101 RID: 257
	// (get) Token: 0x06000C69 RID: 3177 RVA: 0x00041805 File Offset: 0x0003FA05
	public virtual bool FinalPointSet
	{
		get
		{
			return this.OuterPointSet;
		}
	}

	// Token: 0x17000102 RID: 258
	// (get) Token: 0x06000C6A RID: 3178 RVA: 0x0004180D File Offset: 0x0003FA0D
	public virtual Vector3 FinalPoint
	{
		get
		{
			return this.OuterPoint;
		}
	}

	// Token: 0x17000103 RID: 259
	// (get) Token: 0x06000C6B RID: 3179 RVA: 0x00041815 File Offset: 0x0003FA15
	public virtual float FinalPointDistance
	{
		get
		{
			return this.OuterPointDistance;
		}
	}

	// Token: 0x06000C6C RID: 3180 RVA: 0x00041820 File Offset: 0x0003FA20
	public void SetOuterPoint(Vector3 newOuterPoint)
	{
		Vector3 a = default(Vector3);
		if (VA_Helper.GetListenerPosition(ref a))
		{
			this.OuterPointSet = true;
			this.OuterPoint = newOuterPoint;
			this.OuterPointDistance = Vector3.Distance(a, newOuterPoint);
		}
	}

	// Token: 0x06000C6D RID: 3181 RVA: 0x00041859 File Offset: 0x0003FA59
	protected virtual void LateUpdate()
	{
		this.OuterPointSet = false;
	}

	// Token: 0x04000ECF RID: 3791
	public bool OuterPointSet;

	// Token: 0x04000ED0 RID: 3792
	public Vector3 OuterPoint;

	// Token: 0x04000ED1 RID: 3793
	public float OuterPointDistance;
}
