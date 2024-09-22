using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000DE RID: 222
public class Entity : MonoBehaviour
{
	// Token: 0x14000003 RID: 3
	// (add) Token: 0x0600050B RID: 1291 RVA: 0x0001A028 File Offset: 0x00018228
	// (remove) Token: 0x0600050C RID: 1292 RVA: 0x0001A060 File Offset: 0x00018260
	public event Entity.OnTeleportFunction OnTeleport;

	// Token: 0x14000004 RID: 4
	// (add) Token: 0x0600050D RID: 1293 RVA: 0x0001A098 File Offset: 0x00018298
	// (remove) Token: 0x0600050E RID: 1294 RVA: 0x0001A0D0 File Offset: 0x000182D0
	public event Entity.OnEntityMoveCollisionFunction OnEntityMoveInitialCollision;

	// Token: 0x0600050F RID: 1295 RVA: 0x0001A108 File Offset: 0x00018308
	public static void UpdateAllEntities()
	{
		foreach (Entity entity in Entity.allEntities)
		{
			entity.EntityUpdate();
		}
	}

	// Token: 0x06000510 RID: 1296 RVA: 0x0001A158 File Offset: 0x00018358
	public static void UpdateAllEntityStates()
	{
		foreach (Entity entity in Entity.allEntities)
		{
			entity.UpdateState();
		}
	}

	// Token: 0x06000511 RID: 1297 RVA: 0x0001A1A8 File Offset: 0x000183A8
	public void UpdateState()
	{
		if (this.culled || !this.active)
		{
			base.gameObject.SetActive(false);
			return;
		}
		base.gameObject.SetActive(true);
	}

	// Token: 0x06000512 RID: 1298 RVA: 0x0001A1D4 File Offset: 0x000183D4
	public static void UpdateAllEntityTriggerStates()
	{
		foreach (Entity entity in Entity.allEntities)
		{
			entity.UpdateTriggerStateForFrame();
		}
	}

	// Token: 0x06000513 RID: 1299 RVA: 0x0001A224 File Offset: 0x00018424
	private void UpdateTriggerStateForFrame()
	{
		this.triggerEnabledThisFrame = this.enableTriggerNextPhysicsUpdate;
		this.triggerDisabledThisFrame = this.disableTriggerNextPhysicsUpdate;
		this.enableTriggerNextPhysicsUpdate = false;
		this.disableTriggerNextPhysicsUpdate = false;
	}

	// Token: 0x06000514 RID: 1300 RVA: 0x0001A24C File Offset: 0x0001844C
	private void UpdateTriggerState()
	{
		if (this.Squished || this.triggerOff)
		{
			if (this.triggerEnabled)
			{
				this.disableTriggerNextPhysicsUpdate = true;
				this.enableTriggerNextPhysicsUpdate = false;
			}
			this.triggerEnabled = false;
			return;
		}
		if (!this.triggerEnabled)
		{
			this.enableTriggerNextPhysicsUpdate = true;
			this.disableTriggerNextPhysicsUpdate = false;
		}
		this.triggerEnabled = true;
	}

	// Token: 0x06000515 RID: 1301 RVA: 0x0001A2A4 File Offset: 0x000184A4
	private void Awake()
	{
		this.transform = base.transform;
		this.defaultLayer = base.gameObject.layer;
		this.externalActivity.Initialize(this);
		if (this.rendererBase != null)
		{
			this.rendererBaseInitialPosition = this.rendererBase.localPosition;
		}
		if (this.collider.GetType() == typeof(CapsuleCollider))
		{
			foreach (CapsuleCollider capsuleCollider in base.GetComponents<CapsuleCollider>())
			{
				if (!capsuleCollider.isTrigger)
				{
					this.colliderRadius = capsuleCollider.radius;
					break;
				}
			}
			if (this.colliderRadius == 0f)
			{
				Debug.LogWarning(string.Format("Entity {0} did not have a valid collider radius. It will be defaulted to 0.5", this.transform.name));
				this.colliderRadius = 0.5f;
			}
		}
		else if (this.collider.GetType() == typeof(CharacterController))
		{
			this.colliderRadius = base.GetComponent<CharacterController>().radius;
		}
		else
		{
			Debug.LogWarning(string.Format("Entity {0} did not have a valid collider radius. It will be defaulted to 0.5", this.transform.name));
			this.colliderRadius = 0.5f;
		}
		if (this.trigger != null)
		{
			Physics.IgnoreCollision(this.collider, this.trigger, true);
		}
		foreach (Entity entity in Entity.allEntities)
		{
			if (!this.keepCharacterControllerCollision || !entity.keepCharacterControllerCollision)
			{
				Physics.IgnoreCollision(this.collider, entity.collider, true);
			}
			Physics.IgnoreCollision(this.collider, entity.trigger, true);
			Physics.IgnoreCollision(this.trigger, entity.collider, true);
		}
		this.iEntityTrigger = base.GetComponentsInChildren<IEntityTrigger>();
		Entity.allEntities.Add(this);
	}

	// Token: 0x06000516 RID: 1302 RVA: 0x0001A48C File Offset: 0x0001868C
	private void OnDestroy()
	{
		Entity.allEntities.Remove(this);
	}

	// Token: 0x06000517 RID: 1303 RVA: 0x0001A49C File Offset: 0x0001869C
	private void OnTriggerEnter(Collider other)
	{
		if (this.triggerEnabled)
		{
			for (int i = 0; i < this.iEntityTrigger.Length; i++)
			{
				this.iEntityTrigger[i].EntityTriggerEnter(other);
			}
		}
	}

	// Token: 0x06000518 RID: 1304 RVA: 0x0001A4D4 File Offset: 0x000186D4
	private void OnTriggerStay(Collider other)
	{
		if (this.triggerEnabledThisFrame)
		{
			for (int i = 0; i < this.iEntityTrigger.Length; i++)
			{
				this.iEntityTrigger[i].EntityTriggerEnter(other);
			}
		}
		if (this.triggerEnabled)
		{
			for (int j = 0; j < this.iEntityTrigger.Length; j++)
			{
				this.iEntityTrigger[j].EntityTriggerStay(other);
			}
		}
		if (this.triggerDisabledThisFrame)
		{
			for (int k = 0; k < this.iEntityTrigger.Length; k++)
			{
				this.iEntityTrigger[k].EntityTriggerExit(other);
			}
		}
	}

	// Token: 0x06000519 RID: 1305 RVA: 0x0001A55C File Offset: 0x0001875C
	private void OnTriggerExit(Collider other)
	{
		if (this.triggerEnabled || this.triggerDisabledThisFrame)
		{
			for (int i = 0; i < this.iEntityTrigger.Length; i++)
			{
				this.iEntityTrigger[i].EntityTriggerExit(other);
			}
		}
	}

	// Token: 0x0600051A RID: 1306 RVA: 0x0001A59A File Offset: 0x0001879A
	public void Initialize(EnvironmentController environmentController, Vector3 position)
	{
		this.environmentController = environmentController;
		this.transform.position = position;
		this.height = position.y;
	}

	// Token: 0x0600051B RID: 1307 RVA: 0x0001A5BC File Offset: 0x000187BC
	private void Update()
	{
		if (this.environmentController.CellFromPosition(this.transform.position).room != this.currentRoom)
		{
			if (this.currentRoom != null)
			{
				this.currentRoom.functions.OnEntityExit(this);
			}
			this.currentRoom = this.environmentController.CellFromPosition(this.transform.position).room;
			if (this.currentRoom != null)
			{
				this.currentRoom.functions.OnEntityEnter(this);
			}
		}
		if (this.currentRoom != null)
		{
			this.currentRoom.functions.OnEntityStay(this);
		}
		if (this.squished)
		{
			this.squishTime -= Time.deltaTime * this.environmentController.EnvironmentTimeScale;
			if (this.squishTime <= 0f)
			{
				this.Unsquish();
			}
		}
	}

	// Token: 0x0600051C RID: 1308 RVA: 0x0001A6A8 File Offset: 0x000188A8
	public void UpdateInternalMovement(Vector3 movement)
	{
		this.internalMovementVector = movement;
	}

	// Token: 0x0600051D RID: 1309 RVA: 0x0001A6B1 File Offset: 0x000188B1
	public void AddForce(Force force)
	{
		this.forces.Add(force);
	}

	// Token: 0x0600051E RID: 1310 RVA: 0x0001A6BF File Offset: 0x000188BF
	public void RemoveForce(Force force)
	{
		this.forces.Remove(force);
	}

	// Token: 0x0600051F RID: 1311 RVA: 0x0001A6CE File Offset: 0x000188CE
	public void SetActive(bool value)
	{
		if (this.active != value)
		{
			this.active = value;
			this.UpdateLayer();
		}
		if (value)
		{
			this.UpdateState();
		}
	}

	// Token: 0x06000520 RID: 1312 RVA: 0x0001A6EF File Offset: 0x000188EF
	private void SetCulled(bool value)
	{
		if (this.culled != value)
		{
			this.culled = value;
			this.UpdateLayer();
		}
	}

	// Token: 0x06000521 RID: 1313 RVA: 0x0001A707 File Offset: 0x00018907
	public void SetFrozen(bool value)
	{
		if (value)
		{
			this.freezes++;
			this.frozen = true;
			return;
		}
		this.freezes--;
		if (this.freezes <= 0)
		{
			this.frozen = false;
			this.freezes = 0;
		}
	}

	// Token: 0x06000522 RID: 1314 RVA: 0x0001A748 File Offset: 0x00018948
	public void SetTrigger(bool value)
	{
		if (!value)
		{
			this.triggerOffs++;
			this.triggerOff = true;
		}
		else
		{
			this.triggerOffs--;
			if (this.triggerOffs <= 0)
			{
				this.triggerOff = false;
				this.triggerOffs = 0;
			}
		}
		this.UpdateTriggerState();
	}

	// Token: 0x06000523 RID: 1315 RVA: 0x0001A79A File Offset: 0x0001899A
	public void SetIgnoreAddend(bool value)
	{
		if (value)
		{
			this.addendIgnores++;
			this.ignoreAddend = true;
			return;
		}
		this.addendIgnores--;
		if (this.addendIgnores <= 0)
		{
			this.ignoreAddend = false;
			this.addendIgnores = 0;
		}
	}

	// Token: 0x06000524 RID: 1316 RVA: 0x0001A7DA File Offset: 0x000189DA
	public void SetInteractionState(bool value)
	{
		if (!value)
		{
			this.interactionDisables++;
		}
		else if (this.interactionDisables > 0)
		{
			this.interactionDisables--;
		}
		this.UpdateLayer();
	}

	// Token: 0x06000525 RID: 1317 RVA: 0x0001A80C File Offset: 0x00018A0C
	public void SetGrounded(bool value)
	{
		this.grounded = value;
	}

	// Token: 0x06000526 RID: 1318 RVA: 0x0001A818 File Offset: 0x00018A18
	public void UpdateLayer()
	{
		if (base.gameObject != null)
		{
			if (this.culled || !this.active || this.InteractionDisabled)
			{
				base.gameObject.layer = 19;
				return;
			}
			base.gameObject.layer = this.defaultLayer;
		}
	}

	// Token: 0x06000527 RID: 1319 RVA: 0x0001A86C File Offset: 0x00018A6C
	public virtual void EntityUpdate()
	{
		if (this.environmentController != null)
		{
			this.currentChunk = this.environmentController.CellFromPosition(this.transform.position).Chunk;
			if (!this.persistent && this.currentChunk != null && !this.currentChunk.Rendering)
			{
				this.SetCulled(true);
				return;
			}
			this.SetCulled(false);
			Vector3 vector = default(Vector3);
			Vector3 a = default(Vector3);
			Vector3 vector2 = default(Vector3);
			if (!this.Frozen)
			{
				a += this.internalMovementVector;
				for (int i = 0; i < this.forces.Count; i++)
				{
					vector2 += this.forces[i].VelocityThisFrame(Time.deltaTime);
					if (this.forces[i].Dead)
					{
						this.forces.RemoveAt(i);
						i--;
					}
				}
				a *= this.externalActivity.Multiplier;
				if (!this.ignoreAddend)
				{
					vector = this.externalActivity.Addend;
				}
				a.y = 0f;
				vector2.y = 0f;
				vector.y = 0f;
				Vector3 movement = (a + vector) * Time.deltaTime + vector2;
				float magnitude = movement.magnitude;
				this.expectedForcedPosition = this.transform.position + (vector * Time.deltaTime + vector2);
				if (magnitude > 0.0001f)
				{
					this.MoveWithCollision(movement);
				}
				else
				{
					this._position = this.transform.position;
					this._position.y = Entity.physicalHeight;
					this.transform.position = this._position;
				}
			}
			else
			{
				this.expectedForcedPosition = this.transform.position;
			}
			this.velocity = this.transform.position - this.previousPosition;
			this.relativeToForceVelocity = this.transform.position - this.expectedForcedPosition;
			this.previousPosition = this.transform.position;
		}
	}

	// Token: 0x06000528 RID: 1320 RVA: 0x0001AA94 File Offset: 0x00018C94
	public virtual void MoveWithCollision(Vector3 movement)
	{
		int num = Physics.OverlapSphereNonAlloc(this.transform.position, this.colliderRadius, this.neighbor, this.collisionLayerMask, QueryTriggerInteraction.Ignore);
		for (int i = 0; i < num; i++)
		{
			if (this.neighbor[i] != this.collider)
			{
				Vector3 a = default(Vector3);
				float d;
				if (Physics.ComputePenetration(this.collider, this.transform.position, Quaternion.identity, this.neighbor[i], this.neighbor[i].transform.position, this.neighbor[i].transform.rotation, out a, out d))
				{
					this.transform.position += a * d;
				}
			}
		}
		Vector3 position = this.transform.position;
		Vector2 vector = new Vector2(movement.x, movement.z);
		float num2 = movement.magnitude;
		int num3 = 0;
		float num4 = 0.01f;
		int num5 = 0;
		while (num2 > num4 / 2f && num3 < Entity.maxCollisionChecks)
		{
			num3++;
			int num6 = Physics.SphereCastNonAlloc(position, this.colliderRadius - num4, new Vector3(vector.x, 0f, vector.y).normalized, this._hit, num2 + num4 / 2f, this.collisionLayerMask, QueryTriggerInteraction.Ignore);
			float num7 = num2 + num4 / 2f;
			int num8 = 0;
			bool flag = false;
			for (int j = 0; j < num6; j++)
			{
				if (this._hit[j].point != Vector3.zero && this._hit[j].distance < num7)
				{
					flag = true;
					num7 = this._hit[j].distance;
					num8 = j;
				}
			}
			num7 = Mathf.Max(0f, num7 - num4 / 2f);
			position.x += (vector.normalized * num7).x;
			position.z += (vector.normalized * num7).y;
			if (flag)
			{
				if (num3 == 1 && this.OnEntityMoveInitialCollision != null)
				{
					this.OnEntityMoveInitialCollision(this._hit[num8]);
				}
				Vector3 vector2 = (position - this._hit[num8].point) / this.colliderRadius;
				vector2.y = 0f;
				vector2.Normalize();
				Vector2 vector3 = new Vector2(vector2.x, vector2.z);
				vector = vector.normalized * (num2 - num7);
				float num9 = Vector2.SignedAngle(vector3, new Vector2(movement.x, movement.z));
				bool flag2 = false;
				if (num5 == 0)
				{
					num5 = (int)Mathf.Sign(num9);
				}
				else if (num5 != (int)Mathf.Sign(num9))
				{
					num2 = 0f;
					flag2 = true;
				}
				if (!flag2)
				{
					float num10 = vector.magnitude * Mathf.Sin(num9 * 0.017453292f);
					num2 = Mathf.Abs(num10);
					vector = Vector2.Perpendicular(vector3).normalized * num10;
					if (num3 == Entity.maxCollisionChecks)
					{
						Debug.LogWarning(string.Format("Entity {0} has reached the maximum number of physics calcilations this frame. An object was hit.", this.transform.name), this.transform);
					}
				}
			}
			else
			{
				num2 = 0f;
				if (num3 == Entity.maxCollisionChecks)
				{
					Debug.LogWarning(string.Format("Entity {0} has reached the maximum number of physics calcilations this frame. No object was hit.", this.transform.name), this.transform);
				}
			}
		}
		this._position = position;
		this._position.y = Entity.physicalHeight;
		this.transform.position = this._position;
	}

	// Token: 0x06000529 RID: 1321 RVA: 0x0001AE60 File Offset: 0x00019060
	public bool Override(EntityOverrider overrider)
	{
		if (!this.overridden && !overrider.active)
		{
			this.overrider = overrider;
			this.overridden = true;
			overrider.Override(this);
			return true;
		}
		return false;
	}

	// Token: 0x0600052A RID: 1322 RVA: 0x0001AE8A File Offset: 0x0001908A
	public void Release()
	{
		this.overrider = null;
		this.overridden = false;
	}

	// Token: 0x0600052B RID: 1323 RVA: 0x0001AE9A File Offset: 0x0001909A
	public void UpdateHeightAndScale()
	{
		this.SetHeight(this.height);
	}

	// Token: 0x0600052C RID: 1324 RVA: 0x0001AEA8 File Offset: 0x000190A8
	public void SetHeight(float height)
	{
		this.height = height;
		this.SetVerticalScale(this.verticalScaleFactor);
	}

	// Token: 0x0600052D RID: 1325 RVA: 0x0001AEC0 File Offset: 0x000190C0
	public void SetVerticalScale(float factor)
	{
		if (this.rendererBase != null)
		{
			this.verticalScaleFactor = factor;
			Vector3 one = Vector3.one;
			one.y = factor;
			this.rendererBase.localScale = one;
			if (this.grounded)
			{
				this.SetBaseRotation(this.baseRotation);
				return;
			}
			Vector3 localPosition = this.rendererBaseInitialPosition;
			localPosition.y = -Entity.physicalHeight + this.OverriddenHeight;
			this.rendererBase.localPosition = localPosition;
		}
	}

	// Token: 0x0600052E RID: 1326 RVA: 0x0001AF38 File Offset: 0x00019138
	public void SetBaseRotation(float degrees)
	{
		this.baseRotation = degrees;
		Vector3 vector = this.rendererBaseInitialPosition;
		if (this.grounded)
		{
			vector.y = -Entity.physicalHeight + this.OverriddenHeight * this.verticalScaleFactor;
		}
		else
		{
			vector.y = -Entity.physicalHeight + this.OverriddenHeight;
		}
		vector = Quaternion.Euler(degrees, 0f, 0f) * (vector - this.rendererBaseInitialPosition) + this.rendererBaseInitialPosition;
		this.rendererBase.localPosition = vector;
		Vector3 localEulerAngles = default(Vector3);
		localEulerAngles.z = degrees;
		this.rendererBase.transform.localEulerAngles = localEulerAngles;
	}

	// Token: 0x0600052F RID: 1327 RVA: 0x0001AFE6 File Offset: 0x000191E6
	public void Squish(float time)
	{
		this.squished = true;
		this.squishTime = Mathf.Max(time, this.squishTime);
		this.SetVerticalScale(0.5f);
		this.UpdateTriggerState();
	}

	// Token: 0x06000530 RID: 1328 RVA: 0x0001B012 File Offset: 0x00019212
	public void Unsquish()
	{
		this.squished = false;
		this.squishTime = 0f;
		this.SetVerticalScale(1f);
		this.UpdateTriggerState();
	}

	// Token: 0x06000531 RID: 1329 RVA: 0x0001B038 File Offset: 0x00019238
	public void IgnoreEntity(Entity toIgnore, bool value)
	{
		IgnoredEntityData ignoredEntityData = new IgnoredEntityData();
		bool flag = false;
		for (int i = 0; i < this.ignoredEntities.Count; i++)
		{
			if (this.ignoredEntities[i].ignoredEntity == toIgnore)
			{
				ignoredEntityData = this.ignoredEntities[i];
				flag = true;
				break;
			}
		}
		if (value)
		{
			Physics.IgnoreCollision(this.trigger, toIgnore.trigger, true);
			ignoredEntityData.ignores++;
			foreach (IgnoredEntityData ignoredEntityData2 in toIgnore.ignoredEntities)
			{
				if (ignoredEntityData2.ignoredEntity == this)
				{
					ignoredEntityData2.ignores = ignoredEntityData.ignores;
				}
			}
			if (!flag)
			{
				ignoredEntityData.ignoredEntity = toIgnore;
				this.ignoredEntities.Add(ignoredEntityData);
				toIgnore.ignoredEntities.Add(new IgnoredEntityData(this, ignoredEntityData.ignores));
				return;
			}
		}
		else
		{
			if (flag)
			{
				ignoredEntityData.ignores--;
				if (ignoredEntityData.ignores <= 0)
				{
					Physics.IgnoreCollision(this.trigger, toIgnore.trigger, false);
					this.ignoredEntities.Remove(ignoredEntityData);
					for (int j = 0; j < toIgnore.ignoredEntities.Count; j++)
					{
						if (toIgnore.ignoredEntities[j].ignoredEntity == this)
						{
							toIgnore.ignoredEntities.RemoveAt(j);
							j--;
						}
					}
					return;
				}
				using (List<IgnoredEntityData>.Enumerator enumerator = toIgnore.ignoredEntities.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IgnoredEntityData ignoredEntityData3 = enumerator.Current;
						if (ignoredEntityData3.ignoredEntity == this)
						{
							ignoredEntityData3.ignores = ignoredEntityData.ignores;
						}
					}
					return;
				}
			}
			Physics.IgnoreCollision(this.trigger, toIgnore.trigger, false);
		}
	}

	// Token: 0x06000532 RID: 1330 RVA: 0x0001B230 File Offset: 0x00019430
	public bool IsIgnoring(Entity entityToCheck)
	{
		using (List<IgnoredEntityData>.Enumerator enumerator = this.ignoredEntities.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.ignoredEntity == entityToCheck)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06000533 RID: 1331 RVA: 0x0001B290 File Offset: 0x00019490
	public virtual void Teleport(Vector3 position)
	{
		position.y = Entity.physicalHeight;
		this.transform.position = position;
		Entity.OnTeleportFunction onTeleport = this.OnTeleport;
		if (onTeleport == null)
		{
			return;
		}
		onTeleport(position);
	}

	// Token: 0x17000056 RID: 86
	// (get) Token: 0x06000534 RID: 1332 RVA: 0x0001B2BB File Offset: 0x000194BB
	public RoomController CurrentRoom
	{
		get
		{
			return this.currentRoom;
		}
	}

	// Token: 0x17000057 RID: 87
	// (get) Token: 0x06000535 RID: 1333 RVA: 0x0001B2C3 File Offset: 0x000194C3
	public ActivityModifier ExternalActivity
	{
		get
		{
			return this.externalActivity;
		}
	}

	// Token: 0x17000058 RID: 88
	// (get) Token: 0x06000536 RID: 1334 RVA: 0x0001B2CB File Offset: 0x000194CB
	public Vector3 Velocity
	{
		get
		{
			return this.velocity;
		}
	}

	// Token: 0x17000059 RID: 89
	// (get) Token: 0x06000537 RID: 1335 RVA: 0x0001B2D3 File Offset: 0x000194D3
	public Vector3 RelativeToForcedVelocity
	{
		get
		{
			return this.relativeToForceVelocity;
		}
	}

	// Token: 0x1700005A RID: 90
	// (get) Token: 0x06000538 RID: 1336 RVA: 0x0001B2DB File Offset: 0x000194DB
	public Vector3 InternalMovement
	{
		get
		{
			return this.internalMovementVector;
		}
	}

	// Token: 0x1700005B RID: 91
	// (get) Token: 0x06000539 RID: 1337 RVA: 0x0001B2E3 File Offset: 0x000194E3
	private float OverriddenHeight
	{
		get
		{
			if (this.overridden)
			{
				return this.overrider.height;
			}
			return this.height;
		}
	}

	// Token: 0x1700005C RID: 92
	// (get) Token: 0x0600053A RID: 1338 RVA: 0x0001B2FF File Offset: 0x000194FF
	public float InternalHeight
	{
		get
		{
			return this.height;
		}
	}

	// Token: 0x1700005D RID: 93
	// (get) Token: 0x0600053B RID: 1339 RVA: 0x0001B307 File Offset: 0x00019507
	public float BaseHeight
	{
		get
		{
			return this.height * this.verticalScaleFactor;
		}
	}

	// Token: 0x1700005E RID: 94
	// (get) Token: 0x0600053C RID: 1340 RVA: 0x0001B316 File Offset: 0x00019516
	public bool KeepCharacterControllerCollision
	{
		get
		{
			return this.keepCharacterControllerCollision;
		}
	}

	// Token: 0x1700005F RID: 95
	// (get) Token: 0x0600053D RID: 1341 RVA: 0x0001B31E File Offset: 0x0001951E
	public bool Squished
	{
		get
		{
			return this.squished;
		}
	}

	// Token: 0x17000060 RID: 96
	// (get) Token: 0x0600053E RID: 1342 RVA: 0x0001B326 File Offset: 0x00019526
	public bool Frozen
	{
		get
		{
			if (this.overridden)
			{
				return this.frozen || this.overrider.Frozen;
			}
			return this.frozen;
		}
	}

	// Token: 0x17000061 RID: 97
	// (get) Token: 0x0600053F RID: 1343 RVA: 0x0001B34C File Offset: 0x0001954C
	public bool Grounded
	{
		get
		{
			return this.grounded;
		}
	}

	// Token: 0x17000062 RID: 98
	// (get) Token: 0x06000540 RID: 1344 RVA: 0x0001B354 File Offset: 0x00019554
	public bool InteractionDisabled
	{
		get
		{
			if (this.overridden)
			{
				return this.interactionDisables > 0 || this.overrider.InteractionDisabled;
			}
			return this.interactionDisables > 0;
		}
	}

	// Token: 0x06000541 RID: 1345 RVA: 0x0001B37E File Offset: 0x0001957E
	public void Enable(bool val)
	{
		this.trigger.enabled = val;
		this.collider.enabled = val;
	}

	// Token: 0x04000552 RID: 1362
	private static List<Entity> allEntities = new List<Entity>();

	// Token: 0x04000553 RID: 1363
	protected static float physicalHeight = 5f;

	// Token: 0x04000554 RID: 1364
	[SerializeField]
	public float internalFriction = 1f;

	// Token: 0x04000555 RID: 1365
	[SerializeField]
	public float externalFriction = 1f;

	// Token: 0x04000556 RID: 1366
	[SerializeField]
	private LayerMask collisionLayerMask = 2113541;

	// Token: 0x04000557 RID: 1367
	private EnvironmentController environmentController;

	// Token: 0x04000558 RID: 1368
	[SerializeField]
	private ActivityModifier externalActivity;

	// Token: 0x04000559 RID: 1369
	private RoomController currentRoom;

	// Token: 0x0400055A RID: 1370
	private Collider[] neighbor = new Collider[8];

	// Token: 0x0400055B RID: 1371
	private RaycastHit[] _hit = new RaycastHit[16];

	// Token: 0x0400055C RID: 1372
	[SerializeField]
	private Collider collider;

	// Token: 0x0400055D RID: 1373
	[SerializeField]
	private Collider trigger;

	// Token: 0x0400055E RID: 1374
	[SerializeField]
	private Transform rendererBase;

	// Token: 0x0400055F RID: 1375
	private IEntityTrigger[] iEntityTrigger = new IEntityTrigger[0];

	// Token: 0x04000560 RID: 1376
	private Chunk currentChunk;

	// Token: 0x04000561 RID: 1377
	protected new Transform transform;

	// Token: 0x04000562 RID: 1378
	private List<Force> forces = new List<Force>();

	// Token: 0x04000563 RID: 1379
	private EntityOverrider overrider;

	// Token: 0x04000564 RID: 1380
	private List<IgnoredEntityData> ignoredEntities = new List<IgnoredEntityData>();

	// Token: 0x04000565 RID: 1381
	private Vector3 movementVector;

	// Token: 0x04000566 RID: 1382
	private Vector3 internalMovementVector;

	// Token: 0x04000567 RID: 1383
	private Vector3 previousPosition;

	// Token: 0x04000568 RID: 1384
	private Vector3 velocity;

	// Token: 0x04000569 RID: 1385
	private Vector3 _position;

	// Token: 0x0400056A RID: 1386
	private Vector3 expectedForcedPosition;

	// Token: 0x0400056B RID: 1387
	private Vector3 relativeToForceVelocity;

	// Token: 0x0400056C RID: 1388
	private Vector3 rendererBaseInitialPosition;

	// Token: 0x0400056D RID: 1389
	private float height;

	// Token: 0x0400056E RID: 1390
	private float colliderRadius;

	// Token: 0x0400056F RID: 1391
	private float verticalScaleFactor = 1f;

	// Token: 0x04000570 RID: 1392
	private float squishTime;

	// Token: 0x04000571 RID: 1393
	private float baseRotation;

	// Token: 0x04000572 RID: 1394
	[SerializeField]
	private bool persistent = true;

	// Token: 0x04000573 RID: 1395
	[SerializeField]
	private bool keepCharacterControllerCollision;

	// Token: 0x04000574 RID: 1396
	[SerializeField]
	private bool grounded = true;

	// Token: 0x04000575 RID: 1397
	private bool culled;

	// Token: 0x04000576 RID: 1398
	private bool active = true;

	// Token: 0x04000577 RID: 1399
	private bool frozen;

	// Token: 0x04000578 RID: 1400
	private bool squished = true;

	// Token: 0x04000579 RID: 1401
	private bool triggerOff;

	// Token: 0x0400057A RID: 1402
	private bool ignoreAddend;

	// Token: 0x0400057B RID: 1403
	private bool triggerEnabled = true;

	// Token: 0x0400057C RID: 1404
	private bool enableTriggerNextPhysicsUpdate;

	// Token: 0x0400057D RID: 1405
	private bool disableTriggerNextPhysicsUpdate;

	// Token: 0x0400057E RID: 1406
	private bool triggerEnabledThisFrame;

	// Token: 0x0400057F RID: 1407
	private bool triggerDisabledThisFrame;

	// Token: 0x04000580 RID: 1408
	private bool overridden;

	// Token: 0x04000581 RID: 1409
	private int defaultLayer;

	// Token: 0x04000582 RID: 1410
	private int freezes;

	// Token: 0x04000583 RID: 1411
	private int triggerOffs;

	// Token: 0x04000584 RID: 1412
	private int addendIgnores;

	// Token: 0x04000585 RID: 1413
	private int interactionDisables;

	// Token: 0x04000586 RID: 1414
	private static int maxCollisionChecks = 32;

	// Token: 0x02000355 RID: 853
	// (Invoke) Token: 0x06001B86 RID: 7046
	public delegate void OnTeleportFunction(Vector3 position);

	// Token: 0x02000356 RID: 854
	// (Invoke) Token: 0x06001B8A RID: 7050
	public delegate void OnEntityMoveCollisionFunction(RaycastHit hit);
}
