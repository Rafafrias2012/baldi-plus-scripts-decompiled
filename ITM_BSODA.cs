using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000085 RID: 133
public class ITM_BSODA : Item, IEntityTrigger
{
	// Token: 0x06000318 RID: 792 RVA: 0x00010098 File Offset: 0x0000E298
	public override bool Use(PlayerManager pm)
	{
		this.ec = pm.ec;
		base.transform.position = pm.transform.position;
		base.transform.forward = Singleton<CoreGameManager>.Instance.GetCamera(pm.playerNumber).transform.forward;
		this.entity.Initialize(this.ec, base.transform.position);
		this.spriteRenderer.SetSpriteRotation(Random.Range(0f, 360f));
		Singleton<CoreGameManager>.Instance.audMan.PlaySingle(this.sound);
		pm.RuleBreak("Drinking", 0.8f, 0.25f);
		return true;
	}

	// Token: 0x06000319 RID: 793 RVA: 0x00010150 File Offset: 0x0000E350
	private void Update()
	{
		this.moveMod.movementAddend = this.entity.ExternalActivity.Addend + base.transform.forward * this.speed * this.ec.EnvironmentTimeScale;
		this.entity.UpdateInternalMovement(base.transform.forward * this.speed * this.ec.EnvironmentTimeScale);
		this.time -= Time.deltaTime * this.ec.EnvironmentTimeScale;
		if (this.time <= 0f)
		{
			foreach (ActivityModifier activityModifier in this.activityMods)
			{
				activityModifier.moveMods.Remove(this.moveMod);
			}
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x0600031A RID: 794 RVA: 0x0001025C File Offset: 0x0000E45C
	public void EntityTriggerEnter(Collider other)
	{
		Entity component = other.GetComponent<Entity>();
		if ((!other.CompareTag("Player") || !this.launching) && component != null)
		{
			component.ExternalActivity.moveMods.Add(this.moveMod);
			this.activityMods.Add(component.ExternalActivity);
		}
	}

	// Token: 0x0600031B RID: 795 RVA: 0x000102B5 File Offset: 0x0000E4B5
	public void EntityTriggerStay(Collider other)
	{
	}

	// Token: 0x0600031C RID: 796 RVA: 0x000102B8 File Offset: 0x0000E4B8
	public void EntityTriggerExit(Collider other)
	{
		Entity component = other.GetComponent<Entity>();
		if (other.CompareTag("Player"))
		{
			this.launching = false;
		}
		if (component != null)
		{
			component.ExternalActivity.moveMods.Remove(this.moveMod);
			this.activityMods.Remove(component.ExternalActivity);
		}
	}

	// Token: 0x04000345 RID: 837
	private EnvironmentController ec;

	// Token: 0x04000346 RID: 838
	[SerializeField]
	private Entity entity;

	// Token: 0x04000347 RID: 839
	[SerializeField]
	private MovementModifier moveMod;

	// Token: 0x04000348 RID: 840
	[SerializeField]
	private SpriteRenderer spriteRenderer;

	// Token: 0x04000349 RID: 841
	[SerializeField]
	private SoundObject sound;

	// Token: 0x0400034A RID: 842
	[SerializeField]
	private float speed;

	// Token: 0x0400034B RID: 843
	[SerializeField]
	private float time;

	// Token: 0x0400034C RID: 844
	private bool launching = true;

	// Token: 0x0400034D RID: 845
	private List<ActivityModifier> activityMods = new List<ActivityModifier>();
}
