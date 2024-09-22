using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020001D1 RID: 465
public class CursorController : MonoBehaviour
{
	// Token: 0x06000A79 RID: 2681 RVA: 0x00037638 File Offset: 0x00035838
	public void Initialize()
	{
		if (CursorController.Instance != null && CursorController.Instance != this)
		{
			this.initPosition = CursorController.Instance.position;
			this.Hide(CursorController.Instance.hidden);
			Object.Destroy(CursorController.Instance.gameObject);
		}
		CursorController.instance = this;
		this.eventSystem = base.GetComponent<EventSystem>();
		this.position = this.initPosition;
		this.UpdatePosition();
	}

	// Token: 0x06000A7A RID: 2682 RVA: 0x000376B4 File Offset: 0x000358B4
	private void Update()
	{
		if (Singleton<InputManager>.Instance.GetDigitalInput("MouseBoost", false))
		{
			this.speedMultiplier = 4f;
		}
		else
		{
			this.speedMultiplier = 1f;
		}
		if (!this.inGameInput)
		{
			Singleton<InputManager>.Instance.GetAnalogInput(this.cursorAnalogData, out this.analogThisFrame, out this.deltaThisFrame);
		}
		else
		{
			Singleton<InputManager>.Instance.GetAnalogInput(this.inGameAnalogData, out this.analogThisFrame, out this.deltaThisFrame);
		}
		this.position.x = Mathf.Clamp(this.position.x + this.deltaThisFrame.x * Singleton<PlayerFileManager>.Instance.mouseCursorSensitivity + this.analogThisFrame.x * Time.unscaledDeltaTime * Singleton<PlayerFileManager>.Instance.controllerCursorSensitivity * this.speedMultiplier, this.minRange.x, this.maxRange.x);
		this.position.y = Mathf.Clamp(this.position.y + this.deltaThisFrame.y * Singleton<PlayerFileManager>.Instance.mouseCursorSensitivity + this.analogThisFrame.y * Time.unscaledDeltaTime * Singleton<PlayerFileManager>.Instance.controllerCursorSensitivity * this.speedMultiplier, this.minRange.y, this.maxRange.y);
		this.movementThisFrame.x = this.deltaThisFrame.x * Singleton<PlayerFileManager>.Instance.mouseCursorSensitivity + this.analogThisFrame.x * Time.unscaledDeltaTime * Singleton<PlayerFileManager>.Instance.controllerCursorSensitivity * this.speedMultiplier;
		this.movementThisFrame.y = this.deltaThisFrame.y * Singleton<PlayerFileManager>.Instance.mouseCursorSensitivity + this.analogThisFrame.y * Time.unscaledDeltaTime * Singleton<PlayerFileManager>.Instance.controllerCursorSensitivity * this.speedMultiplier;
		this.UpdatePosition();
		if (!this.clickDisabled && !this.hidden)
		{
			this.pointerEventData = new PointerEventData(this.eventSystem);
			if (!this.useRawPosition)
			{
				this.pointerEventData.position = RectTransformUtility.WorldToScreenPoint(Singleton<GlobalCam>.Instance.Cam, this.cursorTransform.position);
			}
			else
			{
				this.pointerEventData.position = this.cursorTransform.position;
			}
			this.results.Clear();
			this.graphicRaycaster.Raycast(this.pointerEventData, this.results);
			int num = 0;
			while (num < this.results.Count && num < 1)
			{
				if (this.results[num].gameObject.tag == "Button")
				{
					this._button = this.results[num].gameObject.GetComponent<MenuButton>();
					if (this._button != null)
					{
						if (!this._button.WasHighlighted)
						{
							if (this._button.audHighlightOverride != null)
							{
								Singleton<MusicManager>.Instance.PlaySoundEffect(this._button.audHighlightOverride);
							}
							else
							{
								Singleton<MusicManager>.Instance.PlaySoundEffect(this.audHighlight);
							}
						}
						this._button.Highlight();
						if (Singleton<InputManager>.Instance.GetDigitalInput(this.clickId, true))
						{
							this._button.Press();
							if (this._button.audConfirmOverride != null)
							{
								Singleton<MusicManager>.Instance.PlaySoundEffect(this._button.audConfirmOverride);
							}
							else
							{
								Singleton<MusicManager>.Instance.PlaySoundEffect(this.audConfirm);
							}
							this.heldButton = this._button;
						}
					}
				}
				num++;
			}
			if (this.heldButton != null && !Singleton<InputManager>.Instance.GetDigitalInput(this.clickId, false))
			{
				this.heldButton.UnHold();
			}
		}
		if (this.blinkFrames > 0)
		{
			this.blinkFrames--;
			if (this.blinkFrames <= 0)
			{
				this.Hide(false);
			}
		}
	}

	// Token: 0x06000A7B RID: 2683 RVA: 0x00037AAC File Offset: 0x00035CAC
	private void UpdatePosition()
	{
		this.localPosition.x = (float)Mathf.RoundToInt(this.position.x);
		this.localPosition.y = (float)Mathf.RoundToInt(this.position.y);
		this.cursorTransform.localPosition = this.localPosition;
	}

	// Token: 0x06000A7C RID: 2684 RVA: 0x00037B04 File Offset: 0x00035D04
	public void Teleport(Vector2 pos)
	{
		this.position.x = pos.x;
		this.position.y = pos.y;
		this.localPosition.x = (float)Mathf.RoundToInt(pos.x);
		this.localPosition.y = (float)Mathf.RoundToInt(pos.y);
		this.cursorTransform.localPosition = this.localPosition;
	}

	// Token: 0x06000A7D RID: 2685 RVA: 0x00037B72 File Offset: 0x00035D72
	public void DisableClick(bool val)
	{
		this.clickDisabled = val;
	}

	// Token: 0x06000A7E RID: 2686 RVA: 0x00037B7B File Offset: 0x00035D7B
	public void Hide(bool hide)
	{
		this.hidden = hide;
		this.cursorTransform.gameObject.SetActive(!hide);
	}

	// Token: 0x06000A7F RID: 2687 RVA: 0x00037B98 File Offset: 0x00035D98
	public void Blink(int frames)
	{
		this.Hide(true);
		this.blinkFrames = frames;
	}

	// Token: 0x06000A80 RID: 2688 RVA: 0x00037BA8 File Offset: 0x00035DA8
	public void SetInputMode(bool inGame)
	{
		this.inGameInput = inGame;
		if (!inGame)
		{
			this.clickId = "MouseSubmit";
			return;
		}
		this.clickId = "Interact";
	}

	// Token: 0x06000A81 RID: 2689 RVA: 0x00037BCB File Offset: 0x00035DCB
	public void SetPositionMode(bool raw)
	{
		this.useRawPosition = raw;
	}

	// Token: 0x06000A82 RID: 2690 RVA: 0x00037BD4 File Offset: 0x00035DD4
	public void SetSprite(Sprite sprite)
	{
		this.image.sprite = sprite;
	}

	// Token: 0x06000A83 RID: 2691 RVA: 0x00037BE2 File Offset: 0x00035DE2
	public void SetColor(Color color)
	{
		this.image.color = color;
	}

	// Token: 0x170000DD RID: 221
	// (get) Token: 0x06000A84 RID: 2692 RVA: 0x00037BF0 File Offset: 0x00035DF0
	public static CursorController Instance
	{
		get
		{
			return CursorController.instance;
		}
	}

	// Token: 0x170000DE RID: 222
	// (get) Token: 0x06000A85 RID: 2693 RVA: 0x00037BF7 File Offset: 0x00035DF7
	public Vector3 LocalPosition
	{
		get
		{
			return this.localPosition;
		}
	}

	// Token: 0x170000DF RID: 223
	// (get) Token: 0x06000A86 RID: 2694 RVA: 0x00037BFF File Offset: 0x00035DFF
	public static Vector2 Movement
	{
		get
		{
			return CursorController.instance.movementThisFrame;
		}
	}

	// Token: 0x04000BF5 RID: 3061
	private static CursorController instance;

	// Token: 0x04000BF6 RID: 3062
	[SerializeField]
	private AnalogInputData cursorAnalogData;

	// Token: 0x04000BF7 RID: 3063
	[SerializeField]
	private AnalogInputData inGameAnalogData;

	// Token: 0x04000BF8 RID: 3064
	public GraphicRaycaster graphicRaycaster;

	// Token: 0x04000BF9 RID: 3065
	[SerializeField]
	private Image image;

	// Token: 0x04000BFA RID: 3066
	[SerializeField]
	private SoundObject audHighlight;

	// Token: 0x04000BFB RID: 3067
	[SerializeField]
	private SoundObject audConfirm;

	// Token: 0x04000BFC RID: 3068
	private MenuButton heldButton;

	// Token: 0x04000BFD RID: 3069
	private EventSystem eventSystem;

	// Token: 0x04000BFE RID: 3070
	private PointerEventData pointerEventData;

	// Token: 0x04000BFF RID: 3071
	public RectTransform rectTransform;

	// Token: 0x04000C00 RID: 3072
	public RectTransform cursorTransform;

	// Token: 0x04000C01 RID: 3073
	public Vector2 initPosition;

	// Token: 0x04000C02 RID: 3074
	public Vector2 position;

	// Token: 0x04000C03 RID: 3075
	public Vector2 minRange;

	// Token: 0x04000C04 RID: 3076
	public Vector2 maxRange;

	// Token: 0x04000C05 RID: 3077
	private Vector2 movementThisFrame;

	// Token: 0x04000C06 RID: 3078
	private Vector2 analogThisFrame;

	// Token: 0x04000C07 RID: 3079
	private Vector2 deltaThisFrame;

	// Token: 0x04000C08 RID: 3080
	private Vector3 localPosition;

	// Token: 0x04000C09 RID: 3081
	private List<RaycastResult> results = new List<RaycastResult>();

	// Token: 0x04000C0A RID: 3082
	private string clickId = "MouseSubmit";

	// Token: 0x04000C0B RID: 3083
	private float speedMultiplier;

	// Token: 0x04000C0C RID: 3084
	private int blinkFrames;

	// Token: 0x04000C0D RID: 3085
	private bool clickDisabled;

	// Token: 0x04000C0E RID: 3086
	private bool hidden;

	// Token: 0x04000C0F RID: 3087
	private bool inGameInput;

	// Token: 0x04000C10 RID: 3088
	private bool useRawPosition;

	// Token: 0x04000C11 RID: 3089
	private MenuButton _button;

	// Token: 0x04000C12 RID: 3090
	private Toggle _toggle;
}
