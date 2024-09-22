using System;
using UnityEngine;

// Token: 0x02000169 RID: 361
public struct CullingData
{
	// Token: 0x06000827 RID: 2087 RVA: 0x000293BC File Offset: 0x000275BC
	public void Initalize()
	{
		this.forwardOccluderAngle = 0f;
		this.mostSignificantForwardOccluderDistance = int.MaxValue;
		this.mostSignificantForwardOccluderOffset = 0;
		this.forwardDistance = 0;
		this.forwardReceivedFromPriority = false;
		this.sidewaysOccluderAngle = 0f;
		this.mostSignificantSidewaysOccluderDistance = int.MaxValue;
		this.mostSignificantSidewaysOccluderOffset = 0;
		this.sidewaysDistance = 0;
		this.sidewaysReceivedFromPriority = false;
	}

	// Token: 0x06000828 RID: 2088 RVA: 0x00029420 File Offset: 0x00027620
	public bool NeedsUpdated(CullingData data, CullingPropagationDirection cullingDirection)
	{
		bool result = false;
		if (!this.forwardReceivedFromPriority)
		{
			if (cullingDirection == CullingPropagationDirection.Sideways)
			{
				this.ForceUpdateForwardOccluder(data.forwardOccluderAngle, data.mostSignificantForwardOccluderDistance, data.mostSignificantForwardOccluderOffset);
				this.forwardReceivedFromPriority = true;
				result = true;
			}
			else if (this.UpdateForwardOccluder(data.forwardOccluderAngle, data.mostSignificantForwardOccluderDistance, data.mostSignificantForwardOccluderOffset))
			{
				result = true;
			}
		}
		if (!this.sidewaysReceivedFromPriority)
		{
			if (cullingDirection == CullingPropagationDirection.Forward)
			{
				this.ForceUpdateSidewaysOccluder(data.sidewaysOccluderAngle, data.mostSignificantSidewaysOccluderDistance, data.mostSignificantSidewaysOccluderOffset);
				this.sidewaysReceivedFromPriority = true;
				result = true;
			}
			else if (this.UpdateSidewaysOccluder(data.sidewaysOccluderAngle, data.mostSignificantSidewaysOccluderDistance, data.mostSignificantSidewaysOccluderOffset))
			{
				result = true;
			}
		}
		return result;
	}

	// Token: 0x06000829 RID: 2089 RVA: 0x000294C5 File Offset: 0x000276C5
	public bool UpdateForwardOccluder(float angle, int distance, int offset)
	{
		if (angle > this.forwardOccluderAngle)
		{
			this.forwardOccluderAngle = angle;
			this.mostSignificantForwardOccluderDistance = distance;
			this.mostSignificantForwardOccluderOffset = offset;
			return true;
		}
		return false;
	}

	// Token: 0x0600082A RID: 2090 RVA: 0x000294E8 File Offset: 0x000276E8
	public void ForceUpdateForwardOccluder(float angle, int distance, int offset)
	{
		this.forwardOccluderAngle = angle;
		this.mostSignificantForwardOccluderDistance = distance;
		this.mostSignificantForwardOccluderOffset = offset;
	}

	// Token: 0x0600082B RID: 2091 RVA: 0x00029500 File Offset: 0x00027700
	public bool UpdateForwardOccluder(int distance, int offset)
	{
		float num = Mathf.Atan2((float)offset, (float)distance);
		if (num > this.forwardOccluderAngle)
		{
			this.mostSignificantForwardOccluderDistance = distance;
			this.mostSignificantForwardOccluderOffset = offset;
			this.forwardOccluderAngle = num;
			return true;
		}
		return false;
	}

	// Token: 0x0600082C RID: 2092 RVA: 0x00029538 File Offset: 0x00027738
	public bool UpdateSidewaysOccluder(float angle, int distance, int offset)
	{
		if (angle > this.sidewaysOccluderAngle)
		{
			this.sidewaysOccluderAngle = angle;
			this.mostSignificantSidewaysOccluderDistance = distance;
			this.mostSignificantSidewaysOccluderOffset = offset;
			return true;
		}
		return false;
	}

	// Token: 0x0600082D RID: 2093 RVA: 0x0002955B File Offset: 0x0002775B
	public void ForceUpdateSidewaysOccluder(float angle, int distance, int offset)
	{
		this.sidewaysOccluderAngle = angle;
		this.mostSignificantSidewaysOccluderDistance = distance;
		this.mostSignificantSidewaysOccluderOffset = offset;
	}

	// Token: 0x0600082E RID: 2094 RVA: 0x00029574 File Offset: 0x00027774
	public bool UpdateSidewaysOccluder(int distance, int offset)
	{
		float num = Mathf.Atan2((float)offset, (float)distance);
		if (num > this.sidewaysOccluderAngle)
		{
			this.mostSignificantSidewaysOccluderDistance = distance;
			this.mostSignificantSidewaysOccluderOffset = offset;
			this.sidewaysOccluderAngle = num;
			return true;
		}
		return false;
	}

	// Token: 0x040008BD RID: 2237
	public float forwardOccluderAngle;

	// Token: 0x040008BE RID: 2238
	public float sidewaysOccluderAngle;

	// Token: 0x040008BF RID: 2239
	public int forwardDistance;

	// Token: 0x040008C0 RID: 2240
	public int sidewaysDistance;

	// Token: 0x040008C1 RID: 2241
	public int mostSignificantForwardOccluderDistance;

	// Token: 0x040008C2 RID: 2242
	public int mostSignificantForwardOccluderOffset;

	// Token: 0x040008C3 RID: 2243
	public int mostSignificantSidewaysOccluderDistance;

	// Token: 0x040008C4 RID: 2244
	public int mostSignificantSidewaysOccluderOffset;

	// Token: 0x040008C5 RID: 2245
	public int disabledDirections;

	// Token: 0x040008C6 RID: 2246
	private bool forwardReceivedFromPriority;

	// Token: 0x040008C7 RID: 2247
	private bool sidewaysReceivedFromPriority;
}
