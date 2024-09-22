using System;
using UnityEngine;

// Token: 0x020001C5 RID: 453
public class GameLever : GameButtonBase
{
	// Token: 0x06000A4A RID: 2634 RVA: 0x00036E61 File Offset: 0x00035061
	private void Awake()
	{
		this.Set(this.startOn);
	}

	// Token: 0x06000A4B RID: 2635 RVA: 0x00036E70 File Offset: 0x00035070
	protected override void Pressed(int playerNumber)
	{
		base.Pressed(playerNumber);
		this.Set(!this.on);
		foreach (IButtonReceiver buttonReceiver in this.buttonReceivers)
		{
			buttonReceiver.ButtonPressed(this.on);
		}
	}

	// Token: 0x06000A4C RID: 2636 RVA: 0x00036EDC File Offset: 0x000350DC
	public void Set(bool val)
	{
		this.on = val;
		if (this.on)
		{
			this.meshRenderer.sharedMaterial = this.onMat;
			this.audMan.PlaySingle(this.audOn);
			return;
		}
		this.meshRenderer.sharedMaterial = this.offMat;
		this.audMan.PlaySingle(this.audOff);
	}

	// Token: 0x04000BB6 RID: 2998
	[SerializeField]
	private MeshRenderer meshRenderer;

	// Token: 0x04000BB7 RID: 2999
	[SerializeField]
	private Material onMat;

	// Token: 0x04000BB8 RID: 3000
	[SerializeField]
	private Material offMat;

	// Token: 0x04000BB9 RID: 3001
	[SerializeField]
	private AudioManager audMan;

	// Token: 0x04000BBA RID: 3002
	[SerializeField]
	private SoundObject audOn;

	// Token: 0x04000BBB RID: 3003
	[SerializeField]
	private SoundObject audOff;

	// Token: 0x04000BBC RID: 3004
	[SerializeField]
	private bool startOn;

	// Token: 0x04000BBD RID: 3005
	private bool on;
}
