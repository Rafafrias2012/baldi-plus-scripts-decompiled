using System;
using UnityEngine;

// Token: 0x020001E9 RID: 489
public class ItemManager : MonoBehaviour
{
	// Token: 0x06000B0B RID: 2827 RVA: 0x0003A1E0 File Offset: 0x000383E0
	private void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		for (int i = 0; i < this.items.Length; i++)
		{
			if (this.items[i] == null)
			{
				this.items[i] = this.nothing;
			}
		}
		Singleton<CoreGameManager>.Instance.GetHud(this.pm.playerNumber).UpdateInventorySize(this.maxItem + 1);
		this.UpdateSelect();
	}

	// Token: 0x06000B0C RID: 2828 RVA: 0x0003A254 File Offset: 0x00038454
	private void Update()
	{
		if (Time.timeScale != 0f)
		{
			Singleton<InputManager>.Instance.GetAnalogInput(this.itemAnalogData, out this._absoluteVector, out this._deltaVector, 0.05f);
			if (Mathf.Sign(this.scrollVal) != Mathf.Sign(this._deltaVector.x))
			{
				this.scrollVal = 0f;
			}
			this.scrollVal += this._deltaVector.x;
			if (this.scrollVal > 0.25f || Singleton<InputManager>.Instance.GetDigitalInput("ItemRight", true))
			{
				this.IncreaseItemSelection();
				this.scrollVal = 0f;
			}
			else if (this.scrollVal < -0.25f || Singleton<InputManager>.Instance.GetDigitalInput("ItemLeft", true))
			{
				this.DecreaseItemSelection();
				this.scrollVal = 0f;
			}
			if (Singleton<InputManager>.Instance.GetDigitalInput("UseItem", true) && !Singleton<PlayerFileManager>.Instance.authenticMode && !this.pm.plm.Entity.InteractionDisabled)
			{
				this.UseItem();
			}
			if (Singleton<InputManager>.Instance.GetDigitalInput("Item1", true))
			{
				this.selectedItem = 0;
				this.UpdateSelect();
				return;
			}
			if (Singleton<InputManager>.Instance.GetDigitalInput("Item2", true))
			{
				this.selectedItem = 1;
				this.UpdateSelect();
				return;
			}
			if (Singleton<InputManager>.Instance.GetDigitalInput("Item3", true))
			{
				this.selectedItem = 2;
				this.UpdateSelect();
				return;
			}
			if (Singleton<InputManager>.Instance.GetDigitalInput("Item4", true))
			{
				this.selectedItem = 3;
				this.UpdateSelect();
				return;
			}
			if (Singleton<InputManager>.Instance.GetDigitalInput("Item5", true))
			{
				this.selectedItem = 4;
				this.UpdateSelect();
				return;
			}
			if (Singleton<InputManager>.Instance.GetDigitalInput("Item6", true) && this.maxItem >= 5)
			{
				this.selectedItem = 5;
				this.UpdateSelect();
				return;
			}
			if (Singleton<InputManager>.Instance.GetDigitalInput("Item7", true) && this.maxItem >= 6)
			{
				this.selectedItem = 6;
				this.UpdateSelect();
				return;
			}
			if (Singleton<InputManager>.Instance.GetDigitalInput("Item8", true) && this.maxItem >= 7)
			{
				this.selectedItem = 7;
				this.UpdateSelect();
				return;
			}
			if (Singleton<InputManager>.Instance.GetDigitalInput("Item9", true) && this.maxItem >= 8)
			{
				this.selectedItem = 8;
				this.UpdateSelect();
			}
		}
	}

	// Token: 0x06000B0D RID: 2829 RVA: 0x0003A4B1 File Offset: 0x000386B1
	private void IncreaseItemSelection()
	{
		this.selectedItem++;
		if (this.selectedItem > this.maxItem)
		{
			this.selectedItem = 0;
		}
		this.UpdateSelect();
	}

	// Token: 0x06000B0E RID: 2830 RVA: 0x0003A4DC File Offset: 0x000386DC
	private void DecreaseItemSelection()
	{
		this.selectedItem--;
		if (this.selectedItem < 0)
		{
			this.selectedItem = this.maxItem;
		}
		this.UpdateSelect();
	}

	// Token: 0x06000B0F RID: 2831 RVA: 0x0003A507 File Offset: 0x00038707
	public void UpdateSelect()
	{
		Singleton<CoreGameManager>.Instance.GetHud(this.pm.playerNumber).SetItemSelect(this.selectedItem, this.items[this.selectedItem].nameKey);
	}

	// Token: 0x06000B10 RID: 2832 RVA: 0x0003A53C File Offset: 0x0003873C
	public void UpdateItems()
	{
		for (int i = 0; i <= this.maxItem; i++)
		{
			Singleton<CoreGameManager>.Instance.GetHud(this.pm.playerNumber).UpdateItemIcon(i, this.items[i].itemSpriteSmall);
		}
	}

	// Token: 0x06000B11 RID: 2833 RVA: 0x0003A584 File Offset: 0x00038784
	public void AddItem(ItemObject item, Pickup pickup)
	{
		if (this.InventoryFull())
		{
			int num = 0;
			while (this.slotLocked[this.selectedItem] && num <= this.maxItem)
			{
				this.IncreaseItemSelection();
				num++;
			}
			if (!this.slotLocked[this.selectedItem])
			{
				pickup.AssignItem(this.items[this.selectedItem]);
				Singleton<CoreGameManager>.Instance.GetHud(this.pm.playerNumber).inventory.LoseItem(this.selectedItem, this.items[this.selectedItem]);
				this.AddItem(item);
				return;
			}
		}
		else
		{
			this.AddItem(item);
		}
	}

	// Token: 0x06000B12 RID: 2834 RVA: 0x0003A628 File Offset: 0x00038828
	public void AddItem(ItemObject item)
	{
		int num = 0;
		if (this.items[this.selectedItem].itemType == Items.None)
		{
			this.items[this.selectedItem] = item;
			num = this.selectedItem;
		}
		else
		{
			bool flag = false;
			for (int i = 0; i <= this.maxItem; i++)
			{
				if (this.items[i].itemType == Items.None)
				{
					this.items[i] = item;
					flag = true;
					num = i;
					break;
				}
			}
			if (!flag)
			{
				this.items[this.selectedItem] = item;
				num = this.selectedItem;
			}
		}
		Singleton<CoreGameManager>.Instance.GetHud(this.pm.playerNumber).UpdateItemIcon(num, this.items[num].itemSpriteSmall);
		Singleton<CoreGameManager>.Instance.GetHud(this.pm.playerNumber).inventory.CollectItem(num, item);
		this.UpdateSelect();
	}

	// Token: 0x06000B13 RID: 2835 RVA: 0x0003A6FA File Offset: 0x000388FA
	public void SetItem(ItemObject item, int slot)
	{
		this.items[slot] = item;
		Singleton<CoreGameManager>.Instance.GetHud(this.pm.playerNumber).UpdateItemIcon(slot, this.items[slot].itemSpriteSmall);
		this.UpdateSelect();
	}

	// Token: 0x06000B14 RID: 2836 RVA: 0x0003A733 File Offset: 0x00038933
	public void UseItem()
	{
		if (!this.disabled && Object.Instantiate<Item>(this.items[this.selectedItem].item).Use(this.pm))
		{
			this.RemoveItem(this.selectedItem);
		}
	}

	// Token: 0x06000B15 RID: 2837 RVA: 0x0003A770 File Offset: 0x00038970
	public void RemoveItem(int val)
	{
		Singleton<CoreGameManager>.Instance.GetHud(this.pm.playerNumber).inventory.LoseItem(val, this.items[val]);
		this.items[val] = this.nothing;
		Singleton<CoreGameManager>.Instance.GetHud(this.pm.playerNumber).UpdateItemIcon(val, this.nothing.itemSpriteSmall);
		this.UpdateSelect();
	}

	// Token: 0x06000B16 RID: 2838 RVA: 0x0003A7E0 File Offset: 0x000389E0
	public void ClearItems()
	{
		for (int i = 0; i < this.items.Length; i++)
		{
			this.items[i] = this.nothing;
			Singleton<CoreGameManager>.Instance.GetHud(this.pm.playerNumber).UpdateItemIcon(i, this.nothing.itemSpriteSmall);
		}
		this.UpdateSelect();
	}

	// Token: 0x06000B17 RID: 2839 RVA: 0x0003A83C File Offset: 0x00038A3C
	public void RemoveRandomItem()
	{
		if (this.HasItem())
		{
			int num = Random.Range(0, this.maxItem + 1);
			while (this.items[num] == this.nothing && !this.slotLocked[num])
			{
				num = Random.Range(0, this.maxItem + 1);
			}
			this.RemoveItem(num);
		}
	}

	// Token: 0x06000B18 RID: 2840 RVA: 0x0003A898 File Offset: 0x00038A98
	public bool HasItem()
	{
		for (int i = 0; i <= this.maxItem; i++)
		{
			if (this.items[i] != this.nothing && !this.slotLocked[i])
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000B19 RID: 2841 RVA: 0x0003A8D8 File Offset: 0x00038AD8
	public bool Has(Items desiredItem)
	{
		for (int i = 0; i <= this.maxItem; i++)
		{
			if (this.items[i].itemType == desiredItem)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000B1A RID: 2842 RVA: 0x0003A90C File Offset: 0x00038B0C
	public bool InventoryFull()
	{
		for (int i = 0; i <= this.maxItem; i++)
		{
			if (this.items[i].itemType == Items.None)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06000B1B RID: 2843 RVA: 0x0003A93C File Offset: 0x00038B3C
	public void Remove(Items itemToRemove)
	{
		for (int i = 0; i <= this.maxItem; i++)
		{
			if (this.items[i].itemType == itemToRemove)
			{
				Singleton<CoreGameManager>.Instance.GetHud(this.pm.playerNumber).inventory.LoseItem(i, this.items[i]);
				this.RemoveItem(i);
				return;
			}
		}
	}

	// Token: 0x06000B1C RID: 2844 RVA: 0x0003A99A File Offset: 0x00038B9A
	public void LockSlot(int slot, bool val)
	{
		if (slot >= 0 && slot <= this.maxItem)
		{
			this.slotLocked[slot] = val;
		}
	}

	// Token: 0x06000B1D RID: 2845 RVA: 0x0003A9B2 File Offset: 0x00038BB2
	public void Disable(bool val)
	{
		if (val)
		{
			this.disables++;
			this.disabled = true;
			return;
		}
		this.disables--;
		if (this.disables <= 0)
		{
			this.disabled = false;
			this.disables = 0;
		}
	}

	// Token: 0x04000C9D RID: 3229
	public PlayerManager pm;

	// Token: 0x04000C9E RID: 3230
	public ItemObject[] items = new ItemObject[9];

	// Token: 0x04000C9F RID: 3231
	public ItemObject nothing;

	// Token: 0x04000CA0 RID: 3232
	public AnalogInputData itemAnalogData;

	// Token: 0x04000CA1 RID: 3233
	private Vector2 _absoluteVector;

	// Token: 0x04000CA2 RID: 3234
	private Vector2 _deltaVector;

	// Token: 0x04000CA3 RID: 3235
	private float scrollVal;

	// Token: 0x04000CA4 RID: 3236
	public int selectedItem;

	// Token: 0x04000CA5 RID: 3237
	public int maxItem;

	// Token: 0x04000CA6 RID: 3238
	private int disables;

	// Token: 0x04000CA7 RID: 3239
	private bool[] slotLocked = new bool[9];

	// Token: 0x04000CA8 RID: 3240
	private bool disabled;
}
