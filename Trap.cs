using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001CA RID: 458
public class Trap : EnvironmentObject
{
	// Token: 0x06000A61 RID: 2657 RVA: 0x000372EC File Offset: 0x000354EC
	private void OnTriggerEnter(Collider other)
	{
		if (!this.triggered && (other.tag == "Player" || (other.tag == "NPC" && !other.isTrigger)))
		{
			this.currentModifier = other.GetComponent<ActivityModifier>();
			this.currentModifier.moveMods.Add(this.moveMod);
			base.StartCoroutine(this.Timer(this.trapTime));
			this.triggered = true;
			if (this.audMan != null)
			{
				this.audMan.PlaySingle(this.audTrigger);
			}
			if (this.spriteRenderer != null)
			{
				this.spriteRenderer.sprite = this.triggeredSprite;
			}
			if (this.meshRenderer != null)
			{
				this.meshRenderer.sharedMaterial = this.triggeredMaterial;
			}
		}
	}

	// Token: 0x06000A62 RID: 2658 RVA: 0x000373CF File Offset: 0x000355CF
	private IEnumerator Timer(float time)
	{
		while (time > 0f)
		{
			time -= Time.deltaTime * this.ec.EnvironmentTimeScale;
			yield return null;
		}
		this.currentModifier.moveMods.Remove(this.moveMod);
		yield break;
	}

	// Token: 0x170000DC RID: 220
	// (get) Token: 0x06000A63 RID: 2659 RVA: 0x000373E5 File Offset: 0x000355E5
	public bool Triggered
	{
		get
		{
			return this.triggered;
		}
	}

	// Token: 0x06000A64 RID: 2660 RVA: 0x000373ED File Offset: 0x000355ED
	public void Set(bool active)
	{
		this.triggered = !active;
	}

	// Token: 0x04000BDC RID: 3036
	private ActivityModifier currentModifier;

	// Token: 0x04000BDD RID: 3037
	[SerializeField]
	private MovementModifier moveMod;

	// Token: 0x04000BDE RID: 3038
	[SerializeField]
	private AudioManager audMan;

	// Token: 0x04000BDF RID: 3039
	[SerializeField]
	private SoundObject audTrigger;

	// Token: 0x04000BE0 RID: 3040
	[SerializeField]
	private SpriteRenderer spriteRenderer;

	// Token: 0x04000BE1 RID: 3041
	[SerializeField]
	private Sprite triggeredSprite;

	// Token: 0x04000BE2 RID: 3042
	[SerializeField]
	private MeshRenderer meshRenderer;

	// Token: 0x04000BE3 RID: 3043
	[SerializeField]
	private Material triggeredMaterial;

	// Token: 0x04000BE4 RID: 3044
	[SerializeField]
	private float trapTime = 5f;

	// Token: 0x04000BE5 RID: 3045
	private bool triggered;
}
