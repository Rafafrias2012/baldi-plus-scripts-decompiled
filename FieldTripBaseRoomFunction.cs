using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001AB RID: 427
public class FieldTripBaseRoomFunction : RoomFunction
{
	// Token: 0x060009AA RID: 2474 RVA: 0x00033894 File Offset: 0x00031A94
	public override void Initialize(RoomController room)
	{
		base.Initialize(room);
		this.objectBase.SetParent(room.objectObject.transform);
		List<ItemObject> list = new List<ItemObject>();
		list.AddRange(this.guaranteedItems);
		while ((float)list.Count < this.itemCount)
		{
			List<ItemObject> list2 = list;
			WeightedSelection<ItemObject>[] items = this.potentialItems;
			list2.Add(WeightedSelection<ItemObject>.RandomSelection(items));
		}
		int num = 0;
		while ((float)num < this.itemCount && num < room.itemSpawnPoints.Count)
		{
			Pickup pickup = room.ec.CreateItem(room, list[num], room.itemSpawnPoints[num].position);
			pickup.gameObject.SetActive(false);
			pickup.OnItemCollected += this.ItemCollected;
			this.pickups.Add(pickup);
			num++;
		}
		this.pickupsToDisable.AddRange(this.pickups);
	}

	// Token: 0x060009AB RID: 2475 RVA: 0x00033974 File Offset: 0x00031B74
	private void Update()
	{
		if (this.minigameActive && !Singleton<GlobalCam>.Instance.TransitionActive && this.cameraRendering)
		{
			Singleton<CoreGameManager>.Instance.GetCamera(0).StopRendering(true);
			this.cameraRendering = false;
		}
	}

	// Token: 0x060009AC RID: 2476 RVA: 0x000339AA File Offset: 0x00031BAA
	public override void OnPlayerEnter(PlayerManager player)
	{
		base.OnPlayerEnter(player);
		if (!this.playerEntered)
		{
			this.playerEntered = true;
			this.introAudioManager.QueueAudio(this.audIntro);
		}
	}

	// Token: 0x060009AD RID: 2477 RVA: 0x000339D4 File Offset: 0x00031BD4
	public override void OnPlayerExit(PlayerManager player)
	{
		base.OnPlayerExit(player);
		if (this.minigamePlayed && !this.itemsMoved)
		{
			this.itemsMoved = true;
			foreach (Pickup pickup in this.pickups)
			{
				pickup.transform.position += this.pickupsToPitOffset;
			}
		}
	}

	// Token: 0x060009AE RID: 2478 RVA: 0x00033A58 File Offset: 0x00031C58
	public void StartMinigame(MinigameBase minigamePrefab)
	{
		if (!this.minigameActive && !this.minigamePlayed)
		{
			this.currentMinigame = Object.Instantiate<MinigameBase>(minigamePrefab);
			this.currentMinigame.Initialize(this, Singleton<CoreGameManager>.Instance.currentMode == Mode.Free);
			this.room.ec.AddTimeScale(this.minigameTimeScale);
			Singleton<InputManager>.Instance.ActivateActionSet("Interface");
			this.minigameActive = true;
			Singleton<CoreGameManager>.Instance.disablePause = true;
			AudioListener.pause = true;
			this.introAudioManager.FlushQueue(true);
		}
	}

	// Token: 0x060009AF RID: 2479 RVA: 0x00033AE4 File Offset: 0x00031CE4
	public void EndMinigame(bool result, bool finished)
	{
		if (this.minigameActive)
		{
			Object.Destroy(this.currentMinigame.gameObject);
			this.room.ec.RemoveTimeScale(this.minigameTimeScale);
			Singleton<InputManager>.Instance.ActivateActionSet("InGame");
			Singleton<InputManager>.Instance.StopFrame();
			this.minigameActive = false;
			Singleton<CoreGameManager>.Instance.GetCamera(0).StopRendering(false);
			this.cameraRendering = true;
			this.minigamePlayed = finished;
			Singleton<CoreGameManager>.Instance.tripPlayed = finished;
			if (finished)
			{
				if (result)
				{
					this.SpawnItems();
				}
				foreach (GameObject gameObject in this.objectToToggleAfterMinigame)
				{
					gameObject.SetActive(!gameObject.activeSelf);
				}
			}
			Singleton<CoreGameManager>.Instance.disablePause = false;
			AudioListener.pause = false;
		}
	}

	// Token: 0x060009B0 RID: 2480 RVA: 0x00033BB0 File Offset: 0x00031DB0
	private void ItemCollected(Pickup pickup, int player)
	{
		this.itemsCollected++;
		this.pickupsToDisable.Remove(pickup);
		pickup.OnItemCollected -= this.ItemCollected;
		if ((float)this.itemsCollected >= this.itemLimit)
		{
			this.HideItems();
		}
	}

	// Token: 0x060009B1 RID: 2481 RVA: 0x00033C00 File Offset: 0x00031E00
	private void SpawnItems()
	{
		foreach (Pickup pickup in this.pickups)
		{
			pickup.gameObject.SetActive(true);
		}
	}

	// Token: 0x060009B2 RID: 2482 RVA: 0x00033C58 File Offset: 0x00031E58
	private void HideItems()
	{
		foreach (Pickup pickup in this.pickupsToDisable)
		{
			pickup.gameObject.SetActive(false);
		}
	}

	// Token: 0x04000AD9 RID: 2777
	private MinigameBase currentMinigame;

	// Token: 0x04000ADA RID: 2778
	private List<Pickup> pickups = new List<Pickup>();

	// Token: 0x04000ADB RID: 2779
	private List<Pickup> pickupsToDisable = new List<Pickup>();

	// Token: 0x04000ADC RID: 2780
	[SerializeField]
	private Transform objectBase;

	// Token: 0x04000ADD RID: 2781
	[SerializeField]
	private GameObject[] objectToToggleAfterMinigame = new GameObject[0];

	// Token: 0x04000ADE RID: 2782
	[SerializeField]
	private AudioManager introAudioManager;

	// Token: 0x04000ADF RID: 2783
	[SerializeField]
	private SoundObject audIntro;

	// Token: 0x04000AE0 RID: 2784
	[SerializeField]
	private List<ItemObject> guaranteedItems = new List<ItemObject>();

	// Token: 0x04000AE1 RID: 2785
	[SerializeField]
	private WeightedItemObject[] potentialItems = new WeightedItemObject[0];

	// Token: 0x04000AE2 RID: 2786
	private TimeScaleModifier minigameTimeScale = new TimeScaleModifier(0f, 0f, 0f);

	// Token: 0x04000AE3 RID: 2787
	[SerializeField]
	private Vector3 pickupsToPitOffset;

	// Token: 0x04000AE4 RID: 2788
	[SerializeField]
	private float itemCount = 8f;

	// Token: 0x04000AE5 RID: 2789
	[SerializeField]
	private float itemLimit = 3f;

	// Token: 0x04000AE6 RID: 2790
	private int itemsCollected;

	// Token: 0x04000AE7 RID: 2791
	private bool minigameActive;

	// Token: 0x04000AE8 RID: 2792
	private bool minigamePlayed;

	// Token: 0x04000AE9 RID: 2793
	private bool cameraRendering = true;

	// Token: 0x04000AEA RID: 2794
	private bool playerEntered;

	// Token: 0x04000AEB RID: 2795
	private bool itemsMoved;
}
