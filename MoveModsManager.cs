using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200011D RID: 285
public class MoveModsManager
{
	// Token: 0x060006E0 RID: 1760 RVA: 0x00022D13 File Offset: 0x00020F13
	public Transform trans(int val)
	{
		return this.transforms[val];
	}

	// Token: 0x060006E1 RID: 1761 RVA: 0x00022D21 File Offset: 0x00020F21
	public MovementModifier moveMod(int val)
	{
		return this.moveMods[val];
	}

	// Token: 0x17000088 RID: 136
	// (get) Token: 0x060006E2 RID: 1762 RVA: 0x00022D30 File Offset: 0x00020F30
	public int Count
	{
		get
		{
			for (int i = 0; i < this.moveMods.Count; i++)
			{
				if (this.transforms[i] == null)
				{
					this.transforms.RemoveAt(i);
					this.moveMods.RemoveAt(i);
					i--;
				}
			}
			return this.moveMods.Count;
		}
	}

	// Token: 0x060006E3 RID: 1763 RVA: 0x00022D90 File Offset: 0x00020F90
	public void AddMoveMod(Transform transform)
	{
		this.transforms.Add(transform);
		this.moveMods.Add(new MovementModifier(Vector3.zero, 1f));
		transform.GetComponent<ActivityModifier>().moveMods.Add(this.moveMods[this.moveMods.Count - 1]);
	}

	// Token: 0x060006E4 RID: 1764 RVA: 0x00022DEC File Offset: 0x00020FEC
	public void Remove(Transform transform)
	{
		for (int i = 0; i < this.transforms.Count; i++)
		{
			if (this.transforms[i] == transform)
			{
				transform.GetComponent<ActivityModifier>().moveMods.Remove(this.moveMods[i]);
				this.transforms.RemoveAt(i);
				this.moveMods.RemoveAt(i);
				i--;
			}
		}
	}

	// Token: 0x060006E5 RID: 1765 RVA: 0x00022E5C File Offset: 0x0002105C
	public void RemoveAll()
	{
		while (this.moveMods.Count > 0)
		{
			this.transforms[0].GetComponent<ActivityModifier>().moveMods.Remove(this.moveMods[0]);
			this.moveMods.RemoveAt(0);
			this.transforms.RemoveAt(0);
		}
	}

	// Token: 0x04000720 RID: 1824
	private List<MovementModifier> moveMods = new List<MovementModifier>();

	// Token: 0x04000721 RID: 1825
	private List<Transform> transforms = new List<Transform>();
}
