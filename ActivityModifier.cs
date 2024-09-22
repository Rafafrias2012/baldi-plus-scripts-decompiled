using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001E8 RID: 488
public class ActivityModifier : MonoBehaviour
{
	// Token: 0x06000B06 RID: 2822 RVA: 0x0003A04C File Offset: 0x0003824C
	public void Initialize(Entity entity)
	{
		this.entity = entity;
	}

	// Token: 0x170000E4 RID: 228
	// (get) Token: 0x06000B07 RID: 2823 RVA: 0x0003A058 File Offset: 0x00038258
	public float Multiplier
	{
		get
		{
			if (this.ignoreMultiplier)
			{
				return 1f;
			}
			float num = 1f;
			foreach (MovementModifier movementModifier in this.moveMods)
			{
				if ((!movementModifier.ignoreGrounded && this.entity.Grounded) || (!movementModifier.ignoreAirborne && !this.entity.Grounded))
				{
					num *= movementModifier.movementMultiplier;
				}
			}
			return num;
		}
	}

	// Token: 0x170000E5 RID: 229
	// (get) Token: 0x06000B08 RID: 2824 RVA: 0x0003A0EC File Offset: 0x000382EC
	public Vector3 Addend
	{
		get
		{
			int num = int.MinValue;
			Vector3 vector = Vector3.zero;
			foreach (MovementModifier movementModifier in this.moveMods)
			{
				if ((!movementModifier.ignoreGrounded && this.entity.Grounded) || (!movementModifier.ignoreAirborne && !this.entity.Grounded))
				{
					if (movementModifier.priority > num)
					{
						vector = Vector3.zero;
						num = movementModifier.priority;
					}
					vector += movementModifier.movementAddend;
				}
			}
			return vector;
		}
	}

	// Token: 0x170000E6 RID: 230
	// (get) Token: 0x06000B09 RID: 2825 RVA: 0x0003A194 File Offset: 0x00038394
	public bool ForceTrigger
	{
		get
		{
			for (int i = 0; i < this.moveMods.Count; i++)
			{
				if (this.moveMods[i].forceTrigger)
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x04000C9A RID: 3226
	public List<MovementModifier> moveMods = new List<MovementModifier>();

	// Token: 0x04000C9B RID: 3227
	private Entity entity;

	// Token: 0x04000C9C RID: 3228
	[SerializeField]
	private bool ignoreMultiplier;
}
