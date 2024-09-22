using System;
using UnityEngine;

// Token: 0x020001AC RID: 428
public class FieldTripEntranceRoomFunction : RoomFunction
{
	// Token: 0x060009B4 RID: 2484 RVA: 0x00033D34 File Offset: 0x00031F34
	public override void Initialize(RoomController room)
	{
		base.Initialize(room);
		if (!Singleton<CoreGameManager>.Instance.tripPlayed && Singleton<CoreGameManager>.Instance.tripAvailable)
		{
			this.busObjects.SetParent(room.objectObject.transform);
			this.animator.Play(Singleton<CoreGameManager>.Instance.currentFieldTrip.animationName, -1, 1f);
			this.volumeAnimator.animationName = Singleton<CoreGameManager>.Instance.currentFieldTrip.animationName;
			this.available = true;
			return;
		}
		this.busObjects.gameObject.SetActive(false);
	}

	// Token: 0x060009B5 RID: 2485 RVA: 0x00033DCC File Offset: 0x00031FCC
	public override void OnPlayerEnter(PlayerManager player)
	{
		base.OnPlayerEnter(player);
		if (!this.entered && this.available)
		{
			this.baldiAudioManager.QueueAudio(this.audCampIntro);
			this.animator.Play("CampIntro", -1, 0f);
			this.entered = true;
		}
		if (Singleton<CoreGameManager>.Instance.tripPlayed)
		{
			this.busObjects.gameObject.SetActive(false);
		}
	}

	// Token: 0x060009B6 RID: 2486 RVA: 0x00033E3C File Offset: 0x0003203C
	public void StartFieldTrip(PlayerManager player)
	{
		if (player.itm.Has(Items.BusPass) || this.unlocked)
		{
			if (!this.unlocked)
			{
				player.itm.Remove(Items.BusPass);
			}
			Singleton<BaseGameManager>.Instance.CallSpecialManagerFunction(1, base.gameObject);
			this.unlocked = true;
			return;
		}
		this.baldiAudioManager.FlushQueue(true);
		this.baldiAudioManager.QueueAudio(this.audNoPass);
	}

	// Token: 0x060009B7 RID: 2487 RVA: 0x00033EAB File Offset: 0x000320AB
	public void BusClicked()
	{
		this.StartFieldTrip(Singleton<CoreGameManager>.Instance.GetPlayer(0));
	}

	// Token: 0x04000AEC RID: 2796
	[SerializeField]
	private Transform busObjects;

	// Token: 0x04000AED RID: 2797
	[SerializeField]
	private AudioManager baldiAudioManager;

	// Token: 0x04000AEE RID: 2798
	[SerializeField]
	private SoundObject audCampIntro;

	// Token: 0x04000AEF RID: 2799
	[SerializeField]
	private SoundObject audNoPass;

	// Token: 0x04000AF0 RID: 2800
	[SerializeField]
	private Animator animator;

	// Token: 0x04000AF1 RID: 2801
	[SerializeField]
	private VolumeAnimator volumeAnimator;

	// Token: 0x04000AF2 RID: 2802
	private bool entered;

	// Token: 0x04000AF3 RID: 2803
	private bool unlocked;

	// Token: 0x04000AF4 RID: 2804
	private bool available;
}
