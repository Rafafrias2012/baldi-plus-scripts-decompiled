using System;
using UnityEngine;

// Token: 0x020000E0 RID: 224
public class Force
{
	// Token: 0x06000546 RID: 1350 RVA: 0x0001B483 File Offset: 0x00019683
	public Force(Vector3 direction, float initialSpeed, float maxSpeed, float acceleration)
	{
		this.direction = new Vector2(direction.x, direction.z);
		this.accelerator = new Accelerator(initialSpeed, maxSpeed, acceleration);
	}

	// Token: 0x06000547 RID: 1351 RVA: 0x0001B4B1 File Offset: 0x000196B1
	public Force(Vector3 direction, float initialSpeed, float acceleration) : this(direction, initialSpeed, initialSpeed, acceleration)
	{
	}

	// Token: 0x06000548 RID: 1352 RVA: 0x0001B4BD File Offset: 0x000196BD
	public Vector3 VelocityThisFrame(float deltaTime)
	{
		return this.Vector3Direction * this.accelerator.UpdateAndGetDistance(deltaTime);
	}

	// Token: 0x17000063 RID: 99
	// (get) Token: 0x06000549 RID: 1353 RVA: 0x0001B4D6 File Offset: 0x000196D6
	public Vector3 Vector3Direction
	{
		get
		{
			return new Vector3(this.direction.x, 0f, this.direction.y);
		}
	}

	// Token: 0x17000064 RID: 100
	// (get) Token: 0x0600054A RID: 1354 RVA: 0x0001B4F8 File Offset: 0x000196F8
	public bool Dead
	{
		get
		{
			return this.accelerator.Dead;
		}
	}

	// Token: 0x0400058A RID: 1418
	private Vector2 direction;

	// Token: 0x0400058B RID: 1419
	private Accelerator accelerator;
}
