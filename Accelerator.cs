using System;
using UnityEngine;

// Token: 0x020000DD RID: 221
public class Accelerator
{
	// Token: 0x06000504 RID: 1284 RVA: 0x00019EAE File Offset: 0x000180AE
	public Accelerator(float initialSpeed, float maxSpeed, float acceleration)
	{
		this.initialSpeed = initialSpeed;
		this.maxSpeed = maxSpeed;
		this.acceleration = acceleration;
		this.lastTime = 0f;
		this.currentSpeed = initialSpeed;
	}

	// Token: 0x06000505 RID: 1285 RVA: 0x00019EDD File Offset: 0x000180DD
	public Accelerator(float initialSpeed, float acceleration) : this(initialSpeed, initialSpeed, acceleration)
	{
	}

	// Token: 0x06000506 RID: 1286 RVA: 0x00019EE8 File Offset: 0x000180E8
	public float UpdateAndGetDistance(float timeDelta)
	{
		float distanceOverTime = this.GetDistanceOverTime(this.lastTime, this.lastTime + timeDelta);
		this.lastTime += timeDelta;
		return distanceOverTime;
	}

	// Token: 0x06000507 RID: 1287 RVA: 0x00019F0C File Offset: 0x0001810C
	public float GetDistanceOverTime(float startTime, float endTime)
	{
		return this.GetDistanceAtTime(endTime) - this.GetDistanceAtTime(startTime);
	}

	// Token: 0x06000508 RID: 1288 RVA: 0x00019F20 File Offset: 0x00018120
	public float GetDistanceAtTime(float time)
	{
		if (this.acceleration == 0f)
		{
			this.currentSpeed = this.initialSpeed;
			return this.initialSpeed * time;
		}
		this.currentSpeed = this.initialSpeed + this.acceleration * time;
		if (this.currentSpeed > this.maxSpeed)
		{
			this.currentSpeed = this.maxSpeed;
			return this.maxSpeed * time - Mathf.Pow(this.initialSpeed - this.maxSpeed, 2f) / (2f * this.acceleration);
		}
		if (this.currentSpeed > 0f)
		{
			return this.DistanceFunction(time);
		}
		this.currentSpeed = 0f;
		return this.DistanceFunction(-this.initialSpeed / this.acceleration);
	}

	// Token: 0x06000509 RID: 1289 RVA: 0x00019FE3 File Offset: 0x000181E3
	private float DistanceFunction(float time)
	{
		return this.initialSpeed * time + 0.5f * this.acceleration * Mathf.Pow(time, 2f);
	}

	// Token: 0x17000055 RID: 85
	// (get) Token: 0x0600050A RID: 1290 RVA: 0x0001A006 File Offset: 0x00018206
	public bool Dead
	{
		get
		{
			return this.currentSpeed <= 0f && this.acceleration <= 0f;
		}
	}

	// Token: 0x0400054C RID: 1356
	private float initialSpeed;

	// Token: 0x0400054D RID: 1357
	private float maxSpeed;

	// Token: 0x0400054E RID: 1358
	private float acceleration;

	// Token: 0x0400054F RID: 1359
	private float lastTime;

	// Token: 0x04000550 RID: 1360
	private float currentSpeed;
}
