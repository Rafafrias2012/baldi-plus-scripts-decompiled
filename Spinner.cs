using System;
using UnityEngine;

// Token: 0x02000138 RID: 312
public class Spinner : EnvironmentObject
{
	// Token: 0x1700008C RID: 140
	// (get) Token: 0x06000753 RID: 1875 RVA: 0x00025C28 File Offset: 0x00023E28
	// (set) Token: 0x06000754 RID: 1876 RVA: 0x00025C30 File Offset: 0x00023E30
	public float CurrentSpeed { get; private set; }

	// Token: 0x06000755 RID: 1877 RVA: 0x00025C39 File Offset: 0x00023E39
	private void Start()
	{
		this.CurrentSpeed = this.rotationSpeed;
		this.target.Rotate(this.axis, Random.Range(0f, this.randomStartMax));
	}

	// Token: 0x06000756 RID: 1878 RVA: 0x00025C68 File Offset: 0x00023E68
	private void Update()
	{
		if (this.fluctuate)
		{
			this.CurrentSpeed = Mathf.Lerp(this.rotationSpeed, this.minRotationSpeed, Mathf.Sin(this.ec.SurpassedGameTime / this.fluctuationScale) / 2f + 0.5f);
		}
		this.target.Rotate(this.axis, this.CurrentSpeed * Time.deltaTime * this.ec.EnvironmentTimeScale);
	}

	// Token: 0x04000806 RID: 2054
	[SerializeField]
	private Transform target;

	// Token: 0x04000807 RID: 2055
	[SerializeField]
	private Vector3 axis = Vector3.up;

	// Token: 0x04000808 RID: 2056
	[SerializeField]
	private float rotationSpeed = 90f;

	// Token: 0x04000809 RID: 2057
	[SerializeField]
	private float minRotationSpeed = -90f;

	// Token: 0x0400080A RID: 2058
	[SerializeField]
	private float fluctuationScale = 1f;

	// Token: 0x0400080B RID: 2059
	[SerializeField]
	private float randomStartMax;

	// Token: 0x0400080D RID: 2061
	[SerializeField]
	private bool fluctuate;
}
