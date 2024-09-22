using System;
using UnityEngine;

// Token: 0x0200011E RID: 286
[Serializable]
public class MovementModifier
{
	// Token: 0x060006E7 RID: 1767 RVA: 0x00022ED7 File Offset: 0x000210D7
	public MovementModifier(Vector3 movementAddend, float movementMultiplier)
	{
		this.movementAddend = movementAddend;
		this.movementMultiplier = movementMultiplier;
	}

	// Token: 0x060006E8 RID: 1768 RVA: 0x00022EF8 File Offset: 0x000210F8
	public MovementModifier(Vector3 movementAddend, float movementMultiplier, int priority)
	{
		this.movementAddend = movementAddend;
		this.movementMultiplier = movementMultiplier;
		this.priority = priority;
	}

	// Token: 0x04000722 RID: 1826
	public Vector3 movementAddend;

	// Token: 0x04000723 RID: 1827
	public float movementMultiplier = 1f;

	// Token: 0x04000724 RID: 1828
	public int priority;

	// Token: 0x04000725 RID: 1829
	public bool forceTrigger;

	// Token: 0x04000726 RID: 1830
	public bool ignoreGrounded;

	// Token: 0x04000727 RID: 1831
	public bool ignoreAirborne;
}
