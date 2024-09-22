using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001C8 RID: 456
public class SodaMachine : EnvironmentObject, IItemAcceptor
{
	// Token: 0x06000A58 RID: 2648 RVA: 0x00037128 File Offset: 0x00035328
	public void InsertItem(PlayerManager pm, EnvironmentController ec)
	{
		base.StartCoroutine(this.Delay(pm));
		this.usesLeft--;
		if (this.usesLeft <= 0 && this.meshRenderer != null)
		{
			this._materials = this.meshRenderer.sharedMaterials;
			this._materials[1] = this.outOfStockMat;
			this.meshRenderer.sharedMaterials = this._materials;
		}
	}

	// Token: 0x06000A59 RID: 2649 RVA: 0x00037198 File Offset: 0x00035398
	public bool ItemFits(Items checkItem)
	{
		return this.requiredItem.itemType == checkItem && this.usesLeft > 0;
	}

	// Token: 0x06000A5A RID: 2650 RVA: 0x000371B3 File Offset: 0x000353B3
	private IEnumerator Delay(PlayerManager pm)
	{
		yield return null;
		if (this.potentialItems.Length != 0)
		{
			ItemManager itm = pm.itm;
			WeightedSelection<ItemObject>[] items = this.potentialItems;
			itm.AddItem(WeightedSelection<ItemObject>.RandomSelection(items));
		}
		else
		{
			pm.itm.AddItem(this.item);
		}
		yield break;
	}

	// Token: 0x04000BCA RID: 3018
	[SerializeField]
	private WeightedItemObject[] potentialItems;

	// Token: 0x04000BCB RID: 3019
	[SerializeField]
	private ItemObject item;

	// Token: 0x04000BCC RID: 3020
	[SerializeField]
	private ItemObject requiredItem;

	// Token: 0x04000BCD RID: 3021
	[SerializeField]
	private MeshRenderer meshRenderer;

	// Token: 0x04000BCE RID: 3022
	private Material[] _materials = new Material[0];

	// Token: 0x04000BCF RID: 3023
	[SerializeField]
	private Material outOfStockMat;

	// Token: 0x04000BD0 RID: 3024
	[SerializeField]
	private int usesLeft = 1;
}
