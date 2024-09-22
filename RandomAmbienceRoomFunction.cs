using System;
using UnityEngine;

// Token: 0x020001AE RID: 430
public class RandomAmbienceRoomFunction : RoomFunction
{
	// Token: 0x060009BB RID: 2491 RVA: 0x00033F45 File Offset: 0x00032145
	private void Start()
	{
		this.time = this.rate;
	}

	// Token: 0x060009BC RID: 2492 RVA: 0x00033F54 File Offset: 0x00032154
	public override void OnPlayerStay(PlayerManager player)
	{
		base.OnPlayerStay(player);
		this.time -= Time.deltaTime;
		if (this.time <= 0f)
		{
			if (Random.value < this.chance)
			{
				AudioManager audioManager = this.audioManager;
				WeightedSelection<SoundObject>[] items = this.potentialSounds;
				audioManager.PlaySingle(WeightedSelection<SoundObject>.RandomSelection(items));
			}
			this.time = this.rate;
		}
	}

	// Token: 0x04000AF8 RID: 2808
	[SerializeField]
	private AudioManager audioManager;

	// Token: 0x04000AF9 RID: 2809
	[SerializeField]
	private WeightedSoundObject[] potentialSounds = new WeightedSoundObject[0];

	// Token: 0x04000AFA RID: 2810
	[SerializeField]
	private float rate = 1f;

	// Token: 0x04000AFB RID: 2811
	[SerializeField]
	private float chance = 0.25f;

	// Token: 0x04000AFC RID: 2812
	private float time;
}
