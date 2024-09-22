using System;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x020001AF RID: 431
public class SilenceRoomFunction : RoomFunction
{
	// Token: 0x060009BE RID: 2494 RVA: 0x00033FE4 File Offset: 0x000321E4
	public override void OnGenerationFinished()
	{
		base.OnGenerationFinished();
		foreach (Cell cell in this.room.cells)
		{
			cell.SetSilence(true);
		}
	}

	// Token: 0x060009BF RID: 2495 RVA: 0x00034040 File Offset: 0x00032240
	public override void OnPlayerEnter(PlayerManager player)
	{
		base.OnPlayerEnter(player);
		AudioListener.volume = 0f;
		this.mixer.SetFloat("EchoWetMix", 1f);
	}

	// Token: 0x060009C0 RID: 2496 RVA: 0x00034069 File Offset: 0x00032269
	public override void OnPlayerExit(PlayerManager player)
	{
		base.OnPlayerExit(player);
		AudioListener.volume = 1f;
		this.mixer.SetFloat("EchoWetMix", 0f);
	}

	// Token: 0x060009C1 RID: 2497 RVA: 0x00034092 File Offset: 0x00032292
	private void OnDestroy()
	{
		Debug.Log("Volume restored because SilenceRoomFunction was destroyed");
		AudioListener.volume = 1f;
		this.mixer.SetFloat("EchoWetMix", 0f);
	}

	// Token: 0x04000AFD RID: 2813
	[SerializeField]
	private AudioMixer mixer;

	// Token: 0x04000AFE RID: 2814
	[SerializeField]
	private bool silenceCells = true;
}
