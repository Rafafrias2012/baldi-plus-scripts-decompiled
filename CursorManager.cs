using System;
using UnityEngine;

// Token: 0x020001D3 RID: 467
public class CursorManager : Singleton<CursorManager>
{
	// Token: 0x06000A8B RID: 2699 RVA: 0x00037DC6 File Offset: 0x00035FC6
	private void Start()
	{
		if (this.lockOnStart)
		{
			this.LockCursor();
		}
	}

	// Token: 0x06000A8C RID: 2700 RVA: 0x00037DD6 File Offset: 0x00035FD6
	public void LockCursor()
	{
		if (this.controlEnabled)
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			this.cursorLocked = true;
		}
	}

	// Token: 0x06000A8D RID: 2701 RVA: 0x00037DF3 File Offset: 0x00035FF3
	public void UnlockCursor()
	{
		if (this.controlEnabled)
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			this.cursorLocked = false;
		}
	}

	// Token: 0x04000C1B RID: 3099
	public bool cursorLocked;

	// Token: 0x04000C1C RID: 3100
	public bool controlEnabled;

	// Token: 0x04000C1D RID: 3101
	public bool lockOnStart;
}
