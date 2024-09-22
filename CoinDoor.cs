using System;
using UnityEngine;

// Token: 0x020000A4 RID: 164
public class CoinDoor : MonoBehaviour, IItemAcceptor
{
	// Token: 0x060003C5 RID: 965 RVA: 0x000138BC File Offset: 0x00011ABC
	private void Start()
	{
		if (this.originalOverlays.Length == 0)
		{
			this.originalOverlays = new Material[this.swingDoor.overlayLocked.Length];
			for (int i = 0; i < this.swingDoor.overlayLocked.Length; i++)
			{
				this.originalOverlays[i] = this.swingDoor.overlayLocked[i];
				this.swingDoor.overlayLocked[i] = this.coinDoorOverlay;
			}
		}
		if (this.lockOnStart)
		{
			this.swingDoor.Lock(true);
			this.lockOnStart = false;
		}
	}

	// Token: 0x060003C6 RID: 966 RVA: 0x00013948 File Offset: 0x00011B48
	public void InsertItem(PlayerManager player, EnvironmentController ec)
	{
		for (int i = 0; i < this.swingDoor.overlayLocked.Length; i++)
		{
			this.swingDoor.overlayLocked[i] = this.originalOverlays[i];
		}
		this.swingDoor.Unlock();
		Object.Destroy(this);
	}

	// Token: 0x060003C7 RID: 967 RVA: 0x00013993 File Offset: 0x00011B93
	public bool ItemFits(Items item)
	{
		return item == Items.Quarter;
	}

	// Token: 0x040003F7 RID: 1015
	[SerializeField]
	private SwingDoor swingDoor;

	// Token: 0x040003F8 RID: 1016
	[SerializeField]
	private Material coinDoorOverlay;

	// Token: 0x040003F9 RID: 1017
	public Material[] originalOverlays;

	// Token: 0x040003FA RID: 1018
	[SerializeField]
	private bool lockOnStart = true;
}
