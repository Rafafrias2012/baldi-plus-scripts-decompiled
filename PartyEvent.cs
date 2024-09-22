using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000076 RID: 118
public class PartyEvent : RandomEvent
{
	// Token: 0x060002AD RID: 685 RVA: 0x0000EAD8 File Offset: 0x0000CCD8
	public override void Initialize(EnvironmentController controller, Random rng)
	{
		base.Initialize(controller, rng);
		WeightedSelection<ItemObject>[] items = this.potentialItems;
		this.currentItem = WeightedSelection<ItemObject>.ControlledRandomSelection(items, rng);
	}

	// Token: 0x060002AE RID: 686 RVA: 0x0000EB04 File Offset: 0x0000CD04
	public override void Begin()
	{
		base.Begin();
		this.office = this.ec.offices[this.crng.Next(0, this.ec.offices.Count)];
		this.office.functionObject.AddComponent<VA_Box>().BoxCollider = this.office.functionObject.GetComponent<BoxCollider>();
		this.partyAudio = Object.Instantiate<AudioManager>(this.partyAudioPre, this.office.transform);
		this.partyAudio.transform.position = this.ec.RealRoomMid(this.office);
		this.partyAudio.QueueAudio(this.musParty);
		this.partyAudio.SetLoop(true);
		foreach (NPC npc in this.ec.Npcs)
		{
			if (npc.Navigator.enabled)
			{
				NavigationState_PartyEvent navigationState_PartyEvent = new NavigationState_PartyEvent(npc, 31, this.office);
				this.navigationStates.Add(navigationState_PartyEvent);
				npc.navigationStateMachine.ChangeState(navigationState_PartyEvent);
			}
		}
		this.currentPickup = Object.Instantiate<Pickup>(this.pickupPre);
		if (this.currentItem != null)
		{
			this.currentPickup.item = this.currentItem;
		}
		else
		{
			Pickup pickup = this.currentPickup;
			WeightedSelection<ItemObject>[] items = this.potentialItems;
			pickup.item = WeightedSelection<ItemObject>.ControlledRandomSelection(items, new Random());
		}
		this.currentPickup.transform.position = this.ec.RealRoomMid(this.office);
		this.currentPickup.transform.position += Vector3.up * 5f;
		this.currentPickup.OnItemCollected += this.ItemCollected;
		for (int i = 0; i < this.balloonCount; i++)
		{
			Object.Instantiate<Balloon>(this.balloon[Random.Range(0, this.balloon.Length)]).Initialize(this.office);
		}
	}

	// Token: 0x060002AF RID: 687 RVA: 0x0000ED2C File Offset: 0x0000CF2C
	public override void End()
	{
		base.End();
		foreach (NavigationState_PartyEvent navigationState_PartyEvent in this.navigationStates)
		{
			navigationState_PartyEvent.End();
		}
		if (!this.itemCollected)
		{
			Object.Destroy(this.currentPickup.gameObject);
		}
		Object.Destroy(this.partyAudio.gameObject);
	}

	// Token: 0x060002B0 RID: 688 RVA: 0x0000EDAC File Offset: 0x0000CFAC
	public void ItemCollected(Pickup pickup, int player)
	{
		this.itemCollected = true;
	}

	// Token: 0x040002C2 RID: 706
	private RoomController office;

	// Token: 0x040002C3 RID: 707
	[SerializeField]
	private Pickup pickupPre;

	// Token: 0x040002C4 RID: 708
	private Pickup currentPickup;

	// Token: 0x040002C5 RID: 709
	[SerializeField]
	private WeightedItemObject[] potentialItems;

	// Token: 0x040002C6 RID: 710
	[SerializeField]
	private ItemObject currentItem;

	// Token: 0x040002C7 RID: 711
	[SerializeField]
	private AudioManager partyAudioPre;

	// Token: 0x040002C8 RID: 712
	private AudioManager partyAudio;

	// Token: 0x040002C9 RID: 713
	private List<NavigationState_PartyEvent> navigationStates = new List<NavigationState_PartyEvent>();

	// Token: 0x040002CA RID: 714
	[SerializeField]
	private Balloon[] balloon = new Balloon[0];

	// Token: 0x040002CB RID: 715
	public int balloonCount = 5;

	// Token: 0x040002CC RID: 716
	[SerializeField]
	private SoundObject musParty;

	// Token: 0x040002CD RID: 717
	private bool itemCollected;
}
