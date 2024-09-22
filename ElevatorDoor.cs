using System;
using UnityEngine;

// Token: 0x020001BD RID: 445
public class ElevatorDoor : Door
{
	// Token: 0x06000A0B RID: 2571 RVA: 0x00035A28 File Offset: 0x00033C28
	public override void Open(bool cancelTimer, bool makeNoise)
	{
		base.Open(cancelTimer, makeNoise);
		this.animator.Play("Open");
		this.audMan.PlaySingle(this.audDoorOpen);
		MeshCollider[] array = this.colliders;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = false;
		}
		base.aTile.Block(this.direction, false);
	}

	// Token: 0x06000A0C RID: 2572 RVA: 0x00035A90 File Offset: 0x00033C90
	public override void Shut()
	{
		base.Shut();
		this.animator.Play("Shut");
		this.audMan.PlaySingle(this.audDoorShut);
		MeshCollider[] array = this.colliders;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = true;
		}
		base.aTile.Block(this.direction, true);
	}

	// Token: 0x04000B68 RID: 2920
	[SerializeField]
	private Animator animator;

	// Token: 0x04000B69 RID: 2921
	[SerializeField]
	private MeshCollider[] colliders;

	// Token: 0x04000B6A RID: 2922
	[SerializeField]
	private AudioManager audMan;

	// Token: 0x04000B6B RID: 2923
	[SerializeField]
	private SoundObject audDoorOpen;

	// Token: 0x04000B6C RID: 2924
	[SerializeField]
	private SoundObject audDoorShut;
}
