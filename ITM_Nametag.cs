using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200008A RID: 138
public class ITM_Nametag : Item
{
	// Token: 0x06000332 RID: 818 RVA: 0x00010A98 File Offset: 0x0000EC98
	public override bool Use(PlayerManager pm)
	{
		pm.SetNametag(true);
		this.pm = pm;
		this.canvas.worldCamera = Singleton<CoreGameManager>.Instance.GetCamera(pm.playerNumber).canvasCam;
		base.StartCoroutine(this.Timer(this.setTime));
		return true;
	}

	// Token: 0x06000333 RID: 819 RVA: 0x00010AE7 File Offset: 0x0000ECE7
	private IEnumerator Timer(float time)
	{
		while (time > 0f)
		{
			time -= Time.deltaTime * this.pm.PlayerTimeScale;
			yield return null;
		}
		this.pm.SetNametag(false);
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x0400036F RID: 879
	[SerializeField]
	private Canvas canvas;

	// Token: 0x04000370 RID: 880
	[SerializeField]
	private float setTime = 30f;
}
