using System;
using UnityEngine;

// Token: 0x02000125 RID: 293
public class Pickup : MonoBehaviour, IClickable<int>
{
	// Token: 0x14000007 RID: 7
	// (add) Token: 0x0600070E RID: 1806 RVA: 0x000238E0 File Offset: 0x00021AE0
	// (remove) Token: 0x0600070F RID: 1807 RVA: 0x00023918 File Offset: 0x00021B18
	public event Pickup.PickupInteractionDelegate OnItemCollected;

	// Token: 0x14000008 RID: 8
	// (add) Token: 0x06000710 RID: 1808 RVA: 0x00023950 File Offset: 0x00021B50
	// (remove) Token: 0x06000711 RID: 1809 RVA: 0x00023988 File Offset: 0x00021B88
	public event Pickup.PickupInteractionDelegate OnItemPurchased;

	// Token: 0x14000009 RID: 9
	// (add) Token: 0x06000712 RID: 1810 RVA: 0x000239C0 File Offset: 0x00021BC0
	// (remove) Token: 0x06000713 RID: 1811 RVA: 0x000239F8 File Offset: 0x00021BF8
	public event Pickup.PickupInteractionDelegate OnItemDenied;

	// Token: 0x06000714 RID: 1812 RVA: 0x00023A30 File Offset: 0x00021C30
	public void Start()
	{
		this.itemSprite.sprite = this.item.itemSpriteLarge;
		base.transform.name = "Item_" + this.item.itemType.ToString();
	}

	// Token: 0x06000715 RID: 1813 RVA: 0x00023A80 File Offset: 0x00021C80
	public void Clicked(int player)
	{
		if (Singleton<CoreGameManager>.Instance.GetPoints(player) >= this.price || this.free)
		{
			if (this.price != 0 && !this.free)
			{
				Singleton<CoreGameManager>.Instance.AddPoints(-this.price, player, true);
				Pickup.PickupInteractionDelegate onItemPurchased = this.OnItemPurchased;
				if (onItemPurchased != null)
				{
					onItemPurchased(this, player);
				}
			}
			this.Collect(player);
			return;
		}
		Pickup.PickupInteractionDelegate onItemDenied = this.OnItemDenied;
		if (onItemDenied == null)
		{
			return;
		}
		onItemDenied(this, player);
	}

	// Token: 0x06000716 RID: 1814 RVA: 0x00023AF8 File Offset: 0x00021CF8
	public void Collect(int player)
	{
		if (this.item.itemType != Items.None)
		{
			if (this.item.audPickupOverride == null)
			{
				Singleton<CoreGameManager>.Instance.audMan.PlaySingle(this.sound);
			}
			else
			{
				Singleton<CoreGameManager>.Instance.audMan.PlaySingle(this.item.audPickupOverride);
			}
			this.stillHasItem = false;
			if (this.item.addToInventory)
			{
				Singleton<CoreGameManager>.Instance.GetPlayer(player).itm.AddItem(this.item, this);
			}
			else
			{
				Object.Instantiate<Item>(this.item.item).Use(Singleton<CoreGameManager>.Instance.GetPlayer(player));
			}
			if (!this.stillHasItem && !this.survivePickup)
			{
				base.gameObject.SetActive(false);
				if (this.icon != null)
				{
					this.icon.spriteRenderer.enabled = false;
				}
			}
			if (!this.stillHasItem)
			{
				this.AssignItem(Singleton<CoreGameManager>.Instance.NoneItem);
			}
			this.stillHasItem = false;
			if (Singleton<CoreGameManager>.Instance.currentMode == Mode.Main)
			{
				Singleton<PlayerFileManager>.Instance.Find(Singleton<PlayerFileManager>.Instance.foundItems, (int)this.item.itemType);
			}
		}
		else
		{
			this.AssignItem(Singleton<CoreGameManager>.Instance.GetPlayer(player).itm.items[Singleton<CoreGameManager>.Instance.GetPlayer(player).itm.selectedItem]);
			Singleton<CoreGameManager>.Instance.GetPlayer(player).itm.RemoveItem(Singleton<CoreGameManager>.Instance.GetPlayer(player).itm.selectedItem);
		}
		Singleton<CoreGameManager>.Instance.GetHud(player).CloseTooltip();
		Pickup.PickupInteractionDelegate onItemCollected = this.OnItemCollected;
		if (onItemCollected == null)
		{
			return;
		}
		onItemCollected(this, player);
	}

	// Token: 0x06000717 RID: 1815 RVA: 0x00023CB0 File Offset: 0x00021EB0
	public void ClickableSighted(int player)
	{
		if (this.showDescription)
		{
			Singleton<CoreGameManager>.Instance.GetHud(player).SetTooltip(this.item.descKey);
		}
	}

	// Token: 0x06000718 RID: 1816 RVA: 0x00023CD5 File Offset: 0x00021ED5
	public void ClickableUnsighted(int player)
	{
		if (this.showDescription)
		{
			Singleton<CoreGameManager>.Instance.GetHud(player).CloseTooltip();
		}
	}

	// Token: 0x06000719 RID: 1817 RVA: 0x00023CEF File Offset: 0x00021EEF
	public bool ClickableHidden()
	{
		return false;
	}

	// Token: 0x0600071A RID: 1818 RVA: 0x00023CF2 File Offset: 0x00021EF2
	public bool ClickableRequiresNormalHeight()
	{
		return true;
	}

	// Token: 0x0600071B RID: 1819 RVA: 0x00023CF8 File Offset: 0x00021EF8
	public void AssignItem(ItemObject item)
	{
		this.stillHasItem = true;
		this.item = item;
		this.itemSprite.sprite = item.itemSpriteLarge;
		base.transform.name = "Item_" + item.itemType.ToString();
	}

	// Token: 0x04000751 RID: 1873
	public ItemObject item;

	// Token: 0x04000752 RID: 1874
	public SpriteRenderer itemSprite;

	// Token: 0x04000753 RID: 1875
	public MapIcon iconPre;

	// Token: 0x04000754 RID: 1876
	public MapIcon icon;

	// Token: 0x04000755 RID: 1877
	public SoundObject sound;

	// Token: 0x04000756 RID: 1878
	public int price;

	// Token: 0x04000757 RID: 1879
	[SerializeField]
	private bool survivePickup;

	// Token: 0x04000758 RID: 1880
	public bool free = true;

	// Token: 0x04000759 RID: 1881
	public bool showDescription;

	// Token: 0x0400075A RID: 1882
	private bool stillHasItem;

	// Token: 0x0200036F RID: 879
	// (Invoke) Token: 0x06001C15 RID: 7189
	public delegate void PickupInteractionDelegate(Pickup pickup, int player);
}
