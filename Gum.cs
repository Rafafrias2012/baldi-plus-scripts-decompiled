using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000024 RID: 36
public class Gum : MonoBehaviour, IEntityTrigger
{
	// Token: 0x060000CF RID: 207 RVA: 0x00005C6C File Offset: 0x00003E6C
	public void Initialize(EnvironmentController environmentController, Beans beans)
	{
		this.ec = environmentController;
		this.entity.Initialize(this.ec, base.transform.position);
		this.entity.SetActive(false);
		this.entity.OnEntityMoveInitialCollision += this.OnEntityMoveCollision;
		this.beans = beans;
		this.Hide();
	}

	// Token: 0x060000D0 RID: 208 RVA: 0x00005CCC File Offset: 0x00003ECC
	private void Update()
	{
		if (!this.hidden)
		{
			if (this.flying)
			{
				this.entity.UpdateInternalMovement(base.transform.forward * this.speed * this.ec.EnvironmentTimeScale);
			}
			else if (this.actMod != null)
			{
				this.entity.UpdateInternalMovement(Vector3.zero);
				base.transform.position = this.actMod.transform.position;
			}
			if (this.done && (!this.hitCollider.enabled || this.hitCollider.transform.position != this.hitColliderPosition || this.hitCollider.transform.rotation != this.hitColliderRotation))
			{
				this.Hide();
			}
		}
	}

	// Token: 0x060000D1 RID: 209 RVA: 0x00005DB0 File Offset: 0x00003FB0
	public void EntityTriggerEnter(Collider other)
	{
		if (this.flying)
		{
			if (other.isTrigger && (other.CompareTag("Player") || other.CompareTag("NPC")) && (other.transform != this.beans.transform || this.leftBeans))
			{
				this.actMod = other.GetComponent<ActivityModifier>();
				this.flying = false;
				this.flyingSprite.SetActive(false);
				this.groundedSprite.SetActive(true);
				base.StartCoroutine(this.Timer(this.setTime));
				this.audMan.FlushQueue(true);
				if (other.CompareTag("Player"))
				{
					this.actMod.moveMods.Add(this.playerMod);
					this.canvas.gameObject.SetActive(true);
					this.canvas.worldCamera = Singleton<CoreGameManager>.Instance.GetCamera(other.GetComponent<PlayerManager>().playerNumber).canvasCam;
					Gum.playerGum.Add(this);
					this.beans.HitPlayer();
					this.beans.GumHit(this, false);
					Singleton<CoreGameManager>.Instance.audMan.PlaySingle(this.audSplat);
					return;
				}
				this.actMod.moveMods.Add(this.moveMod);
				this.beans.HitNPC();
				this.beans.GumHit(this, other.transform == this.beans.transform);
				this.audMan.PlaySingle(this.audSplat);
				return;
			}
			else if (!other.isTrigger)
			{
				int layer = other.gameObject.layer;
			}
		}
	}

	// Token: 0x060000D2 RID: 210 RVA: 0x00005F60 File Offset: 0x00004160
	private void OnEntityMoveCollision(RaycastHit hit)
	{
		if (this.flying && hit.transform.gameObject.layer != 2)
		{
			this.flying = false;
			this.entity.SetFrozen(true);
			this.actMod = null;
			base.transform.rotation = Quaternion.LookRotation(hit.normal * -1f, Vector3.up);
			this.beans.GumHit(this, false);
			this.audMan.FlushQueue(true);
			this.audMan.PlaySingle(this.audSplat);
			this.done = true;
			this.hitCollider = hit.collider;
			this.hitColliderPosition = this.hitCollider.transform.position;
			this.hitColliderRotation = this.hitCollider.transform.rotation;
		}
	}

	// Token: 0x060000D3 RID: 211 RVA: 0x00006039 File Offset: 0x00004239
	public void EntityTriggerStay(Collider other)
	{
	}

	// Token: 0x060000D4 RID: 212 RVA: 0x0000603B File Offset: 0x0000423B
	public void EntityTriggerExit(Collider other)
	{
		if (other.transform == this.beans.transform)
		{
			this.leftBeans = true;
		}
	}

	// Token: 0x060000D5 RID: 213 RVA: 0x0000605C File Offset: 0x0000425C
	private IEnumerator Timer(float time)
	{
		while (time > 0f && !this.cut)
		{
			time -= Time.deltaTime * this.ec.EnvironmentTimeScale;
			yield return null;
		}
		this.cut = false;
		this.actMod.moveMods.Remove(this.moveMod);
		this.actMod.moveMods.Remove(this.playerMod);
		Gum.playerGum.Remove(this);
		this.Hide();
		yield break;
	}

	// Token: 0x060000D6 RID: 214 RVA: 0x00006074 File Offset: 0x00004274
	public void Reset()
	{
		this.hidden = false;
		this.done = false;
		base.StopAllCoroutines();
		this.flyingSprite.SetActive(true);
		this.groundedSprite.SetActive(false);
		this.flying = true;
		this.canvas.gameObject.SetActive(false);
		this.leftBeans = false;
		if (this.actMod != null)
		{
			this.actMod.moveMods.Remove(this.moveMod);
			this.actMod.moveMods.Remove(this.playerMod);
		}
		Gum.playerGum.Remove(this);
		this.entity.SetFrozen(false);
		this.entity.SetActive(true);
		this.entity.SetHeight(5f);
		this.audMan.QueueAudio(this.audWhoosh);
		this.audMan.SetLoop(true);
	}

	// Token: 0x060000D7 RID: 215 RVA: 0x0000615C File Offset: 0x0000435C
	public void Hide()
	{
		this.hidden = true;
		this.done = false;
		this.entity.SetActive(false);
		this.entity.UpdateInternalMovement(Vector3.zero);
		this.flyingSprite.SetActive(false);
		this.groundedSprite.SetActive(false);
		this.flying = false;
		this.canvas.gameObject.SetActive(false);
	}

	// Token: 0x060000D8 RID: 216 RVA: 0x000061C3 File Offset: 0x000043C3
	public void Cut()
	{
		this.cut = true;
	}

	// Token: 0x17000011 RID: 17
	// (get) Token: 0x060000D9 RID: 217 RVA: 0x000061CC File Offset: 0x000043CC
	public bool Hidden
	{
		get
		{
			return this.hidden;
		}
	}

	// Token: 0x060000DA RID: 218 RVA: 0x000061D4 File Offset: 0x000043D4
	private void OnDisable()
	{
		Gum.playerGum.Remove(this);
	}

	// Token: 0x040000D9 RID: 217
	public static List<Gum> playerGum = new List<Gum>();

	// Token: 0x040000DA RID: 218
	public EnvironmentController ec;

	// Token: 0x040000DB RID: 219
	public Beans beans;

	// Token: 0x040000DC RID: 220
	[SerializeField]
	private Entity entity;

	// Token: 0x040000DD RID: 221
	[SerializeField]
	private GameObject flyingSprite;

	// Token: 0x040000DE RID: 222
	[SerializeField]
	private GameObject groundedSprite;

	// Token: 0x040000DF RID: 223
	private Collider hitCollider;

	// Token: 0x040000E0 RID: 224
	[SerializeField]
	private AudioManager audMan;

	// Token: 0x040000E1 RID: 225
	[SerializeField]
	private SoundObject audWhoosh;

	// Token: 0x040000E2 RID: 226
	[SerializeField]
	private SoundObject audSplat;

	// Token: 0x040000E3 RID: 227
	private Vector3 hitColliderPosition;

	// Token: 0x040000E4 RID: 228
	private Quaternion hitColliderRotation;

	// Token: 0x040000E5 RID: 229
	[SerializeField]
	private float speed = 10f;

	// Token: 0x040000E6 RID: 230
	[SerializeField]
	private float setTime = 10f;

	// Token: 0x040000E7 RID: 231
	[SerializeField]
	private MovementModifier moveMod = new MovementModifier(Vector3.zero, 0.1f);

	// Token: 0x040000E8 RID: 232
	[SerializeField]
	private MovementModifier playerMod = new MovementModifier(Vector3.zero, 0.25f);

	// Token: 0x040000E9 RID: 233
	[SerializeField]
	private Canvas canvas;

	// Token: 0x040000EA RID: 234
	private ActivityModifier actMod;

	// Token: 0x040000EB RID: 235
	private bool hidden = true;

	// Token: 0x040000EC RID: 236
	private bool cut;

	// Token: 0x040000ED RID: 237
	private bool leftBeans;

	// Token: 0x040000EE RID: 238
	private bool flying;

	// Token: 0x040000EF RID: 239
	private bool done;
}
