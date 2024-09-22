using System;
using UnityEngine;

// Token: 0x02000070 RID: 112
public class GravityFlipper : MonoBehaviour
{
	// Token: 0x06000285 RID: 645 RVA: 0x0000E184 File Offset: 0x0000C384
	public void Initialize(GravityEvent gravityEvent)
	{
		this.gravityEvent = gravityEvent;
	}

	// Token: 0x06000286 RID: 646 RVA: 0x0000E18D File Offset: 0x0000C38D
	private void Update()
	{
		base.transform.Rotate(Vector3.one * 25f * Time.deltaTime);
	}

	// Token: 0x06000287 RID: 647 RVA: 0x0000E1B4 File Offset: 0x0000C3B4
	private void OnTriggerEnter(Collider other)
	{
		if (other.isTrigger)
		{
			if (other.CompareTag("Player"))
			{
				this.gravityEvent.FlipPlayer();
				this.gravityEvent.DestroyFlipper(this);
				return;
			}
			if (other.CompareTag("NPC"))
			{
				this.gravityEvent.FlipNPC(other.GetComponent<NPC>());
				this.gravityEvent.DestroyFlipper(this);
			}
		}
	}

	// Token: 0x040002A0 RID: 672
	private GravityEvent gravityEvent;

	// Token: 0x040002A1 RID: 673
	public float _npcDistance;
}
