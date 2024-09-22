using System;
using UnityEngine;

// Token: 0x0200008B RID: 139
public class ITM_NanaPeel : Item, IEntityTrigger
{
	// Token: 0x06000335 RID: 821 RVA: 0x00010B10 File Offset: 0x0000ED10
	public override bool Use(PlayerManager pm)
	{
		if (!this.spawned)
		{
			this.Spawn(pm.ec, pm.transform.position, Singleton<CoreGameManager>.Instance.GetCamera(pm.playerNumber).transform.forward, this.throwSpeed);
			this.audioManager.PlaySingle(this.audEnd);
			return true;
		}
		return false;
	}

	// Token: 0x06000336 RID: 822 RVA: 0x00010B70 File Offset: 0x0000ED70
	public void Spawn(EnvironmentController ec, Vector3 position, Vector3 forward, float throwSpeed)
	{
		this.ec = ec;
		base.transform.position = position;
		base.transform.forward = forward;
		this.direction = base.transform.forward;
		this.entity.Initialize(ec, base.transform.position);
		this.height = this.startHeight;
		this.entity.OnEntityMoveInitialCollision += this.OnEntityMoveCollision;
		this.force = new Force(this.direction, throwSpeed, -throwSpeed);
		this.entity.AddForce(this.force);
	}

	// Token: 0x06000337 RID: 823 RVA: 0x00010C10 File Offset: 0x0000EE10
	private void Update()
	{
		if (!this.ready && !this.slipping && !this.dying)
		{
			this.height -= this.gravity * Time.deltaTime * this.ec.EnvironmentTimeScale;
			this.entity.UpdateInternalMovement(Vector3.zero);
			if (this.height <= this.endHeight)
			{
				this.height = this.endHeight;
				this.ready = true;
				this.entity.SetGrounded(true);
				this.audioManager.PlaySingle(this.audSplat);
				this.time = this.maxTime;
			}
			this.entity.SetHeight(this.height);
		}
		else if (this.slipping)
		{
			this.entity.UpdateInternalMovement(this.direction * this.speed * this.ec.EnvironmentTimeScale);
			this.moveMod.movementAddend = this.entity.ExternalActivity.Addend + this.direction * this.speed * this.ec.EnvironmentTimeScale;
			this.time -= Time.deltaTime * this.ec.EnvironmentTimeScale;
		}
		else
		{
			this.entity.UpdateInternalMovement(Vector3.zero);
		}
		if (this.dying && this.force.Dead)
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06000338 RID: 824 RVA: 0x00010D9B File Offset: 0x0000EF9B
	public void EntityTriggerEnter(Collider other)
	{
	}

	// Token: 0x06000339 RID: 825 RVA: 0x00010DA0 File Offset: 0x0000EFA0
	public void EntityTriggerStay(Collider other)
	{
		if (this.ready && !this.slipping)
		{
			Entity component = other.GetComponent<Entity>();
			if (component != null && component.Grounded && component.Velocity.magnitude > 0f)
			{
				this.entity.Teleport(component.transform.position);
				component.ExternalActivity.moveMods.Add(this.moveMod);
				this.slippingEntitity = component;
				this.slipping = true;
				this.ready = false;
				this.direction = component.Velocity.normalized;
				this.audioManager.FlushQueue(true);
				this.audioManager.QueueAudio(this.audSlipping);
				this.audioManager.SetLoop(true);
				if (!this.force.Dead)
				{
					this.entity.RemoveForce(this.force);
				}
			}
		}
	}

	// Token: 0x0600033A RID: 826 RVA: 0x00010E95 File Offset: 0x0000F095
	public void EntityTriggerExit(Collider other)
	{
		if (this.slipping && other.transform == this.slippingEntitity.transform)
		{
			this.End();
		}
	}

	// Token: 0x0600033B RID: 827 RVA: 0x00010EC0 File Offset: 0x0000F0C0
	private void OnEntityMoveCollision(RaycastHit hit)
	{
		if (this.slipping && (Vector3.Angle(-this.direction, hit.normal) <= this.endAngle || this.time < 0f))
		{
			this.direction = Vector3.Reflect(this.direction, hit.normal);
			this.End();
			return;
		}
		if (this.slipping)
		{
			this.direction = Vector3.Reflect(this.direction, hit.normal);
			this.audioManager.PlaySingle(this.audSplat);
		}
	}

	// Token: 0x0600033C RID: 828 RVA: 0x00010F54 File Offset: 0x0000F154
	private void End()
	{
		this.slippingEntitity.ExternalActivity.moveMods.Remove(this.moveMod);
		this.slipping = false;
		this.slippingEntitity = null;
		this.dying = true;
		this.force = new Force(this.direction, this.speed, -this.speed);
		this.entity.AddForce(this.force);
		this.audioManager.FlushQueue(true);
		this.audioManager.PlaySingle(this.audEnd);
	}

	// Token: 0x0600033D RID: 829 RVA: 0x00010FDE File Offset: 0x0000F1DE
	private void OnDestroy()
	{
		if (this.slippingEntitity != null)
		{
			this.slippingEntitity.ExternalActivity.moveMods.Remove(this.moveMod);
		}
	}

	// Token: 0x04000371 RID: 881
	private EnvironmentController ec;

	// Token: 0x04000372 RID: 882
	[SerializeField]
	private Entity entity;

	// Token: 0x04000373 RID: 883
	private Entity slippingEntitity;

	// Token: 0x04000374 RID: 884
	[SerializeField]
	private MovementModifier moveMod;

	// Token: 0x04000375 RID: 885
	private Force force;

	// Token: 0x04000376 RID: 886
	[SerializeField]
	private AudioManager audioManager;

	// Token: 0x04000377 RID: 887
	[SerializeField]
	private SoundObject audSplat;

	// Token: 0x04000378 RID: 888
	[SerializeField]
	private SoundObject audSlipping;

	// Token: 0x04000379 RID: 889
	[SerializeField]
	private SoundObject audEnd;

	// Token: 0x0400037A RID: 890
	private Vector3 direction;

	// Token: 0x0400037B RID: 891
	[SerializeField]
	private float speed = 30f;

	// Token: 0x0400037C RID: 892
	[SerializeField]
	private float endAngle = 30f;

	// Token: 0x0400037D RID: 893
	[SerializeField]
	private float throwSpeed = 5f;

	// Token: 0x0400037E RID: 894
	[SerializeField]
	private float gravity = 5f;

	// Token: 0x0400037F RID: 895
	[SerializeField]
	private float startHeight;

	// Token: 0x04000380 RID: 896
	[SerializeField]
	private float endHeight = -4f;

	// Token: 0x04000381 RID: 897
	[SerializeField]
	private float maxTime = 30f;

	// Token: 0x04000382 RID: 898
	private float height;

	// Token: 0x04000383 RID: 899
	private float time;

	// Token: 0x04000384 RID: 900
	private bool ready;

	// Token: 0x04000385 RID: 901
	private bool slipping;

	// Token: 0x04000386 RID: 902
	private bool dying;

	// Token: 0x04000387 RID: 903
	private bool spawned;
}
