using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001D2 RID: 466
public class CursorInitiator : MonoBehaviour
{
	// Token: 0x06000A88 RID: 2696 RVA: 0x00037C29 File Offset: 0x00035E29
	private void OnEnable()
	{
		this.Inititate();
	}

	// Token: 0x06000A89 RID: 2697 RVA: 0x00037C34 File Offset: 0x00035E34
	public void Inititate()
	{
		CursorController cursorController = Object.Instantiate<CursorController>(this.cursorPre, base.transform);
		cursorController.transform.SetSiblingIndex(base.transform.childCount - 2);
		cursorController.graphicRaycaster = this.graphicRaycaster;
		cursorController.rectTransform.anchoredPosition = new Vector3(this.screenSize.x / 2f * -1f, this.screenSize.y / 2f, 0f);
		cursorController.initPosition.x = this.screenSize.x / 2f;
		cursorController.initPosition.y = this.screenSize.y / 2f * -1f;
		cursorController.minRange.x = 0f;
		cursorController.minRange.y = this.screenSize.y * -1f;
		cursorController.maxRange.x = this.screenSize.x;
		cursorController.maxRange.y = 0f;
		this.currentCursor = cursorController;
		cursorController.SetInputMode(this.useInGameControls);
		cursorController.SetPositionMode(this.useRawPosition);
		if (this.cursorSprite != null)
		{
			cursorController.SetSprite(this.cursorSprite);
		}
		cursorController.SetColor(this.cursorColor);
		cursorController.Initialize();
		Singleton<CursorManager>.Instance.LockCursor();
	}

	// Token: 0x04000C13 RID: 3091
	public CursorController cursorPre;

	// Token: 0x04000C14 RID: 3092
	public CursorController currentCursor;

	// Token: 0x04000C15 RID: 3093
	public GraphicRaycaster graphicRaycaster;

	// Token: 0x04000C16 RID: 3094
	public Sprite cursorSprite;

	// Token: 0x04000C17 RID: 3095
	[SerializeField]
	private Color cursorColor = Color.white;

	// Token: 0x04000C18 RID: 3096
	public Vector2 screenSize = new Vector2(480f, 360f);

	// Token: 0x04000C19 RID: 3097
	public bool useInGameControls;

	// Token: 0x04000C1A RID: 3098
	public bool useRawPosition;
}
