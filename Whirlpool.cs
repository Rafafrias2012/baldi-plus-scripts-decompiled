using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200006C RID: 108
public class Whirlpool : MonoBehaviour
{
	// Token: 0x0600025D RID: 605 RVA: 0x0000D11E File Offset: 0x0000B31E
	private void Start()
	{
		base.StartCoroutine(this.Form());
		this.timeLeft = this.timeUntilItemRemoved;
	}

	// Token: 0x0600025E RID: 606 RVA: 0x0000D13C File Offset: 0x0000B33C
	private void Update()
	{
		base.transform.Rotate(Vector3.up, this.rotationSpeed * Time.deltaTime * this.ec.EnvironmentTimeScale);
		for (int i = 0; i < this.entities.Count; i++)
		{
			if (this.entities[i] == null)
			{
				this.entities.RemoveAt(i);
				this.moveMods.RemoveAt(i);
				i--;
			}
			else
			{
				this._distance = Vector3.Distance(base.transform.position, this.entities[i].transform.position);
				this.ray = new Ray(this.entities[i].transform.position, base.transform.position - this.entities[i].transform.position);
				this.hits = Physics.RaycastAll(this.ray, this._distance, this.layerMask, QueryTriggerInteraction.Ignore);
				this.hitTransforms.Clear();
				this._obstacleHit = false;
				if (this.hits.Length != 0)
				{
					this._obstacleHit = true;
				}
				if (!this._obstacleHit)
				{
					this._direction = base.transform.position + base.transform.forward * this.centerForceOffset - this.entities[i].transform.position;
					this._direction.y = 0f;
					this._distance = Vector3.Distance(base.transform.position + base.transform.forward * this.centerForceOffset + Vector3.up * 5f, this.entities[i].transform.position);
					this._force = (this.sphereCollider.radius - Mathf.Min(this._distance, this.sphereCollider.radius)) / this.sphereCollider.radius * this.maxForce * this.ec.EnvironmentTimeScale;
					if (this._force * Time.deltaTime > this._distance)
					{
						this.moveMods[i].movementAddend = this._direction.normalized * this._distance / Time.deltaTime;
					}
					else
					{
						this.moveMods[i].movementAddend = this._direction.normalized * this._force;
					}
					if (this._distance <= 5f && this.entities[i].Grounded && this.entities[i].Override(this.entityOverrider))
					{
						base.gameObject.layer = 19;
						base.StartCoroutine(this.Teleport(this.entities[i], this.entities[i].transform.CompareTag("Player")));
						base.StartCoroutine(this.Close());
					}
				}
				else
				{
					this.moveMods[i].movementAddend = Vector3.zero;
				}
			}
		}
	}

	// Token: 0x0600025F RID: 607 RVA: 0x0000D490 File Offset: 0x0000B690
	private void OnTriggerEnter(Collider other)
	{
		if (other.isTrigger)
		{
			Entity component = other.GetComponent<Entity>();
			if (component != null)
			{
				this.entities.Add(component);
				MovementModifier movementModifier = new MovementModifier(Vector3.zero, 1f);
				movementModifier.ignoreAirborne = true;
				this.moveMods.Add(movementModifier);
				component.ExternalActivity.moveMods.Add(movementModifier);
			}
		}
	}

	// Token: 0x06000260 RID: 608 RVA: 0x0000D4F8 File Offset: 0x0000B6F8
	private void OnTriggerExit(Collider other)
	{
		if (other.isTrigger)
		{
			Entity component = other.GetComponent<Entity>();
			if (component != null)
			{
				for (int i = 0; i < this.entities.Count; i++)
				{
					if (component == this.entities[i])
					{
						this.entities.RemoveAt(i);
						component.ExternalActivity.moveMods.Remove(this.moveMods[i]);
						this.moveMods.RemoveAt(i);
						return;
					}
				}
			}
		}
	}

	// Token: 0x06000261 RID: 609 RVA: 0x0000D580 File Offset: 0x0000B780
	public void ClearMoveMods()
	{
		foreach (Entity entity in this.entities)
		{
			ActivityModifier component = entity.GetComponent<ActivityModifier>();
			foreach (MovementModifier item in this.moveMods)
			{
				component.moveMods.Remove(item);
			}
		}
	}

	// Token: 0x06000262 RID: 610 RVA: 0x0000D61C File Offset: 0x0000B81C
	private IEnumerator Form()
	{
		Vector3 scale = base.transform.localScale;
		scale.x = 0f;
		scale.z = 0f;
		float time = 0f;
		while (time < this.formTime)
		{
			time += Time.deltaTime * this.ec.EnvironmentTimeScale;
			scale.x = time / this.formTime;
			scale.z = time / this.formTime;
			base.transform.localScale = scale;
			this.audMan.volumeModifier = time / this.formTime;
			yield return null;
		}
		base.transform.localScale = Vector3.one;
		this.sphereCollider.enabled = true;
		this.audMan.volumeModifier = 1f;
		yield break;
	}

	// Token: 0x06000263 RID: 611 RVA: 0x0000D62B File Offset: 0x0000B82B
	public void StartClose()
	{
		base.StopAllCoroutines();
		base.StartCoroutine(this.Close());
	}

	// Token: 0x06000264 RID: 612 RVA: 0x0000D640 File Offset: 0x0000B840
	private IEnumerator Close()
	{
		base.gameObject.layer = 19;
		Vector3 scale = base.transform.localScale;
		float time = this.formTime;
		while (time > 0f)
		{
			time -= Time.deltaTime * this.ec.EnvironmentTimeScale;
			scale.x = time / this.formTime;
			scale.z = time / this.formTime;
			base.transform.localScale = scale;
			this.audMan.volumeModifier = time / this.formTime;
			yield return null;
		}
		base.transform.localScale = Vector3.zero;
		this.audMan.volumeModifier = 0f;
		this.ClearMoveMods();
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06000265 RID: 613 RVA: 0x0000D64F File Offset: 0x0000B84F
	private IEnumerator Teleport(Entity subject, bool player)
	{
		Vector3 newPos = Vector3.zero;
		this.subject = subject;
		this.entityOverrider.SetFrozen(true);
		this.entityOverrider.SetInteractionState(false);
		List<Cell> list = this.ec.AllTilesNoGarbage(false, true);
		list.ConvertEntityUnsafeCells();
		int index = Random.Range(0, list.Count);
		newPos = list[index].FloorWorldPosition;
		Whirlpool newPool = Object.Instantiate<Whirlpool>(this.whirlpoolPre);
		newPool.ec = this.ec;
		newPool.FormTime = 3f;
		newPool.transform.position = newPos;
		newPool.whirlpoolPre = this.whirlpoolPre;
		newPool.flood = this.flood;
		newPool.gameObject.layer = 19;
		this.flood.whirlpools.Add(newPool);
		float sinkPercent = 1f;
		if (subject != null)
		{
			subject.Teleport(base.transform.position);
		}
		while (sinkPercent > 0.2f)
		{
			if (subject == null)
			{
				yield break;
			}
			sinkPercent -= Time.deltaTime * this.ec.EnvironmentTimeScale * this.sinkSpeed;
			this.entityOverrider.SetHeight(subject.InternalHeight * sinkPercent);
			yield return null;
		}
		if (subject == null)
		{
			yield break;
		}
		sinkPercent = 0.2f;
		this.entityOverrider.SetHeight(subject.InternalHeight * sinkPercent);
		if (subject != null)
		{
			subject.Teleport(newPos);
		}
		while (sinkPercent < 1f)
		{
			if (subject == null)
			{
				yield break;
			}
			sinkPercent += Time.deltaTime * this.ec.EnvironmentTimeScale * this.sinkSpeed;
			this.entityOverrider.SetHeight(subject.InternalHeight * sinkPercent);
			yield return null;
		}
		this.entityOverrider.Release();
		this.entityOverrider.SetFrozen(false);
		this.entityOverrider.SetInteractionState(true);
		this.subject = null;
		newPool.StartClose();
		yield break;
	}

	// Token: 0x06000266 RID: 614 RVA: 0x0000D665 File Offset: 0x0000B865
	private void OnDestroy()
	{
		this.entityOverrider.Release();
		this.entityOverrider.SetFrozen(false);
		this.entityOverrider.SetInteractionState(true);
	}

	// Token: 0x17000023 RID: 35
	// (set) Token: 0x06000267 RID: 615 RVA: 0x0000D68A File Offset: 0x0000B88A
	public float FormTime
	{
		set
		{
			this.formTime = value;
		}
	}

	// Token: 0x0400026B RID: 619
	public EnvironmentController ec;

	// Token: 0x0400026C RID: 620
	private List<MovementModifier> moveMods = new List<MovementModifier>();

	// Token: 0x0400026D RID: 621
	private List<Entity> entities = new List<Entity>();

	// Token: 0x0400026E RID: 622
	public Whirlpool whirlpoolPre;

	// Token: 0x0400026F RID: 623
	public FloodEvent flood;

	// Token: 0x04000270 RID: 624
	[SerializeField]
	private AudioManager audMan;

	// Token: 0x04000271 RID: 625
	[SerializeField]
	private SphereCollider sphereCollider;

	// Token: 0x04000272 RID: 626
	[SerializeField]
	private LayerMask layerMask;

	// Token: 0x04000273 RID: 627
	[SerializeField]
	private float rotationSpeed = 90f;

	// Token: 0x04000274 RID: 628
	[SerializeField]
	private float centerForceOffset = 1f;

	// Token: 0x04000275 RID: 629
	[SerializeField]
	private float maxForce = 20f;

	// Token: 0x04000276 RID: 630
	[SerializeField]
	private float formTime = 5f;

	// Token: 0x04000277 RID: 631
	[SerializeField]
	private float timeUntilItemRemoved = 5f;

	// Token: 0x04000278 RID: 632
	[SerializeField]
	private float sinkSpeed = 2f;

	// Token: 0x04000279 RID: 633
	private float _force;

	// Token: 0x0400027A RID: 634
	private Ray ray;

	// Token: 0x0400027B RID: 635
	private RaycastHit[] hits;

	// Token: 0x0400027C RID: 636
	private List<Transform> hitTransforms = new List<Transform>();

	// Token: 0x0400027D RID: 637
	private EntityOverrider entityOverrider = new EntityOverrider();

	// Token: 0x0400027E RID: 638
	private Entity subject;

	// Token: 0x0400027F RID: 639
	private Vector3 _direction;

	// Token: 0x04000280 RID: 640
	public float _distance;

	// Token: 0x04000281 RID: 641
	public float timeLeft;

	// Token: 0x04000282 RID: 642
	private bool _obstacleHit;

	// Token: 0x04000283 RID: 643
	private bool teleporting;
}
