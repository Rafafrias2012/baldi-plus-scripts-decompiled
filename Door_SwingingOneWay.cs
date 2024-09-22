using System;
using UnityEngine;

// Token: 0x020000AA RID: 170
public class Door_SwingingOneWay : SwingDoor
{
	// Token: 0x060003E1 RID: 993 RVA: 0x0001449B File Offset: 0x0001269B
	protected override void Start()
	{
		base.Start();
		base.bTile.Block(this.direction.GetOpposite(), true);
		this.bMapTile.SpriteRenderer.sprite = this.mapLockedSprite;
	}

	// Token: 0x060003E2 RID: 994 RVA: 0x000144D0 File Offset: 0x000126D0
	public override void OpenTimed(float time, bool makeNoise)
	{
		if (!this.locked)
		{
			this.oneWayCollider.enabled = false;
		}
		base.OpenTimed(time, makeNoise);
	}

	// Token: 0x060003E3 RID: 995 RVA: 0x000144EE File Offset: 0x000126EE
	public override void Shut()
	{
		this.oneWayCollider.enabled = true;
		base.Shut();
	}

	// Token: 0x060003E4 RID: 996 RVA: 0x00014502 File Offset: 0x00012702
	public override void Unlock()
	{
		base.Unlock();
		this.bMapTile.SpriteRenderer.sprite = this.mapLockedSprite;
	}

	// Token: 0x04000417 RID: 1047
	[SerializeField]
	private Collider oneWayCollider;
}
