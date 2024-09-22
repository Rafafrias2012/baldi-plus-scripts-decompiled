using System;
using UnityEngine;

// Token: 0x020001EC RID: 492
public class PlayerMovement : MonoBehaviour
{
	// Token: 0x06000B35 RID: 2869 RVA: 0x0003B178 File Offset: 0x00039378
	private void Start()
	{
		this.stamina = this.staminaMax;
		this.entity.Initialize(this.pm.ec, base.transform.position);
	}

	// Token: 0x06000B36 RID: 2870 RVA: 0x0003B1A8 File Offset: 0x000393A8
	private void Update()
	{
		this.realVelocity = (base.transform.position - this.previousPosition).magnitude / Time.deltaTime;
		this.frameVelocity = (base.transform.position - this.previousPosition).magnitude;
		this.previousPosition = base.transform.position;
		if (!Singleton<PlayerFileManager>.Instance.authenticMode)
		{
			this.running = Singleton<InputManager>.Instance.GetDigitalInput("Run", false);
		}
		else
		{
			this.running = Singleton<CoreGameManager>.Instance.authenticScreen.LeverOn;
		}
		this.MouseMove();
		this.PlayerMove();
	}

	// Token: 0x06000B37 RID: 2871 RVA: 0x0003B25C File Offset: 0x0003945C
	private void MouseMove()
	{
		Quaternion rotation = base.transform.rotation;
		int num = 1;
		if (this.pm.reversed)
		{
			num = -1;
		}
		if (!Singleton<PlayerFileManager>.Instance.authenticMode)
		{
			Singleton<InputManager>.Instance.GetAnalogInput(this.cameraAnalogData, out this._absoluteVector, out this._deltaVector, 0.1f);
		}
		else
		{
			Singleton<InputManager>.Instance.GetAnalogInput(this.movementAnalogData, out this._absoluteVector, out this._deltaVector, 0.1f);
			this._deltaVector.x = 0f;
			this._deltaVector.y = 0f;
		}
		float num2 = this._deltaVector.x * (float)num * Singleton<PlayerFileManager>.Instance.mouseCameraSensitivity + this._absoluteVector.x * Time.deltaTime * Singleton<PlayerFileManager>.Instance.controllerCameraSensitivity * (float)num;
		this._rotation = Vector3.zero;
		this._rotation.y = num2 * Time.timeScale * this.pm.PlayerTimeScale;
		rotation.eulerAngles += this._rotation;
		base.transform.rotation = rotation;
	}

	// Token: 0x06000B38 RID: 2872 RVA: 0x0003B384 File Offset: 0x00039584
	private void PlayerMove()
	{
		this._position = base.transform.position;
		this._position.y = this.height;
		base.transform.position = this._position;
		float num = this.walkSpeed;
		if (this.stamina > 0f & this.running)
		{
			num = this.runSpeed;
		}
		int num2 = 1;
		if (this.pm.reversed)
		{
			num2 = -1;
		}
		if (!this.Entity.InteractionDisabled)
		{
			Singleton<InputManager>.Instance.GetAnalogInput(this.movementAnalogData, out this._absoluteVector, out this._deltaVector);
			this._moveX = base.transform.right * this._absoluteVector.x * (float)num2;
			this._moveZ = base.transform.forward * this._absoluteVector.y;
			if (Singleton<PlayerFileManager>.Instance.authenticMode)
			{
				this._moveX = Vector3.zero;
			}
		}
		else
		{
			this._moveX = Vector3.zero;
			this._moveZ = Vector3.zero;
		}
		if (Singleton<PlayerFileManager>.Instance.analogMovement & num == this.walkSpeed)
		{
			this.sensitivity = Mathf.Clamp((this._moveX + this._moveZ).magnitude, 0f, 1f);
		}
		else
		{
			this.sensitivity = 1f;
		}
		this.entity.UpdateInternalMovement((this._moveX + this._moveZ).normalized * num * this.sensitivity * this.pm.PlayerTimeScale);
		this.StaminaUpdate(((this._moveX + this._moveZ).normalized * num * this.sensitivity).magnitude);
	}

	// Token: 0x06000B39 RID: 2873 RVA: 0x0003B568 File Offset: 0x00039768
	public void StaminaUpdate(float unmodifiedSpeed)
	{
		if (this.entity.InternalMovement.magnitude > 0f)
		{
			if (this.running && this.entity.RelativeToForcedVelocity.magnitude > this.walkSpeed * Time.deltaTime * this.pm.PlayerTimeScale * this.entity.ExternalActivity.Multiplier)
			{
				this.stamina = Mathf.Max(this.stamina - this.staminaDrop * Time.deltaTime * this.pm.PlayerTimeScale, 0f);
				if (this.stamina > 0f)
				{
					this.pm.RuleBreak("Running", 0.1f);
				}
			}
		}
		else if (this.stamina < this.staminaMax)
		{
			this.stamina += this.staminaRise * Time.deltaTime * this.pm.PlayerTimeScale;
			if (this.stamina > this.staminaMax)
			{
				this.stamina = this.staminaMax;
			}
		}
		Singleton<CoreGameManager>.Instance.GetHud(this.pm.playerNumber).SetStaminaValue(this.stamina / this.staminaMax);
	}

	// Token: 0x06000B3A RID: 2874 RVA: 0x0003B6A8 File Offset: 0x000398A8
	public void AddStamina(float value, bool limited)
	{
		if (!limited)
		{
			this.stamina += value;
			return;
		}
		if (this.stamina + value > this.staminaMax)
		{
			this.stamina += Mathf.Max(0f, this.staminaMax - this.stamina);
			return;
		}
		this.stamina += value;
	}

	// Token: 0x170000EF RID: 239
	// (get) Token: 0x06000B3B RID: 2875 RVA: 0x0003B70A File Offset: 0x0003990A
	public Entity Entity
	{
		get
		{
			return this.entity;
		}
	}

	// Token: 0x170000F0 RID: 240
	// (get) Token: 0x06000B3C RID: 2876 RVA: 0x0003B712 File Offset: 0x00039912
	public float RealVelocity
	{
		get
		{
			return this.realVelocity;
		}
	}

	// Token: 0x04000CCF RID: 3279
	public PlayerManager pm;

	// Token: 0x04000CD0 RID: 3280
	public CharacterController cc;

	// Token: 0x04000CD1 RID: 3281
	[SerializeField]
	private Entity entity;

	// Token: 0x04000CD2 RID: 3282
	public ActivityModifier am;

	// Token: 0x04000CD3 RID: 3283
	public AnalogInputData movementAnalogData;

	// Token: 0x04000CD4 RID: 3284
	public AnalogInputData cameraAnalogData;

	// Token: 0x04000CD5 RID: 3285
	private Vector3 _position;

	// Token: 0x04000CD6 RID: 3286
	private Vector3 _rotation;

	// Token: 0x04000CD7 RID: 3287
	private Vector3 previousPosition;

	// Token: 0x04000CD8 RID: 3288
	private Vector3 _moveX;

	// Token: 0x04000CD9 RID: 3289
	private Vector3 _moveZ;

	// Token: 0x04000CDA RID: 3290
	private Vector2 _absoluteVector;

	// Token: 0x04000CDB RID: 3291
	private Vector2 _deltaVector;

	// Token: 0x04000CDC RID: 3292
	public float walkSpeed;

	// Token: 0x04000CDD RID: 3293
	public float runSpeed;

	// Token: 0x04000CDE RID: 3294
	public float stamina;

	// Token: 0x04000CDF RID: 3295
	public float staminaDrop;

	// Token: 0x04000CE0 RID: 3296
	public float staminaRise;

	// Token: 0x04000CE1 RID: 3297
	public float staminaMax;

	// Token: 0x04000CE2 RID: 3298
	public float height;

	// Token: 0x04000CE3 RID: 3299
	public float realVelocity;

	// Token: 0x04000CE4 RID: 3300
	public float frameVelocity;

	// Token: 0x04000CE5 RID: 3301
	private float sensitivity;

	// Token: 0x04000CE6 RID: 3302
	private bool running;
}
