using System;
using UnityEngine;

// Token: 0x020001FF RID: 511
public class MinigameTest : MonoBehaviour
{
	// Token: 0x06000B5E RID: 2910 RVA: 0x0003BED4 File Offset: 0x0003A0D4
	private void Start()
	{
		if (this.autoStart)
		{
			this.StartMinigame(this.minigameToPlay);
		}
	}

	// Token: 0x06000B5F RID: 2911 RVA: 0x0003BEEC File Offset: 0x0003A0EC
	private void Update()
	{
		if (!this.minigameActive && Input.anyKeyDown && Input.inputString.Length > 0 && char.IsNumber(Input.inputString, 0))
		{
			this.StartMinigame(int.Parse(Input.inputString[0].ToString()));
		}
	}

	// Token: 0x06000B60 RID: 2912 RVA: 0x0003BF40 File Offset: 0x0003A140
	private void StartMinigame(int minigameToPlay)
	{
		this.currentMinigame = Object.Instantiate<MinigameBase>(this.minigame[minigameToPlay]);
		this.currentMinigame.Initialize(this.testRoomFunction, this.scoringMode);
		Singleton<InputManager>.Instance.ActivateActionSet("Interface");
		this.minigameActive = true;
		AudioListener.pause = true;
	}

	// Token: 0x06000B61 RID: 2913 RVA: 0x0003BF93 File Offset: 0x0003A193
	public void EndMinigame()
	{
		Object.Destroy(this.currentMinigame.gameObject);
		this.minigameActive = false;
		AudioListener.pause = false;
	}

	// Token: 0x04000DAB RID: 3499
	public FieldTripBaseRoomFunction testRoomFunction;

	// Token: 0x04000DAC RID: 3500
	public MinigameBase[] minigame;

	// Token: 0x04000DAD RID: 3501
	private MinigameBase currentMinigame;

	// Token: 0x04000DAE RID: 3502
	public int minigameToPlay;

	// Token: 0x04000DAF RID: 3503
	public bool autoStart;

	// Token: 0x04000DB0 RID: 3504
	public bool scoringMode;

	// Token: 0x04000DB1 RID: 3505
	private bool minigameActive;
}
