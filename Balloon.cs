using System;
using UnityEngine;

// Token: 0x020001BA RID: 442
public class Balloon : MonoBehaviour, IEntityTrigger
{
	// Token: 0x060009E5 RID: 2533 RVA: 0x000353C7 File Offset: 0x000335C7
	private void Start()
	{
		this.ChangeDirection();
		this.directionTime = Random.Range(this.minDirectionTime, this.maxDirectionTime);
		this.entity.OnEntityMoveInitialCollision += this.OnEntityMoveCollision;
	}

	// Token: 0x060009E6 RID: 2534 RVA: 0x00035400 File Offset: 0x00033600
	private void Update()
	{
		this.directionTime -= Time.deltaTime;
		if (this.directionTime <= 0f || (this.changeWhenSlow && this.entity.Velocity.magnitude < this.minSpeed))
		{
			this.ChangeDirection();
			this.directionTime = Random.Range(this.minDirectionTime, this.maxDirectionTime);
		}
		this.entity.UpdateInternalMovement(this.direction * this.speed * this.ec.EnvironmentTimeScale);
	}

	// Token: 0x060009E7 RID: 2535 RVA: 0x00035498 File Offset: 0x00033698
	public void Initialize(RoomController rc)
	{
		this.ec = rc.ec;
		base.transform.position = rc.RandomEventSafeCellNoGarbage().CenterWorldPosition;
		this.entity.Initialize(this.ec, base.transform.position);
	}

	// Token: 0x060009E8 RID: 2536 RVA: 0x000354D8 File Offset: 0x000336D8
	public void EntityTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 13 || other.gameObject.layer == 18)
		{
			this.direction = Vector3.Reflect(this.direction, (base.transform.position - other.transform.position).normalized);
			this.directionTime = Random.Range(this.minDirectionTime, this.maxDirectionTime);
		}
	}

	// Token: 0x060009E9 RID: 2537 RVA: 0x0003554E File Offset: 0x0003374E
	public void EntityTriggerStay(Collider other)
	{
	}

	// Token: 0x060009EA RID: 2538 RVA: 0x00035550 File Offset: 0x00033750
	public void EntityTriggerExit(Collider other)
	{
	}

	// Token: 0x060009EB RID: 2539 RVA: 0x00035552 File Offset: 0x00033752
	private void OnEntityMoveCollision(RaycastHit hit)
	{
		this.direction = Vector3.Reflect(this.direction, hit.normal);
		Random.Range(this.minDirectionTime, this.maxDirectionTime);
	}

	// Token: 0x060009EC RID: 2540 RVA: 0x00035580 File Offset: 0x00033780
	private void ChangeDirection()
	{
		this.direction = Random.insideUnitCircle;
		this.direction.z = this.direction.y;
		this.direction.y = 0f;
		this.direction = this.direction.normalized;
	}

	// Token: 0x060009ED RID: 2541 RVA: 0x000355D4 File Offset: 0x000337D4
	public void Stop()
	{
		this.speed = 0f;
		base.enabled = false;
	}

	// Token: 0x170000D0 RID: 208
	// (get) Token: 0x060009EE RID: 2542 RVA: 0x000355E8 File Offset: 0x000337E8
	public Entity Entity
	{
		get
		{
			return this.entity;
		}
	}

	// Token: 0x04000B44 RID: 2884
	private EnvironmentController ec;

	// Token: 0x04000B45 RID: 2885
	[SerializeField]
	private Entity entity;

	// Token: 0x04000B46 RID: 2886
	[SerializeField]
	private float radius = 1f;

	// Token: 0x04000B47 RID: 2887
	[SerializeField]
	private float minDirectionTime = 2.5f;

	// Token: 0x04000B48 RID: 2888
	[SerializeField]
	private float maxDirectionTime = 10f;

	// Token: 0x04000B49 RID: 2889
	private float directionTime;

	// Token: 0x04000B4A RID: 2890
	private Vector3 direction;

	// Token: 0x04000B4B RID: 2891
	private Vector3 _position;

	// Token: 0x04000B4C RID: 2892
	public float speed = 10f;

	// Token: 0x04000B4D RID: 2893
	public float minSpeed = 1f;

	// Token: 0x04000B4E RID: 2894
	[SerializeField]
	private bool changeWhenSlow;
}
