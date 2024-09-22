using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001E4 RID: 484
public class ClassicSwingDoor : MonoBehaviour
{
	// Token: 0x06000AF7 RID: 2807 RVA: 0x00039BEF File Offset: 0x00037DEF
	private void Start()
	{
		this.oldLockMat = this.door.overlayLocked;
		this.door.overlayLocked = this.door.overlayShut;
		this.door.Lock(true);
		ClassicSwingDoor.allClassicDoors.Add(this);
	}

	// Token: 0x06000AF8 RID: 2808 RVA: 0x00039C2F File Offset: 0x00037E2F
	private void OnDisable()
	{
		ClassicSwingDoor.allClassicDoors.Remove(this);
	}

	// Token: 0x06000AF9 RID: 2809 RVA: 0x00039C40 File Offset: 0x00037E40
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" && Singleton<BaseGameManager>.Instance.FoundNotebooks < 2)
		{
			this.door.audMan.FlushQueue(true);
			this.door.audMan.QueueAudio(this.audYouNeed);
		}
	}

	// Token: 0x06000AFA RID: 2810 RVA: 0x00039C93 File Offset: 0x00037E93
	public void Unlock()
	{
		this.door.Unlock();
		this.door.overlayLocked = this.oldLockMat;
	}

	// Token: 0x04000C8C RID: 3212
	public static List<ClassicSwingDoor> allClassicDoors = new List<ClassicSwingDoor>();

	// Token: 0x04000C8D RID: 3213
	public SwingDoor door;

	// Token: 0x04000C8E RID: 3214
	public SoundObject audYouNeed;

	// Token: 0x04000C8F RID: 3215
	private Material[] oldLockMat;
}
