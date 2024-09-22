using System;
using System.Collections;
using Steamworks;
using TMPro;
using UnityEngine;

// Token: 0x020001DC RID: 476
public class SeedInput : MonoBehaviour
{
	// Token: 0x06000AC7 RID: 2759 RVA: 0x00038F21 File Offset: 0x00037121
	private void Awake()
	{
		if (SteamManager.Initialized)
		{
			this.m_TextInputDismissed = Callback<GamepadTextInputDismissed_t>.Create(new Callback<GamepadTextInputDismissed_t>.DispatchDelegate(this.OnGamepadTextInputDismissed_t));
		}
		this.UpdateText();
	}

	// Token: 0x06000AC8 RID: 2760 RVA: 0x00038F48 File Offset: 0x00037148
	private void Update()
	{
		if (Input.anyKeyDown && this.useSeed)
		{
			if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus))
			{
				if (this.currentValue.Length > 0 && (this.value < 0 || this.currentValue == "-"))
				{
					this.currentValue = this.currentValue.Remove(0, 1);
				}
				else
				{
					this.currentValue = "-" + this.currentValue;
				}
				this.UpdateValue();
				this.UpdateText();
				return;
			}
			if (Input.GetKeyDown(KeyCode.Backspace))
			{
				if (this.currentValue.Length > 0)
				{
					this.currentValue = this.currentValue.Substring(0, this.currentValue.Length - 1);
					if (this.currentValue == "-" || this.currentValue == "")
					{
						this.currentValue = "";
					}
					this.UpdateValue();
					this.UpdateText();
					return;
				}
			}
			else
			{
				if (Input.inputString.Length > 0 && char.IsNumber(Input.inputString, 0))
				{
					this.SetValue(this.currentValue + Input.inputString[0].ToString());
					this.UpdateValue();
					this.UpdateText();
					return;
				}
				if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.RightCommand)) && Input.GetKeyDown(KeyCode.V))
				{
					string systemCopyBuffer = GUIUtility.systemCopyBuffer;
					if (this.ValueIsValid(systemCopyBuffer))
					{
						this.currentValue = systemCopyBuffer;
						this.UpdateValue();
						this.UpdateText();
					}
				}
			}
		}
	}

	// Token: 0x06000AC9 RID: 2761 RVA: 0x000390FC File Offset: 0x000372FC
	private bool ValueIsValid(string value)
	{
		long num;
		return value.Length <= 11 && long.TryParse(value, out num) && num <= 2147483615L && num >= -2147483615L;
	}

	// Token: 0x06000ACA RID: 2762 RVA: 0x00039132 File Offset: 0x00037332
	public void SetValue(int newValue)
	{
		this.value = newValue;
		this.currentValue = newValue.ToString();
		this.useSeed = true;
		this.UpdateText();
	}

	// Token: 0x06000ACB RID: 2763 RVA: 0x00039155 File Offset: 0x00037355
	private void SetValue(string newValue)
	{
		if (Convert.ToInt64(newValue) <= 2147483615L && Convert.ToInt64(newValue) >= -2147483615L)
		{
			this.currentValue = newValue;
		}
	}

	// Token: 0x06000ACC RID: 2764 RVA: 0x0003917C File Offset: 0x0003737C
	public void UpdateValue()
	{
		if (this.currentValue == "-" || this.currentValue == "")
		{
			this.value = 0;
			return;
		}
		this.value = Convert.ToInt32(this.currentValue);
		this.currentValue = this.value.ToString();
	}

	// Token: 0x06000ACD RID: 2765 RVA: 0x000391D8 File Offset: 0x000373D8
	private void UpdateText()
	{
		if (this.useSeed)
		{
			this.tmp.text = Singleton<LocalizationManager>.Instance.GetLocalizedText("But_Seed") + this.currentValue;
			return;
		}
		this.tmp.text = Singleton<LocalizationManager>.Instance.GetLocalizedText("But_Seed") + Singleton<LocalizationManager>.Instance.GetLocalizedText("But_SeedRandom");
	}

	// Token: 0x06000ACE RID: 2766 RVA: 0x00039244 File Offset: 0x00037444
	public void ChangeMode()
	{
		this.useSeed = !this.useSeed;
		this.UpdateText();
		if (this.useSeed && SteamManager.Initialized && (SteamUtils.IsSteamInBigPictureMode() || SteamUtils.IsSteamRunningOnSteamDeck()) && SteamUtils.ShowGamepadTextInput(EGamepadTextInputMode.k_EGamepadTextInputModeNormal, EGamepadTextInputLineMode.k_EGamepadTextInputLineModeSingleLine, Singleton<LocalizationManager>.Instance.GetLocalizedText("Steam_SeedInput"), 11U, this.currentValue))
		{
			Singleton<InputManager>.Instance.FreezeInput(true);
		}
	}

	// Token: 0x06000ACF RID: 2767 RVA: 0x000392B0 File Offset: 0x000374B0
	private void OnGamepadTextInputDismissed_t(GamepadTextInputDismissed_t pCallback)
	{
		if (!pCallback.m_bSubmitted)
		{
			base.StartCoroutine(this.InputUnfreezeDelay());
			return;
		}
		string text = "";
		SteamUtils.GetEnteredGamepadTextInput(out text, pCallback.m_unSubmittedText);
		if (!this.ValueIsValid(text))
		{
			base.StartCoroutine(this.KeyboardReset());
			return;
		}
		this.currentValue = text;
		this.UpdateValue();
		this.UpdateText();
		base.StartCoroutine(this.InputUnfreezeDelay());
	}

	// Token: 0x06000AD0 RID: 2768 RVA: 0x0003931E File Offset: 0x0003751E
	private IEnumerator KeyboardReset()
	{
		float time = 1f;
		while (time > 0f)
		{
			time -= Time.unscaledDeltaTime;
			yield return null;
		}
		SteamUtils.ShowGamepadTextInput(EGamepadTextInputMode.k_EGamepadTextInputModeNormal, EGamepadTextInputLineMode.k_EGamepadTextInputLineModeSingleLine, Singleton<LocalizationManager>.Instance.GetLocalizedText("Steam_SeedBad"), 11U, this.currentValue);
		yield break;
	}

	// Token: 0x06000AD1 RID: 2769 RVA: 0x0003932D File Offset: 0x0003752D
	private IEnumerator InputUnfreezeDelay()
	{
		float time = 0.25f;
		while (time > 0f)
		{
			time -= Time.unscaledDeltaTime;
			yield return null;
		}
		Singleton<InputManager>.Instance.FreezeInput(false);
		yield break;
	}

	// Token: 0x170000E1 RID: 225
	// (get) Token: 0x06000AD2 RID: 2770 RVA: 0x00039335 File Offset: 0x00037535
	public int Seed
	{
		get
		{
			return this.value;
		}
	}

	// Token: 0x170000E2 RID: 226
	// (get) Token: 0x06000AD3 RID: 2771 RVA: 0x0003933D File Offset: 0x0003753D
	public bool UseSeed
	{
		get
		{
			return !(this.currentValue == "") && this.useSeed;
		}
	}

	// Token: 0x04000C55 RID: 3157
	private Callback<GamepadTextInputDismissed_t> m_TextInputDismissed;

	// Token: 0x04000C56 RID: 3158
	[SerializeField]
	private TMP_Text tmp;

	// Token: 0x04000C57 RID: 3159
	private string currentValue = "";

	// Token: 0x04000C58 RID: 3160
	private int value;

	// Token: 0x04000C59 RID: 3161
	private bool useSeed;
}
