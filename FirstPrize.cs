using System;
using UnityEngine;

// Token: 0x02000006 RID: 6
public class FirstPrize : NPC, IItemAcceptor
{
	// Token: 0x0600001B RID: 27 RVA: 0x00002AB0 File Offset: 0x00000CB0
	public override void Initialize()
	{
		base.Initialize();
		this.behaviorStateMachine.ChangeState(new FirstPrize_Active(this));
	}

	// Token: 0x0600001C RID: 28 RVA: 0x00002AC9 File Offset: 0x00000CC9
	protected override void VirtualUpdate()
	{
		this.motorAudMan.pitchModifier = 1f + this.navigator.speed / 50f;
	}

	// Token: 0x0600001D RID: 29 RVA: 0x00002AED File Offset: 0x00000CED
	public void InsertItem(PlayerManager player, EnvironmentController ec)
	{
		this.CutWires();
	}

	// Token: 0x0600001E RID: 30 RVA: 0x00002AF5 File Offset: 0x00000CF5
	public bool ItemFits(Items item)
	{
		return item == Items.Scissors;
	}

	// Token: 0x0600001F RID: 31 RVA: 0x00002AFC File Offset: 0x00000CFC
	public void CutWires()
	{
		this.behaviorStateMachine.ChangeState(new FirstPrize_Stunned(this, this.cutTime));
	}

	// Token: 0x04000033 RID: 51
	[SerializeField]
	public AudioManager audMan;

	// Token: 0x04000034 RID: 52
	[SerializeField]
	public AudioManager motorAudMan;

	// Token: 0x04000035 RID: 53
	[SerializeField]
	public SoundObject[] audHug = new SoundObject[0];

	// Token: 0x04000036 RID: 54
	[SerializeField]
	public SoundObject[] audSee = new SoundObject[0];

	// Token: 0x04000037 RID: 55
	[SerializeField]
	public SoundObject[] audLose = new SoundObject[0];

	// Token: 0x04000038 RID: 56
	[SerializeField]
	public SoundObject[] audRand = new SoundObject[0];

	// Token: 0x04000039 RID: 57
	[SerializeField]
	public SoundObject audBang;

	// Token: 0x0400003A RID: 58
	[SerializeField]
	public float angleRange = 5f;

	// Token: 0x0400003B RID: 59
	[SerializeField]
	public float wanderSpeed = 10f;

	// Token: 0x0400003C RID: 60
	[SerializeField]
	public float chaseSpeed = 100f;

	// Token: 0x0400003D RID: 61
	[SerializeField]
	public float turnSpeed = 22.5f;

	// Token: 0x0400003E RID: 62
	[SerializeField]
	public float minPushSpeed = 10f;

	// Token: 0x0400003F RID: 63
	[SerializeField]
	public float randomAudioChance = 1f;

	// Token: 0x04000040 RID: 64
	[SerializeField]
	public float cutTime = 30f;

	// Token: 0x04000041 RID: 65
	[SerializeField]
	public float slamSpeed = 50f;

	// Token: 0x04000042 RID: 66
	[Range(0f, 127f)]
	public int slamNoiseValue = 64;
}
