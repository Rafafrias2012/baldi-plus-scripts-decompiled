using System;
using UnityEngine;

// Token: 0x020001EA RID: 490
public class PlayerClick : MonoBehaviour
{
	// Token: 0x06000B1F RID: 2847 RVA: 0x0003AA14 File Offset: 0x00038C14
	private void Update()
	{
		this.clickedThisFrame = null;
		if (!this.pm.plm.Entity.InteractionDisabled)
		{
			if (Singleton<PlayerFileManager>.Instance.authenticMode)
			{
				this._cursorPos.x = CursorController.Instance.LocalPosition.x - 96f;
				this._cursorPos.y = CursorController.Instance.LocalPosition.y + 360f - 72f;
				if (this._cursorPos.x < 0f || this._cursorPos.x > 288f || this._cursorPos.y < 0f || this._cursorPos.y > 216f)
				{
					return;
				}
				this._ray = Singleton<CoreGameManager>.Instance.GetCamera(0).camCom.ScreenPointToRay(this._cursorPos);
			}
			else
			{
				this._ray.origin = base.transform.position;
				this._ray.direction = Singleton<CoreGameManager>.Instance.GetCamera(this.pm.playerNumber).transform.forward;
			}
			if (!Physics.Raycast(this._ray, out this.hit, this.reach, this.clickLayers))
			{
				if (this.seesClickable)
				{
					Singleton<CoreGameManager>.Instance.GetHud(this.pm.playerNumber).UpdateReticle(false);
				}
				this.seesClickable = false;
				if (this.currentClickable != null)
				{
					this.currentClickable.ClickableUnsighted(this.pm.playerNumber);
				}
				this.currentClickable = null;
				return;
			}
			this.click = this.hit.transform.GetComponent<IClickable<int>>();
			if (this.click == null || (this.click.ClickableRequiresNormalHeight() && this.entity.Squished))
			{
				if (this.seesClickable)
				{
					Singleton<CoreGameManager>.Instance.GetHud(this.pm.playerNumber).UpdateReticle(false);
				}
				this.seesClickable = false;
				if (this.currentClickable != null)
				{
					this.currentClickable.ClickableUnsighted(this.pm.playerNumber);
				}
				this.currentClickable = null;
				return;
			}
			if (!this.click.ClickableHidden())
			{
				if (!this.seesClickable)
				{
					Singleton<CoreGameManager>.Instance.GetHud(this.pm.playerNumber).UpdateReticle(true);
				}
				this.seesClickable = true;
			}
			else
			{
				if (this.seesClickable)
				{
					Singleton<CoreGameManager>.Instance.GetHud(this.pm.playerNumber).UpdateReticle(false);
				}
				this.seesClickable = false;
			}
			if (Singleton<InputManager>.Instance.GetDigitalInput("Interact", true) & Time.timeScale != 0f)
			{
				this.click.Clicked(this.pm.playerNumber);
				this.clickedThisFrame = (this.click as MonoBehaviour).gameObject;
			}
			if (this.currentClickable != this.click)
			{
				if (this.currentClickable != null)
				{
					this.currentClickable.ClickableUnsighted(this.pm.playerNumber);
				}
				this.currentClickable = this.click;
				this.click.ClickableSighted(this.pm.playerNumber);
				return;
			}
		}
		else
		{
			if (this.seesClickable)
			{
				Singleton<CoreGameManager>.Instance.GetHud(this.pm.playerNumber).UpdateReticle(false);
			}
			this.seesClickable = false;
		}
	}

	// Token: 0x170000E7 RID: 231
	// (get) Token: 0x06000B20 RID: 2848 RVA: 0x0003AD75 File Offset: 0x00038F75
	public LayerMask ClickLayers
	{
		get
		{
			return this.clickLayers;
		}
	}

	// Token: 0x04000CA9 RID: 3241
	[SerializeField]
	private LayerMask clickLayers;

	// Token: 0x04000CAA RID: 3242
	public PlayerManager pm;

	// Token: 0x04000CAB RID: 3243
	[SerializeField]
	private Entity entity;

	// Token: 0x04000CAC RID: 3244
	public GameObject clickedThisFrame;

	// Token: 0x04000CAD RID: 3245
	private Vector3 _cursorPos;

	// Token: 0x04000CAE RID: 3246
	public float reach;

	// Token: 0x04000CAF RID: 3247
	public bool seesClickable;

	// Token: 0x04000CB0 RID: 3248
	private Ray _ray;

	// Token: 0x04000CB1 RID: 3249
	private RaycastHit hit;

	// Token: 0x04000CB2 RID: 3250
	private IClickable<int> click;

	// Token: 0x04000CB3 RID: 3251
	private IClickable<int> currentClickable;
}
