using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000B7 RID: 183
public class StoreScreen : MonoBehaviour
{
	// Token: 0x06000429 RID: 1065 RVA: 0x00015AFC File Offset: 0x00013CFC
	private void Start()
	{
		for (int i = 0; i < this.itemForSale.Length; i++)
		{
			if (i < this.defaultItems.Length)
			{
				this.forSaleImage[i].sprite = this.defaultItems[i].itemSpriteSmall;
				this.itemPrice[i].text = this.defaultItems[i].price.ToString();
				this.itemForSale[i] = this.defaultItems[i];
			}
			else if (Singleton<BaseGameManager>.Instance != null && i < Singleton<BaseGameManager>.Instance.levelObject.totalShopItems)
			{
				ItemObject[] array = this.itemForSale;
				int num = i;
				WeightedSelection<ItemObject>[] shopItems = Singleton<BaseGameManager>.Instance.levelObject.shopItems;
				array[num] = WeightedSelection<ItemObject>.RandomSelection(shopItems);
				this.forSaleImage[i].sprite = this.itemForSale[i].itemSpriteSmall;
				this.itemPrice[i].text = this.itemForSale[i].price.ToString();
			}
			else
			{
				this.forSaleImage[i].sprite = this.defaultItem.itemSpriteSmall;
				this.itemPrice[i].text = "";
				this.itemForSale[i] = this.defaultItem;
				this.forSaleHotSpots[i].SetActive(false);
			}
		}
		if (Singleton<CoreGameManager>.Instance == null || Singleton<CoreGameManager>.Instance.GetPlayer(0) == null)
		{
			for (int j = 0; j < this.inventory.Length; j++)
			{
				this.inventory[j] = this.defaultItem;
			}
		}
		else
		{
			for (int k = 0; k < this.inventory.Length; k++)
			{
				if (k < Singleton<CoreGameManager>.Instance.GetPlayer(0).itm.items.Length)
				{
					this.inventory[k] = Singleton<CoreGameManager>.Instance.GetPlayer(0).itm.items[k];
					this.inventoryImage[k].sprite = this.inventory[k].itemSpriteSmall;
				}
				else
				{
					this.inventory[k] = this.defaultItem;
				}
			}
		}
		if (Singleton<CoreGameManager>.Instance != null)
		{
			this.mapPrice = Singleton<BaseGameManager>.Instance.levelObject.mapPrice;
			this.mapPriceText.text = this.mapPrice.ToString();
			this.ytps = Singleton<CoreGameManager>.Instance.GetPoints(0);
			this.totalPoints.text = this.ytps.ToString();
		}
		else
		{
			this.mapPrice = 300;
			this.mapPriceText.text = this.mapPrice.ToString();
			this.ytps = 500;
			this.totalPoints.text = this.ytps.ToString();
		}
		this.audMan.QueueAudio(this.audJonIntro);
		this.audMan.QueueRandomAudio(this.audIntroP2);
		this.StandardDescription();
		this.audMan.audioDevice.ignoreListenerPause = true;
	}

	// Token: 0x0600042A RID: 1066 RVA: 0x00015DD0 File Offset: 0x00013FD0
	private void Update()
	{
		if (this.coughChanceDelay <= 0f)
		{
			if (Random.Range(0, 40000) == 0)
			{
				this.audMan.PlaySingle(this.audCough);
			}
			this.coughChanceDelay = 0.01666667f;
		}
		this.coughChanceDelay -= Time.unscaledDeltaTime;
		if (this.dragging)
		{
			this.draggedItemImage.transform.localPosition = CursorController.Instance.LocalPosition;
			this.InventoryDescription(this.slotDragging);
		}
	}

	// Token: 0x0600042B RID: 1067 RVA: 0x00015E54 File Offset: 0x00014054
	public void UpdateDescription(int val)
	{
		if (!this.dragging)
		{
			if (val <= 5)
			{
				this.itemDescription.text = Singleton<LocalizationManager>.Instance.GetLocalizedText(this.itemForSale[val].descKey);
				return;
			}
			if (val == 6)
			{
				this.itemDescription.text = Singleton<LocalizationManager>.Instance.GetLocalizedText("Desc_MapFill");
				if (!this.audMan.QueuedAudioIsPlaying)
				{
					this.audMan.QueueAudio(this.audMapInfo);
					return;
				}
			}
			else if (val == 7)
			{
				this.itemDescription.text = Singleton<LocalizationManager>.Instance.GetLocalizedText("Desc_Suspend");
			}
		}
	}

	// Token: 0x0600042C RID: 1068 RVA: 0x00015EEE File Offset: 0x000140EE
	public void InventoryDescription(int val)
	{
		if (!this.dragging)
		{
			this.itemDescription.text = Singleton<LocalizationManager>.Instance.GetLocalizedText(this.inventory[val].descKey);
		}
	}

	// Token: 0x0600042D RID: 1069 RVA: 0x00015F1A File Offset: 0x0001411A
	public void StandardDescription()
	{
		if (!this.dragging)
		{
			this.itemDescription.text = Singleton<LocalizationManager>.Instance.GetLocalizedText("Desc_Store");
		}
	}

	// Token: 0x0600042E RID: 1070 RVA: 0x00015F40 File Offset: 0x00014140
	public void BuyItem(int val)
	{
		if (val <= 5)
		{
			if (!this.itemPurchased[val] && this.ytps >= this.itemForSale[val].price)
			{
				this.itemPurchased[val] = true;
				this.ytps -= this.itemForSale[val].price;
				this.pointsSpent += this.itemForSale[val].price;
				this.totalPoints.text = this.ytps.ToString();
				this.forSaleImage[val].gameObject.SetActive(false);
				this.forSaleHotSpots[val].SetActive(false);
				this.itemPrice[val].text = "SOLD";
				this.itemPrice[val].color = Color.red;
				int i = 0;
				while (i < this.inventory.Length)
				{
					if (i != 5 && this.inventory[i].itemType == Items.None)
					{
						this.inventory[i] = this.itemForSale[val];
						this.inventoryImage[i].sprite = this.inventory[i].itemSpriteSmall;
						if (i > 5)
						{
							this.counterHotSpots[i - 6].SetActive(true);
							this.inventoryImage[i].gameObject.SetActive(true);
							break;
						}
						break;
					}
					else
					{
						i++;
					}
				}
				this.purchaseMade = true;
				if (!this.audMan.QueuedUp)
				{
					this.audMan.QueueRandomAudio(this.audBuy);
				}
				this.StandardDescription();
				return;
			}
			if (!this.audMan.QueuedUp)
			{
				this.audMan.QueueRandomAudio(this.audUnafforable);
				return;
			}
		}
		else if (val == 6)
		{
			if (!this.itemPurchased[val] && this.ytps >= this.mapPrice)
			{
				this.itemPurchased[val] = true;
				this.ytps -= this.mapPrice;
				this.pointsSpent += this.mapPrice;
				this.totalPoints.text = this.ytps.ToString();
				this.mapHotSpot.SetActive(false);
				this.mapPriceText.text = "SOLD";
				this.mapPriceText.color = Color.red;
				Singleton<BaseGameManager>.Instance.CompleteMapOnReady();
				this.purchaseMade = true;
				if (!this.audMan.QueuedUp)
				{
					this.audMan.QueueRandomAudio(this.audMapFilled);
				}
				this.StandardDescription();
				return;
			}
			if (!this.audMan.QueuedUp)
			{
				this.audMan.QueueRandomAudio(this.audUnafforable);
			}
		}
	}

	// Token: 0x0600042F RID: 1071 RVA: 0x000161C4 File Offset: 0x000143C4
	public void ClickInventory(int val)
	{
		if (!this.dragging)
		{
			if (this.inventory[val].itemType != Items.None)
			{
				this.slotDragging = val;
				this.draggedItemImage.sprite = this.inventory[val].itemSpriteSmall;
				this.draggedItemImage.gameObject.SetActive(true);
				this.dragging = true;
				this.inventoryImage[val].sprite = this.defaultItem.itemSpriteSmall;
				return;
			}
		}
		else
		{
			ItemObject itemObject = this.inventory[this.slotDragging];
			this.inventory[this.slotDragging] = this.inventory[val];
			this.inventoryImage[this.slotDragging].sprite = this.inventory[val].itemSpriteSmall;
			this.inventory[val] = itemObject;
			this.inventoryImage[val].sprite = itemObject.itemSpriteSmall;
			this.draggedItemImage.gameObject.SetActive(false);
			this.dragging = false;
		}
	}

	// Token: 0x06000430 RID: 1072 RVA: 0x000162B4 File Offset: 0x000144B4
	public void TryExit()
	{
		bool flag = false;
		for (int i = 6; i < this.inventory.Length; i++)
		{
			if (this.inventory[i].itemType != Items.None)
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			this.exitConfirm.SetActive(true);
			return;
		}
		this.Exit();
	}

	// Token: 0x06000431 RID: 1073 RVA: 0x00016300 File Offset: 0x00014500
	public void Exit()
	{
		if (this.purchaseMade)
		{
			this.audMan.QueueRandomAudio(this.audLeaveHappy);
		}
		else
		{
			this.audMan.QueueRandomAudio(this.audLeaveSad);
		}
		if (Singleton<CoreGameManager>.Instance.GetPlayer(0) != null)
		{
			for (int i = 0; i <= Singleton<CoreGameManager>.Instance.GetPlayer(0).itm.maxItem; i++)
			{
				Singleton<CoreGameManager>.Instance.GetPlayer(0).itm.items[i] = this.inventory[i];
			}
			Singleton<CoreGameManager>.Instance.GetPlayer(0).itm.UpdateItems();
			Singleton<CoreGameManager>.Instance.BackupPlayers();
			Singleton<CoreGameManager>.Instance.AddPoints(this.pointsSpent * -1, 0, false, false);
		}
		Object.Destroy(CursorController.Instance.gameObject);
		base.StartCoroutine(this.WaitForAudio());
	}

	// Token: 0x06000432 RID: 1074 RVA: 0x000163DC File Offset: 0x000145DC
	private void CloseStore()
	{
		if (this.elevatorScreen != null)
		{
			Singleton<GlobalCam>.Instance.Transition(UiTransition.Dither, 0.01666667f);
			this.elevatorScreen.ExitShop();
			this.elevatorScreen.Canvas.gameObject.SetActive(true);
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000433 RID: 1075 RVA: 0x00016434 File Offset: 0x00014634
	private IEnumerator WaitForAudio()
	{
		yield return null;
		while (this.audMan.QueuedAudioIsPlaying)
		{
			yield return null;
		}
		float delay = 0.5f;
		while (delay > 0f)
		{
			delay -= Time.unscaledDeltaTime;
			yield return null;
		}
		this.CloseStore();
		yield break;
	}

	// Token: 0x04000480 RID: 1152
	[SerializeField]
	private ItemObject defaultItem;

	// Token: 0x04000481 RID: 1153
	[SerializeField]
	private Image[] forSaleImage = new Image[6];

	// Token: 0x04000482 RID: 1154
	[SerializeField]
	private Image[] inventoryImage = new Image[12];

	// Token: 0x04000483 RID: 1155
	[SerializeField]
	private Image draggedItemImage;

	// Token: 0x04000484 RID: 1156
	[SerializeField]
	private ItemObject[] defaultItems = new ItemObject[0];

	// Token: 0x04000485 RID: 1157
	private ItemObject[] itemForSale = new ItemObject[6];

	// Token: 0x04000486 RID: 1158
	private ItemObject[] inventory = new ItemObject[12];

	// Token: 0x04000487 RID: 1159
	[SerializeField]
	private TMP_Text[] itemPrice = new TMP_Text[6];

	// Token: 0x04000488 RID: 1160
	[SerializeField]
	private TMP_Text mapPriceText;

	// Token: 0x04000489 RID: 1161
	[SerializeField]
	private TMP_Text banPriceText;

	// Token: 0x0400048A RID: 1162
	[SerializeField]
	private TMP_Text itemDescription;

	// Token: 0x0400048B RID: 1163
	[SerializeField]
	private TMP_Text totalPoints;

	// Token: 0x0400048C RID: 1164
	[SerializeField]
	private GameObject[] forSaleHotSpots = new GameObject[6];

	// Token: 0x0400048D RID: 1165
	[SerializeField]
	private GameObject[] counterHotSpots = new GameObject[6];

	// Token: 0x0400048E RID: 1166
	[SerializeField]
	private GameObject mapHotSpot;

	// Token: 0x0400048F RID: 1167
	[SerializeField]
	private GameObject banHotSpot;

	// Token: 0x04000490 RID: 1168
	[SerializeField]
	private GameObject exitConfirm;

	// Token: 0x04000491 RID: 1169
	[SerializeField]
	private ElevatorScreen elevatorScreen;

	// Token: 0x04000492 RID: 1170
	[SerializeField]
	private AudioManager audMan;

	// Token: 0x04000493 RID: 1171
	[SerializeField]
	private SoundObject audCough;

	// Token: 0x04000494 RID: 1172
	[SerializeField]
	private SoundObject audJonIntro;

	// Token: 0x04000495 RID: 1173
	[SerializeField]
	private SoundObject audExpelInfo;

	// Token: 0x04000496 RID: 1174
	[SerializeField]
	private SoundObject audMapInfo;

	// Token: 0x04000497 RID: 1175
	[SerializeField]
	private SoundObject[] audBuy = new SoundObject[0];

	// Token: 0x04000498 RID: 1176
	[SerializeField]
	private SoundObject[] audLeaveHappy = new SoundObject[0];

	// Token: 0x04000499 RID: 1177
	[SerializeField]
	private SoundObject[] audLeaveSad = new SoundObject[0];

	// Token: 0x0400049A RID: 1178
	[SerializeField]
	private SoundObject[] audMapFilled = new SoundObject[0];

	// Token: 0x0400049B RID: 1179
	[SerializeField]
	private SoundObject[] audExpel = new SoundObject[0];

	// Token: 0x0400049C RID: 1180
	[SerializeField]
	private SoundObject[] audUnafforable = new SoundObject[0];

	// Token: 0x0400049D RID: 1181
	[SerializeField]
	private SoundObject[] audIntroP2;

	// Token: 0x0400049E RID: 1182
	private float coughChanceDelay;

	// Token: 0x0400049F RID: 1183
	private int ytps;

	// Token: 0x040004A0 RID: 1184
	private int pointsSpent;

	// Token: 0x040004A1 RID: 1185
	private int slotDragging;

	// Token: 0x040004A2 RID: 1186
	private int mapPrice;

	// Token: 0x040004A3 RID: 1187
	private int banPrice;

	// Token: 0x040004A4 RID: 1188
	private bool[] itemPurchased = new bool[8];

	// Token: 0x040004A5 RID: 1189
	private bool dragging;

	// Token: 0x040004A6 RID: 1190
	private bool purchaseMade;
}
