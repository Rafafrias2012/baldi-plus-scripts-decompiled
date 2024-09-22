using System;
using System.Collections;
using System.Collections.Generic;
using Rewired;
using Steamworks;
using UnityEngine;

// Token: 0x02000110 RID: 272
public class InputManager : Singleton<global::InputManager>
{
	// Token: 0x060006A5 RID: 1701 RVA: 0x000219EC File Offset: 0x0001FBEC
	protected override void AwakeFunction()
	{
		base.AwakeFunction();
		Debug.Log("Initializing input management.");
		Input.simulateMouseWithTouches = false;
		if (SteamManager.Initialized)
		{
			SteamInput.Init(true);
			SteamInput.RunFrame(true);
			this.steamControllerCount = SteamInput.GetConnectedControllers(this.inputHandle);
			if (this.steamControllerCount > 0)
			{
				this.steamInputActive = true;
				this.ActivateActionSet("Interface");
				this.FillSteamInputDictionary();
				Debug.Log("Steam Input was initialized.");
			}
			else
			{
				Debug.Log("Steam Input was not initialized because there are no Steam Input controllers present.");
			}
		}
		else
		{
			Debug.Log("Steam Input was not initialized because SteamManager was not initialized.");
		}
		this.reInput = ReInput.players.GetPlayer(0);
	}

	// Token: 0x060006A6 RID: 1702 RVA: 0x00021A88 File Offset: 0x0001FC88
	private void Start()
	{
		this.CheckMouseMap();
	}

	// Token: 0x060006A7 RID: 1703 RVA: 0x00021A90 File Offset: 0x0001FC90
	public void CheckMouseMap()
	{
		List<ActionElementMap> results = new List<ActionElementMap>();
		if (this.reInput.controllers.maps.GetAxisMapsWithAction(ControllerType.Mouse, "CursorXDelta", false, results) <= 0 || this.reInput.controllers.maps.GetAxisMapsWithAction(ControllerType.Mouse, "CursorYDelta", false, results) <= 0 || this.reInput.controllers.maps.GetButtonMapsWithAction(ControllerType.Mouse, "MouseSubmit", false, results) <= 0 || this.reInput.controllers.maps.GetAxisMapsWithAction(ControllerType.Mouse, "CursorX", false, results) > 0 || this.reInput.controllers.maps.GetAxisMapsWithAction(ControllerType.Mouse, "CursorY", false, results) > 0 || this.reInput.controllers.maps.GetAxisMapsWithAction(ControllerType.Mouse, "CameraX", false, results) > 0 || this.reInput.controllers.maps.GetAxisMapsWithAction(ControllerType.Mouse, "CameraY", false, results) > 0)
		{
			Debug.LogWarning("Bad mouse map detected! Resetting mouse to default mappings.");
			this.reInput.controllers.maps.LoadDefaultMaps(ControllerType.Mouse);
		}
	}

	// Token: 0x060006A8 RID: 1704 RVA: 0x00021BAC File Offset: 0x0001FDAC
	private void Update()
	{
		if (!this.rewiredControllerAvailable && ReInput.controllers.GetAnyButton())
		{
			this.rewiredControllerAvailable = true;
		}
		if (!this.frameRan)
		{
			this.RunFrame();
		}
	}

	// Token: 0x060006A9 RID: 1705 RVA: 0x00021BD8 File Offset: 0x0001FDD8
	private void RunFrame()
	{
		if (this.steamInputActive)
		{
			SteamInput.RunFrame(true);
			this.steamControllerCount = SteamInput.GetConnectedControllers(this.inputHandle);
			this.UpdateSteamInputHeldStatus();
			if (this.doubleRun)
			{
				this.UpdateSteamInputHeldStatus();
			}
			this.frameRan = true;
			this.doubleRun = false;
		}
	}

	// Token: 0x060006AA RID: 1706 RVA: 0x00021C28 File Offset: 0x0001FE28
	private void UpdateSteamInputHeldStatus()
	{
		foreach (KeyValuePair<string, DigitalInputData> keyValuePair in this.steamDigitalInputs)
		{
			for (int i = 0; i < this.steamControllerCount; i++)
			{
				this._currentInputHandle = this.inputHandle[i];
				this._digitalActionData = SteamInput.GetDigitalActionData(this._currentInputHandle, keyValuePair.Value.handle);
				if (this._digitalActionData.bState > 0)
				{
					keyValuePair.Value.firstFrame[i] = !keyValuePair.Value.active[i];
					keyValuePair.Value.active[i] = true;
				}
				else
				{
					keyValuePair.Value.active[i] = false;
					keyValuePair.Value.firstFrame[i] = false;
				}
			}
		}
	}

	// Token: 0x060006AB RID: 1707 RVA: 0x00021D1C File Offset: 0x0001FF1C
	public bool GetDigitalInput(string id, bool onDown)
	{
		if (this.inputFrozen || this.frameStopped)
		{
			return false;
		}
		if (this.steamInputActive)
		{
			if (!this.frameRan)
			{
				this.RunFrame();
			}
			for (int i = 0; i < this.steamControllerCount; i++)
			{
				if (this.steamDigitalInputs.TryGetValue(id, out this._digitalDictionaryResult))
				{
					if ((this._digitalDictionaryResult.active[i] && !onDown) || this._digitalDictionaryResult.firstFrame[i])
					{
						this.lastUsedInputHandle = this.inputHandle[i];
						return true;
					}
				}
				else
				{
					Debug.LogWarning("SteamInput handle for ID " + id + " was not found.");
				}
			}
		}
		if (onDown)
		{
			return this.reInput.GetButtonDown(id);
		}
		return this.reInput.GetButton(id);
	}

	// Token: 0x060006AC RID: 1708 RVA: 0x00021DE2 File Offset: 0x0001FFE2
	public bool AnyButton(bool onDown)
	{
		if (this.inputFrozen || this.frameStopped)
		{
			return false;
		}
		if (onDown)
		{
			return ReInput.controllers.GetAnyButtonDown();
		}
		return ReInput.controllers.GetAnyButton();
	}

	// Token: 0x060006AD RID: 1709 RVA: 0x00021E0E File Offset: 0x0002000E
	public void GetAnalogInput(AnalogInputData inputData, out Vector2 analog, out Vector2 delta)
	{
		this.GetAnalogInput(inputData, out analog, out delta, 1f);
	}

	// Token: 0x060006AE RID: 1710 RVA: 0x00021E20 File Offset: 0x00020020
	public void GetAnalogInput(AnalogInputData inputData, out Vector2 analog, out Vector2 delta, float steamDeltaAdjustment)
	{
		this._analogOut.x = 0f;
		this._analogOut.y = 0f;
		this._deltaOut.x = 0f;
		this._deltaOut.y = 0f;
		if (!this.inputFrozen && !this.frameStopped)
		{
			if (this.steamInputActive)
			{
				if (!this.frameRan)
				{
					this.RunFrame();
				}
				this.analogInputExists = this.steamAnalogInputs.TryGetValue(inputData.steamAnalogId, out this._analogDictionaryResult);
				this.absInputExists = this.steamAnalogInputs.TryGetValue(inputData.steamDeltaId, out this._deltaDictionaryResult);
				if (this.analogInputExists || this.absInputExists)
				{
					for (int i = 0; i < this.steamControllerCount; i++)
					{
						this._currentInputHandle = this.inputHandle[i];
						if (this.analogInputExists)
						{
							this._analogActionData = SteamInput.GetAnalogActionData(this._currentInputHandle, this._analogDictionaryResult);
							this._analogOut.x = this._analogOut.x + this._analogActionData.x;
							this._analogOut.y = this._analogOut.y + this._analogActionData.y;
						}
						if (this.absInputExists)
						{
							this._analogActionData = SteamInput.GetAnalogActionData(this._currentInputHandle, this._deltaDictionaryResult);
							this._deltaOut.x = this._deltaOut.x + this._analogActionData.x * steamDeltaAdjustment;
							this._deltaOut.y = this._deltaOut.y + -this._analogActionData.y * steamDeltaAdjustment;
						}
					}
				}
			}
			if (inputData.xAnalogId.Length > 0)
			{
				this._analogOut.x = this._analogOut.x + this.reInput.GetAxis(inputData.xAnalogId);
				this._analogOut.y = this._analogOut.y + this.reInput.GetAxis(inputData.yAnalogId);
			}
			if (inputData.xDeltaId.Length > 0)
			{
				this._deltaOut.x = this._deltaOut.x + this.reInput.GetAxis(inputData.xDeltaId);
				this._deltaOut.y = this._deltaOut.y + this.reInput.GetAxis(inputData.yDeltaId);
			}
			if (this._analogOut.magnitude > 1f)
			{
				this._analogOut.Normalize();
			}
		}
		analog = this._analogOut;
		delta = this._deltaOut;
	}

	// Token: 0x060006AF RID: 1711 RVA: 0x00022090 File Offset: 0x00020290
	public string GetInputButtonName(string id)
	{
		if (this.steamInputActive && this.steamDigitalInputs.TryGetValue(id, out this._digitalDictionaryResult))
		{
			for (int i = 0; i < this.steamControllerCount; i++)
			{
				this._currentInputHandle = this.inputHandle[i];
				if (SteamInput.GetDigitalActionOrigins(this._currentInputHandle, SteamInput.GetActionSetHandle(this.currentActionSetId), this._digitalDictionaryResult.handle, this._steamInputOrigins) > 0)
				{
					return SteamInput.GetStringForActionOrigin(this._steamInputOrigins[0]);
				}
			}
		}
		foreach (ControllerType controllerType in this.typeOrder)
		{
			foreach (ActionElementMap actionElementMap in this.reInput.controllers.maps.ButtonMapsWithAction(id, true))
			{
				if (actionElementMap.controllerMap.controllerType == controllerType)
				{
					return actionElementMap.elementIdentifierName;
				}
			}
		}
		return this.reInput.controllers.maps.GetFirstElementMapWithAction(id, true).elementIdentifierName;
	}

	// Token: 0x060006B0 RID: 1712 RVA: 0x000221B8 File Offset: 0x000203B8
	public void ActivateActionSet(string id)
	{
		this.currentActionSetId = id;
		if (this.steamInputActive)
		{
			for (int i = 0; i < this.steamControllerCount; i++)
			{
				this._currentInputHandle = this.inputHandle[i];
				SteamInput.ActivateActionSet(this._currentInputHandle, SteamInput.GetActionSetHandle(id));
			}
			this.RunFrame();
			this.doubleRun = true;
		}
	}

	// Token: 0x060006B1 RID: 1713 RVA: 0x00022215 File Offset: 0x00020415
	public void FreezeInput(bool freeze)
	{
		this.inputFrozen = freeze;
	}

	// Token: 0x060006B2 RID: 1714 RVA: 0x0002221E File Offset: 0x0002041E
	public void StopFrame()
	{
		this.frameStopped = true;
	}

	// Token: 0x060006B3 RID: 1715 RVA: 0x00022228 File Offset: 0x00020428
	public void Rumble(float speed, float duration, float leftStrength, float rightStrength)
	{
		if (Singleton<PlayerFileManager>.Instance.rumble && AudioListener.volume > 0f)
		{
			if (this.steamInputActive)
			{
				for (int i = 0; i < this.steamControllerCount; i++)
				{
					this._currentInputHandle = this.inputHandle[i];
					SteamInput.TriggerVibration(this._currentInputHandle, (ushort)(Mathf.Clamp(speed * leftStrength, 0f, 1f) * 65535f), (ushort)(Mathf.Clamp(speed * rightStrength, 0f, 1f) * 65535f));
				}
				if (this.rumbleStopperRunning)
				{
					base.StopCoroutine(this.rumbleStopper);
				}
				this.rumbleTime = duration;
				this.rumbleStopperRunning = true;
				this.rumbleStopper = this.RumbleStopper();
				base.StartCoroutine(this.rumbleStopper);
				return;
			}
			this.reInput.SetVibration(0, speed * leftStrength, duration);
			this.reInput.SetVibration(1, speed * rightStrength, duration);
		}
	}

	// Token: 0x060006B4 RID: 1716 RVA: 0x0002231C File Offset: 0x0002051C
	public void Rumble(float speed, float duration)
	{
		this.Rumble(speed, duration, 1f, 1f);
	}

	// Token: 0x060006B5 RID: 1717 RVA: 0x00022330 File Offset: 0x00020530
	public void Rumble(float speed, float duration, float signedAngle)
	{
		this.Rumble(speed, duration, Mathf.Clamp(Mathf.Sin(0.017453292f * signedAngle) + 1f, 0f, 1f), Mathf.Clamp(-Mathf.Sin(0.017453292f * signedAngle) + 1f, 0f, 1f));
	}

	// Token: 0x060006B6 RID: 1718 RVA: 0x00022388 File Offset: 0x00020588
	private IEnumerator RumbleStopper()
	{
		while (this.rumbleTime > 0f)
		{
			this.rumbleTime -= Time.unscaledDeltaTime;
			yield return null;
		}
		if (this.steamInputActive)
		{
			for (int i = 0; i < this.steamControllerCount; i++)
			{
				this._currentInputHandle = this.inputHandle[i];
				SteamInput.TriggerVibration(this._currentInputHandle, 0, 0);
			}
		}
		this.rumbleStopperRunning = false;
		yield break;
	}

	// Token: 0x060006B7 RID: 1719 RVA: 0x00022398 File Offset: 0x00020598
	public void SetColor(Color color)
	{
		if (this.steamInputActive)
		{
			for (int i = 0; i < this.steamControllerCount; i++)
			{
				this._currentInputHandle = this.inputHandle[i];
				SteamInput.SetLEDColor(this._currentInputHandle, (byte)(color.r * 255f), (byte)(color.g * 255f), (byte)(color.b * 255f), 0U);
			}
		}
	}

	// Token: 0x060006B8 RID: 1720 RVA: 0x00022404 File Offset: 0x00020604
	public void ResetControlMaps()
	{
		this.reInput.controllers.maps.LoadDefaultMaps(ControllerType.Joystick);
		this.reInput.controllers.maps.LoadDefaultMaps(ControllerType.Keyboard);
		this.reInput.controllers.maps.LoadDefaultMaps(ControllerType.Mouse);
	}

	// Token: 0x17000085 RID: 133
	// (get) Token: 0x060006B9 RID: 1721 RVA: 0x00022453 File Offset: 0x00020653
	public InputHandle_t LastUsedInputHandle
	{
		get
		{
			return this.lastUsedInputHandle;
		}
	}

	// Token: 0x060006BA RID: 1722 RVA: 0x0002245B File Offset: 0x0002065B
	private void LateUpdate()
	{
		this.frameRan = false;
		this.frameStopped = false;
	}

	// Token: 0x060006BB RID: 1723 RVA: 0x0002246C File Offset: 0x0002066C
	private void FillSteamInputDictionary()
	{
		this.steamDigitalInputs.Add("Run", new DigitalInputData(SteamInput.GetDigitalActionHandle("Run")));
		this.steamDigitalInputs.Add("Interact", new DigitalInputData(SteamInput.GetDigitalActionHandle("Interact")));
		this.steamDigitalInputs.Add("LookBack", new DigitalInputData(SteamInput.GetDigitalActionHandle("LookBack")));
		this.steamDigitalInputs.Add("UseItem", new DigitalInputData(SteamInput.GetDigitalActionHandle("UseItem")));
		this.steamDigitalInputs.Add("ItemRight", new DigitalInputData(SteamInput.GetDigitalActionHandle("ItemRight")));
		this.steamDigitalInputs.Add("ItemLeft", new DigitalInputData(SteamInput.GetDigitalActionHandle("ItemLeft")));
		this.steamDigitalInputs.Add("Item1", new DigitalInputData(SteamInput.GetDigitalActionHandle("Item1")));
		this.steamDigitalInputs.Add("Item2", new DigitalInputData(SteamInput.GetDigitalActionHandle("Item2")));
		this.steamDigitalInputs.Add("Item3", new DigitalInputData(SteamInput.GetDigitalActionHandle("Item3")));
		this.steamDigitalInputs.Add("Item4", new DigitalInputData(SteamInput.GetDigitalActionHandle("Item4")));
		this.steamDigitalInputs.Add("Item5", new DigitalInputData(SteamInput.GetDigitalActionHandle("Item5")));
		this.steamDigitalInputs.Add("Item6", new DigitalInputData(SteamInput.GetDigitalActionHandle("Item6")));
		this.steamDigitalInputs.Add("Pause", new DigitalInputData(SteamInput.GetDigitalActionHandle("Pause")));
		this.steamDigitalInputs.Add("Map", new DigitalInputData(SteamInput.GetDigitalActionHandle("Map")));
		this.steamDigitalInputs.Add("MapPlus", new DigitalInputData(SteamInput.GetDigitalActionHandle("MapPlus")));
		this.steamDigitalInputs.Add("MouseSubmit", new DigitalInputData(SteamInput.GetDigitalActionHandle("Click")));
		this.steamDigitalInputs.Add("MouseBoost", new DigitalInputData(SteamInput.GetDigitalActionHandle("MouseBoost")));
		this.steamDigitalInputs.Add("MapZoomPos", new DigitalInputData(SteamInput.GetDigitalActionHandle("MapZoomPos")));
		this.steamDigitalInputs.Add("MapZoomNeg", new DigitalInputData(SteamInput.GetDigitalActionHandle("MapZoomNeg")));
		this.steamAnalogInputs.Add("Movement", SteamInput.GetAnalogActionHandle("Movement"));
		this.steamAnalogInputs.Add("CameraAnalog", SteamInput.GetAnalogActionHandle("CameraAnalog"));
		this.steamAnalogInputs.Add("CameraDelta", SteamInput.GetAnalogActionHandle("CameraDelta"));
		this.steamAnalogInputs.Add("ItemAxis", SteamInput.GetAnalogActionHandle("ItemAxis"));
		this.steamAnalogInputs.Add("CursorAnalog", SteamInput.GetAnalogActionHandle("CursorAnalog"));
		this.steamAnalogInputs.Add("CursorDelta", SteamInput.GetAnalogActionHandle("CursorDelta"));
		this.steamAnalogInputs.Add("MapMovementAnalog", SteamInput.GetAnalogActionHandle("MapMovementAnalog"));
		this.steamAnalogInputs.Add("MapMovementDelta", SteamInput.GetAnalogActionHandle("MapMovementDelta"));
	}

	// Token: 0x060006BC RID: 1724 RVA: 0x00022798 File Offset: 0x00020998
	private void OnDestroy()
	{
		if (this.steamInputActive)
		{
			for (int i = 0; i < this.steamControllerCount; i++)
			{
				this._currentInputHandle = this.inputHandle[i];
				SteamInput.TriggerVibration(this._currentInputHandle, 0, 0);
			}
		}
	}

	// Token: 0x17000086 RID: 134
	// (get) Token: 0x060006BD RID: 1725 RVA: 0x000227DD File Offset: 0x000209DD
	public bool SteamInputActive
	{
		get
		{
			return this.steamInputActive;
		}
	}

	// Token: 0x17000087 RID: 135
	// (get) Token: 0x060006BE RID: 1726 RVA: 0x000227E5 File Offset: 0x000209E5
	public bool RewiredControllerAvailable
	{
		get
		{
			return this.rewiredControllerAvailable;
		}
	}

	// Token: 0x060006BF RID: 1727 RVA: 0x000227F0 File Offset: 0x000209F0
	public InputManager()
	{
		ControllerType[] array = new ControllerType[3];
		array[0] = ControllerType.Joystick;
		array[1] = ControllerType.Mouse;
		this.typeOrder = array;
		this.inputHandle = new InputHandle_t[16];
		this.steamDigitalInputs = new Dictionary<string, DigitalInputData>();
		this.steamAnalogInputs = new Dictionary<string, InputAnalogActionHandle_t>();
		this._steamInputOrigins = new EInputActionOrigin[8];
		this.currentActionSetId = "null";
		base..ctor();
	}

	// Token: 0x040006D4 RID: 1748
	private Player reInput;

	// Token: 0x040006D5 RID: 1749
	private ControllerType[] typeOrder;

	// Token: 0x040006D6 RID: 1750
	private InputHandle_t[] inputHandle;

	// Token: 0x040006D7 RID: 1751
	private InputHandle_t _currentInputHandle;

	// Token: 0x040006D8 RID: 1752
	private InputHandle_t lastUsedInputHandle;

	// Token: 0x040006D9 RID: 1753
	private DigitalInputData _digitalDictionaryResult;

	// Token: 0x040006DA RID: 1754
	private InputAnalogActionHandle_t _analogDictionaryResult;

	// Token: 0x040006DB RID: 1755
	private InputAnalogActionHandle_t _deltaDictionaryResult;

	// Token: 0x040006DC RID: 1756
	private InputDigitalActionData_t _digitalActionData;

	// Token: 0x040006DD RID: 1757
	private InputAnalogActionData_t _analogActionData;

	// Token: 0x040006DE RID: 1758
	private Dictionary<string, DigitalInputData> steamDigitalInputs;

	// Token: 0x040006DF RID: 1759
	private Dictionary<string, InputAnalogActionHandle_t> steamAnalogInputs;

	// Token: 0x040006E0 RID: 1760
	private EInputActionOrigin[] _steamInputOrigins;

	// Token: 0x040006E1 RID: 1761
	private string currentActionSetId;

	// Token: 0x040006E2 RID: 1762
	private IEnumerator rumbleStopper;

	// Token: 0x040006E3 RID: 1763
	private Vector2 _analogOut;

	// Token: 0x040006E4 RID: 1764
	private Vector2 _deltaOut;

	// Token: 0x040006E5 RID: 1765
	private float rumbleTime;

	// Token: 0x040006E6 RID: 1766
	private int steamControllerCount;

	// Token: 0x040006E7 RID: 1767
	private bool steamInputActive;

	// Token: 0x040006E8 RID: 1768
	private bool frameRan;

	// Token: 0x040006E9 RID: 1769
	private bool _digitalInputAlreadyActive;

	// Token: 0x040006EA RID: 1770
	private bool analogInputExists;

	// Token: 0x040006EB RID: 1771
	private bool absInputExists;

	// Token: 0x040006EC RID: 1772
	private bool inputFrozen;

	// Token: 0x040006ED RID: 1773
	private bool rumbleStopperRunning;

	// Token: 0x040006EE RID: 1774
	private bool rewiredControllerAvailable;

	// Token: 0x040006EF RID: 1775
	private bool frameStopped;

	// Token: 0x040006F0 RID: 1776
	private bool doubleRun;

	// Token: 0x040006F1 RID: 1777
	private string analogDebug;

	// Token: 0x040006F2 RID: 1778
	private string absoluteDebug;
}
