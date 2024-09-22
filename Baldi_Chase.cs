using System;
using UnityEngine;

// Token: 0x02000015 RID: 21
public class Baldi_Chase : Baldi_StateBase
{
	// Token: 0x06000091 RID: 145 RVA: 0x000051B4 File Offset: 0x000033B4
	public Baldi_Chase(NPC npc, Baldi baldi) : base(npc, baldi)
	{
	}

	// Token: 0x06000092 RID: 146 RVA: 0x000051BE File Offset: 0x000033BE
	public override void Enter()
	{
		base.Enter();
		this.delayTimer = this.baldi.Delay;
		this.baldi.ResetSlapDistance();
		this.baldi.ResetSprite();
	}

	// Token: 0x06000093 RID: 147 RVA: 0x000051F0 File Offset: 0x000033F0
	public override void OnStateTriggerStay(Collider other)
	{
		base.OnStateTriggerStay(other);
		if (other.CompareTag("Player"))
		{
			bool flag;
			this.baldi.looker.Raycast(other.transform, Vector3.Magnitude(this.baldi.transform.position - other.transform.position), out flag);
			if (flag)
			{
				PlayerManager component = other.GetComponent<PlayerManager>();
				ItemManager itm = component.itm;
				if (!component.invincible)
				{
					if (itm.Has(Items.Apple))
					{
						itm.Remove(Items.Apple);
						this.baldi.TakeApple();
						return;
					}
					this.baldi.CaughtPlayer(component);
				}
			}
		}
	}

	// Token: 0x06000094 RID: 148 RVA: 0x00005294 File Offset: 0x00003494
	public override void Update()
	{
		base.Update();
		this.baldi.UpdateSlapDistance();
		this.delayTimer -= Time.deltaTime * this.npc.TimeScale;
		if (this.delayTimer <= 0f)
		{
			this.baldi.Slap();
			this.ActivateSlapAnimation();
			this.delayTimer = this.baldi.Delay;
		}
	}

	// Token: 0x040000B4 RID: 180
	private float delayTimer;
}
