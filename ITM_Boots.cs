using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000084 RID: 132
public class ITM_Boots : Item
{
	// Token: 0x06000315 RID: 789 RVA: 0x00010014 File Offset: 0x0000E214
	public override bool Use(PlayerManager pm)
	{
		pm.plm.Entity.SetIgnoreAddend(true);
		this.pm = pm;
		this.canvas.worldCamera = Singleton<CoreGameManager>.Instance.GetCamera(pm.playerNumber).canvasCam;
		base.StartCoroutine(this.Timer(this.setTime));
		return true;
	}

	// Token: 0x06000316 RID: 790 RVA: 0x0001006D File Offset: 0x0000E26D
	private IEnumerator Timer(float time)
	{
		time = this.setTime;
		while (time > 0f)
		{
			time -= Time.deltaTime * this.pm.PlayerTimeScale;
			yield return null;
		}
		this.pm.plm.Entity.SetIgnoreAddend(false);
		this.animator.Play("Up", -1, 0f);
		time = 2f;
		while (time > 0f)
		{
			time -= Time.deltaTime;
			yield return null;
		}
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x04000342 RID: 834
	[SerializeField]
	private Canvas canvas;

	// Token: 0x04000343 RID: 835
	[SerializeField]
	private Animator animator;

	// Token: 0x04000344 RID: 836
	[SerializeField]
	private float setTime = 15f;
}
