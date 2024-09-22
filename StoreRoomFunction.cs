using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001B2 RID: 434
public class StoreRoomFunction : RoomFunction
{
	// Token: 0x060009C7 RID: 2503 RVA: 0x00034258 File Offset: 0x00032458
	public override void Initialize(RoomController room)
	{
		base.Initialize(room);
		this.roomBase.SetParent(room.objectObject.transform);
		this.storeData = Singleton<CoreGameManager>.Instance.nextLevel;
		if (!Singleton<CoreGameManager>.Instance.sceneObject.storeUsesNextLevelData)
		{
			this.storeData = Singleton<CoreGameManager>.Instance.sceneObject;
			this.inGameMode = true;
		}
		else
		{
			this.storeData = Singleton<CoreGameManager>.Instance.nextLevel;
		}
		if (this.storeData != null)
		{
			int num = 0;
			while (num < this.storeData.totalShopItems && num < room.itemSpawnPoints.Count)
			{
				ItemSpawnPoint itemSpawnPoint = room.itemSpawnPoints[num];
				Pickup pickup = room.ec.CreateItem(room, Singleton<CoreGameManager>.Instance.NoneItem, itemSpawnPoint.position);
				pickup.OnItemPurchased += this.ItemPurchased;
				pickup.OnItemDenied += this.ItemDenied;
				pickup.OnItemCollected += this.ItemCollected;
				pickup.showDescription = true;
				this.pickups.Add(pickup);
				num++;
			}
			this.Restock();
			if (Singleton<CoreGameManager>.Instance.levelMapHasBeenPurchasedFor != Singleton<CoreGameManager>.Instance.nextLevel && !this.inGameMode)
			{
				this.mapPickup.price = this.storeData.mapPrice;
				this.mapPickup.OnItemPurchased += this.ItemPurchased;
				this.mapPickup.OnItemDenied += this.ItemDenied;
				this.mapPickup.OnItemCollected += this.ItemCollected;
				this.mapTag.SetText(this.storeData.mapPrice.ToString());
			}
			else
			{
				this.mapPickup.gameObject.SetActive(false);
				this.mapTag.SetText(Singleton<LocalizationManager>.Instance.GetLocalizedText("TAG_Sold"));
			}
		}
		room.itemSpawnPoints.Clear();
	}

	// Token: 0x060009C8 RID: 2504 RVA: 0x00034454 File Offset: 0x00032654
	private void Restock()
	{
		if (this.storeData != null)
		{
			for (int i = 0; i < this.pickups.Count; i++)
			{
				WeightedSelection<ItemObject>[] shopItems = this.storeData.shopItems;
				ItemObject itemObject = WeightedSelection<ItemObject>.RandomSelection(shopItems);
				Pickup pickup = this.pickups[i];
				pickup.AssignItem(itemObject);
				int num = itemObject.price;
				if (Random.value < this.saleChance)
				{
					float num2 = Random.Range(this.minSaleDiscount, this.maxSaleDiscount);
					float num3 = (float)num * num2;
					num = Mathf.RoundToInt(num3 - num3 % 10f);
					this.tag[i].SetSale(itemObject.price, num);
				}
				else
				{
					this.tag[i].SetText(itemObject.price.ToString());
				}
				pickup.price = num;
				pickup.showDescription = true;
				pickup.gameObject.SetActive(true);
			}
		}
	}

	// Token: 0x060009C9 RID: 2505 RVA: 0x00034535 File Offset: 0x00032735
	public override void OnGenerationFinished()
	{
		base.OnGenerationFinished();
		if (this.inGameMode)
		{
			this.Close();
			return;
		}
		this.Open();
	}

	// Token: 0x060009CA RID: 2506 RVA: 0x00034552 File Offset: 0x00032752
	private void Update()
	{
		if (Singleton<BaseGameManager>.Instance.FoundNotebooks - this.notebooksAtLastReset >= this.notebooksPerReset)
		{
			this.notebooksAtLastReset = Singleton<BaseGameManager>.Instance.FoundNotebooks;
			if (!this.open)
			{
				this.Open();
				return;
			}
			this.Restock();
		}
	}

	// Token: 0x060009CB RID: 2507 RVA: 0x00034594 File Offset: 0x00032794
	public override void OnPlayerEnter(PlayerManager player)
	{
		base.OnPlayerEnter(player);
		this.totalCustomers++;
		Singleton<CoreGameManager>.Instance.GetHud(player.playerNumber).PointsAnimator.ShowDisplay(true);
		if (this.open)
		{
			if (!this.playerEntered)
			{
				this.johnnyAudioManager.QueueAudio(this.audJonIntro);
				this.johnnyAudioManager.QueueRandomAudio(this.audIntroP2);
				this.playerEntered = true;
				return;
			}
			if (!this.johnnyAudioManager.QueuedAudioIsPlaying)
			{
				this.johnnyAnimator.Play("Johnny_Test", -1, 0f);
				return;
			}
		}
		else
		{
			foreach (Door door in this.room.doors)
			{
				door.Unlock();
			}
		}
	}

	// Token: 0x060009CC RID: 2508 RVA: 0x00034678 File Offset: 0x00032878
	public override void OnNpcEnter(NPC npc)
	{
		base.OnNpcEnter(npc);
		this.totalCustomers++;
		if (!this.open)
		{
			foreach (Door door in this.room.doors)
			{
				door.Unlock();
			}
		}
	}

	// Token: 0x060009CD RID: 2509 RVA: 0x000346EC File Offset: 0x000328EC
	public override void OnPlayerStay(PlayerManager player)
	{
		base.OnPlayerStay(player);
		if (!this.open)
		{
			player.RuleBreak("Faculty", 1f, 0.25f);
		}
	}

	// Token: 0x060009CE RID: 2510 RVA: 0x00034714 File Offset: 0x00032914
	public override void OnPlayerExit(PlayerManager player)
	{
		base.OnPlayerExit(player);
		this.totalCustomers--;
		Singleton<CoreGameManager>.Instance.GetHud(player.playerNumber).PointsAnimator.ShowDisplay(false);
		if (this.open)
		{
			if (!this.playerLeft)
			{
				this.playerLeft = true;
				if (this.itemPurchased)
				{
					this.johnnyAudioManager.QueueRandomAudio(this.audLeaveHappy);
					return;
				}
				this.johnnyAudioManager.QueueRandomAudio(this.audLeaveSad);
				return;
			}
		}
		else
		{
			player.RuleBreak("Faculty", 1f);
			if (this.totalCustomers <= 0)
			{
				this.totalCustomers = 0;
				foreach (Door door in this.room.doors)
				{
					door.Lock(true);
				}
			}
		}
	}

	// Token: 0x060009CF RID: 2511 RVA: 0x00034800 File Offset: 0x00032A00
	public override void OnNpcExit(NPC npc)
	{
		base.OnNpcExit(npc);
		this.totalCustomers--;
		if (!this.open && this.totalCustomers <= 0)
		{
			this.totalCustomers = 0;
			foreach (Door door in this.room.doors)
			{
				door.Lock(true);
			}
		}
	}

	// Token: 0x060009D0 RID: 2512 RVA: 0x00034884 File Offset: 0x00032A84
	private void ItemPurchased(Pickup pickup, int player)
	{
		if (this.open)
		{
			if (!this.johnnyAudioManager.QueuedUp && pickup.item.itemType != Items.Map)
			{
				this.johnnyAudioManager.QueueRandomAudio(this.audBuy);
			}
			this.itemPurchased = true;
			this.playerLeft = false;
		}
	}

	// Token: 0x060009D1 RID: 2513 RVA: 0x000348D4 File Offset: 0x00032AD4
	private void ItemCollected(Pickup pickup, int player)
	{
		this.MarkItemAsSold(pickup);
		pickup.price = 0;
		pickup.showDescription = false;
		if (pickup == this.mapPickup)
		{
			this.BuyMap();
		}
		if (!this.open)
		{
			this.thief = true;
			Singleton<CoreGameManager>.Instance.johnnyHelped = true;
			Singleton<BaseGameManager>.Instance.AngerBaldi(this.baldiStealAnger);
			if (!this.alarmStarted)
			{
				this.SetOffAlarm();
			}
		}
	}

	// Token: 0x060009D2 RID: 2514 RVA: 0x00034944 File Offset: 0x00032B44
	private void ItemDenied(Pickup pickup, int player)
	{
		if (this.open)
		{
			if (!Singleton<CoreGameManager>.Instance.johnnyHelped && pickup.price - Singleton<CoreGameManager>.Instance.GetPoints(player) <= this.johnnyAidLimit)
			{
				pickup.Collect(player);
				this.johnnyAudioManager.FlushQueue(true);
				this.johnnyAudioManager.QueueAudio(this.audHelp);
				Singleton<CoreGameManager>.Instance.johnnyHelped = true;
				Singleton<CoreGameManager>.Instance.AddPoints(-Singleton<CoreGameManager>.Instance.GetPoints(player), player, true);
				this.itemPurchased = true;
				this.playerLeft = false;
				return;
			}
			if (!this.johnnyAudioManager.QueuedUp)
			{
				this.johnnyAudioManager.QueueRandomAudio(this.audUnafforable);
			}
		}
	}

	// Token: 0x060009D3 RID: 2515 RVA: 0x000349F8 File Offset: 0x00032BF8
	private void MarkItemAsSold(Pickup pickup)
	{
		if (this.open)
		{
			if (pickup.price > 0)
			{
				Singleton<CoreGameManager>.Instance.audMan.PlaySingle(this.audBell);
			}
			for (int i = 0; i < this.pickups.Count; i++)
			{
				if (this.pickups[i] == pickup)
				{
					this.tag[i].SetText(Singleton<LocalizationManager>.Instance.GetLocalizedText("TAG_Sold"));
				}
			}
		}
	}

	// Token: 0x060009D4 RID: 2516 RVA: 0x00034A74 File Offset: 0x00032C74
	public void BuyMap()
	{
		Debug.Log("Map purchased");
		Singleton<CoreGameManager>.Instance.levelMapHasBeenPurchasedFor = Singleton<CoreGameManager>.Instance.nextLevel;
		Singleton<CoreGameManager>.Instance.saveMapPurchased = true;
		this.itemPurchased = true;
		this.mapTag.SetText(Singleton<LocalizationManager>.Instance.GetLocalizedText("TAG_Sold"));
		if (!this.johnnyAudioManager.QueuedUp)
		{
			this.johnnyAudioManager.QueueRandomAudio(this.audMapFilled);
		}
	}

	// Token: 0x060009D5 RID: 2517 RVA: 0x00034AEC File Offset: 0x00032CEC
	private void Open()
	{
		if (!this.open)
		{
			this.johnnyBase.gameObject.SetActive(true);
			this.johnnyAudioManager.FlushQueue(true);
		}
		this.open = true;
		foreach (Door door in this.room.doors)
		{
			door.Unlock();
		}
		foreach (Cell cell in this.room.cells)
		{
			if (cell.hasLight)
			{
				cell.SetLight(true);
			}
		}
		foreach (Pickup pickup in this.pickups)
		{
			pickup.free = false;
		}
	}

	// Token: 0x060009D6 RID: 2518 RVA: 0x00034BFC File Offset: 0x00032DFC
	private void Close()
	{
		this.johnnyBase.gameObject.SetActive(false);
		this.open = false;
		foreach (Door door in this.room.doors)
		{
			door.Lock(true);
		}
		foreach (Cell cell in this.room.cells)
		{
			if (cell.hasLight)
			{
				cell.SetLight(false);
			}
		}
		foreach (Pickup pickup in this.pickups)
		{
			pickup.free = true;
		}
	}

	// Token: 0x060009D7 RID: 2519 RVA: 0x00034CFC File Offset: 0x00032EFC
	private void SetOffAlarm()
	{
		this.alarmStarted = true;
		this.room.ec.MakeNoise(this.room.ec.RealRoomMid(this.room), this.alarmNoiseValue);
		this.alarmAudioManager.QueueAudio(this.audAlarm);
		this.alarmAudioManager.SetLoop(true);
		foreach (Cell cell in this.room.cells)
		{
			if (cell.hasLight)
			{
				cell.lightColor = Color.red;
				cell.SetLight(true);
			}
		}
	}

	// Token: 0x04000B00 RID: 2816
	[SerializeField]
	private Transform roomBase;

	// Token: 0x04000B01 RID: 2817
	[SerializeField]
	private Transform johnnyBase;

	// Token: 0x04000B02 RID: 2818
	[SerializeField]
	private PropagatedAudioManagerAnimator johnnyAudioManager;

	// Token: 0x04000B03 RID: 2819
	[SerializeField]
	private AudioManager alarmAudioManager;

	// Token: 0x04000B04 RID: 2820
	[SerializeField]
	private Animator johnnyAnimator;

	// Token: 0x04000B05 RID: 2821
	[SerializeField]
	private new PriceTag[] tag = new PriceTag[0];

	// Token: 0x04000B06 RID: 2822
	[SerializeField]
	private PriceTag mapTag;

	// Token: 0x04000B07 RID: 2823
	private List<Pickup> pickups = new List<Pickup>();

	// Token: 0x04000B08 RID: 2824
	[SerializeField]
	private Pickup mapPickup;

	// Token: 0x04000B09 RID: 2825
	[SerializeField]
	private SoundObject audCough;

	// Token: 0x04000B0A RID: 2826
	[SerializeField]
	private SoundObject audJonIntro;

	// Token: 0x04000B0B RID: 2827
	[SerializeField]
	private SoundObject audExpelInfo;

	// Token: 0x04000B0C RID: 2828
	[SerializeField]
	private SoundObject audMapInfo;

	// Token: 0x04000B0D RID: 2829
	[SerializeField]
	private SoundObject audHelp;

	// Token: 0x04000B0E RID: 2830
	[SerializeField]
	private SoundObject audBell;

	// Token: 0x04000B0F RID: 2831
	[SerializeField]
	private SoundObject[] audBuy = new SoundObject[0];

	// Token: 0x04000B10 RID: 2832
	[SerializeField]
	private SoundObject[] audLeaveHappy = new SoundObject[0];

	// Token: 0x04000B11 RID: 2833
	[SerializeField]
	private SoundObject[] audLeaveSad = new SoundObject[0];

	// Token: 0x04000B12 RID: 2834
	[SerializeField]
	private SoundObject[] audMapFilled = new SoundObject[0];

	// Token: 0x04000B13 RID: 2835
	[SerializeField]
	private SoundObject[] audExpel = new SoundObject[0];

	// Token: 0x04000B14 RID: 2836
	[SerializeField]
	private SoundObject[] audUnafforable = new SoundObject[0];

	// Token: 0x04000B15 RID: 2837
	[SerializeField]
	private SoundObject[] audIntroP2;

	// Token: 0x04000B16 RID: 2838
	[SerializeField]
	private SoundObject audAlarm;

	// Token: 0x04000B17 RID: 2839
	private SceneObject storeData;

	// Token: 0x04000B18 RID: 2840
	[SerializeField]
	private float saleChance = 0.05f;

	// Token: 0x04000B19 RID: 2841
	[SerializeField]
	private float minSaleDiscount = 0.5f;

	// Token: 0x04000B1A RID: 2842
	[SerializeField]
	private float maxSaleDiscount = 0.9f;

	// Token: 0x04000B1B RID: 2843
	[SerializeField]
	private float baldiStealAnger = 2.5f;

	// Token: 0x04000B1C RID: 2844
	[SerializeField]
	private int johnnyAidLimit = 100;

	// Token: 0x04000B1D RID: 2845
	[SerializeField]
	private int notebooksPerReset = 5;

	// Token: 0x04000B1E RID: 2846
	[Range(0f, 127f)]
	public int alarmNoiseValue = 127;

	// Token: 0x04000B1F RID: 2847
	private int notebooksAtLastReset;

	// Token: 0x04000B20 RID: 2848
	private int totalCustomers;

	// Token: 0x04000B21 RID: 2849
	private bool playerEntered;

	// Token: 0x04000B22 RID: 2850
	private bool playerLeft;

	// Token: 0x04000B23 RID: 2851
	private bool itemPurchased;

	// Token: 0x04000B24 RID: 2852
	private bool inGameMode;

	// Token: 0x04000B25 RID: 2853
	private bool open;

	// Token: 0x04000B26 RID: 2854
	private bool thief;

	// Token: 0x04000B27 RID: 2855
	private bool alarmStarted;
}
