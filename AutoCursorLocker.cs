using System;
using UnityEngine;

// Token: 0x020000EF RID: 239
public class AutoCursorLocker : MonoBehaviour
{
	// Token: 0x06000596 RID: 1430 RVA: 0x0001C441 File Offset: 0x0001A641
	private void OnEnable()
	{
		if (this.lockCursorOnEnable)
		{
			Singleton<CursorManager>.Instance.LockCursor();
			return;
		}
		if (this.unlockCursorOnEnable)
		{
			Singleton<CursorManager>.Instance.UnlockCursor();
		}
	}

	// Token: 0x06000597 RID: 1431 RVA: 0x0001C468 File Offset: 0x0001A668
	private void OnDisable()
	{
		if (this.lockCursorOnDisable)
		{
			Singleton<CursorManager>.Instance.LockCursor();
			return;
		}
		if (this.unlockCursorOnDisable)
		{
			Singleton<CursorManager>.Instance.UnlockCursor();
		}
	}

	// Token: 0x040005C7 RID: 1479
	public bool lockCursorOnEnable = true;

	// Token: 0x040005C8 RID: 1480
	public bool unlockCursorOnEnable;

	// Token: 0x040005C9 RID: 1481
	public bool lockCursorOnDisable;

	// Token: 0x040005CA RID: 1482
	public bool unlockCursorOnDisable;
}
