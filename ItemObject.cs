using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020001EF RID: 495
[CreateAssetMenu(fileName = "Item", menuName = "Custom Assets/Item Data Object", order = 2)]
public class ItemObject : ScriptableObject
{
	// Token: 0x04000CF1 RID: 3313
	public Item item;

	// Token: 0x04000CF2 RID: 3314
	public Items itemType;

	// Token: 0x04000CF3 RID: 3315
	public Sprite itemSpriteSmall;

	// Token: 0x04000CF4 RID: 3316
	public Sprite itemSpriteLarge;

	// Token: 0x04000CF5 RID: 3317
	public SoundObject audPickupOverride;

	// Token: 0x04000CF6 RID: 3318
	public string nameKey;

	// Token: 0x04000CF7 RID: 3319
	public string descKey;

	// Token: 0x04000CF8 RID: 3320
	[FormerlySerializedAs("cost")]
	public int value = 25;

	// Token: 0x04000CF9 RID: 3321
	public int price = 50;

	// Token: 0x04000CFA RID: 3322
	public bool addToInventory = true;
}
